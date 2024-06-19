using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }
    
    [NonSerialized] public bool userGaveConsent = false;
    [SerializeField] private GameObject consentMenu;

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

        if (!PlayerPrefs.HasKey("gameSaved"))
        {
            StartCoroutine(ShowConsentMenu());
        }
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        
        if (userGaveConsent)
        {
            AnalyticsService.Instance.StartDataCollection();
        }
    }

    private IEnumerator ShowConsentMenu()
    {
        yield return new WaitForSeconds(0.5f);
        consentMenu.SetActive(true);
    }
}
