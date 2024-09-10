using UnityEngine;

public class CanvasBillboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        // Buscar la cámara principal en la escena
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main camera not found in the scene!");
        }
    }

    private void Update()
    {
        if (mainCameraTransform != null)
        {
            // Hacer que el canvas mire siempre hacia la cámara principal
            transform.LookAt(mainCameraTransform);

            // Opcional: Forzar el canvas a estar siempre en orientación horizontal
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = 0;
            eulerRotation.z = 0;
            transform.eulerAngles = eulerRotation;
        }
    }
}
