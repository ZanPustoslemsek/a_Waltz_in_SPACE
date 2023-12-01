using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] UIPlanet planet;
    [SerializeField] Material outline;
    [SerializeField] MenuScreen screen;

    bool isSelected = false;

    public bool canBeSelected = true;

    [SerializeField] Material deselectedMaterial, selectedMaterial, deselectedOutline, selectedOutline;
    [SerializeField] GameObject StarImage;

    public void SellectButton()
    {
        if(!isSelected)
        {
            AudioManager.Instance.PlayRandom("select");
            if(planet)
            {
                if(canBeSelected)planet.MakeBig();
                planet.AddMaterial(outline);
            }
            isSelected = true;

            screen.DeselectAll(this);
        }
    }

    public void DeSellectButton()
    {
        if(isSelected)
        {
            if(planet)
            {
                if(canBeSelected)planet.MakeSmall();
                planet.RemoveMaterial();
            }
            isSelected = false;
        }
    }

    public void MakeButtonSelectable(bool HasFinished)
    {
        planet.SetFirstMaterial(selectedMaterial);
        outline = selectedOutline;
        canBeSelected = true;
        if(StarImage) StarImage.SetActive(HasFinished);
    }

    public void MakeButtonNonSelectable()
    {
        planet.SetFirstMaterial(deselectedMaterial);
        outline = deselectedOutline;
        canBeSelected = false;
        if(StarImage) StarImage.SetActive(false);
    }

    public void StartLevel(int level = 0)
    {
        if(canBeSelected)
        {
            AudioManager.Instance.PlayRandom("click");
            SceeneTransitionUI.Instance.ZoomOnPlanet(planet);
            GameManager.Instance.StartLevel(level);
        }
        else
        {
            AudioManager.Instance.PlayRandom("bad_click");
        }
    }

    public void LevelSelect(){SaveSystem.Instance.LevelSelect(); StartLevel(0);}

}
