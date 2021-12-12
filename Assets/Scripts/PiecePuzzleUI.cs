using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(Button))]
public class PiecePuzzleUI : MonoBehaviour
{
    private RawImage displayimg; 
    private Button displayButton; 
    public GameObject solidPiece; 
    public GameObject clonePiece; 
    public Camera cameraUI; 

    void Awake() { 
        displayimg = GetComponent<RawImage>(); 
        displayButton = GetComponent<Button>(); 
        displayimg.enabled = false; 
        solidPiece.SetActive(false); 
        clonePiece.SetActive(false); 
    }

    // Start is called before the first frame update
    void Start()
    {
        displayButton.onClick.AddListener(AddPieceToGame); 
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void ToBeSelected() { 
        displayimg.enabled = true; 
        if (!solidPiece.activeSelf) {
            clonePiece.SetActive(true); 
        }
    }

    public void NotToBeSelected() { 
        displayimg.enabled = false; 
        clonePiece.SetActive(false); 
    }

    void AddPieceToGame() { 
        solidPiece.SetActive(!solidPiece.activeSelf); 
        clonePiece.SetActive(false); 
    }
    
}
