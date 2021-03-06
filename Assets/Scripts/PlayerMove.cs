﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isSprinting, isMoving, isDodging, isInvincible;
    private float moveSpeed = 4.5f;
    private float sprintSpeed = 6f;
    private float invincibilityTimeInSeconds = 0.7f;
    private float dodgeTimeInSeconds = 0.8f;
    float verticalInput, horizontalInput, verticalSpeed, horizontalSpeed, timeElapsed;
    private KeyCode dashDirection, potentialDashDirection;
    private ArrayList movementKeys;
    private Vector2 dodgeDirection;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        isSprinting = false;
        isMoving = false;

        movementKeys = new ArrayList();
        movementKeys.Add(KeyCode.W);
        movementKeys.Add(KeyCode.A);
        movementKeys.Add(KeyCode.S);
        movementKeys.Add(KeyCode.D);
    }

    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //If the player presses a key during the frame, they are considered to be moving.
        //A check for wether or not the same button is pushed twice is used to determine player sprinting.
        //The sprint stops when the player stops moving.
        foreach (KeyCode key in movementKeys) {
            if (Input.GetKeyUp(key)) {
                if (key == dashDirection) {
                    isSprinting = false;
                }
                potentialDashDirection = key;
                StartCoroutine("DashCheck");
            }
            if (Input.GetKey(key)) {
                isMoving = true;
            }
        }
        //Debug.Log(isMoving);
        if (!isMoving) {
            isSprinting = false;
        }
        isMoving = false;
        

        if (Input.GetKeyDown(InputKeys.DODGE_KEY) && !isDodging) {
            if (horizontalInput != 0 || verticalInput != 0) {
                dodgeDirection.x = Mathf.Lerp(0, horizontalInput, 0.9f);
                dodgeDirection.y = Mathf.Lerp(0, verticalInput, 0.9f);
                timeElapsed = 0.0f;
                StartCoroutine("DodgeTimer");
                StartCoroutine("InvincibilityTimer");
            }
        }
    }

    private void FixedUpdate() {
        // Horizontal-vertical movement
        Move();
    }

    IEnumerator DashCheck() {
        yield return new WaitForSeconds(0.25f);
        if (Input.GetKey(potentialDashDirection)) {
            dashDirection = potentialDashDirection;
            isSprinting = true;
        }
    }

    IEnumerator InvincibilityTimer() {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTimeInSeconds);
        isInvincible = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    IEnumerator DodgeTimer() {
        isDodging = true;
        yield return new WaitForSeconds(dodgeTimeInSeconds);
        isDodging = false;
    }

    void Move(){
        if (isDodging) {
            timeElapsed += Time.deltaTime;
            rb.velocity = dodgeDirection * (moveSpeed / Mathf.Pow(timeElapsed + 0.55f, 2) / dodgeDirection.magnitude);
        } else {
            horizontalSpeed = Mathf.Lerp(0, horizontalInput, 0.9f);
            verticalSpeed = Mathf.Lerp(0, verticalInput, 0.9f);

            float currentSpeed;
            if (isSprinting) currentSpeed = sprintSpeed;
            else currentSpeed = moveSpeed;

            rb.velocity = new Vector2(horizontalSpeed, verticalSpeed) * currentSpeed;
        }
    }
}
