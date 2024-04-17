using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine.XR.Interaction.Toolkit;

public class ShelfController : NumberOfBlocks
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject attach;

    public GameObject currentBlock;
    [SerializeField] public int numberForNumberBlock;

    protected virtual void Start()
    {
        blocksForTest();
        if (returnNumberOfBlocks(blockPrefab) > 0)
        {
            StartCoroutine(Tool.c_InvokeAfterWait(0.1f, CreateNewBlock));
            substractNumberOfBlocks(blockPrefab);
        }
    }

    protected virtual void CreateNewBlock()
    {
        currentBlock = Instantiate(blockPrefab);
        currentBlock.transform.parent = attach.transform;

        if (currentBlock.name == "NumberBlock")
        {
            currentBlock.GetComponent<ObjectBlock>().setAssociatedString(numberForNumberBlock.ToString());
        }

        float xPos;
        float yPos;
        if (blockPrefab.name == "TurnLeftBlock" || blockPrefab.name == "TurnRightBlock" || blockPrefab.name == "ObjectBlock"
            || blockPrefab.name == "NumberBlock")
        {
            xPos = 0;
            yPos = 0;
        }
        else
        {
            if (currentBlock.GetComponent<BoxCollider>() != null)
            {
                xPos = currentBlock.GetComponent<BoxCollider>().size.z * 4;
            }
            else
            {
                xPos = currentBlock.transform.GetChild(0).GetComponent<BoxCollider>().size.z * 4;
            }
            yPos = 0.2f;
        }
        
        currentBlock.transform.localPosition = new Vector3(xPos, yPos, 0);
        currentBlock.transform.localRotation = blockPrefab.transform.rotation;
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.gameObject == currentBlock){
            if (returnNumberOfBlocks(blockPrefab) > 0)
            {
                CreateNewBlock();
                substractNumberOfBlocks(blockPrefab);
            }
        }
    }
}