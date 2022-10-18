using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Puzzle
{
    private VisualElement root;
    private PuzzleSettings puzzle;
    private VisualElement puzzleContainer;
    private VisualElement puzzlePreview;
    private Label timeLabel;
    
    //Puzzle Setting
    private VisualElement randomImagePreview;
    private Button newRandomImageButton;
    private SliderInt difficultySlider;
    private Label difficultyLabel;

    private readonly VisualTreeAsset puzzlePieceTemplate;
    private UITKEventHelper eventHelper;

    private VisualElement emptyTile;

    public event Action OnGameWon;

    public Puzzle(VisualElement _root)
    {
        root = _root;
        puzzlePieceTemplate = Resources.Load("UXML/PuzzlePiece") as VisualTreeAsset;
        SetupUI();
    }

    private void SetupUI()
    {
        eventHelper = new UITKEventHelper();
        puzzleContainer = root.Q<VisualElement>("puzzle-container");
        puzzlePreview = root.Q<VisualElement>("puzzle-preview");

        //Settings
        randomImagePreview = root.Q<VisualElement>("random--preview");
        newRandomImageButton = root.Q<Button>("random--button");
        difficultySlider = root.Q<SliderInt>("difficulty--slider");
        difficultyLabel = root.Q<Label>("difficulty--label");
        timeLabel = root.Q<Label>("time--label");
        
        GameManager.Instance.OnPuzzleStart += OnPuzzleStart;
        eventHelper.RegisterCallback<ClickEvent>(newRandomImageButton, evt =>
        {
            GameManager.Instance.SetRandomImage(randomImagePreview);
            GameManager.Instance.StartNewPuzzle();
        });
        
        eventHelper.RegisterValueChangedCallback(difficultySlider, evt =>
        {
            difficultyLabel.text = $"{difficultySlider.value} x {difficultySlider.value}";
            GameManager.Instance.AmountPieces = difficultySlider.value;
            GameManager.Instance.StartNewPuzzle();
        });
    }

    private void OnPuzzleStart(PuzzleSettings _puzzle)
    {
        puzzle = _puzzle;
        StartPuzzle();

        timeLabel.schedule.Execute(() =>
        {
            if (puzzle.HasWon())
            {
                timeLabel.text = $"Congratulations, it took you {Mathf.RoundToInt(GameManager.Instance.ElapsedTime / 60):00}:{Mathf.Round(GameManager.Instance.ElapsedTime % 60):00} Minutes to solve this puzzle!";
            }
            else
            {
                timeLabel.text =
                    $"{Mathf.RoundToInt(GameManager.Instance.ElapsedTime / 60):00}:{Mathf.Round(GameManager.Instance.ElapsedTime % 60):00}";
            }
        }).Every(100).Until(() => GameManager.Instance.IsStarted == false);
    }

    private void StartPuzzle()
    {
        puzzlePreview.style.backgroundImage = puzzle.SourceImage;
        puzzleContainer.style.backgroundImage = null;
        randomImagePreview.style.backgroundImage = puzzle.SourceImage;
        
        puzzleContainer.Clear();
        
        foreach (var piece in puzzle.PuzzlePieces)
        {
            var template = puzzlePieceTemplate.Instantiate();
            template.style.width = piece.Width;
            template.style.height = piece.Height;
            template.style.backgroundImage = piece.PieceImage;
            template.style.position = Position.Absolute;
            template.style.left = piece.CurrentPosition.x;
            template.style.bottom = piece.CurrentPosition.y;
            template.name = puzzle.PuzzlePieces.IndexOf(piece).ToString();

            if (piece.IsEmpty)
                emptyTile = template;

            eventHelper.RegisterCallback<ClickEvent>(template, (evt) => TryMovePiece(template, piece));
            puzzleContainer.Add(template);
        }
    }

    private void TryMovePiece(VisualElement tile, PuzzlePiece piece)
    {
        if (piece.IsEmpty)
        {
            Debug.Log("Can't move empty tile");
            return;
        }
        //Check if adjacent tiles are empty
        if (puzzle.IsNextToEmptyTile(piece))
        {
            puzzle.SwapWithEmptyTile(piece);
            tile.style.left = piece.CurrentPosition.x;
            tile.style.bottom = piece.CurrentPosition.y;
            
            emptyTile.style.left = puzzle.EmptyTile.CurrentPosition.x;
            emptyTile.style.bottom = puzzle.EmptyTile.CurrentPosition.y;

            if (puzzle.HasWon())
            {
                OnGameWon?.Invoke();
                puzzleContainer.Clear();
                puzzleContainer.style.backgroundImage = puzzle.SourceImage;
            }
        }
        
    }
}
