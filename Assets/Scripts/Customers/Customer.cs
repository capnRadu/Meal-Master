using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [NonSerialized] public GameManager gameManager;
    [NonSerialized] public Animator modelAnimator;

    [NonSerialized] public int customerOrderWaypointIndex;

    [NonSerialized] public Transform checkPoint;
    [NonSerialized] public Transform orderPoint;
    [NonSerialized] public Transform destroyPoint;
    private List<Transform> waypoints = new List<Transform>();

    private bool hasReceivedOrder = false;

    private void Start()
    {
        waypoints.Add(checkPoint);
        waypoints.Add(orderPoint);
        waypoints.Add(destroyPoint);

        modelAnimator.SetBool("isWalking", true);

        StartCoroutine(MoveToWaypoints());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasReceivedOrder = true;
        }

        if (transform.position == destroyPoint.position)
        {
            gameManager.activeCustomerWaypointsIndex.Remove(customerOrderWaypointIndex);
            gameManager.currentCustomers--;
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveToWaypoints()
    {
        foreach (var waypoint in waypoints)
        {
            while (transform.position != waypoint.position)
            {
                if (waypoint != destroyPoint || (waypoint == destroyPoint && hasReceivedOrder))
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
                }
                else if (waypoint == destroyPoint && hasReceivedOrder)
                {
                    modelAnimator.SetBool("isWalking", true);
                }
            }
        }
    }
}