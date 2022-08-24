using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    public class MenuItem
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo _authInfo =
            new AppAuthInfo { OperatorId = 0, TerminalId = "", UserProcessId = "" };

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public MenuItem()
        { }

        /// <summary>
        /// アプリケーション認証情報を指定して、メニュー情報の
        /// ビジネスロジックレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public MenuItem(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// メニュー項目を取得します。
        /// </summary>
        /// <returns>メニュー項目の一覧</returns>
        public IList<MenuItemInfo> GetMenuItemList()
        {
            //SQL文の作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("A.* ");
            sb.Append("FROM ");
            sb.Append("MenuDefine AS A ");
            sb.Append("WHERE ");
            sb.Append("A.ShowFlag <> 0 ");
            sb.Append("ORDER BY ");
            sb.Append("A.TBunruiCode,A.MBunruiCode ");

            string mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                MenuItemInfo wk_info =
                    new MenuItemInfo()
                    {
                        MenuId = SQLServerUtil.dbDecimal(rdr["MenuId"]),
                        TopMenuCode = SQLServerUtil.dbInt(rdr["TBunruiCode"]),
                        TopMenuName = rdr["TBunruiName"].ToString().Trim(),
                        MiddleMenuCode = SQLServerUtil.dbInt(rdr["MBunruiCode"]),
                        MiddleMenuName = rdr["MBunruiName"].ToString().Trim(),
                        PartOfNameSpace = rdr["Ident1"].ToString().Trim(),
                        ClassName = rdr["Ident2"].ToString().Trim(),
                        SortWeight = SQLServerUtil.dbInt(rdr["SortWeight"]),
                        ShowFlag =
                            NSKUtil.IntToBool(
                                SQLServerUtil.dbInt(rdr["ShowFlag"])),
                        MenuDescription = rdr["Description"].ToString().Trim()
                    };

                //Ident2列の値でメニューのEnableFlagに値を設定する
                //0の場合はEnableFlag=falseとする。
                //それ以外の場合はEnableFlag=true
                wk_info.EnableFlag = true;
                if (rdr["Ident3"].ToString().Trim() == "0")
                {
                    wk_info.EnableFlag = false;
                }

                //Ident3列の値でメニューから表示するFormをDialog表示で
                //行うかどうかの値を設定する
                wk_info.ShowingMode =
                    Convert.ToInt32(
                        rdr["Ident4"].ToString());

                return wk_info;
            });
        }

        /// <summary>
        /// メニュー定義 MenuIdの最大値を取得し
        /// インクリメントして返します
        /// </summary>
        /// <return>MenuId 最大値+1</return>
        public decimal GetNextMenuId()
        {
            //SQL文の作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(" Max(MenuId) AS MenuId ");
            sb.Append("FROM ");
            sb.Append(" MenuDefine AS A ");

            string mySql = sb.ToString();

            decimal menuId = SQLHelper.SimpleReadSingle(mySql, rdr => Convert.ToDecimal(rdr["MenuId"].ToString()));
            return menuId + 1;
        }

        /// <summary>
        /// 指定されたフォームがメニュー定義に存在するか確認する
        /// </summary>
        /// <param name="formName">存在確認するフォーム名</param>
        /// <return>true：存在する、false：存在しない</return>
        public bool ExistMenuDefine(string formName)
        {
            //SQL文の作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(" Ident2 ");
            sb.Append("FROM ");
            sb.Append(" MenuDefine AS A ");
            sb.Append("WHERE ");
            sb.Append(" Ident2 = '" + formName + "'");

            return SQLHelper.SimpleReadHasRows(sb.ToString());
        }

        /// <summary>
        /// メニュー定義を追加します
        /// </summary>
        /// <param name="info">メニュー定義情報</param>
        /// <return>true：追加成功、false：追加失敗</return>
        public bool SetMenuDefine(MenuItemInfo info)
        {
            bool rt_val = false;

            //更新SQLセット用
            List<string> mySqlSetList = new List<string>();

            //存在チェックSQLを作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" A.MenuId ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" MenuDefine AS A ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" A.MenuId = " +
                info.MenuId.ToString() + " ");
            string mySql = sb.ToString();


            //存在チェック
            if (SQLHelper.SimpleReadHasRows(mySql))
            {
                //存在しようがしまいがInsert句を作成

                //SQL文の作成
                StringBuilder sbIns = new StringBuilder();
                sbIns.Append("INSERT INTO ");
                sbIns.Append("MenuDefine ");
                sbIns.Append("( ");
                sbIns.Append(" MenuId ");
                sbIns.Append(",TBunruiCode ");
                sbIns.Append(",TBunruiName ");
                sbIns.Append(",MBunruiCode ");
                sbIns.Append(",MBunruiName ");
                sbIns.Append(",Ident1 ");
                sbIns.Append(",Ident2 ");
                sbIns.Append(",Ident3 ");
                sbIns.Append(",Ident4 ");
                sbIns.Append(",ShowFlag ");
                sbIns.Append(",Description) ");
                sbIns.Append("VALUES ");
                sbIns.Append("( ");
                sbIns.Append(
                    info.MenuId.ToString() + ", ");
                sbIns.Append(
                    info.TopMenuCode.ToString() + ", ");
                sbIns.Append("'" +
                    info.TopMenuName + "', ");
                sbIns.Append(
                    info.MiddleMenuCode.ToString() + ", ");
                sbIns.Append("'" +
                    info.MiddleMenuName + "', ");
                sbIns.Append("'" +
                    info.PartOfNameSpace + "', ");
                sbIns.Append("'" +
                    info.ClassName + "', ");
                sbIns.Append(
                    NSKUtil.BoolToInt(
                        info.EnableFlag).ToString() + ", ");
                sbIns.Append(
                    info.ShowingMode.ToString() + ", ");
                //sbIns.Append(
                //    info.SortWeight.ToString() + ", ");
                sbIns.Append(
                    NSKUtil.BoolToInt(
                        info.ShowFlag).ToString() + ", ");
                sbIns.Append("'" +
                    info.MenuDescription + "' ");
                sbIns.Append(") ");

                //リストに追加
                mySqlSetList.Add(sbIns.ToString());

                //更新処理
                //コネクションを取得
                SqlConnection connSet = SQLHelper.GetSqlConnection();
                connSet.Open();
                SqlTransaction transaction = connSet.BeginTransaction();

                try
                {
                    //SQL実行
                    foreach (string item in mySqlSetList)
                    {
                        SqlCommand cmd = new SqlCommand(item, connSet, transaction);
                        cmd.ExecuteNonQuery();
                    }

                    //何もなければコミット
                    transaction.Commit();

                    rt_val = true;

                }
                catch (Exception)
                {
                    //エラーが出たらロールバック
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    //後片付け
                    connSet.Close();
                }
            }

            return rt_val;
        }

        #endregion
    }
}
