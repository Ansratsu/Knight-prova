using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class Player2 : MonoBehaviour
{

    [SerializeField] float jumpYVelocity = 1f;
    [SerializeField] float initialVelocity = 4f;
    [SerializeField] float currentVelocity;
    private Vector2 targetVelocity;


    Animator animator;
    Rigidbody2D physics;
    SpriteRenderer sprite;

    enum State { Idle, Dance, Kick, Attack, Arrow, Jump, Fall, Jattack, Jattack2, Walk, Run, Rattack, Roll, Lay, Fireball}

    State state = State.Idle;

    bool isGrounded = false; // Se toca o chão
    bool danceInput = false; // S
    bool jumpInput = false; // Seta pra cima
    bool kickInput = false; // X
    bool attackInput = false; // Z
    bool arrowInput = false; // A
    bool fireballInput = false; // A
    bool runInput = false; //Shift
    bool downImput = false; //Space
    bool isAttacking = false; // Se o personagem ta atacando

    private bool m_FacingRight = true;

    float attackTimer = 0f; // Temporizador da animação
    float horizontalInput = 0f;
    public float movementSpeed = 6f;

    public Transform firepoint;
    public GameObject arrowprefab;
    public GameObject fireballprefab;


    void OnCollisionEnter2D(Collision2D collision) //check if isgrounded
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }    

    void Update()
    {
        danceInput = Input.GetKey(KeyCode.S);                    
        kickInput = Input.GetKey(KeyCode.X);
        attackInput = Input.GetKey(KeyCode.Z);
        arrowInput = Input.GetKey(KeyCode.C);
        fireballInput = Input.GetKey(KeyCode.V);
        jumpInput = Input.GetKey(KeyCode.UpArrow);
        downImput = Input.GetKey(KeyCode.Space);
        downImput = Input.GetKey(KeyCode.DownArrow);
        runInput = Input.GetKey(KeyCode.LeftShift);
        horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontalInput * movementSpeed, physics.velocity.y);       
        physics.velocity = movement;

        if(horizontalInput != 0f)
        {
            Flip();
        }   
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Dance: DanceState(); break;
            case State.Lay: LayState(); break;
            case State.Kick: KickState(); break;
            case State.Attack: AttackState(); break;
            case State.Jattack: JattackState(); break;
            case State.Rattack: RattackState(); break;
            case State.Jattack2: Jattack2State(); break;
            case State.Fireball: FireballState(); break;
            case State.Arrow: ArrowState(); break;
            case State.Jump: JumpState(); break;
            case State.Fall: FallState(); break;
            case State.Run: RunState(); break;
            case State.Walk: WalkState(); break;
            case State.Roll: RollState(); break;
        }
    }

    #region Idle

    void IdleState()
    {
        animator.Play("Idle");

        if (isGrounded)
        {
            if (danceInput)
            {
                state = State.Dance;
            }
            else if (downImput)
            {
                state = State.Lay;
            }
            else if (kickInput)
            {
                isAttacking = true;
                state = State.Kick;
            }
            else if (fireballInput)
            {
                isAttacking = true;
                state = State.Fireball;
            }
            else if (jumpInput)
            {
                Vector2 movement = initialVelocity * horizontalInput * Vector2.right;
                Vector2 jump = jumpYVelocity * Vector2.up;
                physics.velocity = movement + jump;

                isGrounded = false;                
                state = State.Jump;
            }
            else if (horizontalInput != 0f) //--------------------------------------------------------------------------------
            {

                state = State.Walk;
            }
            else if (horizontalInput != 0f && runInput) //--------------------------------------------------------------------------------
            {
                state = State.Run;
            }
            else if (attackInput)
            {
                isAttacking = true;
                state = State.Attack;
            }
            else if (arrowInput)
            {
                isAttacking = true;
                state = State.Arrow;
            }
            else if (physics.velocity.y < 0f)
            {
                isGrounded = false;
                state = State.Fall;
            }
        }
        else
        {
            state = State.Fall;
        }
    }

    #endregion

    #region Basic Movement

    void JumpState()
    {
        animator.Play("Jump");

        if (physics.velocity.y < 0 )
        {
            state = State.Fall;
        }
        else if (attackInput)
        {
            isAttacking = true;           
            state = State.Jattack;
        }
        else if (arrowInput)
        {
            isAttacking = true;           
            state = State.Arrow;
        }
        else if (isGrounded == true)
        {
            state = State.Idle;
        }
    }

    void WalkState()
    {
        animator.Play("Walk");

        if(isGrounded)
         {
            if (jumpInput)
            {
                state = State.Jump;
            }
            if (runInput)
            {
                state = State.Run;
            }
            if (downImput)
            {
                state = State.Roll;
            }
            else if (arrowInput)
            {
                isAttacking = true;
                state = State.Arrow;
            }
            else if (kickInput)
            {
                isAttacking = true;
                state = State.Kick;
            }
            else if (attackInput && physics.velocity.y < 0f)
            {
                isGrounded=false;
                isAttacking = true;
                state = State.Jattack;
            }
            else if (attackInput)
            {
                isAttacking = true;
                state = State.Attack;
            }
            else if (horizontalInput == 0f)
            {
                state = State.Idle;
            }   

         } 
        
        else
        {
            state = State.Fall;
        }  

    }
    
    void RunState()
    {
        animator.Play("Run");

        Vector2 movement = new Vector2(horizontalInput * movementSpeed * 1.5f, physics.velocity.y);       
        physics.velocity = movement;

        if(isGrounded)
         {
            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (attackInput && physics.velocity.y < 0f)
            {
                isGrounded=false;
                isAttacking = true;
                state = State.Jattack;
            }
            else if (attackInput)
            {
                isAttacking = true;
                state = State.Rattack;
            }
            else if (horizontalInput == 0f)
            {
                state = State.Idle;
            }

         } 
        
        else
        {
            state = State.Fall;
        }  

    }
    
    void RollState()
    {
        animator.Play("Roll");

        if(isGrounded)
         {
            /*if (jumpInput)
            {
                state = State.Jump;
            }
            else */
            if (attackInput && physics.velocity.y < 0f)
            {
                isGrounded=false;
                isAttacking = true;
                state = State.Jattack;
            }
            else if (attackInput)
            {
                isAttacking = true;
                state = State.Jattack2;
            }
            else if (horizontalInput == 0f)
            {
                state = State.Idle;
            }   

         } 
        
        else
        {
            state = State.Fall;
        }  

    }

    void FallState()
    {

        if (isGrounded == false)
        {
        animator.Play("Fall");
        }
        if (isGrounded == true)
        {
            animator.Play("Falled");
            attackTimer += Time.deltaTime;


            if (attackTimer >= 0.2f)
            {
            EndOfAttack();
            }
        } 

        if (attackInput)
        {
            isAttacking = true;           
            state = State.Jattack;
        }
        else if (arrowInput)
        {
            isAttacking = true;           
            state = State.Arrow;
        }


    }




    #endregion

    #region Attacks

    void AttackState()
    {
        animator.Play("Attack");

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            physics.constraints = RigidbodyConstraints2D.FreezePositionX;     
            physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            if (attackTimer >= 1f) //Se eu soubesse como poderia deixar para ser o mesmo da duração da animação. Não encontrei como.
            {
                EndOfAttack();
            }
        }

    }
    
    void JattackState()
    {
        if (physics.velocity.y >= 0f)
        {
            animator.Play("Jattack0");
        }
        else
        {
            animator.Play("Jattack");
        }

        if (isAttacking)
        {
            
            if (isGrounded) 
            {
                state = State.Jattack2;
            }
        }

    }

    void Jattack2State()
    {
        animator.Play("Jattack2");
        physics.constraints = RigidbodyConstraints2D.FreezePositionX;     
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        if (isAttacking)
        {
            
            if (isGrounded) 
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= 0.58f)
                {
                EndOfAttack();
                }
            }
        }

    }

    void RattackState()
    {
        animator.Play("Rattack");

        
        if (isAttacking)
        {
            
            if (isGrounded) 
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 0.5f)
                {
                physics.constraints = RigidbodyConstraints2D.FreezePositionX;     
                physics.constraints = RigidbodyConstraints2D.FreezeRotation;    
                }
                if (attackTimer >= 0.917f)
                {
                EndOfAttack();
                }
            }
        }

    }

    void ArrowState()
    {
        animator.Play("Arrow");

        if (isGrounded)
        {
        physics.constraints = RigidbodyConstraints2D.FreezePositionX;     
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;

        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

             if (attackTimer >= 0.7f && attackTimer <= 0.72f) 
            {
                Shootarrow();               
            }

            if (attackTimer >= 0.917f) //Se eu soubesse como poderia deixar para ser o mesmo da duração da animação. Não encontrei como.
            {

                EndOfAttack();
            }
        }

    }

    void FireballState()
    {
        animator.Play("Fireball");

        physics.constraints = RigidbodyConstraints2D.FreezePositionX;     
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            
             if (attackTimer >= 0.5f && attackTimer <= 0.52f) 
            {
                Shootfireball();           
            }

            if (attackTimer >= 0.667f) //Se eu soubesse como poderia deixar para ser o mesmo da duração da animação. Não encontrei como.
            {
                EndOfAttack();
            }
        }
    }
     
    void KickState()
    {
        animator.Play("Kick");

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            
            if (attackTimer >= 0.75f) //Se eu soubesse como poderia deixar para ser o mesmo da duração da animação. Não encontrei como.
            {
                EndOfAttack();
            }
        }

    }

    public void EndOfAttack()
    {
        isAttacking = false;
        attackTimer = 0f;
        physics.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (isGrounded)
        {
        state = State.Idle;
        }
        else 
        {

            state = State.Fall;
        }

    }

    #endregion

    #region LongRweapons

    void Shootarrow()
    {
        Instantiate(arrowprefab, firepoint.position, firepoint.rotation);
    }

    void Shootfireball()
    {
        Instantiate(fireballprefab, firepoint.position, firepoint.rotation);
    }

    #endregion

    #region Dance

    void DanceState()
    {
        animator.Play("Dance");

        physics.constraints = RigidbodyConstraints2D.FreezePositionX;
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!isGrounded || !danceInput)
        {
            physics.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            state = State.Idle;
        }
    }

    void LayState()
    {
        animator.Play("Lay");
        physics.constraints = RigidbodyConstraints2D.FreezePositionX;
        physics.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!isGrounded || !downImput)
        {
            physics.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            state = State.Idle;
        }
    }      

    #endregion

    private void Flip()
    {
        if(m_FacingRight == true && horizontalInput < 0f)
        {
        m_FacingRight = !m_FacingRight;

        /*Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/

        transform.Rotate(0f, 180f, 0f);
        }
        
        else if(m_FacingRight == false && horizontalInput > 0f)
        {
        m_FacingRight = !m_FacingRight;

        /*Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/

        transform.Rotate(0f, 180f, 0f);
        }
        else
        {

        }
    }

}
