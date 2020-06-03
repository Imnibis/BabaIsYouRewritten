using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopEffector : MonoBehaviour {

    void Start()
    {
        GetComponent<Effector>().OnCollideEvent.AddListener(OnCollision);
    }

    void OnCollision(ItemInstance item, ItemInstance incoming, Vector2 movementDirection)
    {
        incoming.canMove = false;
    }

}
