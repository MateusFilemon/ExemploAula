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

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Move()
    {
        base.Move();

        //Vector3 _velocity = new Vector3(direction.x * moveSpeed,rb.velocity.y,direction.y * moveSpeed); Não pode ser assim!
        Vector3 _velocity = (transform.right * direction.x + transform.forward * direction.y) * moveSpeed;
        _velocity.y = rb.velocity.y; //sem isso seria tudo zero

        rb.velocity = _velocity;


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

    protected virtual void PlayerInputs() 
    {
        

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        //y representa z(ir pra frente) , o z de um vector2 é sempre 0
        //GetAxisRaw = 0, 1 ou -1 / GetAxis = 0,5 e 1 (para transições fluidas de movimento)

        rotY = Input.GetAxisRaw("Mouse X");

        if (Input.GetButtonDown("Jump") && onGround()) canJump = true;
        // getbutton = se pressiona um botão , getbuttondown = se pressiona uma vez , getbuttonup = se soltou uma tecla
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

    protected override void Animations()
    {
        base.Animations();

        anim.SetFloat("SpeedX", direction.x);
        anim.SetFloat("SpeedZ", direction.y);
    }

}
