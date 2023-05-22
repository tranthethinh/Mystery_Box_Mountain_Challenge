using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider; 
    public int amountToBeKill; 
    private int currentAmount = 0;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = amountToBeKill;
        slider.value = 0;
        slider.fillRect.gameObject.SetActive(false);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.LookAt(Camera.main.transform);
    }
    public void AttackAnimal(int amount)
    {
        currentAmount += amount;
        slider.fillRect.gameObject.SetActive(true);
        slider.value = currentAmount;
        if (currentAmount >= amountToBeKill)
        {
            gameManager.UpdateScore(5);
            Destroy(gameObject, 0.1f);
        }
    }

}
