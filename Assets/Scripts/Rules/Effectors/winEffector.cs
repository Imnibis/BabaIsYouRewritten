using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winEffector : MonoBehaviour {

    void Start()
    {
        GetComponent<Effector>().OnCollideEvent.AddListener(OnCollision);
    }

    private void Update()
    {
        foreach(Item item in GetComponent<Effector>().boundItems)
        {
            if(item.Is<youEffector>())
            {
                Win();
                return;
            }
        }
    }

    void Win()
    {
        ComponentsRegistry.instance.effectors.RemoveAll(x => true);
        ComponentsRegistry.instance.items.RemoveAll(x => true);
        ComponentsRegistry.instance.itemInstances.RemoveAll(x => true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    void OnCollision(ItemInstance item, ItemInstance incoming, Vector2 movementDirection)
    {
        if(incoming.Is<youEffector>())
            Win();
    }

}
