using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text healthText;

    private PlayerCombat playerCombat;

    private void Awake() {
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update() {
        healthText.text = playerCombat.HP.ToString();
    }
}
