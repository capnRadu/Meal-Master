using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapAnimation : MonoBehaviour
{
    [SerializeField] private GameObject tapCirclePrefab;
    private float animationDuration = 0.5f;
    private float initialScale = 0.8f;

    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPosition = Input.mousePosition;
            StartCoroutine(ShowTapCircle(tapPosition));
        }
    }

    IEnumerator ShowTapCircle(Vector2 position)
    {
        GameObject tapCircle = Instantiate(tapCirclePrefab, canvas.transform);
        tapCircle.transform.position = position;

        RectTransform rectTransform = tapCircle.GetComponent<RectTransform>();
        Image image = tapCircle.GetComponent<Image>();
        Color originalColor = image.color;

        rectTransform.localScale = Vector3.one * initialScale;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            float alpha = Mathf.Lerp(1f, 0f, t);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            float scale = Mathf.Lerp(initialScale, 0.3f, t);
            rectTransform.localScale = Vector3.one * scale;

            yield return null;
        }

        Destroy(tapCircle);
    }
}