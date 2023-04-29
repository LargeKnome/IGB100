using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCSelectionText : MonoBehaviour, ISelectableItem
{
    TextMeshProUGUI textMesh;

    public NPCController CurrentNPC { get; private set; }

    public void Init(NPCController npc)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = npc.name;
        CurrentNPC = npc;
    }

    public void OnSelectionChanged(bool selected)
    {
        textMesh.color = selected ? Color.blue : Color.black;
    }
}
