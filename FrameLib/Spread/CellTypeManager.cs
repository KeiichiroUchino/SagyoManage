using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jpsys.SagyoManage.FrameLib.Spread.Cells;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// </summary>
    public class CellTypeSettingAction
    {
        /// <summary>
        /// </summary>
        public Type CellType;
        /// <summary>
        /// </summary>
        public Action<object> Setting;
    }

    /// <summary>
    /// セルに設定する値の型とセル型の紐づけを管理します。
    /// </summary>
    public static class CellTypeManager
    {
        private static readonly Dictionary<Type, Type> Type_CellTypeType;
        private static readonly Dictionary<Type, Action<object>> Type_CellTypeSetting;


        static CellTypeManager()
        {
            Type_CellTypeType = new Dictionary<Type, Type>();
            Type_CellTypeSetting = new Dictionary<Type, Action<object>>();

            RegisterCellTypes();
        }

        /// <summary>
        /// セル型の登録処理をします。
        /// </summary>
        private static void RegisterCellTypes()
        {
            //数値
            RegisterCellType<Int16, CustomNumberCellType>();
            RegisterCellType<Int32, CustomNumberCellType>();
            RegisterCellType<Int64, CustomNumberCellType>();
            RegisterCellType<Double, CustomNumberCellType>();
            RegisterCellType<Single, CustomNumberCellType>();
            RegisterCellType<Decimal, CustomNumberCellType>();
            //** Nullable
            RegisterCellType<Int16?, CustomNumberCellType>();
            RegisterCellType<Int32?, CustomNumberCellType>();
            RegisterCellType<Int64?, CustomNumberCellType>();
            RegisterCellType<Double?, CustomNumberCellType>();
            RegisterCellType<Single?, CustomNumberCellType>();
            RegisterCellType<Decimal?, CustomNumberCellType>();

            //日付
            RegisterCellType<DateTime, CustomDateTimeCellType>();
            //** Nullable
            RegisterCellType<DateTime?, CustomDateTimeCellType>();
            //TimeSpan
            RegisterCellType<TimeSpan,　FarPoint.Win.Spread.CellType.DateTimeCellType>(
                cellType =>
                {
                    cellType.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined;
                    cellType.UserDefinedFormat = "HH:mm";
                });
            //** Nullable
            RegisterCellType<TimeSpan?, FarPoint.Win.Spread.CellType.DateTimeCellType>(
                cellType =>
                {
                    cellType.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined;
                    cellType.UserDefinedFormat = "HH:mm";
                });


            //bool
            RegisterCellType<bool, FarPoint.Win.Spread.CellType.CheckBoxCellType>();
        }

        #region CellTypeの登録

        /// <summary>
        /// 列に設定する値のTypeオブジェクトと対応するセル型を表すICellTypeオブジェクトを登録します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCell"></typeparam>
        private static void RegisterCellType<T, TCell>() where TCell : FarPoint.Win.Spread.CellType.ICellType
        {
            Type_CellTypeType.Add(typeof(T), typeof(TCell));
        }

        /// <summary>
        /// 列に設定する値のTypeオブジェクトと対応するセル型を表すICellTypeオブジェクトを登録します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCell"></typeparam>
        /// <param name="settingAction">CellTypeオブジェクトに設定する内容</param>
        private static void RegisterCellType<T, TCell>(Action<TCell> settingAction) where TCell : FarPoint.Win.Spread.CellType.ICellType
        {
            Type_CellTypeType.Add(typeof(T), typeof(TCell));
            Type_CellTypeSetting.Add(typeof(T), (object cellType) => settingAction((TCell)cellType));
        }

        #endregion

        /// <summary>
        /// </summary>
        public static FarPoint.Win.Spread.CellType.ICellType GetCellType(Type type)
        {
            Type cellTypeType = null;

            if (Type_CellTypeType.TryGetValue(type, out cellTypeType))
            {
                var cellType = (FarPoint.Win.Spread.CellType.ICellType)Activator.CreateInstance(cellTypeType);

                //***設定情報があれば設定する
                Action<object> setting = null;
                if (Type_CellTypeSetting.TryGetValue(type, out setting))
                {
                    setting(cellType);
                }

                return cellType;
            }
            else
            {
                return new FarPoint.Win.Spread.CellType.GeneralCellType();
            }
        }
    }
}
