using UnityEngine;

public class PuzzlePiece
{
   public bool IsEmpty;
   public int Width;
   public int Height;
   public Vector2 SolvedPosition;
   public Vector2 CurrentPosition;
   
   public Texture2D PieceImage;

   public PuzzlePiece(Vector2 _solvedPos, Texture2D _image, int _height, int _width, bool _isEmpty = false)
   {
      SolvedPosition = _solvedPos;
      CurrentPosition = _solvedPos;
      PieceImage = _image;
      IsEmpty = _isEmpty;
      Width = _width;
      Height = _height;
   }
}
