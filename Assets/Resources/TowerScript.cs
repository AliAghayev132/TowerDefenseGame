using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    //Variables

    [SerializeField]    
    private Transform shootingPosition;

    [SerializeField]   
    private Transform head;
       
    [SerializeField]   
    private Transform radar;


    private List<Transform> Enemies;
    private Transform currentEnemy;

    private float nextShootTime = 0f;


    private bool isShoot = false;

    //Tower's variables
    public int towerUpgradePrice = 5;

    public int towerLevel = 1;

    [SerializeField]   
    public float towerFireRate = 1f;

    public int towerDamage = 10;

    private Vector3 moveDirArrow;
    private float angle;


    //Functions
    private void Start()
    {
        Enemies = new List<Transform>();
    }

    //Shooting system
    public void inRange(Collider2D other)
    {
       if(!other.GetComponent<EnemyMovement>().isDead) 
        {
            Enemies.Add(other.gameObject.transform);
            currentEnemy = Enemies[0];
            isShoot = true;
        }
    }
    public void outRange(Collider2D other)
    {
        Enemies.Remove(other.gameObject.transform);
        if(Enemies.Count > 0)
        {
             currentEnemy = Enemies[0];
        }
        else
        {
            currentEnemy = null;
            isShoot = false;
        }
    }

    public void upgrade()
    {
            towerUpgradePrice *= 2; //Upgrade qiymeti qalxir
            towerFireRate -= 0.1f; //Vurma surreti
            towerDamage +=5; //Vurma gucu
            towerLevel++;
    }

    //Accessors
    public  int getTowerDamage()
    {
        return towerDamage;
    }
    public int getTowerUpgrade()
    {
        return towerUpgradePrice;
    }
    public int getTowerLevel()
    {
        return towerLevel;
    }
    private float getTowerFireRate()
    {
        return towerFireRate;
    }

    private void Update()
    {
        if(currentEnemy !=null)
        {
            moveDirArrow = (currentEnemy.position - head.position).normalized;
            angle = Custom.getAngleFromVectorFloat(moveDirArrow);
        }


        if(isShoot)
        {
            if(Time.time > nextShootTime)
            {
                shoot();
                nextShootTime = Time.time + towerFireRate;
            }

        }


        if(Input.GetKey(KeyCode.Z)) //Radar sistemi Sonra key time elave ele
        {
            if(radar.GetComponent<SpriteRenderer>().enabled)
            {
                radar.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                radar.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

    }

    private void shoot()
    {
        if((currentEnemy == null && isShoot) || currentEnemy.GetComponent<EnemyMovement>().isDead)
        {
            for(int i=0;i<Enemies.Count;i++)
            {
                if(Enemies[i] == null || Enemies[i].GetComponent<EnemyMovement>().isDead)
                {
                    Enemies.RemoveAt(i);
                }

            }

            if(Enemies.Count > 0)
            {
                currentEnemy = Enemies[0];
            
            }
            else
            {
                isShoot = false;
            }
        }
        if(isShoot)
            createArrow();
    }

    private void createArrow()
    {
         GameObject newArrow = (GameObject)Instantiate(Resources.Load("Arrow"),shootingPosition.position,Quaternion.identity);
         newArrow.GetComponent<ArrowScript>().setDamage(this.towerDamage);
         newArrow.GetComponent<Rigidbody2D>().velocity = moveDirArrow * 7f ;
         newArrow.transform.eulerAngles = new Vector3(0,0 ,angle); //Oxu müəyyən dərəcədə döndərir
         Destroy(newArrow,1f);
    }   

}

