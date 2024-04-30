using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Data;

public class EventsManager : MonoBehaviour
{
    private string userID;
    private string fileName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {


        // Crear un objeto anónimo con datos
        var data = new
        {
            Nombre = "Juan",
            Edad = 30,
            Ciudad = "Ciudad de Ejemplo"
        };

        // Convertir el objeto en una cadena JSON
        string jsonString = JsonConvert.SerializeObject(data);

        // Especificar la ruta del archivo JSON
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Guardar la cadena JSON en un archivo
        File.WriteAllText(filePath, jsonString);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUserID(string s)
    {
        userID = s;
        fileName = "data_" + userID + "_" + DateTime.Now.ToString();
    }
}

[Serializable]
public class Data
{
    private string id;
    private string dateTime;
    private string actualLevel;

    // Optional constructor to initialize the data
    public Data(string id, string dateTime, string actualLevel)
    {
        this.id = id;
        this.dateTime = dateTime;
        this.actualLevel = actualLevel;
    }
}
