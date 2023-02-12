//Sanchay Ravindiran 2020

/*
    Smoothly fades in an introductory display that
    contains the title of the game, its description
    and other helpful information. This information
    includes game controls for both beast and human
    players as well as headphone recommendations.
*/

using UnityEngine;
using TMPro;

public class IntroductionDisplayController : MonoBehaviour
{
    [SerializeField] private CanvasGroup title;
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private TextMeshProUGUI controls;
    [SerializeField] private TextMeshProUGUI sound;

    private Vector3 controlsInitial;
    private Vector3 soundInitial;
    private float lerpInitial;
    private bool lerping;

    private void Start()
    {
        title.alpha = 1;
        info.color = Color.white;
        controls.color = Color.white;
        controlsInitial = controls.transform.localPosition;
        soundInitial = sound.transform.localPosition;
    }

    void Update()
    {
        if (lerping)
        {
            float t = (Time.time - lerpInitial) / 1f;

            Color fade = Color.white;
            fade.a = Mathf.Lerp(1, 0, t);
            title.alpha = Mathf.Lerp(1, 0, t);
            info.color = fade;
            controls.color = fade;
            sound.color = fade;

            if (t >= 1)
            {
                title.gameObject.SetActive(false);
                info.gameObject.SetActive(false);
                controls.gameObject.SetActive(false);
                sound.gameObject.SetActive(false);
                transform.gameObject.SetActive(false);
                return;
            }
        }

        Color color = info.color;
        color.a = (Mathf.Sin(Time.time * 5f) / 2f) + 1f / 2f;
        info.color = color;

        Vector3 vector1 = controlsInitial;
        Vector3 vector2 = soundInitial;
        vector1.y += Mathf.Sin(Time.time * 2f) * 8f;
        vector2.y += Mathf.Sin(Time.time * 2f + 10f) * 8f;
        controls.transform.localPosition = vector1;
        sound.transform.localPosition = vector2;
    }

    public void Hide()
    {
        lerpInitial = Time.time;
        lerping = true;
    }
}
