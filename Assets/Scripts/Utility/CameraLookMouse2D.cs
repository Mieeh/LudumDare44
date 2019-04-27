using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookMouse2D : MonoBehaviour
{
    
    private void Update() {
        float x = Input.mousePosition.x / Screen.width;
    }

}
