using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    public float movementSpeed;
    public Animator animator;

    public string horizontalAxis { get; set; }
    public string verticalAxis { get; set; }
    public string useAxis { get; set; }
    public string cancelAxis { get; set; }

    Rigidbody2D rb;
    Workstation interactableWorkstation = null;
    bool working = false;

    Vector2 input = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        horizontalAxis = "Horizontal" + playerID;
        verticalAxis = "Vertical" + playerID;
        useAxis = "Fire" + playerID;
        cancelAxis = "Cancel" + playerID;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis(horizontalAxis);
        input.y = Input.GetAxis(verticalAxis);

        if(Input.GetAxis(useAxis) == 1)
        {
            if(interactableWorkstation != null)
            {
                interactableWorkstation.Work(this);
                working = true;
            }
        }
        if(working && Input.GetAxis(cancelAxis) == 1)
        {
            interactableWorkstation.Leave(this);
            working = false;
        }
    }

    private void FixedUpdate()
    {
        if (!working)
        {
            Vector2 moveVec = input.normalized * movementSpeed;
            rb.MovePosition(rb.position + moveVec);
        }

        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Magnitude", input.magnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Workstation"))
        {
            interactableWorkstation = collision.gameObject.GetComponent<Workstation>();
            interactableWorkstation.SetOutlineActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workstation"))
        {
            interactableWorkstation.SetOutlineActive(false);
            interactableWorkstation = null;
        }
    }
}
