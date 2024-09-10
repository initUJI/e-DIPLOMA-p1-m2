using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/* Block is a script associated with the GameObject of the same name, used to manage its logic. 
 * A Block has 3 sockets, one to its right, one below it and a third to place a local context, 
 * particularly in the case of while or if statements.
*/
public abstract class Block : MonoBehaviour
{
    public Block bottomBlock;

    private Renderer objectRenderer;

    public virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public virtual Block getSocketBlock(XRSocketInteractor socket)
    {
        
            Block block = null;

            if (socket != null && socket.GetOldestInteractableSelected() != null)
            {
                block = socket.GetOldestInteractableSelected().transform.gameObject.GetComponent<Block>();
            } 

            return block;
        
    }

    public void TransmitNextBlocksToReferent()
    {

        bottomBlock = getSocketBlock(((WithBottomSocket)this).getBottomSocket());

        Block currentBlock = bottomBlock;
        while (currentBlock != null)
        {
            if (currentBlock is WithBottomSocket)
            {
                currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());
            }
        }

    }

    public void ResetNextBlocks()
    {

        Block currentBlock = bottomBlock;
        while (currentBlock != null)
        {
            currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());

        }
    }

    public override string ToString()
    {
        return GetType().ToString();
    }

}
