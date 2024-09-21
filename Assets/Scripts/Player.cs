using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float jumpYVelocity = 15f;
    [SerializeField] float initialVelocity = 4f;
    //[SerializeField] float maxVelocity = 10f;
    //[SerializeField] float smoothTime = 0.5f;
    [SerializeField] float currentVelocity;
    private Vector2 targetVelocity;

    Animator animator;
    Rigidbody2D physics;
    SpriteRenderer sprite;
   
    enum State { Idle, Walk, Jump, Jattack, Fall, Attack, Crouch, Run, Rattack, Kick, Dance, Arrow}

    State state = State.Idle;

    bool isGrounded = false;
    bool jumpInput = false;
    bool attackInput = false;
    bool jattackInput = false;
    bool rattackInput = false;
    bool arrowInput = false;
    bool danceInput = false;
    bool kickInput = false;
    bool isAttacking = false;
    bool crouchInput = false;
    bool runInput = false;


    float horizontalInput = 0f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        
        /*currentVelocity = initialVelocity;
        targetVelocity = new Vector2(initialVelocity, 0f);
        physics.velocity = targetVelocity;*/
    } 

    void Update()
    {
        jumpInput = Input.GetKey(KeyCode.Space);
        attackInput = Input.GetKey(KeyCode.Z);
        jattackInput = Input.GetKey(KeyCode.Z);
        rattackInput = Input.GetKey(KeyCode.Z);
        kickInput = Input.GetKey(KeyCode.X);
        arrowInput = Input.GetKey(KeyCode.A);
        danceInput = Input.GetKey(KeyCode.S);                    
        crouchInput = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        runInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {

        /* Debug.Log(isDashing);

        // i probably should create a Flip() method and place it only in the actions that you can flip
        if (horizontalInput < 0f && (state != State.Slide && state != State.Climb && state != State.Hang) && state != State.Dash)
        {
            sprite.flipX = false;
        }
        else if (horizontalInput > 0f && (state != State.Slide && state != State.Climb && state != State.Hang) && state != State.Dash)
        {
            sprite.flipX = true;
        }*/

        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Walk: WalkState(); break;
            case State.Jump: JumpState(); break;
            case State.Fall: FallState(); break;
            case State.Arrow: ArrowState(); break;
            case State.Kick: KickState(); break;
            case State.Attack: AttackState(); break;
            case State.Jattack: JattackState(); break;
            case State.Rattack: RattackState(); break;
            case State.Crouch: CrouchState(); break;
            case State.Run: RunState(); break;
            case State.Dance: DanceState(); break;
        }
    }


    #region Basic Movement


    void IdleState()
    {
        // actions
        animator.Play("Idle");

        // transitions
        if (isGrounded)
        {
            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (horizontalInput != 0f)
            {
                state = State.Walk;
            }

            else if (horizontalInput != 0f && runInput)
            {
                state = State.Run;
            }

            else if (attackInput)
            {
                isAttacking = true;
                state = State.Attack;
            }
            else if (crouchInput)
            {
                state = State.Crouch;
            }

            else if (danceInput)
            {
                state = State.Dance;
            }

             else if (kickInput)
            {
                state = State.Kick;
            }

             else if (arrowInput)
            {
                state = State.Arrow;
            }

        }
        else 
        {
            state = State.Fall;
        }
    }

    void WalkState()
    {
       


    }


    void JumpState()
    {
        // actions
        animator.Play("Jump");

        physics.velocity = (initialVelocity * horizontalInput * Vector2.right) + (jumpYVelocity * Vector2.up);

        if (jattackInput)
        {
            state = State.Jattack;
        }
        else 
        {
            state = State.Fall;
        }
    }



        void FallState()
    {
        // actions
        physics.velocity = (physics.velocity.y * Vector2.up) + (initialVelocity * horizontalInput * Vector2.right);

        if (physics.velocity.y > 0f)
        {
            animator.Play("Jump");
        }
        else
        {
            animator.Play("Fall");
        }

        // transitions
       
        if (isGrounded)
        {
            if (horizontalInput != 0f && physics.velocity.y == 0f)// Remove this if
            {
                state = State.Walk;
            }
            else
            {
                state = State.Idle;
            }
        }
       
    }


    void CrouchState()
    {
        // actions
        physics.velocity = new Vector2(0, physics.velocity.y);
        animator.Play("Crouch");

        // transitions
        if (isGrounded)
        {
            if (jumpInput)
            {
                state = State.Jump;
            }
            else if (horizontalInput == 0f && !crouchInput)
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
        // actions
       

        // transitions

            if (isGrounded)
            {
                if (horizontalInput == 0)
                {
                    state = State.Idle;
                }
                else if (horizontalInput != 0f)
                {
                    state = State.Walk;
                }
                else if(crouchInput)
                {
                    state = State.Crouch;
                }
                else if (attackInput)
                {
                    isAttacking = true;
                    state = State.Attack;
                }

            }
            else
            {
                state = State.Fall;
            }
        
    }

    #endregion

    #region Dance

     void DanceState()
    {
        // actions
        //physics.velocity = new Vector2(0, physics.velocity.y);
        animator.Play("Dance");

        // transitions
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



    #region Attack


    void KickState()
    {
        // actions
        //physics.velocity = new Vector2(0, physics.velocity.y);
        animator.Play("Kick");

        // transitions
        if (isGrounded)
        {
            state = State.Idle;
        }
        else 
        {
            state = State.Fall;
        }
    }

        void ArrowState()
    {
        // actions
        //physics.velocity = new Vector2(0, physics.velocity.y);
        animator.Play("Arrow");

        // transitions
        if (isGrounded)
        {
            state = State.Idle;
        }
        else 
        {
            state = State.Fall;
        }
    }


    void AttackState()
    {
        // actions

        if(isAttacking && Input.GetKey(KeyCode.Z)) 
        {
            animator.Play("Attack");
        }

        else
        {
            animator.Play("Attack");
        }

        // transitions
        if (!isAttacking)
        {
            if (isGrounded)
            {
                if (jumpInput)
                {
                    state = State.Jump;
                }
                else if (horizontalInput != 0f)
                {
                    state = State.Walk;
                }
                else if (horizontalInput != 0f && runInput)
                {
                    state = State.Run;
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
    }

        void RattackState()
    {
        // actions

        if(isAttacking && Input.GetKey(KeyCode.Z)) 
        {
            animator.Play("Rattack");
        }

        else
        {
            animator.Play("Rattack");
        }

        // transitions
        if (!isAttacking)
        {
            if (isGrounded)
            {
                if (jumpInput)
                {
                    state = State.Jump;
                }
                else if (horizontalInput != 0f)
                {
                    state = State.Walk;
                }
                else if (horizontalInput != 0f && runInput)
                {
                    state = State.Walk;
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
    }

         void JattackState()
    {
        // actions

        if(isAttacking && Input.GetKey(KeyCode.Z)) 
        {
            animator.Play("Jattack");
        }

        else
        {
            animator.Play("Jattack");
        }

        // transitions
        if (!isAttacking)
        {
            if (isGrounded)
            {
                if (jumpInput)
                {
                    state = State.Jump;
                }
                else if (horizontalInput != 0f)
                {
                    state = State.Walk;
                }
                else if (horizontalInput != 0f && runInput)
                {
                    state = State.Walk;
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
    }

    public void EndOfAttack()
    {
        isAttacking = false;
    }


    #endregion

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

        private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    #endregion

}