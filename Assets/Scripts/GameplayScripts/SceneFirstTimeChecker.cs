using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFirstTimeChecker : MonoBehaviour
{
    private string sceneName;
    private bool isFirstTime;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        isFirstTime = IsFirstTimeOpeningScene(sceneName);

        if (isFirstTime)
        {
            //Debug.Log("Esta es la primera vez que se abre la escena '" + sceneName + "' en esta sesi�n.");
            // Ejecuta tu c�digo aqu�
        }
        else
        {
            //Debug.Log("La escena '" + sceneName + "' ya se ha abierto antes en esta sesi�n.");
        }
    }

    bool IsFirstTimeOpeningScene(string sceneName)
    {
        string key = "FirstTime_" + sceneName;
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}

