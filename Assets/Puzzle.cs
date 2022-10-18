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
    private UITKEventHelper eventHelper;

    private VisualElement emptyTile;

    public Puzzle(VisualElement _root, PuzzleSettings _puzzle)
    {
        root = _root;
        puzzle = _puzzle;
        puzzlePieceTemplate = Resources.Load("UXML/PuzzlePiece") as VisualTreeAsset;
        SetupUI();
    }

    private void SetupUI()
    {
        eventHelper = new UITKEventHelper();
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

            if (piece.IsEmpty)
                emptyTile = template;
            
            eventHelper.RegisterCallback<ClickEvent>(template, (evt) => TryMovePiece(template,piece));
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
        }
        
    }
}
