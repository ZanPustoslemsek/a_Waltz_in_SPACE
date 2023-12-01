using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] UIButton[] buttons;
    [SerializeField] UIButton selectedOnDefault;

    [SerializeField] MenuScreen[] screens;
    [SerializeField] MenuScreen backScreen;
    [SerializeField] bool levelScreen, pauseScreen;

    private void Start() {
        if(!pauseScreen)Invoke("ActivateScreen",0.1f);
    }

    public void ActivateScreen()
    {
        if(levelScreen) SaveSystem.Instance.MakeLevelAccesable(buttons);
        Button b = selectedOnDefault.GetComponent<Button>();
        Slider s = selectedOnDefault.GetComponent<Slider>();
        if(b)b.Select();
        if(s)s.Select();

        selectedOnDefault.SellectButton();
        
        if(pauseScreen)Time.timeScale = 0f;

        FindObjectOfType<EventSystem>().SetSelectedGameObject(selectedOnDefault.gameObject);        
    }

    // bool CanSelect()
    // {
    //     EventSystem es = FindObjectOfType<EventSystem>();
    //     if(!es)return true;
    //     GameObject selected = es.currentSelectedGameObject;
    //     Debug.Log(selected);
    //     foreach(UIButton but in buttons)
    //     {
    //         if(but.gameObject == selected)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    public void DeselectAll(UIButton notDeselect = null)
    {
        foreach(UIButton button in buttons)
        {
            if(button != notDeselect)
            {
                button.DeSellectButton();
            }
            else
            {
                EventSystem es = FindObjectOfType<EventSystem>();
                if(!es)continue;
                if(es.currentSelectedGameObject != button.gameObject) es.SetSelectedGameObject(button.gameObject);
            }
        }
    }
    
    public void OpenScreen(int n = 0)
    {
        AudioManager.Instance.PlayRandom("click");
        StartCoroutine(OpenScreenCo(n));
    }

    IEnumerator OpenScreenCo(int n = 0)
    {
        SceeneTransitionUI.Instance.SetSize(false);
        DeselectAll();
        yield return new WaitForSeconds(1f);
        SceeneTransitionUI.Instance.SetSize(true);
        
        screens[n].gameObject.SetActive(true);
        screens[n].ActivateScreen();

        this.gameObject.SetActive(false);
    }

    public void OpenBackScreen()
    {
        
        if(!pauseScreen)
        {
            if(!backScreen)return;
            AudioManager.Instance.PlayRandom("click");
            StartCoroutine(OpenBackScreenCo());
        }
        else
        {
            Debug.Log("stopPause");
            Time.timeScale = 1f;
            this.transform.parent.gameObject.SetActive(false);

            InputController.controlls.actions.FindActionMap("Menu").Disable();
            InputController.controlls.actions.FindActionMap("Gameplay").Enable();
        }
    }

    IEnumerator OpenBackScreenCo()
    {
        SceeneTransitionUI.Instance.SetSize(false);
        DeselectAll();
        yield return new WaitForSeconds(1f);
        SceeneTransitionUI.Instance.SetSize(true);
        backScreen.gameObject.SetActive(true);
        backScreen.ActivateScreen();
        this.gameObject.SetActive(false);
    }

}
