using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using System;
//using UnityEngine.WSA;
//using UnityEngine.XR.Interaction.Toolkit;

public class ForBlock : ConditionalBlock
{
    bool forIsFinished;
    Coroutine forCoroutine;
    bool endfinded;

    public override void Start()
    {
        base.Start();
        forIsFinished = false;
        endfinded = false;
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
            endfinded = false;
            Debug.Log("Condition vérifiée !");
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Launch and wait the end of c_Execute
            yield return (currentCoroutine = StartCoroutine(c_Execute()));
            StartCoroutine(startFor());
            number--;
        }

        forIsFinished = true;
    }

    public override bool IsFinished()
    {
        return forIsFinished;
    }

    public IEnumerator startFor()
    {
        ExecutableBlock block = this;
        if ((ExecutableBlock)getSocketBlock(bottomSocket) != null)
        {
            block = (ExecutableBlock)getSocketBlock(bottomSocket);
        } 

        if (block != null && !checkIfEnd(block) && !endfinded)
        {
            block.Execute(variables);
        }
        else if (checkIfEnd(block))
        {
            endfinded = true;
        }

        yield return new  WaitForSeconds(1f);

        if ((ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket()) != null)
        {
            block = (ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket());
        }

        if (block != null && !checkIfEnd(block) && !endfinded)
        {
            block.Execute(variables);
        }
        else if (checkIfEnd(block))
        {
            endfinded = true;
        }

        yield return new WaitForSeconds(1f);

        if ((ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket()) != null)
        {
            block = (ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket());
        }

        if (block != null && !checkIfEnd(block) && !endfinded)
        {
            block.Execute(variables);
        }
        else if (checkIfEnd(block))
        {
            endfinded = true;
        }

        yield return new WaitForSeconds(1f);

        if ((ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket()) != null)
        {
            block = (ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket());
        }

        if (block != null && !checkIfEnd(block) && !endfinded)
        {
            block.Execute(variables);
        }
        else if (checkIfEnd(block))
        {
            endfinded = true;
        }

        yield return new WaitForSeconds(1f);

        if ((ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket()) != null)
        {
            block = (ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket());
        }

        if (block != null && !checkIfEnd(block) && !endfinded)
        {
            block.Execute(variables);
        }
        else if (checkIfEnd(block))
        {
            endfinded = true;
        }

        yield return new WaitForSeconds(1f);
    }

    private bool checkIfEnd(Block block)
    {
        if (block.name.Contains("End"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
