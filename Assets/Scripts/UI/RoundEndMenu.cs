using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;

public class RoundEndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundReachedText;
    [SerializeField] private TextMeshProUGUI gainedMoneyText;

    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        roundReachedText.text = $"Round Reached:\r\n{FindObjectOfType<GameManager>().round}";
        gainedMoneyText.text = $"Gained Money:\r\n{Upgrades.Instance.collectedMoney}";

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("currentRound", gameManager.round);
        parameters.Add("collectedMoney", Upgrades.Instance.collectedMoney);
        parameters.Add("playerTotalMoney", Upgrades.Instance.PlayerMoney);
        AnalyticsService.Instance.CustomData("roundEnd", parameters);
        AnalyticsService.Instance.Flush();
    }
}
