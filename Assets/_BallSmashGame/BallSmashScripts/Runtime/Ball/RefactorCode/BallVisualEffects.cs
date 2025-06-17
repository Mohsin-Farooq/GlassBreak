using System.Collections;
using UnityEngine;
public class BallVisualEffects : MonoBehaviour
{
    private Vector3 initialScale;
    public IEnumerator ScaleUp(float duration, Vector3 targetScale)
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
    public IEnumerator ScaleAndReturnToPool(float duration)
    {
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
    public void ResetVisuals()
    {
        transform.localScale = initialScale;
    }
}