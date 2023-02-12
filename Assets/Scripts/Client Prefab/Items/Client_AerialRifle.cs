//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the aerial rifle
    item. This item fires off several bullets in
    quick succession upwards, and it is made to
    allow human hunters to do immense damage against
    beasts that climb the trees above them.

    IMPLEMENTATION NOTE:

    My bad, this class and the following classes
    that extend client human item are mostly made of
    duplicated code because I did not think about
    making another client human gun class and then
    inheriting from that class to reduce bloat. At
    the time when I made this project I was just
    learning about inheritance on my own time, so
    this idea ended up coming to me at the last second.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_AerialRifle : ClientHumanItem
{
    private byte bullets;
    private const byte maxBullets = 20;

    [SerializeField] private GameObject clientBullet;
    [SerializeField] private Transform bulletOffset;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject muzzlePFX;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip gunshot;

    private Transform cameraOffset;

    protected override void Enabled()
    {
        cameraOffset = transform.parent.parent.parent.GetChild(0);
        cameraOffset.localPosition = new Vector2(cameraOffset.localPosition.x, 4);
        Camera.main.orthographicSize = 13;

        muzzleFlash.SetActive(false);
        RefreshAmmo();
        StartCoroutine(StartGun());
    }

    public override void Flip(bool direction)
    {
        spriteRenderer.flipX = !direction;

        if (!direction)
        {
            muzzleFlash.transform.localPosition = new Vector2(.102f, 1.21f);
            bulletOffset.localPosition = new Vector2(.102f, 1.691f);
        }
        else
        {
            muzzleFlash.transform.localPosition = new Vector2(-0.095f, 1.21f);
            bulletOffset.localPosition = new Vector2(-.103f, 1.691f);
        }
    }

    private IEnumerator StartGun()
    {
        for (; ; )
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (Input.GetKey(KeyCode.Space))
            {
                if (!bullets.Equals(0))
                {
                    Instantiate(clientBullet, bulletOffset.position, bulletOffset.rotation);
                    Instantiate(muzzlePFX, bulletOffset.position, Quaternion.identity);

                    audio.PlayOneShot(gunshot);
                    cameraOffset.localPosition = new Vector2(cameraOffset.localPosition.x, 1.5f);
                    muzzleFlash.SetActive(true);

                    Trigger(true, (byte)transform.GetSiblingIndex());;

                    bullets--;
                    RefreshAmmo();

                    yield return new WaitForSeconds(.025f);

                    cameraOffset.localPosition = new Vector2(cameraOffset.localPosition.x, 4f);
                    muzzleFlash.SetActive(false);
                }
                else
                {

                    Refresh("Reloading...");

                    yield return new WaitForSeconds(5);

                    bullets = maxBullets;
                    RefreshAmmo();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void RefreshAmmo()
    {
        Refresh(bullets + "/" + maxBullets + " (full auto)");
    }

    protected override void Disabled()
    {
        StopCoroutine(StartGun());
        muzzleFlash.SetActive(false);

        cameraOffset.localPosition = new Vector2(cameraOffset.localPosition.x, 1.5f);
        Camera.main.orthographicSize = 11;
    }
}
