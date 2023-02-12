//Sanchay Ravindiran 2020

/*
    Type defines the type of message, which lets game
    servers and clients know how to handle any given
    message with its own corresponding protocol.

    Message is the base class that is extended by
    all messages throughout the project. It contains
    the type of message as well as the unique id
    of the client (user) that sent the message or
    who the message is intended for.
*/

public static class Type
{
    public const byte None = 0;
    public const byte Container = 1;
    public const byte Assign = 2;

    public const byte PlayerSpawn = 3;
    public const byte Destroy = 4;

    public const byte PlayerState = 5;

    public const byte PlayerDamage = 6;
    public const byte PlayerDeath = 7;
    public const byte PlayerRevive = 8;
    public const byte PlayerTransform = 9;

    //HUMAN

    public const byte PlayerItem = 10;
    public const byte PlayerItemAdd = 11;
    public const byte PlayerItemEquip = 12;
    public const byte PlayerItemTrigger = 13;
    public const byte PlayerItemClear = 14;

    public const byte PlayerBeastGrab = 15;
}

[System.Serializable]
public abstract class Message
{
    public byte MessageType { set; get; }
    public Message()
    {
        MessageType = Type.None;
    }
    public int User { set; get; }
}