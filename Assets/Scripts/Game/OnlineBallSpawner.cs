using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlineBallSpawner : NetworkBehaviour {
    [SerializeField] private GameObject ballPrefab;
    
    public void SpawnBalls(int ballCount) {
        print("spawning balls");
        for (int i = 0; i < ballCount; i++) {
            float randomX = Random.Range(-2f, 2f);
            Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
            var ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity).GetComponent<OnlineBall>();
            ball.NetworkObject.Spawn();
        }
    }
}
