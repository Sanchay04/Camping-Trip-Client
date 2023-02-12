//Sanchay Ravindiran 2020

/*
    Contains information needed by the game server
    to create an incoming player in the simulations
    of all other connected clients.
*/

[System.Serializable]
public class PlayerSpawn : Message
{
    public PlayerSpawn()
    {
        MessageType = Type.PlayerSpawn;
    }

    public PlayerItem[] Items;
    public byte EquippedItemIndex; //make Server PlayerSpawn Equippedindex updated w/ ItemEquip
    public bool EquippedItemTrigger; //make Server PlayerSpawn ItemTrigger updated w/ ItemTrigger
    public string Name;
    public bool Human;
    public float X;
    public float Y;
}