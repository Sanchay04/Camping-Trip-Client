//Sanchay Ravindiran 2020

/*
    Makes a window appear that prompts
    the player to enter their own name
    that other players connected to the
    same server will be able to see. Prevents
    specific phrases and characters from being
    used in the player names and handles the
    animations and effects involved within the
    window.
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class NameCustomization : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainGroup;

    [SerializeField] private TMP_InputField input;
    [SerializeField] private Notice notice;

    private string[] bannedTerms =
    {
        "PLAY",
        "ADMIN",
        "JAY",
        "$AN",
        "<",
        ">",
        "/",
        " "
    };

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        for (float f = 0; f < 1; f += .05f)
        {
            mainGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        mainGroup.alpha = 1;

        yield break;
    }

    public void SetName()
    {
        string inputText = input.text;

        if (string.IsNullOrEmpty(inputText))
        {
            notice.Say("please enter a name.", 2);
            return;
        }
        else
        {
            for (int i = 0; i < bannedTerms.Length; i++)
            {
                if (inputText.ToUpper().Contains(bannedTerms[i]))
                {
                    notice.Say(string.Format("term <i>{0}</i>  banned", bannedTerms[i]), 2);
                    return;
                }
            }
        }

        ClientWindow.Name = inputText;
        notice.Say(string.Format("name updated to <i>{0}</i>", inputText), 1);
    }

    public void PressedPlay()
    {
        if (string.IsNullOrEmpty(ClientWindow.Name))
        {
            notice.Say("please enter a name.", 2);
            return;
        }

        SceneManager.LoadScene(2);
    }
}
