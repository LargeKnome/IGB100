using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Evidence> evidenceList;

    public List<Evidence> EvidenceList => evidenceList;

    private void Awake()
    {
        evidenceList = new List<Evidence>();
    }

    public void AddEvidence(Evidence newEvidence)
    {
        evidenceList.Add(newEvidence);
    }
}
