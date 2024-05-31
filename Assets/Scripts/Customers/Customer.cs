using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    PlayerController playerController;

    [NonSerialized] public GameManager gameManager;
    [NonSerialized] public Animator modelAnimator;

    [NonSerialized] public int customerOrderWaypointIndex;

    [NonSerialized] public Transform checkPoint;
    [NonSerialized] public Transform orderPoint;
    [NonSerialized] public Transform destroyPoint;
    private List<Transform> waypoints = new List<Transform>();

    public string order;
    [NonSerialized] public bool hasReceivedOrder = false;

    private float waitingTime = 20;
    private float timeSpent = 20;
    [SerializeField] private GameObject timerBarPrefab;
    [NonSerialized] public GameObject timerBar;

    [SerializeField] private GameObject orderCanvasPrefab;
    [NonSerialized] public GameObject orderCanvas;


    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        waypoints.Add(checkPoint);
        waypoints.Add(orderPoint);
        waypoints.Add(destroyPoint);

        modelAnimator.SetBool("isWalking", true);

        StartCoroutine(MoveToWaypoints());
    }

    private void Update()
    {
        if (transform.position == destroyPoint.position)
        {
            gameManager.activeCustomerWaypointsIndex.Remove(customerOrderWaypointIndex);
            gameManager.currentCustomers--;
            Destroy(gameObject);
        }

        if (timerBar != null && timeSpent >= 0)
        {
            timeSpent -= Time.deltaTime;
            timerBar.GetComponentInChildren<TimerBar>().UpdateTimer(timeSpent, waitingTime);
        }
    }

    private IEnumerator MoveToWaypoints()
    {
        foreach (var waypoint in waypoints)
        {
            while (transform.position != waypoint.position)
            {
                if (waypoint != destroyPoint || (waypoint == destroyPoint && (hasReceivedOrder || timeSpent <= 0)))
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoint.position, 10f * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoint.position - transform.position), 10f * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    yield return null;
                }

                if (waypoint == orderPoint && transform.position == waypoint.position)
                {
                    modelAnimator.SetBool("isWalking", false);

                    timerBar = Instantiate(timerBarPrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity, transform);
                    orderCanvas = Instantiate(orderCanvasPrefab, transform.position + new Vector3(0, 11, 0), Quaternion.identity, transform);
                    orderCanvas.GetComponentInChildren<TextMeshProUGUI>().text = order;
                }
                else if (waypoint == destroyPoint && (hasReceivedOrder || timeSpent <= 0))
                {
                    modelAnimator.SetBool("isWalking", true);
                    Destroy(timerBar);
                    Destroy(orderCanvas);
                }
            }
        }
    }

    public void Interact()
    {
        playerController.Serve(order, this);
    }
}