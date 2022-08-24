using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using GrapeCity.Win.Editors;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Frame;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.SQLServerDAL;

namespace Jpsys.SagyoManage.Frame
{
    public partial class CmnSearchSystemNameFrame : Form,IFrameBase
    {
        #region ユーザー定義

        /// <summary>
        /// 画面タイトルの静的な部分
        /// </summary>
        private const string WINDOW_TITLE_PART = "検索";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo _appAuth;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 検索テーブルのクラス領域
        /// </summary>
        private SystemName _SearchTblClass;

        #region Spread_リスト関係

        /// <summary>
        /// ID列番号
        /// </summary>
        private const int COL_ID = 0;
        /// <summary>
        /// 名称列番号
        /// </summary>
        private const int COL_NAME = 1;
        /// <summary>
        /// 検索一覧最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LIST = 2;

        #endregion

        /// <summary>
        /// 検索一覧のデザイナの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialStyleInfoArr;

        /// <summary>
        /// システム名称区分の列挙子
        /// </summary>
        private DefaultProperty.SystemNameKbn systemNameKbn;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> _SystemNameList = null;

        #endregion

        #region コンストラク

        /// <summary>
        ///　本クラスのデフォルトコンストラクタです。
        ///　(通常の呼び出し時には使用しないでください)
        /// </summary>
        public CmnSearchSystemNameFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// システム名称の区分を指定して、本クラスの
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="nameKbn">システム名称の区分</param>
        public CmnSearchSystemNameFrame(DefaultProperty.SystemNameKbn nameKbn)
        {
            InitializeComponent();

            this.systemNameKbn = nameKbn;
        }

        #endregion

        #region 初期化処理

        private void InitCmnSearchSystemNameFrame()
        {
            //画面タイトルの設定
            this.Text =
                FrameUtilites.GetSystemNameKbnString(this.systemNameKbn) +
                    WINDOW_TITLE_PART;

            //親フォームがあるときは、そのフォームを中心に表示する
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //認証アプリケーション情報を設定
            this._appAuth = new AppAuthInfo();
            this._appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this._appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this._appAuth.UserProcessId = this.Name;
            this._appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this._appAuth);

            //検索テーブルのクラスインスタンス作成
            this._SearchTblClass = new SystemName();

            //システム名称一覧を取得
            this._SystemNameList = this._SearchTblClass.GetList((int)systemNameKbn);

            //Style情報を格納するメンバ変数の初期化
            this.InitStyleInfo();

            //Spread関連の初期化
            this.InitSheet();

            //システム名称リストのセット
            this.SetListSheet();

            this.fpListGrid.Focus();
        }

        /// <summary>
        /// SpreadのStyle情報の格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_LIST];

            for (int i = 0; i < COL_MAXCOLNUM_LIST; i++)
            {
                this.initialStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //リストの初期化
            this.InitListSheet();
        }

        /// <summary>
        /// リストの初期化します。
        /// </summary>
        private void InitListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_LIST);
            DefaultSheetStyleModel styleModel =
                new DefaultSheetStyleModel(0, COL_MAXCOLNUM_LIST);

            sheet0.Models.Data = dataModel;
            sheet0.Models.Style = styleModel;

        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitCmnSearchSystemNameFrame();
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインターフェース経由で取得・設定します。
        /// </summary>
        public string FrameName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインターフェース経由で取得・設定します。
        /// </summary>
        public string FrameText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 検索一覧に値を設定します。
        /// 検索条件が指定されている場合は、該当するデータのみを設定します。
        /// </summary>
        private void SetListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索一覧を初期化
                this.InitListSheet();

                List<SystemNameInfo> wk_list = new List<SystemNameInfo>();

                wk_list = this._SystemNameList.ToList();

                //件数取得
                int rowcount = wk_list.Count;
                if (rowcount == 0)
                {
                    //OKボタンを無効にして処理から抜ける
                    this.btnOk.Enabled = false;
                    return;
                }

                //Spreadのデータモデルの作成
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_LIST);


                //ループしてモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    SystemNameInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    //データモデルのセット
                    datamodel.SetValue(j, COL_ID, wk_info.SystemNameCode);
                    datamodel.SetValue(j, COL_NAME, wk_info.SystemName);

                    //タグ
                    datamodel.SetTag(j, COL_ID, wk_info);
                }

                //Spreadにセット
                this.fpListGrid.Sheets[0].Models.Data = datamodel;
                this.fpListGrid.Sheets[0].Models.Style = stylemodel;

                //Spread行の背景を交互に設定
                //this.fpListGrid.Sheets[0].AlternatingRows.Get(1).BackColor =
                //    System.Drawing.Color.FromArgb(
                //    ((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(225)))));

                //検索結果の1行目をフォーカスを合わせて選択状態にする
                this.fpListGrid.Sheets[0].SetActiveCell(0, 0, true);
                this.fpListGrid.Sheets[0].AddSelection(
                    0, -1, 1, this.fpListGrid.Sheets[0].ColumnCount - 1);

                this.fpListGrid.Focus();

                //明細がない場合はOKボタンをロック
                if (this.fpListGrid.Sheets[0].RowCount == 0)
                {
                    btnOk.Enabled = false;
                }
                else
                {
                    btnOk.Enabled = true;
                }

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //デフォルトに戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 選択している明細の検索テーブル情報をメンバにセットします。
        /// </summary>
        private void SetSelectedInfo()
        {
            //選択中の検索テーブル情報を取得
            List<SystemNameInfo> wk_val = this.GetInfoByListOnSelection();

            //選択していない場合は警告を表示する
            if (wk_val.Count == 0)
            {
                MessageBox.Show(FrameUtilites.GetDefineMessage("MW2203009"), "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.fpListGrid.Focus();
            }
            else
            {
                //プロパティに格納
                this.SelectedInfo = wk_val[0];

                //OKで画面を閉じる
                this.DoClose(DialogResult.OK);
            }
        }

        /// <summary>
        /// 検索一覧にて選択中の検索テーブル情報を取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns></returns>
        private List<SystemNameInfo> GetInfoByListOnSelection()
        {
            //返却値
            List<SystemNameInfo> rt_list = new List<SystemNameInfo>();

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行をループで回してコードを取得
                foreach (CellRange item in sheet0.GetSelections())
                {
                    //ブロックの中のセルを取り出してコードをリストに追加
                    for (int i = 0; i < item.RowCount; i++)
                    {
                        SystemNameInfo info =
                            ((SystemNameInfo)sheet0.Cells[item.Row + i, COL_ID].Tag);

                        //Modelの返却用
                        rt_list.Add(info);
                    }
                }
            }
            return rt_list;
        }

        /// <summary>
        /// DialogResultを指定して画面を閉じます。
        /// </summary>
        /// <param name="dialogResult"></param>
        private void DoClose(DialogResult dialogResult)
        {
            this.DialogResult = dialogResult;
        }

        /// <summary>
        /// 結果一覧上でのPreviewKeyDownイベントを処理します。
        /// </summary>
        private void ProcessFpListGirdPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.SetSelectedInfo();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 選択された検索テーブル情報を取得します。
        /// </summary>
        public SystemNameInfo SelectedInfo { get; private set; }

        #endregion

        /// <summary>
        /// OS規程の操作によりフォームを閉じる操作をした場合に、フォームの
        /// 自動入力検証(AutoValidate)処理を行わないようにするために、Windowsメッセージ
        /// の処理メソッドをオーバーライドして、該当する操作時に自動入力検証を
        /// 無効にします。
        /// 無効後はいずれかの処理で必ず自動入力検証をONにしてください。
        /// </summary>
        /// <param name="m">処理対象のWindows Message</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_CLOSE = 0x10;
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xf060;

            switch (m.Msg)
            {
                case WM_SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC_CLOSE)
                    {
                        //Xボタン、コントロールメニューの「閉じる」、 
                        //コントロールボックスのダブルクリック、 
                        //Atl+F4などにより閉じられようとしている 
                        //このときValidatingイベントを発生させない。 
                        this.AutoValidate = AutoValidate.Disable;
                    }


                    break;
                case WM_CLOSE:
                    //Application.Exit以外で閉じられようとしている 
                    //このときValidatingイベントを発生させない。 
                    this.AutoValidate = AutoValidate.Disable;
                    break;
            }

            base.WndProc(ref m);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //OKボタンをクリック
            this.SetSelectedInfo();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //キャンセルボタンクリック
            this.DoClose(DialogResult.Cancel);
        }

        private void fpListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //結果一覧をダブルクリック
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                this.SetSelectedInfo();
            }
        }

        private void fpListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            //結果一覧 - CellClick
            if (e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダ上の場合はその列の自動ソートを実行
                this.fpListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //結果一覧 - PreviewKeyDown
            this.ProcessFpListGirdPreviewKeyDown(e);
        }

    }
}
