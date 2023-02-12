//Sanchay Ravindiran 2020

/*
    Extends client prefab and implements functionality
    specific to the player controlled clientside beast
    prefab. This class reads player input and uses it
    to control the beast character, which can attack
    and maim other players connected to the game server
    who are playing as human beast hunters. The beast
    can also climb trees and transform into a rock, but
    it can be brought down gradually by attacks from
    human players. It extends client prefab so that it can
    synchronize any triggers with the game server, and so
    that it can continuously broadcast its current state to
    other players.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBeastPrefab : ClientPrefab
{
    [Header("Components")]
    [SerializeField] public Animator Animator;
    [SerializeField] public SpriteRenderer SpriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] public Transform ItemOffset;

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;
    private Shader defaultShader;
    private Shader whiteShader;

    [Header("Additional")]
    public bool IsARock;
    [SerializeField] private GameObject corpseAnimationPrefab;
    [SerializeField] private Sprite climbingSprite;
    [SerializeField] private Sprite normalSprite;
    public static int Health = 100;
    public static readonly int MaxHealth = 100;

    private bool climbing;
    private bool direction;
    private bool walking;
    private int speed = 300;
    private bool grounded;

    private bool currentlyHoldingHuman;
    private int userBeingHeld = -1;

    [Header("Starter Items")]
    [SerializeField] private GameObject RockTransform;
    [SerializeField] private GameObject NoneItem;

    private void Awake()
    {
        defaultShader = Shader.Find("Sprites/Default");
        whiteShader = Shader.Find("GUI/Text Shader");
    }

    protected override void Initialize()
    {
        InventoryIndex = 0;

        CameraController.Target(transform);
    }

    protected override void ItemAdded(GameObject item)
    {
        GameObject newItem = Instantiate(item, ItemOffset, false);
        Inventory.Add(newItem);

        for (int i = 0; i < Inventory.Count; i++)
        {
            Inventory[i].SetActive(false);
        }

        newItem.SetActive(true);
        InventoryIndex = newItem.transform.GetSiblingIndex();
    }

    public override void Damage(byte amount, int damager)
    {
        Health -= amount;
        rigidbody.AddForce(new Vector2(0, 300), ForceMode2D.Impulse);
        StartCoroutine(DamageFlicker());

        print("<b>" + Health + "</b>");

        if (Health <= 0)
        {
            PlayerDeath playerDeath = new PlayerDeath();
            Client.Commands.Send(playerDeath, Client.Reliable);
            Dead = true;

            Instantiate(corpseAnimationPrefab, transform.position, Quaternion.identity);

            CameraController.Target(ClientWindow.NetPlayers[damager].transform);

            transform.position = ClientWindow.DeadRoom;
            DeathScreen.Show(ClientWindow.NetPlayers[damager].Name, ClientWindow.Name, ClientWindow.TotalDamageBeforeDeath, false);

            ClientWindow.TotalDamageBeforeDeath = 0;

            for (int i = 0; i < Inventory.Count; i++)
            {
                Destroy(Inventory[i].transform.gameObject);
            }

            Inventory.Clear();
            InventoryIndex = 0;
        }
    }

    private IEnumerator DamageFlicker()
    {
        SpriteRenderer.material.shader = whiteShader;
        SpriteRenderer.material = whiteMaterial;

        yield return new WaitForSeconds(.05f);

        SpriteRenderer.material.shader = defaultShader;
        SpriteRenderer.material = defaultMaterial;

        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Dead) return;
        if (IsARock) return;

        if (collision.tag.Equals("Tree"))
        {
            if (Input.GetKey(KeyCode.C))
            {
                climbing = true;
                speed = 600;
                SpriteRenderer.sprite = climbingSprite;
            }
            else
            {
                climbing = false;
                speed = 300;
                SpriteRenderer.sprite = normalSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Dead) return;
        if (IsARock) return;

        if (collision.tag.Equals("NetHumanPrefab"))
        {
            userBeingHeld = collision.GetComponent<NetPrefab>().User;

            PlayerBeastGrab playerBeastGrab = new PlayerBeastGrab
            {
                GrabbedUser = userBeingHeld,
                GrabberUser = Client.User,
            };

            Client.Commands.Send(playerBeastGrab, Client.Reliable);

            currentlyHoldingHuman = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Dead) return;

        if (collision.tag.Equals("Tree"))
        {
            climbing = false;
            SpriteRenderer.sprite = normalSprite;
        }
    }

    private void Update()
    {
        if (Dead) return;

        Movement();
    }

    private void Movement()
    {
        float input = Input.GetAxis("Horizontal");

        KeyCode Up = KeyCode.W;
        KeyCode Left = KeyCode.A;
        KeyCode Right = KeyCode.D;
        KeyCode Climb = KeyCode.C;

        if (!IsARock)
        {
            if (Input.GetKey(Left))
            {
                rigidbody.velocity = new Vector2(input * speed * Time.deltaTime, rigidbody.velocity.y);

                direction = false;
                walking = true;
                SyncAnimation(true);
                SpriteRenderer.flipX = true;
            }
            if (Input.GetKey(Right))
            {
                rigidbody.velocity = new Vector2(input * speed * Time.deltaTime, rigidbody.velocity.y);

                direction = true;
                walking = true;
                SyncAnimation(true);
                SpriteRenderer.flipX = false;
            }
            if (Input.GetKey(Up) && grounded)
            {
                rigidbody.AddForce(new Vector2(0, 400), ForceMode2D.Impulse);
                grounded = false;
                walking = false;

                SyncAnimation(false);
            }
            if (climbing)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 400 * Time.deltaTime);
                climbing = true;
                SyncAnimation(true);
            }
            if (!Input.GetKey(Up) && !Input.GetKey(Left) && !Input.GetKey(Right) && !climbing)
            {
                walking = false;
                climbing = false;

                if (!grounded)
                {
                    return;
                }

                SyncAnimation(false);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentlyHoldingHuman)
                {
                    PlayerDamage damage = new PlayerDamage
                    {
                        TargetUser = userBeingHeld,
                        Amount = 5
                    };
                    Client.Commands.Send(damage, Client.Reliable);
                    currentlyHoldingHuman = false;
                    userBeingHeld = -1;
                }
            }
        }

        if (IsARock)
        {
            PlayerState = new PlayerBeastState
            {
                direction = direction,
                walking = false,
                climbing = false,
                X = transform.position.x,
                Y = transform.position.y
            };
        }
        else
        {
            PlayerState = new PlayerBeastState
            {
                direction = direction,
                walking = walking,
                climbing = climbing,
                X = transform.position.x,
                Y = transform.position.y
            };
        }
    }



    private void InventoryItemSelection()
    {
        if (Inventory.Count.Equals(0)) return;
        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                InventoryIndex = 0;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                InventoryIndex = 1;
            }

            for (int i = 0; i < Inventory.Count; i++)
            {
                Inventory[i].SetActive(false);
            }
            Inventory[InventoryIndex].SetActive(true);
        }
    }

    private void SyncAnimation(bool walking)
    {
        if (IsARock)
        {
            walking = false;
        }
        Animator.SetBool("walking", walking);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("World"))
        {
            grounded = true;
        }
    }

    public override void Revive()
    {
        transform.position = ClientWindow.BeastSpawnPosition;
        Dead = false;
        transform.parent = null;
        rigidbody.velocity = Vector2.zero;
        rigidbody.isKinematic = false;

        CameraController.Target(transform);
        AddItem(NoneItem, ClientWindow.ItemTypes.BeastNone);
        AddItem(RockTransform, ClientWindow.ItemTypes.BeastRockTransform);
        InventoryIndex = 0;
        InventoryItemSelection();
        DeathScreen.Hide();
    }
}
