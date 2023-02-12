//Sanchay Ravindiran 2020

/*
    A base class that is extended by any classes
    that act as clientside items in the game. This
    means items that act as usual ingame items would
    but also send messages to the game server when
    anything important is done with them so that
    simulations run by other players can have a cohesive,
    up to date view of what other players are doing in
    the game.

    When these important things happen a
    trigger method is called within this base class that
    sends a message containing the item index within
    the player item list and a trigger state to the
    server, which then performs the trigger on all
    other connected clients accordingly.

    MISTAKE NOTE:

    While this is called a Client Human Item, it is also
    extended by a class for an object that acts as an item
    for clientside beasts. Beast players have a clientside
    ability implemented as a human item with a trigger,
    and since this ability was added at the last second
    the name Client "Human" Item persists.
*/

using UnityEngine;
using TMPro;

public class ClientHumanItem : MonoBehaviour
{
    private static TextMeshProUGUI itemNameDisplay;
    private static TextMeshProUGUI itemDynamicInformationDisplay;

    [SerializeField] protected string itemName;

    public static class ItemType
    {
        public const byte Lantern = 0;
        public const byte Musket = 1;
        public const byte Battery = 2;
    }

    private void Awake()
    {
        itemNameDisplay = GameObject.FindWithTag("itemName").GetComponent<TextMeshProUGUI>();
        itemDynamicInformationDisplay = GameObject.FindWithTag("ItemDynamicInformation").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        itemNameDisplay.text = string.Format("({0}) <i>{1}<i>", transform.GetSiblingIndex(), itemName);
        itemDynamicInformationDisplay.text = "-";

        Enabled();
    }

    private void OnDisable()
    {
        itemNameDisplay.text = "";
        itemDynamicInformationDisplay.text = "";

        Disabled();
    }

    protected void Refresh(string dynamicInformation)
    {
        itemDynamicInformationDisplay.text = dynamicInformation;
    }

    protected virtual void Enabled()
    {

    }

    protected virtual void Disabled()
    {

    }

    protected void Trigger(bool trigger, byte index)
    {
        PlayerItemTrigger playerHumanItemTrigger = new PlayerItemTrigger
        {
            ItemTrigger = trigger,
            ItemIndex = index
        };

        Client.Commands.Send(playerHumanItemTrigger, Client.Reliable);
    }
}
