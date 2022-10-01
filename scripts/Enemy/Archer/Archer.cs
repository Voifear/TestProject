using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    float HP = 350;
    int HPattack = 25;

    bool LockAll;
    public bool IsAttack = false;
    public float timeBackBalance = 1.5f;
    public float HpResists = 0.5f;

    public GameObject Bullet;
    public Transform ShotPoint;
    public LayerMask Player;

    private void Start()
    {
        AttackRange = AttackOneRange.GetComponent<CircleCollider2D>().radius;
        AttackRangeTwo = AttackTwoRange.GetComponent<CircleCollider2D>().radius;
    }

    void Update()
    {
        if (IsAttack == true) { HP -= (HPattack / HpResists); IsAttack = false; HPattack = 0; }
        /*if (Balance >= 1)
        {
            timeBackBalance -= Time.deltaTime;
            if (timeBackBalance < 0)
            {
                Balance -= 10;
                Debug.Log("Balanc");
                timeBackBalance = 1.5f;
            }
        }*/

        death();
        BalanceCheck();        
        AttackTwo();
        AttackOne();
        checkingAttackRange();
        checkingAttackRangeTwo();
    }

    public void TakeDamage(int damage)
    {
        if (!LockAll)
        {
            anim.SetTrigger("IsHit");
            Balance += 20;
            IsAttack = true;
            HPattack = 25;
        }
        
    }
    public float AttackRange;
    public bool onAttackRange;
    public Transform AttackOneRange;
    void checkingAttackRange()
    {
        onAttackRange = Physics2D.OverlapCircle(AttackOneRange.position, AttackRange, Player);
    }
    void checkingAttackRangeTwo()
    {
        onAttackRangeTwo = Physics2D.OverlapCircle(AttackTwoRange.position, AttackRangeTwo, Player);
    }

    public float timeLeftAttackTwo = 0;
    public Transform AttackTwoRange;
    public bool onAttackRangeTwo;
    public float AttackRangeTwo;
    void AttackTwo()
    {
        if (timeLeftAttackTwo <= 0)
        {
            if (onAttackRange == true)
            {
                anim.Play("");
                onAttackRange = false;
                timeLeftAttackTwo = 10f;
            }
        } 
        else
        {
            timeLeftAttackTwo -= Time.deltaTime;
        }
        
    }

    public float timeLeftAttackOne = 0;
    void AttackOne()
    {
        if (timeLeftAttackOne <= 0)
        {
            if(onAttackRangeTwo == true)
            {
                anim.SetTrigger("IsAttack1");
                onAttackRangeTwo = false;
                timeLeftAttackOne = 5f;
                
            }           
        }
        else
        {
            timeLeftAttackOne -= Time.deltaTime;
        }
    }
    void Attack()
    {
        Instantiate(Bullet, ShotPoint.position, transform.rotation);
    }

    public float Balance = 0;
    private float timeLeftBalance = 1f;
    void BalanceCheck()
    {
        if (!LockAll)
        {
            if (Balance >= 100)
            {
                HPattack = 50;
                anim.SetTrigger("IsBalance");
                timeLeftBalance -= Time.deltaTime;                
                if (timeLeftBalance < 0)
                {
                    Balance = 0;
                    HPattack = 25;
                    timeLeftBalance = 1f;
                }
            }
        }
        
    }

    void death()
    {
        if (HP <= 0)
        {
            anim.SetTrigger("IsDead");
            LockAll = true;
        }
    }
    void Destrou()
    {
        Destroy(gameObject);
    }
}
