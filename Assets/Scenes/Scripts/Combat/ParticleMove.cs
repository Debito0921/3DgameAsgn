using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleMove : MonoBehaviour
{
    public float speed = 20f;
    public float angle = 4.0f;
    public int magicBotDmg = 20;
    public int powerDmg = 40;
    public float lifetime = 2.0f;

    public static bool isPowered = false;

    private float powerupDuration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 direction = rotation * transform.forward;
        transform.position += direction * (speed * Time.deltaTime);

        if (isPowered)
        {
            //StartCoroutine(PowerupTimer()); 
        }

    }

    private IEnumerator PowerupTimer()
    {
        isPowered = true;

        yield return new WaitForSeconds(powerupDuration);

        isPowered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IDamage damage = null;
            if (other.TryGetComponent(out damage))
            {
                if (isPowered)
                {
                    damage.DealDamage(powerDmg);
                }
                else
                {
                    damage.DealDamage(magicBotDmg);
                }
                FindObjectOfType<Audios>().PlaySound("EnemyHurt");
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
