//Sanchay Ravindiran 2020

/*
    Message used to transform a player into a different
    kind of player - i.e. from a beast into a human,
    or vice versa. This is done when a beast player dies,
    and when another human player is chosen to become the
    beast. It can also be done when the beast player quits
    or disconnects for some other reason.
*/

[System.Serializable]
public class PlayerTransform : Message
{
    public PlayerTransform()
    {
        MessageType = Type.PlayerTransform;
    }
    public bool Human;
}