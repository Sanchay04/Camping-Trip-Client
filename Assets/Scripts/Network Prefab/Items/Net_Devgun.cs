//Sanchay Ravindiran 2020

/*
    Extends network human item and implements functionality
    specific to the network devgun item. This item mimics the
    appearance and state of a client devgun item operated by
    another player connected to the game server.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net_Devgun : NetHumanItem
{
    [SerializeField] private GameObject NetBullet;
    [SerializeField] private Transform BulletOffset;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private GameObject MuzzlePFX;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    private void OnEnable()
    {
        MuzzleFlash.SetActive(false);
    }

    public override void Flip(bool direction)
    {
        SpriteRenderer.flipX = !direction;
        if (direction)
        {
            MuzzleFlash.transform.localPosition = new Vector2(1.7f, .1f);
            BulletOffset.localPosition = new Vector2(2f, .1f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            MuzzleFlash.transform.localPosition = new Vector2(-1.7f, .1f);
            BulletOffset.localPosition = new Vector2(-2f, .1f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
        yield return new WaitForSeconds(.05f);
        MuzzleFlash.SetActive(false);
    }
}
