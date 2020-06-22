using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFood : MonoBehaviour
{
    public int shellHealth;
    public int energy;

    
    public void TakeDamage(int damage)
    {
        shellHealth -= damage;
        if (shellHealth < 0) Die();
    }



    public void TakeBluntDamage(int damage)
    {
        TakeDamage(damage);
        
    }

    public void Die()
    {

        GameObject obj = Instantiate(FoodSpawn.MOVABLE_FOOD, transform.position, new Quaternion());
        obj.GetComponent<Food>().energyValue = energy;

        Destroy(transform.gameObject);
    }

}
