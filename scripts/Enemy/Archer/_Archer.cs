using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Archer : MonoBehaviour
{
    #region Default
    //Аниматор, Физика, СпрайтРендер.
    public Animator _anim;
    public Rigidbody2D _rb;
    public SpriteRenderer _sr;


    void Start()
    {
        box = GetComponents<BoxCollider2D>()[0];
        _Player = FindObjectOfType<Player>();
    }

    void Update()
    {
        //Jump();
        Death();
        Reflect();
    }

    Player _Player;
    [SerializeField] private bool faceRigth;
    void Reflect()
    {
        if ((_Player.transform.position.x < transform.position.x) && (faceRigth))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRigth = !faceRigth;
        }
        else if ((_Player.transform.position.x > transform.position.x) && (!faceRigth))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRigth = true;
        }
    }
    #endregion

    #region Jump
    public LayerMask Detected;
    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius = 0.09161574f;

    [SerializeField] private float jumpforce = 2f;

    /*void Jump()
    {
       if( onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Detected))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpforce);
        }

    }*/
    #endregion

    #region Patrolling


    void Potrling()
    {

    }
    #endregion 

    #region Health_Death
    [SerializeField] private float Health = 100;
    BoxCollider2D box;


    void Death()
    {
        if (Health <= 0)
        {
            _anim.StopPlayback();
            _anim.Play("death");
            _rb.bodyType = RigidbodyType2D.Static;
            this.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    #endregion

    #region Take_Damage

    public void TakeDamage(int damage)
    {
        _anim.StopPlayback();
        _anim.Play("hit");
        Health -= damage;
    }

    #endregion

    #region Attack

    #endregion


}
