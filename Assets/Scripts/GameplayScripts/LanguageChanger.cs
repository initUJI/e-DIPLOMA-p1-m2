using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageChanger : MonoBehaviour
{
    public Sprite spanish;
    public Sprite english;
    public Image imageComponent;

    private const string LanguagePrefKey = "SelectedLanguage";  // Clave para PlayerPrefs

    void Start()
    {
        SetDefaultLanguage();

        // Actualizar el sprite del bot�n basado en el idioma actual
        UpdateButtonSprite();
    }

    public void SetDefaultLanguage()
    {
        // Si existe un idioma guardado en PlayerPrefs, lo utilizamos, si no, usamos el espa�ol por defecto
        string savedLanguage = PlayerPrefs.GetString(LanguagePrefKey, "es");
        StartCoroutine(SetLocale(savedLanguage));
    }

    // Cambiar el idioma y guardarlo en PlayerPrefs
    public void ChangeLanguage(string languageCode)
    {
        PlayerPrefs.SetString(LanguagePrefKey, languageCode);  // Guardar el idioma seleccionado
        PlayerPrefs.Save();  // Asegurar que se guarde
        StartCoroutine(SetLocale(languageCode));
    }

    // Corrutina para establecer el idioma
    private IEnumerator SetLocale(string localeIdentifier)
    {
        // Esperar a que el sistema de localizaci�n est� inicializado
        yield return LocalizationSettings.InitializationOperation;

        // Buscar el Locale correspondiente
        Locale selectedLocale = null;
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == localeIdentifier)
            {
                selectedLocale = locale;
                break;
            }
        }

        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
            Debug.Log("Idioma cambiado a: " + selectedLocale.LocaleName);
        }
        else
        {
            Debug.LogWarning("No se encontr� el idioma: " + localeIdentifier);
        }

        // Actualiza el sprite del bot�n despu�s de cambiar el idioma
        UpdateButtonSprite();
    }

    // Alternar entre idiomas (es/en)
    public void ToggleLanguage()
    {
        StartCoroutine(ToggleLanguageCoroutine());
    }

    private IEnumerator ToggleLanguageCoroutine()
    {
        // Espera a que el sistema de localizaci�n est� inicializado
        yield return LocalizationSettings.InitializationOperation;

        Locale currentLocale = LocalizationSettings.SelectedLocale;

        // Cambiar entre ingl�s y espa�ol
        if (currentLocale.Identifier.Code == "en")
        {
            ChangeLanguage("es");
        }
        else if (currentLocale.Identifier.Code == "es")
        {
            ChangeLanguage("en");
        }
    }

    // Obtener el idioma actual
    public string GetCurrentLanguage()
    {
        if (LocalizationSettings.InitializationOperation.IsDone)
        {
            Locale currentLocale = LocalizationSettings.SelectedLocale;
            return currentLocale != null ? currentLocale.Identifier.Code : "Idioma no seleccionado";
        }
        else
        {
            return "Inicializaci�n de localizaci�n a�n en proceso";
        }
    }

    // Actualizar el sprite del bot�n seg�n el idioma actual
    private void UpdateButtonSprite()
    {
        string currentLanguage = GetCurrentLanguage();

        if (currentLanguage == "es" && imageComponent != null)
        {
            imageComponent.sprite = spanish;
        }
        else if (currentLanguage == "en" && imageComponent != null)
        {
            imageComponent.sprite = english;
        }
    }

    private void Update()
    {
        // Revisa constantemente si el idioma ha cambiado para actualizar el bot�n
        UpdateButtonSprite();
    }
}


