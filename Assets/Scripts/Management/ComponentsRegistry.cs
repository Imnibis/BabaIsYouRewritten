using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsRegistry : MonoBehaviour {

    static ComponentsRegistry _instance;

    public static ComponentsRegistry instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ComponentsRegistry");
                _instance = go.AddComponent<ComponentsRegistry>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public List<Effector> effectors = new List<Effector>();
    public List<Item> items = new List<Item>();
    public List<ItemInstance> itemInstances = new List<ItemInstance>();
    List<ItemInstance> toBeDestroyed = new List<ItemInstance>();

    int currentInstanceID = -1;

    public void UpdateRules()
    {
        foreach(ItemInstance _i in itemInstances)
        {
            if (_i.GetComponent<IsText>())
            {
                _i.GetComponent<IsText>().CheckIfSurrounded();
            }
        }
    }

    public List<ItemInstance> GetInstancesFromItem(Item item)
    {
        return GetInstancesFromItemName(item.itemName);
    }

    public List<ItemInstance> GetInstancesFromItemName(string itemName)
    {
        List<ItemInstance> instances = new List<ItemInstance>();
        foreach(ItemInstance _i in itemInstances)
        {
            if(_i.itemName == itemName)
            {
                instances.Add(_i);
            }
        }
        return instances;
    }

    public int GenerateInstanceID()
    {
        currentInstanceID++;
        return currentInstanceID;
    }

    public List<Item> GetItemsFromName(string itemName)
    {
        List<Item> _items = new List<Item>();
        foreach (Item _i in items)
        {
            if (_i.itemName == itemName)
            {
                _items.Add(_i);
            }
        }
        return _items;
    }

    public bool DoesListContainEffector<T>(List<Effector> list) where T : MonoBehaviour
    {
        foreach(Effector _e in list)
        {
            if(_e.GetComponent<T>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public List<Effector> GetEffectorsFromItemList(List<Item> _items)
    {
        List<Effector> effectors = new List<Effector>();
        foreach (Item _i in _items)
        {
            if (_i.boundEffectors.Count != 0) {
                foreach(Effector _e in _i.boundEffectors)
                {
                    effectors.Add(_e);
                }
            }
        }
        return effectors;
    }

    public List<Effector> GetEffectorsFromItemName(string itemName)
    {
        return GetEffectorsFromItemList(GetItemsFromName(itemName));
    }

    public void CheckItemCollision(ItemInstance item, Vector2 movementDirection)
    {
        foreach (ItemInstance item2 in instance.itemInstances)
        {
            if (item.gridPosition + movementDirection == item2.gridPosition && !item2.Equals(item))
            {
                foreach (Effector effector in instance.GetEffectorsFromItemName(item2.itemName))
                {
                    effector.OnCollideEvent.Invoke(item2, item, movementDirection);
                }
            }
        }
    }

    public void DestroyInstance(ItemInstance inst)
    {
        toBeDestroyed.Add(inst);
    }

    private void LateUpdate()
    {
        foreach(ItemInstance inst in toBeDestroyed)
        {
            itemInstances.Remove(inst);
            Destroy(inst.gameObject);
        }
        toBeDestroyed.Clear();
    }
}
