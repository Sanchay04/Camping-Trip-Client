//Sanchay Ravindiran 2020

/*
    Comprises information regarding the state of
    a human player beyond its generic client prefab
    state information.
*/

[System.Serializable]
public class PlayerHumanState : PlayerState
{
    public PlayerHumanState()
    {
        MessageType = Type.PlayerState;
    }
    public bool Direction;
    public bool Walking;
}
