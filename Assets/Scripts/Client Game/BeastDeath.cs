//Sanchay Ravindiran 2020

/*
  Informs player that the beast has been hunted down
  by fading in an announcement that occupies the center
  of the viewport before fading it out.
*/

using System.Collections;
using UnityEngine;
using TMPro;

public class BeastDeath : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI header;

    public void Show(string beastName)
    {
        header.enabled = true;
        header.text = "beezle <i>" + beastName + "</i> has been <color=#FF004D>hunted</color>";

        StartCoroutine(ShowProcess());
    }

    private IEnumerator ShowProcess()
    {
        float f;

        for (f = 0; f < 1; f += .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(1.5f);

        for (f = 1; f > 0; f -= .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 0;

        header.enabled = false;
    }
}
