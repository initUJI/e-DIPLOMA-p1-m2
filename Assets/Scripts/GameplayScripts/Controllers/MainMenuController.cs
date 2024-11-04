
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuController : MonoBehaviour
{
    public Button startButton;

    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject teleportArea;
    [SerializeField] private GameObject mainBlockPrefab;
    [SerializeField] private GameObject moveForwardPrefab;
    [SerializeField] private GameObject getHumidityPrefab;
    [SerializeField] private GameObject tutorialLevelPrefab;
    [SerializeField] private GameObject testStandPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject countDownObject;
    [SerializeField] private GameObject trashPrefab;
    [SerializeField] private GameObject endFor;
    [SerializeField] private GameObject endIf;
    [SerializeField] private GameObject ifBlock;
    [SerializeField] private GameObject forBlock;

    private GameObject MainOptions;
    private GameObject resetOptions;
    private GameObject TutorialOptions;
    private GameObject ControlsTutorial;
    private GameObject userOptions;
    [HideInInspector] public XRGrabInteractable grabInteractable;
    private GameObject cube;
    [HideInInspector] public GameObject platform;
    [HideInInspector] public GameObject mainBlock;
    [HideInInspector] public GameObject moveForwardBlock;
    [HideInInspector] public GameObject getHumidityBlock;
    [HideInInspector] public TextMeshPro userInputField;
    [HideInInspector] public bool teleportCompleted;
    [HideInInspector] public bool dynamicTutorialCompleted;
    private Vector3 platformLocation;
    private Vector3 block1Location;
    private Vector3 block2Location;
    private Vector3 block3Location;
    private Vector3 levelLocation;
    private Vector3 testStandLocation;
    private Vector3 trashLocation;
    private Vector3 endForLocation;
    private Vector3 endIfLocation;
    private Vector3 ifBlockLocation;
    private Vector3 forBlockLocation;
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted_";

    private EventsManager eventsManager;

    GameObject endfor = null;
    GameObject endif = null;
    GameObject forblock = null;
    GameObject ifblock = null;

    // Start is called before the first frame update
    void Start()
    {
        ClearManuallySelectedLevel();
        eventsManager = FindObjectOfType<EventsManager>();

        MainOptions = menuWindow.transform.GetChild(0).gameObject;
        TutorialOptions = menuWindow.transform.GetChild(1).gameObject;
        ControlsTutorial = menuWindow.transform.GetChild(2).gameObject;
        userOptions = menuWindow.transform.GetChild(3).gameObject;
        resetOptions = menuWindow.transform.GetChild(4).gameObject;
        countDownObject.SetActive(false);

        MainOptions.SetActive(true);
        TutorialOptions.SetActive(false);
        ControlsTutorial.SetActive(false);
        resetOptions.SetActive(false);

        teleportCompleted = false;
        dynamicTutorialCompleted= false;
        platformLocation = new Vector3(0.273f, 0.86f, 0.736f);
        block1Location = new Vector3(0.8f, 0.78f, -0.8f);
        block2Location = new Vector3(-0.3f, 0.8f, -0.82f);
        block3Location = new Vector3(0.13f, 1.0f, -0.92f);
        levelLocation = new Vector3(-0.233f, -0.46f, -0.746f);
        testStandLocation = new Vector3(0.5f, 0.35f, -0.68f);
        trashLocation = new Vector3(0.17f, 0.35f, 0.15f);
        endForLocation = new Vector3(0.76f, 0.87f, -0.73f);
        endIfLocation = new Vector3(0.76f, 1.05f, -0.73f);
        ifBlockLocation = new Vector3(-0.24f, 1.05f, -0.73f);
        forBlockLocation = new Vector3(0.41f, 1.05f, -0.73f);
    }

    private void Update()
    {
        if (IsTutorialCompleted("Controls") && IsTutorialCompleted("Dynamics"))
        {
            startButton.interactable = true;
        }
        else
        {
            //startButton.interactable = false;
        }
    }

    public void openResetOptions()
    {
        resetOptions.SetActive(true);
        MainOptions.SetActive(false);
    }

    public void closeResetOptions()
    {
        MainOptions.SetActive(true);
        resetOptions.SetActive(false);
    }

    public void ClearAllPlayerPrefs()
    {
        // Borra todos los PlayerPrefs
        PlayerPrefs.DeleteAll();
        closeResetOptions();
    }

    public void saveUserId()
    {
        string inputField = userInputField.text;
        PlayerPrefs.SetString("UserID", inputField);
        eventsManager.setUserID(inputField);
    }

    public void loadUserOptions() 
    {
        eventsManager.buttonClicked("USER OPTIONS");
        MainOptions.SetActive(false);
        userOptions.SetActive(true);
    }
    public void startGameplay()
    {
        eventsManager.buttonClicked("START");
        MainOptions.SetActive(false);
        countDownObject.SetActive(true);

        // Start the countdown before loading the new scene
        StartCoroutine(CountdownAndLoadScene(5)); // Replace 10 with the desired duration
    }

    private IEnumerator CountdownAndLoadScene(int duration)
    {
        // Get the TextMeshPro component from the second child of countDownObject
        TextMeshProUGUI countdownText = countDownObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        for (int i = duration; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Update the text with the countdown number
            yield return new WaitForSeconds(1); // Wait for 1 second
        }

        // After the countdown, change to the next scene
        SceneManager.LoadScene(1); // Change to your target scene index or name
    }

    public void StartGameplay(int levelToLoad)
    {
        // Save the selected level number in PlayerPrefs with a custom key
        PlayerPrefs.SetInt("ManuallySelectedLevel", levelToLoad);

        // Ensure the changes in PlayerPrefs are saved
        PlayerPrefs.Save();

        // Register the button click event
        eventsManager.buttonClicked("START");

        // Load the scene with the provided level number
        SceneManager.LoadScene(1);
    }

    public void ClearManuallySelectedLevel()
    {
        // Remove the "ManuallySelectedLevel" from PlayerPrefs
        PlayerPrefs.DeleteKey("ManuallySelectedLevel");

        // Ensure the changes in PlayerPrefs are saved
        PlayerPrefs.Save();
    }

    public void toggleOptionsFinish(GameObject gameObject)
    {
        // Toggle the active state of the GameObject
        if (gameObject != null)
        {
            bool isActive = gameObject.activeSelf; // Check current active state

            // Toggle the gameObject
            gameObject.SetActive(!isActive);

            // If the gameObject was active, deactivate MainOptions
            if (gameObject.activeInHierarchy && MainOptions != null)
            {
                MainOptions.SetActive(false);
            }
            else
            {
                MainOptions.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("GameObject is null");
        }
    }

    public void startTutorial()
    {
        eventsManager.buttonClicked("TUTORIAL");
        MainOptions.SetActive(false);
        TutorialOptions.SetActive(true);
    }

    public void startControlsTutorial()
    {
        eventsManager.buttonClicked("CONTROLS TUTORIAL");
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        cube = Instantiate(cubePrefab);
        grabInteractable = cube.GetComponent<XRGrabInteractable>();
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeTutorialInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeTutorialInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        DynamicTextUpdater dynamicTextUpdater = text.GetComponent<DynamicTextUpdater>();

        text.text = ins.getControlsString(1);
        dynamicTextUpdater.UpdateLocalizedString("controls1");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(1, grabInteractable));

        text.text = ins.getControlsString(2);
        dynamicTextUpdater.UpdateLocalizedString("controls2");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(2, grabInteractable));

        text.text = ins.getControlsString(3);
        platform = Instantiate(platformPrefab);
        dynamicTextUpdater.UpdateLocalizedString("controls3");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(3, grabInteractable, platform));
        yield return new WaitForSeconds(1);

        /*text.text = ins.getControlsString(4);
        xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
        xrOrigin.GetComponent<TeleportationProvider>().enabled = true;
        teleportationProvider.beginLocomotion += OnTeleportationStart;
        teleportationProvider.endLocomotion += OnTeleportationEnd;
        Destroy(cube);
        Destroy(platform);
        dynamicTextUpdater.UpdateLocalizedString("controls4");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(4, null, null, teleportCompleted));*/

        /*Destroy(cube);
        Destroy(platform);
        text.text = ins.getControlsString(5);
        platform = Instantiate(platformPrefab);
        platform.transform.position = platformLocation;
        cube = Instantiate(cubePrefab);
        dynamicTextUpdater.UpdateLocalizedString("controls5");
        yield return new WaitUntil(() => ins.checkControlsInstruccion(5, grabInteractable, platform));*/

        text.text = "Tutorial completed!";
        dynamicTextUpdater.UpdateLocalizedString("completedTutorial");
        SaveTutorialState("Controls", true);
    }

    public void startDynamicsTutorial()
    {
        eventsManager.buttonClicked("DYNAMICS TUTORIAL");
        ControlsTutorial.SetActive(true);
        TutorialOptions.SetActive(false);
        Instruccion instruccion = new Instruccion();
        StartCoroutine(InvokeDynamicsInstruccions(instruccion, ControlsTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    public IEnumerator InvokeDynamicsInstruccions(Instruccion ins, TextMeshProUGUI text)
    {
        DynamicTextUpdater dynamicTextUpdater = text.GetComponent<DynamicTextUpdater>();

        text.text = ins.getDynamicsString(1);
        mainBlock = Instantiate(mainBlockPrefab);
        moveForwardBlock = Instantiate(moveForwardPrefab);
        mainBlock.transform.position = block1Location;
        moveForwardBlock.transform.position = block2Location;
        dynamicTextUpdater.UpdateLocalizedString("dynamics1");

        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(1, mainBlock));
        yield return new WaitForSeconds(1);

        Destroy(mainBlock);
        GameObject trash = Instantiate(trashPrefab);
        trash.transform.position = trashLocation;
        text.text = ins.getDynamicsString(2);
        dynamicTextUpdater.UpdateLocalizedString("dynamics2");
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(2));

        Destroy(trash);
        GameObject testStand = Instantiate(testStandPrefab);
        Destroy(testStand.transform.GetChild(0).gameObject);
        mainBlock = Instantiate(mainBlockPrefab);
        mainBlock.transform.position = block1Location;
        moveForwardBlock = Instantiate(moveForwardPrefab);
        moveForwardBlock.transform.position = block2Location;
        getHumidityBlock = Instantiate(getHumidityPrefab);
        getHumidityBlock.transform.position = block3Location;
        GameObject tutorialLevel = Instantiate(tutorialLevelPrefab);
        GameObject.FindObjectOfType<GameManager>().NewLevel();
        text.text = ins.getDynamicsString(3);
        dynamicTextUpdater.UpdateLocalizedString("dynamics3");
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(3, mainBlock, dynamicTutorialCompleted));

        Destroy(moveForwardBlock);
        Destroy(getHumidityBlock);
        Destroy(mainBlock);
        Destroy(tutorialLevel);
        Destroy(testStand);

        tutorialLevel = Instantiate(tutorialLevelPrefab);
        testStand = Instantiate(testStandPrefab);
        testStand.transform.position = testStandLocation;
        testStand.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        testStand.AddComponent<ButtonManager>();
        text.text = ins.getDynamicsString(4);
        dynamicTextUpdater.UpdateLocalizedString("dynamics4");
        GameObject.FindObjectOfType<GameManager>().setCharacter(GameObject.FindObjectOfType<Character>());
        GameObject.FindObjectOfType<GameManager>().resetCar();
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(4, null, false, testStand));

        Destroy(tutorialLevel);
        Destroy(testStand);
        endfor = Instantiate(endFor);
        endfor.transform.position =endForLocation;
        endif = Instantiate(endIf);
        endif.transform.position = endIfLocation;
        forblock = Instantiate(forBlock);
        forblock.transform.position = forBlockLocation;
        ifblock = Instantiate(ifBlock);
        ifblock.transform.position = ifBlockLocation;
        text.text = ins.getDynamicsString(5);
        dynamicTextUpdater.UpdateLocalizedString("dynamics5");;
        yield return new WaitUntil(() => ins.checkDynamicsInstruccion(5, null, false, null, ifblock, forblock));

        text.text = "Tutorial completed!";
        dynamicTextUpdater.UpdateLocalizedString("completedTutorial");
        SaveTutorialState("Dynamics", true);
    }

    private void OnTeleportationStart(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportaci�n
        Debug.Log("Teleportaci�n empezada");
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        // Se llama cuando se completa la teletransportaci�n
        Debug.Log("Teleportaci�n completada");
        teleportCompleted = true;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void resetScene()
    {
        eventsManager.buttonClicked("RESET MAIN MENU (HOUSE ICON)");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void SaveTutorialState(string tutorialName, bool isCompleted)
    {
        int value = isCompleted ? 1 : 0;
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY + tutorialName, value);
        PlayerPrefs.Save();
        Debug.Log($"Tutorial '{tutorialName}' completado: {isCompleted}");
    }

    public bool LoadTutorialState(string tutorialName)
    {
        if (PlayerPrefs.HasKey(TUTORIAL_COMPLETED_KEY + tutorialName))
        {
            return PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY + tutorialName) == 1;
        }
        else
        {
            return false; // Si no hay un valor guardado, asumimos que no ha sido completado
        }
    }

    public bool IsTutorialCompleted(string tutorialName)
    {
        return LoadTutorialState(tutorialName);
    }
}

public class Instruccion : MainMenuController
{
    private string controls1 = "Aim at the cube with the controller.\r\n\r\nHold down the controller grip to move the cube.\r\n";
    private string controls2 = "Release the grip to release the cube.";
    private string controls3 = "Move the controller while holding the cube to place it on the platform.";
    //private string controls4 = "Aim at the platform floor and press the grip once to move to that location. You can also rotate the view with the left joystick.";
    private string controls5 = "Apply what you have learned to bring the cube to the new platform.";

    private string dynamics1 = "This is the �Main block�. It is in charge of executing all the instructions from top to bottom. " +
    "To place a block, simply grab it and pull it underneath the previous block.";
    private string dynamics2 = "To delete a block press the A button to display the delete window. To confirm " +
        "point the delete button with the controller and press the trigger.";
    private string dynamics3 = "The objective will be to drive the car to the plant to collect the humidity from it. "
        + "In this example, we will need a �Move forward� block to move the car forward one square and a �Get humidity� block to collect humidity.";
    private string dynamics4 = "This is the test stand, use it to help you visualise the path before you start programming. Press all the buttons to see what they do.";
    private string dynamics5 = "Some blocks cannot work on their own, they need an End to know where their function ends. Place each End where it corresponds according to its colour.";
    public string getDynamicsString(int num)
    {
        switch (num)
        {
            case 5: return dynamics5;
            case 4: return dynamics4;
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
           // case 4: return controls4;
            case 3: return controls3;
            case 2: return controls2;
            case 1: return controls1;
            default: return "";
        }
    }

    public bool checkDynamicsInstruccion(int num, GameObject mainBlock = null, bool dynamicTutorialCompleted = false, GameObject testStand = null,
        GameObject ifBlock = null, GameObject forBlock = null)
    {
        switch (num)
        {
            case 5: return checkDynamics5(ifBlock, forBlock);
            case 4: return checkDynamics4(testStand);
            case 3: return checkDynamics3(dynamicTutorialCompleted);
            case 2: return checkDynamics2();
            case 1: return checkDynamics1(mainBlock);
            default: return false;
        }
    }

    public bool checkControlsInstruccion(int num, XRGrabInteractable grab = null, GameObject platform = null, bool teleport = false)
    {
        switch (num)
        {
            case 5: return checkControls5(grab, platform);
            case 4: return checkControls4(teleport);
            case 3: return checkControls3(grab, platform);
            case 2: return checkControls2(grab);
            case 1: return checkControls1(grab);
            default: return false;
        }
    }

    public bool checkDynamics5(GameObject ifBlock, GameObject forBlock)
    {
        // Check if ifBlock or forBlock is null
        if (ifBlock == null || forBlock == null)
        {
            return false;
        }

        // Get XRSocketInteractor from each first child and check if they are valid
        XRSocketInteractor ifSocket = ifBlock.GetComponent<LastBottonSocketHolder>().xRSocketInteractor;
        XRSocketInteractor forSocket = forBlock.GetComponent<LastBottonSocketHolder>().xRSocketInteractor;

        if (ifSocket == null || forSocket == null)
        {
            return false;
        }

        // Check if the socket item names match "EndIf" and "EndFor"
        return GetSocketItemName(ifSocket).Contains("EndIf") && GetSocketItemName(forSocket).Contains("EndFor");
    }


    private string GetSocketItemName(XRSocketInteractor socketInteractor)
    {
        if (socketInteractor.hasSelection) // Comprobar si hay un objeto en el socket
        {
            var selectedObject = socketInteractor.firstInteractableSelected;
            return selectedObject.transform.name; // Devolver el nombre del objeto
        }
        else
        {
            return "Empty"; // Devolver "Empty" si no hay objeto
        }
    }

    public bool checkDynamics4(GameObject testStand)
    {
        return testStand.GetComponent<ButtonManager>().CheckAllButtonsPressed();
    }

    public bool checkDynamics3(bool dynamicTutorialCompleted)
    {
        return dynamicTutorialCompleted;
    }
    public bool checkDynamics2()
    {
        Block[] blocks = GameObject.FindObjectsOfType<Block>();
        if (blocks.Length <= 0)
        {
            return true;
        }
        return false;
    }

    public bool checkDynamics1(GameObject mainBlock)
    {
        if (mainBlock != null)
        {
            MainBlock mainBlockType = mainBlock.GetComponent<MainBlock>();

            if (mainBlockType.getBottonSocketControled() != null)
            {
                Block nextBlock = mainBlockType.getBottonSocketControled();
                if (nextBlock != null && nextBlock.GetType().ToString() == "ForwardBlock")
                {
                    return true;
                }
            }
            
        }
        return false;
    }

    public bool checkControls5(XRGrabInteractable grab, GameObject platform)
    {
        if (!grab.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding)
        {
            return true;
        }
        return false;
    }
    public bool checkControls4(bool teleport)
    {
        if (teleport)
        {
            return true;
        }
        return false;
    }

    public bool checkControls3(XRGrabInteractable grab, GameObject platform)
    {
        if ((!grab.isSelected && platform != null && platform.GetComponent<platformController>().cubeColliding) ||
            platform != null && platform.GetComponent<XRSocketInteractor>().hasSelection)
        {
            return true;
        }
        return false;
    }

    public bool checkControls1(XRGrabInteractable grab)
    {
        if (grab.isSelected)
        {
            return true;
        }
        return false;
    }
    public bool checkControls2(XRGrabInteractable grab)
    {
        if (grab.isSelected)
        {
            return false;
        }
        return true;
    }
}
