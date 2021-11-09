using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = System.Random;
public class PlayerScript : MonoBehaviour
{
    internal enum EnemyStat{
        DAMAGE,HEALTH,MONEY
    }
    //Variables


    //Text
    [SerializeField]
    private Text moneyText;

    [SerializeField]

    private Text healthText;

    [SerializeField]
    
    private Text levelText;

    [SerializeField]
    private Text enemyMoneyText;

    [SerializeField]

    private Text enemyHealthText;

    [SerializeField]

    private Text killedEnemiesText;

    [SerializeField]

    private Text Level;

    [SerializeField]
     private Text enemyDamageText;
    
    public static bool gamePaused = false;


    [SerializeField]
    private GameObject gameOverObject;


    //Player Variables

    [SerializeField]
    private static int money = 45;

    [SerializeField]
    private static float castleHealth = 10f;


    [SerializeField]
    private Transform enemySpawnPosition;
    private static int enemyCountStatic = 10;

    private int maxEnemy = enemyCountStatic;

    public static int killedEnemies;
    private int level = 1;

    [SerializeField]
    private float enemyDamage = 1f;

    [SerializeField]
    private int enemyHealth = 60;

    [SerializeField]
    private int enemyMoney = 2;
     EnemyMovement enemy =  new EnemyMovement();
  
    void Start()
    {
        InvokeRepeating("spawnEnemy",0f,3f);
    }

    void spawnEnemy()
    {

        if(this.maxEnemy > 0)
        {
            GameObject enemy = (GameObject)Instantiate(Resources.Load("Enemy"),new Vector3(enemySpawnPosition.position.x ,enemySpawnPosition.position.y,0f),Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().setDamage(enemyDamage);
            enemy.GetComponent<EnemyMovement>().setHealth(enemyHealth);
            enemy.GetComponent<EnemyMovement>().setMoney(enemyMoney);
            maxEnemy--;
        }
        
        if(enemyCountStatic == 0 && maxEnemy == 0)
        { 
            level++;
            upgradeEnemy();      
        }
    }

    private void changeTexts()
    {
        moneyText.text =  money.ToString();
        healthText.text = castleHealth.ToString();
        levelText.text =  "Level "  + level.ToString();

        enemyMoneyText.text =  enemyMoney.ToString();
        enemyHealthText.text = enemyHealth.ToString();
        enemyDamageText.text = enemyDamage.ToString();
    }

    public static  int getMoney()
    {
        return money;
    }

    public static void decreaseHealth(float Damage)
    {
        castleHealth -= Damage;

        if(castleHealth <0)
            castleHealth = 0f;
    }
    public static void decreaseMoney(int x)
    {
        money -= x;
    }
    public static void decreaseEnemy()
    {
        enemyCountStatic--;
    
    }
    public static void increaseMoney(int x)
    {
        money += x;
    }
    void Update()
    {
        changeTexts();
        if(castleHealth <=0)
        {
            killedEnemiesText.text = "Killed enemies " + killedEnemies.ToString();
            Level.text = "Level " + level.ToString();
            gameOverObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }


    //Buttonlar
    public void quitButton()
    {
        Application.Quit();
    }

    public void restartButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("SampleScene");
        resetAllVariables();
        Time.timeScale = 1f;
    }

    private  void upgradeEnemy()
    {   
        enemyCountStatic = level * 10;     

        maxEnemy = enemyCountStatic;

        Random rand = new Random();

        int upgradeCount = rand.Next(1,4); // Upgrade sayı

        List<EnemyStat> upgrades = new List<EnemyStat>{EnemyStat.DAMAGE,EnemyStat.HEALTH,EnemyStat.MONEY};

        for(int i=0 ;i <=upgradeCount;i++) //Texmini Sececeyi upgrade 
      {
            EnemyStat temp =  upgrades[rand.Next(upgrades.Count)];

            switch(temp)
            {   
                case EnemyStat.DAMAGE:
                    enemyDamage += 0.3f;
                    break;
                case EnemyStat.MONEY:
                    enemyMoney += 2;
                    break;
                case EnemyStat.HEALTH:
                    enemyHealth += 20;
                    break;
            }
            if(upgrades.Count != 0)
            upgrades.Remove(temp);
      }
    }

    private void resetAllVariables()
    {
        money = 45;
        castleHealth = 10;
        enemyCountStatic = 10;
        maxEnemy = enemyCountStatic;
        level = 1;  
        enemyDamage = 1;
        enemyHealth = 60;
        enemyMoney = 5;
        Time.timeScale = 1f;
        killedEnemies = 0;
    }


}
