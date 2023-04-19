using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelController : MonoBehaviour
{
    FirstPersonController player;
    private void Start()
    {
        player = GameController.i.Player;
    }

    void Update()
    {
        if (player.Interactable())
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = player.GetInteractableName(); 
        }
        else
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = ""; 
        }
    }
}
