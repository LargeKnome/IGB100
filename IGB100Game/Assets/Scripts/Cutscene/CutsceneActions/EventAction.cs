using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAction : CutsceneAction
{
    [SerializeField] UnityEvent EventToInvoke;

    public override IEnumerator Play()
    {
        EventToInvoke?.Invoke();
        yield return null;
    }
}
