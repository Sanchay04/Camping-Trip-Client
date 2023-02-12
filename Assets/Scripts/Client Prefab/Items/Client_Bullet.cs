//Sanchay Ravindiran 2020

/*
    The client bullet is a clientside prefab that
    flies in a provided direction and sends a
    message to the game server if it happens to
    make contact with a network beast prefab. This
    allows the beast player connected to the game
    server to take damage under the client authoritative
    networking model.
*/

using UnityEngine;

public class Client_Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("NetBeastPrefab"))
        {
            ClientWindow.TotalDamageBeforeDeath += 1;
            PlayerDamage playerDamage = new PlayerDamage
            {
                Amount = 1,
                TargetUser = collision.GetComponent<NetBeastPrefab>().User
            };
            Client.Commands.Send(playerDamage, Client.Reliable);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Translate(transform.up * 2f, Space.World);
    }
}
