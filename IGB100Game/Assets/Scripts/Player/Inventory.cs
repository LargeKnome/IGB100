using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Evidence> evidenceObjList;
    List<Evidence> keyList;
    List<Evidence> statementList;

    public List<Evidence> EvidenceObjList => evidenceObjList;
    public List<Evidence> KeyList => keyList;
    public List<Evidence> StatementList => statementList;

    public List<List<Evidence>> Evidence;

    private void Awake()
    {
        evidenceObjList = new List<Evidence>();
        keyList = new List<Evidence>();
        statementList = new List<Evidence>();

        Evidence = new()
        {
            evidenceObjList,
            keyList,
            statementList
        };
    }

    public void AddEvidence(Evidence newEvidence)
    {
        if (newEvidence is EvidenceObj evidenceObj)
            evidenceObjList.Add(evidenceObj);
        else if (newEvidence is Key evidenceKey)
            keyList.Add(evidenceKey);
        else if (newEvidence is StatementEvidence statementEvidence)
            statementList.Add(statementEvidence);
    }

    public bool HasEvidence(Evidence evidenceToCheck)
    {
        if (evidenceToCheck is EvidenceObj evidenceObj)
            return evidenceObjList.Cast<EvidenceObj>().Where(o => o == evidenceObj).FirstOrDefault() != null;
        else if (evidenceToCheck is Key key)
            return keyList.Cast<Key>().Where(o => o == key).FirstOrDefault() != null;
        else if (evidenceToCheck is StatementEvidence statementEvidence)
            return HasStatement(statementEvidence.CurrentStatement);

        return false;
    }

    public bool HasStatement(Statement compare)
    {
        return statementList.Cast<StatementEvidence>().Where(o => o.CurrentStatement == compare).FirstOrDefault() != null;
    }
}
