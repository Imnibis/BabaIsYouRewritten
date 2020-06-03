using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushEffector : MonoBehaviour {

    void Start()
    {
        GetComponent<Effector>().OnCollideEvent.AddListener(OnCollision);
        GetComponent<Effector>().OnBindEvent.AddListener(PrioritizeItem);
        GetComponent<Effector>().OnUnbindEvent.AddListener(UnprioritizeItem);
    }

    void OnCollision(ItemInstance item, ItemInstance incoming, Vector2 movementDirection)
    {
        item.canMove = true;
        ComponentsRegistry.instance.CheckItemCollision(item, movementDirection);
        Vector3 mainCamera = Camera.main.transform.position;
        float cameraSize = Camera.main.orthographicSize;
        if (item.canMove &&
                        !(item.gridPosition.x + movementDirection.x < mainCamera.x - cameraSize - 1 ||
                        item.gridPosition.x + movementDirection.x > mainCamera.x + cameraSize - 2 ||
                        item.gridPosition.y + movementDirection.y < mainCamera.y - cameraSize - 1 ||
                        item.gridPosition.y + movementDirection.y > mainCamera.y + cameraSize - 2))
        {
            Animator animator = item.GetComponent<Animator>();
            bool vertical = (movementDirection.y != 0);
            bool facingUp = (movementDirection.y == 1);
            bool facingRight = (movementDirection.x == 1);
            animator.SetBool("facingUp", facingUp);
            animator.SetBool("facingRight", facingRight);
            animator.SetBool("vertical", vertical);
            animator.SetBool("moving", true);
            StartCoroutine(ResetMovingState(animator, item));
        }
        else incoming.canMove = false;
    }

    void PrioritizeItem(Item item)
    {
        foreach (ItemInstance inst in ComponentsRegistry.instance.GetInstancesFromItem(item))
        {
            inst.GetComponent<SpriteRenderer>().sortingOrder = 998;
        }
    }

    void UnprioritizeItem(Item item)
    {
        foreach (ItemInstance inst in ComponentsRegistry.instance.GetInstancesFromItem(item))
        {
            inst.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    IEnumerator ResetMovingState(Animator animator, ItemInstance item)
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool("moving", false);
        yield return new WaitForSeconds(0.168f);
        item.transform.position = new Vector3(Mathf.Round(item.transform.position.x * 10) / 10, Mathf.Round(item.transform.position.y * 10) / 10, item.transform.position.z);
        // Fixes a bug with moving objects getting offset
    }
}
