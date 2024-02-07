using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour
{
    Rigidbody2D rb;
    public float step_angle;
    public float max_error;
    public float force_mag;
    public float max_proximity;
    public float max_distance;
    public float max_speed;
    public float my_position;
    public Vector2 player_pos;
    Rigidbody2D local_rb;
    // Start is called before the first frame update

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        step_angle = 1;
        max_error = 10; 
        force_mag = 1;
        max_proximity = 5;
        max_distance = 10;
        max_speed = 1F;
        player_pos = new Vector2(0F, 0F);
        local_rb = rb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        // for my sanity        
        local_rb.rotation = local_rb.rotation % 360;
        if (local_rb.rotation < 0)
        {
            local_rb.rotation += 360;
        }

        // angle the enemy towards the player
        float dist_player = Vector2.Distance(local_rb.position, Vector2.zero);

        Vector2 targetDirection = Vector2.zero - (Vector2)local_rb.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        float angleDifference = Mathf.DeltaAngle(local_rb.rotation, angle);


        if (dist_player < max_distance)
        {
            if (angleDifference > 95F || angleDifference < 85F){
                local_rb.rotation += step_angle;
            }
        }
        else if (Mathf.Abs(angleDifference) > max_error)
        {
            float direction = 1F;
            float steer = 0F;
  
            if (dist_player < max_proximity)
            {
                direction = Mathf.Sign(angleDifference) * -1f; // Opposite direction if we're too close
            }
            
            float targetAngle = local_rb.rotation + steer + direction * Mathf.Sign(angleDifference) * Mathf.Min(step_angle, Mathf.Abs(angleDifference));
            local_rb.SetRotation(targetAngle);
        }
        
        //  propulsion
        Vector2 forceDirection;
        if (dist_player > max_distance)
        {
            forceDirection = local_rb.transform.right;
        }
        else
        {
            forceDirection = Vector2.zero;
        }
        local_rb.AddForce(forceDirection * force_mag);
        
        // limit the max speed
        if (rb.velocity.magnitude > max_speed)
        {
            local_rb.velocity = local_rb.velocity.normalized * max_speed;
        }
        rb.rotation = local_rb.rotation;
        rb.position = local_rb.position + player_pos;
    }
}
