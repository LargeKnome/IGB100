using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatementEvidence : Evidence
{
    Statement currentStatement;
    NPCController madeBy;

    public Statement CurrentStatement => currentStatement;

    public void Init(Statement statement, NPCController source)
    {
        gameObject.name = source.name;
        itemDescription = new string[] { statement.Dialog };

        currentStatement = statement;
        madeBy = source;

        GameController.i.Player.Inventory.AddEvidence(this);
    }
}
