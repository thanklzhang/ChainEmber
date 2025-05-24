using System;
using System.Collections.Generic;
using UnityEngine;

static internal class EventManager
{
    #region Internal variables

    //static private MessengerHelper messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();

    static public Dictionary<int, Delegate> eventTable = new Dictionary<int, Delegate>();

    static public void Cleanup()
    {

    }

    static public void PrintEventTable()
    {
        Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

        foreach (KeyValuePair<int, Delegate> pair in eventTable)
        {
            Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
        }

        Debug.Log("\n");
    }
    #endregion

    #region Message logging and exception throwing
    static public void OnListenerAdding(int eventType, Delegate listenerBeingAdded)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, null);
        }

        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }

    static public void OnListenerRemoving(int eventType, Delegate listenerBeingRemoved)
    {
        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];

            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        }
        else
        {
            throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        }
    }

    static public void OnListenerRemoved(int eventType)
    {
        if (eventTable[eventType] == null)
        {
            eventTable.Remove(eventType);
        }
    }

    static public void OnBroadcasting(int eventType)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            //throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            //Debug.LogWarning(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }

    }

    static public BroadcastException CreateBroadcastSignatureException(int eventType)
    {
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
    }

    public class BroadcastException : Exception
    {
        public BroadcastException(string msg)
            : base(msg)
        {
        }
    }

    public class ListenerException : Exception
    {
        public ListenerException(string msg)
            : base(msg)
        {
        }
    }
    #endregion

    #region AddListener

    static public void AddListener(int eventType, Callback handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] + handler;
    }

    static public void AddListener<T>(int eventType, Callback<T> handler)
    {
        //Debug.Log("add evenType : " + eventType);
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
    }

    //Two parameters
    static public void AddListener<T, U>(int eventType, Callback<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
    }

    //Three parameters
    static public void AddListener<T, U, V>(int eventType, Callback<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
    }
    #endregion

    #region RemoveListener
    //No parameters


    static public void RemoveListener(int eventType, Callback handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Single parameter
    static public void RemoveListener<T>(int eventType, Callback<T> handler)
    {
        //Debug.Log("r : evenType" + eventType);
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Two parameters
    static public void RemoveListener<T, U>(int eventType, Callback<T, U> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    //Three parameters
    static public void RemoveListener<T, U, V>(int eventType, Callback<T, U, V> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region Broadcast

    //No parameters

    static public void Broadcast(int eventType)
    {
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback callback = d as Callback;

            if (callback != null)
            {
                callback();
            }
            else
            {
                //throw CreateBroadcastSignatureException(eventType);
                Logx.LogWarning("the event hasnt listener : " + eventType);
            }
        }
    }

    //Single parameter

    static public void Broadcast<T>(int eventType, T arg1)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T> callback = d as Callback<T>;

            if (callback != null)
            {
                callback(arg1);
            }
            else
            {

                //throw CreateBroadcastSignatureException(eventType);
                LogNoEventWarning(eventType);
            }
        }
    }

    static public void LogNoEventWarning(int eventType)
    {
        Debug.LogWarning(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));

    }

    //Two parameters
    static public void Broadcast<T, U>(int eventType, T arg1, U arg2)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;

            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                //throw CreateBroadcastSignatureException(eventType);
                LogNoEventWarning(eventType);
            }
        }
    }

    //Three parameters
    static public void Broadcast<T, U, V>(int eventType, T arg1, U arg2, V arg3)
    {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
        OnBroadcasting(eventType);

        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;

            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                //throw CreateBroadcastSignatureException(eventType);
                LogNoEventWarning(eventType);
            }
        }
    }
    #endregion
}

////This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
//public sealed class MessengerHelper : MonoBehaviour
//{
//    void Awake()
//    {
//        //DontDestroyOnLoad(gameObject);
//    }

//    //Clean up eventTable every time a new level loads.
//    public void OnLevelWasLoaded(int unused)
//    {
//        EventManager.Cleanup();
//    }
//}