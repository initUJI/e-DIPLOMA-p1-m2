using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCubicSpawner : MonoBehaviour
{
    [SerializeField] private Material cubeMaterial; // Material que se asignará al cubo
    private Collider[] childColliders; // Colliders de los hijos

    // Diccionario para rastrear los cubos creados por cada objeto con el tag "Block"
    private Dictionary<GameObject, GameObject> blockToCubeMap = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        // Obtener todos los colliders en los hijos del objeto
        childColliders = GetComponentsInChildren<Collider>();
        //Debug.Log("Child colliders obtained: " + childColliders.Length);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Verificar si el objeto que colisiona tiene la etiqueta "Block"
        //Debug.Log("Collision stay detected with object: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Block"))
        {
            //Debug.Log("Collision stay with Block tagged object: " + collision.gameObject.name);

            // Recorremos todos los colliders de los hijos para ver cuál ha colisionado
            foreach (Collider childCollider in childColliders)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    if (contact.thisCollider == childCollider)
                    {
                        //Debug.Log("Collider matched: " + childCollider.name);

                        // Verificamos si ya se ha creado un cubo para este objeto
                        if (!blockToCubeMap.ContainsKey(collision.gameObject))
                        {
                            // Crear el cubo en la posición y tamaño del collider hijo
                            CreateCubeAtCollider(childCollider, collision.gameObject);
                        }
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Verificar si el objeto que deja de colisionar tiene la etiqueta "Block"
        //Debug.Log("Collision exit detected with object: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Block"))
        {
            //Debug.Log("Collision exit with Block tagged object: " + collision.gameObject.name);

            // Si el cubo fue creado para este objeto, destruirlo
            if (blockToCubeMap.ContainsKey(collision.gameObject))
            {
                //Debug.Log("Destroying created cube.");
                Destroy(blockToCubeMap[collision.gameObject]);
                blockToCubeMap.Remove(collision.gameObject);
            }
        }
    }

    private void CreateCubeAtCollider(Collider collider, GameObject block)
    {
        //Debug.Log("Creating cube at collider: " + collider.name);

        // Crear un nuevo cubo en la posición y rotación del collider
        GameObject createdCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createdCube.transform.position = collider.bounds.center;
        createdCube.transform.rotation = collider.transform.rotation;

        // Ajustar el tamaño del cubo para que coincida con el tamaño del collider
        createdCube.transform.localScale = collider.bounds.size;

        // Asignar el material desde el Inspector
        if (cubeMaterial != null)
        {
            createdCube.GetComponent<Renderer>().material = cubeMaterial;
        }

        // Evitar que el cubo interfiera con la física (si es necesario)
        createdCube.GetComponent<Collider>().enabled = false;

        //Debug.Log("Cube created successfully.");

        // Guardar la referencia del cubo en el diccionario para este objeto "Block"
        blockToCubeMap[block] = createdCube;
    }
}
