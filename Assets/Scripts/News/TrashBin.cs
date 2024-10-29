using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private EventsManager eventsManager;
    private LevelManager levelManager;
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene el tag "Block"
        if (other.CompareTag("Block"))
        {
            // Destruye el objeto con el tag "Block"
            onTriggerEnterDeleteRubish(other.gameObject);
            //Destroy(other.gameObject);
            //Debug.Log("Objeto destruido en la papelera.");
        }
    }

    public void onTriggerEnterDeleteRubish(GameObject gameObject)
    {
        //gameObject.GetComponent<BouncyScaleScript>().SetNextScaleDown();
        gameObject.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();

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
