using UnityEngine;
using UnityEngine.UI;

public class InterfaceInteractions : MonoBehaviour
{
    [SerializeField] private GameObject r_MenuIcon;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private InterfacePresets[] interfacePresets;
    [SerializeField] private ActiveCardOptions activeCardOptions;
    [SerializeField] private int maxInterfacePagesIndex;

    [SerializeField] private Button leftScrollerButton;
    [SerializeField] private Button rightScrollerButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject timerText;

    public int currentIndex = 0;

    //card active checker section
    public delegate void OnCardActivated(string cardName );
    public static OnCardActivated onCardActivated;

    public bool[] cardsAtivated  = new bool[3];

    private void Awake() {
        leftScrollerButton.onClick.AddListener(LeftPageScroller);
        rightScrollerButton.onClick.AddListener(RightPageScroller);
        closeButton.onClick.AddListener(ClosePanel);

        // Initialize card activation states
        cardsAtivated[0] = false;
        cardsAtivated[1] = false;
        cardsAtivated[2] = false;
    }
    private void Update() {
        if(Input.GetKey(KeyCode.R))
        {
            OpenPanel();
        }
    }

    private void OpenPanel()
    {
        r_MenuIcon.GetComponent<Animator>().SetTrigger("openMenu");
        menuPanel.SetActive(true);
        r_MenuIcon.SetActive(false);
        timerText.SetActive(false);
        Time.timeScale = 0;
    }

    private void ClosePanel()
    {
        r_MenuIcon.SetActive(true);
        menuPanel.SetActive(false);
        timerText.SetActive(true);
        Time.timeScale = 1;
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

    private void StoringCardActivated( string cardName)
    {
        switch(cardName)
        {
            case "Stronger":
                CheckingCardsTemplateStade("Stronger");
                cardsAtivated[0] = true;
                break;
            case "Faster":
                CheckingCardsTemplateStade("Faster");
                cardsAtivated[1] = true;
                break;
            case "Utility":
                CheckingCardsTemplateStade("Utility");
               cardsAtivated[2] = true;
                break;
        }
    }   

    private void CheckingCardsTemplateStade( string spriteName ) {

            int activeCardCount = 0;
            foreach (bool cardActivated in cardsAtivated)
            {
                if (cardActivated)
                {
                    activeCardCount++;
                }
            }
            Debug.Log(activeCardCount);

        if (activeCardCount == 0)
        {
            activeCardOptions.cardTemplates[0].gameObject.SetActive(true);

            activeCardOptions.cardTemplates[0].GetComponent<Image>().sprite = SwitchSprite(spriteName);

        }
        else if (activeCardCount >= 1  )
        {
            activeCardOptions.cardTemplates[0].gameObject.SetActive(true);
            activeCardOptions.cardTemplates[1].gameObject.SetActive(true);

            activeCardOptions.cardTemplates[1].GetComponent<Image>().sprite = SwitchSprite(spriteName);

        }
        else if (activeCardCount >= 2 )
        {
            activeCardOptions.cardTemplates[0].gameObject.SetActive(true);
            activeCardOptions.cardTemplates[1].gameObject.SetActive(true); 
            activeCardOptions.cardTemplates[2].gameObject.SetActive(true);

            activeCardOptions.cardTemplates[2].GetComponent<Image>().sprite = SwitchSprite(spriteName);

        }
    }

    private Sprite SwitchSprite(string stateName)
    {
        if(stateName == null)
        {
            return null;
        }else{
            switch (stateName)
            {
                case "Stronger":
                    return activeCardOptions.strongerCard;
                case "Faster":
                    return activeCardOptions.fasterCard;
                case "Utility":
                    return activeCardOptions.utilityCard;
                default:
                    return null; 
            }
        }
    }
    private void OnEnable() {
        onCardActivated+=StoringCardActivated;
    }
    private void OnDisable() {
        onCardActivated-=StoringCardActivated;
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

[System.Serializable] 
public class ActiveCardOptions
{
    public Image[] cardTemplates;
    public Sprite strongerCard;
    public Sprite fasterCard;
    public Sprite utilityCard;
}
