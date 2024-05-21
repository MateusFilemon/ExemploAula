using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Player : Character
{
    protected Rigidbody rb;

    protected float rotY;
    [SerializeField] protected float rotSpeed;


    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        if(photonView.IsMine) Camera.main.gameObject.SetActive(false);

        Debug.Log("A new player has arrived!");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
    }

    protected override void Update()
    {
        base.Update();

        if (!photonView.IsMine) return;

        PlayerInputs();
        RotatePlayer();
        //unity! executa esse metodo ai!
    }
    #endregion

    protected virtual void PlayerInputs()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        //y representa z(ir pra frente) , o z de um vector2 é sempre 0
        //GetAxisRaw = 0, 1 ou -1 / GetAxis = 0,5 e 1 (para transições fluidas de movimento)

        rotY = Input.GetAxisRaw("Mouse X");

        if(Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && onGround()) 
        {
            Attack();
        }

        if (Input.GetButtonDown("Jump") && onGround() && !attacking) canJump = true;
        // getbutton = se pressiona um botão , getbuttondown = se pressiona uma vez , getbuttonup = se soltou uma tecla
    }

    #region Attack System

    void Attack() 
    {
        //CanAttack(false);
        attacking = true;
        //Quando ele pode atacar. 0.5 para 2 segundos(1 divido por 0.5 dá 2), 0.25 para 4 segundos
        
        nextAttackTime = Time.time + 1f/attackSpeed;
        //Invoke = quer executar um metodo depois de um tempo
        Invoke("AllowToAttackAgain", 1f / attackSpeed);
        photonView.RPC("AttackPun", RpcTarget.All);
        //podia ser nameof(AttackPun)
    }

    void AllowToAttackAgain() 
    {
        attacking = false;
    }

    [PunRPC]

    void AttackPun() 
    {
        anim.SetTrigger("MeleeAttack");
        Invoke("TryToHitEnemy", attackDelay);
    }

    void TryToHitEnemy() 
    {
        var _enemies = Physics.OverlapSphere(posAttack.position, attackRange, enemyLayer);

        foreach(var _enemy in _enemies) 
        {
            Debug.Log("Acerto o inimgo: " + _enemy.name);
        }
    }

    #endregion

    #region Movement
    protected override void Move()
    {
        base.Move();

        //Vector3 _velocity = new Vector3(direction.x * moveSpeed,rb.velocity.y,direction.y * moveSpeed); Não pode ser assim!
        Vector3 _velocity = (transform.right * direction.x + transform.forward * direction.y) * moveSpeed;
        _velocity.y = rb.velocity.y; //sem isso seria tudo zero

        if (attacking)
        {
            _velocity.x = 0f;
            _velocity.z = 0f;
            //anula o input, anula a movimentação
        }

        rb.velocity = _velocity;


    }

    protected virtual void RotatePlayer() 
    {
        transform.Rotate(0, rotY * rotSpeed * Time.deltaTime, 0f);
        //esse time.deltatime equaliza(todos viram 1) a rotaçao independente da quantidade de frames da maquina
    }

    public override void Jump(float _value)
    {
        base.Jump(_value);

        Vector3 _velocity = rb.velocity;
        _velocity.y = 0f;

        rb.velocity = _velocity;

        rb.AddForce(Vector3.up * _value);

    }
    #endregion

    protected override void Animations()
    {
        base.Animations();

        anim.SetFloat("SpeedX", direction.x);
        anim.SetFloat("SpeedZ", direction.y);
    }

}
