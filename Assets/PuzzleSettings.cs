using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSettings
{
    public Texture2D SourceImage;
    public Texture2D ReferenceImage;
    
    public int PiecesAmount;
    public List<PuzzlePiece> PuzzlePieces { get; private set; }

    public PuzzleSettings(Texture2D _sourceImage, int _amount)
    {
        SourceImage = _sourceImage;
        PiecesAmount = _amount;

        MakePuzzle();
    }

    private void MakePuzzle()
    {
        PuzzlePieces = new List<PuzzlePiece>(PiecesAmount - 1);

        int rows = PiecesAmount;
        int colums = PiecesAmount;

        var widthHeight = 900 / PiecesAmount;

        int setIndex = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < colums; c++)
            {
                //Skip the last piece since one tile is empty
                if (setIndex == (PiecesAmount * PiecesAmount) - 1)
                    return;
                var leftPos = r * widthHeight;
                var topPos = c * widthHeight;
                Color[] col = SourceImage.GetPixels (leftPos, topPos, widthHeight, widthHeight);
                Texture2D m2Texture = new Texture2D (widthHeight, widthHeight);
                m2Texture.SetPixels (col);
                m2Texture.Apply ();
                
                //Sprite mySprity = Sprite.Create(SourceImage,new Rect(0,0,100,100),new Vector2(0,0),100);
                PuzzlePiece piece = new PuzzlePiece(new Vector2(leftPos,topPos), new Vector2(leftPos,topPos), m2Texture);
                PuzzlePieces.Add(piece);
                setIndex++;
            }
        }
    }
}
