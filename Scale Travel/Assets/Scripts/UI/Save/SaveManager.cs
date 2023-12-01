using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] string fileName;

    public SaveData saveData;

    private FileDataHandler dataHandler;

    public static SaveManager Instance;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame();
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }
    public void NewGame()
    {
        saveData = new SaveData();
    }

    public void LoadGame()
    {
        saveData = dataHandler.Load();

        if(saveData == null)
        {
            NewGame();
        }
    }

    public void SaveGame()
    {
        dataHandler.Save(saveData);
    }
}
