using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; 
    
    private AudioManager am;

    private LevelExit levelExit;

    public int jumpAudio;


    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float bounceForce = 8f;

    //public float crouchSpeed;
    //[SerializeField] private bool canStand;
    //[SerializeField] private bool crouch = false;
    //[SerializeField] private GameObject headPosition;

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
    public bool attacked; //attackedCharged;

    // Colliders for each of the attacks
    public GameObject hurtBox; //chargedHurtBox;

    // Bool to check if player has picked up weapon
    public bool hasWeapon;

    // Weapon GameObject
    public GameObject weapon;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();

        theCam = Camera.main;

        attacked = false;

        hurtBox.SetActive(false);

        anim = GetComponent<Animator>();

        //hasWeapon = false;

        //AddEvent(5, 0f, "EnableEnemyHurtBox", 0);
        //AddEvent(5, .4f, "DisableEnemyHurtBox", 0);

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

                    am.PlaySFX(jumpAudio);
                }
            }
            else
            {
                moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
                if (canDoubleJump && Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    canDoubleJump = false;
                    Debug.Log("DOUBLE JUMP");
                    am.PlaySFX(jumpAudio);
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

        Attacking();

        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);

        // Checks if player has weapon active or not
        if (hasWeapon)
        {
            //weaponManager.weapon.SetActive(true);

            weapon.SetActive(true);
        }
        else
        {
            //weaponManager.weapon.SetActive(false);

            weapon.SetActive(true);
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

    // Attack basick
    public void Attacking()
    {
        if (hasWeapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Attacking");
                
                attacked = true;
                hurtBox.SetActive(true);

                Debug.Log("ATTACK");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                attacked = false;
                hurtBox.SetActive(false);
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