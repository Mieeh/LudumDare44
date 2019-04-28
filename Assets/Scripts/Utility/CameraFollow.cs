using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float follow_speed = 1.0f;

    private void Update(){
        if(transform.position != target.position) {
            transform.position = target.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

}
