using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] EvidenceUI objectInspection;

    [SerializeField] GameObject noteImage;
    [SerializeField] TextMeshProUGUI noteText;

    public void Init(Evidence currentEvidence)
    {
        objectInspection.gameObject.SetActive(false);
        noteImage.SetActive(false);

        if(currentEvidence is Key || currentEvidence is EvidenceObj)
        {
            objectInspection.gameObject.SetActive(true);
            objectInspection.Init(currentEvidence);
        }
        if(currentEvidence is Note note)
        {
            noteImage.SetActive(true);

            string output = "";

            foreach(var line in note.Page)
                output += line + "\n";

            noteText.text = output;
        }
    }
}
