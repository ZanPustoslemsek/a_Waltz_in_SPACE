using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static PlayerInput controlls;
    DefaultControl defaultControl;
    private float mouseScrollY;
    Planet currentPlanet = null;
    [SerializeField] Material outline;
    [SerializeField] Planet defaultPlanet;
    [SerializeField] GameObject pauseMenu; 

    private void Awake() 
    {
        defaultControl = new DefaultControl();

        // defaultControl.Gameplay.MouseScrollY.performed += x => mouseScrollY = x.ReadValue<float>();
        // defaultControl.Gameplay.Boost.performed += x => Boost();
        // defaultControl.Gameplay.KeyboardScroll.performed += x => GetKeyScroll(x.ReadValue<float>());
        // defaultControl.Gameplay.MoveKeys.performed += x => MoveCurrentPlanet(x.ReadValue<Vector2>());
        // defaultControl.Gameplay.Esc.performed += x => EscapeGameplay();

        // defaultControl.Menu.Esc.performed += x => EscapeMenu();
    }

    private void Start() {
        if(defaultPlanet)currentPlanet = defaultPlanet;
        if(currentPlanet)currentPlanet.AddMaterial(outline);

        controlls = this.GetComponent<PlayerInput>();
        if(GameManager.Instance.isMainMenu)
        {
            controlls.actions.FindActionMap("Gameplay").Disable();
            controlls.actions.FindActionMap("Menu").Enable();
        }
        else
        {
            controlls.actions.FindActionMap("Gameplay").Enable();
            controlls.actions.FindActionMap("Menu").Disable();
        }

    }
    private void Update() 
    {
        GetMouseClick();
        GetMouseScroll();
    }
    public void MouseScrollY(InputAction.CallbackContext context)
    {
        if(context.performed)mouseScrollY = context.ReadValue<float>();
    }
    public void GetMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    Planet pl = hit.collider.gameObject.GetComponent<Planet>();
                    if(pl)
                    {
                        if(currentPlanet)currentPlanet.RemoveMaterial();
                        currentPlanet = pl;
                        currentPlanet.AddMaterial(outline);

                    }
                }
            }
        }
    }
    public void GetMouseScroll()
    {
        if(!currentPlanet){return;}
        if(mouseScrollY > 0)
        {
            currentPlanet.ChangeSizeMouse(1f);
        }
        else if(mouseScrollY < 0)
        {
            currentPlanet.ChangeSizeMouse(-1f);
        }
    }
    public void GetKeyScroll(InputAction.CallbackContext context)
    {
        if(!context.performed)return;
        if(!currentPlanet){return;}
        float keyScroll = context.ReadValue<float>();
        if(keyScroll > 0)
        {
            currentPlanet.ChangeSizeKey(1f);
        }
        else if(keyScroll < 0)
        {
            currentPlanet.ChangeSizeKey(-1f);
        }
    }
    public void Boost(InputAction.CallbackContext context)
    {
        if(context.performed)PlayerMovement.Instance.StartBoost();
    }
    // public void MoveCurrentPlanet(InputAction.CallbackContext context)
    // {
    //     Vector2 input = context.ReadValue<Vector2>();
    //     Debug.Log(input);
    //     if(!currentPlanet)return;
    //     currentPlanet.RemoveMaterial();
    //     currentPlanet = currentPlanet.GetNextPlanet(input);
    //     currentPlanet.AddMaterial(outline);
    // }
     public void MoveCurrentPlanet(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!currentPlanet)return;
        currentPlanet.RemoveMaterial();
        currentPlanet = currentPlanet.GetNextPlanet(context.ReadValue<float>());
        currentPlanet.AddMaterial(outline);
        }
    }
    public void EscapeGameplay(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            pauseMenu.SetActive(true);
            if(pauseMenu)pauseMenu.GetComponentInChildren<MenuScreen>().ActivateScreen();
            controlls.actions.FindActionMap("Gameplay").Disable();
            controlls.actions.FindActionMap("Menu").Enable();
        }
        
    }
    public void EscapeMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            FindObjectOfType<MenuScreen>().OpenBackScreen();
            if(!GameManager.Instance.isMainMenu)
            {
                controlls.actions.FindActionMap("Menu").Disable();
                controlls.actions.FindActionMap("Gameplay").Enable();
            }
        }
    }
    #region  - Enable / Diable -

    void OnEnable()
    {
        defaultControl.Enable();
    }

    void onDisable()
    {
        defaultControl.Disable();
    }

    #endregion
}
