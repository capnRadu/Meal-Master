using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Patty : MonoBehaviour
{
    private float timeToCook = 7;
    private float timeSpent = 0;
    [NonSerialized] public bool isCooked = false;

    [SerializeField] private GameObject timerBarPrefab;
    [NonSerialized] public GameObject timerBar;

    [SerializeField] private Material cookedMaterial;

    private void Start()
    {
        timeToCook -= timeToCook * Upgrades.Instance.Grill.upgradeAmount / 100;
    }

    private void Update()
    {
        if (transform.parent.parent.tag == "Grill" && !isCooked)
        {
            if (timerBar == null)
            {
                timerBar = Instantiate(timerBarPrefab, transform.position + new Vector3(0.7f, 3, 0), Quaternion.identity, transform);
                timerBar.transform.rotation = Quaternion.Euler(0, -35, 0);
            }

            timeSpent += Time.deltaTime;
            timerBar.GetComponentInChildren<TimerBar>().UpdateTimer(timeSpent, timeToCook);

            if (timeSpent >= timeToCook)
            {
                isCooked = true;
                GetComponent<MeshRenderer>().material = cookedMaterial;
            }
        }
    }
}
