//Sanchay Ravindiran 2020

/*
    Extends network prefab and implements functionality
    specific to the network human prefab. This class takes
    messages from the game server and follows their
    instructions to make the network human prefab mimic the
    appearance and state of another player controlling their
    own client human prefab on their own simulation. Like
    the network beast prefab, the network human prefab would
    equip an item whenever its origin client human prefab
    equipped an item, and it would die when its client
    human prefab died - and so on.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetHumanPrefab : NetPrefab
{
    [Header("[NET] Components")]
    [SerializeField] private Animator Animator;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    private byte EquippedItem;
    private bool ItemEquipped;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        transform.GetChild(1).GetComponent<TextMeshPro>().text = Name;

        yield break;
    }

    protected override void UpdateAdditionalState(PlayerState playerState)
    {
        PlayerHumanState state = (PlayerHumanState)playerState;
        bool direction = state.Direction;

        Animator.SetBool("Walking", state.Walking);
        SpriteRenderer.flipX = !direction;

        if (ItemEquipped)
        {
            ItemOffset.GetChild(EquippedItem).GetComponent<NetHumanItem>().Flip(direction);
            if (!direction)
            {
                ItemOffset.localPosition = new Vector2(.5f, 0);
            }
            else
            {
                ItemOffset.localPosition = new Vector2(-.5f, 0);
            }
        }

    }

    public override void EquipItem(byte itemIndex)
    {
        foreach (Transform child in ItemOffset)
        {
            if (!child.transform.GetSiblingIndex().Equals(itemIndex))
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                EquippedItem = itemIndex;
                ItemEquipped = true;
                child.gameObject.SetActive(true);
            }
        }
    }

    public override void TriggerItem(byte itemIndex, bool trigger)
    {
        ItemOffset.GetChild(itemIndex).GetComponent<NetHumanItem>().TriggerMain(trigger);
    }
}
