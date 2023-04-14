using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    // Update is called once per frame
    FirstPersonController player;
    private void Start()
    {
        player = GameController.i.Player;
    }

    void Update()
    {
        if (player.Interactable())
            GetComponent<Image>().color = Color.yellow;
        else
            GetComponent<Image>().color = Color.white;
    }
}
