using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
//using Unity.Burst.Intrinsics;
//using System.Net.Sockets;
//using UnityEngine.UIElements.Experimental;
//using static UnityEditor.Sprites.Packer;

/*
 * MainBlock is associated with the game object of the same name. 
 * This is the starting point for the game. It is under this component that the user adds blocks. 
 * A button is provided to launch the execution of the algorithm thus created. 
 * It is also possible to pause the execution of the algorithm with an other button.
 */
public class MainBlock : Block, WithBottomSocket
{

    // Globals variables.
    public static Dictionary<string, int> variables;

    [SerializeField] XRSocketInteractor bottomSocket;
    [HideInInspector] public ExecutableBlock currentBlock;

    public static bool paused;
    public static bool error;

    Coroutine currentCoroutine;

    bool activeIf = false;
    IfBlock ifBlock;

    bool activeFor = false;
    ForBlock forBlock;

    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public override void Start() {
        base.Start();
        variables = new Dictionary<string, int>();
        paused = false;

        //Execute();
    }

    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.P))
        {
            Execute();
        }
    }

    /* Calls the c_Execute coroutine */
    public void Execute() {

        if (!paused)
        {
            GameManager.DisplayOnPrompt("Execution !");
            error = false;
            variables = new Dictionary<string, int>();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine(c_Execute());

        } else
        {
            GameManager.ReportError(this, "You cannot execute or re-execute when the pause is active");
        }

    }

    /* Go from bottom socket to bottom socket through the blocks.
     * Between each execution two pauses are made, 
     * one to wait for the end of the previous execution, 
     * and another as long as the pause attribute is not equal to false. 
     * In this way, the user has time to see the character perform each step and has the option of pausing.
     */
    IEnumerator c_Execute() {

        bool wasIfBlock = false;
        bool ifConditionChecked = false;

        SetGlowing(true);

        Debug.Log("MainBlock : We wait 1 second...");
        yield return new WaitForSeconds(1);

        GameManager.Reset();

        Debug.Log("MainBlock : BEGIN");
        currentBlock = (ExecutableBlock) getSocketBlock(bottomSocket);

        while (currentBlock != null && !error)
        {
            GameManager.currentBlock = currentBlock;

            currentBlock.SetGlowing(true);

            yield return new WaitUntil(() => !paused); // Wait until pause equals false
                                                       //Debug.Log("MainBlock : Next block !");

            if (currentBlock as ForBlock)
            {
                activeFor = true;
                forBlock = (ForBlock)currentBlock;
                currentBlock.Execute(variables);
            }

            else if (currentBlock as IfBlock)
            {
                wasIfBlock = true;
                ifConditionChecked = false;
                ifBlock = (IfBlock)currentBlock;
                if (ifBlock.conditionChecked(variables))
                {
                    ifConditionChecked = true;
                    ifBlock.Execute(variables);
                    activeIf = true;
                } else
                {
                    ifBlock.isFinished = true;
                }
            }
            else if (currentBlock as EndIfBlock)
            {
                activeIf = false;
                ifBlock = null;
                currentBlock.Execute(variables);
            }
            else if (currentBlock as EndForBlock)
            {
                
                activeFor = false;
                currentBlock.Execute(variables);
            }
            else if (currentBlock as ElseBlock) {
                ElseBlock elseBlock = (ElseBlock) currentBlock;
                if (wasIfBlock)
                {
                    if(!ifConditionChecked)
                    {
                        elseBlock.Execute(variables);
                    } else
                    {
                        elseBlock.isFinished = true;
                    }
                } else
                {
                    GameManager.ReportError(elseBlock, "An else block must be preceded by an if block");
                }
            } else {
                if (activeIf && ifBlock != null)
                {
                    currentBlock.Execute(variables); // Execute the current block
                }
                else if (ifBlock == null && !activeIf)
                {
                    currentBlock.Execute(variables); // Execute the current block
                }

                else if (activeFor && forBlock != null)
                {
                    currentBlock.Execute(variables); // Execute the current block
                }
                else if (forBlock == null && !activeFor)
                {
                    currentBlock.Execute(variables); // Execute the current block
                }

                else if(ifBlock == null && !activeIf || forBlock == null && !activeFor)
                {
                    currentBlock.Execute(variables); // Execute the current block
                }
            }

            Debug.Log("W");
            yield return new WaitUntil(() => !error);
            yield return new WaitUntil(() => currentBlock.IsFinished()); // Wait until the end of the previous block;
            Debug.Log("B");

            currentBlock.SetGlowing(false);
            
            currentBlock = (ExecutableBlock)currentBlock.getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket()); // Go to the next block  
        }

        SetGlowing(false);

        Debug.Log("MainBlock : END");

    }

    public void TooglePause()
    {
        paused = !paused;
    }

    
}
