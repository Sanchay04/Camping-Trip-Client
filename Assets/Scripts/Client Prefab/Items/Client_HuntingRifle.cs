//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the client hunting rifle item.
    This item is used by human players to hunt down the
    beast, and it does a considerable amount of damage
    to the beast at the expense of a long cooldown and
    higher recoil.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_HuntingRifle : ClientHumanItem
{
    private byte Bullets;
    private const byte MaxBullets = 10;
    private int forceDirection;

    [SerializeField] private GameObject ClientBullet;
    [SerializeField] private Transform BulletOffset;

    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private GameObject MuzzlePFX;

    [SerializeField] private SpriteRenderer SpriteRenderer;

    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioClip Gunshot;

    private Transform CameraOffset;

    protected override void Enabled()
    {
        CameraOffset = transform.parent.parent.parent.GetChild(0);

        MuzzleFlash.SetActive(false);
        RefreshAmmo();
        StartCoroutine(StartGun());
    }

    public override void Flip(bool direction)
    {
        SpriteRenderer.flipX = !direction;
        if (direction)
        {
            MuzzleFlash.transform.localPosition = new Vector2(1.7f, .1f);
            BulletOffset.localPosition = new Vector2(2f, .1f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, -90);

            forceDirection = -1;
        }
        else
        {
            MuzzleFlash.transform.localPosition = new Vector2(-1.7f, .1f);
            BulletOffset.localPosition = new Vector2(-2f, .1f);
            BulletOffset.localRotation = Quaternion.Euler(0, 0, 90);

            forceDirection = 1;
        }
    }

    private IEnumerator StartGun()
    {
        Rigidbody2D clientPrefabRigidbody = transform.parent.parent.parent.GetComponent<Rigidbody2D>();
        for (; ; )
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!Bullets.Equals(0))
                {
                    Instantiate(ClientBullet, BulletOffset.position, BulletOffset.rotation);
                    Instantiate(MuzzlePFX, BulletOffset.position, Quaternion.identity);
                    Audio.PlayOneShot(Gunshot);

                    CameraOffset.localPosition = new Vector2(0, CameraOffset.localPosition.y);
                    clientPrefabRigidbody.AddForce(new Vector2(forceDirection * 100, 100), ForceMode2D.Impulse);
                    Bullets--;
                    RefreshAmmo();
                    MuzzleFlash.SetActive(true);

                    yield return new WaitForSeconds(.05f);

                    MuzzleFlash.SetActive(false);
                    CameraOffset.localPosition = new Vector2(1.5f * -forceDirection, CameraOffset.localPosition.y);
                    Trigger(true, (byte)transform.GetSiblingIndex());

                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    Refresh("Reloading...");

                    yield return new WaitForSeconds(2);
                    Bullets = MaxBullets;
                    RefreshAmmo();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void RefreshAmmo()
    {
        Refresh(Bullets + "/" + MaxBullets + " (bolt action)");
    }

    protected override void Disabled()
    {
        StopCoroutine(StartGun());
        MuzzleFlash.SetActive(false);
    }
}
