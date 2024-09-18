using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Collections;
using UnityEngine.XR;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    public float fpsThreshold = 10.0f; // Umbral mínimo de FPS
    public float restartDelay = 4.0f;  // Tiempo de espera antes de verificar si se debe reiniciar
    private bool isRestarting = false; // Para evitar reinicios múltiples
    public TextMeshProUGUI warningText; // Referencia al TextMeshPro para mostrar el aviso
    private string warningMessage;      // Mensaje de aviso según el idioma

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.antiAliasing = 0;
        Application.targetFrameRate = -1;
        QualitySettings.masterTextureLimit = 2; // Reduce la calidad de las texturas
        Time.fixedDeltaTime = 0.02f; // Disminuye la frecuencia de simulación física
        Application.targetFrameRate = 90; // Apunta a 90 FPS
        XRSettings.eyeTextureResolutionScale = 0.75f; // Escala al 75% de la resolución nativa
        QualitySettings.shadowDistance = 50f; // Reduce la distancia de renderizado de sombras

        // Detecta el idioma de la aplicación
        var selectedLocale = LocalizationSettings.SelectedLocale;
        var culture = selectedLocale.Identifier.Code;

        if (culture.StartsWith("es"))
        {
            warningMessage = "Rendimiento bajo detectado. La escena se reiniciará en breve...";
        }
        else
        {
            warningMessage = "Low performance detected. The scene will restart shortly...";
        }
    }

    void Update()
    {

        // Calcula el FPS actual
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Verifica si los FPS caen por debajo del umbral y no se está reiniciando ya
        if (fps < fpsThreshold && !isRestarting)
        {
            Debug.LogWarning("FPS ha caído por debajo de " + fpsThreshold + ". Esperando para verificar...");
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
        isRestarting = true;  // Marca que el proceso de verificación ha comenzado
        yield return new WaitForSeconds(restartDelay);  // Espera un tiempo antes de verificar

        // Recalcula el deltaTime y el FPS después de esperar
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
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