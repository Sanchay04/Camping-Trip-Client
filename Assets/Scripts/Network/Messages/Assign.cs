//Sanchay Ravindiran 2020

/*
    Message used to assign incoming clients with a unique identifier.
*/

[System.Serializable]
public class Assign : Message
{
    public Assign()
    {
        MessageType = Type.Assign;
    }
    public int User { set; get; }
}