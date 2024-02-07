using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public float movementSpeed;
    public float rotation;
    public Vector2 velocity;

    public Vector2 map_position;

    // Start is called before the first frame update
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rotation = 0;
        map_position = new Vector2(0, 0);
    }

    // Update is called once per frame (not fixed time step!)
    void Update()
    {
        
    }

    // FixedUpdate is called once per physics frame (fixed time step!)
    private void FixedUpdate()
    {
        // TODO:Make this look at the ship's wheel
        rb.rotation += Input.GetAxis(horizontalAxis)*-5;

        Vector2 forward = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        velocity = forward * movementSpeed * Input.GetAxis(verticalAxis);  // and this the product between the sail (vela?) and the wind
        map_position += velocity * Time.deltaTime;  
    }
}
