using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coldEffector : MonoBehaviour {
    private void Start()
    {
        GetComponent<contraryEffector>().opposite = typeof(hotEffector);
    }
}
