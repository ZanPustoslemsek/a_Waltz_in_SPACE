using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPref, collectPref;
    int collected = 0;
    Vector3[] collectablesObj;
    Transform[] collectParent;
    int maxCollected = 0;
    [SerializeField] GameObject meteorPref;
    private int numMeteors;
    [SerializeField] int maxNumMeteors = 3;
    [SerializeField] GameObject explosion;
    public static GameManager Instance;

    public bool isMainMenu = false;
    [SerializeField] int levelIndx; 

    public Vector2 boundry = new Vector2(26.5f, 15.5f);

    private void Awake() {
        if(Instance)
        {
            Instance.ReneuSceene();
            Destroy(this);
        }
        else Instance = this;
    }
    private void Start() 
    {
        if(!isMainMenu)
        {
            AudioManager.Instance.StopDynamicly("menuTheme", 2f, false);
            AudioManager.Instance.PlayNotForced("mainTheme");
            ReneuSceene();    
        }
        else
        {
            AudioManager.Instance.Stop("mainTheme");
            AudioManager.Instance.PlayNotForced("menuTheme");
            AudioManager.Instance.Stop("baseFire");
            AudioManager.Instance.Stop("boostFire");
            AudioManager.Instance.Stop("allballs");
        }
    }

    public void ReneuSceene()
    {
        Collectable[] collectables = FindObjectsOfType<Collectable>();
        maxCollected = collectables.Length;

        collectablesObj = new Vector3[maxCollected];
        collectParent = new  Transform[maxCollected];
        for(int i = 0;i<maxCollected;i++)
        {
            collectablesObj[i] = collectables[i].transform.position;
            collectParent[i] = collectables[i].transform.parent;
        }

        MakePlayer();

        Base.Instance.SetBase(maxCollected);
        
    }

    public void Collect()
    {
        collected++;
        Base.Instance.ReadyToFinish(true);
    }

    public void Finish()
    {
        if(collected >= maxCollected)
        {
            SceeneTransitionUI.Instance.SetSize(false);
            Debug.Log("Finished the Level!!");
            PlayerMovement.Instance.Finish();

            SaveSystem.Instance.FinishedLevel(levelIndx);

            AudioManager.Instance.Play("base");
            AudioManager.Instance.StopDynamicly("base",2f);
            
            StartCoroutine(Transition());
        }
    }
    IEnumerator Transition()
    {
        Debug.Log("inTransition");
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        if(levelIndx == 12) SceneControler.Instance.LoadSceene(0);
        else SceneControler.Instance.NextLevel();
    }

    public void DestroyPlayer()
    {
        StartCoroutine(DestroyPlayerCo());
    }
    bool canMakePlayer = true;
    IEnumerator DestroyPlayerCo()
    {
        if(canMakePlayer)
        {
            AudioManager.Instance.Play("explosion");
            AudioManager.Instance.Stop("baseFire");
            AudioManager.Instance.Stop("boostFire");

            Camera.main.GetComponent<Animator>().SetTrigger("shake");
            
            Instantiate(explosion,PlayerMovement.Instance.transform.position, Quaternion.identity);
            
            Destroy(PlayerMovement.Instance.gameObject);
            canMakePlayer = false;
            yield return new WaitForSeconds(2f);
            MakePlayer();
            canMakePlayer = true;
        }
        

    }

    public void AddMeteor(Vector3 pos)
    {
        if(numMeteors < maxNumMeteors)
        {
            Instantiate(meteorPref, pos, Quaternion.identity);
            numMeteors++;
        }
    }
    public void DestroyMeteor(GameObject met)
    {
        Destroy(met, 0.01f);
        numMeteors--;
    }

    void MakePlayer()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if(player)Destroy(player.gameObject);
        Instantiate(playerPref,new Vector3(0,0,-2.5f), Quaternion.identity);
        Collectable[] collectables = FindObjectsOfType<Collectable>();
        for(int i = 0;i<collectables.Length;i++)
        {
            Destroy(collectables[i].gameObject);
        }
        for(int i = 0;i<maxCollected;i++)
        {
            Instantiate(collectPref,collectablesObj[i],Quaternion.identity, collectParent[i]);
        }
        Planet[] planets = FindObjectsOfType<Planet>();
        foreach(Planet pl in planets){pl.ResetPlanet();}

        Rotate[] rotates = FindObjectsOfType<Rotate>();
        foreach(Rotate r in rotates){r.ResetRotation();}

        DestroyableObj[] dest = FindObjectsOfType<DestroyableObj>();
        foreach(DestroyableObj de in dest){de.ResetObj();}
        
        collected = 0;

        if(Base.Instance)Base.Instance.ReadyToFinish(false);
    }

    int lv;
    public void StartLevel(int level)
    {   
        lv = level;
        StartCoroutine(SetTransition());
        SceeneTransitionUI.Instance.SetSize(false);  
        AudioManager.Instance.StopDynamicly("menuTheme", 2f);    

        
    }
    IEnumerator SetTransition()
    {
        Debug.Log("inTransition");
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        SceneControler.Instance.LoadSceene(lv);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
