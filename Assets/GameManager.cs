using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int AmountPieces;
    public UIDocument UIDoc;

    private System.Random random;
    
    public static GameManager Instance { get; private set; }

    private Puzzle puzzleUI;
    private PuzzleSettings currentPuzzle;
    private int seed;
    private float startTime;
    public float ElapsedTime { get; private set; }

    public bool IsStarted { get; private set; }

    public event Action<PuzzleSettings> OnPuzzleStart;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        puzzleUI = new Puzzle(UIDoc.rootVisualElement);
        random = new System.Random();
        seed = random.Next(0, 10000);
        StartNewPuzzle();
        
        puzzleUI.OnGameWon += PuzzleUIOnOnGameWon;
    }

    private void Update()
    {
        ElapsedTime = Time.time - startTime;
    }

    private void PuzzleUIOnOnGameWon()
    {
        if (IsStarted){
            IsStarted = false;
            Debug.Log($"{ElapsedTime/60} Minutes");
        }
        
    }

    public void StartNewPuzzle()
    {
        StartCoroutine(StartNewPuzzle(seed));
    }

    public void SetRandomImage(VisualElement visualElement)
    {
        seed = random.Next(0, 10000);
        StartCoroutine(GetAndSetRandomImage(visualElement, seed));
    }

    public void SetGrayScaleImage(VisualElement visualElement)
    {
        StartCoroutine(GetAndSetRandomImage(visualElement, seed,true));
    }
    
    IEnumerator GetAndSetRandomImage(VisualElement element, int seed, bool isGrayScale = false)
    {
        string MediaUrl = isGrayScale ? $"https://picsum.photos/seed/{seed}/900/900?grayscale" : $"https://picsum.photos/seed/{seed}/900/900";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            element.style.backgroundImage = (DownloadHandlerTexture.GetContent(request));
        }
    }


    IEnumerator StartNewPuzzle(int seed)
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
            currentPuzzle = new PuzzleSettings(DownloadHandlerTexture.GetContent(request), AmountPieces);
            OnPuzzleStart?.Invoke(currentPuzzle);
            startTime = Time.time;
            IsStarted = true;
        }
    }


    
    
}
