using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Puzzle
{
    private VisualElement root;
    private PuzzleSettings puzzle;
    private VisualElement puzzleContainer;
    
    private readonly VisualTreeAsset puzzlePieceTemplate;

    public Puzzle(VisualElement _root, PuzzleSettings _puzzle)
    {
        root = _root;
        puzzle = _puzzle;
        puzzlePieceTemplate = Resources.Load("UXML/PuzzlePiece") as VisualTreeAsset;
        SetupUI();
    }

    private void SetupUI()
    {
        puzzleContainer = root.Q<VisualElement>("puzzle-container");
        //puzzleContainer.style.backgroundImage = puzzle.PuzzlePieces[0].PieceImage;
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
            puzzleContainer.Add(template);
        }
    }
    
    

}
