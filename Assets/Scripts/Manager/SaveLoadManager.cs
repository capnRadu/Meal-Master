using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

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

        LoadGameDataConsent();
    }

    private void Start()
    {
        LoadGameRest();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("gameSaved", 1);

        PlayerPrefs.SetInt("dataConsent", AnalyticsManager.Instance.userGaveConsent ? 1 : 0);
        PlayerPrefs.SetInt("expertModeLocked", DifficultyManager.Instance.expertLocked ? 1 : 0);
        PlayerPrefs.SetInt("totalPlayerMoney", Upgrades.Instance.PlayerMoney);

        PlayerPrefs.SetInt("grillUpgradeLevel", Upgrades.Instance.Grill.level);
        PlayerPrefs.SetInt("grillUpgradeAmount", Upgrades.Instance.Grill.upgradeAmount);
        PlayerPrefs.SetInt("grillUpgradeCost", Upgrades.Instance.Grill.upgradeCost);

        PlayerPrefs.SetInt("fryerUpgradeLevel", Upgrades.Instance.Fryer.level);
        PlayerPrefs.SetInt("fryerUpgradeAmount", Upgrades.Instance.Fryer.upgradeAmount);
        PlayerPrefs.SetInt("fryerUpgradeCost", Upgrades.Instance.Fryer.upgradeCost);

        PlayerPrefs.SetInt("sodaUpgradeLevel", Upgrades.Instance.Soda.level);
        PlayerPrefs.SetInt("sodaUpgradeAmount", Upgrades.Instance.Soda.upgradeAmount);
        PlayerPrefs.SetInt("sodaUpgradeCost", Upgrades.Instance.Soda.upgradeCost);

        PlayerPrefs.SetInt("moneyUpgradeLevel", Upgrades.Instance.Money.level);
        PlayerPrefs.SetInt("moneyUpgradeAmount", Upgrades.Instance.Money.upgradeAmount);
        PlayerPrefs.SetInt("moneyUpgradeCost", Upgrades.Instance.Money.upgradeCost);

        PlayerPrefs.SetInt("speedUpgradeLevel", Upgrades.Instance.Speed.level);
        PlayerPrefs.SetInt("speedUpgradeAmount", Upgrades.Instance.Speed.upgradeAmount);
        PlayerPrefs.SetInt("speedUpgradeCost", Upgrades.Instance.Speed.upgradeCost);

        PlayerPrefs.SetInt("effectsEnabled", DifficultyManager.Instance.effectsEnabled ? 1 : 0);
        PlayerPrefs.SetInt("qualityLevel", QualitySettings.GetQualityLevel());

        Debug.Log("Game saved");
    }

    private void LoadGameDataConsent()
    {
        if (PlayerPrefs.HasKey("gameSaved"))
        {
            bool dataConsent = PlayerPrefs.GetInt("dataConsent") == 1 ? true : false;
            AnalyticsManager.Instance.userGaveConsent = dataConsent;
        }
    }

    private void LoadGameRest()
    {
        if (PlayerPrefs.HasKey("gameSaved"))
        {
            bool expertMode = PlayerPrefs.GetInt("expertModeLocked") == 1 ? true : false;
            DifficultyManager.Instance.expertLocked = expertMode;

            Upgrades.Instance.PlayerMoney = PlayerPrefs.GetInt("totalPlayerMoney");

            Upgrades.Instance.Grill.level = PlayerPrefs.GetInt("grillUpgradeLevel");
            Upgrades.Instance.Grill.upgradeAmount = PlayerPrefs.GetInt("grillUpgradeAmount");
            Upgrades.Instance.Grill.upgradeCost = PlayerPrefs.GetInt("grillUpgradeCost");

            Upgrades.Instance.Fryer.level = PlayerPrefs.GetInt("fryerUpgradeLevel");
            Upgrades.Instance.Fryer.upgradeAmount = PlayerPrefs.GetInt("fryerUpgradeAmount");
            Upgrades.Instance.Fryer.upgradeCost = PlayerPrefs.GetInt("fryerUpgradeCost");

            Upgrades.Instance.Soda.level = PlayerPrefs.GetInt("sodaUpgradeLevel");
            Upgrades.Instance.Soda.upgradeAmount = PlayerPrefs.GetInt("sodaUpgradeAmount");
            Upgrades.Instance.Soda.upgradeCost = PlayerPrefs.GetInt("sodaUpgradeCost");

            Upgrades.Instance.Money.level = PlayerPrefs.GetInt("moneyUpgradeLevel");
            Upgrades.Instance.Money.upgradeAmount = PlayerPrefs.GetInt("moneyUpgradeAmount");
            Upgrades.Instance.Money.upgradeCost = PlayerPrefs.GetInt("moneyUpgradeCost");

            Upgrades.Instance.Speed.level = PlayerPrefs.GetInt("speedUpgradeLevel");
            Upgrades.Instance.Speed.upgradeAmount = PlayerPrefs.GetInt("speedUpgradeAmount");
            Upgrades.Instance.Speed.upgradeCost = PlayerPrefs.GetInt("speedUpgradeCost");

            bool postProcessingEffectsEnabled = PlayerPrefs.GetInt("effectsEnabled") == 1 ? true : false;
            DifficultyManager.Instance.effectsEnabled = postProcessingEffectsEnabled;

            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel"));

            Debug.Log("Game loaded");
        }
    }
}
