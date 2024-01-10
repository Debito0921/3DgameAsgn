using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstantiatePat : MonoBehaviour
{
    public Transform firepoint;
    public GameObject magicBallPrefab;
    [SerializeField] public float cd;
    public float currentCD = 0;
    [SerializeField] public float speed;
    [SerializeField] public float rotationAngle;

    private bool canShoot = true;

    private Animator anim;
    public AudioSource castSound;

    //coolDownUI
    public Image coolDownUI;
    public TextMeshProUGUI remainingTime;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        castSound = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        bool isAttack = anim.GetBool("isAttack");
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
            coolDownUI.fillAmount = 0;
            remainingTime.text = string.Empty;
        }
    }

    public void ShootPrefabs()
    {
        currentCD += Time.deltaTime;
        canShoot = false;
        StartCoroutine(StartCooldown());

        // Get the position and rotation of the firepoint
        Vector3 position = firepoint.position;
        Quaternion rotation = firepoint.rotation;

        Quaternion leftRotation = Quaternion.Euler(0f, rotationAngle, 0f);
        rotation *= leftRotation;

        // Instantiate the Magic Ball particle system at the firepoint position and rotation
        GameObject magicBall = Instantiate(magicBallPrefab, position, rotation);
        castSound.Play();


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
        coolDownUI.fillAmount = 0f;
        remainingTime.text = string.Empty; // Reset the cooldown text
        canShoot = true;
    }
}
