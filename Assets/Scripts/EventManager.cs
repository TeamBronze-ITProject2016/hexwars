/*EventManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Allows callbacks corresponding to game events to be registered 
and triggered, and provides a stack for numeric event data.*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class EventManager : MonoBehaviour
    {
        /*Whether to print event registration/deregistration/triggers to console*/
        private const bool DEBUG_EVENTMANAGER = true;

        /*Maps string representations of an event to the corresponding event object*/
        private static Dictionary<string, UnityEvent> eventDict = null;

        /*Maps string representations of an event to a a list of floats*/
        private static Dictionary<string, List<float>> eventFloatsDict = null;

        /*Register an event listener, creating the event if it does not exist*/
        public static void registerListener(string eventname, UnityAction listener)
        {
            /*Create event dictionary and event data dictionary, if necessary*/
            if (eventDict == null)
                eventDict = new Dictionary<string, UnityEvent>();

            if (eventFloatsDict == null)
                eventFloatsDict = new Dictionary<string, List<float>>();

            /*Find event in dictionary, or create it if it does not exist*/
            UnityEvent curEvent = null;

            if (!eventDict.ContainsKey(eventname))
            {
                curEvent = new UnityEvent();
                eventDict[eventname] = curEvent;
            }
            else
            {
                curEvent = eventDict[eventname];
            }

            /*Register listener as a listener for the given event*/
            curEvent.AddListener(listener);

            if (DEBUG_EVENTMANAGER)
            {
                Debug.Log("Added listener for event " + eventname);
            }
        }

        /*Remove a previously registered listener for the given event*/
        public static void deRegisterListener(string eventname, UnityAction listener)
        {
            if (eventDict == null)
                return;

            if (!eventDict.ContainsKey(eventname))
            {
                Debug.LogWarning("EventManager: attempted to deregister from nonexistent event '" + eventname + "'");
                return;
            }

            UnityEvent curEvent = eventDict[eventname];
            curEvent.RemoveListener(listener);

            if (DEBUG_EVENTMANAGER)
            {
                Debug.Log("Removed listener for event " + eventname);
            }
        }

        /*Trigger the given event*/
        public static void triggerEvent(string eventname)
        {
            if (eventDict == null)
                eventDict = new Dictionary<string, UnityEvent>();

            if (!eventDict.ContainsKey(eventname))
            {
                Debug.LogWarning("EventManager: nonexistent event '" + eventname + "' triggered!");
                return;
            }

            UnityEvent curEvent = eventDict[eventname];
            curEvent.Invoke();

            if (DEBUG_EVENTMANAGER)
            {
                Debug.Log("Triggered event " + eventname);
            }
        }

        /*Push event data - floats*/
        public static void pushEventDataFloat(string eventname, float data)
        {
            if (!eventFloatsDict.ContainsKey(eventname))
            {
                eventFloatsDict[eventname] = new List<float>();
            }

            eventFloatsDict[eventname].Add(data);
        }

        /*Return and remove the last float pushed for the given event*/
        public static bool popEventDataFloat(string eventname, ref float outval)
        {
            /*Ensure list exists and is not empty*/
            if (!eventFloatsDict.ContainsKey(eventname))
                return false;

            int lastindex = eventFloatsDict[eventname].Count - 1;

            if (lastindex == -1)
                return false;

            /*Remove and return item*/
            outval = eventFloatsDict[eventname][lastindex];
            eventFloatsDict[eventname].RemoveAt(lastindex);
            return true;
        }
    }
}