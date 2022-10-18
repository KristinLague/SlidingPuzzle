using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int AmountPieces;
    public UIDocument UIDoc;

    private System.Random random;

    private Puzzle puzzleUI;
    
    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        StartCoroutine(DownloadImage(random.Next(0,100)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator DownloadImage(int seed)
    {
        string MediaUrl = $"https://picsum.photos/seed/{seed}/900/900";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("SUCCESS");
            
            PuzzleSettings currentPuzzle = new PuzzleSettings(DownloadHandlerTexture.GetContent(request), AmountPieces);
            StartPuzzle(currentPuzzle);
        }
        // YourRawImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }

    private void StartPuzzle(PuzzleSettings puzzle)
    {
        puzzleUI = new Puzzle(UIDoc.rootVisualElement, puzzle);
    }
    
    
}
