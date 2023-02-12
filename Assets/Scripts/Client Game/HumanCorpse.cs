//Sanchay Ravindiran 2020

/*
    Instantiated at a point at which a player
    dies or quits the game. The current object
    will immediately soar through the air and
    land back on the ground before resting on it
    indefinitely.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCorpse : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("World"))
        {
            Destroy(rigidbody);
            animator.SetBool("Falling", false);
            Destroy(animator, .1f);
            gameObject.isStatic = true;
        }
    }
}
