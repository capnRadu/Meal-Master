using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class ConsentMenu : MonoBehaviour
{
    [SerializeField] private Button optInButton;
    [SerializeField] private Button deleteDataButton;
    [SerializeField] private Button optOutButton;

    private void OnEnable()
    {
        switch (AnalyticsManager.Instance.userGaveConsent)
        {
            case true:
                optInButton.interactable = false;
                optOutButton.interactable = true;
                deleteDataButton.interactable = true;
                break;
            case false:
                optInButton.interactable = true;
                optOutButton.interactable = false;
                deleteDataButton.interactable = false;
                break;
        }
    }

    public void OptIn()
    {
        AnalyticsManager.Instance.userGaveConsent = true;
        AnalyticsService.Instance.StartDataCollection();
        optInButton.interactable = false;
        optOutButton.interactable = true;
        deleteDataButton.interactable = true;
        Debug.Log("Analytics data collection started");
    }

    public void OptOut()
    {
        AnalyticsManager.Instance.userGaveConsent = false;
        AnalyticsService.Instance.StopDataCollection();
        optInButton.interactable = true;
        optOutButton.interactable = false;
        deleteDataButton.interactable = false;
        Debug.Log("Analytics data collection stopped");
    }

    public void DeleteData()
    {
        AnalyticsManager.Instance.userGaveConsent = false;
        AnalyticsService.Instance.RequestDataDeletion();
        optInButton.interactable = true;
        optOutButton.interactable = false;
        deleteDataButton.interactable = false;
        Debug.Log("Analytics data deleted and data collection stopped");
    }
}