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
    public static GameObject objectInBehind;

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

    private EventsManager eventsManager;

    public void Start()
    {
        whichHand = false;
        errorMaterial = errorMaterialGiven;
        warningMaterial = warningMaterialGiven;
        eventsManager = FindObjectOfType<EventsManager>();
    }

    public void setCharacter(Character c)
    {
        character = c;
    }

    public void NewLevel()
    {
        objectsFound = false;
        
        StartCoroutine(Tool.c_InvokeAfterWait(1f, FindObjects));
    }

    private void FindObjects()
    {
        Debug.Log("Init GameManager");
        character = FindObjectOfType<Character>();
        characterInitialPosition = character.transform.position;
        characterInitialRotation = character.transform.rotation;
        characterInitialScale = character.transform.localScale;

        if (GameObject.FindGameObjectWithTag("TextScreenPrint") != null)
        {
            printScreenTMP = GameObject.FindGameObjectWithTag("TextScreenPrint").GetComponent<TMP_Text>();
        }
        

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

    public void moveCharacter(String direction)
    {
        switch (direction) {
            case "Forward": character.Forward(); break;
            case "Behind": character.Behind(); break;
            case "Right": character.TurnRight(); break;
            case "Left": character.TurnLeft(); break;
        }

        eventsManager.characterMoving(direction);
    }

    public static void Reset()
    {
        if (character != null)
        {
            character.transform.position = characterInitialPosition;
            character.transform.rotation = characterInitialRotation;
            character.transform.localScale = characterInitialScale;
            character.ResetTheTargetPosition(character.transform.position, character.transform.rotation);
        }
        

        if (originalObstacles.Count > 0)
        {
            foreach (GameObject go in originalObstacles)
            {
                if (go != null)
                {
                    go.SetActive(true);
                    Rigidbody goRigidbody = go.GetComponent<Rigidbody>();
                    if (goRigidbody != null)
                    {
                        goRigidbody.detectCollisions = true;
                    }
                }
                
            }
            foreach (GameObject go in executionObstacles)
            {
                Destroy(go);
            }
        }
        

    }

    public void resetCar()
    {
        Reset();
    }

    public static void ReportError(MonoBehaviour obj, string message)
    {
        if (printScreenTMP != null)
        {
            printScreenTMP.text = "ERROR in " + obj.ToString() + " : " + message;
        }

        
        MainBlock.error = true;
        
        if(obj as Block)
        {
            obj.StartCoroutine(MakeBlockFlash((Block)obj, errorMaterial));
        }
        
    }

    public static void ReportWarning(MonoBehaviour obj, string message)
    {
        if (printScreenTMP != null)
        {
            printScreenTMP.text = "WARNING in " + obj.ToString() + " : " + message;
        }


        if (obj as Block)
        {
            obj.StartCoroutine(MakeBlockFlash((Block)obj, warningMaterial));
        }
    }

    static IEnumerator MakeBlockFlash(Block block, Material material)
    {
        for(int i = 0; i < 3; i++)
        {
            if (block != null && material != null)
            {
                block.ChangeBlockMaterial(material);
                yield return new WaitForSeconds(0.3f);
                block.InitBlockMaterial();
                yield return new WaitForSeconds(0.3f);
            }          
        }      
    }

    public static void DisplayOnPrompt(string message)
    {
        if (printScreenTMP != null)
        {
            printScreenTMP.text = message;
        }

    }

}
