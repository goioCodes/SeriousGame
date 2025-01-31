using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float windAcceleration;
    public float slowdownAcceleration;
    public float frictionDrift;
    public float rotation;
    public Vector2 velocity;

    public Vector2 map_position;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rotation = 0;
        map_position = Vector2.zero;
    }

    // Update is called once per frame (not fixed time step!)
    void Update()
    {
        
    }

    // FixedUpdate is called once per physics frame (fixed time step!)
    private void FixedUpdate()
    {
        // TODO:Make this look at the ship's wheel
        rotation = rotation + Workstation.timonRotation * rotationSpeed;
        rb.rotation = rotation;

        float airForce = Mathf.Clamp(Vector2.Dot(new Vector2(-Mathf.Sin(Workstation.velaRotation * Mathf.Deg2Rad), Mathf.Cos(Workstation.velaRotation * Mathf.Deg2Rad)), GameController.gameController.windDirectionShipView), 0, 1);
        float objectiveSpeed = airForce * movementSpeed;
        Debug.Log("Objective speed: " + objectiveSpeed);

        Vector2 forward = new Vector2(transform.right.x, transform.right.y);
        Vector2 ortho = new Vector2(transform.up.x, transform.up.y);


        if(velocity.magnitude < objectiveSpeed) 
        {
            velocity += forward * windAcceleration * Time.deltaTime;
            Debug.Log("Increasing speed: " + forward * windAcceleration * Time.deltaTime);
        }
        else
        {
            velocity -= velocity.normalized * slowdownAcceleration * Time.deltaTime;
            Debug.Log("Decreasing speed: " + forward * slowdownAcceleration * Time.deltaTime);
        }

        if (velocity.magnitude > movementSpeed)
        {
            velocity = velocity.normalized * movementSpeed;
        }
        Vector2 orthoVelocity = ortho * Vector2.Dot(ortho, velocity);
        velocity -= orthoVelocity * frictionDrift * Time.deltaTime;

        map_position += velocity * Time.deltaTime;  
    }
}
