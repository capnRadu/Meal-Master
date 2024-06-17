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

    public List<string> order = new List<string>();
    [NonSerialized] public bool hasReceivedOrder = false;
    private bool hasLostHat = false;

    private float waitingTime = 20;
    private float timeSpent = 20;
    [SerializeField] private GameObject timerBarPrefab;
    [NonSerialized] public GameObject timerBar;

    [SerializeField] private GameObject orderCanvasPrefab;
    [NonSerialized] public GameObject orderCanvas;
    private GameObject happyIcon;
    private GameObject confusedIcon;
    private GameObject angryIcon;
    private GameObject hamburgerIcon;
    private GameObject friesIcon;
    private GameObject sodaIcon;
    private GameObject saladIcon;
    private TextMeshProUGUI ordersNumber;

    public enum orderState { Happy, Confused, Angry };
    public orderState currentState;

    [NonSerialized] public int happyOrderMoney = 15;
    [NonSerialized] public int confusedOrderMoney = 10;
    [NonSerialized] public int angryOrderMoney = 5;

    private void Start()
    {
        Setup();

        StartCoroutine(MoveToWaypoints());
    }

    private void Update()
    {
        if (transform.position == destroyPoint.position)
        {
            DestroyCustomer();
        }

        if (timerBar != null && timeSpent >= 0)
        {
            timeSpent -= Time.deltaTime;
            timerBar.GetComponentInChildren<TimerBar>().UpdateTimer(timeSpent, waitingTime);

            ordersNumber.text = $"x{order.Count}";

            ManageCustomerEmotion();
        }
    }

    private void Setup()
    {
        playerController = FindObjectOfType<PlayerController>();

        waypoints.Add(checkPoint);
        waypoints.Add(orderPoint);
        waypoints.Add(destroyPoint);

        modelAnimator.SetBool("isWalking", true);

        currentState = orderState.Happy;

        switch (DifficultyManager.Instance.currentDifficulty)
        {
            case DifficultyManager.difficultyMode.Novice:
                waitingTime = 45;
                timeSpent = 45;
                happyOrderMoney = 15;
                confusedOrderMoney = 10;
                angryOrderMoney = 5;
                break;
            case DifficultyManager.difficultyMode.Expert:
                waitingTime = 15;
                timeSpent = 15;
                happyOrderMoney = 30;
                confusedOrderMoney = 20;
                angryOrderMoney = 10;
                break;
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

                    if (waypoint == destroyPoint)
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

                if (waypoint == orderPoint && transform.position == waypoint.position)
                {
                    modelAnimator.SetBool("isWalking", false);

                    timerBar = Instantiate(timerBarPrefab, transform.position + new Vector3(1.42f, 11, 0), Quaternion.identity, transform);
                    orderCanvas = Instantiate(orderCanvasPrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity, transform);

                    GetIconReferences();

                    UpdateOrderIcon();
                }

                yield return null;
            }
        }
    }

    private void GetIconReferences()
    {
        foreach (Transform transform in orderCanvas.transform)
        {
            switch (transform.tag)
            {
                case "Happy Icon":
                    happyIcon = transform.gameObject;
                    break;
                case "Confused Icon":
                    confusedIcon = transform.gameObject;
                    break;
                case "Angry Icon":
                    angryIcon = transform.gameObject;
                    break;
                case "Hamburger Icon":
                    hamburgerIcon = transform.gameObject;
                    break;
                case "Fries Icon":
                    friesIcon = transform.gameObject;
                    break;
                case "Soda Icon":
                    sodaIcon = transform.gameObject;
                    break;
                case "Salad Icon":
                    saladIcon = transform.gameObject;
                    break;
                case "Orders Number":
                    ordersNumber = transform.GetComponent<TextMeshProUGUI>();
                    break;
            }
        }
    }

    private void UpdateOrderIcon()
    {
        switch (order[0])
        {
            case "Hamburger":
                hamburgerIcon.SetActive(true);
                break;
            case "Fries":
                friesIcon.SetActive(true);
                break;
            case "Soda":
                sodaIcon.SetActive(true);
                break;
            case "Salad":
                saladIcon.SetActive(true);
                break;
        }
    }

    private void DestroyCustomer()
    {
        gameManager.activeCustomerWaypointsIndex.Remove(customerOrderWaypointIndex);
        gameManager.currentCustomers--;
        Destroy(gameObject);
    }

    private void ManageCustomerEmotion()
    {
        if (timeSpent <= waitingTime * 3 / 4)
        {
            happyIcon.SetActive(false);
            confusedIcon.SetActive(true);

            currentState = orderState.Confused;
            StartCoroutine(ScaleIcon(confusedIcon.transform, 0.5f));
        }

        if (timeSpent <= waitingTime * 1 / 2)
        {
            confusedIcon.SetActive(false);
            angryIcon.SetActive(true);

            currentState = orderState.Angry;
            StartCoroutine(ScaleIcon(angryIcon.transform, 0.5f));
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