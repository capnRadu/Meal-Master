using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator playerAnimator;

    IInteractable interactable;
    private GameObject activeInteractable;
    [SerializeField] private GameObject holdingPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);

                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactableObject))
                {
                    interactable = interactableObject;
                }
            }
        }

        UpdateAnimation();

        if (HasReachedDestination() && interactable != null)
        {
            interactable.Interact();
            interactable = null;
        }
    }

    private void UpdateAnimation()
    {
        if (agent.velocity != Vector3.zero)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
    }

    private bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void PickUp(GameObject interactablePrefab)
    {
        if (activeInteractable == null)
        {
            GameObject interactable = Instantiate(interactablePrefab, holdingPoint.transform.position, Quaternion.identity);
            interactable.transform.SetParent(holdingPoint.transform);
            activeInteractable = interactable;
        }
    }

    public void ThrowToTrash()
    {
        if (holdingPoint.transform.childCount > 0)
        {
            Destroy(holdingPoint.transform.GetChild(0).gameObject);
            activeInteractable = null;
        }
    }

    public void Tray(GameObject _placePoint)
    {
        if (activeInteractable != null && _placePoint.transform.childCount == 0)
        {
            activeInteractable.transform.SetParent(_placePoint.transform);
            activeInteractable.transform.position = _placePoint.transform.position;
            activeInteractable = null;
        }
        else if (activeInteractable == null && _placePoint.transform.childCount == 1)
        {
            activeInteractable = _placePoint.transform.GetChild(0).gameObject;
            activeInteractable.transform.SetParent(holdingPoint.transform);
            activeInteractable.transform.position = holdingPoint.transform.position;
        }
    }

    public void Grill(GameObject _placePoint)
    {
        if (activeInteractable != null)
        {
            if (activeInteractable.tag == "Patty" && _placePoint.transform.childCount == 0 && !activeInteractable.GetComponent<Patty>().isCooked)
            {
                activeInteractable.transform.SetParent(_placePoint.transform);
                activeInteractable.transform.position = _placePoint.transform.position;
                activeInteractable = null;
            }
        }
        else if (activeInteractable == null && _placePoint.transform.childCount == 1 && _placePoint.transform.GetChild(0).GetComponent<Patty>().isCooked)
        {
            activeInteractable = _placePoint.transform.GetChild(0).gameObject;
            activeInteractable.transform.SetParent(holdingPoint.transform);
            activeInteractable.transform.position = holdingPoint.transform.position;
            Destroy(activeInteractable.GetComponent<Patty>().timerBar);
        }
    }

    public void FinishHamburger(GameObject interactablePrefab)
    {
        if (activeInteractable != null)
        {
            if (activeInteractable.tag == "Patty" && activeInteractable.GetComponent<Patty>().isCooked)
            {
                Destroy(activeInteractable);
                GameObject interactable = Instantiate(interactablePrefab, holdingPoint.transform.position, Quaternion.identity);
                interactable.transform.SetParent(holdingPoint.transform);
                activeInteractable = interactable;
            }
        }
    }
}