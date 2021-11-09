using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public enum states
{
    IDLE,BUILD,UPGRADE,
}

public class GridManager : MonoBehaviour
{


    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private GameObject selector;

    [SerializeField]
    private GameObject TowerInformation;

    [SerializeField] 
    
    private Text arrowDamageText;
    [SerializeField] 
    
    private Text towerMoneyText;
    [SerializeField] 
    private Text levelText;

    [SerializeField] 
    
    private Text fireRateText;

    
    [SerializeField] 
    private GameObject playerMod;

    [SerializeField] 
    private Sprite[] modArray;

    private int [,] gridArray;

    private Dictionary<Vector2,GameObject> towers;

    private states state = states.IDLE; 

    private Vector2Int border;

    void Start()
    {
        towers = new Dictionary<Vector2, GameObject>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        border.x = bounds.size.x;
        border.y = bounds.size.y;
        gridArray = new int[bounds.size.x,bounds.size.y];       //Tilemap daki dolu  olan yerlerin pozisyalarını qeyd edir 
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];        
                if (tile != null) 
                {
                    gridArray[x,y] = 2;
                }
            }
        }        
    }



    private void keyUpdate()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            state = states.IDLE;
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            state = states.BUILD;
        }       
        else if(Input.GetKey(KeyCode.Alpha3))
        {
            state = states.UPGRADE;
        }
        
    }

    void Update()
    {
            keyUpdate();
           
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int worldPositionInt = new Vector2Int((int)worldPosition.x,(int)worldPosition.y);    
            selector.transform.position = new Vector3(worldPositionInt.x,worldPositionInt.y);

           TowerInformation.SetActive(false);

        if(!PlayerScript.gamePaused)
        {

            if(worldPositionInt.x < border.x && worldPositionInt.y < border.y && gridArray[worldPositionInt.x,worldPositionInt.y] == 1)
            {
                TowerInformation.SetActive(true);
                var item = towers[new Vector2(worldPositionInt.x,worldPositionInt.y)].GetComponent<TowerScript>();

                if(item.getTowerLevel() != 5)
                {
                    towerMoneyText.text = item.towerUpgradePrice.ToString();
                    arrowDamageText.text = item.towerDamage.ToString();
                    fireRateText.text = item.towerFireRate.ToString();
                    levelText.text = item.towerLevel.ToString();
                }
                else
                {
                    string maxString = "Max";
                    towerMoneyText.text = maxString;
                    arrowDamageText.text = maxString;
                    fireRateText.text = maxString;
                    levelText.text = maxString;

                }
            }




            if(worldPositionInt.x < border.x && worldPositionInt.y < border.y && worldPositionInt.x >=0 && worldPositionInt.y >=0)
            { 
                var selectorItem = selector.GetComponent<SpriteRenderer>();
        
                if(state == states.BUILD)
                {
                     playerMod.GetComponent<SpriteRenderer>().sprite = modArray[1];
                     selectorItem.enabled =  true;
                     selectorItem.color = new Color(255,255,255,0.4f);
                            

                    if(gridArray[worldPositionInt.x,worldPositionInt.y] == 0 && PlayerScript.getMoney() >= 15)
                     {
                         if(Input.GetMouseButtonDown(0))
                            {
                                towers[new Vector2(worldPositionInt.x,worldPositionInt.y)]  =
                                (GameObject)Instantiate(Resources.Load("Tower"),
                                                new Vector3(worldPositionInt.x,worldPositionInt.y,1),Quaternion.identity);
                                            
                                                gridArray[worldPositionInt.x,worldPositionInt.y] = 1; 
                                                PlayerScript.decreaseMoney(15);
                            }
                     }
                      else  
                      {
                        selectorItem.color = Color.red;
                      }
                }
                else if(state == states.UPGRADE)
                {
                    playerMod.GetComponent<SpriteRenderer>().sprite = modArray[2];
                    selectorItem.enabled = false; 

                        if(gridArray[worldPositionInt.x,worldPositionInt.y] == 1)
                        {
                            int money = towers[new Vector2(worldPositionInt.x,worldPositionInt.y)].GetComponent<TowerScript>().getTowerUpgrade();

                            if(Input.GetMouseButtonDown(0) && PlayerScript.getMoney() >= money && towers[new Vector2(worldPositionInt.x,worldPositionInt.y)].GetComponent<TowerScript>().getTowerLevel() < 5)
                             {
                                towers[new Vector2(worldPositionInt.x,worldPositionInt.y)].GetComponent<TowerScript>().upgrade();
                                PlayerScript.decreaseMoney(money);
                             }
                        }
                }
                else
                {
                    playerMod.GetComponent<SpriteRenderer>().sprite = modArray[0];
                    selectorItem.enabled = false; 
                }
            }
        }
            
    }
}
    


    

