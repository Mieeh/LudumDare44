using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMove : MonoBehaviour
{
    public List<GameObject> itemSlots = new List<GameObject>();
    private int cursorPosition = 0;
    private ArrayList keysToCheck;
    // Start is called before the first frame update
    void Start()
    {
        setCursorPosition();

        keysToCheck = new ArrayList();
        keysToCheck.Add(KeyCode.W);
        keysToCheck.Add(KeyCode.A);
        keysToCheck.Add(KeyCode.S);
        keysToCheck.Add(KeyCode.D);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode key in keysToCheck) {
            if (Input.GetKeyDown(key)) {
                moveCursor(key);
            }
        }
    }

    void moveCursor(KeyCode direction) {
        switch (direction) {
        case KeyCode.W:
            if (cursorPosition == 0) {
                cursorPosition = 14;
            } else if (cursorPosition == 1){
                cursorPosition = 15;
            } else {
                cursorPosition -= 2;
            }
            break;
        case KeyCode.A:
            if (cursorPosition % 2 == 0) {
                cursorPosition++;
            } else {
                cursorPosition--;
            }
            break;
        case KeyCode.S:
            if (cursorPosition == 14) {
                cursorPosition = 0;
            } else if (cursorPosition == 15){
                cursorPosition = 1;
            } else {
                cursorPosition += 2;
            }
            break;
        case KeyCode.D:
            if (cursorPosition % 2 == 0) {
                cursorPosition++;
            } else {
                cursorPosition--;
            }
            break;
        }
        setCursorPosition();
    }

    void setCursorPosition(){
        transform.position = itemSlots[cursorPosition].transform.position;
    }
}
