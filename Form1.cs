using System;
using System.Drawing;
using System.Drawing.Imaging; // 이미지 포맷 지정을 위해 필수
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO; // 파일 경로 및 확장자 처리를 위해 추가

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        enum ToolType { Line, Rectangle, Circle }
        private Bitmap canvasBitmap;
        private Graphics canvasGraphics;
        private bool isDrawing = false;
        private Point startPoint;
        private Point endPoint;
        private ToolType currentTool = ToolType.Line;
        private Color currentColor = Color.Black;
        private int currentLineWidth = 2;

        public Form1()
        {
            InitializeComponent();

            // 캔버스 초기화
            canvasBitmap = new Bitmap(picCanvas.Width, picCanvas.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);
            picCanvas.Image = canvasBitmap;

            // 이벤트 연결
            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;

            btnLine.Click += btnLine_Click;
            btnRectangle.Click += btnRectangle_Click;
            btnCircle.Click += btnCircle_Click;

            // '저장' 버튼 이벤트 연결 (image_287739.png 요구사항)
            btnSaveFile.Click += btnSave_Click;

            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            cmbColor.SelectedIndex = 0;

            trbLineWidth.Minimum = 1;
            trbLineWidth.Maximum = 10;
            trbLineWidth.Value = 2;
            trbLineWidth.ValueChanged += trbLineWidth_ValueChanged;
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = e.Location;
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            endPoint = e.Location;
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            isDrawing = false;
            endPoint = e.Location;

            using (Pen pen = new Pen(currentColor, currentLineWidth))
            {
                DrawShape(canvasGraphics, pen, startPoint, endPoint);
            }
            picCanvas.Invalidate();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (!isDrawing) return;

            using (Pen previewPen = new Pen(currentColor, currentLineWidth))
            {
                previewPen.DashStyle = DashStyle.Dash;
                DrawShape(e.Graphics, previewPen, startPoint, endPoint);
            }
        }

        private void DrawShape(Graphics g, Pen pen, Point p1, Point p2)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias; // 선을 부드럽게 처리
            Rectangle rect = GetRectangle(p1, p2);
            switch (currentTool)
            {
                case ToolType.Line:
                    g.DrawLine(pen, p1, p2);
                    break;
                case ToolType.Rectangle:
                    g.DrawRectangle(pen, rect);
                    break;
                case ToolType.Circle:
                    g.DrawEllipse(pen, rect);
                    break;
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }

        // --- 파일 저장 기능 (SaveFileDialog 사용) ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "이미지 저장";
                sfd.OverwritePrompt = true;
                // .png, .jpg, .bmp 3가지 포맷 필터 설정
                sfd.Filter = "PNG 파일 (*.png)|*.png|JPEG 파일 (*.jpg)|*.jpg|BMP 파일 (*.bmp)|*.bmp";
                sfd.DefaultExt = "png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ext = Path.GetExtension(sfd.FileName).ToLower();
                        ImageFormat format = ImageFormat.Png;

                        if (ext == ".jpg" || ext == ".jpeg") format = ImageFormat.Jpeg;
                        else if (ext == ".bmp") format = ImageFormat.Bmp;

                        canvasBitmap.Save(sfd.FileName, format);
                        MessageBox.Show("저장이 완료되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("오류 발생: " + ex.Message);
                    }
                }
            }
        }

        private void btnLine_Click(object sender, EventArgs e) => currentTool = ToolType.Line;
        private void btnRectangle_Click(object sender, EventArgs e) => currentTool = ToolType.Rectangle;
        private void btnCircle_Click(object sender, EventArgs e) => currentTool = ToolType.Circle;

        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbColor.SelectedIndex)
            {
                case 0: currentColor = Color.Black; break;
                case 1: currentColor = Color.Red; break;
                case 2: currentColor = Color.Blue; break;
                case 3: currentColor = Color.Green; break;
            }
        }

        private void trbLineWidth_ValueChanged(object sender, EventArgs e)
        {
            currentLineWidth = trbLineWidth.Value;
        }
    }
}