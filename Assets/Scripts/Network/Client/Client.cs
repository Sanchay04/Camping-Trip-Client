//Sanchay Ravindiran 2020

/*
    This class represents the main network gateway throughout
    the client project. It implements the functionality needed
    to send and receive packets to and from a game server, and
    every other class then uses the abstractions provided by
    this class to perform network operations via messages.

    Each client connected to the game server is assigned its
    own unique id, and this id is placed on all communication
    between the server and this client. When a client does
    not send messages to the server for long enough a timeout
    is issued, and the client is disconnected via the engine's
    builtin udp socket layer.
*/

using System.Collections;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[RequireComponent(typeof(ClientWindow))]
public class Client : MonoBehaviour
{
    public static class Setup
    {
        public static class Host
        {
            public static int Port;
        }
        public static class Destination
        {
            public static string Address;
            public static int Port;
        }
        public static class Connection
        {
            public static ushort SendDelay = 10;
            public static ushort MessageQueue = 128;
            public static byte ConnectionAttempts = 10;
        }
    }

    private static ClientWindow ClientWindow;

    private static byte Error;
    private static byte[] MessageBuffer = new byte[2560];
    private static BinaryFormatter BinaryFormatter = new BinaryFormatter();
    private static Queue RecievedMessages = new Queue();

    private static int Host = -1;
    private static int Server = -1;
    public static int User = -1; //replaced User with Connection (more accurate name)

    private static bool Hosted;
    private static bool Connected;
    public static bool Ready;
    public static byte Reliable;
    public static byte ReliableOrdered;
    public static byte Unreliable;
    public static byte UnreliableFragmented;

    [Space]
    [SerializeField] private int HostPort;
    [SerializeField] private int DestinationPort;
    [SerializeField] private string DestinationAddress;

    private void Awake()
    {
        ClientWindow = GetComponent<ClientWindow>();
    }

    public void On()
    {
        if (Hosted) return;

        GlobalConfig globalConfig = new GlobalConfig();
        globalConfig.ReactorModel = ReactorModel.SelectReactor;

        NetworkTransport.Init(globalConfig);
        Application.runInBackground = true;

        print(MessageBuffer.Length);

        ConnectionConfig connectionConfig = new ConnectionConfig();
        connectionConfig.SendDelay = Setup.Connection.SendDelay;
        connectionConfig.MaxSentMessageQueueSize = Setup.Connection.MessageQueue;
        connectionConfig.MaxConnectionAttempt = Setup.Connection.ConnectionAttempts;
        connectionConfig.MinUpdateTimeout = 1;


        Reliable = connectionConfig.AddChannel(QosType.Reliable);
        ReliableOrdered = connectionConfig.AddChannel(QosType.ReliableSequenced);
        Unreliable = connectionConfig.AddChannel(QosType.Unreliable);
        UnreliableFragmented = connectionConfig.AddChannel(QosType.UnreliableFragmented);

        HostTopology hostTopology = new HostTopology(connectionConfig, 1);
        Host = NetworkTransport.AddHost(hostTopology, HostPort, null);

        if (Host.Equals(-1))
        {
            Say("Setup Error");
            Commands.Off();
            return;
        }

        Hosted = true;
        Server = NetworkTransport.Connect(Host, DestinationAddress, DestinationPort, 0, out Error);

        Say("On");
    }

    public static class Commands
    {
        public static void Off()
        {
            if (!Hosted) return;

            Hosted = false;
            Connected = false;
            Ready = false;
            Application.runInBackground = false;

            NetworkTransport.Disconnect(Host, Server, out Error);
            NetworkTransport.RemoveHost(Host);
            NetworkTransport.Shutdown();

            RecievedMessages.Clear();

            Host = -1;
            User = -1;
            Server = -1;

            Say("Off");
        }

        public static void Send(Message message, byte channel)
        {
            if (!Ready) return;

            message.User = User;

            byte[] messageBox;
            MemoryStream memoryStream = new MemoryStream();

            BinaryFormatter.Serialize(memoryStream, message);
            messageBox = memoryStream.ToArray();

            NetworkTransport.Send(Host, Server, channel, messageBox, messageBox.Length, out Error);
        }
    }

    private void Update()
    {
        if (!Hosted) return;

        CatchMessages();
        ProcessMessages();
    }
    private void CatchMessages()
    {
        int recievingHost;
        int recievingUser;
        int recievingChannel;
        int recievingMessageLength;

        NetworkEventType messageType = NetworkEventType.Nothing;
        do
        {
            messageType = NetworkTransport.Receive(out recievingHost, out recievingUser, out recievingChannel, MessageBuffer, MessageBuffer.Length, out recievingMessageLength, out Error);
            switch (messageType)
            {
                case NetworkEventType.ConnectEvent:

                    Connected = true;
                    Say("Connected");

                    break;
                case NetworkEventType.DisconnectEvent:

                    Say("Disconnected");
                    Commands.Off();

                    break;
                case NetworkEventType.DataEvent:

                    Say("Got");
                    print(recievingMessageLength);
                    AddToRecievedMessages(MessageBuffer);

                    break;
                case NetworkEventType.Nothing:
                    break;
            }
        }
        while (messageType != NetworkEventType.Nothing);
    }
    private void AddToRecievedMessages(byte[] buffer)
    {
        MemoryStream memoryStream = new MemoryStream(buffer);
        RecievedMessages.Enqueue(BinaryFormatter.Deserialize(memoryStream));
    }
    private void ProcessMessages()
    {
        if (RecievedMessages.Count.Equals(0)) return;

        for (int i = 0; i < RecievedMessages.Count; i++)
        {
            object thing = RecievedMessages.Dequeue();

            if (!(thing is Container)) return;

            Message[] messages = ((Container)thing).Messages;
            print(messages.GetType());

            for (int k = 0; k < messages.Length; k++)
            {
                if (messages[k].MessageType.Equals(Type.Assign))
                {
                    Assign((Assign)messages[k]);
                }
                else
                {
                    ClientWindow.RecievedMessage(messages[k]);
                }
            }

        }
    }
    private void Assign(Assign assign)
    {
        User = assign.User;
        Ready = true;

        Say("Ready");
    }
    private static void Say(string text)
    {
        Debug.Log(string.Format("<b>Client:</b> {0}", text));
    }
    private void OnApplicationQuit()
    {
        Commands.Off();
    }
}
