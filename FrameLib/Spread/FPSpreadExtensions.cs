using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarPoint.Win.Spread;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// FPSpreadクラスの拡張メソッドを定義します。
    /// </summary>
    public static class FPSpreadExtensions
    {
        /// <summary>
        /// シートのデータをCSVファイルに出力します。
        /// ファイルのパスはファイル選択のダイアログを表示します。
        /// ヘッダは列ヘッダのみ出力します。
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="defaultFileName">ダイアログ画面に表示するファイル名の既定値</param>
        public static void ExportToExcelSaveFileDialog(this FpSpread spread, string defaultFileName)
        {
            ExportToExcelSaveFileDialog(spread, defaultFileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        }

        /// <summary>
        /// シートのデータをCSVファイルに出力します。
        /// ファイルのパスはファイル選択のダイアログを表示します。
        /// ヘッダはヘッダ出力指定子に基づいて出力します。
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="defaultFileName">ダイアログ画面に表示するファイル名の既定値</param>
        /// <param name="includeHeader">ヘッダ出力指定子</param>
        public static void ExportToExcelSaveFileDialog(this FpSpread spread, string defaultFileName, FarPoint.Win.Spread.Model.IncludeHeaders includeHeader)
        {
            //選択があれば「名前を付けて保存ダイアログを表示」
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                //選択中のテーブル + .csvを既定のファイル名に
                sfd.FileName = defaultFileName + ".xls";
                //はじめに表示されるフォルダを指定する
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                //[ファイルの種類]に表示される選択肢を指定する
                sfd.Filter =
                    "xlsファイル(*.xls)|*.xls|すべてのファイル(*.*)|*.*";
                //[ファイルの種類]の選択をテキストに
                sfd.FilterIndex = 0;
                //タイトルを設定する
                sfd.Title = "保存先のファイルを選択してください";
                //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                sfd.RestoreDirectory = true;

                //ダイアログを表示する
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(spread, sfd.FileName, includeHeader);
                }
            }
        }


        /// <summary>
        /// シートのデータをCSVファイルに出力します。
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="filePath"></param>
        /// <param name="includeHeader">ヘッダ出力指定子</param>
        public static void ExportToExcel(this FpSpread spread, string filePath, FarPoint.Win.Spread.Model.IncludeHeaders includeHeader)
        {
            try
            {
                spread.Sheets[0].Protect = false; //Excelの保護モードを解除するために Protect プロパティを変更する
               
                spread.SaveExcel(filePath, includeHeader);

                spread.Sheets[0].Protect = true; //上での変更を戻す

                //処理完了メッセージ
                MessageBox.Show(
                    "データ出力が完了しました。",
                    "出力",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (System.IO.IOException err)
            {
                //IOExceptionが発生したらメッセージを表示。そうでない場合はthrowする
                MessageBox.Show(
                    "CSVファイルの作成に失敗しました。" + "\r\n" + err.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }
    }
}
