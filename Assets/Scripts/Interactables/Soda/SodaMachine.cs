using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodaMachine : BasicIngredient
{
    private float timeToCook = 4;
    private float timeSpent = 0;
    [NonSerialized] public bool isRefilled = true;

    [SerializeField] private GameObject timerBarPrefab;
    [NonSerialized] public GameObject timerBar;

    [SerializeField] private GameObject sodaProp;

    private void Update()
    {
        if (!isRefilled)
        {
            if (timerBar == null)
            {
                timerBar = Instantiate(timerBarPrefab);
                timerBar.transform.position = transform.position + new Vector3(0, 7, 0);
                timerBar.transform.rotation = Quaternion.Euler(0, 35, 0);
                timerBar.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                timerBar.transform.SetParent(transform);
            }

            timeSpent += Time.deltaTime;
            timerBar.GetComponentInChildren<TimerBar>().UpdateTimer(timeSpent, timeToCook);

            if (timeSpent >= timeToCook)
            {
                isRefilled = true;
                Destroy(timerBar);
                sodaProp.SetActive(true);
            }
        }
    }

    public override void Interact()
    {
        if (isRefilled && playerController.activeInteractable == null)
        {
            sodaProp.SetActive(false);
            isRefilled = false;
            timeSpent = 0;
            base.Interact();
        }
    }
}