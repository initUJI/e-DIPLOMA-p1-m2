using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/* 
 * Any class inheriting from LocalScopeBlock can use a local context, i.e. create local variables. 
 * For example, control structures such as while, if or for can create a local context.
 */

public abstract class LocalContextBlock : ExecutableBlock, WithBottomSocket, WithLocalContextSocket
{
    [SerializeField] public XRSocketInteractor bottomSocket, localContextSocket;

    [SerializeField] GameObject blockLink;

    public Block currentFirstBlock;

    public bool isFinished;


    protected Coroutine currentCoroutine;


    private IfBlock currentIfBlock;
    private ForBlock currentForBlock;

    protected void Execute()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(c_Execute());
    }


    // Variables used in a local context.
    protected Dictionary<string, int> variables;

    public override void Start()
    {
        base.Start();
        isFinished = false;
    }

    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public XRSocketInteractor getLocalContextSocket()
    {
        return localContextSocket;
    }
    public void TransmitAllBlocksToReferent()
    {
        currentFirstBlock = getSocketBlock(localContextSocket);
        currentFirstBlock.TransmitNextBlocksToReferent();
        BrowseChildAndUpdate();
    }
    public void ResetAllBlocks()
    {
        currentFirstBlock.ResetNextBlocks();
        TranslateBottomSocket();
        BrowseChildAndUpdate();
    }

    public void TranslateBottomSocket()
    {
        Debug.Log("TranslateBottomSocket !");
    }

    public void BrowseChildAndUpdate()
    {

        Block currentBlock = getSocketBlock(((WithLocalContextSocket)this).getLocalContextSocket());
        while (currentBlock != null)
        {
            currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());
        }

        TranslateBottomSocket();
    }

    /* Go from bottom socket to bottom socket through the blocks.
     * Between each execution two pauses are made, 
     * one to wait for the end of the previous execution, 
     * and another as long as the pause attribute is not equal to false. 
     * In this way, the user has time to see the character perform each step and has the option of pausing.
     */
    protected IEnumerator c_Execute()
    {

        isFinished = false;
        //bool wasIfBlock = false;
        //bool ifConditionChecked = false;

        Debug.Log("LocalContextBlock : BEGIN");
        ExecutableBlock currentBlock = (ExecutableBlock)getSocketBlock(localContextSocket);

        while (currentBlock != null && !MainBlock.error)
        {
            GameManager.currentBlock = currentBlock;

            yield return new WaitUntil(() => !MainBlock.paused); // Wait until pause equals false
            //Debug.Log("MainBlock : Next block !");                     

            if (currentBlock as ForBlock)
            {
                currentForBlock = (ForBlock)currentBlock;
                currentForBlock.Execute(variables);
                
            }

            else if (currentBlock as IfBlock)
            {
                //wasIfBlock = true;
                //ifConditionChecked = false;
                IfBlock ifBlock = (IfBlock)currentBlock;
                if (ifBlock.conditionChecked(variables))
                {
                    //ifConditionChecked = true;
                    ifBlock.Execute(variables);
                    currentIfBlock = ifBlock;
                }
            }
            /*else if (currentBlock as ElseBlock)
            {
                ElseBlock elseBlock = (ElseBlock)currentBlock;
                if (wasIfBlock)
                {
                    if (!ifConditionChecked)
                    {
                        elseBlock.Execute(variables);
                    }
                }
                else
                {
                    GameManager.ReportError(elseBlock, "An else block must be preceded by an if block");
                }
            }*/
            else
            {
            }

            yield return new WaitUntil(() => !MainBlock.error);
            yield return new WaitUntil(() => currentBlock.IsFinished()); // Wait until the end of the previous block;

            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket()); // Go to the next block  
        }


        isFinished = true;
        Debug.Log("LocalContextBlock : END");

    }

    public override bool IsFinished()
    {
        return isFinished;
    }
}
