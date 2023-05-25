using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] EvidenceUI objectInspection;

    [SerializeField] GameObject noteImage;
    [SerializeField] TextMeshProUGUI noteText;

    [SerializeField] Image characterImage;

    public void Init(Evidence currentEvidence)
    {
        objectInspection.gameObject.SetActive(false);
        noteImage.SetActive(false);
        characterImage.gameObject.SetActive(false);

        if(currentEvidence is Key || currentEvidence is EvidenceObj)
        {
            objectInspection.gameObject.SetActive(true);
            objectInspection.Init(currentEvidence);
        }
        else if(currentEvidence is Note note)
        {
            noteImage.SetActive(true);

            string output = "";

            foreach(var line in note.Page)
                output += line + "\n";

            noteText.text = output;
        }
        else if(currentEvidence is StatementEvidence statement)
        {
            noteImage.SetActive(true);

            noteText.text = statement.CurrentStatement.Dialog;
        }
        else if(currentEvidence is NPCController npc)
        {
            characterImage.gameObject.SetActive(true);
            characterImage.sprite = npc.Image;
            characterImage.preserveAspect = true;
        }
    }
}
