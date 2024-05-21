using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageChanger : MonoBehaviour
{
    // Llama a esta función con el identificador del idioma que deseas establecer, por ejemplo, "en" para inglés, "es" para español, etc.
    public void ChangeLanguage(string languageCode)
    {
        StartCoroutine(SetLocale(languageCode));
    }

    private IEnumerator SetLocale(string localeIdentifier)
    {
        // Espera a que el sistema de localización esté inicializado
        yield return LocalizationSettings.InitializationOperation;

        // Busca el Locale correspondiente
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
            Debug.LogWarning("No se encontró el idioma: " + localeIdentifier);
        }
    }

    public string GetCurrentLanguage()
    {
        // Asegúrate de que el sistema de localización esté inicializado
        if (LocalizationSettings.InitializationOperation.IsDone)
        {
            Locale currentLocale = LocalizationSettings.SelectedLocale;
            return currentLocale != null ? currentLocale.Identifier.Code : "Idioma no seleccionado";
        }
        else
        {
            return "Inicialización de localización aún en proceso";
        }
    }

    public void ToggleLanguage()
    {
        StartCoroutine(ToggleLanguageCoroutine());
    }

    private IEnumerator ToggleLanguageCoroutine()
    {
        // Espera a que el sistema de localización esté inicializado
        yield return LocalizationSettings.InitializationOperation;

        Locale currentLocale = LocalizationSettings.SelectedLocale;

        // Cambia el idioma basado en el idioma actual
        if (currentLocale.Identifier.Code == "en")
        {
            SetLocale("es");
        }
        else if (currentLocale.Identifier.Code == "es")
        {
            SetLocale("en");
        }
    }
}

