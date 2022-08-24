using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Jpsys.SagyoManage.Frame.EventData
{
    /// <summary>
    /// 帳票印刷に使用するデータの作成終了を通知するイベントのイベントデータ
    /// のクラスです。
    /// </summary>
    public class ReportDataCreatedEventArgs : System.EventArgs
    {
        //データの保持用
        private IList[] _reportDataListArray =
            (IList[])(new ArrayList()).ToArray(typeof(IList));

        //例外の保持用
        private Exception _reportErr = null;

        //例外の存在有無
        private bool _hasReportErr = false;

        /// <summary>
        /// 帳票印刷に使用するデータを格納したリストの配列を指定して
        /// イベントデータのインスタンスを初期化します。
        /// 通常はリストが１つだけ格納していますが、サブレーポートのように、リストを
        /// 複数使用するには、複数のリストを格納します。
        /// </summary>
        /// <param name="reportDataListArray">帳票印刷に使用するデータを格納したリストの配列</param>
        public ReportDataCreatedEventArgs(IList[] reportDataListArray)
        {
            this._reportDataListArray = reportDataListArray;
            this._hasReportErr = false;
        }

        /// <summary>
        /// 帳票印刷に必要なデータ作成中に発生した例外の例外オブジェクトを指定して
        /// イベントデータのインスタンスを初期化します。
        /// このコンストラクタは、例外が発生したときのみ使用します。
        /// </summary>
        /// <param name="reportError">帳票印刷に必要なデータ作成中に発生した例外</param>
        public ReportDataCreatedEventArgs(Exception reportError)
        {
            this._reportErr = reportError;
            this._hasReportErr = true;
        }


        /// <summary>
        /// ビジネスロジックなどで作成された帳票印刷に必要なデータのリストを
        /// 格納した配列を取得します。
        /// </summary>
        public IList[] ReportDataListArray
        {
            get { return this._reportDataListArray; }
        }

        /// <summary>
        /// 帳票印刷に必要なデータを作成中に例外が発生した場合その例外を取得します。
        /// </summary>
        public Exception ReportError
        {
            get { return this._reportErr; }
        }

        /// <summary>
        /// 帳票印刷に必要なデータを作成中に例外が発生したかどうかを取得します。
        /// 例外が発生している場合にはReportErrorプロパティから発生した例外の
        /// オブジェクトを取得して適切に処理してください。
        /// （true:例外が発生している）
        /// </summary>
        public bool HasReportError
        {
            get { return this._hasReportErr; }
        }
    }
}
