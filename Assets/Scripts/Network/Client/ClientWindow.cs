//Sanchay Ravindiran 2020

/*
    While the client class is responsible for abstracting the sending and
    receiving of packets, the client window class decides what to do with
    the packets that are received and sends its own packets accordingly
    based on procedures defined for each kind of message and the circumstance
    of the game. In short, this class handles the messages received from
    the game server and translates them into events that occur in the game.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
[RequireComponent(typeof(Client))]
public class ClientWindow : MonoBehaviour
{
    [SerializeField] public static string Name;
    [Space]

    [SerializeField] private GameObject ClientHumanPrefab;
    [SerializeField] private GameObject NetHumanPrefab;
    [SerializeField] private GameObject ClientBeastPrefab;
    [SerializeField] private GameObject NetBeastPrefab;

    [Space]
    [SerializeField] private GameObject HumanCorpseAnimationPrefab;
    [SerializeField] private GameObject BeastCorpseAnimationPrefab;

    [Space]
    [SerializeField] private BeastDeath BeastDeath;

    //[SerializeField] private

    [Space]
    [SerializeField] private GameObject[] NetPrefabItems; //REFER TO ItemTypes() in ClientPrefab

    [Space]
    [SerializeField] private IntroductionDisplayController IntroductionDisplay;

    public static class ItemTypes //Correspond to NetPrefab index in NetPrefabItems[]
    {
        //Humans
        public const byte HumanLantern = 0;
        public const byte HumanHuntingRifle = 1;
        public const byte HumanAerialRifle = 2;
        public const byte HumanGlock = 3;
        public const byte HumanBroolahsuit = 4;
        public const byte HumanDevgun = 5;
        //Beast
        public const byte BeastNone = 6;
        public const byte BeastRockTransform = 7;
    }

    private static float ZIncrement;

    public static Dictionary<int, NetPrefab> NetPlayers = new Dictionary<int, NetPrefab>();
    [SerializeField] private Client clientInstance;
    [SerializeField] private Notice noticeInstance;

    public static int TotalDamageBeforeDeath;

    public static readonly Vector2 DeadRoom = new Vector2(-9999, -9999);
    public static readonly Vector2 HumanSpawnPosition = new Vector2(6, 7);
    public static readonly Vector2 BeastSpawnPosition = new Vector2(-10, 20);

    private IEnumerator Start()
    {
        noticeInstance.Say("YEEE", 0);

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Application.runInBackground = true;
        IntroductionDisplay.Hide();
        Instantiate(ClientHumanPrefab, HumanSpawnPosition, Quaternion.identity);
        yield break;
    }

    public void RecievedMessage(Message message)
    {
        switch (message.MessageType)
        {
            case Type.PlayerSpawn:
                InstantiateAndAddPlayerPrefabToReference(message as PlayerSpawn);
                break;
            case Type.Destroy:
                int recUser = message.User;
                NetPrefab user = NetPlayers[recUser];
                if (user.Human)
                {
                    Instantiate(HumanCorpseAnimationPrefab, user.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(BeastCorpseAnimationPrefab, user.transform.position, Quaternion.identity);
                    BeastDeath.Show(user.Name);
                }
                Destroy(user.gameObject);
                NetPlayers.Remove(recUser);
                break;
            case Type.PlayerState:
                UpdateNetPlayerState(message as PlayerState);
                break;
            case Type.PlayerItemAdd:
                AddNetPlayerItem(message as PlayerItemAdd);
                break;
            case Type.PlayerItemEquip:
                EquipNetPlayerItem(message as PlayerItemEquip);
                break;
            case Type.PlayerItemTrigger:
                TriggerNetPlayerItem(message as PlayerItemTrigger);
                break;
            case Type.PlayerItemClear:
                ClearNetPlayerItem(message as PlayerItemClear);
                break;
            case Type.PlayerBeastGrab:
                GotGrabbed(message as PlayerBeastGrab);
                break;
            case Type.PlayerDamage:
                DamageNetPlayer(message as PlayerDamage);
                break;
            case Type.PlayerDeath:
                KillNetPlayer(message as PlayerDeath);
                break;
            case Type.PlayerRevive:
                ReviveNetPlayer(message as PlayerRevive);
                break;
            case Type.PlayerTransform:
                TransformNetPlayerAtRuntime(message as PlayerTransform);
                break;
            default:
                Debug.Log("Unhandled network message: " + message.MessageType);
                break;
        }
    }

    #region Client/Net Prefab instantiation
    private void InstantiateAndAddPlayerPrefabToReference(PlayerSpawn recPlayerSpawn)
    {
        if (NetPlayers.ContainsKey(recPlayerSpawn.User)) return;

        Vector3 recPlayerPosition = GetDistortionFreePosition(new Vector2(recPlayerSpawn.X, recPlayerSpawn.Y));
        bool recIsHuman = recPlayerSpawn.Human;
        int recUser = recPlayerSpawn.User;

        if (recUser.Equals(Client.User)) //if own player
        {
            if (GetClientGameObject() == null)
            {
                IntroductionDisplay.Hide();
                if (recIsHuman)
                {
                    Instantiate(ClientHumanPrefab, HumanSpawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(ClientBeastPrefab, BeastSpawnPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            GameObject prefab;

            if (recIsHuman)
            {
                prefab = NetHumanPrefab;
            }
            else
            {
                prefab = NetBeastPrefab;
            }

            NetPrefab netPrefab = Instantiate(prefab, recPlayerPosition, Quaternion.identity).GetComponent<NetPrefab>();
            netPrefab.Name = recPlayerSpawn.Name;
            netPrefab.Human = recIsHuman;
            netPrefab.User = recUser;
            netPrefab.name = GetNetPrefabName(recUser);

            if (recPlayerSpawn.Items != null)
            {
                for (int i = 0; i < recPlayerSpawn.Items.Length; i++)
                {
                    PlayerItem item = recPlayerSpawn.Items[i];
                    Transform addedItem = Instantiate(NetPrefabItems[item.ItemID], netPrefab.ItemOffset, false).transform;

                    addedItem.localPosition = GetDistortionFreePosition(Vector2.zero);
                    addedItem.SetSiblingIndex(item.ItemIndex);
                }
                netPrefab.EquipItem(recPlayerSpawn.EquippedItemIndex);
            }

            NetPlayers.Add(recUser, netPrefab);
            netPrefab.Sync = true;
        }
    }
    #endregion

    #region No Clientside Involvement

    private void UpdateNetPlayerState(PlayerState state) //no clientside involvement
    {
        if (state.User.Equals(Client.User) || !NetPlayers.ContainsKey(state.User)) return;

        NetPlayers[state.User].UpdateState(state);
    }

    private void AddNetPlayerItem(PlayerItemAdd itemAdd) //no clientside involvement
    {
        if (itemAdd.User.Equals(Client.User)) return;

        NetPrefab netPrefab = NetPlayers[itemAdd.User];
        Transform newItem = Instantiate(NetPrefabItems[itemAdd.ItemID], netPrefab.ItemOffset, false).transform;

        newItem.transform.localPosition = GetDistortionFreePosition(Vector2.zero);
        netPrefab.EquipItem((byte)newItem.GetSiblingIndex());
    }

    private void EquipNetPlayerItem(PlayerItemEquip itemEquip) //no clientside involvement
    {
        if (itemEquip.User.Equals(Client.User)) return;

        NetPlayers[itemEquip.User].EquipItem(itemEquip.ItemIndex);
    }

    private void TriggerNetPlayerItem(PlayerItemTrigger itemTrigger) //no clientside involvement
    {
        if (itemTrigger.User.Equals(Client.User)) return;

        NetPlayers[itemTrigger.User].TriggerItem(itemTrigger.ItemTrigger, itemTrigger.ItemIndex);
    }

    private void ClearNetPlayerItem(PlayerItemClear itemClear)
    {
        if (itemClear.User.Equals(Client.User)) return;

        foreach (GameObject item in NetPlayers[itemClear.User].ItemOffset)
        {
            Destroy(item.gameObject);
        }
    }

    private void KillNetPlayer(PlayerDeath playerDeath)
    {
        if (playerDeath.User.Equals(Client.User)) return;

        NetPrefab deadPlayer = NetPlayers[playerDeath.User];
        GameObject corpsePrefab;
        deadPlayer.Sync = false;

        if (deadPlayer.Human)
        {
            corpsePrefab = HumanCorpseAnimationPrefab;
        }
        else
        {
            corpsePrefab = BeastCorpseAnimationPrefab;
            BeastDeath.Show(deadPlayer.Name);
        }

        Instantiate(corpsePrefab, GetDistortionFreePosition(deadPlayer.transform.position), Quaternion.identity);

        foreach (Transform child in deadPlayer.ItemOffset.transform)
        {
            Destroy(child.gameObject);
        }

        deadPlayer.transform.position = DeadRoom;
        deadPlayer.gameObject.SetActive(false);
    }

    #endregion

    #region Clientside Involvement (Messages can apply to client)

    private void GotGrabbed(PlayerBeastGrab beastGrab)
    {
        if (beastGrab.GrabbedUser.Equals(Client.User))
        {
            (GetClientPrefabScript() as ClientHumanPrefab).Grab(((NetBeastPrefab)NetPlayers[beastGrab.GrabberUser]).Restrainer);
        }
    }

    private void DamageNetPlayer(PlayerDamage playerDamage)
    {
        if (playerDamage.TargetUser.Equals(Client.User))
        {
            GetClientPrefabScript().Damage(playerDamage.Amount, playerDamage.User);
            return;
        }

        NetPlayers[playerDamage.TargetUser].Damage(playerDamage.Amount);
    }

    private void TransformNetPlayerAtRuntime(PlayerTransform playerTransformation)
    {
        return;

        bool recIsHuman = playerTransformation.Human;
        int recUser = playerTransformation.User;

        print("Calledtransform");
        if (recUser.Equals(Client.User))
        {
            if (recIsHuman)
            {
                Destroy(GetClientGameObject().gameObject);

                print("Calledtransform1");
                Instantiate(ClientHumanPrefab, HumanSpawnPosition, Quaternion.identity);
                CameraController.Target(GetClientGameObject().transform);

            }
            else
            {
                Destroy(GetClientGameObject().gameObject);

                print("Calledtransform2");
                Instantiate(ClientBeastPrefab, BeastSpawnPosition, Quaternion.identity);
                CameraController.Target(GetClientGameObject().transform);
            }
        }
        else
        {
            print(recUser + " - " + Client.User);
            GameObject prefab;
            string previousName = NetPlayers[recUser].Name;

            Destroy(NetPlayers[recUser].gameObject);

            if (recIsHuman)
            {
                prefab = NetHumanPrefab;
            }
            else
            {
                prefab = NetBeastPrefab;
            }

            NetPrefab replacementPrefab = Instantiate(prefab, DeadRoom, Quaternion.identity).GetComponent<NetPrefab>();
            replacementPrefab.Name = previousName;
            replacementPrefab.Human = recIsHuman;
            replacementPrefab.User = recUser;
            replacementPrefab.name = GetNetPrefabName(recUser);
            replacementPrefab.Sync = true;

            NetPlayers[recUser] = replacementPrefab;
        }
    }

    private void ReviveNetPlayer(PlayerRevive playerRevival)
    {
        print("Attempted revival");
        if (playerRevival.User.Equals(Client.User))
        {
            GetClientPrefabScript().Revive();
            return;
        }

        NetPrefab netPrefab = NetPlayers[playerRevival.User];
        Transform netPrefabTransform = netPrefab.transform;

        netPrefab.Sync = true;
        netPrefab.gameObject.SetActive(true);
        netPrefabTransform.eulerAngles = Vector3.zero;
        netPrefabTransform.GetChild(0).eulerAngles = Vector3.zero;
    }

    #endregion

    public static Vector3 GetDistortionFreePosition(Vector2 position)
    {
        return new Vector3(position.x, position.y, ZIncrement += .0001f);
    }

    private static string GetNetPrefabName(int recUser)
    {
        return "[SERVER]#<b>" + recUser + "</b>:" + System.DateTime.Now;
    }

    private static GameObject GetClientGameObject()
    {
        return GameObject.FindWithTag("ClientPrefab");
    }

    private static ClientPrefab GetClientPrefabScript()
    {
        return GameObject.FindWithTag("ClientPrefab").GetComponent<ClientPrefab>();
    }
}
