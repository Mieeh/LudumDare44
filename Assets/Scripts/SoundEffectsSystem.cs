using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsSystem : MonoBehaviour
{
    
    private static Dictionary<string, AudioClip> sfx_list = new Dictionary<string, AudioClip>();
    private static bool loaded_sfx = false;

    private void Awake() {
        // Should we load SFX?
        if(!loaded_sfx)
            LoadAllSFX();
    }

    public static void PlaySFX(string name, float volume = 1.0f, ulong delay = 0){
        if(sfx_list.ContainsKey(name)){
            // Spawn sfx
            GameObject temp = new GameObject();
            
            temp.AddComponent<AudioSource>().clip = sfx_list[name];
            temp.GetComponent<AudioSource>().volume = volume;

            temp.GetComponent<AudioSource>().Play(delay);
            Destroy(temp, sfx_list[name].length);
        }
    }

    private static void LoadAllSFX(){
        var temp = Resources.LoadAll<AudioClip>("");
        foreach(var x in temp){
            sfx_list.Add(x.name, x);
        }
    }

}
