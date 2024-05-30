using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buns : BasicIngredient
{
    public override void Interact()
    {
        playerController.FinishHamburger(ingredientPrefab);
    }
}