using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.Model
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ModelClassAttribute : Attribute
    {

        private DBType _dbtype;
        private string _dbtablename;

        private string _dispname;

        public ModelClassAttribute(string dbcolname)
        {
            _dbtype = ModelClassAttribute.DBType.SQL;
            _dbtablename = dbcolname;
            _dispname = "";
        }

        public ModelClassAttribute(string dbcolname, string dispname)
            : this(dbcolname)
        {
            _dbtype = ModelClassAttribute.DBType.SQL;
            _dispname = dispname;
        }

        public ModelClassAttribute(string dbcolname, string dispname, ModelClassAttribute.DBType dbtype)
            : this(dbcolname)
        {
            _dbtype = dbtype;
            _dispname = dispname;
        }

        public DBType DatabaseType { get { return this._dbtype; } }
        public string DatabaseTableName { get { return this._dbtablename; } }
        public string DisplayName { get { return this._dispname; } }

        public enum DBType : int
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// SQL
            /// </summary>
            SQL = 1,
            /// <summary>
            /// アクセス
            /// </summary>
            Access = 2,

        }

    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ModelPropertyAttribute : Attribute
    {
        private string _dbcolname;

        private ModelPropertyAttributeType _type;
        private ModelPropertyConvertAttribute _converttype;


        public ModelPropertyAttribute(string dbcolname)
        {
            _dbcolname = dbcolname;
            _type = ModelPropertyAttributeType.None;
            _converttype = ModelPropertyConvertAttribute.None;
        }

        public ModelPropertyAttribute(string dbcolname,
            ModelPropertyAttributeType type)
            : this(dbcolname)
        {
            _type = type;
        }

        public ModelPropertyAttribute(string dbcolname,
            ModelPropertyAttributeType type,
            ModelPropertyConvertAttribute convertType)
            : this(dbcolname, type)
        {
            _converttype = convertType;
        }

        public string DatabaseColName { get { return this._dbcolname; } }

        public ModelPropertyAttributeType ModelPropertyType { get { return this._type; } }

        public ModelPropertyConvertAttribute ModelPropertyConvertType { get { return this._converttype; } }


        public static PropertyInfo GetPropInfoByPropType<T>(ModelPropertyAttributeType type)
            where T : class,
            new()
        {
            PropertyInfo rt_info = null;

            //抽出したデータから資格取得情報を作成
            T info = new T();

            PropertyInfo[] infoArray = typeof(T).GetProperties();

            // プロパティ情報出力をループで回す
            foreach (PropertyInfo prop in infoArray)
            {
                var colAtr =
                    ReflectionUtil.GetAttributeByPropertyInfo<ModelPropertyAttribute>(prop);

                if (colAtr != null)
                {

                    if (colAtr.ModelPropertyType.HasFlag(type))
                    {
                        rt_info = prop;
                    }
                }

            }

            return rt_info;

        }

        public static object GetValueByPropType<T>(T info, ModelPropertyAttributeType type)
            where T : class,
            new()
        {

            PropertyInfo[] infoArray = typeof(T).GetProperties();

            // プロパティ情報出力をループで回す
            foreach (PropertyInfo prop in infoArray)
            {
                var colAtr =
                    ReflectionUtil.GetAttributeByPropertyInfo<ModelPropertyAttribute>(prop);

                if (colAtr != null)
                {

                    if (colAtr.ModelPropertyType.HasFlag(type))
                    {
                        return prop.GetValue(info);
                    }
                }

            }

            throw new NotImplementedException(typeof(T).Name + "に対してModelPropertyAttributeType=" + type.ToString() + "が設定されていません");

        }

    }


    //0	0	0x00000000
    //1	1	0x00000001
    //2	2	0x00000002
    //3	4	0x00000004
    //4	8	0x00000008
    //5	16	0x00000010
    //6	32	0x00000020
    //7	64	0x00000040
    //8	128	0x00000080
    //9	256	0x00000100
    //10	512	0x00000200
    //11	1024	0x00000400
    //12	2048	0x00000800
    //13	4096	0x00001000
    //14	8192	0x00002000
    //15	16384	0x00004000
    //16	32768	0x00008000
    //17	65536	0x00010000
    //18	131072	0x00020000
    //19	262144	0x00040000
    //20	524288	0x00080000
    //21	1048576	0x00100000
    //22	2097152	0x00200000
    //23	4194304	0x00400000
    //24	8388608	0x00800000
    //25	16777216	0x01000000
    //26	33554432	0x02000000
    //27	67108864	0x04000000
    //28	134217728	0x08000000
    //29	268435456	0x10000000
    //30	536870912	0x20000000
    //31	1073741824	0x40000000
    //32	2147483648	0x80000000



    [Flags]
    public enum ModelPropertyAttributeType : int
    {
        /// <summary>
        /// 変換なし
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// 主キー
        /// </summary>
        IsPrimarykey = 0x00000001,

        /// <summary>
        /// マスターのコード
        /// </summary>
        MasterCode = 0x00000010,
        /// <summary>
        /// マスターの名前
        /// </summary>
        MasterName = 0x00000020,
        /// <summary>
        /// マスターの略称
        /// </summary>
        MasterShortName = 0x00000040,
        /// <summary>
        /// マスターのカナ
        /// </summary>
        MasterKana = 0x00000080,
        /// <summary>
        /// マスターの非表示フラグ
        /// </summary>
        MasterDisableFlag = 0x00000100,

    }

    //0,1,2,4,8
    [Flags]
    public enum ModelPropertyConvertAttribute : int
    {
        /// <summary>
        /// 変換なし
        /// </summary>
        None = 0x00000000,


        ModeleToDatabaseAsInt32 = 0x00000001,


        //ModeleToDatabaseAsEnum = 0x000000xx,
    }


    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ModelPropertyRelateAttribute : Attribute
    {
        private string _dbfkcolname;

        public ModelPropertyRelateAttribute(string dbfkcolname)
        {
            _dbfkcolname = dbfkcolname;
        }

        public string RelateKeyPropertyName { get { return this._dbfkcolname; } }
    }
}
