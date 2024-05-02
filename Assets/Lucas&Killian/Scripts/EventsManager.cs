using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
using static System.IO.StreamWriter;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Net.Sockets;

public class EventsManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    private XRGrabInteractable[] grabInteractables;

    private string userID;
    private string fileName = "";
    private LevelManager levelManager;
    private string filePath;
    private TextMeshProUGUI errorText;
    private GameObject canvas;
    private float timer = 0;
    private float timeShowingError = 10f;
    private XRSocketInteractor socket;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //Faltan eventos: evento se da al boton de abandonar nivel,
    //evento se confirma abandonar nivel

    //Falta funcionalidad: poder abandonar nivel

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        errorText = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        errorText.text = string.Empty;
        grabInteractables = FindObjectsOfType<XRGrabInteractable>();

        Data data = new Data("userID", "DateTime", "actualLevel", "action");
        writeInJson(data);
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        TeleportationProvider teleportationProvider = xrOrigin.GetComponent<TeleportationProvider>();
        teleportationProvider.endLocomotion += OnTeleportationEnd;
    }

    public void playPressed()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "BUTTON PLAY PRESSED");
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "BUTTON PLAY PRESSED");
        }

        writeInJson(data);
    }

    public void levelCompleted(int num)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "LEVEL COMPLETED: " + num.ToString());
            writeInJson(data);
        }
    }

    public void sceneCleaned()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "SCENE CLEANED, ALL BLOCKS RESETED");
            writeInJson(data);
        }
    }

    public void clueUsed(string clue)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "USED CLUE: " + clue);
            writeInJson(data);
        }
    }

    public void testStandUsed(string direction)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "PRESSED TEST STAND: " + direction);
            writeInJson(data);
        }
    }

    public void characterMoving(string direction)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "CHARACTER START MOVING: " + direction);
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "CHARACTER START MOVING: " + direction);
        }

        writeInJson(data);
    }

    public void deleteWindowOpen()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "DELETE BLOCK WINDOW OPEN");
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "DELETE BLOCK WINDOW OPEN");
        }

        writeInJson(data);
    }

    public void deleteWindowClose()
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "DELETE BLOCK WINDOW CLOSE");
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "DELETE BLOCK WINDOW CLOSE");
        }

        writeInJson(data);
    }

    public void deleteBlock(GameObject block)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "DELETED BLOCK: " + block.name);
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(),"MAIN MENU", "DELETED BLOCK: " + block.name);
        }

        writeInJson(data);
    }


    public void subscribeGrabEvents(XRGrabInteractable grab)
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    public void subscribeSocketsEvents(XRSocketInteractor xRSocket)
    {
        socket = xRSocket;
        xRSocket.selectEntered.AddListener(OnSocketEntered);
        xRSocket.selectExited.AddListener(OnSocketExit);
    }

    private void OnSocketEntered(SelectEnterEventArgs interactor)
    {
        Debug.Log("Objeto cogido");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(),
                "OBJECT " + interactor.interactableObject.transform.gameObject.name + 
                " IN " + socket.gameObject.name + " FROM " + socket.gameObject.transform.root.gameObject.name);
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU",
                 "OBJECT " + interactor.interactableObject.transform.gameObject.name +
                " IN " + socket.gameObject.name + " FROM " + socket.gameObject.transform.root.gameObject.name);
        }

        writeInJson(data);
    }

    // Método llamado cuando se suelta el objeto
    private void OnSocketExit(SelectExitEventArgs interactor)
    {
        Debug.Log("Objeto soltado");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(),
                "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(),
                "MAIN MENU", "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        writeInJson(data);
    }

    private void OnGrabbed(SelectEnterEventArgs interactor)
    {
        Debug.Log("Objeto cogido");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), 
                "OBJECT GRABBED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", 
                "OBJECT GRABBED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        writeInJson(data);
    }

    // Método llamado cuando se suelta el objeto
    private void OnReleased(SelectExitEventArgs interactor)
    {
        Debug.Log("Objeto soltado");

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), 
                "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), 
                "MAIN MENU", "OBJECT RELEASED: " + interactor.interactableObject.transform.gameObject.name.ToString());
        }

        writeInJson(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.activeInHierarchy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeShowingError)
        {
            canvas.SetActive(false);
            errorText.text = string.Empty;
            timer = 0;
        }
    }

    private void writeInJson(Data data) 
    {
        try
        {
            if (filePath != null && filePath.Length > 0)
            {
                string jsonString = JsonUtility.ToJson(data);
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine(jsonString);

                writer.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al escribir en el archivo JSON con Path " +filePath+ ": " + ex.Message);
        }
    }

    private void errorMessage(string error)
    {
        canvas.SetActive(true);
        errorText.text = error;
    }

    private void tryFindingLevelManager()
    {
        if (levelManager == null && FindObjectOfType<LevelManager>() != null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }
    }

    private void OnTeleportationEnd(LocomotionSystem locomotionSystem)
    {
        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "TELEPORTATION");
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "TELEPORTATION");
        }

        writeInJson(data);
    }

    public void setUserID(string s)
    {
        userID = s;
        fileName = "data_" + userID + "_" + DateTime.Now.ToString();
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        Data data;

        tryFindingLevelManager();

        if (levelManager != null)
        {
            data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "USER REGISTERED");
        }
        else
        {
            data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "USER REGISTERED");
        } 

        writeInJson(data);
    }

    public void buttonClicked(string button)
    {
        try
        {
            Data data;

            tryFindingLevelManager();

            if (levelManager != null)
            {
                data = new Data(userID, DateTime.Now.ToString(), levelManager.getActualLevel().ToString(), "PRESSED BUTTON: " + button);
            }
            else
            {
                data = new Data(userID, DateTime.Now.ToString(), "MAIN MENU", "PRESSED BUTTON: " + button);
            }

            writeInJson(data);
        }
        catch 
        {
            errorMessage("Error trying to write in Json");
        }
    }
}

[Serializable]
public class Data
{
    private string id;
    private string dateTime;
    private string actualLevel;
    private string action;

    // Optional constructor to initialize the data
    public Data(string id, string dateTime, string actualLevel, string action)
    {
        this.id = id;
        this.dateTime = dateTime;
        this.actualLevel = actualLevel;
        this.action = action;
    }
}
