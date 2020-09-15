using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delayTime;
    void Start()
    {
        Destroy(gameObject, delayTime);
    }
}
