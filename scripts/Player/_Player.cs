 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Player : MonoBehaviour
{
    #region DEFAULT/////////////////////////////////////

    private float _MovementInputDirection;
    private float _TimeAttack = 0.30f;

    private int _FacingDirection = 1;

    private Rigidbody2D _rb;
    private Animator _anim;

    [SerializeField] private Transform _GroundCheck;
    [SerializeField] private Transform _WallCheckOne;
    [SerializeField] private Transform _WallCheckTwo;
    [SerializeField] private Transform _AttackPose;

    [SerializeField] private LayerMask _WhatIsGround;

    [SerializeField] private float _MovementSpeed = 10.0f;
    [SerializeField] private float _JumpForce = 16.0f;
    [SerializeField] private float _GroundCheckRadius;
    [SerializeField] private float _WallCheckDistance;
    [SerializeField] private float _WallSlideSpeed;
    [SerializeField] private float _RollSpeed;
    [SerializeField] private float _MovementForceInAir;
    [SerializeField] private float _AttackCheckRadius;

    [SerializeField] private bool _IsRolling;
    [SerializeField] private bool _IsBlockMoveX;

    private bool _FacingRight = true;
    private bool _IsGrounded;
    private bool _IsTouchingWallOne;
    private bool _IsTouchingWallTwo;
    private bool _IsWallSliding;
    private bool _IsWallJumping;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInput();
        UpdateAnimations();
        CheckMovementDirection();
        CheckWallSliding();
        Attack();

        //ROLL
        if (_IsRolling)
        {
            _rb.velocity = new Vector2(_RollSpeed * _FacingDirection, 0);
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundsings();
    }

    private void CheckSurroundsings()
    {
        _IsGrounded = Physics2D.OverlapCircle(_GroundCheck.position, _GroundCheckRadius, _WhatIsGround);

        _IsTouchingWallOne = Physics2D.Raycast(_WallCheckOne.position, transform.right, _WallCheckDistance, _WhatIsGround);
        _IsTouchingWallTwo = Physics2D.Raycast(_WallCheckTwo.position, transform.right, _WallCheckDistance, _WhatIsGround);
    }

    private void UpdateAnimations()
    {
        _anim.SetFloat("Movement", Mathf.Abs(_MovementInputDirection));
        _anim.SetFloat("VelocityY", _rb.velocity.y);

        _anim.SetBool("IsGrounded", _IsGrounded);
        _anim.SetBool("IsSliding", _IsWallSliding);

    }

    private void Reflect()
    {
        if (!_IsWallSliding || !_IsBlockMoveX)
        {
            _FacingDirection *= -1;
            _FacingRight = !_FacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    #endregion

    #region ROLL/////////////////////////////////////////////

    private void Roll()
    {
        if (_IsGrounded && _IsRolling)
        {
            _anim.StopPlayback();
            _anim.Play("Roll");
        }

    }

    #endregion

    #region WALL_SLIDE & WALL_JUMP /////////////////////////////////////

    private void CheckWallSliding()
    {
        if((_IsTouchingWallOne && _IsTouchingWallTwo) && !_IsGrounded && _rb.velocity.y < 0)
        {
            _IsWallSliding = true;
        }
        else
        {
            _IsWallSliding = false;
        }
    }

    #endregion

    #region RUN & JUMP ///////////////////////////////////////////////

    //RUN//////////////////////////////////////
    private void CheckInput()
    {
        if (!_IsBlockMoveX)
        {
            _MovementInputDirection = Input.GetAxisRaw("Horizontal");
        }

        if ((Input.GetButtonDown("Jump") && _IsGrounded) || (_IsWallSliding && Input.GetButtonDown("Jump")))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _IsGrounded)
        {
            _IsRolling = true;
            Roll();
        }

    }

    private void CheckMovementDirection()
    {
        if (_FacingRight && _MovementInputDirection < 0)
        {
            if (!_IsBlockMoveX)
            {
                Reflect();
            }
        }
        else if (!_FacingRight && _MovementInputDirection > 0)
        {
            if (!_IsBlockMoveX)
            {
                Reflect();
            }

        }
    }
    
    private void ApplyMovement()
    {
        if (_IsGrounded || (!_IsGrounded && !_IsWallSliding && !_IsWallJumping))
        {
            if (!_IsBlockMoveX)
            {
                _rb.velocity = new Vector2(_MovementSpeed * _MovementInputDirection, _rb.velocity.y);
            }
        }        

        if (_IsWallSliding)
        {
            _IsWallJumping = false;
            if (_rb.velocity.y < -_WallSlideSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_WallSlideSpeed);
            }
        }
    }

    //JUMP/////////////////////////////////////
    private void Jump()
    {
        if (_IsGrounded && !_IsWallSliding)
        {
            _IsWallJumping = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _JumpForce);
        }


        else if (_IsWallSliding && _MovementInputDirection == 0)
        {
            _rb.velocity = new Vector2(_MovementForceInAir * -_FacingDirection, _JumpForce );
            _IsWallSliding = false;
            _IsWallJumping = true;
            Reflect();
        }

    }

    #endregion

    #region ATTACK

    public int damage = 1;
    [SerializeField] private LayerMask _Enemy;
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _anim.SetTrigger("Attack_One");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(_AttackPose.position, _AttackCheckRadius, _Enemy);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().Death();
            }
        }
    }

    #endregion

    #region DRAW GIZMOS///////////////////////////////////////////////

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_GroundCheck.position, _GroundCheckRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(_WallCheckOne.position, new Vector3(_WallCheckOne.position.x + _WallCheckDistance, _WallCheckOne.position.y, _WallCheckOne.position.z));
        Gizmos.DrawLine(_WallCheckTwo.position, new Vector3(_WallCheckTwo.position.x + _WallCheckDistance, _WallCheckTwo.position.y, _WallCheckTwo.position.z));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_AttackPose.position, _AttackCheckRadius);
    }

    #endregion
}
   