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
    private bool isInGame = false; 
    public GameObject solidPiece; 
    public GameObject clonePiece; 

    [SerializeField]
    public Vector3 positionToAppear; 

    void Awake() { 
        displayimg = GetComponent<RawImage>(); 
        displayButton = GetComponent<Button>(); 
        displayimg.enabled = false; 
        solidPiece.SetActive(false); 
    }

    // Start is called before the first frame update
    void Start()
    {
        displayButton.onClick.AddListener(AddPieceToGame); 
    }

    public void ToBeSelected() { 
        displayimg.enabled = true; 
        clonePiece.SetActive(true); 
    }

    public void NotToBeSelected() { 
        displayimg.enabled = false; 
        clonePiece.SetActive(false); 
    }

    public void AddPieceToGame() { 
        // clonePiece.SetActive(!clonePiece.activeSelf); 
        solidPiece.SetActive(!solidPiece.activeSelf); 
        solidPiece.GetComponent<Transform>().localPosition = positionToAppear; 
        displayimg.enabled = true; 
        isInGame = !isInGame; 
    }

    public bool isPieceInGame() { 
        return isInGame; 
    }
    
}
