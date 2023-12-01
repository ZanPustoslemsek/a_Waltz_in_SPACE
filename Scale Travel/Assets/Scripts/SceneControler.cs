using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{   
    public static SceneControler Instance;
    private void Awake() 
    {
        if(Instance) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public void ReloadSceene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }

    public void LoadSceene(int ind)
    {
        SceneManager.LoadScene(ind);
    }
}
