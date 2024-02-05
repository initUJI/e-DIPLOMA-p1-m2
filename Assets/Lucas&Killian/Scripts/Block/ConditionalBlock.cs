using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ConditionalBlock : LocalContextBlock, WithRightSocket
{

    [SerializeField] protected XRSocketInteractor rightSocket;

    [SerializeField] Material TODELETE;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Start()
    {
        base.Start();
        variables = new Dictionary<string, int>();
    }


    public bool conditionChecked(Dictionary<string, int> variables)
    {
        String toInterpret = "";
        Block block = getSocketBlock(rightSocket);
        
        if (block == null)
        {
            GameManager.ReportError(this, "Missing condition");    
        }

        while (block != null)
        {
            toInterpret += ((WithAssociatedString)block).getAssociatedString();
            if(block is WithRightSocket)
            {
                block = getSocketBlock(((WithRightSocket)block).getRightSocket());
            } else
            {
                block = null;
            } 
        }

        bool result = InterpretConditionalExpression(toInterpret, variables);

        if(!result)
        {
            isFinished = true;
        } 

        return InterpretConditionalExpression(toInterpret, variables);
    }

    public bool InterpretConditionalExpression(string expression, Dictionary<string, int> variables)
    {
        bool result = false;
        switch(expression)
        {
            case "TREE":
            case "ROCK":
            case "FLAG":
                if (GameManager.objectInFront != null)
                {
                    result = GameManager.objectInFront.CompareTag(expression);
                }
                break;
            case "!TREE":
            case "!ROCK":
            case "!FLAG":
                result = true;
                if (GameManager.objectInFront != null)
                {
                    string expressionCut = expression.Substring(1);
                    result = !(GameManager.objectInFront.CompareTag(expressionCut));
                }
                break;
            default:
                string localExpression = (Tool.ReplaceVariablesInExpression(expression, variables)).Replace("==", "="); ;

                try
                {
                    DataTable dt = new DataTable();
                    result = (bool)dt.Compute(localExpression, "");
                } catch
                {
                    GameManager.ReportError(this, "Incorrect Boolean expression");
                }

                break;
        }

        return result;
        
    }

}

       
   
