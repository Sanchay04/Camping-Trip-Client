//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the client lantern item.
    This item allows human players to illuminate their
    surroundings and to make their way through the
    dark forest where the beast lurks looking for people
    to grab. However, each lantern can only burn for
    so long, so players must relight them at the campfire
    every 100 seconds so that they do not become lost
    in the wilderness.
*/

using UnityEngine;

public class Client_Lantern : ClientHumanItem
{
    [SerializeField] private Light Lantern;
    [SerializeField] private Animator LanternAnimator;

    [SerializeField] private SpriteRenderer SpriteRenderer;
    private bool BurntOut;

    private float Flame = 100f;

    protected override void Enabled()
    {
        LanternAnimator.SetBool("Lit", !BurntOut);
        Refresh(FlameCount());
    }

    public override void Flip(bool direction)
    {
        SpriteRenderer.flipX = direction;
    }

    private void Update()
    {
        if (BurntOut) return;

        if (Flame <= 0)
        {
            Lantern.enabled = false;

            LanternAnimator.SetBool("Lit", false);

            PlayerItemTrigger trigger = new PlayerItemTrigger
            {
                ItemIndex = (byte)transform.GetSiblingIndex(),
                ItemTrigger = false
            };
            Client.Commands.Send(trigger, Client.Reliable);

            BurntOut = true;

            return;
        }

        Flame -= Time.deltaTime;
        Refresh(FlameCount());
    }

    protected override void Disabled()
    {
        Refresh("-");
    }

    private string FlameCount()
    {
        return string.Format("<i>{0}<i>", Mathf.RoundToInt(Flame));
    }

    public void Relight()
    {
        Flame = 300f;
        Lantern.enabled = true;
        LanternAnimator.SetBool("Lit", true);

        PlayerItemTrigger trigger = new PlayerItemTrigger
        {
            ItemIndex = (byte)transform.GetSiblingIndex(),
            ItemTrigger = true
        };
        Client.Commands.Send(trigger, Client.Reliable);

        BurntOut = false;

        Refresh(FlameCount());
    }
}
