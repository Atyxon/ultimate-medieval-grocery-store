using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomersManager : MonoBehaviour
{
    public TimeManager timeMng;
    public GameObject customerPrefab;
    public Transform[] spawnPoints;
    public storeManager store;
    public int maxSpawnedCustomers;
    public int currentSpawnedCustomers;
    [Space]
    public float timer;
    public Vector2 minMaxSpawnCooldownTime;
    public float cooldown;
    private void Start()
    {
        cooldown = Random.Range(minMaxSpawnCooldownTime.x, minMaxSpawnCooldownTime.y);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            if(timeMng.time >= 450 && timeMng.time <= 1170)
            if (currentSpawnedCustomers < maxSpawnedCustomers)
            {
                GameObject newCustomer = Instantiate(customerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, spawnPoints[Random.Range(0, spawnPoints.Length)].rotation);
                currentSpawnedCustomers++;
            }
            timer = 0;
            cooldown = Random.Range(minMaxSpawnCooldownTime.x, minMaxSpawnCooldownTime.y);
        }
    }
    public void CustomerDespawn()
    {
        currentSpawnedCustomers--;
    }
}
