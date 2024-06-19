using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DifficultyManager;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private Image loadingBarFill;

    [SerializeField] private GameObject lockedMenu;

    public void Play(bool expertMode)
    {
        if (!expertMode || (expertMode && !DifficultyManager.Instance.expertLocked))
        {
            StartCoroutine(LoadSceneAsync(1));
        }
        else
        {
            lockedMenu.SetActive(true);
        }
    }

    public void Quit(bool deleteSaves)
    {
        if (deleteSaves)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Game data deleted");
        }
        else
        {
            SaveLoadManager.Instance.SaveGame();
        }

        Application.Quit();
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingMenu.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    public void SetDifficulty(int difficulty)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        switch (difficulty)
        {
            case 0:
                DifficultyManager.Instance.currentDifficulty = difficultyMode.Novice;

                parameters.Add("gameMode", DifficultyManager.Instance.currentDifficulty.ToString());
                AnalyticsService.Instance.CustomData("chooseGameMode", parameters);
                AnalyticsService.Instance.Flush();
                break;
            case 1:
                if (!DifficultyManager.Instance.expertLocked)
                {
                    DifficultyManager.Instance.currentDifficulty = difficultyMode.Expert;

                    parameters.Add("gameMode", DifficultyManager.Instance.currentDifficulty.ToString());
                    AnalyticsService.Instance.CustomData("chooseGameMode", parameters);
                    AnalyticsService.Instance.Flush();
                }
                break;
        }

        Debug.Log(DifficultyManager.Instance.currentDifficulty);
    }

    public void UpdateMoneyText()
    {
        Upgrades.Instance.UpdatePlayerMoneyText();
    }
}
