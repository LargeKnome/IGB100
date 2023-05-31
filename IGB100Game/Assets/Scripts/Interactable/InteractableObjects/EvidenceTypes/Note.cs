using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Evidence, Interactable
{
    [SerializeField] List<string> page;

    public List<string> Page => page;

    protected override void OnStart()
    {
        SetUpMats();
    }

    public IEnumerator Interact()
    {
        yield return OnPickup();
    }
}
