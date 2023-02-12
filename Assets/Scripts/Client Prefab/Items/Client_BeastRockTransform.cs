//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the beast rock transformation
    item. This item allows beast players to transform into
    a rock, fooling passerby human players by blending into
    its surrounding environment.
*/

using UnityEngine;

public class Client_BeastRockTransform : ClientHumanItem
{
    [SerializeField] private Sprite rock;
    [SerializeField] private Sprite beast;
    private ClientBeastPrefab clientBeastPrefab;

    protected override void Enabled()
    {
        clientBeastPrefab = transform.parent.parent.parent.GetComponent<clientBeastPrefab>();

        Trigger(true, (byte)transform.GetSiblingIndex());

        clientBeastPrefab.IsARock = true;
        clientBeastPrefab.SpriteRenderer.sprite = rock;
        clientBeastPrefab.Animator.SetBool("Walking", false);

        Refresh("Phenomenal Disguise");
    }

    protected override void Disabled()
    {
        clientBeastPrefab.SpriteRenderer.sprite = beast;
        clientBeastPrefab.IsARock = false;

        Trigger(false, (byte)transform.GetSiblingIndex());
    }
}
