using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 1.0f;
    float verticalInput, horizontalInput;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
    }

    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
        // Horizontal-vertical movement
        Move();
    }

    void Move(){
        rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;
    }
}
