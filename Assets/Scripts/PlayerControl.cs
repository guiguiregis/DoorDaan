using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
    public bool moveRight;
    public bool moveLeft;
    private Rigidbody2D rg2d;
    Animator anim;
    SpriteRenderer spr;
    public static bool blocking;
    public static bool punching;
    public static bool grab;
    public static bool blocked;
    public GameObject punchObj;
    public GameObject punchObjFlip;
    public GameObject enemy;
    public Transform contactOrigin, contactEnd,contactEndFlip,grabTarget;
    private Transform grabTargetEn;
    public bool contact;
    private bool pushed;
    public int tapCount;
    public float tapTimeCount;
    public bool tapTiming;
    public static bool roffoMode;
    public Button left, right, feinte, daan;
    private bool roffing;

    // Use this for initialization
    void Start () {
        rg2d = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        grabTargetEn = GameObject.Find("grabTargetEn").transform;
    }
	
	// Update is called once per frame
	void Update () {
        FlipIt();
        Raycasting();
        RoffoAction();
        if(tapTiming){
            tapTimeCount += Time.deltaTime;
        }
		if(punching)
        {
            if (moveLeft || moveRight)
            {
                moveRight = false;
                moveLeft = false;
            }
            anim.SetBool("dpunch", true);
        }
        if (blocking)
        {
            if (moveLeft || moveRight)
            {
                moveRight = false;
                moveLeft = false;
            }
            anim.SetBool("block", true);
        }
        if (grab)
        {
            if (moveLeft || moveRight)
            {
                moveRight = false;
                moveLeft = false;
            }
            Daan();
        }

        if (roffoMode)
        {
            
            anim.SetBool("grabAttack", true);
            left.interactable = false;
            right.interactable = false;
            feinte.interactable = false;
            daan.interactable = false;
            transform.localPosition = new Vector3(grabTargetEn.position.x, transform.localPosition.y,0.0f);
        }else
        {
            left.interactable = true;
            right.interactable = true;
            feinte.interactable = true;
            daan.interactable = true;
            anim.SetBool("grabAttack", false);
        }

        if (roffing)
        {
            ///
        }

	}

    void FixedUpdate()
    {
        if (moveRight && !punching && !blocking)
        {
            if(!contact && !spr.flipX)
                rg2d.AddForce((Vector2.right * 50f));
            if(contact && spr.flipX)
                rg2d.AddForce((Vector2.right * 50f));
            if (!contact && spr.flipX)
                rg2d.AddForce((Vector2.right * 50f));
            if (spr.flipX)
            {
                anim.SetBool("goRight", false);
                anim.SetBool("goLeft", true);
            }
            else
            {
                anim.SetBool("goLeft", false);
                anim.SetBool("goRight", true);
            }
            
        }
        if (moveLeft && !punching && !blocking)
        {
            if(contact && !spr.flipX)
                rg2d.AddForce((Vector2.left * 50f));
            if (!contact && !spr.flipX)
                rg2d.AddForce((Vector2.left * 50f));
            if (!contact && spr.flipX)
                rg2d.AddForce((Vector2.left * 50f));
            if (spr.flipX)
            {
                anim.SetBool("goLeft", false);
                anim.SetBool("goRight", true);
            }
            else
            {
                anim.SetBool("goRight", false);
                anim.SetBool("goLeft", true);
            }
            
        }
    }

    public void goRight()
    {
        if(roffoMode)
            moveRight = false;
        else
        {
                moveRight = true;
        }
            
    }

    public void goLeft()
    {
        if (roffoMode)
            moveLeft = false;
        else
            moveLeft = true;
    }

    public void doubleHit()
    {
        
        if(tapCount == 0)
        {
           
            tapTiming = true;
        }
        if(tapCount == 1)
        {
            
        }
    }

    public void doubleHitDown()
    {
        
            tapCount++;
    }

    public void RoffoAction()
    {
        if (tapCount == 2 && tapTimeCount < 0.2)
        {
            if (contact)
            {
               
                roffoMode = true;
                //EnemyControl.roffoMode = true;
                tapCount = 0;
                tapTimeCount = 0;
                tapTiming = false;
            }
            else
            {
                roffoMode = false;
                //EnemyControl.roffoMode = false;
                tapCount = 0;
                tapTimeCount = 0;
                tapTiming = false;
            }
            
        }else if (tapCount >= 2 && tapTimeCount >= 0.2)
        {
            tapCount = 0;
            tapTimeCount = 0;
            tapTiming = false;
        }
        else if (tapCount > 2)
        {
            tapCount = 0;
            tapTimeCount = 0;
            tapTiming = false;
        }
        else if (tapCount == 1 && tapTimeCount >= 0.2)
        {
           
            tapCount = 0;
            tapTimeCount = 0;
            tapTiming = false;
        }
    }

    public void punch()
    {
        if (roffoMode)
            roffing = true;
        else
            punching = true;
        //Standing();
        
    }

    public void block()
    {
        blocking = true;
        //Standing();
        //anim.SetBool("dpunch", false);
        
    }

    public void Standing()
    {
        moveLeft = false;
        moveRight = false;
        anim.SetBool("goRight", false);
        anim.SetBool("goLeft", false);
    }

    public void Idle()
    {
        moveLeft = false;
        moveRight = false;
        punching = false;
        blocking = false;
        grab = false;
        pushed = false;
        anim.SetBool("goRight", false);
        anim.SetBool("goLeft", false);
        anim.SetBool("dpunch", false);
        anim.SetBool("block", false);
        anim.SetBool("grabAttack", false);
        anim.SetBool("baseProj2", false);
        anim.SetBool("grabToBaseProj", false);

    }

    public void OnPunchColliderStarted()
    {
        if (spr.flipX)
        {
            punchObjFlip.SetActive(true);
        }
        else
        {
            punchObj.SetActive(true);
        }
        
    }

    public void OnPunchColliderEnded()
    {
        if (spr.flipX)
        {
            punchObjFlip.SetActive(false);
        }
        else
        {
            punchObj.SetActive(false);
        }
    }

    public void PushedEnded()
    {
        pushed = false;
    }

    void FlipIt()
    {
        if(transform.position.x < enemy.transform.position.x)
        {
            spr.flipX = false;
        }else
        {
            spr.flipX = true;

        }
    }

    void Raycasting()
    {
        if (spr.flipX)
        {
            Debug.DrawLine(contactOrigin.position, contactEndFlip.position, Color.red);
            if (EnemyControl.block)
                contact = false;
            else
                contact = Physics2D.Linecast(contactOrigin.position, contactEndFlip.position, 1 << LayerMask.NameToLayer("enemy"));
        }
        else
        {
            Debug.DrawLine(contactOrigin.position, contactEnd.position, Color.red);
            if (EnemyControl.block)
                contact = false;
            else
                contact = Physics2D.Linecast(contactOrigin.position, contactEnd.position, 1 << LayerMask.NameToLayer("enemy"));
        }
            
    }

    void Daan()
    {
        if (contact)
        {
            
            //if (EnemyControl.readyToDaan)
            //{
                //anim.SetBool("grabToBaseProj", false);
                anim.SetBool("baseProj2", true);
            //}
            /*else
            {
                
                anim.SetBool("grabToBaseProj", true);
                anim.SetBool("baseProj2", false);
            }*/
            //if(!EnemyControl.block)
            //    EnemyControl.readyToDaan = true;
            //else
            //    EnemyControl.readyToDaan = false;
        }
    }

    public void grapNDaan()
    {
        grab = true;
    }

}
