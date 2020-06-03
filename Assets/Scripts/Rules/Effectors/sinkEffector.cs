using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sinkEffector : MonoBehaviour {

    private void Start()
    {
        GetComponent<contraryEffector>().opposite = typeof(heavyEffector);
    }

}
