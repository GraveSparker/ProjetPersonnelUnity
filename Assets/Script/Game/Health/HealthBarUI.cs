using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;          // assign the child "fill" Image
    [SerializeField] private bool smooth = true;       // smooth transition
    [SerializeField] private float smoothSpeed = 6f;   // higher = faster

    private Coroutine lerpCoroutine;

    public void Initialize(int currentHealth, int maxHealth)
    {
        float ratio = (float)currentHealth / Mathf.Max(1, maxHealth);
        SetFillInstant(ratio);
    }

    public void SetFillInstant(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        if (lerpCoroutine != null) { StopCoroutine(lerpCoroutine); lerpCoroutine = null; }
        fillImage.fillAmount = normalized;
    }

    public void SetFill(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);

        if (!smooth)
        {
            SetFillInstant(normalized);
            return;
        }

        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpFill(normalized));
    }

    private IEnumerator LerpFill(float target)
    {
        while (!Mathf.Approximately(fillImage.fillAmount, target))
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, target, Time.deltaTime * smoothSpeed);
            yield return null;
        }
        fillImage.fillAmount = target;
        lerpCoroutine = null;
    }
}

