using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    class ItemPosition
    {
        public int id;
        public Vector3 position;
        public Sprite sprite;

        public ItemPosition(int id, Vector3 position, Sprite sprite)
        {
            this.id = id;
            this.position = position;
            this.sprite = sprite;
        }
    }

    class Instant
    {
        public List<ItemPosition> itemPositions = new List<ItemPosition>();

        public Instant(List<ItemPosition> itemPositions)
        {
            this.itemPositions = itemPositions;
        }
    }

    List<Instant> instants = new List<Instant>();

    private void Start()
    {
        EventManager.instance.OnMoveEvent.AddListener(RegisterItemPositions);
        EventManager.instance.OnWinEvent.AddListener(EmptyItemPositions);
    }

    void RegisterItemPositions(Item movingItem)
    {
        List<ItemPosition> itemPositions = new List<ItemPosition>();
        foreach (ItemInstance instance in ComponentsRegistry.instance.itemInstances)
        {
            itemPositions.Add(new ItemPosition(instance.id, instance.transform.position, instance.GetComponent<SpriteRenderer>().sprite));
        }
        instants.Insert(0, new Instant(itemPositions));
    }

    void RevertToLastInstant()
    {
        if (instants.Count == 0) return;
        Instant instant = instants[0];
        instants.RemoveAt(0);

        foreach(ItemPosition itemPos in instant.itemPositions)
        {
            foreach(ItemInstance inst in ComponentsRegistry.instance.itemInstances)
            {
                if(itemPos.id == inst.id)
                {
                    inst.gameObject.transform.position = itemPos.position;
                    if (inst.itemName == "baba")
                    {
                        inst.GetComponent<SpriteRenderer>().sprite = itemPos.sprite;
                    }
                    break;
                }
            }
        }
    }

    void EmptyItemPositions()
    {
        instants.Clear();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            RevertToLastInstant();
        }
        if(Input.GetButtonDown("Restart"))
        {
            EventManager.instance.OnWinEvent.Invoke();
            ComponentsRegistry.instance.effectors.RemoveAll(x => true);
            ComponentsRegistry.instance.items.RemoveAll(x => true);
            ComponentsRegistry.instance.itemInstances.RemoveAll(x => true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
