using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollingBackground : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    public float width, height;
    public float speed = -3f;
    public bool Vertical;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = boxCollider2D.size.x;
        height = boxCollider2D.size.y;
        if (Vertical)
        {
            rb.velocity = new Vector2(0, speed);
        }
        else
        {
            rb.velocity = new Vector2(speed, 0);

        }

    }

    // Update is called once per frame
    void Update()
    {
        /*if (transform.position.y < -height && Vertical)
        {
            Reposition();
        }
        else if (transform.position.x < -width && !Vertical)
        {
            Reposition();
        }*/
        if (transform.position.y < -85f && Vertical)
        {
            Reposition();
        }
    }
    public void ChangeBgSpeed(float newSpeed)
    {
        if (Vertical)
        {
            rb.velocity = new Vector2(0f, newSpeed);
        }
        else
        {
            rb.velocity = new Vector2(newSpeed, 0f);
        }
    }
    private void Reposition()
    {
        /*if (Vertical)
        {
            Vector2 vector = new Vector2(0f, height * 2f);
            transform.position = (Vector2)transform.position + vector;
        }
        else
        {
            Vector2 vector = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + vector;
        }*/
        if (Vertical)
        {
            Vector2 vector = new Vector2(0f, 170f);
            transform.position = (Vector2)transform.position + vector;
        }
    }
}
