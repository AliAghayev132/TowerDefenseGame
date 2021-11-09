using UnityEngine;

public class FightControl : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            GetComponentInParent<TowerScript>().inRange(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            GetComponentInParent<TowerScript>().outRange(other);
        }
    }
}
