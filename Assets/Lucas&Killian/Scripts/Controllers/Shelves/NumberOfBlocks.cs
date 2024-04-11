using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfBlocks : MonoBehaviour
{
    private int numberOfFor = 0;
    private int numberOfForward = 0;
    private int numberOfGetHumidity = 0;
    private int numberOfIf = 0;
    private int numberOfLeft = 0;
    private int numberOfNumber = 0;
    private int numberOfObject = 0;
    private int numberOfRight = 0;
    private int numberOfTurn = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int returnNumberOfBlocks(GameObject block)
    {
        Debug.Log(block.GetComponent<Block>().GetType().ToString());
        switch (block.GetComponent<Block>().GetType().ToString())
        {
            case "TurnBlock":
                return numberOfTurn;
            case "TurnRightBlock":
                return numberOfRight;
            case "ObjectBlock":
                return numberOfObject;
            case "NumberBlock":
                return numberOfNumber;
            case "TurnLeftBlock":
                return numberOfLeft;
            case "IfBlock":
                return numberOfIf;
            case "GetHumidityBlock":
                return numberOfGetHumidity;
            case "ForwardBlock":
                return numberOfForward;
            case "ForBlock":
                return numberOfFor;
            default:
                return 0;
        }
    }

    public void substractNumberOfBlocks(GameObject block)
    {
        switch (block.GetComponent<Block>().GetType().ToString())
        {
            case "TurnBlock":
                numberOfTurn--;
                break;
            case "TurnRightBlock":
                numberOfRight--;
                break;
            case "ObjectBlock":
                numberOfObject--;
                break;
            case "NumberBlock":
                numberOfNumber--;
                break;
            case "TurnLeftBlock":
                numberOfLeft--;
                break;
            case "IfBlock":
                numberOfIf--;
                break;
            case "GetHumidityBlock":
                numberOfGetHumidity--;
                break;
            case "ForwardBlock":
                numberOfForward--;
                break;
            case "ForBlock":
                numberOfFor--;
                break;
            default:
                break;
        }
    }

    public void sumNumberOfBlocks(GameObject block)
    {
        switch (block.GetComponent<Block>().GetType().ToString())
        {
            case "TurnBlock":
                numberOfTurn++;
                break;
            case "TurnRightBlock":
                numberOfRight++;
                break;
            case "ObjectBlock":
                numberOfObject++;
                break;
            case "NumberBlock":
                numberOfNumber++;
                break;
            case "TurnLeftBlock":
                numberOfLeft++;
                break;
            case "IfBlock":
                numberOfIf++;
                break;
            case "GetHumidityBlock":
                numberOfGetHumidity++;
                break;
            case "ForwardBlock":
                numberOfForward++;
                break;
            case "ForBlock":
                numberOfFor++;
                break;
            default:
                break;
        }
    }

    public void blocksForLevel1()
    {
        numberOfFor = 0;
        numberOfForward = 7;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 2;
        numberOfNumber = 0;
        numberOfObject = 0;
        numberOfRight = 2;
        numberOfTurn = 2;
    }

    public void blocksForLevel2()
    {
        numberOfFor = 0;
        numberOfForward = 7;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 2;
        numberOfNumber = 0;
        numberOfObject = 0;
        numberOfRight = 2;
        numberOfTurn = 2;
    }

    public void blocksForLevel3()
    {
        numberOfFor = 1;
        numberOfForward = 1;
        numberOfGetHumidity = 2;
        numberOfIf = 0;
        numberOfLeft = 0;
        numberOfNumber = 0;
        numberOfObject = 0;
        numberOfRight = 0;
        numberOfTurn = 0;
    }

    public void blocksForLevel4()
    {
        numberOfFor = 1;
        numberOfForward = 1;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = 1;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
    }

    public void blocksForLevel5()
    {
        numberOfFor = 1;
        numberOfForward = 3;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = 1;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
    }

    public void blocksForLevel6()
    {
        numberOfFor = 1;
        numberOfForward = 2;
        numberOfGetHumidity = 2;
        numberOfIf = 1;
        numberOfLeft = 2;
        numberOfNumber = 1;
        numberOfObject = 1;
        numberOfRight = 2;
        numberOfTurn = 2;
    }
}
