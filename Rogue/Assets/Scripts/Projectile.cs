using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTimer = 15f;
    private void Start()
    {
        Destroy(this.gameObject, destroyTimer);
    }
}
