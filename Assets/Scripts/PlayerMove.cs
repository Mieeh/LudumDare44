using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isSprinting, isMoving;
    private float moveSpeed = 2.0f;
    private float sprintSpeed = 3.0f;
    float verticalInput, horizontalInput, verticalSpeed, horizontalSpeed;
    private KeyCode dashDirection, potentialDashDirection;
    private ArrayList keysToCheck;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
        isSprinting = false;
        isMoving = false;

        keysToCheck = new ArrayList();
        keysToCheck.Add(KeyCode.W);
        keysToCheck.Add(KeyCode.A);
        keysToCheck.Add(KeyCode.S);
        keysToCheck.Add(KeyCode.D);
    }

    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //If the player presses a key during the frame, they are considered to be moving.
        //A check for wether or not the same button is pushed twice is used to determine player sprinting.
        //The sprint stops when the player stops moving.
        foreach (KeyCode key in keysToCheck) {
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

    void Move(){
        //Debug.Log(isSprinting);
        verticalSpeed = Mathf.Lerp(0, verticalInput, 0.9f);
        horizontalSpeed = Mathf.Lerp(0, horizontalInput, 0.9f);

        float currentSpeed;
        if (isSprinting) currentSpeed = sprintSpeed;
        else currentSpeed = moveSpeed;

        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed) * currentSpeed;
    }
}
