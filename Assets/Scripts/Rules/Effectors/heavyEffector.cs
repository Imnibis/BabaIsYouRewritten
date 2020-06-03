using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heavyEffector : MonoBehaviour {
    private void Start()
    {
        GetComponent<contraryEffector>().opposite = typeof(sinkEffector);
    }
}
