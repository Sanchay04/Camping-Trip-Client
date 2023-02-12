//Sanchay Ravindiran 2020

/*
    This class handles the animations and effects of
    a notice board, or a small portion of the viewport
    dedicated to delivering the player with important
    information in a concise manner. The type of a notice
    is characterized by its color - green notices are good,
    red notices are bad, and white notices are neutral.
*/

using System.Collections;
using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
    public int Distance;
    private Vector2 OriginalPosition;

    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] Color BadMessageColor;
    [SerializeField] Color GoodMessageColor;

    private void Start()
    {
        OriginalPosition = transform.localPosition;
        TextComponent.color = new Color(1, 1, 1, 0);
    }

    public void Say(string message, byte type)
    {
        ResetAnimation();
        StartCoroutine(Animate(message, type, true));
    }

    private void ResetAnimation()
    {
        StopAllCoroutines();
        transform.localPosition = new Vector2(OriginalPosition.x - Distance, 0);
        TextComponent.color = new Color(TextComponent.color.r, TextComponent.color.g, TextComponent.color.b, 0);
    }

    private IEnumerator Animate(string message, byte type, bool fade)
    {
        Vector2 leftPosition = new Vector2(OriginalPosition.x - Distance, OriginalPosition.y);
        Vector2 rightPosition = new Vector2(OriginalPosition.x + Distance, OriginalPosition.y);

        switch (type)
        {
            case 1: //good
                TextComponent.color = GoodMessageColor;
                break;
            case 2: //bad
                TextComponent.color = BadMessageColor;
                break;
            case 3: //neutral
                TextComponent.color = new Color(1, 1, 1, 0);
                break;
        }

        Color textColor = TextComponent.color;

        TextComponent.text = message;
        transform.localPosition = leftPosition;

        TextComponent.color = new Color(textColor.r, textColor.g, textColor.b, 0);
        float i1 = 0;

        for (float f = 0; f <= 1; f += .02f)
        {
            i1 += .05f;
            TextComponent.color = new Color(textColor.r, textColor.g, textColor.b, i1);
            transform.localPosition = Vector2.Lerp(transform.localPosition, OriginalPosition, 7 * Time.deltaTime);
            yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(.1f);
        float i2 = 1;

        for (float f = 1; f >= 0; f -= .02f)
        {
            i2 -= .1f;
            TextComponent.color = new Color(textColor.r, textColor.g, textColor.b, i2);
            transform.localPosition = Vector2.Lerp(transform.localPosition, rightPosition, 5 * Time.deltaTime);
            yield return new WaitForSeconds(.01f);
        }

        TextComponent.color = new Color(textColor.r, textColor.g, textColor.b, 0);

        yield break;
    }
}
