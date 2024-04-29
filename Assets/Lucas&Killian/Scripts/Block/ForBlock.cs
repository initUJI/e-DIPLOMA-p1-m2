using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using UnityEngine.WSA;
//using UnityEngine.XR.Interaction.Toolkit;

public class ForBlock : ConditionalBlock
{
    bool forIsFinished;
    Coroutine forCoroutine;

    public override void Start()
    {
        base.Start();
        forIsFinished = false;
    }
    public override void Execute(Dictionary<string, int> variables)
    {
        this.variables = variables;

        if (forCoroutine != null)
        {
            StopCoroutine(forCoroutine);
        }

        forCoroutine = StartCoroutine(c_ExecuteFor());
    }

    public IEnumerator c_ExecuteFor()
    {
        forIsFinished = false;

        Block block = getSocketBlock(rightSocket);
        String toInterpret = ((WithAssociatedString)block).getAssociatedString();
        int number;
        int.TryParse(toInterpret, out number);

        while (number > 0)
        {
            Debug.Log("Condition vérifiée !");
            Debug.Log(currentCoroutine);
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Launch and wait the end of c_Execute
            yield return (currentCoroutine = StartCoroutine(c_Execute()));
            startFor();
            number--;
        }

        Debug.Log("eeeeeeeeeeeeeeeeeeeeeeeeeeee");
        forIsFinished = true;
    }

    public override bool IsFinished()
    {
        return forIsFinished;
    }

    public void startFor()
    {
        ExecutableBlock block = (ExecutableBlock)getSocketBlock(bottomSocket);
        Debug.Log(block);
        block.Execute(variables);
        /*while (block != null)
        {
            block.Execute(variables);
            block = (ExecutableBlock)getSocketBlock(bottomSocket);
        }*/
    }

}
