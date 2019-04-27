using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 1.0f;
    float verticalInput, horizontalInput, verticalSpeed, horizontalSpeed;

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
        verticalSpeed = Mathf.Lerp(0, verticalInput, 0.9f);
        horizontalSpeed = Mathf.Lerp(0, horizontalInput, 0.9f);
        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed) * moveSpeed;
    }
}
