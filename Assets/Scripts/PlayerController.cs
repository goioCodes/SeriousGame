using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string horizontalAxis;
    public string verticalAxis;
    public float movementSpeed;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 moveVec = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)).normalized * movementSpeed;
        rb.MovePosition(rb.position + moveVec);
    }
}
