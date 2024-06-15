using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float spawnRate = 5f;

    private List<string> orders = new List<string> { "Hamburger", "Fries", "Soda", "Salad" };

    [SerializeField] private List<GameObject> chefHats = new List<GameObject>();
    [SerializeField] private AudioSource failSfx;

    private void Update()
    {
        if (currentCustomers < maximumCustomers && !isSpawning)
        {
            StartCoroutine(SpawnCustomer());
            isSpawning = true;
        }
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
        customerScript.order = orders[UnityEngine.Random.Range(0, orders.Count)];

        currentCustomers++;

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}