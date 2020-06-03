using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string itemName;
    public GameObject itemPrefab;
    public List<Effector> boundEffectors;

    private void Start()
    {
        ComponentsRegistry.instance.items.Add(this);
    }

    public bool Is<T>() where T : MonoBehaviour
    {
        return ComponentsRegistry.instance.DoesListContainEffector<T>(boundEffectors);
    }

    public bool Is(System.Type T)
    {
        foreach (Item item in ComponentsRegistry.instance.GetItemsFromName(itemName))
        {
            foreach (Effector _e in item.boundEffectors)
            {
                if (_e.GetComponent(T) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void UnbindEffector(Effector effector)
    {
        boundEffectors.Remove(effector);
    }

    public void BindEffector(Effector effector)
    {
        if (!boundEffectors.Contains(effector))
        {
            boundEffectors.Add(effector);
        }
    }
}
