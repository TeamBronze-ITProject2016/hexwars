/*EventManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Allows callbacks corresponding to game events to be registered 
and triggered.*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    /*Whether to print event registration/deregistration/triggers to console*/
    private const bool DEBUG_EVENTMANAGER = true;

    /*Maps string representations of an event to the corresponding event object*/
    private static Dictionary<string, UnityEvent> eventDict = null;

	/*Register an event listener, creating the event if it does not exist*/
    public static void registerListener(string eventname, UnityAction listener){
        if (eventDict == null)
            eventDict = new Dictionary<string, UnityEvent>(); 

        UnityEvent curEvent = null;

        if (!eventDict.ContainsKey(eventname)){
            curEvent = new UnityEvent();
            eventDict[eventname] = curEvent;
        }else{
            curEvent = eventDict[eventname];
        }

        curEvent.AddListener(listener);

        if (DEBUG_EVENTMANAGER){
            Debug.Log("Added listener for event " + eventname);
        }
    }

    /*Remove a previously registered listener for the given event*/
    public static void deRegisterListener(string eventname, UnityAction listener) {
        if (eventDict == null)
            eventDict = new Dictionary<string, UnityEvent>();

        if (!eventDict.ContainsKey(eventname)){
            Debug.LogWarning("EventManager: attempted to deregister from nonexistent event '" + eventname + "'");
            return;
        }

        UnityEvent curEvent = eventDict[eventname];
        curEvent.RemoveListener(listener);

        if (DEBUG_EVENTMANAGER){
            Debug.Log("Removed listener for event " + eventname);
        }
    }

    /*Trigger the given event*/
    public static void triggerEvent(string eventname){
        if (eventDict == null)
            eventDict = new Dictionary<string, UnityEvent>();

        if (!eventDict.ContainsKey(eventname)){
            Debug.LogWarning("EventManager: nonexistent event '" + eventname + "' triggered!");
            return;
        }

        UnityEvent curEvent = eventDict[eventname];
        curEvent.Invoke();

        if (DEBUG_EVENTMANAGER){
            Debug.Log("Triggered event " + eventname);
        }
    }
}
