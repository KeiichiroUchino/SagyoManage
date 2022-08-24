using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Configuration;
using Microsoft.VisualBasic;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    public static class SQLHelper
    {
        /// <summary>
        /// 常設列の入力・登録・更新の各種別を定義する列挙体です。
        /// </summary>
        [Flags]
        public enum PopulateColumnOptions
        {
            /// <summary>
            /// 入力関連列を表します。
            /// </summary>
            EntryColumns = 0x01,
            /// <summary>
            /// 登録関連列を表します。
            /// </summary>
            AdditionColumns = 0x02,
            /// <summary>
            /// 更新関連列を表します。
            /// </summary>
            UpdateColumns = 0x04
        }

        /// <summary>
        /// 伝票番号管理マスタの区分番号の種別を定義する列挙体です。
        /// </summary>
        public enum SequenceIdKind : int
        {
            //-- マスタ系
            /// <summary>
            /// 1001:オペレータマスタ
            /// </summary>
            OperatorMst = 1001,
            /// <summary>
            /// 1002:得意先
            /// </summary>
            Tokuisaki = 1002,
            /// <summary>
            /// 1003:作業場所
            /// </summary>
            SagyoBasho = 1003,
            /// <summary>
            /// 1004:契約
            /// </summary>
            Keiyaku = 1004,
            /// <summary>
            /// 1005:社員
            /// </summary>
            Staff = 1005,
            /// <summary>
            /// 1006:名称
            /// </summary>
            Meisho = 1006,
            /// <summary>
            /// 1007:車両
            /// </summary>
            Car = 1007,
            /// <summary>
            /// 1008:作業大分類
            /// </summary>
            SagyoDaiBunrui = 1008,
            /// <summary>
            /// 1009:作業中分類
            /// </summary>
            SagyoChuBunrui = 1009,
            /// <summary>
            /// 1010:作業小分類
            /// </summary>
            SagyoShoBunrui = 1010,

            //トラン系
            /// <summary>
            /// 2001:作業案件
            /// </summary>
            SagyoAnken = 2001,
            /// <summary>
            /// 2002:作業員割り当て
            /// </summary>
            SagyoinWariate = 2002,
            /// <summary>
            /// 2003:車両割り当て
            /// </summary>
            CarWariate = 2003,

            //トラン系コード
            /// <summary>
            /// 3001:作業案件
            /// </summary>
            SagyoAnkenCode = 3001,
            /// <summary>
            /// 3002:作業員割り当て
            /// </summary>
            SagyoinWariateCode = 3002,
            /// <summary>
            /// 3003:車両割り当て
            /// </summary>
            CarWariateCode = 3003,

            //システム管理用

            /// <summary>
            /// 9001:操作履歴ID
            /// </summary>
            OpereteHistoryId = 9001,

        }

        //一意キー取得メソッドをロックするためのロックオブジェクト
        private static object sequenceIdLockObject = new object();

        ///// <summary>
        ///// bizCommonクラスのインスタンスを保持します。
        ///// </summary>
        //private static BizCommon bizCommonDAL = new BizCommon();

        //修正 20190204 T.Mawatari Start //
        //public const string DEC_KEY = "DecryptKeyHaishaManageNsk5930?";
        public static string DEC_KEY = "DecryptKeyHaishaManage"
            + ConfigurationManager.AppSettings["EditionName"]
            + "Nsk5930?";
        //修正 20190204 T.Mawatari End //

        #region SQLServer接続文字列暗号化関連の変数値

        /// <summary>
        /// 復号化されたSQLServer接続文字列
        /// </summary>
        private static string decriptedSQLServerName = string.Empty;
        /// <summary>
        /// 復号化されたデータベース名
        /// </summary>
        private static string decriptedSQLDBName = string.Empty;
        /// <summary>
        /// 復号化された接続ID
        /// </summary>
        private static string decriptedSQLUserId = string.Empty;
        /// <summary>
        /// 復号化された接続IDに対するパスワード
        /// </summary>
        private static string decriptedSQLUserPass = string.Empty;
        /// <summary>
        /// 復号化されたSQLタイムアウト値（実際は暗号化されていないが他の変数との整合のため）
        /// </summary>
        private static string decriptedSQLTimeOut = string.Empty;
        /// <summary>
        /// 復号化された信頼関係接続の有無（デフォルトはFalse）
        /// </summary>
        private static bool decriptedSQLTrusty = false;
        /// <summary>
        /// 復号化されたSQLServerへの接続設定が格納されたかどうかのフラグ
        /// </summary>
        private static bool storedDecripteSQLSetting = false;

        #endregion

        /// <summary>
        /// アプリケーション構成ファイルの定義より、SQLServerConnectionの
        /// クラスをインスタンス化して取得します。
        /// </summary>
        /// <returns>構成ファイルの定義から作成したSQLServerとの接続を格納したオブジェクト</returns>
        public static SQLServerConnection GetSqlServerConn()
        {
            SQLServerConnection conn = null;

            //接続文字列が暗号化されているか確認
            //　EncryptedConnectionSettingキーの値を確認。キーがない場合は暗号化されていないものと判断する。
            var encflgstring = ConfigurationManager.AppSettings["EncryptedConnectionSetting"] ?? false.ToString();
            var encflg = Convert.ToBoolean(encflgstring);

            if (encflg)
            {
                //接続文字列は暗号化されている
                conn = SQLHelper.GetSqlServerConnEncripted();
            }
            else
            {
                //暗号化されていない

                //設定ファイルから取得
                string sqlsv =
                    ConfigurationManager.AppSettings["SQLConnServerName"];
                string sqldb =
                    ConfigurationManager.AppSettings["SQLConnDbName"];
                string sqluser =
                    ConfigurationManager.AppSettings["SQLConnUserid"];
                string sqlpass =
                    ConfigurationManager.AppSettings["SQLConnPassword"];
                string sqltimeout =
                    ConfigurationManager.AppSettings["SQLConnTimeout"];
                bool sqltrusty =
                    Convert.ToBoolean(
                    ConfigurationManager.AppSettings["SQLConnTrusty"]);

                conn =
                    new SQLServerConnection(
                    sqlsv, sqldb, sqluser, sqlpass, sqltimeout, sqltrusty);
            }

            return conn;
        }

        /// <summary>
        /// アプリケーション構成ファイルの定義より、SQLServerConnectionの
        /// クラスをインスタンス化して取得します。アプリケーション構成ファイル上の
        /// 設定値が暗号化されている場合に使用します。
        /// </summary>
        /// <returns>構成ファイルの暗号化された定義から取得したSQLServerとの接続を格納したオブジェクト</returns>
        public static SQLServerConnection GetSqlServerConnEncripted()
        {
            //変更 接続文字列が暗号化されているとSQL文の連続実行時に遅くなるので対策 T.kuroki 20150119

            //復号化されたSQLSererへの接続設定が格納されていないときは・・・
            if (!storedDecripteSQLSetting)
            {

                //必要な値が構成ファイルにあるかをチェック

                //いったん構成ファイルの設定値を取得しておく
                string enc_sqlsv =
                    ConfigurationManager.AppSettings["SQLConnServerName"];
                string enc_sqldb =
                    ConfigurationManager.AppSettings["SQLConnDbName"];
                string enc_sqluser =
                    ConfigurationManager.AppSettings["SQLConnUserid"];
                string enc_sqlpass =
                    ConfigurationManager.AppSettings["SQLConnPassword"];
                //タイムアウト値は暗号化しない
                //string enc_sqltimeout =
                //    ConfigurationManager.AppSettings["SQLConnTimeout"];
                string enc_sqltrusty =
                    ConfigurationManager.AppSettings["SQLConnTrusty"];

                //復号化処理
                //--SQLServer名
                decriptedSQLServerName = NSKUtil.DecryptString(enc_sqlsv, DEC_KEY);
                //--データベース名
                decriptedSQLDBName = NSKUtil.DecryptString(enc_sqldb, DEC_KEY);
                //--User名称
                decriptedSQLUserId = NSKUtil.DecryptString(enc_sqluser, DEC_KEY);
                //--Password名称
                decriptedSQLUserPass = NSKUtil.DecryptString(enc_sqlpass, DEC_KEY);
                //--タイムアウト値（暗号化されていないのでAppSettingsから直接取得）
                decriptedSQLTimeOut = ConfigurationManager.AppSettings["SQLConnTimeout"];
                //--信頼関係接続の有無
                decriptedSQLTrusty = Convert.ToBoolean(NSKUtil.DecryptString(enc_sqltrusty, DEC_KEY));

                //SQLServerConnectionクラスが、引数なしで実行された時の対策で、NSKUtilDBSettingsにも
                //接続文字列を設定しておく
                NSKUtilDBSettings.SqlConnServerName = decriptedSQLServerName;
                NSKUtilDBSettings.SqlConnDBName = decriptedSQLDBName;
                NSKUtilDBSettings.SqlConnUserId = decriptedSQLUserId;
                NSKUtilDBSettings.SqlConnPassword = decriptedSQLUserPass;
                NSKUtilDBSettings.SqlConnTimeout = decriptedSQLTimeOut;
                NSKUtilDBSettings.SqlConnTrusty = decriptedSQLTrusty.ToString();

                //復号化されたSQLServerへの接続設定が格納されたか判断するフラグをONにしておく
                storedDecripteSQLSetting = true;
            }

            //タイムアウト値は暗号化されていないので普通に取得
            //var sqltimeout = ConfigurationManager.AppSettings["SQLConnTimeout"];

            //追加　接続文字列を都度都度作らないようにするために追加　T.Kuroki 20150119
            SQLServerConnection conn =
                new SQLServerConnection(
                    decriptedSQLServerName, decriptedSQLDBName, decriptedSQLUserId, decriptedSQLUserPass,
                    decriptedSQLTimeOut, decriptedSQLTrusty);

            return conn;

        }

        /// <summary>
        /// アプリケーション構成ファイルの定義より、SQLConnectionの
        /// クラスをインスタンス化して取得します。
        /// </summary>
        /// <returns>構成ファイルの定義から作成したSQLServerとの接続を格納したオブジェクト</returns>
        public static SqlConnection GetSqlConnection()
        {
            SQLServerConnection sqlServerConn = GetSqlServerConn();

            string connectionString =
                string.Format(@"Server={0};Database={1};User ID={2};Password={3};Trusted_Connection={4};Connection Timeout={5};",
                sqlServerConn.ServerName,
                sqlServerConn.DataBaseName,
                sqlServerConn.UserId,
                sqlServerConn.Password,
                sqlServerConn.IntegratedSecurity.ToString(),
                sqlServerConn.Timeout);

            SqlConnection connection = new SqlConnection(connectionString);

            return connection;
        }

        /// <summary>
        /// 接続が開かれたコネクションオブジェクトを使用して、コマンドのExecuteReaderを実行します。
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReaderOnConnection(SqlConnection conn, SqlCommand sqlCommand)
        {
            sqlCommand.Connection = conn;
            return sqlCommand.ExecuteReader();
        }

        /// <summary>
        /// SqlCommandを、指定したSQLTransaction上でExecuteReaderを実行します。
        /// Connectionは指定したSQLTransactionに関連するConnectionを使用します。
        /// </summary>
        /// <param name="transaction">SQLTransaction</param>
        /// <param name="command">SqlCommand</param>
        /// <returns>ExecuteReaderの結果を表すSqlDataReader</returns>
        public static SqlDataReader ExecuteReaderOnTransaction(SqlTransaction transaction, SqlCommand command)
        {
            command.Connection = transaction.Connection;
            command.Transaction = transaction;
            return command.ExecuteReader();
        }

        /// <summary>
        /// SqlCommandを、指定したSqlTransaction上でExecuteNonQueryを実行します。
        /// </summary>
        /// <param name="transaction">SqlTransaction</param>
        /// <param name="command">SqlCommand</param>
        /// <returns>影響を受けた行数</returns>
        public static int ExecuteNonQueryOnTransaction(SqlTransaction transaction, SqlCommand command)
        {
            command.Connection = transaction.Connection;
            command.Transaction = transaction;
            command.CommandTimeout = 300;
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新時に他から更新されている場合の例外
        /// 指定したトランザクション上でExecuteNonqueryを実行します。
        /// 影響を受けた行数が0の場合は、楽観的排他エラーの例外を起こします。 
        /// 影響を受けた行数が1以上場合は、致命的なエラーの例外を起こします。 
        /// </summary>
        /// <returns></returns>
        public static int ExecuteNonQueryForSingleRecordUpdate(SqlTransaction transaction, SqlCommand sqlCommand)
        {
            return ExecuteNonQueryForSingleRecordUpdate(transaction, sqlCommand, string.Empty);
        }

        /// <summary>
        /// SqlTransaction、SqlCommand、テーブル名を指定して、
        /// 更新時に他から更新されている場合の例外
        /// 指定したトランザクション上でExecuteNonqueryを実行します。
        /// 影響を受けた行数が0の場合は、楽観的排他エラーの例外を起こします。 
        /// 影響を受けた行数が1以上場合は、致命的なエラーの例外を起こします。
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="sqlCommand"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryForSingleRecordUpdate(SqlTransaction transaction, SqlCommand sqlCommand, string tableName)
        {
            int result = 0;

            result = SQLHelper.ExecuteNonQueryOnTransaction(transaction, sqlCommand);

            if (result == 0)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("この{0}データは既に他の利用者に更新されている為、更新できません。" +
                    "\r\n" + "最新情報を確認してください。", tableName)
                    , MessageBoxIcon.Warning);
            }
            else if (result > 1)
            {
                //アプリケーションの終了が要求される例外
                throw new
                    Model.DALExceptions.ApplicationFailureException();
            }
            return result;
        }

        /// <summary>
        /// 削除時に他から削除されている場合の例外
        /// 指定したトランザクション上でExecuteNonqueryを実行します。
        /// 影響を受けた行数が0の場合は、楽観的排他エラーの例外を起こします。 
        /// 影響を受けた行数が1以上場合は、致命的なエラーの例外を起こします。 
        /// </summary>
        /// <returns></returns>
        public static int ExecuteNonQueryForSingleRecordDelete(SqlTransaction transaction, SqlCommand sqlCommand)
        {
            return ExecuteNonQueryForSingleRecordDelete(transaction, sqlCommand, string.Empty);
        }

        /// <summary>
        /// SqlTransaction、SqlCommand、テーブル名を指定して、
        /// 削除時に他から削除されている場合の例外
        /// 指定したトランザクション上でExecuteNonqueryを実行します。
        /// 影響を受けた行数が0の場合は、楽観的排他エラーの例外を起こします。 
        /// 影響を受けた行数が1以上場合は、致命的なエラーの例外を起こします。
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="sqlCommand"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryForSingleRecordDelete(SqlTransaction transaction, SqlCommand sqlCommand, string tableName)
        {
            int result = 0;

            result = SQLHelper.ExecuteNonQueryOnTransaction(transaction, sqlCommand);

            if (result == 0)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("この{0}データは既に他の利用者に更新されている為、削除できません。" +
                    "\r\n" + "最新情報を確認してください。", tableName)
                    , MessageBoxIcon.Warning);
            }
            else if (result > 1)
            {
                //アプリケーションの終了が要求される例外
                throw new
                    Model.DALExceptions.ApplicationFailureException();
            }
            return result;
        }

        public static void ThrowCanNotDeleteException(string tableName, string code = null)
        {
            string key = string.Empty;

            if (code != null)
                key = code + "\n";

            //リトライ可能な例外
            throw new
                Model.DALExceptions.CanRetryException(
               string.Format("該当データは他のデータから使用されているため削除できません。\n" +
                     key + "\n" +
                     "【使用しているデータ】\n" +
                        tableName)
                    , MessageBoxIcon.Warning);
        }

        public static void UniqueConstraintViolationException(SqlException err)
        {

            //リトライ可能な例外
            throw new
                Model.DALExceptions.CanRetryException(
                "入力されたコードは既に存在する為、新規登録できません。",
                MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static decimal GetSequenceId(SQLHelper.SequenceIdKind seqKind)
        {
            return GetSequenceId((int)seqKind);
        }

        /// <summary>
        /// 伝票管理マスタの区分番号のInt値を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。原則直接呼出しは禁止です。列挙体を引数にとる
        /// メソッドから呼び出すようにしてください。
        /// </summary>
        /// <param name="seqKindId">伝票管理マスタの区分番号種別のInt値</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static decimal GetSequenceId(int seqKindId)
        {
            //複数取得を件数1件で呼び出して先頭を返す
            return GetSequenceIds(seqKindId, 1).First();
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static List<decimal> GetSequenceIds(SQLHelper.SequenceIdKind seqkind, int count)
        {
            return GetSequenceIds((int)seqkind, count);
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static List<decimal> GetSequenceIds(int seqKindId, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("取得件数は0より下を指定できません。", "count");
            }

            //0件の場合は空の要素を返す。
            if (count == 0)
            {
                return new List<decimal>();
            }

            //返却値
            List<decimal> rt_val = new List<decimal>();

            //先頭のId。この値から取得件数分を返す
            decimal startId;

            //マルチスレッドで実行されるのでロックしておく
            lock (sequenceIdLockObject)
            {
                //更新用SQL格納用
                string mySqlUpd = string.Empty;

                //後方処理で使用するためにいったん変数に格納
                int sqlKindInt = seqKindId;

                //存在チェックと現状の最大値取得
                //SQL作成
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                sb.Append("A.[Id] ");
                sb.Append("FROM ");
                sb.Append("IDManage AS A WITH(ROWLOCK, XLOCK, HOLDLOCK) ");
                sb.Append("WHERE ");
                sb.Append("A.[Kbn]=" + sqlKindInt.ToString());
                string mySql = sb.ToString();

                SqlConnection connUpd = SQLHelper.GetSqlConnection();
                connUpd.Open();
                SqlTransaction transaction = connUpd.BeginTransaction();
 
                try
                {
                    using (SqlDataReader rdr = SQLHelper.ExecuteReaderOnTransaction(transaction, new SqlCommand(mySql)))
                    {
                        if (rdr.Read())
                        {
                            //存在してたら、現在のIDを取得
                            startId = SQLServerUtil.dbDecimal(rdr["Id"]);

                            //1加算する
                            startId++;

                            //UpdateのSQLを作成
                            StringBuilder sbUpd = new StringBuilder();
                            sbUpd.Append("UPDATE ");
                            sbUpd.Append("IDManage ");
                            sbUpd.Append("SET ");
                            sbUpd.Append("[Id] = " + (startId + count - 1).ToString() + " ");
                            sbUpd.Append("WHERE ");
                            sbUpd.Append("[Kbn]=" + sqlKindInt.ToString());

                            mySqlUpd = sbUpd.ToString();

                        }
                        else
                        {
                            //存在していなかったら新規に作る

                            //新規なので1をセット
                            startId = 1;

                            //InsertのSQLを作成
                            StringBuilder sbIns = new StringBuilder();
                            sbIns.Append("INSERT INTO ");
                            sbIns.Append("IDManage ");
                            sbIns.Append("( ");
                            sbIns.Append("[Kbn],[Id]");
                            sbIns.Append(") ");
                            sbIns.Append("VALUES( ");
                            sbIns.Append(
                                sqlKindInt.ToString() + ",");
                            sbIns.Append(
                                count.ToString());
                            sbIns.Append(") ");

                            mySqlUpd = sbIns.ToString();
                        }

                    }
                    try
                    {
#if DEBUG
                        Console.WriteLine(mySqlUpd);
#endif
                            
                        SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(mySqlUpd));
                        //エラーがなければコミット
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        //エラー時はロールバック
                        transaction.Rollback();
                        throw;
                    }
                }
                finally
                {
                    connUpd.Close();
                }
            }

            //件数分をリストに追加する
            for (decimal i = 0; i < count; i++)
            {
                rt_val.Add(startId + i);
            }

            return rt_val;
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDをIdIteratorオブジェクトに格納して返します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>IDIteratorオブジェクト</returns>
        public static IdIterator GetSequenceIdsAsIterator(int seqkind, int count)
        {
            return new IdIterator(GetSequenceIds(seqkind, count));
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDをIdIteratorオブジェクトに格納して返します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>IDIteratorオブジェクト</returns>
        public static IdIterator GetSequenceIdsAsIterator(SQLHelper.SequenceIdKind seqkind, int count)
        {
            return GetSequenceIdsAsIterator((int)seqkind, count);
        }

        /// <summary>
        /// SQLで指定する文字列のためのサニタイズ処理をします。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sanitize(this string str)
        {
            return GetSanitaizingSqlString(str);
        }

        /// <summary>
        /// SQL文で指定する任意の文字列をサニタイズします。
        /// </summary>
        /// <param name="str">サニタイズする文字列</param>
        /// <returns>サニタイズされた文字列</returns>
        /// <remarks>SQLインジェクション対策の為に、SQL文に任意で指定する
        /// 文字列をサニタイズします。
        /// </remarks>
        public static string GetSanitaizingSqlString(string str)
        {
            string rt_val = string.Empty;

            //「'」を「''」に置き換える
            rt_val = str.Replace("'", "''");

            return rt_val;
        }

        /// <summary>
        /// 時刻を表す分数を格納しているデータベースのフィールド値を指定して、
        /// 指定された値を対応するTimeSpan値に変換して取得します。フィールドの型は
        /// decimalで整数４桁とします。
        /// フィールドの値がnullの場合には、TimeSpan.MinValueを返却します。
        /// </summary>
        /// <param name="val">時刻を表す分数を格納しているフィールド値</param>
        /// <returns>変換された値</returns>
        public static TimeSpan dbCustomDecimalToTimeSpan(object val)
        {
            TimeSpan rt_val = new TimeSpan();

            decimal dec_val = 0;
            int int_val = 0;

            //decimal値判断
            if (val is decimal)
            {
                //いったんdecimal用の格納領域にセット
                dec_val = (decimal)val;

                //桁数が超えている場合は例外を投げておく（基本的に4桁は超えないはずなので・・・）
                if (dec_val.ToString().Length > 4)
                {
                    //とりあえず、メッセージは手打ち T.Kuroki@NSK
                    throw new ApplicationException("時刻を格納しているフィールドの規定桁数を越えています。");
                }
                else
                {
                    //桁数に問題がなければ処理用にintに変換しておく
                    int_val = Convert.ToInt32(dec_val);
                }
            }
            else if (val is System.DBNull || val == null)
            {
                //nullの時には、返却値にMinValueをセット
                rt_val = TimeSpan.MinValue;
            }

            if (rt_val != TimeSpan.MinValue)
            {
                //MinValueじゃないときのみ、TimeSpanの値をnewする
                rt_val = new TimeSpan(0, int_val, 0);
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// TimeSpan構造体を指定して、時刻を表す分数を格納するデータベースの
        /// フィールドに対応する値に変換して文字列で取得します。
        /// TimeSpan構造体の値がTimeSpan.MinValueの場合はデータベース上で
        /// NULLを表す文字列に変換します。
        /// </summary>
        /// <param name="val">変換したいTimeSpan構造体</param>
        /// <returns>変換された値</returns>
        public static string TimeSpanTodbCustomDecimalString(TimeSpan val)
        {
            string rt_val = string.Empty;

            if (val == TimeSpan.MinValue)
            {
                //TimeSpanがMinValueの場合はNULL
                rt_val = "NULL";
            }
            else
            {
                //MinValueで無い場合は総分数を文字列に変換する
                rt_val = val.TotalMinutes.ToString();
            }

            return rt_val;
        }

        /// <summary>
        /// 既定の設定でトランザクションを開始して、指定したSQLTransactionをパラメータにとるデリゲートに引数として渡します。
        /// デリゲートの実行が完了した場合はトランザクションをコミットします。
        /// 例外が発生した場合はトランザクションをロールバックします。
        /// </summary>
        /// <param name="action"></param>
        public static void ActionWithTransaction(Action<SqlTransaction> action)
        {
            ActionWithTransaction(action, System.Data.IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 既定の設定でトランザクションを開始して、指定したSQLTransactionをパラメータにとるデリゲートに引数として渡します。
        /// デリゲートの実行が完了した場合はトランザクションをコミットします。
        /// 例外が発生した場合はトランザクションをロールバックします。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isolationLevel">分離レベル</param>
        public static void ActionWithTransaction(Action<SqlTransaction> action, System.Data.IsolationLevel isolationLevel)
        {
            ActionWithTransaction((tx, args) =>
            {
                args.Rollback = false;
                action(tx);
            }, isolationLevel);
        }

        /// <summary>
        /// 既定の設定でトランザクションを開始して、SQLTransactionと処理データ情報をパラメータにとるデリゲートに引数として渡します。
        /// デリゲートの実行が完了した場合はトランザクションをコミットします。
        /// 例外が発生した場合はトランザクションをロールバックします。
        /// </summary>
        /// <param name="action">トランザクションと処理データ情報をパラメータにとるデリゲート</param>
        public static void ActionWithTransaction(Action<SqlTransaction, TransactionArgs> action)
        {
            ActionWithTransaction(action, System.Data.IsolationLevel.ReadCommitted);
        }


        /// <summary>
        /// 既定の設定でトランザクションを開始して、SQLTransactionと処理データ情報をパラメータにとるデリゲートに引数として渡します。
        /// デリゲートの実行が完了した場合はトランザクションをコミットします。
        /// 例外が発生した場合はトランザクションをロールバックします。
        /// </summary>
        /// <param name="action">トランザクションと処理データ情報をパラメータにとるデリゲート</param>
        /// <param name="isolationLevel">分離レベル</param>
        public static void ActionWithTransaction(Action<SqlTransaction, TransactionArgs> action, System.Data.IsolationLevel isolationLevel)
        {
            using (SqlConnection conn = SQLHelper.GetSqlConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(isolationLevel);

                try
                {
                    var args = new TransactionArgs();
                    action(transaction, args);

                    if (args.Rollback)
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 指定したSQLクエリを結合して一つのSQLにして返します。
        /// </summary>
        /// <param name="mySqlSetList"></param>
        /// <returns></returns>
        public static string SQLQueryJoin(IEnumerable<string> queries)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var query in queries)
            {
                sb.AppendLine(query + ";");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 指定したバージョンカラムを表すbyteの配列がバージョン値を持つかどうかを返します。
        /// </summary>
        /// <param name="versionColumnValue"></param>
        /// <returns></returns>
        public static bool HasVersionValue(byte[] versionColumnValue)
        {
            return (versionColumnValue != null && versionColumnValue.Length > 0);
        }

        /// <summary>
        /// DataReaderの取得値が.NETネイティブ型のbyte[]に変換
        /// されるときに、null値をbyte[0]に変換して取得します。
        /// </summary>
        /// <param name="val">DataReaderから取得した特定の列</param>
        /// <returns>nullの場合byte[0]に変換された値。それ以外はbyte[]値</returns>
        /// <remarks>
        /// 指定したDataReaderからの取得値が.NETネイティブ型のbyte[]に変換
        /// できないときには例外が発生します。ただし、null値の場合はbyte[0]に変換
        /// するため、例外は発生しません。
        /// それ以外の、byte[]型に変換できる場合はbyte[]型に変換した値を取得
        /// します。
        /// </remarks>
        /// <exception cref="jp.co.jpsys.util.NSKArgumentException">
        /// byte[]型に変換できない値が指定されています。
        /// </exception>
        /// <example>
        /// </example>
        public static byte[] dbByteArr(object val)
        {

            byte[] ret = new byte[0];

            if (val is byte[])
            {
                ret = (byte[])val;
            }
            else if (val is System.DBNull || val == null)
            {

            }
            else
            {
                throw new NSKArgumentException("byte[]型に変換できない値が指定されています。");
            }

            return ret;
        }

        #region 常設列関連

        /// <summary>
        /// SQL文作成時の常設列の列選択リストの文字列を取得します。
        /// 文字列の末尾にカンマを追加されません。
        /// </summary>
        /// <param name="popColOpt">取得する常設列のオプション値</param>
        /// <returns>常設列の列選択リストの文字列</returns>
        public static string GetPopulateColumnSelectionString(
            SQLHelper.PopulateColumnOptions popColOpt)
        {
            string rt_val = string.Empty;

            StringBuilder sb = new StringBuilder();

            //オプション値をAND演算する

            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                sb.Append("[EntryOperatorId],");
                sb.Append("[EntryProcessId],");
                sb.Append("[EntryTerminalId],");
                sb.Append("[EntryDateTime],");

            }
            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                PopulateColumnOptions.AdditionColumns)
            {
                sb.Append("[AddOperatorId],");
                sb.Append("[AddProcessId],");
                sb.Append("[AddTerminalId],");
                sb.Append("[AddDateTime],");

            }
            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                PopulateColumnOptions.UpdateColumns)
            {
                sb.Append("[UpdateOperatorId],");
                sb.Append("[UpdateProcessId],");
                sb.Append("[UpdateTerminalId],");
                sb.Append("[UpdateDateTime]");

            }

            rt_val = sb.ToString();

            //末尾がカンマになっている時は省く
            if (rt_val.EndsWith(","))
            {
                rt_val = rt_val.Remove(rt_val.Length - 1);
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// AppAuthInfoクラスと、どの常設列に対するUpdate句を作成するかを格納した
        /// オプション値を指定して、常設列に対するUpdate句を作成して取得します。
        /// </summary>
        /// <param name="authInfo">認証済み利用者情報</param>
        /// <param name="popColOpt">どの常設列に対するUpdate句を作成するかを指定したオプション値</param>
        /// <returns>
        /// オプション値で指定された、常設列に設定するUpdate句
        /// </returns>
        public static string GetPopulateColumnUpdateString(AppAuthInfo authInfo,
            SQLHelper.PopulateColumnOptions popColOpt)
        {
            string rt_val = string.Empty;

            //登録に使う日付
            DateTime wk_now = DateTime.Now;

            //それぞれの関連のUpdate句を保持するStringBuilder
            StringBuilder sbEnt = new StringBuilder();
            StringBuilder sbAdd = new StringBuilder();
            StringBuilder sbUpd = new StringBuilder();

            //関連するUpdate句を保持するList
            List<StringBuilder> sb_list = new List<StringBuilder>();


            //指定された常設列の区分によってUpdate句用の値指定文字列を作成
            //オプション値をAND演算する
            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                //入力関連
                sbEnt.Append("[EntryOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbEnt.Append("[EntryProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbEnt.Append("[EntryTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbEnt.Append("[EntryDateTime]=" +
                    SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbEnt);

            }

            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                    PopulateColumnOptions.AdditionColumns)
            {
                //登録関連
                sbAdd.Append("[AddOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbAdd.Append("[AddProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbAdd.Append("[AddTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbAdd.Append("[AddDateTime]=" +
                    SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbAdd);

            }

            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                    PopulateColumnOptions.UpdateColumns)
            {
                //更新関連
                sbUpd.Append("[UpdateOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbUpd.Append("[UpdateProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbUpd.Append("[UpdateTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbUpd.Append("[UpdateDateTime]=" +
                    SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbUpd);

            }


            //それぞれの関連項目用のUpdate句が入ったListを大回転
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < sb_list.Count; i++)
            {
                if (sb_list[i].ToString().Length != 0)
                {
                    sbResult.Append(sb_list[i].ToString());

                    //すでに項目が存在しているときには、結合用にカンマを入れる
                    if (sbResult.ToString().Length != 0 &&
                        i != sb_list.Count - 1)
                    {
                        sbResult.Append(",");
                    }
                }
            }

            rt_val = sbResult.ToString();

            return rt_val;
        }

        /// <summary>
        /// AppAuthInfoクラスと、どの常設列に対するInsert句を作成するかを格納したオプション
        /// 値を指定して、常設列に対するInsert句を作成して取得します。
        /// </summary>
        /// <param name="baseInfo">認証済み利用者情報</param>
        /// <param name="popColOpt">どの常設列に対するInsert句を作成するかを指定したオプション値</param>
        /// <returns>
        /// オプション値で設定された、常設列に設定するInsert句
        /// </returns>
        public static string GetPopulateColumnInsertString(AppAuthInfo authInfo,
            SQLHelper.PopulateColumnOptions popColOpt)
        {
            string rt_val = string.Empty;

            //登録に使う日付
            DateTime wk_now = DateTime.Now;

            //それぞれの関連のInsert句を保持するStringBuilder
            StringBuilder sbEnt = new StringBuilder();
            StringBuilder sbAdd = new StringBuilder();
            StringBuilder sbUpd = new StringBuilder();

            //関連するInsert句を保持するList
            List<StringBuilder> sb_list = new List<StringBuilder>();


            //指定された常設列の区分によってInsert句用の値指定文字列を作成
            //オプション値をAND演算する
            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                //入力関連
                sbEnt.Append(authInfo.OperatorId.ToString() + ",");
                sbEnt.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbEnt.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbEnt.Append(SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbEnt);

            }

            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                    PopulateColumnOptions.AdditionColumns)
            {
                //登録関連
                sbAdd.Append(authInfo.OperatorId.ToString() + ",");
                sbAdd.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbAdd.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbAdd.Append(SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbAdd);

            }

            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                    PopulateColumnOptions.UpdateColumns)
            {
                //更新関連
                sbUpd.Append(authInfo.OperatorId.ToString() + ",");
                sbUpd.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbUpd.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbUpd.Append(SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbUpd);

            }


            //それぞれの関連項目用のInsert句が入ったListを大回転
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < sb_list.Count; i++)
            {
                if (sb_list[i].ToString().Length != 0)
                {
                    sbResult.Append(sb_list[i].ToString());

                    //すでに項目が存在しているときには、結合用にカンマを入れる
                    if (sbResult.ToString().Length != 0 &&
                        i != sb_list.Count - 1)
                    {
                        sbResult.Append(",");
                    }
                }
            }

            rt_val = sbResult.ToString();

            return rt_val;
        }

        /// <summary>
        /// IModelBaseに定義されたプロパティに対応するデータリーダの内の列の
        /// 値を取得して、AbstractModelBaseクラスを継承したオブジェクトに、
        /// 取得した値を設定します。
        /// </summary>
        /// <param name="info">AbstractModelBaseに定義されたプロパティに値を設定したいオブジェクト</param>
        /// <param name="rdr">対応するデータリーダ</param>
        /// <param name="columnPrefix">列名のプレフィックス</param>
        /// <returns>値を設定したオブジェクト</returns>
        public static T GetModelBaseValues<T>(T info, SqlDataReader rdr, string columnNamePrefix = "") where T : AbstractModelBase
        {
            info.EntryOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "EntryOperatorId"]);
            info.EntryProcessId = rdr[columnNamePrefix + "EntryProcessId"].ToString();
            info.EntryTerminalId = rdr[columnNamePrefix + "EntryTerminalId"].ToString();
            info.EntryDateTime = SQLHelper.dbNullableDate(rdr[columnNamePrefix + "EntryDateTime"]);

            info.AddOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "AddOperatorId"]);
            info.AddProcessId = rdr[columnNamePrefix + "AddProcessId"].ToString();
            info.AddTerminalId = rdr[columnNamePrefix + "AddTerminalId"].ToString();
            info.AddDateTime = SQLHelper.dbNullableDate(rdr[columnNamePrefix + "AddDateTime"]);

            info.UpdateOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "UpdateOperatorId"]);
            info.UpdateProcessId = rdr[columnNamePrefix + "UpdateProcessId"].ToString();
            info.UpdateTerminalId = rdr[columnNamePrefix + "UpdateTerminalId"].ToString();
            info.UpdateDateTime = SQLHelper.dbNullableDate(rdr[columnNamePrefix + "UpdateDateTime"]);
    
            return info;
        }

        /// <summary>
        /// IModelBaseに定義されたプロパティに対応するデータリーダの内の列の
        /// 値を取得して、AbstractModelBaseクラスを継承したオブジェクトに、
        /// 取得した値を設定します。
        /// </summary>
        /// <param name="info">AbstractModelBaseに定義されたプロパティに値を設定したいオブジェクト</param>
        /// <param name="rdr">対応するデータリーダ</param>
        /// <returns>値を設定したオブジェクト</returns>
        public static T GetTimeStampModelBaseValues<T>(T info, SqlDataReader rdr, string columnNamePrefix = "") where T : AbstractTimeStampModelBase
        {
            GetModelBaseValues(info, rdr, columnNamePrefix);
            info.VersionColumn = SQLHelper.dbByteArr(rdr[columnNamePrefix + "VersionColumn"]);

            return info;
        }

        #region 需要予測

        /// <summary>
        /// 指定された登録情報によって
        /// 新規登録か更新による新規登録の
        /// 常設列の取得オプションを返す
        /// </summary>
        /// <param name="info">登録情報</param>
        /// <returns>新規登録用の常設列の取得オプション</returns>
        public static SQLHelper.PopulateColumnOptions GetCreatePopulateColumnOptions(
            AbstractTimeStampModelBase info)
        {
            // 新規登録用の常設列の取得オプションを作る
            SQLHelper.PopulateColumnOptions popOption;

            if (info.AddDateTime.IsNullOrMinValue())
            {
                // 新規登録の場合
                popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;
            }
            else
            {
                // 更新による新規登録の場合
                popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;
            }

            //返却
            return popOption;
        }

        /// <summary>
        /// AppAuthInfoクラスと、どの常設列に対するInsert句を作成するかを格納したオプション
        /// 値、登録情報を指定して、常設列に対するInsert句を作成して取得します。
        /// 登録関連には編集前の登録情報を使用します
        /// </summary>
        /// <param name="baseInfo">認証済み利用者情報</param>
        /// <param name="popColOpt">どの常設列に対するInsert句を作成するかを指定したオプション値</param>
        /// <param name="info">登録情報</param>
        /// <returns>
        /// オプション値で設定された、常設列に設定するInsert句
        /// </returns>
        public static string GetPopulateColumnInsertString(AppAuthInfo authInfo,
            SQLHelper.PopulateColumnOptions popColOpt,
            AbstractTimeStampModelBase info)
        {
            string rt_val = string.Empty;

            //登録に使う日付
            DateTime wk_now = DateTime.Now;

            //それぞれの関連のInsert句を保持するStringBuilder
            StringBuilder sbEnt = new StringBuilder();
            StringBuilder sbAdd = new StringBuilder();
            StringBuilder sbUpd = new StringBuilder();

            //関連するInsert句を保持するList
            List<StringBuilder> sb_list = new List<StringBuilder>();


            //指定された常設列の区分によってInsert句用の値指定文字列を作成
            //オプション値をAND演算する
            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                //入力関連
                sbEnt.Append(authInfo.OperatorId.ToString() + ",");
                sbEnt.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbEnt.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbEnt.Append(SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbEnt);

            }

            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                    PopulateColumnOptions.AdditionColumns)
            {
                //登録関連（編集前の登録情報を使用する）
                sbAdd.Append(info.AddOperatorId.ToString() + ",");
                sbAdd.Append("N'" + info.AddProcessId.Trim() + "',");
                sbAdd.Append("N'" + info.AddTerminalId.Trim() + "',");
                sbAdd.Append(SQLHelper.DateTimeToDbDateTime(info.AddDateTime) + " ");

                sb_list.Add(sbAdd);

            }

            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                    PopulateColumnOptions.UpdateColumns)
            {
                //更新関連
                sbUpd.Append(authInfo.OperatorId.ToString() + ",");
                sbUpd.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbUpd.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbUpd.Append(SQLHelper.DateTimeToDbDateTime(wk_now) + " ");

                sb_list.Add(sbUpd);

            }


            //それぞれの関連項目用のInsert句が入ったListを大回転
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < sb_list.Count; i++)
            {
                if (sb_list[i].ToString().Length != 0)
                {
                    sbResult.Append(sb_list[i].ToString());

                    //すでに項目が存在しているときには、結合用にカンマを入れる
                    if (sbResult.ToString().Length != 0 &&
                        i != sb_list.Count - 1)
                    {
                        sbResult.Append(",");
                    }
                }
            }

            rt_val = sbResult.ToString();

            return rt_val;
        }

        #endregion

        #region トラDON登録用

        /// <summary>
        /// IModelBaseに定義されたプロパティに対応するデータリーダの内の列の
        /// 値を取得して、AbstractModelBaseクラスを継承したオブジェクトに、
        /// 取得した値を設定します。
        /// </summary>
        /// <param name="info">AbstractModelBaseに定義されたプロパティに値を設定したいオブジェクト</param>
        /// <param name="rdr">対応するデータリーダ</param>
        /// <returns>値を設定したオブジェクト</returns>
        public static T GetTimeStampModelBaseValuesForTraDon<T>(T info, SqlDataReader rdr, string columnNamePrefix = "") where T : AbstractTimeStampModelBase
        {
            GetModelBaseValuesForTraDon(info, rdr, columnNamePrefix);
            info.VersionColumn = SQLHelper.dbByteArr(rdr[columnNamePrefix + "VersionColumn"]);

            return info;
        }

        /// <summary>
        /// IModelBaseに定義されたプロパティに対応するデータリーダの内の列の
        /// 値を取得して、AbstractModelBaseクラスを継承したオブジェクトに、
        /// 取得した値を設定します。
        /// </summary>
        /// <param name="info">AbstractModelBaseに定義されたプロパティに値を設定したいオブジェクト</param>
        /// <param name="rdr">対応するデータリーダ</param>
        /// <param name="columnPrefix">列名のプレフィックス</param>
        /// <returns>値を設定したオブジェクト</returns>
        public static T GetModelBaseValuesForTraDon<T>(T info, SqlDataReader rdr, string columnNamePrefix = "") where T : AbstractModelBase
        {
            info.EntryOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "EntryOperatorId"]);
            info.EntryProcessId = rdr[columnNamePrefix + "EntryProcessId"].ToString();
            info.EntryTerminalId = rdr[columnNamePrefix + "EntryTerminalId"].ToString();
            info.EntryDateTime = NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "EntryDateTime"]));
            info.AddOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "AddOperatorId"]);
            info.AddProcessId = rdr[columnNamePrefix + "AddProcessId"].ToString();
            info.AddTerminalId = rdr[columnNamePrefix + "AddTerminalId"].ToString();
            info.AddDateTime = NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "AddDateTime"]));
            info.UpdateOperatorId = SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "UpdateOperatorId"]);
            info.UpdateProcessId = rdr[columnNamePrefix + "UpdateProcessId"].ToString();
            info.UpdateTerminalId = rdr[columnNamePrefix + "UpdateTerminalId"].ToString();
            info.UpdateDateTime = NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr[columnNamePrefix + "UpdateDateTime"]));

            return info;
        }

        /// <summary>
        /// AppAuthInfoクラスと、どの常設列に対するInsert句を作成するかを格納したオプション
        /// 値を指定して、常設列に対するInsert句を作成して取得します。
        /// </summary>
        /// <param name="authInfo">認証済み利用者情報</param>
        /// <param name="popColOpt">どの常設列に対するInsert句を作成するかを指定したオプション値</param>
        /// <returns>
        /// オプション値で設定された、常設列に設定するInsert句
        /// </returns>
        public static string GetPopulateColumnInsertStringForTraDon(AppAuthInfo authInfo,
            SQLHelper.PopulateColumnOptions popColOpt)
        {
            string rt_val = string.Empty;

            //登録に使う日付
            DateTime wk_now = DateTime.Now;

            //それぞれの関連のInsert句を保持するStringBuilder
            StringBuilder sbEnt = new StringBuilder();
            StringBuilder sbAdd = new StringBuilder();
            StringBuilder sbUpd = new StringBuilder();

            //関連するInsert句を保持するList
            List<StringBuilder> sb_list = new List<StringBuilder>();


            //指定された常設列の区分によってInsert句用の値指定文字列を作成
            //オプション値をAND演算する
            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                //入力関連
                sbEnt.Append(authInfo.OperatorId.ToString() + ",");
                sbEnt.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbEnt.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbEnt.Append(NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbEnt);

            }

            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                    PopulateColumnOptions.AdditionColumns)
            {
                //登録関連
                sbAdd.Append(authInfo.OperatorId.ToString() + ",");
                sbAdd.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbAdd.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbAdd.Append(NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbAdd);

            }

            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                    PopulateColumnOptions.UpdateColumns)
            {
                //更新関連
                sbUpd.Append(authInfo.OperatorId.ToString() + ",");
                sbUpd.Append("N'" + authInfo.UserProcessId.Trim() + "',");
                sbUpd.Append("N'" + authInfo.TerminalId.Trim() + "',");
                sbUpd.Append(NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbUpd);

            }


            //それぞれの関連項目用のInsert句が入ったListを大回転
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < sb_list.Count; i++)
            {
                if (sb_list[i].ToString().Length != 0)
                {
                    sbResult.Append(sb_list[i].ToString());

                    //すでに項目が存在しているときには、結合用にカンマを入れる
                    if (sbResult.ToString().Length != 0 &&
                        i != sb_list.Count - 1)
                    {
                        sbResult.Append(",");
                    }
                }
            }

            rt_val = sbResult.ToString();

            return rt_val;
        }

        /// <summary>
        /// AppAuthInfoクラスと、どの常設列に対するUpdate句を作成するかを格納した
        /// オプション値を指定して、常設列に対するUpdate句を作成して取得します。
        /// </summary>
        /// <param name="authInfo">認証済み利用者情報</param>
        /// <param name="popColOpt">どの常設列に対するUpdate句を作成するかを指定したオプション値</param>
        /// <returns>
        /// オプション値で指定された、常設列に設定するUpdate句
        /// </returns>
        public static string GetPopulateColumnUpdateStringForTraDon(AppAuthInfo authInfo,
            SQLHelper.PopulateColumnOptions popColOpt)
        {
            string rt_val = string.Empty;

            //登録に使う日付
            DateTime wk_now = DateTime.Now;

            //それぞれの関連のUpdate句を保持するStringBuilder
            StringBuilder sbEnt = new StringBuilder();
            StringBuilder sbAdd = new StringBuilder();
            StringBuilder sbUpd = new StringBuilder();

            //関連するUpdate句を保持するList
            List<StringBuilder> sb_list = new List<StringBuilder>();


            //指定された常設列の区分によってUpdate句用の値指定文字列を作成
            //オプション値をAND演算する
            if ((popColOpt & PopulateColumnOptions.EntryColumns) ==
                    PopulateColumnOptions.EntryColumns)
            {
                //入力関連
                sbEnt.Append("[EntryOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbEnt.Append("[EntryProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbEnt.Append("[EntryTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbEnt.Append("[EntryDateTime]=" +
                    NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbEnt);

            }

            if ((popColOpt & PopulateColumnOptions.AdditionColumns) ==
                    PopulateColumnOptions.AdditionColumns)
            {
                //登録関連
                sbAdd.Append("[AddOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbAdd.Append("[AddProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbAdd.Append("[AddTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbAdd.Append("[AddDateTime]=" +
                    NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbAdd);

            }

            if ((popColOpt & PopulateColumnOptions.UpdateColumns) ==
                    PopulateColumnOptions.UpdateColumns)
            {
                //更新関連
                sbUpd.Append("[UpdateOperatorId]=" +
                    authInfo.OperatorId.ToString() + ",");
                sbUpd.Append("[UpdateProcessId]=N'" +
                    authInfo.UserProcessId.Trim() + "',");
                sbUpd.Append("[UpdateTerminalId]=N'" +
                    authInfo.TerminalId.Trim() + "',");
                sbUpd.Append("[UpdateDateTime]=" +
                    NSKUtil.DateTimeToDecimalWithTime(wk_now).ToString() + " ");

                sb_list.Add(sbUpd);

            }


            //それぞれの関連項目用のUpdate句が入ったListを大回転
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < sb_list.Count; i++)
            {
                if (sb_list[i].ToString().Length != 0)
                {
                    sbResult.Append(sb_list[i].ToString());

                    //すでに項目が存在しているときには、結合用にカンマを入れる
                    if (sbResult.ToString().Length != 0 &&
                        i != sb_list.Count - 1)
                    {
                        sbResult.Append(",");
                    }
                }
            }

            rt_val = sbResult.ToString();

            return rt_val;
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static decimal GetTraDonSequenceId(SQLHelper.SequenceIdKind seqKind)
        {
            return GetTraDonSequenceId((int)seqKind);
        }

        /// <summary>
        /// 伝票管理マスタの区分番号のInt値を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。原則直接呼出しは禁止です。列挙体を引数にとる
        /// メソッドから呼び出すようにしてください。
        /// </summary>
        /// <param name="seqKindId">伝票管理マスタの区分番号種別のInt値</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static decimal GetTraDonSequenceId(int seqKindId)
        {
            //複数取得を件数1件で呼び出して先頭を返す
            return GetTraDonSequenceIds(seqKindId, 1).First();
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static List<decimal> GetTraDonSequenceIds(SQLHelper.SequenceIdKind seqkind, int count)
        {
            return GetTraDonSequenceIds((int)seqkind, count);
        }

        /// <summary>
        /// 伝票管理マスタの区分番号の種類と取得件数を指定して、テーブルへの値挿入時に
        /// 必要なIDを取得します。
        /// </summary>
        /// <param name="seqKind">伝票管理マスタの区分番号種別</param>
        /// <param name="count">取得件数</param>
        /// <returns>シーケンシャルに取得したID</returns>
        public static List<decimal> GetTraDonSequenceIds(int seqKindId, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("取得件数は0より下を指定できません。", "count");
            }

            //0件の場合は空の要素を返す。
            if (count == 0)
            {
                return new List<decimal>();
            }

            //返却値
            List<decimal> rt_val = new List<decimal>();

            //先頭のId。この値から取得件数分を返す
            decimal startId;

            //マルチスレッドで実行されるのでロックしておく
            lock (sequenceIdLockObject)
            {
                //更新用SQL格納用
                string mySqlUpd = string.Empty;

                //後方処理で使用するためにいったん変数に格納
                int sqlKindInt = seqKindId;

                //存在チェックと現状の最大値取得
                //SQL作成
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ");
                sb.Append("A.[Id] ");
                sb.Append("FROM ");
                sb.Append("IDManage AS A WITH(ROWLOCK, XLOCK, HOLDLOCK) ");
                sb.Append("WHERE ");
                sb.Append("A.[Kbn]=" + sqlKindInt.ToString());
                string mySql = sb.ToString();

                string connString = SQLHelper.GetConnectionStringTraDon();

                // 接続する
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        using (SqlDataReader rdr = SQLHelper.ExecuteReaderOnTransaction(transaction, new SqlCommand(mySql)))
                        {
                            if (rdr.Read())
                            {
                                //存在してたら、現在の伝票番号を取る
                                startId = SQLServerUtil.dbDecimal(rdr["Id"]);

                                //1加算する
                                startId++;

                                //UpdateのSQLを作成
                                StringBuilder sbUpd = new StringBuilder();
                                sbUpd.Append("UPDATE ");
                                sbUpd.Append("IDManage ");
                                sbUpd.Append("SET ");
                                sbUpd.Append("[Id] = " + (startId + count - 1).ToString() + " ");
                                sbUpd.Append("WHERE ");
                                sbUpd.Append("[Kbn]=" + sqlKindInt.ToString());

                                mySqlUpd = sbUpd.ToString();

                            }
                            else
                            {
                                //存在していなかったら新規に作る

                                //新規なので1をセット
                                startId = 1;

                                //InsertのSQLを作成
                                StringBuilder sbIns = new StringBuilder();
                                sbIns.Append("INSERT INTO ");
                                sbIns.Append("IDManage ");
                                sbIns.Append("( ");
                                sbIns.Append("[Kbn],[Id]");
                                sbIns.Append(") ");
                                sbIns.Append("VALUES( ");
                                sbIns.Append(
                                    sqlKindInt.ToString() + ",");
                                sbIns.Append(
                                    count.ToString());
                                sbIns.Append(") ");

                                mySqlUpd = sbIns.ToString();
                            }

                        }
                        try
                        {
#if DEBUG
                            Console.WriteLine(mySqlUpd);
#endif

                            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(mySqlUpd));
                            //エラーがなければコミット
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            //エラー時はロールバック
                            transaction.Rollback();
                            throw;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            //件数分をリストに追加する
            for (decimal i = 0; i < count; i++)
            {
                rt_val.Add(startId + i);
            }

            return rt_val;
        }

        #endregion

        #endregion

        /// <summary>
        /// SimpleReadを使用して、指定したSQLの実行結果が行を含むかどうかを返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool SimpleReadHasRows(string sql)
        {
            return SimpleReadHasRows(sql, null);
        }

        /// <summary>
        /// SimpleReadを使用して、指定したSQLの実行結果が行を含むかどうかを返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool SimpleReadHasRows(string sql, SqlTransaction transaction)
        {
            return SimpleReadSingle(sql, rdr => rdr.HasRows, transaction);
        }


        /// <summary>
        /// 指定したSQLを実行して結果を返します。
        /// レコードは上位1件のみ返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL分</param>
        /// <param name="fetchFunc">レコード単位の読み取り関数</param>
        /// <returns></returns>
        public static T SimpleReadSingle<T>(string sql, Func<SqlDataReader, T> fetchFunc)
        {
            return SimpleRead(sql, fetchFunc).FirstOrDefault();
        }

        /// <summary>
        /// 指定したSQLを実行して結果を返します。
        /// レコードは上位1件のみ返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL分</param>
        /// <param name="fetchFunc">レコード単位の読み取り関数</param>
        /// <returns></returns>
        public static T SimpleReadSingle<T>(string sql, Func<SqlDataReader, T> fetchFunc, SqlTransaction transaction)
        {
            return SimpleRead(sql, fetchFunc, transaction).FirstOrDefault();
        }

        /// <summary>
        /// 指定したSQLを実行して結果を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL分</param>
        /// <param name="fetchFunc">レコード単位の読み取り関数</param>
        /// <returns></returns>
        public static List<T> SimpleRead<T>(string sql, Func<SqlDataReader, T> fetchFunc)
        {
            return SimpleRead(sql, fetchFunc, null);
        }

        /// <summary>
        /// 指定したSQLを実行して結果を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL分</param>
        /// <param name="fetchFunc">レコード単位の読み取り関数</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<T> SimpleRead<T>(string sql, Func<SqlDataReader, T> fetchFunc, SqlTransaction transaction)
        {
            List<T> result = new List<T>();

            using (var connectionDirector = new ConnectionDirector(transaction))
            {
                SqlConnection conn = connectionDirector.Connection;
                var command = conn.CreateCommand();
                if (transaction != null)
                    command.Transaction = transaction;

                command.CommandText = sql;
                command.CommandTimeout = 300;
                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.Add(fetchFunc(rdr));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 2つのRowVersion値を比較して等しいかどうかを判断します。(true:等値)
        /// RowVersion値が違った場合は例外をスローします。
        /// </summary>
        /// <param name="rv1">2つめと比較するRowVersion値</param>
        /// <param name="rv2">1つめと比較するRowVersion値</param>
        /// <returns>等しいかどうか</returns>
        public static bool CheckRowVersionValue(byte[] rv1, byte[] rv2)
        {
            bool rt_val = true;

            //値の比較
            if (!Enumerable.SequenceEqual(rv1, rv2))
            {
                rt_val = false;
                //例外をスロー
                throw new
                    Model.DALExceptions.RowVersionUnmatchingException("RowVersionが一致しません。");
            }

            return rt_val;
        }

        /// <summary>
        /// 指定したSQLを実行して、実行結果を変換関数で変換して返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL分</param>
        /// <param name="converter">DataReaderからの変換関数</param>
        /// <param name="transaction">トランザクション。nullの場合は、このメソッドスコープ内のみで有効なトランザクションを作成して開始します。</param>
        /// <returns></returns>
        public static T Read<T>(string sql, Func<SqlDataReader, T> converter, SqlTransaction transaction = null)
        {
            using (var connectionDirector = new ConnectionDirector(transaction))
            {
                SqlConnection conn = connectionDirector.Connection;
                var command = conn.CreateCommand();
                if (transaction != null)
                    command.Transaction = transaction;

                command.CommandText = sql;
                using (var rdr = command.ExecuteReader())
                {
                    return converter(rdr);
                }
            }
        }

        /// <summary>
        /// SQ文とトランザクションを指定しSQL文の結果の件数を返します
        /// 列名は Count です。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int GetCountByQuery(string sql, SqlTransaction transaction)
        {
            string query = "SELECT COUNT(*) CNT FROM (" + sql + ") CountTable";

            if (transaction == null)
            {
                return SimpleReadSingle(query, rdr => SQLServerUtil.dbInt(rdr["CNT"]));
            }
            else
            {
                return SimpleReadSingle(query, rdr => SQLServerUtil.dbInt(rdr["CNT"]), transaction);
            }
        }

        /// <summary>
        /// 値型リストを指定して、SQLQueryのwhere文のINの中を作成します。
        /// </summary>
        /// <param name="list">値型のリスト</param>
        /// <returns>SQLQueryのwhere文のINの中</returns>
        public static string GetSQLQueryWhereInByStructList<T>(IList<T> list) where T : struct
        {
            StringBuilder sb = new StringBuilder();

            //ループで回してIN句に含める
            for (int i = 0; i < list.Count; i++)
            {
                //初回は","を付けない
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(list[i].ToString() + " ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 値型リストを指定して、SQLQueryのwhere文のINの中を作成します。
        /// </summary>
        /// <param name="list">値型のリスト</param>
        /// <returns>SQLQueryのwhere文のINの中</returns>
        public static string GetSQLQueryWhereInByStructList(IList<string> list)
        {
            StringBuilder sb = new StringBuilder();

            //ループで回してIN句に含める
            for (int i = 0; i < list.Count; i++)
            {
                //初回は","を付けない
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append("N'" +  SQLHelper.GetSanitaizingSqlString(list[i].ToString()) + "' ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 指定したSELECT文を実行して、実行結果にレコードが存在するかどうかを返します。
        /// パフォーマンスのために指定したクエリをEXISTS句でラップするので共通表式などは指定できません
        /// </summary>
        /// <param name="selectQuery"></param>
        /// <returns></returns>
        public static bool RecordExists(string selectQuery, SqlTransaction transaction)
        {
            bool result = false;

            SqlCommand command = new SqlCommand();
            command.Connection = transaction.Connection;
            command.Transaction = transaction;

            //引数のクエリを加工して存在チェック用のSQLを作成する。（引数のクエリをそのまま使うと大量データの場合にパフォーマンスが芳しくない為）
            string existQuery = string.Format("SELECT 'TRUE' WHERE EXISTS ({0})", selectQuery);
            command.CommandText = existQuery;


            using (SqlDataReader rdr = command.ExecuteReader())
            {
                result = rdr.HasRows;
            }

            return result;
        }

        /// <summary>
        /// ブール型に値を変換します
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool dbBit(object val)
        {
            bool ret = false;

            if (val is bool)
            {
                ret = (bool)val;
            }
            else if (val is System.DBNull || val == null)
            {
            }
            else
            {
                throw new NSKArgumentException("bool 型に変換できない値が指定されています。");
            }

            return ret;
        }



        /// <summary>
        /// DataReaderの値をDateTimeに変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T dbValue<T>(object val)
        {
            if (val is DBNull)
                return default(T);

            return (T)val;
        }

        #region DateTimeの変換

        /// <summary>
        /// DataReaderの値をDateTimeに変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime dbDate(object val)
        {
            if (val is DBNull)
                return DateTime.MinValue;

            if (val is DateTime)
                return (DateTime)val;

            throw new NSKArgumentException("DateTime 型に変換できない値が指定されています。");    
        }


        /// <summary>
        /// DataReaderの値をNull許容のDateTimeに変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime? dbNullableDate(object val)
        {
            if (val is DBNull)
                return null;

            if (val is DateTime)
                return (DateTime)val;

            throw new NSKArgumentException("DateTime 型に変換できない値が指定されています。");
        }

        /// <summary>
        /// DateTime型の値をSQL文で使用できる形式の文字列に変換します。（シングルコーテーション付き）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateTimeToDbDateTime(DateTime? val)
        {
            if (!val.HasValue)
                return "NULL";

            return "'" + val.Value.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'";
        }

        /// <summary>
        /// DateTime型の値をSQL文で使用できる形式の文字列に変換します。（シングルコーテーション付き）
        /// 引数の値がDatetime.MinValueの場合はNULLを返します
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateTimeToDbDateTime(DateTime val)
        {
            if (val == DateTime.MinValue)
                return "NULL";

            return "'" + val.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'";
        }

        #endregion

        #region DateTimeの変換（YYYYMM形式）

        /// <summary>
        /// DataReaderの値をDateTimeに変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime dbDateForYYYYMM(object val)
        {
            if (!(val is decimal))
                throw new NSKArgumentException("DateTime（年月）に変換できない型が指定されました");

            return NSKUtil.DecimalYYYYMMToDateTime((decimal)val);
        }


        /// <summary>
        /// DataReaderの値をNull許容のDateTimeに変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime? dbNullableDateForYYYYMM(object val)
        {
            if (val is DBNull)
                return null;

            if (!(val is decimal))
                throw new NSKArgumentException("DateTime（年月）に変換できない型が指定されました");

            if (((decimal)val) == 0)
                return null;

            return NSKUtil.DecimalYYYYMMToDateTime((decimal)val);
        }

        /// <summary>
        /// DataReaderの値をNull許容のInt32に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Int32? dbNullableInt(object val)
        {
            if (val is DBNull)
                return null;

            if (val is Int32)
                return (Int32)val;

            throw new NSKArgumentException("Int32 型に変換できない値が指定されています。");
        }

        /// <summary>
        /// DataReaderのNull値の場合、Stringのnull値に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String dbNullableString(object val)
        {
            if (val is DBNull)
                return null;

            if (val is String)
                return (String)val;

            if (val is Int32)
                return val.ToString();

            if (val is Decimal)
                return val.ToString();

            if (val is double)
                return val.ToString();

            if (val is Boolean)
                return val.ToString();

            throw new NSKArgumentException("String 型に変換できない値が指定されています。");
        }

        /// <summary>
        /// DateTime型の値をデータベースのDecimalに等しい値に変換します。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static decimal DateTimeToDbDecimalForYYYYMM(DateTime? val)
        {
            if (!val.HasValue)
                return 0;

            return NSKUtil.DateTimeToDecimalYYYYMM(val.Value);
        }

        #endregion

        /// <summary>
        /// DateTime型の値をSQL文で使用できる形式の文字列に変換します。（シングルコーテーション付き）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateTimeToDbDate(DateTime? val)
        {
            if (!val.HasValue)
                return "NULL";

            return "'" + val.Value.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'";
        }

        /// <summary>
        /// DateTime型の値をSQL文で使用できる形式の文字列に変換します。（シングルコーテーション付き）
        /// 引数の値がDatetime.MinValueの場合はNULLを返します
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateTimeToDbDate(DateTime val)
        {
            if (val == DateTime.MinValue)
                return "NULL";

            return "'" + val.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'";
        }

        /// <summary>
        /// SQLExceptionのエラー番号を定義する列挙体です。
        /// </summary>
        public enum SQLExceptionNumber : int
        {
            SanjyutsuOverFlow = 8115
        }

        /// <summary>
        /// トラDONDBの接続文字列を設定ファイルから取得
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionStringTraDon()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            //接続文字列が暗号化されているか確認
            //　EncryptedConnectionSettingキーの値を確認。キーがない場合は暗号化されていないものと判断する。
            var encflgstring = ConfigurationManager.AppSettings["EncryptedConnectionSetting"] ?? false.ToString();
            var encflg = Convert.ToBoolean(encflgstring);

            string strDataSource = string.Empty;
            string strInitialCatalog = string.Empty;
            bool boolIntegratedSecurity = false;
            string strUserID = string.Empty;
            string strPassword = string.Empty;
            int intConnectTimeout = 0;

            if (encflg)
            {
                //接続文字列は暗号化されている

                //いったん構成ファイルの設定値を取得しておく
                string enc_sqlsv =
                    ConfigurationManager.AppSettings["TraDonSQLServerName"];
                string enc_sqldb =
                    ConfigurationManager.AppSettings["TraDonSQLDbName"];
                string enc_sqluser =
                    ConfigurationManager.AppSettings["TraDonSQLUserid"];
                string enc_sqlpass =
                    ConfigurationManager.AppSettings["TraDonSQLPassword"];
                string enc_sqltrusty =
                    ConfigurationManager.AppSettings["TraDonSQLTrusty"];

                //復号して取得取得
                strDataSource = NSKUtil.DecryptString(enc_sqlsv, DEC_KEY);
                strInitialCatalog = NSKUtil.DecryptString(enc_sqldb, DEC_KEY);
                boolIntegratedSecurity = bool.Parse(NSKUtil.DecryptString(enc_sqltrusty, DEC_KEY));
                strUserID = NSKUtil.DecryptString(enc_sqluser, DEC_KEY);
                strPassword = NSKUtil.DecryptString(enc_sqlpass, DEC_KEY);
                intConnectTimeout = int.Parse(ConfigurationManager.AppSettings["TraDonSQLTimeout"]);
            }
            else
            {
                //暗号化されていない

                //設定ファイルから取得
                strDataSource = ConfigurationManager.AppSettings["TraDonSQLServerName"];
                strInitialCatalog = ConfigurationManager.AppSettings["TraDonSQLDbName"];
                boolIntegratedSecurity = bool.Parse(ConfigurationManager.AppSettings["TraDonSQLTrusty"]);
                strUserID = ConfigurationManager.AppSettings["TraDonSQLUserid"];
                strPassword = ConfigurationManager.AppSettings["TraDonSQLPassword"];
                intConnectTimeout = int.Parse(ConfigurationManager.AppSettings["TraDonSQLTimeout"]);
            }

            return new SqlConnectionStringBuilder()
            {
                DataSource = strDataSource,
                InitialCatalog = strInitialCatalog,
                IntegratedSecurity = boolIntegratedSecurity,
                UserID = strUserID,
                Password = strPassword,
                ConnectTimeout = intConnectTimeout
            }.ToString();
        }

        /// <summary>
        /// 顧客識別名をもとに顧客判定を行います。
        /// </summary>
        /// <param name="s">顧客識別名</param>
        /// <returns>顧客判定結果</returns>
        public static bool IsCustomer(string s)
        {
            string customer = StringHelper.ConvertToString(ConfigurationManager.AppSettings["EditionName"]).Trim();
            return customer.Equals(s);
        }

        /// <summary>
        /// テーブル名と列名を引き渡して、整数部の桁数を返却します。
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static int GetIntegerPartDigitsFromDatabase(string tableName, string columnName)
        {
            //返却値用
            int rt_val = 0;

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("	TABLE_CATALOG ");
            sb.Append("	,TABLE_SCHEMA ");
            sb.Append("	,TABLE_NAME ");
            sb.Append("	,COLUMN_NAME ");
            sb.Append("	,NUMERIC_PRECISION ");
            sb.Append("	,NUMERIC_SCALE ");
            sb.Append("	,CHARACTER_MAXIMUM_LENGTH ");
            sb.Append("FROM ");
            sb.Append("	INFORMATION_SCHEMA.COLUMNS ");
            sb.Append("WHERE ");
            sb.Append("	TABLE_NAME = N'" + tableName + "' ");
            sb.Append("AND	COLUMN_NAME = N'" + columnName + "' ");

            String mySql = sb.ToString();

            //SQLServerとの接続を確立
            SQLServerConnection conn = SQLHelper.GetSqlServerConn();

            try
            {
                conn.Open();
                using (SqlDataReader rdr = conn.ReadMyData(mySql))
                {
                    while (rdr.Read())
                    {
                        var intdigval = rdr["NUMERIC_PRECISION"];
                        var digdigval = rdr["NUMERIC_SCALE"];
                        var chrdigval = rdr["CHARACTER_MAXIMUM_LENGTH"];

                        if (intdigval == null || intdigval == System.DBNull.Value)
                        {
                            if (chrdigval == null || chrdigval == System.DBNull.Value)
                            {

                            }
                            else
                            {
                                //文字列の長さ
                                rt_val = Convert.ToInt32(chrdigval);
                            }
                        }
                        else
                        {
                            //整数部分
                            rt_val =
                                Convert.ToInt32(intdigval) -
                                Convert.ToInt32(digdigval);
                        }
                        //1件取得したら、強制的に抜ける。
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //後片付け
                conn.Close();
            }

            return rt_val;

        }

        /// <summary>
        /// テーブル名と列名を引き渡して、小数部の桁数を返却します。
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static int GetDecimalPartDigitsFromDatabase(string tableName, string columnName)
        {
            //返却値用
            int rt_val = 0;

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("	TABLE_CATALOG ");
            sb.Append("	,TABLE_SCHEMA ");
            sb.Append("	,TABLE_NAME ");
            sb.Append("	,COLUMN_NAME ");
            sb.Append("	,NUMERIC_PRECISION ");
            sb.Append("	,NUMERIC_SCALE ");
            sb.Append("FROM ");
            sb.Append("	INFORMATION_SCHEMA.COLUMNS ");
            sb.Append("WHERE ");
            sb.Append("	TABLE_NAME = N'" + tableName + "' ");
            sb.Append("AND	COLUMN_NAME = N'" + columnName + "' ");

            String mySql = sb.ToString();

            //SQLServerとの接続を確立
            SQLServerConnection conn = SQLHelper.GetSqlServerConn();

            try
            {
                conn.Open();
                using (SqlDataReader rdr = conn.ReadMyData(mySql))
                {
                    while (rdr.Read())
                    {
                        var val = rdr["NUMERIC_SCALE"];

                        if (val == null || val == System.DBNull.Value)
                        {

                        }
                        else
                        {
                            //整数部分
                            rt_val = Convert.ToInt32(val);
                        }

                        //1件取得したら、強制的に抜ける。
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //後片付け
                conn.Close();
            }

            return rt_val;
        }
    }

    /// <summary>
    /// ActionWithTransactionのデリゲートに渡すデータを表します。
    /// </summary>
    public class TransactionArgs
    {
        public TransactionArgs()
            : this(false)
        { }

        public TransactionArgs(bool rollback)
        {
            this.Rollback = rollback;
        }

        /// <summary>
        /// トランザクションを意図的にロールバックするかどうか
        /// </summary>
        public bool Rollback { get; set; }
    }

    /// <summary>
    /// コネクションの生存を管理します。
    /// コネクションが外部から指定された場合は、コネクションの開閉処理はしません。
    /// コネクションが外部から指定されない場合は、オブジェクト内でコネクション作成および開閉処理をします。
    /// </summary>
    internal class ConnectionDirector : IDisposable
    {
        private readonly bool _connectionCreatedAtThis = false;
        private readonly SqlConnection _conn;

        /// <summary>
        /// 初期化処理をします。
        /// 外部からのコネクションがない場合は、コネクションを作成して接続を開きます。
        /// </summary>
        /// <param name="transaction"></param>
        internal ConnectionDirector(SqlTransaction transaction)
        {
            if (transaction != null)
            {
                _conn = transaction.Connection;
            }
            else
            {
                _conn = SQLHelper.GetSqlConnection();
                _connectionCreatedAtThis = true;

                _conn.Open();
            }
        }

        /// <summary>
        /// コネクションを取得します。
        /// </summary>
        public SqlConnection Connection { get { return _conn; } }

        public void Dispose()
        {
            if (_connectionCreatedAtThis)
            {
                _conn.Close();
            }
        }
    }

}
