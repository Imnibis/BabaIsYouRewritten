using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemEvent : UnityEvent<Item> { };

[System.Serializable]
public class ItemInstanceEvent : UnityEvent<ItemInstance> { };

[System.Serializable]
public class CollisionEvent : UnityEvent<ItemInstance, ItemInstance, Vector2> { };

public class EventManager : MonoBehaviour {

    static EventManager _instance;

    public static EventManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("EventManager");
                _instance = go.AddComponent<EventManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Events")]
    [Space]

    public ItemEvent OnMoveEvent;

    public UnityEvent OnWinEvent;

    // Use this for initialization
    void Awake () {
        if (OnMoveEvent == null)
            OnMoveEvent = new ItemEvent();

        OnMoveEvent.AddListener(FireMoveEvent);

        if (OnWinEvent == null)
            OnWinEvent = new UnityEvent();

        OnWinEvent.AddListener(FireWinEvent);
    }

    void FireMoveEvent(Item item)
    {
        FireEvent<ItemEvent, Item>("Move", item);
    }

    void FireWinEvent()
    {
        FireEvent<UnityEvent>("Win");
    }

    void FireEvent<T>(string _event) where T : UnityEvent
    {
        foreach (Effector effector in ComponentsRegistry.instance.effectors)
        {
            Type effectorType = effector.GetType();
            FieldInfo field = effectorType.GetField("On" + _event + "Event");
            T e = (T)field.GetValue(effector);
            e.Invoke();
        }
    }

    void FireEvent<T, T0>(string _event, T0 arg) where T : UnityEvent<T0>
    {
        foreach(Effector effector in ComponentsRegistry.instance.effectors)
        {
            Type effectorType = effector.GetType();
            FieldInfo field = effectorType.GetField("On" + _event + "Event");
            T e = (T) field.GetValue(effector);
            e.Invoke(arg);
        }
    }
}
