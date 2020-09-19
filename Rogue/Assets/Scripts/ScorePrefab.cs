using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePrefab : MonoBehaviour
{
    public float destroyTime = 4f, moveAmount = 1f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector2(0, moveAmount));
    }
}
