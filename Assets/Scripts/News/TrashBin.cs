using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private EventsManager eventsManager;
    private LevelManager levelManager;

    public float requiredTimeInTrigger = 1f; // Tiempo necesario para destruir el objeto
    private Dictionary<GameObject, Coroutine> objectsInTrigger = new Dictionary<GameObject, Coroutine>();
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        // Verifica si el objeto que entra en el trigger tiene el tag "Block"
        if (other.CompareTag("Block"))
        {
            // Destruye el objeto con el tag "Block"
            onTriggerEnterDeleteRubish(other.gameObject);
            Destroy(other.gameObject);
            Debug.Log("Objeto destruido en la papelera.");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene el tag "Block"
        if (other.CompareTag("Block"))
        {
            Debug.Log($"{other.name} ha entrado en el trigger.");

            // Inicia una coroutine para destruir el objeto después del tiempo dado
            Coroutine destructionCoroutine = StartCoroutine(DestroyAfterTime(other.gameObject));
            objectsInTrigger.Add(other.gameObject, destructionCoroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale del trigger tiene el tag "Block"
        if (other.CompareTag("Block") && objectsInTrigger.ContainsKey(other.gameObject))
        {
            Debug.Log($"{other.name} ha salido del trigger antes de ser destruido.");

            // Detiene la coroutine si el objeto sale antes de cumplir el tiempo requerido
            StopCoroutine(objectsInTrigger[other.gameObject]);
            objectsInTrigger.Remove(other.gameObject);
        }
    }

    private IEnumerator DestroyAfterTime(GameObject target)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(requiredTimeInTrigger);

        // Verifica si el objeto aún existe antes de destruirlo
        if (target != null)
        {
            onTriggerEnterDeleteRubish(target);
            Destroy(target);
            Debug.Log($"{target.name} fue destruido después de {requiredTimeInTrigger} segundos en el trigger.");
        }

        // Asegúrate de eliminar el objeto de la lista
        objectsInTrigger.Remove(target);
    }

    public void onTriggerEnterDeleteRubish(GameObject gameObject)
    {
        //gameObject.GetComponent<BouncyScaleScript>().SetNextScaleDown();
        Destroy(gameObject);

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
            levelManager.sumNumberOfBlocks(gameObject);
        }

        // Notificar al EventsManager
        eventsManager.deleteBlock(gameObject);

        // Actualizar estantes (shelves)
        UpdateShelves(gameObject);
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
}
