using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour {

    public int id;
    public Vector2 gridPosition;
    public string itemName;
    public bool canMove = true;

    private void Start()
    {
        id = ComponentsRegistry.instance.GenerateInstanceID();
        ComponentsRegistry.instance.itemInstances.Add(this);
    }

    private void Update()
    {
        Vector2 oldGridPosition = gridPosition;
        gridPosition = new Vector2(Mathf.FloorToInt(transform.position.x - 0.6f),
            Mathf.FloorToInt(transform.position.y - 0.6f));
        if(oldGridPosition != gridPosition)
        {
            if(itemName == "text")
                ComponentsRegistry.instance.UpdateRules();
        }
    }

    public bool Is<T>() where T : MonoBehaviour
    {
        foreach(Item item in ComponentsRegistry.instance.GetItemsFromName(itemName))
        {
            if (item.Is<T>())
                return true;
        }
        return false;
    }

    public bool Is(System.Type T)
    {
        foreach (Item item in ComponentsRegistry.instance.GetItemsFromName(itemName))
        {
            if (item.Is(T))
                return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        ComponentsRegistry.instance.itemInstances.Remove(this);
    }
}
