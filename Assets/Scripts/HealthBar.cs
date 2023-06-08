using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public Slider slider;
    
    public Transform startPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    private void Update()
    {
        
        if(currentHealth == 0)
        {
            transform.position = startPosition.position+new Vector3(0,10,0);
            currentHealth = 100;
            slider.value = currentHealth;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ( collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("S_Enemy"))
        {
            TakeDamage(5);
            Destroy(collision.gameObject);
        }
    }

 
    void TakeDamage(int health)
    {
        currentHealth -= health;
        slider.value = currentHealth;
    }
}
