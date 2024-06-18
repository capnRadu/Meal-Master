using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeStandard : MonoBehaviour
{
    public string upgradeName;
    private int level;
    private int maxLevel = 5;
    private int upgradeAmount;
    private int upgradeCost;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeAmountText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;

    [SerializeField] private AudioSource upgradeSfx;
    [SerializeField] private AudioSource upgradeMaxSfx;

    private void Start()
    {
        foreach (var upgrade in Upgrades.Instance.upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                level = upgrade.level;

                upgradeAmount = upgrade.upgradeAmount;
                upgradeCost = upgrade.upgradeCost;

                levelText.text = $"Level {level}/{maxLevel}";

                if (upgradeName == "Speed")
                {
                    upgradeAmountText.text = $"{upgradeAmount}";
                }
                else
                {
                    upgradeAmountText.text = $"+{upgradeAmount}%";
                }

                if (level == maxLevel)
                {
                    upgradeButtonText.text = "Maxed";
                }
                else
                {
                    upgradeButtonText.text = $"Upgrade ({upgradeCost})";
                }
            }
        }
    }

    public void BuyUpgrade()
    {
        if (level >= maxLevel || Upgrades.Instance.PlayerMoney < upgradeCost)
        {
            upgradeMaxSfx.Play();
            return;
        }

        level++;
        Upgrades.Instance.PlayerMoney -= upgradeCost;
        Upgrades.Instance.UpdatePlayerMoneyText();
        upgradeCost += (int) level * upgradeCost * 1/2;

        switch (upgradeName)
        {
            case "Grill":
            case "Fryer":
            case "Soda":
                upgradeAmount += 10;
                upgradeAmountText.text = $"+{upgradeAmount}%";
                break;
            case "Money":
                upgradeAmount += 20;
                upgradeAmountText.text = $"+{upgradeAmount}%";
                break;
            case "Speed":
                upgradeAmount += 2;
                upgradeAmountText.text = $"{upgradeAmount}";
                break;
        }

        levelText.text = $"Level {level}/{maxLevel}";
        upgradeButtonText.text = $"Upgrade ({upgradeCost})";

        foreach (var upgrade in Upgrades.Instance.upgrades)
        {
            if (upgrade.name == upgradeName)
            {
                upgrade.level = level;
                upgrade.upgradeAmount = upgradeAmount;
                upgrade.upgradeCost = upgradeCost;
            }
        }

        upgradeSfx.Play();

        if (level == maxLevel)
        {
           upgradeButtonText.text = "Maxed";
        }
    }
}