using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 1;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    private float width, height;
    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = boxCollider2D.size.x;
        height = boxCollider2D.size.y;
        rb.velocity = new Vector2(0, speed);
    }
    void Update()
    {
        /*if (transform.position.x < -width)
        {
            RepositionX();
        }*/
        if (transform.position.y < -height * 10f)
        {
            RepositionY();
        }
    }
    private void RepositionX()
    {
        Vector2 vector = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + vector;
    }
    private void RepositionY()
    {
        Vector2 vector = new Vector2(0, height * 20f);
        transform.position = (Vector2)transform.position + vector;
    }
}
