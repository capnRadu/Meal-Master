using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour, IInteractable
{
    PlayerController playerController;

    [SerializeField] private GameObject placePoint;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        playerController.Tray(placePoint);
    }
}
