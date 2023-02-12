//Sanchay Ravindiran 2020

/*
    Extends network human item and implements
    functionality specific to the network aerial rifle
    item. This item mimics the appearance and state of
    a client aerial rifle item operated by another player
    connected to the game server.

    IMPLEMENTATION NOTE:

    My bad again, like the classes that extend the
    client human item, this class and the following
    classes consist of a lot of a duplicated code because
    I did not think about making a network human gun
    class to reduce redundancy. At this point I was running
    out of time, and I was adding functionality at the
    last moment and haphazardly implementing things using
    concepts I was learning in my own time.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net_AerialRifle : NetHumanItem
{
    [Header("Components")]
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
        if (!direction)
        {
            MuzzleFlash.transform.localPosition = new Vector2(.102f, 1.21f);
            BulletOffset.localPosition = new Vector2(.102f, 1.691f);
        }
        else
        {
            MuzzleFlash.transform.localPosition = new Vector2(-0.095f, 1.21f);
            BulletOffset.localPosition = new Vector2(-.103f, 1.691f);
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
