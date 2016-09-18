/*ReplayManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles storage and playback of replay data
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    /*Class which stores replay data and plays it back*/
    public class ReplayManager : MonoBehaviour {

        /*The length of the recorded replay.*/
        private float replayLength = 0.0f;

        /*The maximum possible length of a replay*/
        private const float MAX_REPLAY_LENGTH = 10.0f;

        /*The minimum time between data points in the replay*/
        private const float  MIN_REPLAY_POINT_INTERVAL = 1.0f/60.0f; /*60 points/sec*/

        /*Playback data*/
        private bool playing = false;
        private float playbackStart = -1.0f;
        private float originalTimeScale = 1.0f;
        private const float PLAYBACK_TIMESCALE = 0.5f;

        /*Replay data structure*/
        private struct ReplayData{
            public Vector2 position;
            public Quaternion rotation;
            public float time;

            public ReplayData(Vector2 pos, Quaternion rot, float time){
                this.position = pos;
                this.rotation = rot;
                this.time = time;
            }
        };

        /*Maps game objects to replay data*/
        private Dictionary<GameObject, ArrayList> replayDict;

        /*Initialise Replay Manager*/
        void Start () {
            replayDict = new Dictionary<GameObject, ArrayList>();
        }

        /*Find the closest data point in list to the given point. Returns the index of the
         * data point if successful or -1 if unsuccessful*/
        int findClosestReplayData(ArrayList list, float time, float maxDist) {
            int closest = -1, left = 0, right = list.Count - 1;
            float deltaMax = float.PositiveInfinity;

            if (right < left)
                return closest;

            /*Modified binary search*/
            while(true){
                int midIndex = (left + right)/2;
                ReplayData middle = (ReplayData)list[midIndex];

                float delta = Mathf.Abs(middle.time - time);

                /*If we're getting further away, return closest point*/
                if(delta > deltaMax)
                    return closest;

                /*Record closest data point within maximum distance*/
                if(delta < deltaMax && delta <= maxDist){
                    deltaMax = Mathf.Abs(middle.time - time);
                    closest = midIndex;
                }

                if (middle.time < time) {
                    left = midIndex + 1;
                } else if (middle.time == time) {
                    return closest;
                } else { /*middle.time > time*/
                    right = midIndex - 1;
                }
            }
        }
        
        /*Update the stored replay data for all registered GameObjects.*/
        void Update () {
            if (replayDict == null) {
                Debug.Log("ReplayDict is null - Start() not called?");
                Start();
            }

            if (Input.GetKeyDown(KeyCode.R))
                playback();

            if (getPlaybackTime() > getReplayLength()) { 
                playing = false;
                Time.timeScale = originalTimeScale;
            }

            //Debug.Log("Count " + replayDict.Count); count is 0 here?

            /*Playback*/
            if (playing) {
                foreach(KeyValuePair<GameObject, ArrayList> pair in replayDict){
                    /*Get closest data point*/
                    ArrayList list = pair.Value;
                    int index = findClosestReplayData(list, getPlaybackTime(), 
                        MIN_REPLAY_POINT_INTERVAL * 2.0f);

                    /*Check if we failed*/
                    if (index == -1) {
                        Debug.Log("Failed to find replay data point");
                        continue;
                    }

                    /*Set GameObject data to recorded values*/
                    GameObject obj = pair.Key;
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    ReplayData data = (ReplayData)list[index];

                    /*Calculate velocity and angular velocity if applicable*/
                    if (rb && index < list.Count - 1) {
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
                    obj.transform.position = data.position;
                    obj.transform.rotation = data.rotation;
                    Debug.Log("Set values...");
                }

                /*Don't record data if we are playing the replay*/
                return;
            }

            /*Record data*/
            foreach(KeyValuePair<GameObject, ArrayList> pair in replayDict){
                /*Get latest replay data point, if it exists*/
                ArrayList curList = pair.Value;

                /*Check interval between last data point and now*/
                if(curList.Count > 0){
                    ReplayData data = (ReplayData)curList[curList.Count - 1];

                    if(Time.time - data.time < MIN_REPLAY_POINT_INTERVAL)
                        continue;
                }

                /*Record data point*/
                GameObject obj = pair.Key;
                ReplayData curPoint = new ReplayData(new Vector2(obj.transform.position.x,
                    obj.transform.position.y), obj.transform.rotation, Time.time);
                curList.Add(curPoint);

                //Debug.Log("Recorded data point");

                /*Check if we have reached MIN_REPLAY_LENGTH seconds worth of data
                 if so, remove earliest data point*/
                if(curList.Count >= 2){
                    ReplayData first = (ReplayData)curList[0];

                    if(curPoint.time - first.time > MAX_REPLAY_LENGTH)
                        curList.RemoveAt(0);
                }
            }
            
            /*Record length of replay*/
            replayLength = Mathf.Clamp(replayLength + Time.deltaTime, 0.0f, 
                MAX_REPLAY_LENGTH);
        }

        /*Register a game object. 
         * Returns a non-negative index if successful, or -1 if unsuccessful.*/
        public bool registerGameObject(GameObject obj){
            /*Check if null, or already registered*/
            if(!obj){
                Debug.LogWarning("ReplayManager: Attempted to register null GameObject!");
                return false;
            }

            if (replayDict == null) {
                Debug.LogError("????????");
                Start();
            }
            
            if(replayDict.ContainsKey(obj)){
                Debug.LogWarning(@"ReplayManager: Attempted to register already registered
                GameObject!");
                return true;
            }

            /*Register game object*/
            ArrayList list = new ArrayList();
            replayDict.Add(obj, list);
            Debug.Log("ReplayManager - GameObject Registered");
            return true;
        }

        /*De-register a game object.*/
        public void deregisterGameObject(GameObject obj){
            if(!replayDict.Remove(obj))
                Debug.LogWarning(@"ReplayManager: Attempted to remove nonexistent 
                GameObject!");
        }

        /*Return the total length of the recorded replay*/
        public float getReplayLength(){
            return replayLength;
        }

        /*Start playing back the replay*/
        public void playback(){
            playing = true;
            playbackStart = Time.time;
            originalTimeScale = Time.timeScale;
            Time.timeScale = PLAYBACK_TIMESCALE;
        }

        /*Returns true while the replay is playing*/
        public bool isPlaying(){
            return playing;
        }

        /*Get the time elapsed since playback began*/
        public float getPlaybackTime(){
            return Time.time - playbackStart;
        }
    }
}