using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceeneTransitionUI : MonoBehaviour
{
    public static SceeneTransitionUI Instance;
    [SerializeField] RectTransform mask, panel;
    float size = 1;
    float baseZoom, zoomedZoom, zoom;

    float timeToLerp = 2; //lerp for two seconds.
    float timeLerped = 0.0f;
    Vector3 planetPos;
    bool zoomOnPlanet = false;
    private void Awake() 
    {
        
    }
    private void Start() 
    {
        Instance = this;
        mask.localScale = Vector3.zero;
        size = 1f;
        baseZoom = Camera.main.orthographicSize;
        zoomedZoom = baseZoom/2f;
        Camera.main.orthographicSize = zoomedZoom;
        zoom = baseZoom;

        Camera.main.GetComponent<Animator>().enabled = false;
        Camera.main.transform.position = new Vector3(0,100,-2.5f);

        mask.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        Invoke("StartC",0.1f);
    }

    void StartC()
    {StartCoroutine(UpdateCo());}

    bool isSize1 = false;
    IEnumerator UpdateCo()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0f);
            timeLerped += Time.unscaledDeltaTime;
            mask.localScale = Vector3.Lerp(mask.localScale, Vector3.one * size,timeLerped / timeToLerp);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom,timeLerped / timeToLerp);
            if(zoomOnPlanet) Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, planetPos, timeLerped / (timeToLerp - 1f));
            else Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0,100,-2.5f*(1-size)), timeLerped / timeToLerp);
            if(size == 1)
            {
                if(mask.localScale.magnitude >= 0.99f)
                {
                    if(!isSize1)
                    {
                        MenuScreen screen = FindObjectOfType<MenuScreen>();
                        if(screen) screen.ActivateScreen();

                        mask.gameObject.SetActive(false);
                        panel.gameObject.SetActive(false);                    

                        Camera.main.GetComponent<Animator>().enabled = true;
                        isSize1 = true;
                    }
                }
                else{isSize1 = false;}
            }
            else{isSize1 = false;}
        }
        
    }

    // private void Update() 
    // {
    //     useCoroutine = false;
    //     timeLerped += Time.unscaledDeltaTime;
    //     mask.localScale = Vector3.Lerp(mask.localScale, Vector3.one * size,timeLerped / timeToLerp);
    //     Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom,timeLerped / timeToLerp);
    //     Base baseObj = FindObjectOfType<Base>();
    //     if(zoomOnPlanet) Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, planetPos, timeLerped / (timeToLerp - 1f));
    //     else Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0,100,-2.5f*(1-size)), timeLerped / timeToLerp);
    //     if(size == 1)
    //     {
    //         if(mask.localScale.magnitude >= 0.99f)
    //         {
    //             if(!isSize1)
    //             {
    //                 mask.gameObject.SetActive(false);
    //                 panel.gameObject.SetActive(false);

    //                 MenuScreen screen = FindObjectOfType<MenuScreen>();
    //                 if(screen) screen.ActivateScreen();

    //                 Camera.main.GetComponent<Animator>().enabled = true;
    //                 isSize1 = true;
    //             }
    //         }
    //         else{isSize1 = false;}
    //     }
    //     else{isSize1 = false;}
    // }
    public void SetSize(bool b = true)
    {
        if(b)
        {
            size = 1;zoom = baseZoom;
        }
        else 
        {
            size = 0;zoom = zoomedZoom;
        }
        mask.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        timeLerped = 0f;
        Camera.main.GetComponent<Animator>().enabled = false;
    }

    public void ZoomOnPlanet(UIPlanet planet)
    {
        planetPos = new Vector3(planet.transform.position.x,Camera.main.transform.position.y, planet.transform.position.z);
        zoomOnPlanet = true;
    }
}
