/*ReplayManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles storage and playback of replay data.*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars {
    /*Class which stores replay data and plays it back*/
    public class ReplayManager : MonoBehaviour {

        private static bool DEBUG_REPLAYMANAGER = false;

        /*The length of the recorded replay.*/
        private static float replayLength = 0.0f;

        /*The maximum possible length of a replay*/
        private const float MAX_REPLAY_LENGTH = 10.0f;

        /*The minimum time between data points in the replay*/
        private const float MIN_REPLAY_POINT_INTERVAL = 1.0f / 60.0f; /*60 points/sec*/

        /*Playback data*/
        private static bool playing = false;
        private static float playbackStart = -1.0f;
        private static float originalTimeScale = 1.0f;
        private const float PLAYBACK_TIMESCALE = 1.0f;

        /*Replay data structure*/
        private struct ReplayData {
            public Vector2 position;
            public Quaternion rotation;
            public float time;

            public ReplayData(Vector2 pos, Quaternion rot, float time) {
                this.position = pos;
                this.rotation = rot;
                this.time = time;
            }
        };

        /*Maps game objects to replay data*/
        private static Dictionary<GameObject, ArrayList> replayDict;

        /*Initialise Replay Manager*/
        public static void init() {
            replayDict = new Dictionary<GameObject, ArrayList>();
            EventManager.registerListener("replayStart", playback);
            EventManager.registerListener("partadded", onPartAdded);

            if (DEBUG_REPLAYMANAGER)
                Debug.Log("ReplayManager::init() called.");
        }

        /*Find the closest data point in list to the given point. Returns the index of the
         * data point if successful or -1 if unsuccessful*/
        private static int findClosestReplayData(ArrayList list, float time, float maxDist) {
            int closest = -1, left = 0, right = list.Count - 1;
            float deltaMax = float.PositiveInfinity;

            if (right < left)
                return closest;

            /*Modified binary search*/
            while (true) {
                int midIndex = (left + right) / 2;
                ReplayData middle = (ReplayData)list[midIndex];

                float delta = Mathf.Abs(middle.time - time);

                /*If we're getting further away, return closest point*/
                if (delta > deltaMax)
                    return closest;

                /*Record closest data point within maximum distance*/
                if (delta < deltaMax && delta <= maxDist) {
                    deltaMax = Mathf.Abs(middle.time - time);
                    closest = midIndex;
                }

                /*Return closest point if left, right and middle have converged*/
                if (left >= right)
                    return closest;

                if (middle.time < time) {
                    left = midIndex + 1;
                } else if (middle.time == time) {
                    return closest;
                } else { /*middle.time > time*/
                    right = midIndex - 1;
                }
            }
        }

        /*Initalise the replay manager.*/
        void Start() {
            ReplayManager.init();
        }

        void Update()
        {
            // Trigger replay (work-in-progress)
            if (Input.GetKeyDown(KeyCode.F5))
            {
                EventManager.triggerEvent("replayStart");
                EventManager.triggerEvent("gameover");
            }
        }

        /*Update the stored replay data for all registered GameObjects.*/
        public static void doUpdate() {
            if (replayDict == null) {
                Debug.LogWarning("ReplayManager::doUpdate() - Not initialised!");
                init();
            }

            /*For debugging, remove me.*/
            if (Input.GetKeyDown(KeyCode.R)) {
                EventManager.triggerEvent("replayStart");
            }

            if (getPlaybackTime() > getReplayLength()) {
                playing = false;
                Time.timeScale = originalTimeScale;
                replayLength = 0.0f;
                EventManager.triggerEvent("replayStop");
                Debug.Log("Replay finished.");
            }

            /*Playback*/
            if (playing) {

                foreach (KeyValuePair<GameObject, ArrayList> pair in replayDict) {

                    /*Get closest data point*/
                    ArrayList list = pair.Value;
                    int index = findClosestReplayData(list, playbackStart/*Time.time*/ - replayLength + getPlaybackTime(),
                        MIN_REPLAY_POINT_INTERVAL * 2.0f);

                    /*Check if we failed*/
                    if (index == -1) {
                        if(DEBUG_REPLAYMANAGER)
                            Debug.Log("ReplayManager: Failed to find replay datapoint!");
                        continue;
                    }

                    if (DEBUG_REPLAYMANAGER)
                        Debug.Log("Getting game object and rigidbody");

                    /*Set GameObject data to recorded values, if we fail due to a missing reference exception
                     then remove the game object.*/
                    GameObject obj = pair.Key;
                    Rigidbody2D rb;
                    ReplayData data = (ReplayData)list[index];
                    try { 
                        rb = obj.GetComponent<Rigidbody2D>();
                    } catch (MissingReferenceException e) {
                        deregisterGameObject(obj);
                        continue;
                    }

                    /*Calculate velocity and angular velocity if applicable*/
                    if (rb && index < list.Count - 1) {

                        if (DEBUG_REPLAYMANAGER)
                            Debug.Log("Calculating velocity and angular velocity");

                        ReplayData next = (ReplayData)list[index + 1];
                        float timeDelta = next.time - data.time;
                        Vector2 vel = (next.position - data.position) / timeDelta;
                        Vector3 rvel = (next.rotation.eulerAngles - data.rotation.eulerAngles)
                                       / timeDelta;

                        /*Set velocity and angular velocity*/
                        rb.velocity = vel;
                        rb.angularVelocity = rvel.z;
                    }

                    /*Set position and rotation*/
                    if (DEBUG_REPLAYMANAGER)
                        Debug.Log("Setting values");

                    obj.transform.position = data.position;
                    obj.transform.rotation = data.rotation;
                }

                /*Don't record data if we are playing the replay*/
                return;
            }

            /*Record data*/
            foreach (KeyValuePair<GameObject, ArrayList> pair in replayDict) {
                /*Get latest replay data point, if it exists*/
                ArrayList curList = pair.Value;

                /*Check interval between last data point and now*/
                if (curList.Count > 0) {
                    ReplayData data = (ReplayData)curList[curList.Count - 1];

                    if (Time.time - data.time < MIN_REPLAY_POINT_INTERVAL)
                        continue;
                }

                /*Record data point*/
                GameObject obj = pair.Key;
                ReplayData curPoint;
                curPoint.time = 0.0f;

                /*Try and record the data point, if we fail due to a missing reference exception
                 then remove the game object.*/
                try { 
                    curPoint = new ReplayData(new Vector2(obj.transform.position.x,
                        obj.transform.position.y), obj.transform.rotation, Time.time);
                    curList.Add(curPoint);
                } catch (MissingReferenceException e) {
                    deregisterGameObject(obj);
                    continue;
                }

                if (DEBUG_REPLAYMANAGER)
                    Debug.Log("Recorded data point");

                /*Check if we have reached MIN_REPLAY_LENGTH seconds worth of data
                 if so, remove earliest data point*/
                if (curList.Count >= 2) {
                    ReplayData first = (ReplayData)curList[0];

                    if (DEBUG_REPLAYMANAGER)
                        Debug.Log("Reached end, removing first datapoint");

                    if (curPoint.time - first.time > MAX_REPLAY_LENGTH) {
                        curList.RemoveAt(0);
                    }
                }
            }

            /*Record length of replay*/
            replayLength = Mathf.Clamp(replayLength + Time.deltaTime, 0.0f,
                MAX_REPLAY_LENGTH);
        }

        /*Register a game object. 
         * Returns a non-negative index if successful, or -1 if unsuccessful.*/
        static public bool registerGameObject(GameObject obj) {
            /*Check if null, or already registered*/
            if (!obj) {
                Debug.LogWarning("ReplayManager::registerGameObject() - Attempted to register null GameObject!");
                return false;
            }

            if (replayDict == null) {
                Debug.LogWarning("ReplayManager::registerGameObject() - Not initialised!");
                init();
            }

            if (replayDict.ContainsKey(obj)) {
                Debug.LogWarning(@"ReplayManager: Attempted to register already registered
                GameObject!");
                return true;
            }

            /*Register game object*/
            ArrayList list = new ArrayList();
            replayDict.Add(obj, list);

            if (DEBUG_REPLAYMANAGER)
                Debug.Log("ReplayManager - GameObject Registered");
            return true;
        }

        /*De-register a game object.*/
        static public void deregisterGameObject(GameObject obj) {
            if (!replayDict.Remove(obj))
                Debug.LogWarning(@"ReplayManager: Attempted to remove nonexistent 
                GameObject!");

            if (DEBUG_REPLAYMANAGER)
                Debug.Log("ReplayManager::deregisterGameObject() called.");
        }

        /*Return the total length of the recorded replay*/
        static public float getReplayLength() {
            return replayLength;
        }

        /*Start playing back the replay*/
        static public void playback() {
            if (playing) {
                Debug.LogWarning("ReplayManager::Playback() - Already playing replay!");
                return;
            }

            Debug.Log("Playing replay, "+replayLength + " seconds");

            playing = true;
            playbackStart = Time.time;
            originalTimeScale = Time.timeScale;
            Time.timeScale = PLAYBACK_TIMESCALE;
        }

        /*Returns true while the replay is playing*/
        static public bool isPlaying() {
            return playing;
        }

        /*Part add callback. Invalidate the replay if a part is added during it.*/
        static void onPartAdded() {
            float x = 0.0f, y = 0.0f, z = 0.0f;
            if(!EventManager.popEventDataFloat("partadded", ref x) ||
               !EventManager.popEventDataFloat("partadded", ref y) ||
               !EventManager.popEventDataFloat("partadded", ref z)) {
                   Debug.LogWarning("ReplayManager::onPartAdded(): failed to get coordinates of part!");
                   return;
            }

            /*Check if the part addition is visible*/
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(x, y, z));

            if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.y < 0)
                return;

            if (screenPoint.x > Screen.width || screenPoint.y > Screen.height)
                return;

            /*Visible, invalidate replay*/
            replayLength = 0.0f;
        }

        /*Get the time elapsed since playback began*/
        static public float getPlaybackTime() {
            if (!isPlaying())
                return 0.0f;

            return Time.time - playbackStart;
        }
    }
}