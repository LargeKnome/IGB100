using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAction : CutsceneAction
{
    [SerializeField] List<string> dialog;

    public override IEnumerator Play()
    {
        foreach(var line in dialog)
            yield return DialogManager.i.ShowLine(line, line != dialog[dialog.Count - 1]);
    }
}
