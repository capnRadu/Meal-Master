using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIngredient : MonoBehaviour, IInteractable
{
    [NonSerialized] public PlayerController playerController;

    public GameObject ingredientPrefab;

    protected virtual void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public virtual void Interact()
    {
        playerController.PickUp(ingredientPrefab);
    }
}
