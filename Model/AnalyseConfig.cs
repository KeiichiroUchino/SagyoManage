using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    public class AnalyseShowSetting
    {
        public string ColumnName { get; set; } // カラム名
        public int? DecimalPlaces { get; set; } // 小数点以下何桁まで表示するか
        public bool? ShowSeparatorFlag { get; set; } // ３桁区切りにするかどうか
        public bool? ShowTimeFlag { get; set; } // true:日付+時刻を表示 false:日付だけ表示
        public bool? SummableFlag { get; set; } // true:日付+時刻を表示 false:日付だけ表示

        public AnalyseShowSetting(string ColumnName, int? DecimalPlaces, bool? ShowSeparatorFlag, bool? ShowTimeFlag, bool? SummableFlag = null) {
            this.ColumnName = ColumnName;
            this.DecimalPlaces = DecimalPlaces;
            this.ShowSeparatorFlag = ShowSeparatorFlag;
            this.ShowTimeFlag = ShowTimeFlag;
            this.SummableFlag = SummableFlag;
        }
    }

    [Serializable]
    public class DataType
    {
        /// <summary>
        /// 売上や入金などコンボボックスに表示される名前です
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// uriageview,nyukinviewなど参照先のテーブル名です
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 解析ツール画面右側に表示するカラム名のリストです 
        /// </summary>
        public List<string> ColumnNameList { get; set; }

        /// <summary>
        /// 取引日、請求日、入力日など対象日コンボボックスに表示する対象日のリストです
        /// </summary>
        public List<string> TargetDateList { get; set; } 

        public DataType(string DisplayName, string TableName, List<string> ColumnNameList, List<string> TargetDateList) {
            this.DisplayName = DisplayName;
            this.TableName = TableName;
            this.ColumnNameList = ColumnNameList;
            this.TargetDateList = TargetDateList;
        }
    }

    /// <summary>
    /// 解析ツール画面右側のMRowのRowの状態を保持するモデルです。
    /// </summary>
    [Serializable]
    public class ColumnConfig
    {
        public string ColumnName { get; set; }
        public bool IsChecked { get; set; }
        public string SortOrder { get; set; }
        public int? SortPriority { get; set; }
        public bool IsSum { get; set; }

        public ColumnConfig(string ColumnName) :
            this(ColumnName,false,"",null,false){
        }

        public ColumnConfig(string ColumnName, bool IsChecked, string SortOrder, int? SortPriority, bool IsSum) {
            this.ColumnName = ColumnName;
            this.IsChecked = IsChecked;
            this.SortOrder = SortOrder;
            this.SortPriority = SortPriority;
            this.IsSum = IsSum;
        }
    }

    /// <summary>
    /// 解析ツール画面全体の状態を保持するモデルです
    /// </summary>
    [Serializable]
    public class AnalyseConfig
    {
        public decimal ID;
        public string ConfigName;
        public DataType DataType;
        public string TargetDate;
        public Dictionary<string,string[]> CodeRangeConfig;
        public List<ColumnConfig> ColumnConfigList;
        
        public AnalyseConfig(string ConfigName, DataType DataType, string TargetDate, 
            List<ColumnConfig> ColumnConfigList, Dictionary<string,string[]> CodeRangeConfig) {
            this.ConfigName = ConfigName;
            this.DataType = DataType;
            this.TargetDate = TargetDate;
            this.ColumnConfigList = ColumnConfigList;
            this.CodeRangeConfig = CodeRangeConfig;
        }
    }

    /// <summary>
    /// 解析ツールの画面全体の状態を保持するモデルをシリアライズした物をメンバーに持つモデルです。
    /// メンバーのSerializedDataにはAnalyseConfigモデルがZIP圧縮されてbyte[]配列になり入っている、
    /// というイメージです。
    /// </summary>
    public class SerializedAnalyseConfig : AbstractTimeStampModelBase
    {
        public decimal? ID;
        public string ConfigName;
        public byte[] SerializedData;

        public SerializedAnalyseConfig(decimal? ID, string ConfigName, byte[] SerializedData){
            this.ID = ID;
            this.ConfigName = ConfigName;
            this.SerializedData = SerializedData;
        }
    }

}
