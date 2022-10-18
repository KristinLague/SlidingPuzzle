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

    public PuzzlePiece FindTileNextToEmpty()
    {
        PuzzlePiece piece = null;
        int random = Random.Range(0, 100);

        switch (random)
        {
            case <= 25:
                //Up 
                Vector2 up = new Vector2(EmptyTile.CurrentPosition.x, EmptyTile.CurrentPosition.y + EmptyTile.Height);
                piece = GetPieceByPosition(up);
                break;
            case <= 50:
                Vector2 right = new Vector2(EmptyTile.CurrentPosition.x + EmptyTile.Width, EmptyTile.CurrentPosition.y);
                piece = GetPieceByPosition(right);
                break;
            case <= 75:
                Vector2 left = new Vector2(EmptyTile.CurrentPosition.x - EmptyTile.Width, EmptyTile.CurrentPosition.y);
                piece = GetPieceByPosition(left);
                break;
            default:
                Vector2 down = new Vector2(EmptyTile.CurrentPosition.x, EmptyTile.CurrentPosition.y - EmptyTile.Height);
                piece = GetPieceByPosition(down);
                break;
        }

        return piece;
    }

    private PuzzlePiece GetPieceByPosition(Vector2 pos)
    {
        return PuzzlePieces.Find(x => x.CurrentPosition == pos);
    }

    public bool IsNextToEmptyTile(PuzzlePiece piece)
    {
        bool isHorizontal = Mathf.Abs(piece.CurrentPosition.x - EmptyTile.CurrentPosition.x) == piece.Width && Mathf.Abs(piece.CurrentPosition.y - EmptyTile.CurrentPosition.y) < piece.Height;
        bool isVertical = Mathf.Abs(piece.CurrentPosition.y - EmptyTile.CurrentPosition.y) == piece.Height &&  Mathf.Abs(piece.CurrentPosition.x - EmptyTile.CurrentPosition.x) < piece.Width;
        return isHorizontal ^ isVertical;
    }

    public void SwapWithEmptyTile(PuzzlePiece piece)
    {
        if(piece == null)
            return;
        
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
        for (int i = 0; i < PiecesAmount * 50; i++)
        {
            SwapWithEmptyTile(FindTileNextToEmpty());
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
}
