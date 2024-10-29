using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CenterWallController : MonoBehaviour
{
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private LevelManager levelManager;
    private Animator animator;

    [HideInInspector] public bool wallUP;

    private int actualLevel;
    private Image arrowDownImage;
    private Coroutine blinkCoroutine;
    private bool blinkActivatedOnce = false; // Nueva bandera para rastrear si ya se activó el parpadeo

    void Start()
    {
        animator = GetComponent<Animator>();
        arrowDownImage = arrowDown.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        arrowDown.SetActive(true);
        arrowUp.SetActive(false);
        wallUP = false;

        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        // Esperamos hasta que actualLevel sea mayor que 0
        while ((actualLevel = levelManager.getActualLevel()) <= 0)
        {
            yield return null; // Espera un frame y vuelve a comprobar
        }

        // Una vez inicializado, continuamos con la lógica según el nivel
        if (actualLevel == 1)
        {
            // Si estamos en el nivel 1 y el parpadeo no se ha activado previamente
            if (!blinkActivatedOnce)
            {
                StartCoroutine(ActivateBlinkEffectAfterDelay(30f));
            }
        }
        else
        {
            // Si estamos en otros niveles, bajamos la pared automáticamente
            centerWallDown();
        }
    }

    IEnumerator ActivateBlinkEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (arrowDown.activeInHierarchy)
        {
            // Iniciar el parpadeo
            blinkCoroutine = StartCoroutine(BlinkArrowDown());
            blinkActivatedOnce = true; // Marcar que el parpadeo ya se activó una vez
        }
    }

    IEnumerator BlinkArrowDown()
    {
        while (true)
        {
            // Alternar entre colores para crear el efecto de parpadeo
            arrowDownImage.color = Color.red;  // Color de parpadeo
            yield return new WaitForSeconds(0.5f);
            arrowDownImage.color = Color.white;  // Color original
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void centerWallDown()
    {
        // Detener el parpadeo si está activo
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            arrowDownImage.color = Color.white;  // Restaurar el color original
        }

        StartCoroutine(WaitForAnimation("CenterWallDown"));
    }

    public void centerWallUp()
    {
        StartCoroutine(WaitForAnimation("CenterWallUp"));
    }

    IEnumerator WaitForAnimation(string animation)
    {
        animator.Play(animation);
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animationState.length);

        // Cambiar el estado de las flechas según la animación
        switch (animation)
        {
            case "CenterWallDown":
                arrowUp.SetActive(true);
                arrowDown.SetActive(false);
                wallUP = false;
                break;
            case "CenterWallUp":
                arrowUp.SetActive(false);
                arrowDown.SetActive(true);
                wallUP = true;
                break;
            default:
                break;
        }
    }
}
