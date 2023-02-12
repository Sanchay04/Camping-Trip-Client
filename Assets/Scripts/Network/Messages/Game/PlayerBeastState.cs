//Sanchay Ravindiran 2020

/*
    Comprises information about the state of
    a player beast beyond its generic client prefab state
    information.
*/

[System.Serializable]
public class PlayerBeastState : PlayerState
{
    public PlayerBeastState()
    {
        MessageType = Type.PlayerState;
    }

    public bool Direction;
    public bool Walking;
    public bool Climbing;
}
