using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSelectionUI : SelectionUI<NPCSelectionText>
{
    [SerializeField] GameObject selectionTextPrefab;

    List<NPCSelectionText> selectionList;
    public void Init(List<NPCController> npcs)
    {
        selectionList = new();

        foreach(Transform child in transform)
            Destroy(child.gameObject);

        foreach (var npc in npcs)
        {
            var selectionObj = Instantiate(selectionTextPrefab, transform);
            selectionObj.GetComponent<NPCSelectionText>().Init(npc);
            selectionList.Add(selectionObj.GetComponent<NPCSelectionText>());
        }

        SetItems(selectionList, 1);
    }
}
