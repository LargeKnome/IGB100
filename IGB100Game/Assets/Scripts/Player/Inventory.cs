using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Evidence> evidenceObjList;
    List<Evidence> keyList;
    List<Evidence> statementList;
    List<Evidence> npcList;

    public List<List<Evidence>> Evidence;

    public static Inventory i;

    private void Awake()
    {
        i = this;

        evidenceObjList = new List<Evidence>();
        keyList = new List<Evidence>();
        statementList = new List<Evidence>();
        npcList = new List<Evidence>();

        Evidence = new()
        {
            evidenceObjList,
            keyList,
            statementList,
            npcList
        };
    }

    public void AddEvidence(Evidence newEvidence)
    {
        if (HasEvidence(newEvidence))
            return;

        if (newEvidence is EvidenceObj evidenceObj)
            evidenceObjList.Add(evidenceObj);
        else if (newEvidence is Key evidenceKey)
            keyList.Add(evidenceKey);
        else if (newEvidence is StatementEvidence statementEvidence)
            statementList.Add(statementEvidence);
        else if (newEvidence is NPCController npcEvidence)
            npcList.Add(npcEvidence);
    }

    public bool HasEvidence(Evidence evidenceToCheck)
    {
        if (evidenceToCheck is EvidenceObj evidenceObj)
            return evidenceObjList?.Cast<EvidenceObj>().Where(o => o == evidenceObj).FirstOrDefault() != null;
        else if (evidenceToCheck is Key key)
            return keyList?.Cast<Key>().Where(o => o == key).FirstOrDefault() != null;
        else if (evidenceToCheck is StatementEvidence statementEvidence)
            return HasStatement(statementEvidence.CurrentStatement);
        else if(evidenceToCheck is NPCController npcEvidence)
            return npcList?.Cast<EvidenceObj>().Where(o => o == npcEvidence).FirstOrDefault() != null;

        return false;
    }

    public bool HasStatement(Statement compare)
    {
        return statementList?.Cast<StatementEvidence>().Where(o => o.CurrentStatement == compare).FirstOrDefault() != null;
    }
}
