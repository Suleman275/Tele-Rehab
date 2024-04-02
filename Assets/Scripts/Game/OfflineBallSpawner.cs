using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineBallSpawner : MonoBehaviour {
    [SerializeField] private GameObject ballPrefab;
    
    public void SpawnBalls(int ballCount) {
        for (int i = 0; i < ballCount; i++) {
            float randomX = Random.Range(-2f, 2f);
            Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
            var ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            ball.transform.SetParent(transform);
        }
    }
}