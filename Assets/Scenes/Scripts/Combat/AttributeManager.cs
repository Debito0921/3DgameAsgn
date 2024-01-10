using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public int health;
    public int attack;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.2f), Random.Range(0.5f, 1.5f), Random.Range(-0.2f, -0.5f));
        DmgGenerator.current.CreatePopUp(transform.position + randomness, amount.ToString(), Color.black);
    }

    public void DealDamage (GameObject target)
    {
        var atm = target.GetComponent<AttributeManager>();
        if (atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
}
