using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotEffector : MonoBehaviour {
    private void Start()
    {
        GetComponent<contraryEffector>().opposite = typeof(coldEffector);
    }
}
