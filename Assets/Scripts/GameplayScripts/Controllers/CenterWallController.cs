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
    private bool blinkActivatedOnce = false;

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
        while ((actualLevel = levelManager.getActualLevel()) <= 0)
        {
            yield return null;
        }

        if (actualLevel == 1)
        {
            if (!blinkActivatedOnce)
            {
                StartCoroutine(ActivateBlinkEffectAfterDelay(30f));
            }
        }
        else
        {
            centerWallDown();
        }
    }

    IEnumerator ActivateBlinkEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (arrowDown.activeInHierarchy)
        {
            blinkCoroutine = StartCoroutine(BlinkArrowDown());
            blinkActivatedOnce = true;
        }
    }

    IEnumerator BlinkArrowDown()
    {
        while (true)
        {
            arrowDownImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            arrowDownImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void centerWallDown()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            arrowDownImage.color = Color.white;
        }

        StartCoroutine(WaitForAnimation("CenterWallDown"));
    }

    public void centerWallUp()
    {
        StartCoroutine(WaitForAnimation("CenterWallUp"));

        // Iniciar temporizador de 1 minuto para bajar la pared automáticamente
        StartCoroutine(AutoLowerWallAfterDelay(60f));
    }

    private IEnumerator AutoLowerWallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Si la pared sigue arriba después del retraso, se baja automáticamente
        if (wallUP)
        {
            centerWallDown();
        }
    }

    IEnumerator WaitForAnimation(string animation)
    {
        animator.Play(animation);
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animationState.length);

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
