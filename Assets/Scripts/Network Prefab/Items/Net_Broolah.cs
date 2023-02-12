//Sanchay Ravindiran 2020

/*
    Extends network human item and implements functionality
    specific to the network broolah suit item. This item
    mimics the appearance and state of a client broolah
    suit item operated by another player connected to the
    game server.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net_Broolah : NetHumanItem
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private GameObject NetBullet;
    [SerializeField] private Transform BulletOffset;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private GameObject MuzzlePFX;

    private void OnEnable()
    {
        MuzzleFlash.SetActive(false);
    }

    public override void Trigger(bool trigger)
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        Instantiate(NetBullet, BulletOffset.position, BulletOffset.rotation);
        Instantiate(MuzzlePFX, BulletOffset.position, Quaternion.identity);
        MuzzleFlash.SetActive(true);

        yield return new WaitForSeconds(.025f);

        MuzzleFlash.SetActive(false);
    }

    public override void Flip(bool direction)
    {
        SpriteRenderer.flipX = !direction;

        if (!direction)
        {
            transform.localPosition = new Vector2(-.5f, 0);
            MuzzleFlash.transform.localPosition = new Vector2(-.31f, -.038f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, 90);
            BulletOffset.localPosition = new Vector2(1f, -.08f);
        }
        else
        {
            transform.localPosition = new Vector2(.5f, 0);
            MuzzleFlash.transform.localPosition = new Vector2(.31f, -.038f);
            BulletOffset.localPosition = new Vector2(-1f, -.08f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, -90);
        }
    }
}
