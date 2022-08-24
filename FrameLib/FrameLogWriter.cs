using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.Property;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 画面操作のログを出力するためのログ出力クラスです。
    /// </summary>
    public class FrameLogWriter
    {
        /// <summary>
        /// ロギング時の操作種類を表す列挙体です。
        /// </summary>
        public enum LoggingOperateKind : int
        {
            /// <summary>
            /// 画面の起動
            /// </summary>
            ShownFrame = 1,
            /// <summary>
            /// 画面の終了
            /// </summary>
            CloseFrame = 2,
            /// <summary>
            /// データの登録（新規作成）
            /// </summary>
            NewItem = 3,
            /// <summary>
            /// データの修正
            /// </summary>
            UpdateItem = 4,
            /// <summary>
            /// データの削除
            /// </summary>
            DelItem = 5,
            /// <summary>
            /// 印刷
            /// </summary>
            PrintPrinting = 6,
            /// <summary>
            /// プレビュー
            /// </summary>
            PrintPreview = 7,
            /// <summary>
            /// 処理の実行（計算や集計などの一括処理）
            /// </summary>
            ProcessTask = 8,
            /// <summary>
            /// コード変更
            /// </summary>
            ChangeCode = 9,
            /// <summary>
            /// FAX
            /// </summary>
            PrintFax = 10,
            /// <summary>
            /// ＣＳＶ出力
            /// </summary>
            FileOutPut = 11,
        }

        /// <summary>
        /// 認証情報の保持領域
        /// </summary>
        private AppAuthInfo _authInfo;

        /// <summary>
        /// コンストラクタのプライベート化
        /// </summary>
        private FrameLogWriter()
        { }



        /// <summary>
        /// 認証情報を指定して、ログ出力クラスをインスタンス化して取得します。
        /// </summary>
        /// <param name="authInfo">認証情報</param>
        /// <returns>ログ出力クラスのインスタンス</returns>
        public static FrameLogWriter GetLogger(AppAuthInfo authInfo)
        {
            FrameLogWriter.CreateInstance();

            FrameLogWriter.myInstance._authInfo = authInfo;
            return FrameLogWriter.myInstance;
        }

        /// <summary>
        /// 操作種類を指定して、ログ出力を行います。
        /// </summary>
        /// <param name="kind">操作種類をあらわす列挙体の値</param>
        public void LoggingLog(LoggingOperateKind kind)
        {
            this.LoggingLog(kind, string.Empty);
        }


        /// <summary>
        /// 操作種類と操作条件文字列を指定して、ログ出力を行います。
        /// </summary>
        /// <param name="kind">操作種類を表す列挙体の値</param>
        /// <param name="jyoken">操作条件文字列</param>
        public void LoggingLog(LoggingOperateKind kind, string jyoken)
        {

            //ログ出力要素を作成
            LoggingProcessItemModel model =
                new LoggingProcessItemModel
                {
                    LogSrcAuthInfo = this._authInfo,
                    OperateKindString = GetOperateKindString(kind),
                    RemarkText = jyoken
                };

            //スレッドプールで実行するためにコールバックを定義
            WaitCallback wc = new WaitCallback(WrittingLog);

            //スレッドプールのキューにコールバックと状態としてmodelを渡す
            ThreadPool.QueueUserWorkItem(wc, model);
        }


        /// <summary>
        /// 本クラスのインスタンスを静的に保持します。
        /// </summary>
        private static FrameLogWriter myInstance;

        /// <summary>
        /// 本クラスのインスタンスを作成します。
        /// </summary>
        private static void CreateInstance()
        {
            //すでに作成してある場合は作らない
            if (FrameLogWriter.myInstance == null)
            {
                FrameLogWriter.myInstance = new FrameLogWriter();
            }
        }


        /// <summary>
        /// ロギング時の操作種類を表す列挙体を指定して、
        /// 対応する文字列表記を取得します。
        /// </summary>
        /// <param name="kind">ロギング時の操作種類を表す列挙体。</param>
        /// <returns>対応する文字列表記</returns>
        public static string GetOperateKindString(LoggingOperateKind kind)
        {
            string rt_val = string.Empty;

            switch (kind)
            {
                case LoggingOperateKind.ShownFrame:
                    rt_val = "画面の起動";
                    break;
                case LoggingOperateKind.CloseFrame:
                    rt_val = "画面の終了";
                    break;
                case LoggingOperateKind.NewItem:
                    rt_val = "データの登録";
                    break;
                case LoggingOperateKind.UpdateItem:
                    rt_val = "データの修正";
                    break;
                case LoggingOperateKind.DelItem:
                    rt_val = "データの削除";
                    break;
                case LoggingOperateKind.PrintPrinting:
                    rt_val = "印刷";
                    break;
                case LoggingOperateKind.PrintPreview:
                    rt_val = "プレビュー";
                    break;
                case LoggingOperateKind.ProcessTask:
                    rt_val = "処理の実行";
                    break;
                case LoggingOperateKind.ChangeCode:
                    rt_val = "コード変更";
                    break;
                case LoggingOperateKind.PrintFax:
                    rt_val = "FAX";
                    break;
                case LoggingOperateKind.FileOutPut:
                    rt_val = "ＣＳＶ出力";
                    break;
                default:
                    break;
            }

            return rt_val;
        }

        /// <summary>
        /// ログ出力要素を表すインスタンスを指定して、ログ出力処理を
        /// 開始します。
        /// </summary>
        /// <param name="state"></param>
        private static void WrittingLog(object state)
        {

            try
            {
                LoggingProcessItemModel model = new LoggingProcessItemModel();

                //stateを復元する
                model = (LoggingProcessItemModel)state;

#if DEBUG
                //Debug Log
                Console.WriteLine("WriteLog:" + model.OperateKindString + " " + model.RemarkText);
#endif

                //操作履歴出力のためのBLLのインスタンスを作る
                OperateHistory bll = new OperateHistory();


                //*** s.arimura 2011/11/09 無効に
                ////NSK管理用ログインの場合は、ユーザ情報を強制的に書き換える
                //if (Property.UserProperty.GetInstance().NSKAdminLoginFlag)
                //{
                //    //DBに作成している管理用のアカウントのIDが999999999999999999
                //    //なので決め打ち
                //    model.LogSrcAuthInfo.OperatorId = 999999999999999999;
                //}

                //追加 T.kuroki 20111109
                // 管理者ログイン時のユーザ偽装を行わないため、オペレータIDが無い
                // この状態では、オペレータ名がログに出力できないので、ログを書く
                // メソッドの手前でAppAuthInfoにオペレータ名を管理者のときのみ設
                // 定する。
                if (Property.UserProperty.GetInstance().NSKAdminLoginFlag)
                {
                    model.LogSrcAuthInfo.OperatorId = 0;
                    model.LogSrcAuthInfo.OperetorName = "NSK管理者";
                }
                else
                {
                    model.LogSrcAuthInfo.OperetorName =
                        Property.UserProperty.GetInstance().LoginOperatorName;
                }

                bll.SetLoggingData(model.LogSrcAuthInfo, model.OperateKindString,
                    model.RemarkText);

            }
            catch (Exception err)
            {
                //Debug T.Kuroki 
                //後で実装する。
                Console.WriteLine("ログ出力エラー発生" + err.Message);
            }
        }

        /// <summary>
        /// ログ出力を実行するスレッドプールで実行するメソッドに渡す値を保持するための
        /// クラスです。
        /// </summary>
        private class LoggingProcessItemModel
        {
            /// <summary>
            /// ログ出力要素とするアプリケーション認証情報を取得・設定します。
            /// </summary>
            public AppAuthInfo LogSrcAuthInfo { get; set; }
            /// <summary>
            /// 操作種別を表す文字列を取得・設定します。
            /// </summary>
            public string OperateKindString { get; set; }
            /// <summary>
            /// ログ出力適用項目を取得・設定します。
            /// </summary>
            public string RemarkText { get; set; }
        }
    }
}
