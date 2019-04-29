using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Sprite[] animation_frames;
    public float animation_timer;

    public Image image;
    public Image fadePanel;
    public TMPro.TMP_Text press_any_key_text;

    private bool startFlag = false;

    private void Start() {
        StartCoroutine("TitleScreenAnimation");
        StartCoroutine("FlashText");
    }

    private void Update() {
        // Input for starting game
        if(Input.anyKeyDown && !startFlag){
            startFlag = true;
            StartCoroutine("StartGame");
        }
    }

    IEnumerator FlashText(){

        bool eh = false;

        while(true){
            eh = !eh;
            if(eh)
                press_any_key_text.color = Color.clear;
            else
                press_any_key_text.color = Color.white;

            yield return new WaitForSeconds(0.3f);
        }

    }

    IEnumerator TitleScreenAnimation(){

        int i = 0;

        // Animate the title screen
        while(true){
            i++;
            if(i == animation_frames.Length)
                i = 0;

            image.sprite = animation_frames[i];
            yield return new WaitForSeconds(animation_timer); 
        }
    }

    IEnumerator StartGame(){
        const float darkenSpeed = 1.5f;
        
        // Fade out
        while(fadePanel.color != Color.black){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.black, darkenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("main");
    }
}
