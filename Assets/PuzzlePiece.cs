using UnityEngine;

public class PuzzlePiece
{
   public int Width;
   public int Height;
   public Vector2 SolvedPosition;
   public Vector2 CurrentPosition;
   
   public Texture2D PieceImage;

   public PuzzlePiece(Vector2 _solvedPos, Vector2 _currentPos, Texture2D _image)
   {
      SolvedPosition = _solvedPos;
      CurrentPosition = _currentPos;
      PieceImage = _image;
      Width = _image.width;
      Height = _image.height;
   }
}
