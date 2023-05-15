using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reliability : MonoBehaviour
{
    int currentReliability = 100;

    int gainedReliability = 0;
    int lostReliability = 0;

    public static Reliability i;

    public event Action<int> OnReliabilityChanged;

    private void Awake()
    {
        i = this;
    }

    public void AffectReliability(int diff)
    {
       if(diff < 0)
            lostReliability += diff;
        else
            gainedReliability += diff;

       currentReliability += diff;

        if (currentReliability > 100)
            currentReliability = 100;

        if(currentReliability <= 0)
        {
            //Game over
        }

        OnReliabilityChanged?.Invoke(currentReliability);
    }
}
