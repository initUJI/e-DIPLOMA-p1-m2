using LevelGenerator.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject teleportArea;
    [SerializeField] private GameObject mainBlockPrefab;
    [SerializeField] private GameObject moveForwardPrefab;
    [SerializeField] private GameObject getHumidityPrefab;
    [SerializeField] private GameObject tutorialLevelPrefab;

    private GameObject MainOptions;
    private GameObject TutorialOptions;
    private GameObject ControlsTutorial;
    [HideInInspector] public XRGrabInteractable grabInteractable;
    private GameObject cube;
    [HideInInspector] public GameObject platform;
    [HideInInspector] public GameObject mainBlock;
    [HideInInspector] public GameObject moveForwardBlock;
    [HideInInspector] public GameObject getHumidityBlock;
    [HideInInspector] public bool teleportCompleted;
    [HideInInspector] public bool dynamicTutorialCompleted;
    private Vector3 platformLocation;
    private Vector3 block1Location;
    private Vector3 block2Location;
    private Vector3 block3Location;
    private Vector3 levelLocation;

    // Start is called before the first frame update
    void Start()
    {
        MainOptions = menuWindow.transform.GetChild(0).gameObject;
        TutorialOptions = menuWindow.transform.GetChild(1).gameObject;
        ControlsTutorial = menuWindow.transform.GetChild(2).gameObject;

        MainOptions.SetActive(true);
        TutorialOptions.SetActive(false);
        ControlsTutorial.SetActive(false);

        teleportCompleted = false;
        dynamicTutorialCompleted= false;
        platformLocation = new Vector3(0.273f, 0.86f, 0.736f);
        block1Location = new Vector3(0.65f, 0.78f, -0.93f);
        block2Location = new Vector3(-0.3f, 0.8f, -0.82f);
        block3Location = new Vector3(0.13f, 1.0f, -0.92f);
        levelLocation = new Vector3(-0.233f, -0.46f, -0.746f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGameplay()
    {
        SceneManager.LoadScene(1);
    }

    public void startTutorial()
    {
        MainOptions.SetActive(false);
        TutorialOptions.SetActive(true);
    }

    public void startControlsTutorial()
    {
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        cube = Instantiate(cubePrefab);
        grabInteractable = cube.GetComponent<XRGrabInteractable>();
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeTutorialInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeTutorialInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        text.text = ins.getControlsString(1);
        yield return new WaitUntil(() => ins.checkControlsInstruccion(1));

        text.text = ins.getControlsString(2);
        yield return new WaitUntil(() => ins.checkControlsInstruccion(2));

        text.text = ins.getControlsString(3);
        platform = Instantiate(platformPrefab);
        yield return new WaitUntil(() => ins.checkControlsInstruccion(3));

        text.text = ins.getControlsString(4);
        xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
        xrOrigin.GetComponent<TeleportationProvider>().enabled = true;
        teleportationProvider.beginLocomotion += OnTeleportationStart;
        teleportationProvider.endLocomotion += OnTeleportationEnd;
        Destroy(cube);
        Destroy(platform);
        yield return new WaitUntil(() => ins.checkControlsInstruccion(4));

        text.text = ins.getControlsString(5);
        platform = Instantiate(platformPrefab);
        platform.transform.position = platformLocation;
        yield return new WaitUntil(() => ins.checkControlsInstruccion(5));

        text.text = "Tutorial completed!";
    }

    public void startDynamicsTutorial()
    {
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        grabInteractable = cube.GetComponent<XRGrabInteractable>();
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeDynamicsInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeDynamicsInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        text.text = ins.getDynamicsString(1);
        mainBlock = Instantiate(mainBlockPrefab);
        moveForwardBlock = Instantiate(moveForwardPrefab);
        mainBlock.transform.position = block1Location;
        moveForwardBlock.transform.position = block2Location;
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(1));

        Destroy(moveForwardBlock);
        text.text = ins.getDynamicsString(2);
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(2));

        mainBlock.transform.position = block1Location;
        moveForwardBlock = Instantiate(moveForwardPrefab);
        moveForwardBlock.transform.position = block2Location;
        getHumidityBlock = Instantiate(getHumidityPrefab);
        getHumidityBlock.transform.position = block3Location;
        Instantiate(tutorialLevelPrefab);
        GameObject.FindObjectOfType<GameManager>().NewLevel();
        text.text = ins.getDynamicsString(3);
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(3));

        text.text = "Tutorial completed!";
    }

    private void OnTeleportationStart(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportación
        Debug.Log("Teleportación empezada");
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportación
        Debug.Log("Teleportación completada");
        teleportCompleted = true;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void resetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

public class Instruccion : MainMenuController
{
    private string controls1 = "Aim at the cube and hold down the controller grip to pick it up.";
    private string controls2 = "Release the grip to release the cube.";
    private string controls3 = "Move the controller while holding the cube to place it on the platform.";
    private string controls4 = "Aim at the platform floor and press the grip once to move to that location. You can also rotate the view with the left joystick.";
    private string controls5 = "Apply what you have learned to bring the cube to the new platform.";

    private string dynamics1 = "This is the ‘Main block’. It is in charge of executing all the instructions from top to bottom. " +
    "To place a block, simply grab it and pull it underneath the previous block.";
    private string dynamics2 = "To delete a block press the A button to display the delete window. To confirm, keep the A button pressed, " +
        "point the delete button with the controller and press the trigger.";
    private string dynamics3 = "The objective will be to drive the car to the plant to collect the humidity from it. "
        + "In this example, we will need a ‘Move forward’ block to move the car forward one square and a ‘Get humidity’ block to collect humidity.";

    public string getDynamicsString(int num)
    {
        switch (num)
        {
            case 3: return dynamics3;
            case 2: return dynamics2;
            case 1: return dynamics1;
            default: return "";
        }
    }

    public string getControlsString(int num)
    {
        switch (num)
        {
            case 5: return controls5;
            case 4: return controls4;
            case 3: return controls3;
            case 2: return controls2;
            case 1: return controls1;
            default: return "";
        }
    }

    public bool checkDynamicsInstruccion(int num)
    {
        switch (num)
        {
            case 3: return checkDynamics3();
            case 2: return checkDynamics2();
            case 1: return checkDynamics1();
            default: return false;
        }
    }

    public bool checkControlsInstruccion(int num)
    {
        switch (num)
        {
            case 5: return checkControls5();
            case 4: return checkControls4();
            case 3: return checkControls3();
            case 2: return checkControls2();
            case 1: return checkControls1();
            default: return false;
        }
    }

    public bool checkDynamics3()
    {
        return dynamicTutorialCompleted;
    }
    public bool checkDynamics2()
    {
        Block[] blocks = GameObject.FindObjectsOfType<Block>();
        if (blocks.IsEmpty())
        {
            return true;
        }
        return false;
    }

    public bool checkDynamics1()
    {
        if (mainBlock != null)
        {
            MainBlock mainBlockType = mainBlock.GetComponent<MainBlock>();
            Block nextBlock = mainBlockType.currentBlock.getSocketBlock(((WithBottomSocket)mainBlockType.currentBlock).getBottomSocket());
            if (nextBlock.GetType().ToString() == "MoveForward")
            {
                return true;
            }
        }
        return false;
    }

    public bool checkControls5()
    {
        if (!grabInteractable.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding)
        {
            return true;
        }
        return false;
    }
    public bool checkControls4()
    {
        if (teleportCompleted)
        {
            return true;
        }
        return false;
    }

    public bool checkControls3()
    {
        if (!grabInteractable.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding)
        {
            return true;
        }
        return false;
    }

    public bool checkControls1()
    {
        if (grabInteractable.isSelected)
        {
            return true;
        }
        return false;
    }
    public bool checkControls2()
    {
        if (grabInteractable.isSelected)
        {
            return false;
        }
        return true;
    }
}
