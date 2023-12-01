using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObj : MonoBehaviour
{
    [SerializeField] GameObject moon;
    [SerializeField] ParticleSystem particle;
    bool isDestroyed = false;

    private void Start() {
        moon.transform.eulerAngles = new Vector3(Random.Range(0f,360f), Random.Range(0f,360f), Random.Range(0f,360f));
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if(player)
            {
                if(player.isBoosting)
                {
                    StartCoroutine(DestroyThisObj());
                }
                else
                {
                    GameManager.Instance.DestroyPlayer();
                }
            }
        }
    }

    IEnumerator DestroyThisObj()
    {
        if(!isDestroyed)
        {
            //particle.gameObject.SetActive(true);
            moon.GetComponent<HitCollider>().destroy = false;
            moon.GetComponent<Collider>().isTrigger = true;
            moon.SetActive(false);
            particle.Play();
            isDestroyed = true;
            yield return  new WaitForSeconds(5f);
            ResetObj();
        }
    }

    public void ResetObj()
    {
        //particle.gameObject.SetActive(false);
        moon.SetActive(true);
        particle.Stop();
        isDestroyed = false;
        moon.GetComponent<Collider>().isTrigger = false;
        moon.GetComponent<HitCollider>().destroy = true;
    }
}
