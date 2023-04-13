using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<FirstPersonController>().gameObject;
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y + 180, 0);
    }
}
