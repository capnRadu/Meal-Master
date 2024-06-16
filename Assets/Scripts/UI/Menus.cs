using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private Image loadingBarFill;

    public void Play(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
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
}
