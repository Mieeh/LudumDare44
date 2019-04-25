using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    // List of all observers
    public List<ObserverGameObject> observerList;

    private void Start(){
        // Go through the scene and find all observers
        var tempList = FindObjectsOfType<ObserverGameObject>();
        foreach(var x in tempList){
            observerList.Add(x);
        }
    }

    public void Notify(string eventType, string action){
        foreach(var _obersver in observerList){
            _obersver.Notified(eventType, action);
        }
    }
}
