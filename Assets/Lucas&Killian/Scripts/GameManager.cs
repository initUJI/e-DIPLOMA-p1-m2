using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{

    public static Character character;

    static Vector3 characterInitialPosition;
    static Quaternion characterInitialRotation;
    static Vector3 characterInitialScale;

    public static GameObject objectInFront;

    public static TMP_Text printScreenTMP;

    public static XRController principalHandController;

    public static XRController secondaryHandController;

    public static bool whichHand; // 0 <=> right ; 1 <=> left ; 

    static bool objectsFound;

    [SerializeField] Material errorMaterialGiven;
    [SerializeField] Material warningMaterialGiven;

    static Material errorMaterial;
    static Material warningMaterial;

    public static Block currentBlock;

    public static List<GameObject> originalObstacles;
    public static List<GameObject> executionObstacles;


    public void Start()
    {
        whichHand = false;
        errorMaterial = errorMaterialGiven;
        warningMaterial = warningMaterialGiven;
        NewLevel();
    }

    public void NewLevel()
    {
        objectsFound = false;
        
        StartCoroutine(Tool.c_InvokeAfterWait(0.1f, FindObjects));
    }

    private void FindObjects()
    {
        Debug.Log("Init GameManager");
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();
        characterInitialPosition = character.transform.position;
        characterInitialRotation = character.transform.rotation;
        characterInitialScale = character.transform.localScale;

        printScreenTMP = GameObject.FindGameObjectWithTag("TextScreenPrint").GetComponent<TMP_Text>();

        if(!whichHand)
        {
            principalHandController = GameObject.FindGameObjectWithTag("RightHandController").GetComponent<XRController>();
            secondaryHandController = GameObject.FindGameObjectWithTag("LeftHandController").GetComponent<XRController>();
        } else
        {
            secondaryHandController = GameObject.FindGameObjectWithTag("RightHandController").GetComponent<XRController>();
            principalHandController = GameObject.FindGameObjectWithTag("LeftHandController").GetComponent<XRController>();
        }

        originalObstacles = new List<GameObject>();
        originalObstacles.AddRange(GameObject.FindGameObjectsWithTag("TREE"));
        originalObstacles.AddRange(GameObject.FindGameObjectsWithTag("ROCK"));
        originalObstacles.AddRange(GameObject.FindGameObjectsWithTag("OBSTACLE"));
        originalObstacles.AddRange(GameObject.FindGameObjectsWithTag("MISTERIOUS"));

        executionObstacles = new List<GameObject>();

        Debug.Log("End Init GameManager");
        objectsFound = true;
    }

    private void Update()
    {
        //Debug.Log(objectInFront.tag.ToString());
        
        if (GameManager.objectInFront != null && GameManager.objectInFront.CompareTag("FLAG"))
        {
            Debug.Log("Bear : \" There is a flag ! \" ");
            GameManager.printScreenTMP.text = "Level ended !";
        }
    }

    public static IEnumerator c_InvokeAfterInit(Action action)
    {
        yield return new WaitUntil(() => objectsFound);
        action.Invoke();
        yield return null;
    }

    public static void InvokeAfterInit(MonoBehaviour obj, Action action)
    {
        obj.StartCoroutine(c_InvokeAfterInit(action));
    }

    public static void Reset()
    {
        character.transform.position = characterInitialPosition;
        character.transform.rotation = characterInitialRotation;
        character.transform.localScale = characterInitialScale;
        character.ResetTheTargetPosition(character.transform.position, character.transform.rotation);

        foreach(GameObject go in originalObstacles)
        {
            go.SetActive(true);
            Rigidbody goRigidbody = go.GetComponent<Rigidbody>();
            if (goRigidbody != null)
            {
                goRigidbody.detectCollisions = true;
            }
        }
        foreach (GameObject go in executionObstacles)
        {
            Destroy(go);
        }

    }

    public static void ReportError(MonoBehaviour obj, string message)
    {
        printScreenTMP.text = "ERROR in " + obj.ToString() + " : " + message;
        MainBlock.error = true;
        
        if(obj as Block)
        {
            obj.StartCoroutine(MakeBlockFlash((Block)obj, errorMaterial));
        }
        
    }

    public static void ReportWarning(MonoBehaviour obj, string message)
    {
        printScreenTMP.text = "WARNING in " + obj.ToString() + " : " + message;

        if (obj as Block)
        {
            obj.StartCoroutine(MakeBlockFlash((Block)obj, warningMaterial));
        }
    }

    static IEnumerator MakeBlockFlash(Block block, Material material)
    {
        for(int i = 0; i < 3; i++)
        {
            block.ChangeBlockMaterial(material);
            yield return new WaitForSeconds(0.3f);
            block.InitBlockMaterial();
            yield return new WaitForSeconds(0.3f);
        }
        
    }

    public static void DisplayOnPrompt(string message)
    {
        printScreenTMP.text = message;
    }

}
