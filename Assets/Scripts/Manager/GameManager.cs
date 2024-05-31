using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}