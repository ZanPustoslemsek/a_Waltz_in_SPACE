using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    bool canSpawn = true;
    private void Start() {
        StartCoroutine("SpawnMeteor");
    }
    IEnumerator SpawnMeteor()
    {
        float r = Random.Range(-1f,1f);
        if(r > 0)
        {
            if(canSpawn)GameManager.Instance.AddMeteor(transform.position);
        }
            
        yield return new WaitForSeconds(Random.Range(Konst.minTimeMeteoriteSpawn, Konst.maxTimeMeteoriteSpawn));
        StartCoroutine("SpawnMeteor");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            canSpawn = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
        {
            canSpawn = true;
        }
    }
}
