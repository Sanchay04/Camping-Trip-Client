//Sanchay Ravindiran 2020

/*
    Provides player with different pieces of information
    that form a death screen once their character dies
    in the game. This screen is comprised of a randomly
    chosen tip depending on whether the player is human
    or beast, information regarding the circumstances
    under which the player lost, and the income they had
    accumulated from their gameplay up to that point.
*/

using System.Collections;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI assailantText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private HighscoreDisplay highscoreDisplay;

    [SerializeField] private CanvasGroup canvasGroup;

    private string[] humanTips =
    {
        "<b>tip 1/5:</b> having trouble surviving your trip? try to travel in groups!",
        "<b>tip 2/5:</b> struggling to track the beast? try following its smell!",
        "<b>tip 3/5:</b> make sure you conserve lantern light on your trip so that it doesn't burn out!",
        "<b>tip 4/5:</b> go to sonche's home camp r.v. to buy gear!",
        "<b>tip 5/5:</b> you can get free lanterns in the left hand side of home camp."
    };

    private string[] beastTips =
    {
        "<b>tenet 1/5:</b> use trees to ambush the pickings.",
        "<b>tenet 2/5:</b> pick off stray pickings one by one and try not to approach a group of them.",
        "<b>tenet 3/5:</b> use your items to hide in plain sight of the pickings.",
        "<b>tenet 4/5:</b> you have night vision, the pickings do not.",
        "<b>tenet 5/5:</b> the pickings' camp is ripe with pickings.",
    };

    public void Show(string assailant, string name, int damage, bool human)
    {
        highscoreDisplay.Refresh();

        if (human)
        {
            assailantText.text = string.Format("you got <color=#FF004D>crushed</color> by Beezle <i>{0}</i>", assailant);
            damageText.text = string.Format("great job <i>{0}</i>, you got <color=#FF004D>{1}</color> damage on him; <i>(that's ${2})</i>", name, damage, damage * 25);
            tipText.text = humanTips[Random.Range(0, 4)];
        }
        else
        {
            assailantText.text = string.Format("you got <color=#FF004D>hunted down</color> by picking <i>{0}</i>", assailant);
            damageText.text = string.Format("brother <i>{0}</i>, you drained <color=#FF004D>{1}</color> pickings; <i>(that's ${2} in picking currency)</i>", name, damage/5, (damage/5) * 50);
            tipText.text = beastTips[Random.Range(0, 4)];
        }

        StopCoroutine(HideProcess());
        StartCoroutine(ShowProcess());
    }

    public void Hide()
    {
        StopCoroutine(ShowProcess());
        StartCoroutine(HideProcess());
    }

    private IEnumerator ShowProcess()
    {
        for (float f = 0; f < 1; f += .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;

        yield break;
    }

    private IEnumerator HideProcess()
    {
        canvasGroup.interactable = false;

        for (float f = 1; f > 0; f -= .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 0;

        yield break;
    }
}
