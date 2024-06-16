using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance { get; private set; }

    private int playerMoney = 0;
    public int PlayerMoney
    {
        get { return playerMoney; }
        set
        {
            playerMoney = value;
        }
    }

    private TextMeshProUGUI playerMoneyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var text = GameObject.FindWithTag("Player Money");

        if (text != null)
        {
            playerMoneyText = text.GetComponent<TextMeshProUGUI>();
            playerMoneyText.text = playerMoney.ToString();
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        StartCoroutine(TextAnimation(playerMoneyText, amount));
    }

    public IEnumerator TextAnimation(TextMeshProUGUI text, int amount, float scaleDuration = 0.2f, float scaleFactor = 1.5f)
    {
        int currentAmount = int.Parse(text.text);
        int targetAmount = currentAmount + amount;
        Vector3 originalScale = text.transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        yield return ScaleText(text, originalScale, targetScale, scaleDuration / 2);

        while (currentAmount < targetAmount)
        {
            currentAmount++;
            text.text = currentAmount.ToString();

            yield return new WaitForSeconds(0.01f);
        }

        yield return ScaleText(text, targetScale, originalScale, scaleDuration / 2);
    }

    private IEnumerator ScaleText(TextMeshProUGUI text, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = toScale;
    }
}
