//Sanchay Ravindiran 2020

/*
    A base class that is extended by any classes
    that act as serverside items in the game. These
    are items that exist only as mirror copies of the
    items held by other players connected to the game
    server. These items can be thought of as ghost
    items that maintain the appearance and state of
    items held by other players across the network, but
    don't actually do that much on the player's actual
    game client. In short, network human items are items
    that maintain the appearance of synchronization
    and cohesion for the player.
*/

using UnityEngine;

public class NetHumanItem : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip TriggerSound;
    [SerializeField] private AudioSource SoundPlayer;

    public virtual void Trigger(bool trigger) //trigger
    {

    }

    public virtual void TriggerMain(bool trigger)
    {
        if (SoundPlayer == null)
        {
            Trigger(trigger);
        }
        else
        {
            SoundPlayer.PlayOneShot(TriggerSound);
            Trigger(trigger);
        }
    }

    public virtual void Flip(bool direction)
    {

    }
}
