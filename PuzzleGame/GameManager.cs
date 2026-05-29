using System;
using System.Drawing;

namespace PuzzleGame
{
    public class GameManager
    {
        public PuzzlePiece[] Board { get; private set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int PieceWidth { get; private set; }
        public int PieceHeight { get; private set; }

        public GameManager(Bitmap sourceImage, int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Board = new PuzzlePiece[rows * cols];
            PieceWidth = sourceImage.Width / Cols;
            PieceHeight = sourceImage.Height / Rows;

            InitializeBoard(sourceImage);
            ShuffleAndRotate();
        }

        private void InitializeBoard(Bitmap sourceImage)
        {
            int index = 0;
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    Rectangle cropRect = new Rectangle(x * PieceWidth, y * PieceHeight, PieceWidth, PieceHeight);
                    Bitmap pieceImg = new Bitmap(PieceWidth, PieceHeight);

                    using (Graphics g = Graphics.FromImage(pieceImg))
                    {
                        g.DrawImage(sourceImage, new Rectangle(0, 0, PieceWidth, PieceHeight), cropRect, GraphicsUnit.Pixel);
                    }

                    Board[index] = new PuzzlePiece(index, pieceImg);
                    index++;
                }
            }
        }

        private void ShuffleAndRotate()
        {
            Random rnd = new Random();

            for (int i = Board.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var temp = Board[i];
                Board[i] = Board[j];
                Board[j] = temp;
            }

            foreach (var piece in Board)
            {
                if (rnd.Next(2) == 0) // Імовірність 1/2
                {
                    piece.Rotate180();
                }
            }
        }

        public void SwapPieces(int index1, int index2)
        {
            var temp = Board[index1];
            Board[index1] = Board[index2];
            Board[index2] = temp;
        }

        public void RotatePiece(int index)
        {
            Board[index].Rotate180();
        }

        public bool IsGameWon()
        {
            for (int i = 0; i < Board.Length; i++)
            {
                if (Board[i].OriginalIndex != i || Board[i].IsRotated)
                {
                    return false;
                }
            }
            return true;
        }
    }
}