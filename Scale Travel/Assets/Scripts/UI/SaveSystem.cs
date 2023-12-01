using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] string fileName;
    public SaveData saveData;
    private FileDataHandler dataHandler;
    public static SaveSystem Instance;

    bool toLevelSelect;

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

    public void MakeLevelAccesable(UIButton[] buttons)
    {
        bool b = true;
        for(int i = 0;i<12;i++)
        {
            if(i == 0 || i == 3 || i == 6)
            {
                buttons[i].MakeButtonSelectable(saveData.HasFinished[i]);
                if(!saveData.HasFinished[i]) b = false;
            }
            else if(i == 9)
            {
                if(b)buttons[i].MakeButtonSelectable(saveData.HasFinished[i]);
                else buttons[i].MakeButtonNonSelectable();
            }
            else if(saveData.HasFinished[i-1])
            {
                buttons[i].MakeButtonSelectable(saveData.HasFinished[i]);
                if(!saveData.HasFinished[i]) b = false;
            }
            
            else
            {
                buttons[i].MakeButtonNonSelectable();
                b = false;
            }
        }
        
    }

    public void FinishedLevel(int levelIndx)
    {
        levelIndx--;
        if(levelIndx < 12) saveData.HasFinished[levelIndx] = true;
        SaveGame();
    }

    public void LevelSelect(){toLevelSelect = true;}
    public void OnMainMenu()
    {
        if(toLevelSelect)
        {
            MenuScreen screen = FindObjectOfType<MenuScreen>();
            screen.gameObject.SetActive(false);
            MenuHolder.Instance.levelSelect.gameObject.SetActive(true);
            MenuHolder.Instance.levelSelect.ActivateScreen();
        }
    }

    public bool AreAllLevelFinished()
    {
        for(int i = 0;i<12;i++)
        {
            if(!saveData.HasFinished[i]) return false;
        }
        return true;
    }
}
