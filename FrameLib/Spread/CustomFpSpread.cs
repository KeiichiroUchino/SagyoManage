using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// </summary>
    public partial class CustomFpSpread : FarPoint.Win.Spread.FpSpread
    {
        private System.Windows.Forms.ImageList imageList;

        /// <summary>
        /// </summary>
        public CustomFpSpread()
        {
            InitializeComponent();

            imageList = new System.Windows.Forms.ImageList();
        }

        /// <summary>
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// ゴースト付きで DragDrop を開始します。
        /// ゴーストは ActiveRowにある先頭セルの文字列 をキャプチャします。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="allowEffects"></param>
        /// <param name="ghostText">ゴーストのテキスト</param>
        /// <param name="font">テキストのフォント</param>
        /// <returns></returns>
        public DragDropEffects DoDragDropWithGhost(object data, DragDropEffects allowEffects, string ghostText, Font font)
        {
            this.Refresh();

            //描画するテキストのサイズ
            var textSize = TextRenderer.MeasureText(ghostText, font);
            // ドラッグするイメージのサイズを求める
            Size imageSize = new Size(textSize.Width, textSize.Height);

            //***ImageListのサイズの上限を超えないようにカットする
            if (imageSize.Width > 256)
            {
                imageSize.Width = 256;
            }
            if (imageSize.Height > 256)
            {
                imageSize.Height = 256;
            }

            if (imageSize.Width == 0 || imageSize.Height == 0)
            {
                return this.DoDragDrop(data, allowEffects);
            }

            #region ドラッグするイメージを作成する

            //***ここの imageRectangle は Intersect した後の値
            Bitmap bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            using (Brush fontBrush = new SolidBrush(Color.Black))
            {
                string text = ghostText;
                graphics.DrawString(text, font, fontBrush, 0, 0);
            }

            #endregion

            this.imageList.Images.Clear();
            this.imageList.ImageSize = bitmap.Size;
            this.imageList.Images.Add(bitmap);
            //解放
            graphics.Dispose();

            return this.DoDragDropWithGhostInternal(data, allowEffects, bitmap, new Point(0, 0));
        }

        /// <summary>
        /// ゴースト付きで DragDrop を開始します。
        /// ゴーストは ActiveRowにある先頭セルの文字列 をキャプチャします。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="allowEffects"></param>
        /// <returns></returns>
        public DragDropEffects DoDragDropWithGhost(object data, DragDropEffects allowEffects)
        {
            this.Refresh();

            SheetView sheetView = this.ActiveSheet;
            var activeRow = sheetView.ActiveRow;
            int activeRowIndex = activeRow.Index;

            //行の中のどのセルをゴーストにするか。今のところは暫定で Index:0 のものにしておく
            int cellIndex = 0;
            Cell cell = sheetView.Cells[activeRowIndex, cellIndex];

            if (activeRow == null)
            {
                return DragDropEffects.None;
            }

            Point mousePosition = this.PointToClient(Control.MousePosition);

            // ドラッグするイメージのサイズを求める
            Rectangle imageRectangle = this.GetCellRectangle(0, 0, activeRowIndex, cellIndex);
            Point cellPoint = new Point(imageRectangle.Left, imageRectangle.Top);

            //カーソルを中心に256 * 256の四角形の位置をとる。この部分だけを描画する。ImageListのサイズ上限が 256 * 256 のため
            Rectangle cursorNearRectangle = new Rectangle(mousePosition.X - 128, mousePosition.Y - 128, 256, 256);
            imageRectangle.Intersect(cursorNearRectangle);

            if (imageRectangle.Width == 0 || imageRectangle.Height == 0)
            {
                return this.DoDragDrop(data, allowEffects);
            }

            #region ドラッグするイメージを作成する

            //***ここの imageRectangle は Intersect した後の値
            Bitmap bitmap = new Bitmap(imageRectangle.Width, imageRectangle.Height);
            Graphics graphics = Graphics.FromImage(bitmap);


            //Graphics上のポジション
            Point positionInGraphics = cellPoint;
            //X,Yを修正する
            positionInGraphics.Offset(-imageRectangle.X, -imageRectangle.Y);

            using (Brush fontBrush = new SolidBrush(Color.Black))
            {
                string text = cell.Text;
                graphics.DrawString(text, this.Font, fontBrush, positionInGraphics);
            }

            #endregion

            this.imageList.Images.Clear();
            this.imageList.ImageSize = bitmap.Size;
            this.imageList.Images.Add(bitmap);

            //解放
            graphics.Dispose();

            //ドラッグした点を計算
            Point dragPoint = new Point(mousePosition.X - imageRectangle.X, mousePosition.Y - imageRectangle.Y);

            return this.DoDragDropWithGhostInternal(data, allowEffects, bitmap, dragPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="allowEffects"></param>
        /// <param name="bitmap"></param>
        /// <param name="dragPoint">イメージ対象とドラッグ点の相対位置　指定した分だけ左と上にゴースト位置がずれる</param>
        /// <returns></returns>
        public DragDropEffects DoDragDropWithGhostInternal(object data, DragDropEffects allowEffects, Bitmap bitmap, Point dragPoint)
        {
            //this.imageList.Images.Clear();
            //this.imageList.ImageSize = bitmap.Size;
            //this.imageList.Images.Add(bitmap);

            ////ドラッグ開始メソッド（W32）
            //Win32API.ImageList.ImageList_BeginDrag(this.imageList.Handle, 0, dragPoint.X, dragPoint.Y);
            //Win32API.ImageList.ImageList_DragEnter(Win32API.Common.GetDesktopWindow(), Cursor.Position.X, Cursor.Position.Y);

            //try
            //{
            //    return this.DoDragDrop(data, allowEffects);
            //}
            //finally
            //{

            //}

            return new DragDropEffects();
        }
    }
}
