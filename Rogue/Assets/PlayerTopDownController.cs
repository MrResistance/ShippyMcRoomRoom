using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D rb;
    public Camera camera;

    public Vector2 mouseposition;
    public Vector2 lookdir;
    public float angle,h,v;

    public float movementspeed;
    public float movementspeedbonus;
    void Start()
    {
        movementspeedbonus = 100;
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);
        lookdir = mouseposition - rb.position;
        angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        rb.AddForce(transform.up * v * speed());
    }
    private float speed()
    {
        return movementspeed * (movementspeedbonus / 100);
    }
}
