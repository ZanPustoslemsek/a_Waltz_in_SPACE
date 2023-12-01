using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Transform obj;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] bool controlledByPlanet;
    [SerializeField] Planet planet;
    Vector3 pos;
    float distance;
    Quaternion startRotation;    
    float delta;
    [SerializeField] int numIndic = 16;
    [SerializeField] GameObject indicatorPref;
    GameObject[] indicators;
    [SerializeField] Planet controlledPlanet;

    bool canRotate = false;
    private void Start() {

        canRotate = false;
        startRotation = transform.rotation;
        pos = obj.localPosition.normalized;
        distance = Vector3.Distance(transform.position, obj.position);

        if(controlledByPlanet)
        {
            distance = Dist();
            obj.localPosition = pos * distance;
            rotateSpeed = GetSpeed();
        }

        if(indicatorPref)
        {
            indicators = new GameObject[numIndic];
            delta = 360f / (float)numIndic;
            Vector3 spawnPos = pos * distance;
            for(int i = 0;i<numIndic;i++)
            {
                spawnPos = Quaternion.Euler(0,delta,0) * spawnPos;
                indicators[i] = Instantiate(indicatorPref, transform.position + spawnPos, Quaternion.identity, this.transform.parent);
            } 
        }
    }
    void FixedUpdate()
    {
        if(controlledByPlanet)rotateSpeed = Mathf.Lerp(rotateSpeed, GetSpeed(), Time.deltaTime * Konst.scaleUpSpeed);
        if(controlledByPlanet)distance = Dist();
        obj.localPosition = Vector3.Lerp(obj.localPosition, pos*distance,Time.deltaTime * Konst.scaleUpSpeed);
        if(indicatorPref)
        {
            Vector3 spawnPos = pos * distance;
            for(int i = 0;i<numIndic;i++)
            {
                spawnPos = Quaternion.Euler(0,delta,0) * spawnPos;
                indicators[i].transform.position = Vector3.Lerp(indicators[i].transform.position,  transform.position + spawnPos,Time.deltaTime * Konst.scaleUpSpeed);
            }
        }
        if(canRotate)transform.Rotate(new Vector3(0,rotateSpeed, 0) * Time.deltaTime);

    }

    float Dist()
    {
        return (Konst.max_dist - Konst.min_dist)* (1-(planet.radius - Konst.minRadius) / (Konst.maxRadius - Konst.minRadius)) + Konst.min_dist;
    } 

    float GetSpeed()
    {
        return (Konst.max_rotateSpeed - Konst.min_rotateSpeed)* (planet.radius - Konst.minRadius) / (Konst.maxRadius - Konst.minRadius) + Konst.min_rotateSpeed;
    }

    public void ResetRotation()
    {
        StartCoroutine(ResetRotationCo());
    }

    IEnumerator ResetRotationCo()
    {
        float timeToLerp = 1;
        float timeLerped = 0.0f;
        Vector3 scale;
        if(controlledPlanet)
        {
            float radius = controlledPlanet.radius;
            controlledPlanet.ResetPlanet();
            scale = controlledPlanet.radius * 2 * Vector3.one;
            controlledPlanet.radius = radius;
            controlledPlanet.OutsideControll(true);
        }
        else scale = obj.localScale;

        while(true)
        {
            timeLerped += Time.deltaTime;
            obj.localScale = Vector3.Lerp(obj.localScale, Vector3.zero,timeLerped / timeToLerp);
            if(obj.localScale.magnitude <= 0.001f){obj.localScale = Vector3.zero;break;}
            yield return new WaitForSeconds(0);
        }

        timeLerped = 0;
        transform.rotation = startRotation;
        canRotate = true;
        Debug.Log("startRotation");


        while(true)
        {
            timeLerped += Time.deltaTime;
            obj.localScale = Vector3.Lerp(obj.localScale, scale,timeLerped / timeToLerp);
            if(obj.localScale.magnitude >= scale.magnitude - 0.001f){obj.localScale = scale;break;}
            yield return new WaitForSeconds(0);
        }       

        if(controlledPlanet) controlledPlanet.OutsideControll(false); 
    }
}
