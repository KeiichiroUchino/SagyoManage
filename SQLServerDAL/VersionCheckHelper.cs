using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.ComLib;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// バージョンカラムチェック用のメソッドを提供します。
    /// </summary>
    internal static class VersionCheckHelper
    {

        /// <summary>
        /// モデルインスタンスのバージョンチェックを行います。
        /// バージョンチェックでエラーが見つかった場合は、例外をスローします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">Sqlトランザクション</param>
        /// <param name="tableName">テーブル名</param>
        /// <param name="tableDisplayText">テーブルの表示用テキスト</param>
        /// <param name="idColumn">ID列の名前</param>
        /// <param name="checkTarget">チェック対象のリスト</param>
        /// <param name="idSelector">モデルからIDを取得するセレクタ</param>
        internal static void CheckVersionForList<T>(SqlTransaction transaction, string tableName, string tableDisplayText, string idColumn, IEnumerable<T> checkTarget, Func<T, decimal> idSelector) where T : AbstractSequenceKeyTimeStampModelBase
        {
            var idVersions = GetIdVersions(transaction, tableName, idColumn, checkTarget, idSelector);
            ThrowExeptionIfContainsUnmatchVersion(checkTarget, idVersions, idSelector, tableDisplayText);
        }

        /// <summary>
        /// 指定したモデルインスタンスのID/VersionペアをDBから取得します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">テーブル名</param>
        /// <param name="idColumn">ID列の名前</param>
        /// <param name="source">対象のリスト</param>
        /// <param name="idSelector">モデルからIDを取得するセレクタ</param>
        private static IEnumerable<IdVersionPair> GetIdVersions<T>(SqlTransaction transaction, string tableName, string idColumn, IEnumerable<T> source, Func<T, decimal> idSelector) where T : AbstractSequenceKeyTimeStampModelBase
        {
            //***IN句に指定するリストを作る
            var allIds = source.Where(element => element.IsPersisted)
                    .Select(idSelector);

            //***対象が存在しない場合、SQL上でIn句の値セットが空になるためにエラーが発生してしまう。
            //***それを回避するために対象が存在しない場合は即リターンする。
            if (!allIds.Any())
            {
                return Enumerable.Empty<IdVersionPair>();
            }

            List<IdVersionPair> result = new List<IdVersionPair>();

            //一回のクエリでIN句のリストが多すぎるとエラーになるので、1000個ずつに分けて処理する。
            foreach (var ids in allIds.Chunk(1000))
            {
                string idListInSql = string.Join(", ", ids.Select(id => id.ToString()));

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT");
                sb.AppendLine("	{0} AS Id ");
                sb.AppendLine("	,VersionColumn ");
                sb.AppendLine("FROM	");
                sb.AppendLine("	{1} ");
                sb.AppendLine("WHERE ");
                sb.AppendLine(" {0} IN ({2}) ");

                string sql = string.Format(sb.ToString(), idColumn, tableName, idListInSql);

                var idVersionPairs = SQLHelper.SimpleRead(sql, rdr =>
                    new IdVersionPair(
                        SQLServerUtil.dbDecimal(rdr["Id"]),
                        SQLHelper.dbByteArr(rdr["VersionColumn"])), transaction);

                result.AddRange(idVersionPairs);
            }

            return result;
        }

        /// <summary>
        /// モデルインスタンスのコレクションとDBから取り出したID/Versionペアを比較して
        /// バージョン不一致のモデルインスタンスが見つかった場合は例外をスローします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">モデルインスタンスのコレクション</param>
        /// <param name="idVersions">ID/Versionペアのコレクション</param>
        /// <param name="idSelector">モデルインスタンスからIDを取得するセレクタ</param>
        /// <param name="tableName">テーブル名</param>
        private static void ThrowExeptionIfContainsUnmatchVersion<T>(IEnumerable<T> entities, IEnumerable<IdVersionPair> idVersions, Func<T, decimal> idSelector, string tableDisplayText) where T : AbstractSequenceKeyTimeStampModelBase
        {
            //ループを回しては一チェック
            foreach (T item in entities.Where(element => element.IsPersisted))
            {
                //モデルインスタンスに対応するIdVersionペアを取り出す。
                IdVersionPair hitIdVersion = idVersions.FirstOrDefault(idVersion => idVersion.Id == idSelector(item));

                if (hitIdVersion != null)
                {
                    try
                    {
                        //取得できたら排他チェックを行う
                        SQLHelper.CheckRowVersionValue(item.VersionColumn, hitIdVersion.Version);
                    }
                    catch (Model.DALExceptions.RowVersionUnmatchingException)
                    {
                        //リトライ可能な例外
                        throw new
                            Model.DALExceptions.CanRetryException(
                            string.Format("この{0}データは既に他の利用者に更新されている為、更新できません。" +
                            "\r\n" + "最新情報を確認してください。", tableDisplayText)
                            , MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
