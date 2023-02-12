//Sanchay Ravindiran 2020

/*
    Provides the player with a lantern when
    the player comes into contact with the
    radius surrounding the current object.
    This lantern is added to the player's
    item list.
*/

using UnityEngine;

public class FreeLanterns : MonoBehaviour
{
    [SerializeField] private GameObject lantern;
    [SerializeField] private Notice notice

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientPrefab"))
        {
            if (collision.GetComponent<ClientHumanPrefab>())
            {
                collision.GetComponent<ClientHumanPrefab>().AddItem(lantern, ClientWindow.ItemTypes.HumanLantern);

                notice.Say("got a lantern", 1);
            }
        }
    }
}
