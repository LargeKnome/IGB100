using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reliability : MonoBehaviour
{
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip loss;

    int currentReliability = 100;

    int gainedReliability = 0;
    int lostReliability = 0;

    public static Reliability i;

    public event Action<int> OnReliabilityChanged;

    [SerializeField] Cutscene failedCutscene;

    private void Awake()
    {
        i = this;
    }

    public void AffectReliability(int diff)
    {
        if (diff < 0)
        {
            lostReliability += diff;
            AudioManager.i.PlaySFX(loss);
        }
        else
        {
            gainedReliability += diff;
            AudioManager.i.PlaySFX(success);
        }

        currentReliability += diff;

        currentReliability = Mathf.Clamp(currentReliability, 0, 100);

        OnReliabilityChanged?.Invoke(currentReliability);

        if (currentReliability == 0)
        {
            while (GameController.i.StateMachine.CurrentState != FreeRoamState.i)
                GameController.i.StateMachine.Pop();

            failedCutscene.StartCutscene();
        }
    }
}
