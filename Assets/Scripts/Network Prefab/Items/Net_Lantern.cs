//Sanchay Ravindiran 2020

/*
    Extends network prefab and implements functionality
    specific to the network lantern item. This item mimics the appearance
    and state of a client lantern item operated by another player
    connected to the game server.
*/

using UnityEngine;

public class Net_Lantern : NetHumanItem
{
    [SerializeField] private Light Lantern;
    [SerializeField] private Animator LanternAnimator;

    [SerializeField] private SpriteRenderer SpriteRenderer;

    public override void Trigger(bool trigger)
    {
        Lantern.enabled = trigger;
        LanternAnimator.SetBool("Lit", trigger);
    }

    public override void Flip(bool direction)
    {
        SpriteRenderer.flipX = direction;
    }
}
