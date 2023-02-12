//Sanchay Ravindiran 2020

/*
    Extends client prefab and implements functionality
    specific to the player controlled human prefab. This
    class reads player input and uses it to control the
    human hunter character, which can use its own items
    as well as items purchased from the RV shop to hunt
    down the player controlled beast. The human character
    does very little damage to the beast, but it is just
    as nimble. However, it is also really fragile and can
    be grabbed and carried around by the beast with ease.
    This class extends client prefab so that whenever
    anything important happens it can sychronize its state
    with the game server, and so that it can continuously
    broadcast its state to other players.
*/

using UnityEngine;

public class ClientHumanPrefab : ClientPrefab
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraOffset;
    [SerializeField] Transform ItemOffset;

    [Header("Additional")]
    [SerializeField] private GameObject corpseAnimationPrefab;

    [Header("Debug")]

    public static int Health = 5;
    public static readonly int MaxHealth = 5;
    public static float Money = 1100000;

    private bool grabbed;
    private bool direction;
    private bool walking;

    private int speed = 500;
    public byte Jumps = 2;

    protected override void Initialize()
    {
        CameraController.Target(cameraOffset);
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

        if (Health <= 0)
        {
            PlayerDeath playerDeath = new PlayerDeath();
            Client.Commands.Send(playerDeath, Client.Reliable);

            rigidbody.isKinematic = true;
            Dead = true;
            Instantiate(corpseAnimationPrefab, ClientWindow.GetDistortionFreePosition(transform.position), Quaternion.identity);

            CameraController.Target(ClientWindow.NetPlayers[damager].transform);

            transform.position = ClientWindow.DeadRoom;
            DeathScreen.Show(ClientWindow.NetPlayers[damager].Name, ClientWindow.Name, ClientWindow.TotalDamageBeforeDeath, true);

            Money += ClientWindow.TotalDamageBeforeDeath * 25;
            ClientWindow.TotalDamageBeforeDeath = 0;

            for (int i = 0; i < Inventory.Count; i++)
            {
                Destroy(Inventory[i].transform.gameObject);
            }

            Inventory.Clear();
            InventoryIndex = 0;
        }
    }

    public override void Revive()
    {
        Dead = false;
        grabbed = false;
        transform.position = ClientWindow.HumanSpawnPosition;
        transform.parent = null;
        rigidbody.velocity = Vector2.zero;
        rigidbody.isKinematic = false;

        CameraController.Target(transform.GetChild(0));
        DeathScreen.Hide();
    }

    private void Update()
    {
        if (Dead) return;

        Movement();
        InventoryItemSelection();
    }

    public void Grab(Transform restrainer)
    {
        animator.SetBool("walking", false);
        rigidbody.velocity = Vector2.zero;
        rigidbody.isKinematic = true;

        for (int i = 0; i < Inventory.Count; i++)
        {
            Inventory[i].SetActive(false);
        }

        grabbed = true;
        transform.parent = restrainer;
        transform.localPosition = Vector2.zero;
    }

    private void Movement()
    {
        float input = Input.GetAxis("Horizontal");

        KeyCode Up = KeyCode.W;
        KeyCode Left = KeyCode.A;
        KeyCode Right = KeyCode.D;

        if (!grabbed)
        {
            if (Input.GetKey(Left))
            {
                rigidbody.velocity = new Vector2(input * speed * Time.deltaTime, rigidbody.velocity.y);

                direction = false;
                walking = true;
                animator.SetBool("walking", true);
                spriteRenderer.flipX = true;
                ItemOffset.localPosition = new Vector2(.5f, 0);

                cameraOffset.localPosition = new Vector2(-1.5f, cameraOffset.localPosition.y);

                if (!Inventory.Count.Equals(0))
                {
                    ItemOffset.transform.GetChild(InventoryIndex).GetComponent<ClientHumanItem>().Flip(direction);
                }
            }
            if (Input.GetKey(Right))
            {
                rigidbody.velocity = new Vector2(input * speed * Time.deltaTime, rigidbody.velocity.y);

                direction = true;
                walking = true;
                animator.SetBool("walking", true);
                spriteRenderer.flipX = false;
                ItemOffset.localPosition = new Vector2(-.5f, 0);

                cameraOffset.localPosition = new Vector2(1.5f, cameraOffset.localPosition.y);

                if (!Inventory.Count.Equals(0))
                {
                    ItemOffset.transform.GetChild(InventoryIndex).GetComponent<ClientHumanItem>().Flip(direction);
                }
            }
            if (Input.GetKey(Up) && Jumps > 0)
            {
                if (Input.GetKeyDown(Up))
                {

                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                    rigidbody.AddForce(new Vector2(0, 200), ForceMode2D.Impulse);
                    Jumps--;
                    walking = false;

                    animator.SetBool("walking", false);
                }
            }
            if (!Input.GetKey(Up) && !Input.GetKey(Left) && !Input.GetKey(Right))
            {
                walking = false;

                if (!Jumps.Equals(2))
                {
                    return;
                }

                animator.SetBool("walking", false);
            }
        }

        PlayerState = new PlayerHumanState
        {
            direction = direction,
            walking = walking,
            X = transform.position.x,
            Y = transform.position.y,
        };
    }

    private void InventoryItemSelection()
    {
        if (grabbed) return;

        float scroll = (Input.mouseScrollDelta).y;
        int temporaryIndex = InventoryIndex;
        int accurateCount = Inventory.Count - 1;

        if (Inventory.Count.Equals(0)) return;

        if (scroll.Equals(0))
        {
            return;
        }

        else if (scroll > 0)
        {
            temporaryIndex++;
        }
        else if (scroll < 0)
        {
            temporaryIndex--;
        }

        if (temporaryIndex < 0)
        {
            temporaryIndex = accurateCount;
        }
        else if (temporaryIndex > accurateCount)
        {
            temporaryIndex = 0;
        }


        for (int i = 0; i < Inventory.Count; i++)
        {
            Inventory[i].SetActive(false);
        }

        InventoryIndex = temporaryIndex;
        Inventory[InventoryIndex].SetActive(true);
        ItemOffset.transform.GetChild(InventoryIndex).GetComponent<ClientHumanItem>().Flip(direction);

        PlayerItemEquip playerHumanItemEquip = new PlayerItemEquip
        {
            ItemIndex = (byte)InventoryIndex
        };
        Client.Commands.Send(playerHumanItemEquip, Client.Reliable);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("World"))
        {
            Jumps = 2;
        }
    }
}
