using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float bounceForce = 8f;

    public float crouchSpeed;
    [SerializeField] private bool canStand;
    [SerializeField] private bool crouch = false;
    [SerializeField] private GameObject headPosition;

    public float numberOfJumps;
    public bool canDoubleJump = true;

    private Vector3 moveDirection;

    public CharacterController charController;

    private Camera theCam;

    public GameObject playerModel;
    public float rotateSpeed;

    public Animator anim;

    public bool isKnocking;
    public float knockBackLenght = .5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    public GameObject[] playerPieces;

    public bool stopMove;

    // Timer for the charged attack
    private float holdDownTime = 0;

    // Bools to check which attacked occured
    public bool    attackedNormal = false, 
                   attackedWithWeapon = false, 
                   attackedCharged = false;

    // Colliders for each of the attacks
    public GameObject hurtBox, weaponHurtBox, chargedHurtBox;

    // Bool to check if player has picked up weapon
    public bool hasWeapon = false;

    // Weapon GameObject
    public GameObject weapon;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;

        hurtBox.SetActive(false);
        weaponHurtBox.SetActive(false);
        chargedHurtBox.SetActive(false);

        anim = GetComponent<Animator>();

        AddEvent(5, 0f, "EnableEnemyHurtBox", 0);
        AddEvent(5, .4f, "DisableEnemyHurtBox", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnocking && !stopMove)
        {
            float yStore = moveDirection.y;
            // moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection = moveDirection * moveSpeed;
            moveDirection.y = yStore;

            // Jump
            if (charController.isGrounded)
            {
                canDoubleJump = true;
                moveDirection.y = -1f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    numberOfJumps = 1;

                    Debug.Log("JUMP");
                }
            }
            // Código para melhorar o salto dado pelo prof Diogo, para futura implementação 

            //else if ((Input.GetButton("Jump")) && (numberOfJumps == 1))
            //{
            //    moveDirection.y += Physics.gravity.y * 0.2f * Time.deltaTime * gravityScale;
            //}
            else
            {
                moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
                if (canDoubleJump && Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    canDoubleJump = false;
                    Debug.Log("DOUBLE JUMP");
                }
            }

            charController.Move(moveDirection * Time.deltaTime);

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }

        // When knockback of player happens
        if (isKnocking)
        {
            knockBackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection = playerModel.transform.forward * -knockBackPower.x;
            moveDirection.y = yStore;

            if (charController.isGrounded)
            {
                moveDirection.y = -1f;
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection * Time.deltaTime);

            if (knockBackCounter <= 0)
            {
                isKnocking = false;
            }
        }

        // Stop moving the player
        if (stopMove)
        {
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            charController.Move(moveDirection);
        }

        // Call the crouch and attack when they are activated
        Crouching();
        Attacking();
        AttackingWithWeapon(); 
        SwingAttackWithWeapon();

        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);

        // Checks if player has weapon active or not
        if (hasWeapon == true)
        {
            weapon.SetActive(true);
        }
        else
        {
            weapon.SetActive(false);
        }

        // Activates weapon just in case end of level doesnt work
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            hasWeapon = true;
        }
    }

    // Knockback of player
    public void KnockBack()
    {
        isKnocking = true;
        knockBackCounter = knockBackLenght;
        Debug.Log("knocked back");
        moveDirection.y = knockBackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }

    //// Bounce of the player on enemies head
    //public void Bounce()
    //{
    //    moveDirection.y = bounceForce;
    //    charController.Move(moveDirection * Time.deltaTime);
    //}

    // Crouching
    public void Crouching()
    {
        // Raycast when the player is under something and he can't stand up,
        // used for debugging 
        if (Physics.Raycast(headPosition.transform.position, Vector3.up, 0.5f))
        {
            canStand = false;
            Debug.DrawRay(headPosition.transform.position, Vector3.up, 
                Color.green);
        }
        else
        {
            canStand = true;
        }

        // When key pressed, checks if can crouch or not
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("Crouching");
            if (crouch == false && canStand == true)
            {

                Debug.Log(" can Crouch");
                crouch = true; // It also means we are crouching
                anim.SetBool("Crouching", true);

                jumpForce = 0f;
                moveSpeed = 2.5f;

                charController.height = .5f;
                charController.center = new Vector3(0f, .3f, 0f);
            }
        }
        else
        {
            if (crouch == true && canStand == true)
            {
                Debug.Log(" cant Crouch");
                crouch = false; // It also means we are standing
                anim.SetBool("Crouching", false);

                jumpForce = 15f;
                moveSpeed = 5f;

                charController.height = 1f;
                charController.center = new Vector3(0f, .58f, 0f);
            }
            else if (crouch == true && canStand == false)
            {
                crouch = false; // It also means we are crouching
                anim.SetBool("Crouching", true);

                jumpForce = 0f;
                moveSpeed = 2.5f;

                charController.height = .5f;
                charController.center = new Vector3(0f, .3f, 0f);
            }
        }
    }

    // Attack basick
    public void Attacking()
    {
        if (!hasWeapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Attacking");
                Debug.Log("ATTACK");

                attackedNormal = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                attackedNormal = false;
            }
        }
    }

    // Attack with weapon
    public void AttackingWithWeapon()
    {
        if (hasWeapon == true)
        {
            Debug.Log("Has weapon");
            
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("AttackingWeapon");
                Debug.Log("ATTACK WITH WEAPON");

                attackedWithWeapon = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                attackedWithWeapon = false;
            }
        }
    }

    // Swing attack, charged and only with weapon
    public void SwingAttackWithWeapon()
    {
        if (hasWeapon == true)
        {
            if (Input.GetMouseButton(1))
            {
                holdDownTime += Time.deltaTime;
            }

            if ((Input.GetMouseButtonUp(1)) && (holdDownTime > 2))
            {
                anim.SetTrigger("SwingAttack");
                Debug.Log("SWING ATTACK");

                attackedCharged = true;
            }

            if ((Input.GetMouseButtonUp(1)) && (holdDownTime < 2))
            {
                holdDownTime = 0;

                attackedCharged = false;
            }
        }        
    }

    // NORMAL attack hurtbox
    public void EnableEnemyHurtBox()
    {
        hurtBox.SetActive(true);
    }
    public void DisableEnemyHurtBox()
    {
        hurtBox.SetActive(false);
    }

    // WEAPON attack hurtbox
    public void EnableWeaponEnemyHurtBox()
    {
        weaponHurtBox.SetActive(true);
    }
    public void DisableWeaponEnemyHurtBox()
    {
        weaponHurtBox.SetActive(false);
    } 
    
    // CHARGED attack hurtbox
    public void EnableChargedEnemyHurtBox()
    {
        chargedHurtBox.SetActive(true);
    }
    public void DisableChargedEnemyHurtBox()
    {
        chargedHurtBox.SetActive(false);
    }

    // Animation events 
    void AddEvent(int Clip, float time, string functionName, float floatParameter)
    {
        anim = GetComponent<Animator>();
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.floatParameter = floatParameter;
        animationEvent.time = time;
        AnimationClip clip = anim.runtimeAnimatorController.animationClips[Clip];
        clip.AddEvent(animationEvent);
    }
}