using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    private CharacterController controller;
    public Transform cam; 
    [SerializeField] public float speed;
    [SerializeField] public float sprint;
    [SerializeField] public float strafe;
    private Vector3 velocity;

    [SerializeField] private float jumpForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] public float gravity;

    private bool isJumping = false;
    private float jumpTime = 0f;
    private const float maxJumpTime = 0.5f;

    private Animator anim;
    private Rigidbody rb;
    public AudioSource dmgPowerups;

    private bool isBackward = false;

    /// <summary>
    /// Poison Water
    /// </summary>
    private bool isWater = false;
    private float timer = 0f;
    private float waterInterval = 0.5f;

    /// <summary>
    /// PowerUp UI 
    /// </summary>
    public Image coolDownUI;
    public TextMeshProUGUI remainingTime;
    public float cd = 10f;

    public Score score;
    public HP playerHP;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
        dmgPowerups = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.down * gravity);
        Moving();

        timer += Time.deltaTime;
        if (timer >= waterInterval)
        {
            timer = 0f;
            isWater = false;
        }

        coolDownUI.fillAmount = 1;
        remainingTime.text = string.Empty;

    }

    
    private void Moving()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        //Movements 
        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            rb.velocity = transform.forward * speed;
            Walk();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.velocity = transform.forward * sprint;
               //Run();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            rb.velocity = -transform.forward * speed;
            anim.SetBool("back", true);
            isBackward = true;
            anim.SetFloat("BackwardBlend", 0, 0.1f, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.Translate(Vector3.left * strafe * Time.deltaTime);
            rb.velocity = -transform.right * strafe;
            Left();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * strafe * Time.deltaTime);
            rb.velocity = transform.right * strafe;
            Right();
        }
        else
        {
            rb.velocity = Vector3.zero;
            Idle();
            anim.SetBool("back", false);
            isBackward = false;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        } else
        {
            anim.SetBool("isJump", false);

        }

    }


    

    private void Idle()
    {
        anim.SetFloat("Blend", 0, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {
        anim.SetFloat("Blend", 0.25f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        //anim.SetFloat("Blend", 1, 0.1f, Time.deltaTime);
    }

    private void Backwards()
    {
        anim.SetBool("isBackward", true);
    }

    private void Left()
    {
        anim.SetFloat("Blend", 0.75f, 0.1f, Time.deltaTime);
        if (isBackward)
        {
            anim.SetFloat("BackwardBlend", 0.5f, 0.1f, Time.deltaTime);
        } 
    }

    private void Right()
    {
        anim.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
        if (isBackward)
        {
            anim.SetFloat("BackwardBlend", 1, 0.1f, Time.deltaTime);
        } 
    }

    private void Jump()
    {
        //isJumping = true;
        //anim.SetBool("isJump", true);
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        isJumping = true;
        anim.SetBool("isJump", true);

        // Calculate the vertical velocity required to achieve the desired jump height
        float jumpVelocity = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics.gravity.y));

        // Calculate the upward force based on the character's mass
        float upwardForce = jumpVelocity * rb.mass;

        // Calculate the impulse required to achieve the desired upward force
        float impulse = upwardForce - rb.velocity.y * rb.mass;

        // Apply the upward force to the character
        rb.AddForce(Vector3.up * impulse, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //isGrounded = true;
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isGrounded = true;
        }
        

        if (collision.gameObject.CompareTag("Powerups"))
        {
            ParticleMove.isPowered = true;
            dmgPowerups.Play();
            Destroy(collision.gameObject);
            StartCoroutine(StartCooldown());
        }

        if (playerHP.healthBar.value < 100.0f)
        {
            if (collision.gameObject.CompareTag("HPups"))
            {
                gameObject.GetComponent<AttributeManager>().TakeDamage(-50);
                FindObjectOfType<Audios>().PlaySound("heal");
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Portal"))
        {
            score.DoneGame();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            if (!isWater)
            {
                gameObject.GetComponent<AttributeManager>().TakeDamage(2);
                isWater = true;
                FindObjectOfType<Audios>().PlaySound("maleHurt");
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        float timePassed = 0f;
        while (timePassed < cd)
        {
            timePassed += Time.deltaTime;
            float fillAmount = 1 - (timePassed / cd);
            coolDownUI.fillAmount = fillAmount;
            remainingTime.text = (cd - timePassed).ToString("F2"); // Display the remaining cooldown time //Math.ceil (cd - timePassed) for whole number
            yield return null;
        }
        coolDownUI.fillAmount = 1f;
        remainingTime.text = string.Empty; // Reset the cooldown text
        ParticleMove.isPowered = false;
    }
}
