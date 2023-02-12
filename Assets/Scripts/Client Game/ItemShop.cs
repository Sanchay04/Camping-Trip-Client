//Sanchay Ravindiran 2020

/*
    Underlays the shop by dictating whether
    or not it can be interacted with and
    handles the effects and animations that
    make it usable by the player. Also provides
    the player with helpful information at the
    top of the shop window.
*/

using System.Collections;
using UnityEngine;
using TMPro;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI moneyUI;

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(ShowProcess());
    }

    private IEnumerator ShowProcess()
    {
        moneyUI.enabled = true;
        RefreshMoneyUI();

        for (float f = 0; f < 1; f += .1f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        yield break;
    }

    public void Hide()
    {
        moneyUI.enabled = false;

        StopAllCoroutines();
        StartCoroutine(HideProcess());
    }

    public void RefreshMoneyUI()
    {
        float money = ClientHumanPrefab.Money;

        if (money.Equals(0))
        {
            moneyUI.text = string.Format("${0} - <i>you're broke; survive the beast for a few minutes or kill him to get a paycheck</i>", money);
        }
        else
        {
            moneyUI.text = "$" + money + " <-- welcome to favorites voting. you have $2 million so you can try out all the weapons";
        }
    }

    private IEnumerator HideProcess()
    {
        for (float f = 1; f > 0; f -= .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        RefreshMoneyUI();

        yield break;
    }
}
