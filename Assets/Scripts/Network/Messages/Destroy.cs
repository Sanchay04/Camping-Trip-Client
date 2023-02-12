//Sanchay Ravindiran 2020

/*
    Used to destroy anything associated with the sender's
    client id.
*/

[System.Serializable]
public class Destroy : Message
{
    public Destroy()
    {
        MessageType = Type.Destroy;
    }
}