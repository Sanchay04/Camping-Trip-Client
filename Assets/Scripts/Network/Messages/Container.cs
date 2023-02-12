//Sanchay Ravindiran 2020

/*
    Contains numerous messages grouped into a single message cluster.
*/

[System.Serializable]
public class Container : Message
{
    public Container()
    {
        MessageType = Type.Container;
    }
    public Message[] Messages { set; get; }
}