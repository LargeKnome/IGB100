using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneAction
{
    public virtual IEnumerator Play()
    {
        yield break;
    }
}
