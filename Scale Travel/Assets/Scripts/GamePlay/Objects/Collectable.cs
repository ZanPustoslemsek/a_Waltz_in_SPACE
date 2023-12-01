using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Transform gfx;
    bool isCollected = false;
    float timeToLerp = 0.5f; //lerp for two seconds.
    float timeLerped = 0.0f;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Collect();
            isCollected = true;
            AudioManager.Instance.Play("collect");
        }
    }

    private void FixedUpdate() 
    {
        if(isCollected)
        {
            timeLerped += Time.deltaTime;
            this.transform.position = Vector3.Lerp(transform.position, Base.Instance.transform.position, timeLerped / timeToLerp);
            gfx.localScale = Vector3.Lerp(transform.localScale, Vector3.zero,timeLerped / timeToLerp);
            
            if(timeLerped >= timeToLerp)
            {
                Destroy(gameObject,0.5f);
            }
        }
    }
}
