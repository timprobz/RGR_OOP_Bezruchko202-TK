using System;
using System.Drawing;
using System.Windows.Forms;

namespace PuzzleGame
{
    public class MainForm : Form
    {
        private Panel topPanel, puzzlePanel;
        private Button btnLoadImage, btnStartGame;
        private NumericUpDown numRows, numCols;

        private Bitmap originalImage;
        private GameManager gameManager;
        private PuzzleSlot firstSelectedSlot = null;

        public MainForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Гра: Пазл";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            topPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.LightGray };

            btnLoadImage = new Button { Text = "Завантажити фото", Location = new Point(10, 15), Width = 130 };
            btnLoadImage.Click += BtnLoadImage_Click;

            Label lblRows = new Label { Text = "Рядки (N):", Location = new Point(160, 20), Width = 65 };
            numRows = new NumericUpDown { Location = new Point(230, 18), Width = 50, Minimum = 2, Maximum = 10, Value = 3 };

            Label lblCols = new Label { Text = "Стовпці (M):", Location = new Point(290, 20), Width = 75 };
            numCols = new NumericUpDown { Location = new Point(370, 18), Width = 50, Minimum = 2, Maximum = 10, Value = 3 };

            btnStartGame = new Button { Text = "Почати гру", Location = new Point(440, 15), Width = 100, Enabled = false };
            btnStartGame.Click += BtnStartGame_Click;

            topPanel.Controls.AddRange(new Control[] { btnLoadImage, lblRows, numRows, lblCols, numCols, btnStartGame });

            puzzlePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };

            this.Controls.Add(puzzlePanel);
            this.Controls.Add(topPanel);
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Зображення|*.jpg;*.png;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image loadedImage = Image.FromFile(ofd.FileName);
                    originalImage = ScaleImage(loadedImage, 800, 550);
                    btnStartGame.Enabled = true;
                }
            }
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;

            puzzlePanel.Controls.Clear();
            firstSelectedSlot = null;

            int rows = (int)numRows.Value;
            int cols = (int)numCols.Value;

            gameManager = new GameManager(originalImage, rows, cols);

            for (int i = 0; i < gameManager.Board.Length; i++)
            {
                int r = i / cols;
                int c = i % cols;
                Point location = new Point(c * gameManager.PieceWidth, r * gameManager.PieceHeight);

                PuzzleSlot slot = new PuzzleSlot(i, gameManager.PieceWidth, gameManager.PieceHeight, location);
                slot.SetPiece(gameManager.Board[i]);
                slot.MouseClick += Slot_MouseClick;

                puzzlePanel.Controls.Add(slot);
            }
        }

        private void Slot_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameManager == null || gameManager.IsGameWon()) return;

            PuzzleSlot clickedSlot = (PuzzleSlot)sender;

            // Правий клік: Поворот
            if (e.Button == MouseButtons.Right)
            {
                gameManager.RotatePiece(clickedSlot.BoardIndex);
                clickedSlot.SetPiece(gameManager.Board[clickedSlot.BoardIndex]);

                if (firstSelectedSlot != null)
                {
                    firstSelectedSlot.DeselectSlot();
                    firstSelectedSlot = null;
                }

                CheckWin();
            }
            // Лівий клік: Обмін
            else if (e.Button == MouseButtons.Left)
            {
                if (firstSelectedSlot == null)
                {
                    firstSelectedSlot = clickedSlot;
                    firstSelectedSlot.SelectSlot();
                }
                else
                {
                    if (firstSelectedSlot != clickedSlot)
                    {
                        gameManager.SwapPieces(firstSelectedSlot.BoardIndex, clickedSlot.BoardIndex);

                        firstSelectedSlot.SetPiece(gameManager.Board[firstSelectedSlot.BoardIndex]);
                        clickedSlot.SetPiece(gameManager.Board[clickedSlot.BoardIndex]);
                    }

                    firstSelectedSlot.DeselectSlot();
                    firstSelectedSlot = null;
                    CheckWin();
                }
            }
        }

        private void CheckWin()
        {
            if (gameManager.IsGameWon())
            {
                foreach (Control ctrl in puzzlePanel.Controls)
                {
                    if (ctrl is PuzzleSlot slot) slot.RemoveBorder();
                }
                MessageBox.Show("Вітаємо! Ви успішно відновили зображення!", "Перемога");
            }
        }

        private Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratio = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);
            if (ratio >= 1.0) return new Bitmap(image);
            return new Bitmap(image, (int)(image.Width * ratio), (int)(image.Height * ratio));
        }
    }
}