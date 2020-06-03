using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileBaseGameObjectDictionary : SerializableDictionary<TileBase, GameObject> { }

public class ComponentManager : MonoBehaviour {

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBaseGameObjectDictionary components;

    void Awake () {
        ParseComponents();
	}

    void ParseComponents()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            ParseComponentsOnPosition(pos);
        }
    }

    void ParseComponentsOnPosition(Vector3Int pos)
    {
        foreach (KeyValuePair<TileBase, GameObject> entry in components)
        {
            if (tilemap.GetTile(pos) == entry.Key)
            {
                InstantiateComponent(entry.Value, pos);
            }
        }
    }

    GameObject InstantiateComponent(GameObject go, Vector3Int pos)
    {
        Vector3 position = tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f);
        tilemap.SetTile(pos, null);
        return Instantiate(go, position, Quaternion.Euler(Vector3.zero));
    }
}
