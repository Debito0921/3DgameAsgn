using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour, IDamage
{
    
    public Slider healthBar;
    private AttributeManager atm;

    public void DealDamage(int damage)
    {
        atm.TakeDamage(damage);

    }

    //private int Health = 100;
    // Start is called before the first frame update
    void Start()
    {
        atm = GetComponent<AttributeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = atm.health;
        //avoid health overload
        if (atm.health >= 100)
        {
            atm.health = 100;
        }
    }
  
}
