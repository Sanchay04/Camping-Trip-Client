//Sanchay Ravindiran 2020

/*
    Contains the position of a player. This is
    usually sent continuously between the game
    client and game server.
*/

[System.Serializable]
public class PlayerState : Message
{
    public PlayerState()
    {
        MessageType = Type.PlayerState;
    }
    public float X;
    public float Y;
}