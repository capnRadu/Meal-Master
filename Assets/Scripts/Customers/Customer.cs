using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private bool hasLostHat = false;

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

            if (timeSpent <= waitingTime * 3 / 4)
            {
                foreach (Transform transform in orderCanvas.transform)
                {
                    if (transform.tag == "Happy Icon")
                    {
                        transform.gameObject.SetActive(false);
                    }

                    if (transform.tag == "Confused Icon")
                    {
                        transform.gameObject.SetActive(true);
                        StartCoroutine(ScaleIcon(transform, 0.5f));
                    }
                }
            }

            if (timeSpent <= waitingTime * 1 / 2)
            {
                foreach (Transform transform in orderCanvas.transform)
                {
                    if (transform.tag == "Confused Icon")
                    {
                        transform.gameObject.SetActive(false);
                    }

                    if (transform.tag == "Angry Icon")
                    {
                        transform.gameObject.SetActive(true);
                        StartCoroutine(ScaleIcon(transform, 0.3f));
                    }
                }
            }
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

                    timerBar = Instantiate(timerBarPrefab, transform.position + new Vector3(1.42f, 11, 0), Quaternion.identity, transform);
                    orderCanvas = Instantiate(orderCanvasPrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity, transform);
                    
                    foreach (Transform transform in orderCanvas.transform)
                    {
                        if (order == "Hamburger" && transform.tag == "Hamburger Icon")
                        {
                            transform.gameObject.SetActive(true);
                        }
                        else if (order == "Fries" && transform.tag == "Fries Icon")
                        {
                            transform.gameObject.SetActive(true);
                        }
                        else if (order == "Soda" && transform.tag == "Soda Icon")
                        {
                            transform.gameObject.SetActive(true);
                        }
                        else if (order == "Salad" && transform.tag == "Salad Icon")
                        {
                            transform.gameObject.SetActive(true);
                        }
                    }
                }
                else if (waypoint == destroyPoint && (hasReceivedOrder || timeSpent <= 0))
                {
                    if (!hasReceivedOrder && !hasLostHat)
                    {
                        hasLostHat = true;
                        gameManager.LoseHat();
                    }

                    modelAnimator.SetBool("isWalking", true);
                    Destroy(timerBar);
                    Destroy(orderCanvas);
                }
            }
        }
    }

    private IEnumerator ScaleIcon(Transform icon, float duration)
    {
        if (icon == null)
        {
            yield break;
        }

        Vector3 originalScale = icon.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (icon == null)
            {
                yield break;
            }

            icon.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (icon == null)
            {
                yield break;
            }

            icon.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (icon != null)
        {
            icon.localScale = originalScale;
        }
    }

    public void Interact()
    {
        playerController.Serve(order, this);
    }
}