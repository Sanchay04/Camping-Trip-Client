//Sanchay Ravindiran 2020

/*
    Extends network human item and implements functionality
    specific to the network beast rock transformation item.
    This item mimics the appearance and state of a client
    beast rock transformation item operated by another player
    connected to the game server.
*/

using UnityEngine;

public class Net_BeastRockTransform : NetHumanItem
{
    [SerializeField] private Sprite Rock;
    private NetBeastPrefab NetBeastPrefab;

    public override void Trigger(bool trigger)
    {
        NetBeastPrefab = transform.parent.parent.parent.GetComponent<NetBeastPrefab>();

        if (trigger)
        {
            NetBeastPrefab.SpriteRenderer.sprite = Rock;
            NetBeastPrefab.IsARock = true;
        }
        else
        {
            NetBeastPrefab.IsARock = false;
        }
    }
}
