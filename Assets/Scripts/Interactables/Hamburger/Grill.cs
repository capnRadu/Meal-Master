using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour, IInteractable
{
    PlayerController playerController;

    [SerializeField] private GameObject placePoint;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        playerController.Grill(placePoint);
    }
}
