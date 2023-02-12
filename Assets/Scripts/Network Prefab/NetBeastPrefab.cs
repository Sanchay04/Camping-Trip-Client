//Sanchay Ravindiran 2020

/*
    Extends network prefab and implements functionality
    specific to the network beast prefab. This class
    takes messages from the game server and follows
    their instructions to make the network beast prefab
    follow the appearance and state of another player
    controlling their client beast prefab on their own
    simulation. For instance, the network beast prefab
    would play a damage animation when the other beast
    player takes damage, and the network beast prefab
    would move to the right when the other beast player
    moves to the right - and so on.
*/

using System.Collections;
using UnityEngine;

public class NetBeastPrefab : NetPrefab
{
    [Header("[NET] Components")]
    [SerializeField] private Animator Animator;
    [SerializeField] public SpriteRenderer SpriteRenderer;
    [SerializeField] public Transform Restrainer;

    [Header("[NET] Materials")]
    [SerializeField] private Material DefaultMaterial;
    [SerializeField] private Material WhiteMaterial;

    [Header("[NET] Additional")]
    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private Sprite ClimbingSprite;

    private Shader DefaultShader;
    private Shader WhiteShader;

    public bool IsARock;

    private void Awake()
    {
        DefaultShader = Shader.Find("Sprites/Default");
        WhiteShader = Shader.Find("GUI/Text Shader");
    }

    public override void Damage(byte amount)
    {
        StartCoroutine(DamageFlicker());
    }

    private IEnumerator DamageFlicker()
    {
        SpriteRenderer.material.shader = WhiteShader;
        SpriteRenderer.material = WhiteMaterial;

        yield return new WaitForSeconds(.05f);

        SpriteRenderer.material.shader = DefaultShader;
        SpriteRenderer.material = DefaultMaterial;

        yield break;
    }

    protected override void UpdateAdditionalState(PlayerState playerState)
    {
        if (IsARock) return;

        PlayerBeastState state = (PlayerBeastState)playerState;
        Animator.SetBool("Walking", state.Walking);

        if (state.Climbing)
        {
            SpriteRenderer.sprite = ClimbingSprite;
        }
        else
        {
            SpriteRenderer.sprite = NormalSprite;
        }
        SpriteRenderer.flipX = !state.Direction;
    }

    public override void TriggerItem(byte itemIndex, bool trigger) //Grab/ attack
    {
        ItemOffset.GetChild(itemIndex).GetComponent<NetHumanItem>().Trigger(trigger);
    }

    public override void EquipItem(byte itemIndex)
    {
        foreach (Transform child in ItemOffset)
        {
            if (!child.transform.GetSiblingIndex().Equals(itemIndex))
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
