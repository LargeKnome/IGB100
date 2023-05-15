using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReliabilityUI : MonoBehaviour
{
    [SerializeField] RectTransform reliabilityBar;
    void Start()
    {
        Reliability.i.OnReliabilityChanged += UpdateReliability;
    }

    void UpdateReliability(int reliability)
    {
        reliabilityBar.localScale = new Vector3(reliability / 100f, 1, 1);
    }
}
