//Sanchay Ravindiran 2020

/*
    Message used to signal to the game server
    that a player has taken damage from a certain
    other player.
*/

[System.Serializable]
public class PlayerDamage : Message
{
    public PlayerDamage()
    {
        MessageType = Type.PlayerDamage;
    }
    public byte Amount;
    public int TargetUser;
}