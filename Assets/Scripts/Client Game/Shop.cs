//Sanchay Ravindiran 2020

/*
    Dispenses client prefabs - ingame items -
    into the player's item list when the player
    comes into contact with the radius surrounding
    the current object and clicks on an item.
    Dispenses different items based on event feedback
    from different UI widgets attached to the ItemShop
    UI script, and notifies players of information
    regarding each different item as well as alerts
    when they cannot afford any particular item.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ItemShop itemShopUI;
    [SerializeField] private Notice notice;

    [SerializeField] private GameObject huntingRifle;
    [SerializeField] private GameObject aerialRifle;
    [SerializeField] private GameObject glock;
    [SerializeField] private GameObject broolahSuit;
    [SerializeField] private GameObject devGun;

    private ClientHumanPrefab shopper;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientPrefab"))
        {
            if (collision.GetComponent<ClientHumanPrefab>())
            {
                shopper = collision.GetComponent<ClientHumanPrefab>();
                itemShopUI.Show();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientPrefab"))
        {
            if (collision.GetComponent<ClientHumanPrefab>())
            {
                itemShopUI.Hide();
            }
        }
    }

    public void BoughtHuntingRifle()
    {
        if (!(ClientHumanPrefab.Money >= 0))
        {
            notice.Say("not enough cash", 2);
            return;
        }
        else
        {
            shopper.AddItem(huntingRifle, ClientWindow.ItemTypes.HumanHuntingRifle);
            itemShopUI.RefreshMoneyUI();

            notice.Say("purchased <i>hunting rifle<i>", 1);
        }
    }

    public void BoughtAerialRifle()
    {
        if (!(ClientHumanPrefab.Money >= 100))
        {
            notice.Say("not enough cash", 2);
            return;
        }
        else
        {
            ClientHumanPrefab.Money -= 100;
            shopper.AddItem(aerialRifle, ClientWindow.ItemTypes.HumanAerialRifle);
            itemShopUI.RefreshMoneyUI();

            notice.Say("purchased <i>aerial rifle<i>", 1);
        }
    }

    public void BoughtGlock()
    {
        if (!(ClientHumanPrefab.Money >= 1000))
        {
            notice.Say("not enough cash", 2);
            return;
        }
        else
        {
            ClientHumanPrefab.Money -= 1000;
            shopper.AddItem(glock, ClientWindow.ItemTypes.HumanGlock);
            itemShopUI.RefreshMoneyUI();

            notice.Say("purchased <i>glock<i>", 1);
        }
    }

    public void BoughtBroolah()
    {
        if (!(ClientHumanPrefab.Money >= 20000))
        {
            notice.Say("not enough cash", 2);
            return;
        }
        else
        {
            ClientHumanPrefab.Money -= 20000;
            shopper.AddItem(broolahSuit, ClientWindow.ItemTypes.HumanBroolahsuit);
            itemShopUI.RefreshMoneyUI();

            notice.Say("purchased <i>broolah suit<i>", 1);
        }
    }

    public void BoughtDevGun()
    {
        if (!(ClientHumanPrefab.Money >= 1000000))
        {
            notice.Say("not enough cash", 2);
            return;
        }
        else
        {
            ClientHumanPrefab.Money -= 1000000;
            shopper.AddItem(devGun, ClientWindow.ItemTypes.HumanDevgun);
            itemShopUI.RefreshMoneyUI();

            notice.Say("purchased <i>devgun<i>", 1);
        }
    }

    public void HuntingRifleInfo()
    {
        notice.Say("a standard gun for beezle hunting", 3);
    }

    public void AerialRifleInfo()
    {
        notice.Say("a gun specially made for shooting up", 3);
    }

    public void GlockInfo()
    {
        notice.Say("a slightly faster gun", 3);
    }

    public void BroolahInfo()
    {
        notice.Say("a suit engineered to hunt beezles", 3);
    }

    public void DevGunInfo()
    {
        notice.Say("a gun that knows no bounds", 3);
    }

    public void LanternInfo()
    {
        notice.Say("an upgraded version of the lanterns by that tree in camp", 3);
    }
}
