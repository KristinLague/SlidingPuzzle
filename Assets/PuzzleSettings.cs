using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSettings
{
    public Texture2D SourceImage;
    public Texture2D ReferenceImage;

    public int PiecesAmount;
    public List<PuzzlePiece> PuzzlePieces { get; private set; }
    public PuzzlePiece EmptyTile { get; private set; }

    public PuzzleSettings(Texture2D _sourceImage, int _amount)
    {
        SourceImage = _sourceImage;
        PiecesAmount = _amount;

        MakePuzzle();
    }

    public bool IsNextToEmptyTile(PuzzlePiece piece)
    {
        bool isHorizontal = Mathf.Abs(piece.CurrentPosition.x - EmptyTile.CurrentPosition.x) == piece.Width && Mathf.Abs(piece.CurrentPosition.y - EmptyTile.CurrentPosition.y) < piece.Height;
        bool isVertical = Mathf.Abs(piece.CurrentPosition.y - EmptyTile.CurrentPosition.y) == piece.Height &&  Mathf.Abs(piece.CurrentPosition.x - EmptyTile.CurrentPosition.x) < piece.Width;
        return isHorizontal ^ isVertical;
    }

    public void SwapWithEmptyTile(PuzzlePiece piece)
    {
        Vector2 temp = piece.CurrentPosition;
        piece.CurrentPosition = EmptyTile.CurrentPosition;
        EmptyTile.CurrentPosition = temp;
    }

    private void MakePuzzle()
    {
        PuzzlePieces = new List<PuzzlePiece>(PiecesAmount - 1);

        int rows = PiecesAmount;
        int colums = PiecesAmount;

        var widthHeight = 900 / PiecesAmount;

        int setIndex = 0;
        for (int c = 0; c < colums; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                bool isEmpty = (setIndex == (PiecesAmount * PiecesAmount) - 1);
                    
                var leftPos = r * widthHeight;
                var bottomPos = c * widthHeight;
                
                Color[] col = SourceImage.GetPixels (leftPos, bottomPos, widthHeight, widthHeight);
                Texture2D m2Texture = new Texture2D (widthHeight, widthHeight);
                m2Texture.SetPixels (col);
                m2Texture.Apply ();
                
                PuzzlePiece piece = new PuzzlePiece(new Vector2(leftPos,bottomPos), isEmpty ? null : m2Texture, widthHeight,widthHeight, isEmpty);
                PuzzlePieces.Add(piece);

                if (isEmpty)
                    EmptyTile = piece;
                
                setIndex++;
            }
        }
        
        ShufflePieces();
    }

    private void ShufflePieces()
    {
        List<Vector2> possiblePositions = new List<Vector2>();
        foreach (var piece in PuzzlePieces)
        {
            if (!piece.IsEmpty)
                possiblePositions.Add(piece.SolvedPosition);
        }

        Debug.Log(PuzzlePieces.Count);
        Shuffle(possiblePositions);
        for(int i = 0; i < PuzzlePieces.Count; i++)
        {
            if(!PuzzlePieces[i].IsEmpty)
                PuzzlePieces[i].CurrentPosition = possiblePositions[i];
        }
    }

    public bool HasWon()
    {
        foreach (var piece in PuzzlePieces)
        {
            if (piece.CurrentPosition != piece.SolvedPosition)
                return false;
        }

        return true;
    }
    
    private void Shuffle<T>(IList<T> values)
    {
        System.Random rng = new System.Random();

        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rng.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
}
