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

            // PictureBox의 SizeMode 설정
            picCanvas.SizeMode = PictureBoxSizeMode.StretchImage;

            // 캔버스 초기화 (디자인 타임의 PictureBox 크기 기준)
            InitializeCanvas(pnlCanvas.Width, pnlCanvas.Height);

            // 이벤트 핸들러 등록
            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;

            // 폼 전체에서 마우스 휠 감지 (Ctrl + 휠)
            this.MouseWheel += Form1_MouseWheel;

            // Panel에 마우스 휠 이벤트도 등록
            pnlCanvas.MouseWheel += Form1_MouseWheel;

            // 버튼 이벤트 (이름이 일치하는지 확인 필요)
            btnLine.Click += (s, e) => currentTool = ToolType.Line;
            btnRectangle.Click += (s, e) => currentTool = ToolType.Rectangle;
            btnCircle.Click += (s, e) => currentTool = ToolType.Circle;
            btnSaveFile.Click += btnSave_Click;
            btnOpenFile.Click += btnOpen_Click;

            if (cmbColor.Items.Count > 0) cmbColor.SelectedIndex = 0;
            cmbColor.SelectedIndexChanged += (s, e) => {
                // 간단한 색상 변환 예시
                string selected = cmbColor.SelectedItem.ToString();
                currentColor = Color.FromName(selected);
            };

            trbLineWidth.Scroll += (s, e) => currentLineWidth = trbLineWidth.Value;
        }

        private void InitializeCanvas(int width, int height)
        {
            if (canvasBitmap != null) canvasBitmap.Dispose();
            canvasBitmap = new Bitmap(width, height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);
            UpdateCanvasDisplay();
        }

        private void UpdateCanvasDisplay()
        {
            if (canvasBitmap == null) return;

            // 1. PictureBox의 크기를 (원본 * 배율)로 물리적으로 키웁니다.
            // 이 작업이 Panel의 AutoScroll을 트리거합니다.
            int newWidth = (int)(canvasBitmap.Width * zoomFactor);
            int newHeight = (int)(canvasBitmap.Height * zoomFactor);

            picCanvas.Size = new Size(newWidth, newHeight);
            picCanvas.Image = canvasBitmap;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                // 휠 방향에 따라 배율 조정 (0.1 ~ 5.0 배)
                if (e.Delta > 0) zoomFactor = Math.Min(zoomFactor + 0.1f, 5.0f);
                else zoomFactor = Math.Max(zoomFactor - 0.1f, 0.1f);

                UpdateCanvasDisplay();
                // 휠 동작 시 폼이 스크롤되는 것을 방지하기 위해 이벤트 처리 완료 표시
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private Point GetAdjustedPoint(Point p)
        {
            // Panel의 스크롤 위치를 고려하여 실제 PictureBox 상의 좌표 계산
            Point adjustedForScroll = new Point(
                p.X + pnlCanvas.AutoScrollPosition.X,
                p.Y + pnlCanvas.AutoScrollPosition.Y
            );

            // 확대된 화면의 좌표를 원본 비트맵 좌표로 역계산
            return new Point((int)(adjustedForScroll.X / zoomFactor), (int)(adjustedForScroll.Y / zoomFactor));
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
            picCanvas.Invalidate(); // Paint 이벤트 호출
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

            // 미리보기 그리기 시에도 현재 배율 반영
            using (Pen previewPen = new Pen(currentColor, currentLineWidth))
            {
                previewPen.DashStyle = DashStyle.Dash;

                // 줌된 좌표로 미리보기 표시
                Point scaledStart = new Point((int)(startPoint.X * zoomFactor), (int)(startPoint.Y * zoomFactor));
                Point scaledEnd = new Point((int)(endPoint.X * zoomFactor), (int)(endPoint.Y * zoomFactor));

                DrawShapeScaled(e.Graphics, previewPen, scaledStart, scaledEnd);
            }
        }

        private void DrawShapeScaled(Graphics g, Pen pen, Point p1, Point p2)
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

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 파일 잠김 방지를 위해 스트림으로 읽기
                    using (var img = Image.FromFile(ofd.FileName))
                    {
                        canvasBitmap = new Bitmap(img);
                        canvasGraphics = Graphics.FromImage(canvasBitmap);
                    }
                    zoomFactor = 1.0f;
                    UpdateCanvasDisplay();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (canvasBitmap == null) return;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG|*.png|JPG|*.jpg";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    canvasBitmap.Save(sfd.FileName, sfd.FileName.EndsWith(".jpg") ? ImageFormat.Jpeg : ImageFormat.Png);
                }
            }
        }
    }
}