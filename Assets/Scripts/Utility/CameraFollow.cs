using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float follow_speed = 1.0f;

    // Cam shake
    private bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;

    private Vector3 OriginalPos;
    private Quaternion OriginalRot;

    private void Update(){
        if(transform.position != target.position) {
            transform.position = target.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        if (ShakeIntensity > 0)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10) + Random.insideUnitSphere * ShakeIntensity;
            transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

            ShakeIntensity -= ShakeDecay*Time.deltaTime;
        }   
        else if (Shaking)
        {
            transform.eulerAngles = Vector3.zero;
            Shaking = false;
        }
    }

    public void DoShake(float intensity, float length)
    {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;

        ShakeIntensity = intensity;
        ShakeDecay = length;
        Shaking = true;
    }

    public void LightShake(){
        DoShake(0.05f, 0.25f);
    }

    public void MediumShake(){
        DoShake(0.08f, 0.3f);
    }

}
