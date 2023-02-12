//Sanchay Ravindiran 2020

/*
    These messages comprise any information
    needed to synchronize the state of a
    player's client human item with the states
    of items in other player simulations connected
    to the game server.
*/

[System.Serializable]
public class PlayerItem : Message
{
    public PlayerItem()
    {
        MessageType = Type.PlayerItem;
    }
    public byte ItemID;
    public byte ItemIndex;
}

[System.Serializable]
public class PlayerItemAdd : Message
{
    public PlayerItemAdd()
    {
        MessageType = Type.PlayerItemAdd;
    }
    public byte ItemID;
}

[System.Serializable]
public class PlayerItemEquip : Message
{
    public PlayerItemEquip()
    {
        MessageType = Type.PlayerItemEquip;
    }
    public byte ItemIndex;
}


[System.Serializable]
public class PlayerItemTrigger : Message
{
    public PlayerItemTrigger()
    {
        MessageType = Type.PlayerItemTrigger;
    }
    public byte ItemIndex;
    public bool ItemTrigger;
}

[System.Serializable]
public class PlayerItemClear : Message
{
    public PlayerItemClear()
    {
        MessageType = Type.PlayerItemClear;
    }
}



