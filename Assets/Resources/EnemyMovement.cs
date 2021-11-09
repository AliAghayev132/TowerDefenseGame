using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField]
    private int followedCubeNumber = 1;
    [SerializeField]
    private int followedCodeNumberMax = 6;

    //Animasiyalar üçün

    [SerializeField]
    private Animator animator;
    private bool increaseMoney = true;
    private bool isAttacking = false;
    public bool isDead = false;

    private int Health;
    private float Damage;
    private int Money;

    [SerializeField]
    private float enemySpeed = 0.5f;

    GameObject followedCube;

    private  void Start()
    {
        followedCube = GameObject.Find(followedCubeNumber.ToString());
    }

     private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "FollowedCube")
        {
            followedCubeNumber++;
            if(followedCubeNumber <= followedCodeNumberMax)
            {
                followedCube = GameObject.Find(followedCubeNumber.ToString());
            }
            else
            {
                Attack(); //Saraya çatdıqda
            } 
           
        }
        if(other.gameObject.tag == "Arrow")
        {
            this.Health -= other.gameObject.GetComponent<ArrowScript>().getDamage();
            Destroy(other.gameObject);
        }
    }
    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking",true);

        increaseMoney = false;
        PlayerScript.decreaseHealth(Damage);

        Invoke("destroyEnemy",0.5f);
        Invoke("PlayerScript.decreaseEnemy",0.6f);     
    }

    private void setDead()
    {
        isDead = true;
        animator.SetBool("isDead",true);
    }

    //For playerscript
    public void setDamage(float Damage)
    {
        this.Damage = Damage;
    }
    
    public void setHealth(int Health)
    {
        this.Health = Health;
    }

    public void setMoney(int Money)
    {
        this.Money = Money;
    }

    private void destroyEnemy()
    {
        if(increaseMoney)
            PlayerScript.increaseMoney(Money);
        PlayerScript.decreaseEnemy();
        Destroy(gameObject);
    }
        void Update() 
        {
            if(Health <= 0 && !isDead)
            {
                setDead();
                PlayerScript.killedEnemies++;
                Invoke("destroyEnemy",3f); 
            }
            else if(!isDead && !isAttacking)
            {
                Vector3 moveDir = (followedCube.transform.position - transform.position).normalized;
                transform.position += moveDir * enemySpeed * Time.deltaTime;
            }

        }
}   
