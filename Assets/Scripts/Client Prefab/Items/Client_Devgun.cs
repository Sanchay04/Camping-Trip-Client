//Sanchay Ravindiran 2020

/*
    Extends the client human item and implements
    functionality specific to the client devgun item.
    The developer gun item was added into the game to
    test it out but I kept it in because I realized that
    I didn't have enough time to add more items or anything
    else to the game before the celebration event.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_Devgun : ClientHumanItem
{
    private byte bullet;
    private const byte maxBullets = 255;
    private int forceDirection;

    [SerializeField] private GameObject clientBullet;
    [SerializeField] private Transform bulletOffset;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject muzzlePFX;

    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Enabled()
    {
        muzzleFlash.SetActive(false);
        RefreshAmmo();
        StartCoroutine(StartGun());
    }

    public override void Flip(bool direction)
    {
        spriteRenderer.flipX = !direction;

        if (direction)
        {
            muzzleFlash.transform.localPosition = new Vector2(1.7f, .1f);
            bulletOffset.localPosition = new Vector2(2f, .1f);
            bulletOffset.localRotation = Quaternion.Euler(0, 0, -90);

            forceDirection = -1;
        }
        else
        {
            muzzleFlash.transform.localPosition = new Vector2(-1.7f, .1f);
            bulletOffset.localPosition = new Vector2(-2f, .1f);
            bulletOffset.localRotation = Quaternion.Euler(0, 0, 90);

            forceDirection = 1;
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
                if (!bullet.Equals(0))
                {
                    Instantiate(clientBullet, bulletOffset.position, bulletOffset.rotation);
                    Instantiate(muzzlePFX, bulletOffset.position, Quaternion.identity);
                    muzzleFlash.SetActive(true);

                    Trigger(true, (byte)transform.GetSiblingIndex());

                    clientPrefabRigidbody.AddForce(new Vector2(forceDirection * 50, 150), ForceMode2D.Impulse);
                    bullet--;
                    RefreshAmmo();

                    yield return new WaitForSeconds(.05f);

                    muzzleFlash.SetActive(false);
                }
                else
                {
                    Refresh("Reloading...");

                    yield return new WaitForSeconds(1);

                    bullet = maxBullets;
                    RefreshAmmo();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void RefreshAmmo()
    {
        Refresh(bullet + "/" + maxBullets + " (full auto)");
    }

    protected override void Disabled()
    {
        StopCoroutine(StartGun());
        muzzleFlash.SetActive(false);
    }
}
