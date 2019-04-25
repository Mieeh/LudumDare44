using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentTest : ObserverGameObject
{
    public override void Notified(string eventType, string action) {
        if(eventType == "player"){
            print("Test from Camera.main.AchivmentTest.Notified, testing the observer pattern");
            if(action == "jumped"){
                print("Player Jumped!");
            }
            else if(action == "double jumped"){
                print("Player Double Jumped!");
            }
        }
    }
}
