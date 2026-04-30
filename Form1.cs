using System.Drawing.Printing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        enum ToolType{ Line, Rectangle, Circle }  // 사용할도형타입
        private Bitmap canvasBitmap;          // 실제그림이저장되는비트맵
        private Graphics canvasGraphics;      // 비트맵위에그리기위한객체
        private bool isDrawing= false;       // 현재드래그중인지여부
        private Point startPoint;             // 드래그시작점
        private Point endPoint;               // 드래그끝점
        private ToolType currentTool= ToolType.Line;  // 현재선택된도형
        private Color currentColor= Color.Black;      //현재색상
        private int currentLineWidth= 2;              // 현재선두께

        public Form1()
        {
            InitializeComponent();

            // 캔버스초기화
            canvasBitmap= new Bitmap(picCanvas.Width, picCanvas.Height);
            canvasGraphics= Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);   // 캔버스를흰색으로초기화
            
            picCanvas.Image= canvasBitmap;   // 그린그림을화면(PictureBox)에표시

            // 마우스이벤트연결
            picCanvas.MouseDown+= PicCanvas_MouseDown;
            picCanvas.MouseMove+= PicCanvas_MouseMove;
            picCanvas.MouseUp+= PicCanvas_MouseUp;
            // picCanvas가다시그려질때PicCanvas_Paint함수를실행하도록연결
            picCanvas.Paint+= PicCanvas_Paint;
            
            // 도형선택버튼이벤트연결
            btnLine.Click+= btnLine_Click;
            btnRectangle.Click+= btnRectangle_Click;
            btnCircle.Click+= btnCircle_Click;

            // 색상콤보박스이벤트연결
            cmbColor.SelectedIndexChanged+= cmbColor_SelectedIndexChanged;
            cmbColor.SelectedIndex= 0;  // 기본값: Black
            
            // 선두께트랙바이벤트연결
            trbLineWidth.Minimum= 1;    // 최소값
            trbLineWidth.Maximum= 10;   // 최대값
            trbLineWidth.Value= 2;
            trbLineWidth.ValueChanged+= trbLineWidth_ValueChanged;
        }
        private void PicCanvas_MouseDown(object sender, MouseEventArgs  e)
        {
            isDrawing = true;             // 드래그시작
            startPoint= e.Location;      // 시작점저장
         }
        private void PicCanvas_MouseMove(object sender, MouseEventArgs  e)
        {
            if (!isDrawing) return;       // 그림그리기와상관없는마우스움직임은무시
            
            endPoint= e.Location;        // 현재위치갱신
            
            // picCanvas를다시그려라(Paint 이벤트를발생시킨다)
            
            picCanvas.Invalidate();       // 화면다시그리기(미리보기)
         }
        private void PicCanvas_MouseUp(object sender, MouseEventArgs  e)
        {
            if (!isDrawing) return;     // 그림그리기와상관없는마우스움직임은무시
            
            isDrawing= false;          // 드래그종료
            
            endPoint= e.Location;
            
            // 실제비트맵에도형그리기(확정)
            using (Pen pen= new Pen(currentColor, currentLineWidth))
            {
                DrawShape(canvasGraphics, pen, startPoint, endPoint);
            }
            picCanvas.Invalidate();     // 다시그려서결과반영, Paint 이벤트발생
        }
        private void PicCanvas_Paint(object sender, PaintEventArgs  e)
        {
            if (!isDrawing) return;
            
            // 점선펜(미리보기용)
            using (Pen previewPen= new Pen(currentColor, currentLineWidth))
            {
                previewPen.DashStyle= DashStyle.Dash;
                DrawShape(e.Graphics, previewPen, startPoint, endPoint);
            }
        }
        private void DrawShape(Graphics g, Pen pen, Point p1, Point p2) 
        {
            Rectangle rect = GetRectangle(p1, p2);
            switch (currentTool)
            { 
                case ToolType.Line: 
                    g.DrawLine(pen, p1, p2);
                    break; case ToolType.Rectangle:
                    g.DrawRectangle(pen, rect); 
                    break; case ToolType.Circle: 
                    g.DrawEllipse(pen, rect); break;
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
        private void btnLine_Click(object sender, EventArgs  e) 
        { 
            currentTool = ToolType.Line;
        }
        private void btnRectangle_Click(object sender, EventArgs e) 
        {
            currentTool = ToolType.Rectangle; 
        }
        private void btnCircle_Click(object sender, EventArgs  e) 
        {
            currentTool = ToolType.Circle; 
        }
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs  e)
        {
            switch (cmbColor.SelectedIndex)
            {
                case 0: // Black 검정
                    currentColor= Color.Black;
                    break;
                case 1: // Red 빨강
                    currentColor= Color.Red;
                    break;
                case 2: // Blue 파랑
                    currentColor= Color.Blue;
                    break;
                case 3: // Green 녹색
                    currentColor= Color.Green;
                    break;
                default:
                    currentColor= Color.Black;
                    break;
            }
        }
        private void trbLineWidth_ValueChanged(object sender, EventArgs e) 
        {
            currentLineWidth = trbLineWidth.Value; 
        }
    }
}
