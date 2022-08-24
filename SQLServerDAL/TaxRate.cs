using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.ComLib;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 消費税率テーブルのデータアクセスレイヤです。
    /// </summary>
    public class TaxRate
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo _authInfo =
            new AppAuthInfo
            {
                OperatorId = 0,
                TerminalId = "",
                UserProcessId = "",
                UserProcessName = ""
            };

        /// <summary>
        /// テーブル名
        /// </summary>
        private string _tableName = "消費税";

        /// <summary>
        /// 消費税クラスのデフォルトコンストラクタです。
        /// </summary>
        public TaxRate()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、消費税テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public TaxRate(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド
        /// <summary>
        /// SqlTransaction情報、消費税情報を指定して、
        /// 消費税情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">消費税情報</param>
        public void SaveList(SqlTransaction transaction, IList<TaxRateInfo> list)
        {
            try
            {
                List<string> mySqlSetList = new List<string>();

                //消費税
                mySqlSetList.AddRange(
                    this.CreateSqlCommand_TaxHistory(transaction, list));

                //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                string query = SQLHelper.SQLQueryJoin(mySqlSetList);

                System.Diagnostics.Debug.WriteLine(
                    string.Format("SQL:{0}", query));

                SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
            }
            catch (SqlException err)
            {
                if (err.Number == Constants.SQLErrors.UniqueConstraintViolation)
                {
                    //リトライ可能な例外
                    throw new
                        Model.DALExceptions.CanRetryException(
                        string.Format("{0}情報の登録時にキー違反が発生したため、保存できませんでした。"
                            , this._tableName),
                        MessageBoxIcon.Warning);
                }
                else
                {
                    //ハンドルしない場合は再度スローする
                    throw;
                }
            }            
        }

        /// <summary>
        /// 対象日付を指定して、消費税率を取得します。
        /// </summary>
        /// <param name="date">対象日付</param>
        /// <returns>消費税率</returns>
        public decimal GetTaxRate(DateTime date)
        {            
            TaxRateInfo info =
                this.GetListInternal(null)
                .OrderByDescending(ob => ob.StartDate)
                .FirstOrDefault(element => element.StartDate <= date);

            if (info == null)
            {
                return 0;
            }

            return info.TaxRate;
        }

        /// <summary>
        /// 検索条件情報を指定して、消費税情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>消費税情報のリスト</returns>
        public IList<TaxRateInfo> GetList()
        {
            return GetListInternal(null);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<TaxRateInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<TaxRateInfo> rt_list = new List<TaxRateInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	TaxHistory.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	TaxHistory ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" TaxHistory.StartDate ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                TaxRateInfo rt_info = new TaxRateInfo
                {
                    StartDate = SQLHelper.dbDate(rdr["StartDate"]),
                    TaxRate = SQLServerUtil.dbDecimal(rdr["TaxRate"]),
                };


                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、消費税情報を指定して、
        /// 消費税情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">消費税情報</param>
        private IList<string> CreateSqlCommand_TaxHistory(SqlTransaction transaction, IList<TaxRateInfo> list)
        {
            //戻り値用
            List<string> rt_list = new List<string>();

            //DELETE文を作成
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE  ");
            sbDel.AppendLine(" TaxHistory ");
            //戻り値のリストに追加
            rt_list.Add(sbDel.ToString());

            foreach (TaxRateInfo item in list)
            {
                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" TaxHistory ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	StartDate ");
                sb.AppendLine("	,TaxRate ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + SQLHelper.DateTimeToDbDateTime(item.StartDate) + "");
                sb.AppendLine("," + item.TaxRate.ToString() + " ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
                sb.AppendLine(") ");

                //戻り値のリストに追加
                rt_list.Add(sb.ToString());
            }
            return rt_list;
        }

        #endregion
    }
}
