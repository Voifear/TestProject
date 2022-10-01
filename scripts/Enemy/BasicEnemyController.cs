using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    #region DEFAULT/////////////////////////////////////////////////////
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }

    private State currentState;

    [SerializeField]
    private float
        GroundCheckDistance,
        WallCheckDistance,
        MovementSpeed,
        MaxHealth,
        KnockbackDuration;
    [SerializeField]
    private Transform
        GroundCheck,
        WallCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Vector2 knockbackSpeed;

    private float 
        currentHealth,
        KnockBackStartTime;

    private int 
        facingDirection,
        damageDirection;

    private Vector2 movement;

    private bool
        GroundDetected,
        WallDetected;

    private GameObject alive;

    private Rigidbody2D aliveRb;

    private Animator aliveAnim;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        facingDirection = 1;
    }

    private void Update()
    {
        Anim();
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateKnockBackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    #endregion

    #region WALKING STATE///////////////////////////////////

    private void EnterWalkingState()
    {

    }

    private void UpdateWalkingState()
    {
        GroundDetected = Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, whatIsGround);
        WallDetected = Physics2D.Raycast(WallCheck.position, transform.right, WallCheckDistance, whatIsGround);

        if (!GroundDetected || WallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(MovementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }

    private void ExitWalkingState()
    {

    }

    #endregion

    #region KNOCKBACK STATE////////////////////////////////////////////

    private void EnterKnockBackState()
    {
        KnockBackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("KnockBack", true);
    }

    private void UpdateKnockBackState()
    {
        if(Time.time >= KnockBackStartTime + KnockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }

    private void ExitKnockBackState()
    {
        aliveAnim.SetBool("KnockBack", false);
    }

    #endregion

    #region DEAD STATE//////////////////////////////////////////////////////////

    private void EnterDeadState()
    {
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    #endregion

    #region OTHER FUNCTION///////////////////////////////////////////

    private void Anim()
    {
        aliveAnim.SetFloat("Walk", Mathf.Abs(movement.x));
    }

    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        if(attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        if (currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f); 
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitKnockBackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterKnockBackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position, new Vector2(GroundCheck.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(WallCheck.position, new Vector2(WallCheck.position.x + WallCheckDistance, WallCheck.position.y));
    }

    #endregion
}
