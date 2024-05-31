using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepFryer : BasicIngredient
{
    private float timeToCook = 5;
    private float timeSpent = 0;
    private bool isFried = false;
    private bool startFryer = false;

    [SerializeField] private GameObject timerBarPrefab;
    [NonSerialized] public GameObject timerBar;

    [SerializeField] private GameObject friesProp;

    [SerializeField] private float offsetZ;

    private void Update()
    {
        if (startFryer)
        {
            if (timerBar == null)
            {
                timerBar = Instantiate(timerBarPrefab);
                timerBar.transform.position = transform.position + new Vector3(0, 3, offsetZ);
                timerBar.transform.rotation = Quaternion.Euler(0, 35, 0);
                timerBar.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                timerBar.transform.SetParent(transform);
            }

            timeSpent += Time.deltaTime;
            timerBar.GetComponentInChildren<TimerBar>().UpdateTimer(timeSpent, timeToCook);

            if (timeSpent >= timeToCook)
            {
                isFried = true;
                startFryer = false;
                Destroy(timerBar);
                friesProp.SetActive(true);
            }
        }
    }

    public override void Interact()
    {
        if (isFried && playerController.activeInteractable == null)
        {
            friesProp.SetActive(false);
            isFried = false;
            timeSpent = 0;
            base.Interact();
        }
        else if (!isFried)
        {
            startFryer = true;
        }
    }
}