using System.Collections;
using System.Collections.Generic;
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

    public void Quit()
    {
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
        switch (difficulty)
        {
            case 0:
                DifficultyManager.Instance.currentDifficulty = difficultyMode.Novice;
                break;
            case 1:
                DifficultyManager.Instance.currentDifficulty = difficultyMode.Expert;
                break;
        }

        Debug.Log(DifficultyManager.Instance.currentDifficulty);
    }
}
