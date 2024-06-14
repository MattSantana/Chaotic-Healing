using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceInteractions : MonoBehaviour
{
    [SerializeField] private GameObject r_MenuIcon;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private InterfacePresets[] interfacePresets;
    [SerializeField] private int maxInterfacePagesIndex;

    [SerializeField] private Button leftScrollerButton;
    [SerializeField] private Button rightScrollerButton;

    public int currentIndex = 0;

    private void Awake() {
        leftScrollerButton.onClick.AddListener(LeftPageScroller);
        rightScrollerButton.onClick.AddListener(RightPageScroller);
    }
    private void Update() {
        if(Input.GetKey(KeyCode.R))
        {
            r_MenuIcon.SetActive(false);
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void LeftPageScroller()
    {
        if(currentIndex > 0)
        {
            currentIndex--;
        }
        else{ return; }
        
        switch(currentIndex)
        {
            case 0:
                MenuPageZero();
                break;
            case 1: 
                MenuPageOne();
                break;
        }
    
    }

    public void RightPageScroller()
    {
        if(currentIndex < maxInterfacePagesIndex )
        {
            currentIndex++;
        }
        else{ return; }
        
        switch(currentIndex)
        {
            case 1:
                MenuPageOne();
                break;
            case 2: 
                MenuPageTwo();
                break;
        }
    }

    private void MenuPageZero()
    {
        //menu de poções

        #region // conteúdo da página de poções

            //indicadores da página
            interfacePresets[0].indicatorAtivated.enabled = true;
            interfacePresets[0].indicatorDesativated.enabled = false;

            //conteúdo da página
            interfacePresets[0].tableOfContentLeft.SetActive(true);
            interfacePresets[0].tableOfContentRight.SetActive(true);

            //extra conteúdo

            //section holder
            interfacePresets[0].sectionHolderPanel.SetActive(true);

        #endregion

        #region // conteúdo da página de cartas

            //indicadores da página
            interfacePresets[1].indicatorAtivated.enabled = false;
            interfacePresets[1].indicatorDesativated.enabled = true;

            //conteúdo da página
            interfacePresets[1].tableOfContentLeft.SetActive(false);
            interfacePresets[1].tableOfContentRight.SetActive(false);

            //extra conteúdo
            interfacePresets[1].extraTableOfContentLeft.SetActive(false);

            //section holder
            interfacePresets[1].sectionHolderPanel.SetActive(false);

        #endregion

    }
    private void MenuPageOne()
    {
        //menu de cartas

        #region // conteúdo da página de cartas

            //indicadores da página
            interfacePresets[1].indicatorAtivated.enabled = true;
            interfacePresets[1].indicatorDesativated.enabled = false;

            //conteúdo da página
            interfacePresets[1].tableOfContentLeft.SetActive(true);
            interfacePresets[1].tableOfContentRight.SetActive(true);

            //extra conteúdo
            interfacePresets[1].extraTableOfContentLeft.SetActive(false);

            //section holder
            interfacePresets[1].sectionHolderPanel.SetActive(true);

        #endregion

        #region // conteúdo da página de poções

            //indicadores da página
            interfacePresets[0].indicatorAtivated.enabled = false;
            interfacePresets[0].indicatorDesativated.enabled = true;

            //conteúdo da página
            interfacePresets[0].tableOfContentLeft.SetActive(false);
            interfacePresets[0].tableOfContentRight.SetActive(false);

            //extra conteúdo

            //section holder
            interfacePresets[0].sectionHolderPanel.SetActive(false);

        #endregion

    }

    private void MenuPageTwo()
    {
        // menu de cartas 

        #region // conteúdo da página de cartas

            //indicadores da página
            interfacePresets[1].indicatorAtivated.enabled = true;
            interfacePresets[1].indicatorDesativated.enabled = false;

            //conteúdo da página
            interfacePresets[1].tableOfContentLeft.SetActive(false);
            interfacePresets[1].tableOfContentRight.SetActive(false);

            //extra conteúdo
            interfacePresets[1].extraTableOfContentLeft.SetActive(true);

            //section holder
            interfacePresets[1].sectionHolderPanel.SetActive(true);

        #endregion

        #region // conteúdo da página de poções

            //indicadores da página
            interfacePresets[0].indicatorAtivated.enabled = false;
            interfacePresets[0].indicatorDesativated.enabled = true;

            //conteúdo da página
            interfacePresets[0].tableOfContentLeft.SetActive(false);
            interfacePresets[0].tableOfContentRight.SetActive(false);

            //extra conteúdo

            //section holder
            interfacePresets[0].sectionHolderPanel.SetActive(false);

        #endregion

    }
}


[System.Serializable] 
public class InterfacePresets
{
    public string nameIdentification;
    public Image indicatorDesativated;
    public Image indicatorAtivated;
    public GameObject tableOfContentLeft;
    public GameObject extraTableOfContentLeft;
    public GameObject tableOfContentRight;
    public GameObject sectionHolderPanel;
}
