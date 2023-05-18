using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReliabilityUI : MonoBehaviour
{
    [SerializeField] RectTransform reliabilityBar;

    private void Start()
    {
        Reliability.i.OnReliabilityChanged += UpdateReliability;
        UpdateReliability(100);
    }

    void UpdateReliability(int reliability)
    {
        reliabilityBar.localScale = new Vector3(reliability / 100f, 1, 1);

        if(reliability < 25)
            reliabilityBar.GetComponent<Image>().color = Color.red;
        else if (reliability < 50)
            reliabilityBar.GetComponent<Image>().color = Color.yellow;
        else
            reliabilityBar.GetComponent<Image>().color = Color.green;
    }
}
