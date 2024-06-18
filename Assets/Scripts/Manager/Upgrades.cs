using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance { get; private set; }

    [NonSerialized] public int collectedMoney = 0;
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

    // Upgrades values
    public class Upgrade
    {
        public string name;
        public int level;
        public int upgradeAmount;
        public int upgradeCost;
    }

    public Upgrade Grill = new Upgrade
    {
        name = "Grill",
        level = 0,
        upgradeAmount = 0,
        upgradeCost = 100
    };

    public Upgrade Fryer = new Upgrade
    {
        name = "Fryer",
        level = 0,
        upgradeAmount = 0,
        upgradeCost = 80
    };

    public Upgrade Soda = new Upgrade
    {
        name = "Soda",
        level = 0,
        upgradeAmount = 0,
        upgradeCost = 50
    };

    public Upgrade Money = new Upgrade
    {
        name = "Money",
        level = 0,
        upgradeAmount = 0,
        upgradeCost = 350
    };

    public Upgrade Speed = new Upgrade
    {
        name = "Speed",
        level = 0,
        upgradeAmount = 10,
        upgradeCost = 120
    };

    [NonSerialized] public List<Upgrade> upgrades = new List<Upgrade>();

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

    private void Start()
    {
        if (upgrades.Count == 0)
        {
            upgrades.Add(Grill);
            upgrades.Add(Fryer);
            upgrades.Add(Soda);
            upgrades.Add(Money);
            upgrades.Add(Speed);
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
        UpdatePlayerMoneyText();
    }

    public void UpdatePlayerMoneyText()
    {
        var text = GameObject.FindWithTag("Player Money");

        if (text != null)
        {
            playerMoneyText = text.GetComponent<TextMeshProUGUI>();

            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                playerMoneyText.text = playerMoney.ToString();
            }
            else
            {
                collectedMoney = 0;
                playerMoneyText.text = collectedMoney.ToString();
            }
        }
    }

    public void AddMoney(int amount)
    {
        collectedMoney += amount;
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