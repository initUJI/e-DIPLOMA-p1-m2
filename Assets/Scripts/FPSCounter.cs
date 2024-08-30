using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    public float fpsThreshold = 10.0f; // Umbral m�nimo de FPS
    public float restartDelay = 4.0f;  // Tiempo de espera antes de verificar si se debe reiniciar
    private bool isRestarting = false; // Para evitar reinicios m�ltiples
    public TextMeshProUGUI warningText; // Referencia al TextMeshPro para mostrar el aviso
    private string warningMessage;      // Mensaje de aviso seg�n el idioma

    void Start()
    {
        // Detecta el idioma de la aplicaci�n
        var selectedLocale = LocalizationSettings.SelectedLocale;
        var culture = selectedLocale.Identifier.Code;

        if (culture.StartsWith("es"))
        {
            warningMessage = "Rendimiento bajo detectado. La escena se reiniciar� en breve...";
        }
        else
        {
            // Por defecto, se usa ingl�s
            warningMessage = "Low performance detected. The scene will restart shortly...";
        }
    }

    void Update()
    {
        // Calcula el FPS actual
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Verifica si los FPS caen por debajo del umbral y no se est� reiniciando ya
        if (fps < fpsThreshold && !isRestarting)
        {
            Debug.LogWarning("FPS ha ca�do por debajo de " + fpsThreshold + ". Esperando para verificar...");
            if (warningText != null)
            {
                warningText.text = warningMessage; // Asigna el mensaje en el idioma correspondiente
                warningText.gameObject.SetActive(true); // Activa el aviso
            }
            StartCoroutine(CheckAndRestartScene());  // Inicia la corrutina para verificar y reiniciar la escena
        }
    }

    // Corrutina para verificar y reiniciar la escena si los FPS no se recuperan
    private IEnumerator CheckAndRestartScene()
    {
        isRestarting = true;  // Marca que el proceso de verificaci�n ha comenzado
        yield return new WaitForSeconds(restartDelay);  // Espera un tiempo antes de verificar

        // Recalcula el FPS despu�s de esperar
        float fps = 1.0f / deltaTime;

        // Si los FPS siguen por debajo del umbral, reinicia la escena
        if (fps < fpsThreshold)
        {
            Debug.LogWarning("FPS no se han recuperado. Reiniciando escena...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("FPS se han recuperado. No se reinicia la escena.");
            isRestarting = false;  // Restablece el flag ya que no es necesario reiniciar
            if (warningText != null)
            {
                warningText.gameObject.SetActive(false); // Oculta el aviso si no se necesita reiniciar
            }
        }
    }
}
