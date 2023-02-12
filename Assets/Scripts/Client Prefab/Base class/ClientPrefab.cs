//Sanchay Ravindiran 2020

/*
    A base class that is extended by any objects
    that act as clientside prefabs in the game.
    These are prefabs that would perform like
    ordinary objects controlled by the player,
    except like client human items they send out
    messages to the game server when anything
    important is done with them.

    When these important things happen different
    kinds of messages are sent to the game server
    to synchronize other simulations with the state
    of this particular player. Also, every client
    prefab continuously sends its state to the game
    server at a rate of ~11 times a second, and this
    state comprises things like a prefab's position
    and orientation because the game utilizes a client
    authoritative model.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrefab : MonoBehaviour
{
    [Header("Inherited")]
    public PlayerState PlayerState;
    public bool Dead;
    public List<GameObject> Inventory = new List<GameObject>();
    public int InventoryIndex;

    protected DeathScreen DeathScreen;

    private void OnEnable()
    {
        DeathScreen = GameObject.FindWithTag("DeathScreen").GetComponent<DeathScreen>();
        StartCoroutine(SendStateLoop());
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    private void OnDisable()
    {
        StopCoroutine(SendStateLoop());
    }

    private IEnumerator SendStateLoop()
    {
        for (; ; )
        {
            if (!Dead)
            {
                Client.Commands.Send(PlayerState, Client.Unreliable);
            }
            yield return new WaitForSeconds(.09f);
        }
    }

    public virtual void Damage(byte amount, int damager)
    {

    }

    public virtual void Revive()
    {

    }

    public void AddItem(GameObject item, byte itemType)
    {
        PlayerItemAdd playerHumanItemAdd = new PlayerItemAdd
        {
            ItemID = itemType,
        };
        Client.Commands.Send(playerHumanItemAdd, Client.Reliable);
        ItemAdded(item);
    }

    protected virtual void ItemAdded(GameObject item)
    {

    }
}
