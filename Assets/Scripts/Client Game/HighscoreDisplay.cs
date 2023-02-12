//Sanchay Ravindiran 2020

/*
    Displays the highscores of all players who
    are currently participating in the arcade
    when the player comes into contact with the
    radius surrounding the current object. The
    player with the most ingame currency is listed
    at the top of the leaderboard as the richest
    hunter.
*/

using UnityEngine;
using TMPro;

public class HighscoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro displayText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        displayText.text = string.Empty;
        Entry[] entries = File.Score(ClientWindow.Name, ClientHumanPrefab.Money);

        print(entries.Length);

        for (int i = entries.Length; i-- > 0;)
        {
            if (i == 9)
            {
                print("g");
                displayText.text = displayText.text + string.Format("\n<color=#FFFFFF><i>{0}</i> - ${1}</color>\nrichest hunter\n", entries[i].name, entries[i].score);
            }
            else
            {
                displayText.text = displayText.text + string.Format("\n<i>{0}</i> - ${1}", entries[i].name, entries[i].score);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientPrefab"))
        {
            displayText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("ClientPrefab"))
        {
            displayText.enabled = false;
        }
    }
}
