using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool[] HasFinished;

    public SaveData()
    {
        HasFinished = new bool[12];
    }
}
