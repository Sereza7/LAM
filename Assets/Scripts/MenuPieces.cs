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

    int i = 0; 

    // Awake is called before Start 
    void Awake() { 
        PiecePuzzleUI[] listedepiecesPPUI = new PiecePuzzleUI[NbDePieces]; 
        for (int index = 0; index < NbDePieces; index++) { 
            listedepiecesPPUI[index] = conteneurDesPieces.transform.GetChild(index).gameObject.GetComponent<PiecePuzzleUI>(); 
        }
        listedepieces = listedepiecesPPUI; 
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonLeft.onClick.AddListener(VersLaGauche); 
        buttonRight.onClick.AddListener(VersLaDroite);
    }

    // Update is called once per frame
    void Update()
    {
        i = Mathf.Clamp(i,0,NbDePieces-1); 
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
