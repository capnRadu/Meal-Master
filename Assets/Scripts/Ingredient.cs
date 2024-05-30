using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    PlayerController playerController;

    public GameObject ingredientPrefab;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        playerController.PickUp(ingredientPrefab);
    }
}
