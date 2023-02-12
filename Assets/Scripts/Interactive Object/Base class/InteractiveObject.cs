//Sanchay Ravindiran 2020

/*
    This base class may be extended by other
    classes that need to be attached to interactive
    objects, or objects in the game that need to
    do something to any given player's item when it
    comes into contact with the object.
*/

using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected Notice Notice;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientItem"))
        {
            ClientTouched(collision.GetComponent<ClientHumanItem>());
        }
    }

    protected virtual void ClientTouched(ClientHumanItem clientItem)
    {

    }
}
