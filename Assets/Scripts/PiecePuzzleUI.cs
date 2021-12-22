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

	private AudioSource audioSource;
	public AudioClip[] audioFiles;
	private float minVol = 0.1f;
	private float maxVol = 0.2f;

	[SerializeField]
    public Vector3 positionToAppear; 

    void Awake() { 
        displayimg = GetComponent<RawImage>(); 
        displayButton = GetComponent<Button>(); 
        displayimg.enabled = false; 
        solidPiece.SetActive(false);
		//initialize audio
		if (this.transform.GetComponent<AudioSource>())
		{
			this.audioSource = this.transform.GetComponent<AudioSource>();
		}
		else
		{
			this.audioSource = this.gameObject.AddComponent<AudioSource>();
		}
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

		//Play sound
		AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
		audioSource.clip = randomAudio;
		audioSource.volume = Random.Range(minVol, maxVol);
		audioSource.Play();
	}

    public bool isPieceInGame() { 
        return isInGame; 
    }
    
}
