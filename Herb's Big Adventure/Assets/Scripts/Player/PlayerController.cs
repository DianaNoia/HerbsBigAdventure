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
    [SerializeField] private bool attack = false;

    public float offsetTime = 1f;
    private float timer = 0f;

    public float numberOfJumps;
    public bool canDoubleJump = true;

    [SerializeField] private GameObject headPosition;
    public GameObject hurtBox;

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

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
        hurtBox.SetActive(false);
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

        if (stopMove)
        {
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            charController.Move(moveDirection);
        }

        Crouching();
        Attacking();

        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);
    }

    public void KnockBack()
    {
        isKnocking = true;
        knockBackCounter = knockBackLenght;
        Debug.Log("knocked back");
        moveDirection.y = knockBackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Crouching()
    {
        if (Physics.Raycast(headPosition.transform.position, Vector3.up, 0.5f))
        {
            canStand = false;
            Debug.DrawRay(headPosition.transform.position, Vector3.up, Color.green);
        }
        else
        {
            canStand = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (crouch == true && canStand == true)
            {
                crouch = false; // It also means we are standing
                anim.SetBool("Crouching", false);
                jumpForce = 15f;
                charController.height = 1f;
                charController.center = new Vector3(0f, .58f, 0f);
            }
            else
            {
                crouch = true; // It also means we are crouching
                anim.SetBool("Crouching", true);
                jumpForce = 0f;
                charController.height = .5f;
                charController.center = new Vector3(0f, .47f, 0f);
            }
        }
    }

    public void Attacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hurtBox.activeInHierarchy)
            {
                timer += Time.deltaTime;
                if (timer > offsetTime)
                {
                    timer = 0f;
                    hurtBox.SetActive(true);
                }
            }
            attack = true;
            
            
            anim.SetTrigger("Attacking");
            
            Debug.Log("ATTACK");
        }
    }
}