//Sanchay Ravindiran 2020

/*
    Shows the player my logo before
    they enter the game and performs
    the effects and animations involved
    in doing this.
*/

using System.Collections;
using UnityEngine;

public class Logos : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Color nameSceneColor;

    private IEnumerator Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        Camera camera = Camera.main;

        for (float f = 0; f < 1; f += .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(.5f);

        for (float f = 1; f > 0; f -= .05f)
        {
            canvasGroup.alpha = f;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = 0;

        while (camera.backgroundColor != nameSceneColor)
        {
            camera.backgroundColor = Color.Lerp(camera.backgroundColor, nameSceneColor, Time.deltaTime * 7f);
            yield return new WaitForEndOfFrame();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
