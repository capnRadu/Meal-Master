using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAnimation : MonoBehaviour
{
    private Transform interactableTransform;
    private float animationDuration = 0.15f;
    private float scaleFactor = 0.8f;
    private bool isAnimating = false;

    void Start()
    {
        interactableTransform = GetComponent<Transform>();
    }

    void OnMouseDown()
    {
        if (!isAnimating && Time.timeScale != 0)
        {
            AnimateIngredient();
        }
    }

    public void AnimateIngredient()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        isAnimating = true;

        Vector3 originalScale = interactableTransform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            interactableTransform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        interactableTransform.localScale = targetScale;

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            interactableTransform.localScale = Vector3.Lerp(targetScale, originalScale, (elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        interactableTransform.localScale = originalScale;

        isAnimating = false;
    }
}