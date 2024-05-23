using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro; // Aseg�rate de tener esta referencia si usas TextMeshPro

public class DynamicTextUpdater : MonoBehaviour
{
    public LocalizeStringEvent localizeStringEvent;

    // Puedes inyectar las dependencias de LocalizedString si deseas actualizar din�micamente
    private LocalizedString localizedString = new LocalizedString();

    void Start()
    {
        // Si necesitas inicializar algo
    }

    // M�todo para actualizar el texto din�micamente
    public void UpdateLocalizedString(string newKey)
    {
        // Cambia la clave del LocalizedString
        localizedString.TableReference = "TextsTable"; // Reemplaza con el nombre de tu tabla
        localizedString.TableEntryReference = newKey;

        // Asigna el LocalizedString al evento de localizaci�n
        localizeStringEvent.StringReference = localizedString;

        // Opcional: Forzar una actualizaci�n del evento
        //localizeStringEvent.OnEnable();
    }
}

