//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the broolah suit item.
    This item allows human players to put on an iron
    suit that they can fly around in and fire off
    bullets with. This item was added in for fun and
    to test different things during development, but
    I kept it in and made it super expensive so it
    was hard to get.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_Broolah : ClientHumanItem
{
    private byte bullets;
    private const byte maxBullets = 50;
    private int forceDirection;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject clientBullet;
    [SerializeField] private Transform bulletOffset;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject muzzlePFX;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip gunshot;

    private Transform cameraOffset;

    protected override void Enabled()
    {
        cameraOffset = transform.parent.parent.parent.GetChild(0);

        muzzleFlash.SetActive(false);
        RefreshAmmo();

        StartCoroutine(StartGun());
    }

    public override void Flip(bool direction)
    {
        spriteRenderer.flipX = !direction;

        if (!direction)
        {
            transform.localPosition = new Vector2(-.5f, 0);
            muzzleFlash.transform.localPosition = new Vector2(-.31f, -.038f);
            bulletOffset.localRotation = Quaternion.Euler(0, 0, 90);
            bulletOffset.localPosition = new Vector2(1f, -.08f);

            forceDirection = 1;
        }
        else
        {
            transform.localPosition = new Vector2(.5f, 0);
            muzzleFlash.transform.localPosition = new Vector2(.31f, -.038f);
            bulletOffset.localPosition = new Vector2(-1f, -.08f);
            bulletOffset.localRotation = Quaternion.Euler(0, 0, -90);

            forceDirection = -1;
        }
    }

    private IEnumerator StartGun()
    {
        Rigidbody2D clientPrefabRigidbody = transform.parent.parent.parent.GetComponent<Rigidbody2D>();

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

                    cameraOffset.localPosition = new Vector2(0, cameraOffset.localPosition.y);
                    muzzleFlash.SetActive(true);
                    Trigger(true, (byte)transform.GetSiblingIndex());
                    clientPrefabRigidbody.AddForce(new Vector2(forceDirection * 10, 50), ForceMode2D.Impulse);
                    bullets--;
                    RefreshAmmo();

                    yield return new WaitForSeconds(.025f);

                    cameraOffset.localPosition = new Vector2(1.5f * forceDirection, cameraOffset.localPosition.y);
                    muzzleFlash.SetActive(false);
                }
                else
                {

                    Refresh("Reloading...");

                    yield return new WaitForSeconds(1);
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
        cameraOffset.localPosition = new Vector2(1.5f * forceDirection, cameraOffset.localPosition.y);

        StopCoroutine(StartGun());
        muzzleFlash.SetActive(false);
    }
}
