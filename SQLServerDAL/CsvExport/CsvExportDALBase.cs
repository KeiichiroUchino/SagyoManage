using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;


namespace Jpsys.SagyoManage.SQLServerDAL.CsvExport
{
    public abstract class CsvExportDALBase
    {
        public abstract string GetName();
        /// <summary>
        /// データを取得するためのSQLを記述します。並び替えのためのORDERBY句は除いてください。
        /// </summary>
        /// <returns></returns>
        protected abstract string GetSelectoinDataSqlWithoutOrderBy(bool hideDisabledData);
        /// <summary>
        /// 並び替えのためのOrder句を取得します。
        /// </summary>
        /// <returns></returns>
        protected abstract string GetOrderByClause();

        /// <summary>
        /// 列名のキャッシュ
        /// </summary>
        private string[] columnNames = null;
        /// <summary>
        /// 列名を取得します。
        /// </summary>
        /// <returns></returns>
        public string[] GetColumnNames()
        {
            //まだ取得していなければ取りに行くのでその判定
            if (this.columnNames == null)
            {
                List<string> columnNamesAsList = new List<string>();

                //速度を確保するためにTOP1件のみとる
                string sql =
                    string.Format("SELECT TOP 0 * FROM ({0}) AS Data", this.GetSelectoinDataSqlWithoutOrderBy(false));

                //SQLServerとの接続を確立
                SqlConnection conn = SQLHelper.GetSqlConnection();

                try
                {
                    conn.Open();
                    using (SqlDataReader rdr = SQLHelper.ExecuteReaderOnConnection(conn, new SqlCommand(sql)))
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            columnNamesAsList.Add(rdr.GetName(i));
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

                this.columnNames = columnNamesAsList.ToArray();
            }

            return this.columnNames;
        }

        /// <summary>
        /// CsvDataを取得します。
        /// </summary>
        /// <param name="selectionColumns"></param>
        /// <returns></returns>
        public List<string[]> GetCsvData(string[] selectionColumns, bool disabledFlag)
        {
            List<string[]> rt_list = new List<string[]>();

            //***タイトル
            List<string> colname_list = new List<string>();
            foreach (string item in selectionColumns)
            {
                colname_list.Add(item);
            }
            rt_list.Add(colname_list.ToArray());

            //***データ
            string sql = this.GetSelectoinDataSqlWithoutOrderBy(disabledFlag) + this.GetOrderByClause();
            var data = SQLHelper.SimpleRead(sql, rdr =>
            {
                //1行分のリスト
                List<string> wk_list = new List<string>();

                //引数の配列に格納されているものを取得する
                foreach (string item in selectionColumns)
                {
                    wk_list.Add(rdr[item].ToString());
                }

                return wk_list.ToArray();
            });

            rt_list.AddRange(data);
            return rt_list;
        }
    }
}
