using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pause_object;
    private bool is_paused = false;
    public UnityEngine.PostProcessing.PostProcessingProfile x;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            is_paused = !is_paused;
            if(is_paused){
                pause_object.SetActive(true);
                Time.timeScale = 0.0f;
                x.grain.enabled = true;
            }
            else{
                pause_object.SetActive(false);
                Time.timeScale = 1.0f;
                x.grain.enabled = false;
            }
        }
    }
}
