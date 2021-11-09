using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private int damage;

    public int getDamage()
    {
        return this.damage;
    }
    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
