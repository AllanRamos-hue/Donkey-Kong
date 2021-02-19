using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarril : MonoBehaviour
{
    public Rigidbody2D barril;
    public float barrilSpeed;
    public float spawnTime;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnDelay);
    }

    public void Spawn()
    {
        Rigidbody2D barrilInstance;
        barrilInstance = Instantiate(barril, transform.position, transform.rotation) as Rigidbody2D;
        barrilInstance.AddForce(transform.right * barrilSpeed);

    }
}
