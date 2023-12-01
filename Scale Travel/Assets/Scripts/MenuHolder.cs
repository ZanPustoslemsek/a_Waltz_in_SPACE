using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHolder : MonoBehaviour
{
    public static MenuHolder Instance;
    public MenuScreen levelSelect;
    [SerializeField] GameObject fireworks;
    void Start()
    {
        Instance = this;
        SaveSystem.Instance.OnMainMenu();

        fireworks.SetActive(SaveSystem.Instance.AreAllLevelFinished());
    }
}
