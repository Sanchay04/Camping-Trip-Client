//Sanchay Ravindiran 2020

/*
    Notifies the game server that a beast player has
    grabbed and is carrying a human player.
*/

[System.Serializable]
public class PlayerBeastGrab : Message
{
    public PlayerBeastGrab()
    {
        MessageType = Type.PlayerBeastGrab;
    }
    public int GrabbedUser;
}
