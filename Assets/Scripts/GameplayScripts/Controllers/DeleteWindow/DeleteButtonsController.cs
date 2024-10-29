using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonsController : MonoBehaviour
{
    public float deadTime = 1.0f;
   // private bool _deadTimeActive = false;
    private EventsManager eventsManager;
    private LevelManager levelManager;

    public void onTriggerEnterDelete()
    {
        var parentObject = transform.parent.parent.gameObject;
        parentObject.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();

        // Inicializar EventsManager y LevelManager si no están ya asignados
        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }

        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }

        // Sumar bloques en LevelManager si está asignado
        if (levelManager != null)
        {
            levelManager.sumNumberOfBlocks(parentObject);
        }

        // Notificar al EventsManager
        eventsManager.deleteBlock(parentObject);

        // Actualizar estantes (shelves)
        UpdateShelves(parentObject);
    }


    private void UpdateShelves(GameObject parentObject)
    {
        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        foreach (ShelfController shelf in shelves)
        {
            if (parentObject.name.Contains(shelf.blockPrefab.name))
            {
                if (levelManager != null && levelManager.returnNumberOfBlocks(parentObject) == 1)
                {
                    shelf.callCreateNewBlock();
                }
                shelf.actualiceText();
            }
        }
    }

    public void closeDeleteWindow()
    {
        var parentObject = transform.parent.parent.gameObject;

        gameObject.transform.parent.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();
        parentObject.GetComponent<CanBeDeleted>().openWindow = false;

        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }

        eventsManager.deleteWindowClose();
    }
}
