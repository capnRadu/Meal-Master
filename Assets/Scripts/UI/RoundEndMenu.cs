using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundEndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundReachedText;
    [SerializeField] private TextMeshProUGUI gainedMoneyText;

    private void Start()
    {
        roundReachedText.text = $"Round Reached:\r\n{FindObjectOfType<GameManager>().round}";
        gainedMoneyText.text = $"Gained Money:\r\n{Upgrades.Instance.collectedMoney}";
    }
}
