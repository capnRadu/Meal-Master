using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private List<GameObject> customerModels = new List<GameObject>();

    [SerializeField] private List<Transform> customerOrderPoints = new List<Transform>();
    [SerializeField] private List<Transform> customerCheckPoints = new List<Transform>();
    [NonSerialized] public List<int> activeCustomerWaypointsIndex = new List<int>();

    [SerializeField] private Transform spawnDestroyPoint;
    private int maximumCustomers = 4;
    [NonSerialized] public int currentCustomers = 0;
    private bool isSpawning = false;
    private float spawnRate = 11f;

    private List<string> orders = new List<string> { "Hamburger", "Fries", "Soda", "Salad" };

    [SerializeField] private List<GameObject> chefHats = new List<GameObject>();
    [SerializeField] private AudioSource failSfx;
    [SerializeField] private GameObject roundEndMenu;
    [SerializeField] private GameObject pauseButton;

    [NonSerialized] public int round = 0;
    private int customersPerRound = 4;
    private int totalSpawnedCustomers = 0;
    [SerializeField] private GameObject roundPanel;
    [SerializeField] private TextMeshProUGUI currentRoundPanelText;
    [SerializeField] private TextMeshProUGUI currentRoundHudText;
    private bool isStartingNewRound = false;

    private void Start()
    {
        switch (DifficultyManager.Instance.currentDifficulty)
        {
            case DifficultyManager.difficultyMode.Novice:
                spawnRate = 11f;
                break;
            case DifficultyManager.difficultyMode.Expert:
                spawnRate = 6f;
                break;
        }

        NewRound();
    }

    private void Update()
    {
        if (!isStartingNewRound)
        {
            if (currentCustomers < maximumCustomers && !isSpawning && totalSpawnedCustomers != customersPerRound)
            {
                StartCoroutine(SpawnCustomer());
                isSpawning = true;
            }
            else if (currentCustomers == 0 && totalSpawnedCustomers == customersPerRound)
            {
                NewRound();
            }
        }
    }

    private void NewRound()
    {
        round++;
        totalSpawnedCustomers = 0;
        customersPerRound += 2;

        if (round == 5 && DifficultyManager.Instance.expertLocked)
        {
            DifficultyManager.Instance.expertLocked = false;
        }

        if (spawnRate > 2f)
        {
           spawnRate -= 1f;
        }

        currentRoundPanelText.text = "Round " + round;
        currentRoundHudText.text = "Round " + round;
        StartCoroutine(RoundPanel(roundPanel.transform));
    }

    private IEnumerator RoundPanel(Transform panel)
    {
        isStartingNewRound = true;

        if (round == 1)
        {
            yield return new WaitForSeconds(1f);
        }

        panel.gameObject.SetActive(true);
        panel.localScale = Vector3.zero;

        Vector3 originalScale = panel.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            panel.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        yield return new WaitForSeconds(2f);

        while (elapsedTime < duration)
        {
            panel.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.gameObject.SetActive(false);

        isStartingNewRound = false;
    }

    private IEnumerator SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnDestroyPoint.position, spawnDestroyPoint.rotation);
        Customer customerScript = customer.GetComponent<Customer>();

        var randomOrderWaypointIndex = -1;
        while (randomOrderWaypointIndex == -1 || activeCustomerWaypointsIndex.Contains(randomOrderWaypointIndex))
        {
            randomOrderWaypointIndex = UnityEngine.Random.Range(0, customerOrderPoints.Count);
        }
        activeCustomerWaypointsIndex.Add(randomOrderWaypointIndex);

        customerScript.customerOrderWaypointIndex = randomOrderWaypointIndex;
        customerScript.checkPoint = customerCheckPoints[randomOrderWaypointIndex];
        customerScript.orderPoint = customerOrderPoints[randomOrderWaypointIndex];
        customerScript.destroyPoint = spawnDestroyPoint;

        GameObject customerModel = Instantiate(customerModels[UnityEngine.Random.Range(0, customerModels.Count)], customer.transform.position, customer.transform.rotation);
        customerModel.transform.SetParent(customer.transform);

        customerScript.modelAnimator = customerModel.GetComponent<Animator>();
        customerScript.gameManager = this;

        string order = orders[UnityEngine.Random.Range(0, orders.Count)];

        switch (DifficultyManager.Instance.currentDifficulty)
        {
            case DifficultyManager.difficultyMode.Novice:
                customerScript.order.Add(order);
                break;
            case DifficultyManager.difficultyMode.Expert:
                int randomOrdersNumber = UnityEngine.Random.Range(1, 3);
                for (int i = 0; i < randomOrdersNumber; i++)
                {
                    customerScript.order.Add(order);
                }
                break;
        }

        currentCustomers++;
        totalSpawnedCustomers++;

        yield return new WaitForSeconds(spawnRate);

        isSpawning = false;
    }

    public void LoseHat()
    {
        if (chefHats.Count > 0)
        {
            failSfx.Play();
            StartCoroutine(DeactivateIcon(chefHats[chefHats.Count - 1].transform));
        }
    }

    private IEnumerator DeactivateIcon(Transform icon)
    {
        if (icon == null)
        {
            yield break;
        }

        Vector3 originalScale = icon.localScale;
        Vector3 targetScale = originalScale * 1.5f;
        float duration = 0.2f;
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
            Destroy(icon.gameObject);
            chefHats.RemoveAt(chefHats.Count - 1);

            if (chefHats.Count == 0)
            {
                pauseButton.GetComponent<PauseButton>().PauseGame(false);
                pauseButton.SetActive(false);
                roundEndMenu.SetActive(true);
            }
        }
    }
}