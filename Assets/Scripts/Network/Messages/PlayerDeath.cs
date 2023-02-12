//Sanchay Ravindiran 2020

/*
    Message used to signal the death of a
    player. The deaths of human players versus
    beast players are handled differently - when
    a human player dies the player can respawn,
    while when a beast player dies it flies into
    the air and lands on the ground before a lone
    sunflower blossoms from the body. Another random
    player is then chosen to become the beast.
*/

[System.Serializable]
public class PlayerDeath : Message
{
    public PlayerDeath()
    {
        MessageType = Type.PlayerDeath;
    }
}
