//Sanchay Ravindiran 2020

/*
    A network bullet is created the moment
    another player fires a client bullet. This
    network bullet goes in the same direction
    of the client bullet at the same speed.
    However, it doesn't actually affect anything
    on the player's game.
*/

using UnityEngine;

public class Net_Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("NetBeastPrefab"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Translate(transform.up * 2f, Space.World);
    }
}
