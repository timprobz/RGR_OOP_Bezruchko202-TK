using System.Drawing;

namespace PuzzleGame
{
    public class PuzzlePiece
    {
        public int OriginalIndex { get; private set; }
        public bool IsRotated { get; private set; }
        public Bitmap Image { get; private set; }

        public PuzzlePiece(int originalIndex, Bitmap image)
        {
            OriginalIndex = originalIndex;
            Image = image;
            IsRotated = false;
        }

        public void Rotate180()
        {
            Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            IsRotated = !IsRotated;
        }
    }
}