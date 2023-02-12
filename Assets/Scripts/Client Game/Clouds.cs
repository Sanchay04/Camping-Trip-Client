//Sanchay Ravindiran 2020

/*
    Moves clouds slowly across the background of the
    viewport and into its edge before wrapping them
    back around to make it look like more clouds seamlesly
    emerge.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float newPos = Mathf.Repeat(Time.time * -1f, 20);
        transform.localPosition = startPosition + Vector2.right * newPos;
    }
}
