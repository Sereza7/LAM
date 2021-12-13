using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPieces : MonoBehaviour
{
    public RectTransform menuPanel; 
    public Button buttonLeft, buttonRight; 

    public int NbDePieces; 
    public GameObject conteneurDesPieces; 
    private PiecePuzzleUI[] listedepieces; 

    int i = 1; 

    // Awake is called before Start 
    void Awake() { 
        // On récupère toutes les pièces de puzzle dans la liste de pièces 
        PiecePuzzleUI[] listedepiecesPPUI = new PiecePuzzleUI[NbDePieces]; 
        for (int index = 0; index < NbDePieces; index++) { 
            listedepiecesPPUI[index] = conteneurDesPieces.transform.GetChild(index).gameObject.GetComponent<PiecePuzzleUI>(); 
        }
        listedepieces = listedepiecesPPUI; 
    }

    // Start is called before the first frame update
    void Start()
    {
        // On initialise les boutons qui permettront de mettre 
        buttonLeft.onClick.AddListener(VersLaGauche); 
        buttonRight.onClick.AddListener(VersLaDroite);

        // On ajoute la première pièce qui sera toujours dans la partie puzzle et on change son statut en pièce qui ne doit pas être sélectionnée. 
        listedepieces[0].AddPieceToGame(); 
        listedepieces[0].NotToBeSelected(); 
    }

    // Update is called once per frame
    void Update()
    {
        i = Mathf.Clamp(i,1,NbDePieces-1); 
        for (int j = 0; j < listedepieces.Length; j++) { 
            listedepieces[j].NotToBeSelected();
        }
        listedepieces[i].ToBeSelected();  
    }

    void VersLaGauche() 
    { 
        i -= 1; 
    }

    void VersLaDroite() 
    { 
        i += 1; 
    }
}
