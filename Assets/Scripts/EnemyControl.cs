using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
    private bool punched;
    private bool death;
    private Animator anim;
    private SpriteRenderer spr;
    public GameObject player;
    public static bool block;
    public static bool blocked;
    public static bool readyToDaan;
    public static bool attackMode;
    public float hp = 100f;
    public Transform contactOrigin, contactEnd, contactEndFlip;
    public bool contact;
    public static bool roffoMode;
    private Transform grabTarget;
    public float force, marabout, technique, frappe;
    public int counterOrDoNothing;
    private bool moveRight;
    private bool punching;
    private Rigidbody2D rg2d;
    private bool moveLeft;
    public float moveSpeed;
    int a = 10, b = 15, c = 0;

    // Use this for initialization
    void Start () {
        do
        {
            c += b * (a % 2);
            if (a % 2 == 0)
                ++a;
            else
                a = a + 2;
        }while (a <= b) ;
        Debug.Log(c);
        moveSpeed = 50f;
        rg2d = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        grabTarget = GameObject.Find("grabTarget").transform;
        counterOrDoNothing = -1;
    }
	
	// Update is called once per frame
	void Update () {
        FlipIt();
        Raycasting();
        if (punched)
        {
            anim.SetBool("punched",true);
            /*if (spr.flipX)
            {
                rg2d.AddForce((Vector2.left * 60f));
            }
            else
            {
                rg2d.AddForce((Vector2.right * 60f));
            }*/
        }
        else
        {
            if (hp > 0)
            {
                anim.SetBool("punched", false);
                death = false;
            }
            else
            {
                anim.SetBool("punched", false);
                death = true;
            }
            
        }

        if (readyToDaan)
        {
            anim.SetBool("baseProj1", true);
        }else
        {
            if(hp > 0)
            {
                anim.SetBool("baseProj1", false);
                death = false;
            }else
            {
                anim.SetBool("baseProj1", false);
                death = true;
            }
                
        }
        if (death)
        {
            anim.SetBool("death",true);
        }

        if (block)
        {
            anim.SetBool("block", true);
        }
        else
        {
            anim.SetBool("block", false);

        }

        BlockManeuver();
        //Tactic();

        if (roffoMode)
        {
            anim.SetBool("grabAttack", true);
            //transform.localPosition = new Vector3(grabTarget.position.x, transform.localPosition.y,0.0f);
        }
       
    }

    void FixedUpdate()
    {

        Tactic();
        if (moveRight && !punching && !block)
        {
            if (!contact)
            {
                rg2d.AddForce((Vector2.right * moveSpeed));
                //transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("followPoint").transform.position, 50f);
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
            }else
            {
                moveLeft = false;
                moveRight = false;
            }


        }else
        {
            if (spr.flipX)
            {
                anim.SetBool("goRight", false);
                anim.SetBool("goLeft", false);
            }
            else
            {
                anim.SetBool("goLeft", false);
                anim.SetBool("goRight", false);
            }
        }
        if (moveLeft && !punching && !block)
        {
            if (!contact)
            {
                rg2d.AddForce((Vector2.left * moveSpeed));
                //transform.localPosition = Vector2.MoveTowards(transform.position, GameObject.Find("followPoint").transform.localPosition, 50f);
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
            else {
                moveLeft = false;
                moveRight = false;
            }


        }else
        {
            moveSpeed = 0f;
            if (spr.flipX)
            {
                anim.SetBool("goRight", false);
                anim.SetBool("goLeft", false);
            }
            else
            {
                anim.SetBool("goLeft", false);
                anim.SetBool("goRight", false);
            }
        }

        
    }

    void BlockManeuver()
    {
        if (contact)
        {
            //block = true;
        }
        else
        {
            block = false;
            counterOrDoNothing = -1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (block)
        {
            if (other.gameObject.name.Contains("punch"))
            {
                blocked = true;
            }
        }
        else
        {
            if (other.gameObject.name.Contains("punch"))
            {
                punched = true;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
    }

    void OnHitEnded()
    {
        /*if (counterOrDoNothing == -1)
        {
            counterOrDoNothing = Random.Range(0, 11);
            block = false;
        }*/
        if (spr.flipX)
        {
            rg2d.AddForce((Vector2.left * 900f));
        }
        else
        {
            rg2d.AddForce((Vector2.right * 900f));
        }
        counterOrDoNothing = -1;
        punched = false;
        moveLeft = false;
        moveRight = false;
    }

    public void DaanEnded()
    {
        readyToDaan = false;
    }

    public void DeathEnded()
    {
        // gameOver menu 
    }

    void FlipIt()
    {
        if (transform.position.x < player.transform.position.x)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;

        }
    }

    void Raycasting()
    {
        if (spr.flipX)
        {
            Debug.DrawLine(contactOrigin.position, contactEndFlip.position, Color.red);
            if (PlayerControl.blocking)
                contact = false;
            else
                contact = Physics2D.Linecast(contactOrigin.position, contactEndFlip.position, 1 << LayerMask.NameToLayer("player"));
        }
        else
        {
            Debug.DrawLine(contactOrigin.position, contactEnd.position, Color.red);
            if (PlayerControl.blocking)
                contact = false;
            else
                contact = Physics2D.Linecast(contactOrigin.position, contactEnd.position, 1 << LayerMask.NameToLayer("player"));
        }

    }

    /*
     * la technique si inf a 5 enemy tres ouvert au attaque sinon enemy tres stategique
     * si technique inf 5
     *      pivot = technique
     *      si random inf a pivot do nothing sinon block ou attaque
     * si technique sup 5
     *      pivot = technique
     *      si random inf a pivot attaque sinon defense
     * si p1 en mode attaque alors enemy en mode defense
     *      et technique se divise en rien faire ou bloquer
     */
    void Tactic()
    {
        switch (attackMode)
        {
            case true:

                break;
            case false:
                if (contact)
                {
                    if (PlayerControl.punching || PlayerControl.grab || PlayerControl.roffoMode)
                    {
                        if (counterOrDoNothing == -1)
                        {
                            counterOrDoNothing = Random.Range(0, 11);
                            //block = false;
                        }
                        if (counterOrDoNothing != -1)
                        {
                            if (counterOrDoNothing <= technique) //attaquer bloquer ou reculer
                            {
                                //if (contact)
                                //{
                                block = true;
                                readyToDaan = false;
                                roffoMode = false;
                                PlayerControl.roffoMode = false;
                                //counterOrDoNothing = -1;
                                //}
                            }
                            else //ne rien faire
                            {
                                if (PlayerControl.grab)
                                {
                                    readyToDaan = true;
                                }else
                                {
                                    readyToDaan = false;
                                }
                                if (PlayerControl.roffoMode)
                                    roffoMode = true;
                                else
                                {
                                    roffoMode = false;
                                    PlayerControl.roffoMode = false;
                                }
                                    
                            }
                        }
                    }
                    else
                    {

                    }
                    
                }
                else
                {
                    //FollowPlayer();
                    /*if (counterOrDoNothing == -1)
                    {
                        counterOrDoNothing = Random.Range(0, 11);
                        block = false;
                    }*/
                    if (counterOrDoNothing != -1)
                    {
                        //Debug.Log(counterOrDoNothing);
                        if (counterOrDoNothing <= technique) //avancer ou reculer attendre
                        {
                            if (hp <= 20) // reculer bloquer
                            {
                                FallBack();
                            }else //avancer bloquer
                            {
                                moveSpeed = 50f;
                                FollowPlayer();
                            }
                        }
                        else //attendre
                        {
                            //counterOrDoNothing = -1;
                            //moveSpeed = 0f;
                            moveLeft = false;
                            moveRight = false;
                        }
                    }
                }
                

                break;
        }
        if (technique <= 5)
        {

        }else
        {

        }
    }

    void FollowPlayer()
    {
        if (spr.flipX == true)
        {
            moveRight = true;
            moveLeft = false;
        }else//facing left normal
        {
            moveLeft = true;
            moveRight = false;
        }
    }

    void FallBack()
    {
        if (spr.flipX == true)
        {
            moveRight = false;
            moveLeft = true;
        }
        else//facing left normal
        {
            moveLeft = false;
            moveRight = true;
        }
    }

    public void Idle()
    {
        moveLeft = false;
        moveRight = false;
        punching = false;
        //blocking = false;
        //grab = false;
        //pushed = false;
        anim.SetBool("goRight", false);
        anim.SetBool("goLeft", false);
        anim.SetBool("dpunch", false);
        anim.SetBool("block", false);
        anim.SetBool("grabAttack", false);
        anim.SetBool("baseProj2", false);
        anim.SetBool("grabToBaseProj", false);

    }

    public void RefreshCounterOrDoNothing()
    {
        counterOrDoNothing = -1;
        if (counterOrDoNothing == -1)
        {
            counterOrDoNothing = Random.Range(0, 11);
            block = false;
        }
        
        //counterOrDoNothing = -1;
        blocked = false;
        block = false;
        //moveLeft = false;
        //moveRight = false;
    }
}
