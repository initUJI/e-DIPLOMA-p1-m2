using UnityEngine;
using System;

public class CommandLineArgs1_V2 : MonoBehaviour
{
    string[] args;

    void Start()
    {
        // Obtener los argumentos de la línea de comandos
        args = Environment.GetCommandLineArgs();
    }


    public string getFirstArgument()
    {
        string parameter = null;

        if (args.Length > 1)
        {
            parameter = args[1]; // Primer argumento pasado después del nombre del .exe
            Debug.Log($"First parameter: {parameter}");
            // Realiza acciones basadas en el argumento
        }

        return parameter;
    }
}

