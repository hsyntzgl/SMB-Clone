using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float accelerationForWalk, accelerationForRun;
    [SerializeField] private float friction;
    [SerializeField] private float maxWalkSpeed, maxRunSpeed;
    [SerializeField] private float jumpForce, fallForce;

    [SerializeField] private float maxJumpingTime;

    [SerializeField] private float fireRate;

    [SerializeField] private Transform fireTransform;
    [SerializeField] private GameObject fireGameObject;

    private float jumpingTime = 0f;
    private float speed;
    private float currentFireCountDown;

    private float moveInput;

    private bool fired = false;

    private bool isControllerActive = true;

    private bool isLookingRight = true;
    private bool readyForJump = true;

    private bool isJumping = false;

    private bool runAndFireKeyPressing;

    private bool leftArrowPressing;
    private bool rightArrowPressing;

    private bool crouchKeyPressing;

    private bool marioCanGoRight = true;
    private bool marioCanGoLeft = true;

    private bool shortJump;
    private bool longJump;
    private bool fall;

    private MarioLevelSystem marioLevelSystem;
    private MarioHead marioHead;
    private MarioFoot marioFoot;

    private Rigidbody2D playerRB;
    private CapsuleCollider2D capsuleCollider2D;

    private Animator animatorController;

    private readonly float slowSpeed = 5f;
    private readonly float gravity = 1f;

    public bool MarioCanGoRight
    {
        set => marioCanGoRight = value;
    }
    public bool MarioCanGoLeft
    {
        set => marioCanGoLeft = value;
    }
    public bool IsControllerActive
    {
        get => isControllerActive;
        set => isControllerActive = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        marioLevelSystem = GetComponent<MarioLevelSystem>();
        marioHead = transform.Find("Mario Head").gameObject.GetComponent<MarioHead>();
        marioFoot = transform.Find("Mario Foot").gameObject.GetComponent<MarioFoot>();

        playerRB = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControllerActive)
        {
            leftArrowPressing = Input.GetKey(KeyCode.LeftArrow);
            rightArrowPressing = Input.GetKey(KeyCode.RightArrow);
            runAndFireKeyPressing = Input.GetKey(KeyCode.Z);
            crouchKeyPressing = marioLevelSystem.CurrentLevel != 0 && Input.GetKey(KeyCode.DownArrow);

            JumpController();
            Fire();
        }
        AnimationController();
    }
    void FixedUpdate()
    {
        Controller();
        if (isControllerActive)
        {
            if (shortJump)
            {
                ShortJump();
            }
            else if (longJump)
            {
                LongJump();
            }
            else if (fall)
            {
                Fall();
            }
        }
    }
    private void Controller()
    {
        if (leftArrowPressing && !rightArrowPressing && !crouchKeyPressing)
        {
            if (marioCanGoLeft)
            {
                if (speed > 0)
                {
                    speed -= (GetAcceleration() + friction) * Time.deltaTime;
                    animatorController.SetBool("Skidding", true);
                }
                else if (speed > -GetMaxSpeed())
                {
                    speed -= GetAcceleration() * Time.deltaTime;
                    animatorController.SetBool("Skidding", false);
                }
                else
                {
                    animatorController.SetBool("Skidding", false);
                    speed += friction * Time.deltaTime;
                }
            }
            else
            {
                speed = 0;
            }
        }
        if (rightArrowPressing && !leftArrowPressing && !crouchKeyPressing)
        {
            if (marioCanGoRight)
            {
                if (speed < 0)
                {
                    speed += (GetAcceleration() + friction) * Time.deltaTime;
                    animatorController.SetBool("Skidding", true);
                }
                else if (speed < GetMaxSpeed())
                {
                    speed += GetAcceleration() * Time.deltaTime;
                    animatorController.SetBool("Skidding", false);
                }
                else
                {
                    speed -= (friction + GetAcceleration()) * Time.deltaTime;
                    animatorController.SetBool("Skidding", false);
                }
            }
            else
            {
                speed = 0;
            }
        }
        if (!(leftArrowPressing || rightArrowPressing) && IsGrounded())
        {
            if (speed > 0)
            {
                speed -= friction * Time.deltaTime;
            }
            else if (speed < 0)
            {
                speed += friction * Time.deltaTime;
            }
            animatorController.SetBool("Skidding", false);
        }
        playerRB.velocity = new Vector2(Mathf.RoundToInt(speed), playerRB.velocity.y);
        CrouchingController();
        Flip();
    }
    private void JumpController()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.X) && readyForJump)
            {
                AudioManager.instance.MarioJump();
                isJumping = true;
                shortJump = true;
            }
            else
            {
                if (!readyForJump)
                {
                    isJumping = false;
                    fall = false;
                    shortJump = false;
                    longJump = false;
                    readyForJump = true;
                    jumpingTime = 0f;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.X) && isJumping && readyForJump && !IsHeadHit())
            {
                if (jumpingTime < maxJumpingTime)
                {
                    jumpingTime += Time.deltaTime;
                    longJump = true;
                }
                else
                {
                    readyForJump = false;
                    longJump = false;
                    fall = true;
                }
            }
            else
            {
                readyForJump = false;
                longJump = false;
                shortJump = false;
                fall = true;
            }
        }
    }
    private void ShortJump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
    }
    private void LongJump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
    }
    private void Fall()
    {
        if (playerRB.velocity.y < 0)
        {
            playerRB.gravityScale = fallForce * gravity;
        }
        else if (playerRB.velocity.y > 0)
        {
            playerRB.gravityScale = (fallForce / 2) * gravity;
        }
    }
    private void CrouchingController()
    {
        if (marioLevelSystem.CurrentLevel > 0)
        {
            if (crouchKeyPressing)
                animatorController.SetBool("Crouching", true);
            else
                animatorController.SetBool("Crouching", false);
        }
    }
    private void Fire()
    {
        if (CanFire())
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (marioLevelSystem.CurrentLevel == 2 && FireBall.fireBallCount < 2)
                {
                    fired = true;

                    AudioManager.instance.FireBall();

                    animatorController.SetTrigger("Fire");

                    GameObject temp = Instantiate(fireGameObject);

                    temp.GetComponent<FireBall>().SetVelocity(isLookingRight);
                    temp.transform.position = fireTransform.position;
                }
            }
        }
    }
    private void Flip()
    {
        if (isLookingRight && Input.GetKey(KeyCode.LeftArrow) || !isLookingRight && Input.GetKey(KeyCode.RightArrow))
        {
            if (IsGrounded())
            {
                isLookingRight = !isLookingRight;
                GetComponent<SpriteRenderer>().flipX = !isLookingRight;

                fireTransform.localPosition = new Vector2(fireTransform.localPosition.x * -1, fireTransform.localPosition.y);
            }
        }
    }
    private void AnimationController()
    {
        float speed = Mathf.RoundToInt(Mathf.Abs(this.speed));

        animatorController.SetBool("Run", leftArrowPressing || rightArrowPressing || speed > 0);
        animatorController.SetBool("Jump", !IsGrounded());
    }
    private float GetMaxSpeed()
    {
        if (runAndFireKeyPressing) return maxRunSpeed;
        return maxWalkSpeed;
    }
    private float GetAcceleration()
    {
        if (runAndFireKeyPressing) return accelerationForRun;
        return accelerationForWalk;
    }
    private bool CanFire()
    {
        if (currentFireCountDown > 0 && fired)
        {
            currentFireCountDown -= Time.deltaTime;
            return false;
        }
        currentFireCountDown = fireRate;
        fired = false;
        return true;
    }
    private void OnBecameInvisible()
    {
        if (Camera.main.transform.position.y > transform.position.y && isControllerActive)
        {
            LevelManager.instance.MarioDead();
        }
    }
    public void WalkSlowSpeed()
    {
        speed = slowSpeed;
        rightArrowPressing = true;
    }
    public void JumpWithOutController()
    {
        ShortJump();
    }
    public void ResetPlayerVelocity()
    {
        playerRB.velocity = Vector3.zero;
        speed = 0;
    }
    public void LookRight()
    {
        isLookingRight = true;
    }
    public void DeactiveAllComponents()
    {
        playerRB.isKinematic = true;
        capsuleCollider2D.enabled = false;
    }
    public bool IsPlayerStop() => Mathf.RoundToInt(playerRB.velocity.x) == 0 && Mathf.RoundToInt(playerRB.velocity.y) == 0;
    public bool IsGrounded() => marioFoot.IsGround;
    public bool IsHeadHit() => marioHead.HeadHit;
}