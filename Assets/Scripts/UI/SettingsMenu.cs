using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject postProcessingVolume;
    [SerializeField] private Button ultraButton;
    [SerializeField] private Button veryHighButton;
    [SerializeField] private Button highButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button lowButton;
    [SerializeField] private Button veryLowButton;
    [SerializeField] private Toggle effectsDisabledToggle;

    private void Awake()
    {
        effectsDisabledToggle.SetIsOnWithoutNotify(postProcessingVolume.activeSelf);

        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                veryLowButton.interactable = false;
                break;
            case 1:
                lowButton.interactable = false;
                break;
            case 2:
                mediumButton.interactable = false;
                break;
            case 3:
                highButton.interactable = false;
                break;
            case 4:
                veryHighButton.interactable = false;
                break;
            case 5:
                ultraButton.interactable = false;
                break;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        foreach (Button button in new Button[] { veryLowButton, lowButton, mediumButton, highButton, veryHighButton, ultraButton })
        {
            button.interactable = true;
        }

        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                veryLowButton.interactable = false;
                break;
            case 1:
                lowButton.interactable = false;
                break;
            case 2:
                mediumButton.interactable = false;
                break;
            case 3:
                highButton.interactable = false;
                break;
            case 4:
                veryHighButton.interactable = false;
                break;
            case 5:
                ultraButton.interactable = false;
                break;
        }
    }

    public void DisableEffects()
    {
        postProcessingVolume.SetActive(!postProcessingVolume.activeSelf);
        DifficultyManager.Instance.effectsEnabled = postProcessingVolume.activeSelf;
    }
}
