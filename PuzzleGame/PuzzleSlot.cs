using System.Drawing;
using System.Windows.Forms;

namespace PuzzleGame
{
    public class PuzzleSlot : PictureBox
    {
        public int BoardIndex { get; private set; }

        public PuzzleSlot(int boardIndex, int width, int height, Point location)
        {
            BoardIndex = boardIndex;
            Width = width;
            Height = height;
            Location = location;
            BorderStyle = BorderStyle.FixedSingle;
            SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void SetPiece(PuzzlePiece piece)
        {
            this.Image = piece.Image;
        }

        public void SelectSlot() { BorderStyle = BorderStyle.Fixed3D; }
        public void DeselectSlot() { BorderStyle = BorderStyle.FixedSingle; }
        public void RemoveBorder() { BorderStyle = BorderStyle.None; }
    }
}