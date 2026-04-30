using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

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

        private float zoomFactor = 1.0f;

        public Form1()
        {
            InitializeComponent();

            // 초기 캔버스 설정
            InitializeCanvas(picCanvas.Width, picCanvas.Height);

            // 이벤트 연결
            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;

            btnLine.Click += (s, e) => currentTool = ToolType.Line;
            btnRectangle.Click += (s, e) => currentTool = ToolType.Rectangle;
            btnCircle.Click += (s, e) => currentTool = ToolType.Circle;

            // 버튼 이름 확인: 디자인 창에서의 Name과 일치해야 합니다.
            btnSaveFile.Click += btnSave_Click;
            btnOpenFile.Click += btnOpen_Click;

            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;

            // 콤보박스 아이템이 비어있으면 SelectedIndex 설정 시 오류가 납니다.
            if (cmbColor.Items.Count > 0) cmbColor.SelectedIndex = 0;

            trbLineWidth.Scroll += (s, e) => currentLineWidth = trbLineWidth.Value;
        }

        private void InitializeCanvas(int width, int height)
        {
            if (canvasBitmap != null) canvasBitmap.Dispose();
            if (canvasGraphics != null) canvasGraphics.Dispose();

            canvasBitmap = new Bitmap(width, height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);

            picCanvas.Size = new Size((int)(width * zoomFactor), (int)(height * zoomFactor));
            picCanvas.Image = canvasBitmap;
        }

        // --- 외부 이미지 불러오기 (수정됨: 파일 잠금 방지) ---
        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Image.FromFile 대신 스트림을 사용하면 원본 파일이 잠기지 않습니다.
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (Image loadedImg = Image.FromStream(fs))
                        {
                            canvasBitmap = new Bitmap(loadedImg.Width, loadedImg.Height);
                            canvasGraphics = Graphics.FromImage(canvasBitmap);
                            canvasGraphics.DrawImage(loadedImg, 0, 0);
                        }
                    }
                    UpdateCanvasDisplay();
                }
            }
        }

        // --- 파일 저장 기능 추가 ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG 파일|*.png|JPG 파일|*.jpg|BMP 파일|*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string ext = Path.GetExtension(sfd.FileName).ToLower();
                    ImageFormat format = ImageFormat.Png;

                    if (ext == ".jpg") format = ImageFormat.Jpeg;
                    else if (ext == ".bmp") format = ImageFormat.Bmp;

                    canvasBitmap.Save(sfd.FileName, format);
                    MessageBox.Show("저장되었습니다.");
                }
            }
        }

        // --- 색상 선택 메서드 추가 ---
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colorText = cmbColor.SelectedItem.ToString();
            if (colorText.Contains("검정") || colorText.Contains("Black")) currentColor = Color.Black;
            else if (colorText.Contains("빨강") || colorText.Contains("Red")) currentColor = Color.Red;
            else if (colorText.Contains("파랑") || colorText.Contains("Blue")) currentColor = Color.Blue;
            else if (colorText.Contains("녹색") || colorText.Contains("Green")) currentColor = Color.Green;
        }

        private void UpdateCanvasDisplay()
        {
            picCanvas.Size = new Size(
                (int)(canvasBitmap.Width * zoomFactor),
                (int)(canvasBitmap.Height * zoomFactor)
            );
            picCanvas.SizeMode = PictureBoxSizeMode.Zoom;
            picCanvas.Image = canvasBitmap;
        }

        private Point GetAdjustedPoint(Point p)
        {
            return new Point((int)(p.X / zoomFactor), (int)(p.Y / zoomFactor));
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = GetAdjustedPoint(e.Location);
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            endPoint = GetAdjustedPoint(e.Location);
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            isDrawing = false;
            endPoint = GetAdjustedPoint(e.Location);

            using (Pen pen = new Pen(currentColor, currentLineWidth))
            {
                DrawShape(canvasGraphics, pen, startPoint, endPoint);
            }
            picCanvas.Invalidate();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (!isDrawing) return;

            e.Graphics.ScaleTransform(zoomFactor, zoomFactor);
            using (Pen previewPen = new Pen(currentColor, currentLineWidth))
            {
                previewPen.DashStyle = DashStyle.Dash;
                DrawShape(e.Graphics, previewPen, startPoint, endPoint);
            }
        }

        public void SetZoom(float scale)
        {
            zoomFactor = scale;
            UpdateCanvasDisplay();
        }

        private void DrawShape(Graphics g, Pen pen, Point p1, Point p2)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = GetRectangle(p1, p2);
            switch (currentTool)
            {
                case ToolType.Line: g.DrawLine(pen, p1, p2); break;
                case ToolType.Rectangle: g.DrawRectangle(pen, rect); break;
                case ToolType.Circle: g.DrawEllipse(pen, rect); break;
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }
    }
}