using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Effector : MonoBehaviour {

    public string effectorType;

    public List<Item> boundItems;

    private Item interestingItem;

    [Header("Events")]
    [Space]

    public ItemEvent OnBindEvent;
    public ItemEvent OnUnbindEvent;

    public ItemEvent OnMoveEvent;

    public CollisionEvent OnCollideEvent;

    public UnityEvent OnWinEvent;

    private void Awake()
    {
        if (OnBindEvent == null)
        {
            OnBindEvent = new ItemEvent();
        }

        if (OnUnbindEvent == null)
        {
            OnUnbindEvent = new ItemEvent();
        }

        if (OnMoveEvent == null)
        {
            OnMoveEvent = new ItemEvent();
        }

        if (OnCollideEvent == null)
        {
            OnCollideEvent = new CollisionEvent();
        }

        if (OnWinEvent == null)
        {
            OnWinEvent = new UnityEvent();
        }
    }

    void Start () {
        ComponentsRegistry.instance.effectors.Add(this);
	}

    public void UnbindItem(Item item)
    {
        OnUnbindEvent.Invoke(item);
        item.UnbindEffector(this);
        boundItems.Remove(item);
    }

    public void BindItem(Item item)
    {
        OnBindEvent.Invoke(item);
        item.BindEffector(this);
        if (!boundItems.Contains(item))
        {
            boundItems.Add(item);
        }
    }
}
