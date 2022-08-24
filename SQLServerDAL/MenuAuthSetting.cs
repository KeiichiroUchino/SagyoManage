using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    public class MenuAuthSetting
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo _authInfo =
            new AppAuthInfo { OperatorId = 0, TerminalId = "", UserProcessId = "" };

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public MenuAuthSetting()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、備品棚卸更新処理の
        /// ビジネスロジックレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo"></param>
        public MenuAuthSetting(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }


        #region パブリックメソッド

        /// <summary>
        /// 権限IDを指定して、権限別メニュー設定情報の一覧を取得します。
        /// </summary>
        /// <param name="authId">権限ID</param>
        /// <returns>権限別メニュー設定情報の一覧</returns>
        public IList<MenuAuthSettingInfo> GetMenuAuthSettingList(decimal authId)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	AA.*, ");
            sb.AppendLine("	BB.AuthId ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	( ");
            sb.AppendLine(" SELECT ");
            sb.AppendLine("	A.* ");
            sb.AppendLine("	FROM ");
            sb.AppendLine("	MenuDefine AS A ");
            //メニュー定義情報[表示フラグ]=1(表示)のみ対象とします
            sb.AppendLine("WHERE ");
            sb.AppendLine("	A.ShowFlag = " +
                NSKUtil.BoolToInt(true).ToString() + " ");
            sb.AppendLine("AND A.Ident2 <> '' ");
            sb.AppendLine("	) AS AA ");
            sb.AppendLine("	LEFT OUTER JOIN ");
            sb.AppendLine("	( ");
            sb.AppendLine("	SELECT ");
            sb.AppendLine("	A.* ");
            sb.AppendLine("	FROM ");
            sb.AppendLine("	AuthIgnorMenu AS A ");
            sb.AppendLine("	WHERE ");
            //引数[権限ID]と同じものを対象とします
            sb.AppendLine("	A.AuthId = " +
                authId.ToString() + " ");
            sb.AppendLine("	) AS BB ");
            sb.AppendLine("	ON AA.MenuId = BB.MenuId ");
            //並び替え(大分類コード, 中分類コード)
            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	AA.TBunruiCode, AA.MBunruiCode ");

            string mySql = sb.ToString();

            Debug.Print("\nmySql = " + mySql + "\n");


            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //使用許可フラグ
                bool allowUseFlag = false;
                //メニュー使用不可情報テーブルの権限ID
                decimal ignoreAuthId = SQLServerUtil.dbDecimal(rdr["AuthId"]);
                //メニュー定義情報に対するメニュー使用不可情報がなければ使用許可フラグをTrueに
                if (ignoreAuthId == 0)
                {
                    allowUseFlag = true;
                }

                MenuAuthSettingInfo wk_info
                    = new MenuAuthSettingInfo()
                    {
                        AuthId = ignoreAuthId,
                        MenuId = SQLServerUtil.dbDecimal(rdr["MenuId"]),
                        TBunruiCode = SQLServerUtil.dbInt(rdr["TBunruiCode"]),
                        TBunruiName = rdr["TBunruiName"].ToString(),
                        MBunruiCode = SQLServerUtil.dbInt(rdr["MBunruiCode"]),
                        MBunruiName = rdr["MBunruiName"].ToString(),
                        AllowUseFlag = allowUseFlag,
                    };

                return wk_info;
            });

        }

        /// <summary>
        /// 権限別メニュー設定情報の一覧と権限IDを指定して
        /// メニュー使用不可情報を登録します。
        /// </summary>
        /// <param name="MenuAuthSettingList">権限別メニュー設定情報の一覧</param>
        /// <param name="authId">権限別メニュー設定情報の一覧</param>
        public void SetMenuAuthSettingList(IList<MenuAuthSettingInfo>
            menuAuthorizeList, decimal authId)
        {
            //実行するSQLのリスト
            List<string> mySqlSetList = new List<string>();

            //最初に検索条件で物理削除しておく
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE FROM ");
            sbDel.AppendLine(" AuthIgnorMenu ");
            sbDel.AppendLine("WHERE ");
            sbDel.AppendLine("  AuthId = " +
                authId.ToString() + " ");

            //更新用リストに追加
            mySqlSetList.Add(sbDel.ToString());

            //明細の存在チェック
            //--チェック用のSQLを作成
            foreach (MenuAuthSettingInfo item in menuAuthorizeList)
            {
                //使用許可フラグがfalseの場合はInsert文を作成
                if (!item.AllowUseFlag)
                {
                    StringBuilder sbIns = new StringBuilder();

                    sbIns.AppendLine("INSERT INTO ");
                    sbIns.AppendLine("AuthIgnorMenu ");
                    sbIns.AppendLine("(");
                    sbIns.AppendLine("AuthId, MenuId ");
                    sbIns.AppendLine(") ");
                    sbIns.AppendLine("VALUES ");
                    sbIns.AppendLine("( ");
                    sbIns.AppendLine(
                        item.AuthId.ToString() + ", ");
                    sbIns.AppendLine(
                        item.MenuId.ToString() + " ");
                    sbIns.AppendLine(") ");

                    mySqlSetList.Add(sbIns.ToString());
                }
            }



            //更新処理
            //コネクションを取得

            using (SqlConnection connSet = SQLHelper.GetSqlConnection())
            {
                connSet.Open();
                SqlTransaction transaction = connSet.BeginTransaction();

                try
                {
                    //SQL実行
                    foreach (string item in mySqlSetList)
                    {
                        SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(item));
                    }

                    //何もなければコミット
                    transaction.Commit();
                }
                catch (Exception err)
                {
                    //エラーが出たらロールバック
                    transaction.Rollback();
                    throw err;
                }
                finally
                {
                    //後片付け
                    connSet.Close();
                }
            }

        }

        #endregion
    }
}
