using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class ShopMenu : MonoBehaviour
{
    public void BuyMoney(Product product)
    {
        Upgrades.Instance.PlayerMoney += (int) product.definition.payout.quantity;
        Upgrades.Instance.UpdatePlayerMoneyText();
    }
}
