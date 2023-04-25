using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public const bool DEBUG_MODE = true;

    static Dictionary<Type, IList> EventLibrary = new Dictionary<Type, IList>();

    /// <summary>
    /// Create a subscription to a new or existing event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Callback"></param>
    public static void Subscribe<T>(Action<T> Callback)
    {
        Type t = typeof(T);

        if (!EventLibrary.ContainsKey(t))
            EventLibrary.Add(t, new List<Action<T>>());

        EventLibrary[t].Add(Callback);

        if (DEBUG_MODE)
            Debug.Log($"[Subscribe] subscription To Event :{t.ToString()}");
    }

    /// <summary>
    /// Invoke all callbacks that have subscribed to an event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Event"></param>
    public static void Publish<T>(T Event)
    {
        Type t = typeof(T);

        if (DEBUG_MODE)
            Debug.Log($"[Publish] event of type{t}");

        if (!EventLibrary.ContainsKey(t))
        {
            Debug.LogWarning($"Event of type {t} not found");
            return;
        }

        IList subscriber_list = new List<Action<T>>(EventLibrary[t].Cast<Action<T>>());
        IList unsubscribe_list = new List<Action<T>>();

        foreach (Action<T> callback in EventLibrary[t])
        {
            if (callback.Target == null || callback.Target.Equals(null))
            {
                // This callback is hanging, as its target object was destroyed, remove it
                if (DEBUG_MODE)
                    Debug.Log("[Unsubscribe] should remove subscription to type " + t);

                unsubscribe_list.Add(callback);
            }

            try
            {
                callback.Invoke(Event);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }


        // Remove actions that have had their target objects destroyed
        foreach (Action<T> subscription in unsubscribe_list)
        {
            EventBus.Unsubscribe<T>(subscription);
        }
    }


    /// <summary>
    /// Remove actions that have had their target objects destroyed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="CallBack"></param>
    public static void Unsubscribe<T>(Action<T> CallBack)
    {
        Type t = typeof(T);
        if (EventLibrary.ContainsKey(t) && EventLibrary[t].Count > 0)
        {
            EventLibrary[t].Remove(CallBack);

            if (DEBUG_MODE)
                Debug.Log($"[Unsubscribe] removed a subscription to type {t}");
        }

    }
}
