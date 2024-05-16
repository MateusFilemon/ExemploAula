using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class Character : MonoBehaviourPun
{
    [Header("Animations")]
    [SerializeField] protected Animator anim;

    [Header("Life Controller")]
    protected float currentLife;
    [Tooltip("Define a vida m�xima do Personagem")]
    [SerializeField] protected float maxLife;
    protected bool dead;
    [SerializeField] Image life;

    //Header d� titulo para as variaveis
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
        UpdateLifeInPhoton();
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
    
    protected void UpdateLifeInPhoton() 
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateLife", RpcTarget.AllBuffered, currentLife);
            //mostrar a vida para todos
            //currentLife precisa estar pois � um metodo que pede um valor externo 
        }
    }
    
    [PunRPC]
    protected void UpdateLife(float _currentLife) 
    { 
        currentLife = _currentLife;
        life.fillAmount = currentLife / maxLife;
    }

    public virtual void TakeDamage(float _value)
    {
        currentLife = Mathf.Max(currentLife - _value, 0);
        //-= e = currentLife - � igual
        if (currentLife == 0) Death();

        UpdateLifeInPhoton();

    }

    public virtual void Heal(float _value)
    {
        currentLife = Mathf.Min(currentLife + _value, maxLife);
        
        UpdateLifeInPhoton();
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

