using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBlock : ConditionalBlock
{
    bool forIsFinished;
    Coroutine forCoroutine;
    bool endfinded;
    Block endIfBlock;

    public override void Start()
    {
        base.Start();
        forIsFinished = false;
        endfinded = false;
        endIfBlock = null;
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

    private IEnumerator ExecuteBlockChain()
    {
        ExecutableBlock block = (ExecutableBlock)this.getSocketBlock(bottomSocket);
        bool activeIf = false;
        IfBlock ifBlock = null;
        MainBlock mainBlock = FindFirstObjectByType<MainBlock>();
        List<Outline> outlines = mainBlock.FindAllBlockOutlines();
        // Asegurarnos de que no hay bucle infinito
        HashSet<ExecutableBlock> executedBlocks = new HashSet<ExecutableBlock>();

        while (block != null && !checkIfEnd(block) && !endfinded)
        {
            Debug.Log("Executing block: " + block.GetType().Name);
            // Evitar ejecutar el mismo bloque m�s de una vez
            if (executedBlocks.Contains(block))
            {
                Debug.LogError("Detected a loop. Block already executed: " + block.GetType().Name);
                yield break;
            }
            executedBlocks.Add(block);

            yield return new WaitForSeconds(1f);

            if (block is IfBlock)
            {
                ifBlock = (IfBlock)block;
                if (ifBlock.conditionChecked(variables))
                {
                    activeIf = true;
                }
                else
                {
                    ifBlock.isFinished = true;
                }
            }
            else if (block is EndIfBlock)
            {
                ifBlock = null;
                activeIf = false;
                endIfBlock = block;
            }

            else if (activeIf && ifBlock != null || !activeIf && ifBlock == null)
            {
                mainBlock.DisableOutlines(outlines);
                block.Execute(variables);
                block.gameObject.GetComponent<Outline>().enabled = true;
                yield return new WaitUntil(() => block.IsFinished());
            }

            if (block is EndForBlock)
            {
                Debug.Log("Found EndForBlock, breaking out of the loop.");
                endfinded = true;
                break;
            }

            block = (ExecutableBlock)getSocketBlock(((WithBottomSocket)block).getBottomSocket());

            if (block == null)
            {
                Debug.Log("Next block is null, breaking out of the loop.");
                break;
            }
        }

        yield return null;
    }

    public IEnumerator c_ExecuteFor()
    {

        forIsFinished = false;

        Block block = getSocketBlock(rightSocket);

        if (block == null)
        {
            yield return null;
        }

        String toInterpret = ((WithAssociatedString)block).getAssociatedString();
        int number;
        int.TryParse(toInterpret, out number);

        while (number > 0)
        {
            endfinded = false;
            //Debug.Log("Condition v�rifi�e !");
            //Debug.Log(number);
            /*if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }*/
            number--;
            MainBlock mainBlock = FindObjectOfType<MainBlock>();
            mainBlock.activeIf = false;
            mainBlock.ifBlock = null;
            yield return StartCoroutine(ExecuteBlockChain());
            
        }

        forIsFinished = true;
        yield return null;
    }

    public override bool IsFinished()
    {
        return forIsFinished;
    }

    private bool checkIfEnd(Block block)
    {
        if (block.name.Contains("EndFor"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
