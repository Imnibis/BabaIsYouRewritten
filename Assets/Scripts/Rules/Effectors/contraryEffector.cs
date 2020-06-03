using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contraryEffector : MonoBehaviour {
    
    public System.Type opposite;

	// Use this for initialization
	void Start () {
        GetComponent<Effector>().OnCollideEvent.AddListener(OnCollision);
	}

    private void Update()
    {
        foreach (Item item in GetComponent<Effector>().boundItems)
        {
            if (item.Is(opposite))
            {
                foreach(ItemInstance instance in ComponentsRegistry.instance.GetInstancesFromItem(item))
                {
                    Destroy(instance.gameObject);
                }
            }
        }
    }

    public void OnCollision(ItemInstance item, ItemInstance incoming, Vector2 movementDirection)
    {
        if(incoming.Is(opposite))
        {
            Destroy(incoming.gameObject);
            Destroy(item.gameObject);
        }
    }

}
