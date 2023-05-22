using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject rockPrefab;
    public Transform player;
    public float spawnBoundary = 44f;
    private float spawnInterval = 1f;
    private float spawnInterval2 = 22f;
    public int maxEnemyCount = 22;

    private int currentEnemyCount;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private void Start()
    {
        scoreText.gameObject.SetActive(true);
        UpdateScore(0);
        currentEnemyCount = 0;
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
        InvokeRepeating("SpawnS_Enemy", spawnInterval2, spawnInterval2);
    }
    private void SpawnS_Enemy()
    {
        // Generate random position within the specified boundary
        Vector3 spawnrockPosition = Random.insideUnitSphere * 10f;
        spawnrockPosition.y = 20f; // Ensure enemy is at the same level as the player

        // Offset the spawn position relative to the player
        spawnrockPosition += player.position;
        Instantiate(rockPrefab, spawnrockPosition, Quaternion.identity);
    }
    private void SpawnEnemy()
    {
        
        if (currentEnemyCount >= maxEnemyCount)
        {
            return;
        }
        for (int i = 0; i < 11; i++)
        {
            // Generate random position within the specified boundary
            Vector3 spawnPosition = Random.insideUnitSphere * spawnBoundary;
            spawnPosition.y = 0f; // Ensure enemy is at the same level as the player

            // Offset the spawn position relative to the player
            spawnPosition += player.position;

            // Instantiate the enemy at the spawn position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            currentEnemyCount++;
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = " Score:" + score;
        currentEnemyCount--;
    }
}
