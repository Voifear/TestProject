using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //публичные переменные:
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;
    public Transform topCheck;
    private float topCheckRadius;
    public LayerMask Roof;

    private bool jumpLock = false;

    public Transform DopPosition;
    public float dopRadius;

    public bool BlockMoveX;
    public bool BlockMoveY;
    public bool BlockRoll;
    public bool BlockJump;
    public bool BlockCrouch;

    void Start()
    {
        poseCrouch.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        checkRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;

        topCheckRadius = topCheck.GetComponent<CircleCollider2D>().radius;

        WallCheckRadiusUp = WallCheckUp.GetComponent<CircleCollider2D>().radius;
        WallCheckRadiusDown = WallCheckDown.GetComponent<CircleCollider2D>().radius;

        gravityDef = rb.gravityScale;
    }

   
    void Update()
    {
        if (BlockRoll == true) { TimeBlockRoll -= Time.deltaTime; }
        run();
        Reflect();
        jump();
        roll();
        MoveOnWall();
        WallJump();
        Attack();
        Dead();
    }

    private void FixedUpdate()
    {
        CheckingGround();
        CheckingWall();
        CrouchCheck();
        
    }

    public bool faceRigth = true;
    void Reflect()
    {
        if (!BlockMoveX)
        {
            if ((moveVector.x > 0 && !faceRigth) || (moveVector.x < 0 && faceRigth))
            {
                transform.localScale *= new Vector2(-1, 1);
                faceRigth = !faceRigth;
            }
        }
            
    }


    public Vector2 moveVector;
    public float speed = 2f;
    void run()
    {
        if (BlockMoveX|| BlockMoveY)
        {
            moveVector.x = 0;
        }

        else 
        {
             moveVector.x = Input.GetAxisRaw("Horizontal");
             rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
        }
        anim.SetFloat("is run", Mathf.Abs(moveVector.x));

    }

    public float jumpforce = 2f;
    void jump()
    {
        if (!BlockJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && onGround && !jumpLock)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            }
        }
        

    }

    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius = 0.09161574f;
    public LayerMask Ground;
    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }

    public bool onWall;
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    private float WallCheckRadiusUp;
    private float WallCheckRadiusDown;
    public LayerMask Wall;
    void CheckingWall()
    {
        onWall = (Physics2D.OverlapCircle(WallCheckUp.position, WallCheckRadiusUp, Wall) && Physics2D.OverlapCircle(WallCheckDown.position, WallCheckRadiusDown, Wall));
        anim.SetBool("OnWall", onWall);
    }

    public Collider2D poseStand;
    public Collider2D poseCrouch;
    void CrouchCheck()
    {
        if (!BlockCrouch)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                
                anim.SetBool("crouch", true);
                poseStand.enabled = false;
                poseCrouch.enabled = true;
                speed = 1f;
                jumpLock = true;
                Physics2D.IgnoreLayerCollision(9, 10, true);
                Invoke("IgnoreLayerOff", 0.5f);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl) &&!Physics2D.OverlapCircle(topCheck.position, topCheckRadius, Roof))
            {
                
                anim.SetBool("crouch", false);
                poseStand.enabled = true;
                poseCrouch.enabled = false;
                speed = 2f;
                jumpLock = false;
            }
        }
        
    }

    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(DopPosition.position, dopRadius);
    }

    void LedgeGo()
    {
        transform.position = new Vector3(DopPosition.position.x, DopPosition.position.y, transform.position.z);
    }

    public void StartAnimLadge()
    {
        BlockCrouch = true;
        BlockMoveX = true;
        BlockRoll = true;
        BlockMoveY = true;
        rb.velocity = Vector2.zero;
        anim.Play("LedgeClimb");
    }

    public float upDownSpeed = 2f;
    public float slideSpeed = 1;
    private float gravityDef;
    void MoveOnWall()
    {
        if (onWall && !onGround)
        {
            if (!BlockMoveY && moveVector.y == 0)
            {
                BlockRoll = true;
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, slideSpeed);
            }
            else
            {
                slideSpeed = -0.5f;
                anim.SetFloat("SlideDown", 0);
            }
        }
        else if (!onGround && !onWall) { rb.gravityScale = gravityDef; }
        else if (onGround && !onWall) { rb.gravityScale = gravityDef; }
    }


    public float JumpWallTime = 0.5f;
    private float timerJumpWall;
    public Vector2 JumpAngle = new Vector2(5f, 3);
    void WallJump()
    {
        if (onWall && !onGround && Input.GetKeyDown(KeyCode.Space))
        {
            BlockMoveX = true;
            BlockRoll = true;
            BlockMoveY = true;

            moveVector.x = 0;

            transform.localScale *= new Vector2(-1, 1);
            faceRigth = !faceRigth;

            rb.gravityScale = gravityDef;
            rb.velocity = new Vector2(0, 0);

            rb.velocity = new Vector2(transform.localScale.x * JumpAngle.x, JumpAngle.y);
        }
        
        if(BlockRoll && (timerJumpWall += Time.deltaTime) >= JumpWallTime)
        {
            if (onWall || onGround || Input.GetAxisRaw("Horizontal") != 0)
            {
                BlockMoveX = false;
                BlockRoll = false;
                BlockMoveY = false;
                timerJumpWall = 0;
            }
           
        }
    }

    public bool AttackBlock = false;
    public float AttackQ = 0;
    public bool EnemyAttack;

    public Transform AttackPos;
    public LayerMask enemyDamage;
    public int damage = 25;
    public float AttackRange;
    void Attack()
    {
        if (!AttackBlock)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AttackQ++;
                if (AttackQ == 1)
                {
                    anim.SetTrigger("Attack_one");
                    
                }
                if (AttackQ == 2)
                {
                    anim.SetTrigger("Attack_two");
                    
                }
            }
        }
    }
    
    void AttackDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, enemyDamage);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<_Archer>().TakeDamage(damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }

    void AttackUp()
    {
        AttackQ = 0; 
    }

    public float rollImpulse = 5;
    public float TimeBlockRoll = 0.5f;
    void roll()
    {
        if (!BlockRoll)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.StopPlayback();
                anim.Play("roll");
                BlockMoveX = true;
                BlockMoveY = true;
                poseStand.enabled = false;
                poseCrouch.enabled = true;
                Physics2D.IgnoreLayerCollision(9, 10, true);
                Physics2D.IgnoreLayerCollision(9, 13, true);
                rb.velocity = new Vector2(0, 0);
                if (!faceRigth) 
                {
                    rb.velocity = (Vector2.left * rollImpulse); 
                }
                else 
                {
                    rb.velocity = (Vector2.right * rollImpulse);
                }

            }
        }   
    }

    void RollEnd()
    {
        BlockMoveX = false;
        BlockMoveY = false;
        BlockRoll = true;
        poseStand.enabled = true;
        poseCrouch.enabled = false;
        Physics2D.IgnoreLayerCollision(9, 13, false);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    public int HP = 100;
    public void EnemyDamage(int damage)
    {
        HP -= damage;
        anim.Play("hit");
        Debug.Log("Attack");
    }

    void Dead()
    {
        if (HP <= 0)
        {
            anim.SetTrigger("IsDeath");
            BlockMoveX = true;
            this.enabled = false;
        }
    }

}
