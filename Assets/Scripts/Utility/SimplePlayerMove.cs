using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ContactSides{
    LEFT,
    RIGHT,
    NONE
}

public class SimplePlayerMove : MonoBehaviour
{
    // Unity Components
    private Rigidbody2D rbody;
    private BoxCollider2D boxCollider;

    public float horizontalMoveSpeed = 4.5f;
    public float jumpForce = 7f;

    private const float airFriction = 0.8f; // LMAO
    private bool doubleJumpFlag = false;
    private bool isSpriting = false;

    [Header("Optional Settings")]
    // Optional Settings
    public bool canDoubleJump;
    public bool canSprint;
    [Tooltip("Factor for speed increase, based on horizontalMoveSpeed")]
    public float sprintSpeedFactor = 1.0f;

    void Awake() {
        rbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();        
    }

    void Update(){
        HorizontalMove();

        if(Input.GetKeyDown(KeyCode.W)){
            Jump();
        }

        // Sprint check
        if(Input.GetKey(KeyCode.LeftShift)){
            isSpriting = true;
        }
        else{
            isSpriting = false;
        }
    }

    private void HorizontalMove(){
        float xInput = Input.GetAxisRaw("Horizontal");

        // Duct-tape physics
        bool grounded = IsGrounded();
        if(!grounded) 
            xInput*=airFriction;

        if(grounded && isSpriting) 
            xInput*=sprintSpeedFactor;
        else if(!grounded && isSpriting) 
            xInput *= (Mathf.Max(sprintSpeedFactor*0.8f, 1.0f));

        rbody.velocity = new Vector2(xInput*horizontalMoveSpeed, rbody.velocity.y);
    }

    private void Jump(){
        if(IsGrounded()){
            rbody.velocity = new Vector2(rbody.velocity.x, jumpForce);
            FindObjectOfType<Subject>().Notify("player", "jumped");
        }
        else{
            if(canDoubleJump && doubleJumpFlag){
                rbody.velocity = new Vector2(rbody.velocity.x, jumpForce);
                doubleJumpFlag = false;
                FindObjectOfType<Subject>().Notify("player", "double jumped");
            }
        }
    }

    private bool IsGrounded(){
        float rayOffset = transform.localScale.y/2 + 0.1f;
        float rayLength = 0.1f;

        RaycastHit2D ray = Physics2D.Raycast((Vector2)transform.position + Vector2.down*rayOffset, 
        Vector2.down, rayLength);

        if(ray.collider != null){
            return true;
        }
    
        return false;
    }

    private ContactSides ContactSideCheck(){
        float rayOffset = transform.localScale.x/2 + 0.05f;
        float rayLength = 0.05f;

        // Right check
        RaycastHit2D ray = Physics2D.Raycast((Vector2)transform.position + Vector2.right*rayOffset, 
        Vector2.right, rayLength);

        if(ray.collider != null){
            return ContactSides.RIGHT;
        }

        // Left check
        ray = Physics2D.Raycast((Vector2)transform.position + Vector2.left*rayOffset,
        Vector2.left, rayLength);

        if(ray.collider != null){
            return ContactSides.LEFT;
        }
        
        return ContactSides.NONE;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(canDoubleJump){
            if(IsGrounded()){
                doubleJumpFlag = true;
            }
        }
    }

}
