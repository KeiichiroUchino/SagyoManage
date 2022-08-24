using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;
using System.Configuration;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 車種グループ明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class CarKindGroupMeisai
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
        /// 車種グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public CarKindGroupMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public CarKindGroupMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// 検索条件情報を指定して、車種グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種グループ情報一覧</returns>
        public IList<CarKindGroupMeisaiInfo> GetList(CarKindGroupMeisaiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 車種グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種グループ情報一覧</returns>
        private IList<CarKindGroupMeisaiInfo> GetListInternal(SqlTransaction transaction, CarKindGroupMeisaiSearchParameter para)
        {
            //返却用の一覧
            List<CarKindGroupMeisaiInfo> rt_list = new List<CarKindGroupMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 CarKindGroupMeisai.* ");
            sb.AppendLine("	,ToraDONCarKind.CarKindCd ToraDONCarKindCode ");
            sb.AppendLine("	,ToraDONCarKind.CarKindNM ToraDONCarKindName ");
            sb.AppendLine("	,ToraDONCarKind.DisableFlag ToraDONDisableFlag ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" CarKindGroupMeisai ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" CarKindGroup ");
            sb.AppendLine(" ON CarKindGroup.CarKindGroupId = CarKindGroupMeisai.CarKindGroupId ");
            sb.AppendLine(" AND CarKindGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" TORADON_CarKind ToraDONCarKind ");
            sb.AppendLine(" ON ToraDONCarKind.CarKindId = CarKindGroupMeisai.ToraDONCarKindId ");
            sb.AppendLine(" AND ToraDONCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1 ");

            if (para != null)
            {
                if (para.CarKindGroupId.HasValue)
                {
                    sb.AppendLine(" AND CarKindGroupMeisai.CarKindGroupId = " + para.CarKindGroupId.ToString() + " ");
                }
                if (para.Gyo.HasValue)
                {
                    sb.AppendLine(" AND CarKindGroupMeisai.Gyo = " + para.Gyo.ToString() + " ");
                }
                if (para.ToraDONCarKindId.HasValue)
                {
                    sb.AppendLine(" AND CarKindGroupMeisai.ToraDONCarKindId = " + para.ToraDONCarKindId.ToString() + " ");
                }
                if (para.ToraDONDisableFlag.HasValue)
                {
                    sb.AppendLine(" AND ToraDONCarKind.DisableFlag = " + (NSKUtil.BoolToInt(para.ToraDONDisableFlag.Value)).ToString() + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	CarKindGroupMeisai.CarKindGroupId, CarKindGroupMeisai.Gyo ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                CarKindGroupMeisaiInfo rt_info = new CarKindGroupMeisaiInfo
                {
                    CarKindGroupId = SQLServerUtil.dbDecimal(rdr["CarKindGroupId"]),
                    Gyo = SQLServerUtil.dbInt(rdr["Gyo"]),
                    ToraDONCarKindId = SQLServerUtil.dbDecimal(rdr["ToraDONCarKindId"]),

                    ToraDONCarKindCode = SQLServerUtil.dbInt(rdr["ToraDONCarKindCode"]),
                    ToraDONCarKindName = rdr["ToraDONCarKindName"].ToString(),

                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
