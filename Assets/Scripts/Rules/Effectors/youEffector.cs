using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youEffector : MonoBehaviour {

    [SerializeField] float movementCooldown = 0.5f;

    float lastMovement = 0;

    public List<Item> controlledItems;

    private Item interestingItem;

    private void Awake()
    {
        GetComponent<Effector>().OnBindEvent.AddListener(AddControlledItem);
        GetComponent<Effector>().OnUnbindEvent.AddListener(RemoveControlledItem);
    }

    // Update is called once per frame
    void Update () {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !(Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0))
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
	}

    void Move(float x, float y)
    {
        if (controlledItems.Count == 0)
            return;

        if(Time.time - lastMovement >= movementCooldown)
        {
            // TODO: Add an animation
            foreach(Item itemType in controlledItems)
            {
                foreach (ItemInstance item in ComponentsRegistry.instance.GetInstancesFromItem(itemType))
                {
                    item.canMove = true;
                    EventManager.instance.OnMoveEvent.Invoke(itemType);
                    ComponentsRegistry.instance.CheckItemCollision(item, new Vector2(x, y));
                    Vector3 mainCamera = Camera.main.transform.position;
                    float cameraSize = Camera.main.orthographicSize;
                    if (item.canMove && 
                        !(item.gridPosition.x + x < mainCamera.x - cameraSize - 1 ||
                        item.gridPosition.x + x > mainCamera.x + cameraSize - 2 ||
                        item.gridPosition.y + y < mainCamera.y - cameraSize - 1 ||
                        item.gridPosition.y + y > mainCamera.y + cameraSize - 2))
                    {
                        Animator animator = item.GetComponent<Animator>();
                        bool vertical = (y != 0);
                        bool spreadLegs = !animator.GetBool("spreadLegs");
                        bool facingUp = (y == 1);
                        bool facingRight = (x == 1);
                        animator.SetBool("spreadLegs", spreadLegs);
                        animator.SetBool("facingUp", facingUp);
                        animator.SetBool("facingRight", facingRight);
                        animator.SetBool("vertical", vertical);
                        animator.SetBool("moving", true);
                        animator.SetBool("initialConditions", false);
                        StartCoroutine(ResetMovingState(animator, item));
                        lastMovement = Time.time;
                    }
                }
            }
        }
    }

    private IEnumerator ResetMovingState(Animator animator, ItemInstance item)
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool("moving", false);
        yield return new WaitForSeconds(0.168f);
        item.transform.position = new Vector3(Mathf.Round(item.transform.position.x * 10) / 10, Mathf.Round(item.transform.position.y * 10) / 10, item.transform.position.z);
    }

    public void AddControlledItem(Item item)
    {
        interestingItem = item;

        if(!controlledItems.Exists(isSameItem))
        {
            controlledItems.Add(item);
            foreach (ItemInstance inst in ComponentsRegistry.instance.GetInstancesFromItem(item))
            {
                inst.GetComponent<SpriteRenderer>().sortingOrder = 999;
            }
        }
        interestingItem = null;
    }

    public void RemoveControlledItem(Item item)
    {
        interestingItem = item;
        foreach(ItemInstance inst in ComponentsRegistry.instance.GetInstancesFromItem(item))
        {
            inst.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        controlledItems.RemoveAll(isSameItem);
        interestingItem = null;
    }

    private bool isSameItem(Item item)
    {
        return (interestingItem.itemName == item.itemName);
    }
}
