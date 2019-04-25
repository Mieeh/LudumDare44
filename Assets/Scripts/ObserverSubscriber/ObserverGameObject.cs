using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObserverGameObject : MonoBehaviour
{
    /*
    Example args could be
    eventType = "playerAction"
    action = "dead"
    or
    eventType = "playerAction"
    action = "jumped"
    */
    public abstract void Notified(string eventType, string action);

}
