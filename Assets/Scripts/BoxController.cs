using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxController : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject MBposition;
    public TextMeshProUGUI openText;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(MBposition.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        openText.gameObject.SetActive(false); // Deactivate the game object
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.SetParent(parentObject.transform, false);
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("S_Enemy"))
        {
            transform.SetParent(MBposition.transform, false);
        }
        if (other.CompareTag("OpenLocation"))
        {
            openText.gameObject.SetActive(true);
            transform.SetParent(MBposition.transform, false);
            StartCoroutine(DeactivateAfterDelay()); // Start the coroutine
        }
        
    }
}
