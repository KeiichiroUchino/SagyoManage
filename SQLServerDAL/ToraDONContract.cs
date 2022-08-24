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
using System.Configuration;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// トラDON請負テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDONContract
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
        /// トラDON請負クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDONContract()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、トラDON請負テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDONContract(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定でトラDON請負情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONContractInfo GetInfo(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONContractSearchParameter()
            {
                ContractId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、トラDON請負情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON請負情報のリスト</returns>
        public IList<ToraDONContractInfo> GetList(ToraDONContractSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<ToraDONContractInfo> GetListInternal(SqlTransaction transaction, ToraDONContractSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONContractInfo> rt_list = new List<ToraDONContractInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Contract.* ");
            sb.AppendLine("	,Tokuisaki.TokuisakiCd ");
            sb.AppendLine("	,Tokuisaki.TokuisakiNM ");
            sb.AppendLine("	,ClmClass.ClmClassCd ");
            sb.AppendLine("	,ClmClass.ClmClassNM ");
            sb.AppendLine(" ,TaxKbnShort.SystemNameName AS TaxDispKbnSNM ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Contract Contract ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki Tokuisaki ");
            sb.AppendLine(" ON  Tokuisaki.TokuisakiId = Contract.TokuisakiId ");
            sb.AppendLine(" AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_ClmClass ClmClass ");
            sb.AppendLine(" ON  ClmClass.ClmClassId = Contract.ClmClassId ");
            sb.AppendLine(" AND ClmClass.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" (SELECT * FROM SystemName WHERE SystemNameKbn = " +
                (int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort + ") AS TaxKbnShort ON ");
            sb.Append(" Contract.TaxDispKbn = TaxKbnShort.SystemNameCode ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Contract.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ContractId.HasValue)
                {
                    sb.AppendLine("AND Contract.ContractId = " + para.ContractId.ToString() + " ");
                }
                if (para.TokuisakiId.HasValue)
                {
                    sb.AppendLine("AND Contract.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (para.ClmClassId.HasValue)
                {
                    sb.AppendLine("AND Contract.ClmClassId = " + para.ClmClassId.ToString() + " ");
                }
                if (para.ContractCode.HasValue)
                {
                    sb.AppendLine("AND Contract.ContractCd = " + para.ContractCode.ToString() + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ISNULL(Tokuisaki.TokuisakiCd, 0), ISNULL(ClmClass.ClmClassCd, 0), Contract.ContractCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONContractInfo rt_info = new ToraDONContractInfo()
                {
                    ContractId = SQLServerUtil.dbDecimal(rdr["ContractId"]),
                    TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                    ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]),
                    ContractCode = SQLServerUtil.dbInt(rdr["ContractCd"]),
                    ContractName = rdr["ContractNM"].ToString(),
                    ContractStartDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["ContractStartDate"])),
                    ContractEndDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["ContractEndDate"])),
                    AddUpDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["AddUpDate"])),
                    ContractPrice = SQLServerUtil.dbDecimal(rdr["ContractPrice"]),
                    PriceInContractPrice = SQLServerUtil.dbDecimal(rdr["PriceInContractPrice"]),
                    TollFeeInContractPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInContractPrice"]),
                    TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]),
                    TaxDispKbnShortName = rdr["TaxDispKbnSNM"].ToString(),
                    EndKbn = SQLServerUtil.dbInt(rdr["EndKbn"]),
                    Memo = rdr["Memo"].ToString(),
                    PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]),
                    PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]),
                    PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]),
                    PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]),
                    PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]),
                    ClmFixDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["ClmFixDate"])),

                    TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]),
                    TokuisakiName = rdr["TokuisakiNM"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
