//Sanchay Ravindiran 2020

/*
    Message used to signal the revival of a player,
    or a respawn.
*/

[System.Serializable]
public class PlayerRevive : Message
{
    public PlayerRevive()
    {
        MessageType = Type.PlayerRevive;
    }
}