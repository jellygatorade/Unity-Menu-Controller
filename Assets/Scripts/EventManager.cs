using System;
using System.Collections.Generic;
using UnityEngine;

// EventManager class by Bernardo Pacheco
// Using an Event Manager to Decouple your Game in Unity
// http://bernardopacheco.net/using-an-event-manager-to-decouple-your-game-in-unity

// C# static modifer tutorial example
// https://www.youtube.com/watch?v=8xcIy9cV-6g

public class EventManager : MonoBehaviour {
  private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

  private static EventManager eventManager;

  public static EventManager instance {
    get {
      if (!eventManager) {
        eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

        if (!eventManager) {
          Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
        } else {
          eventManager.Init();

          //  Sets this to not be destroyed when reloading scene
          DontDestroyOnLoad(eventManager);
        }
      }
      return eventManager;
    }
  }

  void Init() {
    if (eventDictionary == null) {
      eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
    }
  }

  public static void AddListener(string eventName, Action<Dictionary<string, object>> listener) {
    Action<Dictionary<string, object>> thisEvent;
    
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent += listener;
      instance.eventDictionary[eventName] = thisEvent;
    } else {
      thisEvent += listener;
      instance.eventDictionary.Add(eventName, thisEvent);
    }
  }

  public static void RemoveListener(string eventName, Action<Dictionary<string, object>> listener) {
    if (eventManager == null) return;
    Action<Dictionary<string, object>> thisEvent;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent -= listener;
      instance.eventDictionary[eventName] = thisEvent;
    }
  }

  public static void TriggerEvent(string eventName, Dictionary<string, object> message) {
    Action<Dictionary<string, object>> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent.Invoke(message);
    }
  }
}
