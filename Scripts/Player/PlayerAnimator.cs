using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Refs
    Animator anim;
    PlayerMovement pm;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDirection.x != 0 || pm.moveDirection.y != 0)
        {
            anim.SetBool("Move", true);

            DirectionCheck();

        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    void DirectionCheck()
    {
        if (pm.lastHorizontalV < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
