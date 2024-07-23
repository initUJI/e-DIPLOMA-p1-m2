using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SingleObjectSocket : XRSocketInteractor
{
    protected override void Start()
    {
        base.Start();
        // Agregar listeners para los eventos de selección y deselección
        selectEntered.AddListener(OnSelectEntered);
        selectExited.AddListener(OnSelectExited);
    }

    protected override void OnDestroy()
    {
        if (this != null)
        {
            // Eliminar listeners cuando el objeto se destruya
            selectEntered.RemoveListener(OnSelectEntered);
            selectExited.RemoveListener(OnSelectExited);
        }
        base.OnDestroy();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Desactivar el socket para evitar que se seleccionen nuevos objetos
        socketActive = false;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Reactivar el socket cuando el objeto se retire
        socketActive = true;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (hasSelection)
        {
            // El socket ya tiene un objeto seleccionado, no se puede seleccionar otro
            return false;
        }

        GameObject currentBlock = interactable.transform.gameObject;

        Block newBlockComponent = currentBlock.GetComponent<Block>();
        if (newBlockComponent != null && newBlockComponent.bottomBlock != null)
        {
            if (newBlockComponent.bottomBlock == this.gameObject)
            {
                // Si el bottomBlock del nuevo bloque es este mismo socket, no permitir la selección
                return false;
            }
        }

        return base.CanSelect(interactable);
    }
}


