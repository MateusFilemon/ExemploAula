using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Character : MonoBehaviourPun
{
    [Header("Animations")]
    [SerializeField] protected Animator anim;

    [Header("Life Controller")]
    protected float currentLife;
    [Tooltip("Define a vida máxima do Personagem")]
    [SerializeField] protected float maxLife;
    protected bool dead;

    //Header dá titulo para as variaveis
    [Header("Movement Controller")]
    [SerializeField] protected float moveSpeed;
    protected Vector2 direction;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundDistance;
    protected bool canJump;
    [SerializeField] protected float jumpForce;

    #region Unity Methods
    protected virtual void Awake()
    {
        currentLife = maxLife;
    }

    protected virtual void Update()
    {
        Animations();
    }


    protected virtual void FixedUpdate()
    {
        if (canJump) Jump(jumpForce);
    }
    #endregion

    #region Life Controller
    public virtual void TakeDamage(float _value)
    {
        currentLife = Mathf.Max(currentLife - _value, 0);
        //-= e = currentLife - é igual
        if (currentLife == 0) Death();

    }

    public virtual void Heal(float _value)
    {
        currentLife = Mathf.Min(currentLife + _value, maxLife);
    }

    protected virtual void Death()
    {
        dead = true;
    }
    #endregion
    #region Movement Controller
    protected virtual void Move()
    {

    }

    public virtual void Jump(float _value)
    {
        canJump = false;

    }

    protected virtual bool onGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }
    #endregion

    #region Animations
    protected virtual void Animations() 
    {
        anim.SetBool("Dead", dead);
        anim.SetBool("OnGround", onGround());
    }


    #endregion


}

