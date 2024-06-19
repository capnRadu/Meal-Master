using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded ad loaded.");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Rewarded ad failed to load with error: {error}, message: {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Rewarded ad show failed with error: {error}, message: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Rewarded ad show started.");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Rewarded ad clicked.");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.hatIndex = 2;

            foreach (GameObject hat in gameManager.chefHats)
            {
                hat.SetActive(true);
            }

            gameManager.adMenu.SetActive(false);
            gameManager.pauseButton.SetActive(true);
            gameManager.settingsButton.SetActive(true);
            gameManager.pauseButton.GetComponent<PauseButton>().PauseGame(false);
        }
    }

    private void Awake()
    {
        #if UNITY_IOS
            adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
        #endif
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }
}
