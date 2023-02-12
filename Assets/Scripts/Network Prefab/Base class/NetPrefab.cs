//Sanchay Ravindiran 2020

/*
    A base class that is extended by any classes
    attached to objects that act as serverside
    prefabs in the game. These are prefabs that
    exist solely to mimic the appearances and states
    of other players connected to the game server.
    Like the network human item, the network prefab
    can be thought of as a ghost prefab that is used
    to maintain the illusion of a persistent virtual
    environment for players.
*/

using UnityEngine;

public class NetPrefab : MonoBehaviour
{
    [Header("[NET] Inherited")]
    public bool Sync; //Disable when dead; (Disable(), teleport to dead room, wait for Revive()/ Destroy prefab and replace with another))
    public bool Human;
    public int User;
    public string Name;

    [SerializeField] public Transform ItemOffset;

    private Vector2 PreviousTargetPosition;
    private Vector2 CurrentTargetPosition;

    //private Shader

    public void UpdateState(PlayerState playerState)
    {
        if (!Sync) return;

        PreviousTargetPosition = CurrentTargetPosition;
        CurrentTargetPosition = new Vector2(playerState.X, playerState.Y);

        if (Vector2.Distance(PreviousTargetPosition, CurrentTargetPosition) > 5)
        {
            transform.position = CurrentTargetPosition;
        }

        UpdateAdditionalState(playerState);
    }

    public virtual void Damage(byte amount)
    {

    }

    public virtual void EquipItem(byte itemIndex)
    {

    }

    public virtual void TriggerItem(byte itemIndex, bool trigger)
    {

    }

    protected void Update()
    {
        if (!Sync) return;

        transform.position = Vector2.Lerp(transform.position, CurrentTargetPosition, 15 * Time.deltaTime);
    }

    public void TriggerItem(bool trigger, byte index)
    {
        if (!Sync) return;

        ItemOffset.GetChild(index).GetComponent<NetHumanItem>().Trigger(trigger);
    }

    protected virtual void UpdateAdditionalState(PlayerState playerState)
    {

    }
}
