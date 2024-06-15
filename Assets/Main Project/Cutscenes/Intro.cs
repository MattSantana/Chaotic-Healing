using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private string novoTexto;
    private int cena;
   

    [SerializeField]
    public TMP_Text _title;
    public GameObject Image1;
    public GameObject Image2;
    public GameObject Image3;


    // Start is called before the first frame update
    void Start()
    {
        cena = 0;
    }

    public void OnButtonClick()
    {
        if(cena == 0)
        {
            novoTexto = "Nobody saw him again after this incident.";
        }
        if(cena == 1)
        {
            novoTexto = "However, the legend says he would be back in 10.000 years.";
        }
         if(cena == 2)
        {
            Destroy (Image1);
            novoTexto = "As foretold, the fisherman appeared, willing to sell it for a 10000 hard earned pieces.";
        }
         if(cena == 3)
        {
            Destroy (Image2);
            novoTexto = "Willing to pursue this amazing artifact, The Healer Cat is now open for business";
        }
        if(cena < 3)
        {
            SceneManager.LoadScene(2);
        }
        _title.text = novoTexto;
        cena += 1;
    }
}
