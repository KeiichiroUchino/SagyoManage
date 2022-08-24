using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.BizProperty
{
    public class ShohizeiCalculator
    {
        /// <summary>
        /// 外税の金額から消費税を計算します。
        /// </summary>
        /// <param name="kingaku">外税の金額</param>
        /// <param name="zeiritsu">税率（8%の場合、0.08で指定）</param>
        /// <param name="marumeKbn">丸め区分</param>
        /// <returns>消費税の計算結果情報</returns>
        public static ShohizeiCalculatorResult CalcBySoto(decimal kingaku, decimal zeiritsu, int marumeKbn)
        {
            decimal shohizei =  MarumeNumberCalculator.Marume(marumeKbn, kingaku * zeiritsu);

            return new ShohizeiCalculatorResult()
            {
                Hontai = kingaku,
                Shohizei = shohizei,
                KazeiTaisho = kingaku
            };
        }

        /// <summary>
        /// 内税の金額から消費税を計算します。
        /// </summary>
        /// <param name="kingaku">内税の金額</param>
        /// <param name="zeiritsu">税率（8%の場合、0.08で指定）</param>
        /// <param name="marumeKbn">丸め区分</param>
        /// <returns>消費税の計算結果情報</returns>
        public static ShohizeiCalculatorResult CalcByUchi(decimal kingaku, decimal zeiritsu, int marumeKbn)
        {
            decimal shohizei = MarumeNumberCalculator.Marume(marumeKbn, (kingaku / (1 + zeiritsu)) * zeiritsu);

            return new ShohizeiCalculatorResult()
            {
                Hontai = kingaku - shohizei,
                Shohizei = shohizei,
                KazeiTaisho = kingaku - shohizei
            };
        }

        /// <summary>
        /// 税区分に応じて消費税の計算結果を返します。
        /// </summary>
        /// <param name="zeiKbn">税区分</param>
        /// <param name="kingaku">金額</param>
        /// <param name="zeiritsu">税率（8%の場合、0.08で指定）</param>
        /// <param name="marumeKbn">丸め区分</param>
        /// <returns></returns>
        public static ShohizeiCalculatorResult Calc(int zeiKbn, decimal kingaku, decimal zeiritsu, int marumeKbn)
        {
            ShohizeiCalculatorResult rt_val = new ShohizeiCalculatorResult();

            switch ((BizProperty.DefaultProperty.ZeiKbn)zeiKbn)
            {
                case DefaultProperty.ZeiKbn.Sotozei:
                    rt_val = CalcBySoto(kingaku, zeiritsu, marumeKbn);
                    break;
                case DefaultProperty.ZeiKbn.Uchizei:
                    rt_val = CalcByUchi(kingaku, zeiritsu, marumeKbn);
                    break;
                case DefaultProperty.ZeiKbn.Hikazei:
                    rt_val.Hontai = kingaku;
                    break;
                //case DefaultProperty.ZeiKbn.ShohinSansho:
                //    break;
                default:
                    break;
            }

            return rt_val;
        }
    }

    public struct ShohizeiCalculatorResult
    {
        /// <summary>
        /// 本体金額（内税の場合は、税抜です）
        /// </summary>
        public decimal Hontai { get; set; }
        /// <summary>
        /// 消費税
        /// </summary>
        public decimal Shohizei { get; set; }
        /// <summary>
        /// 税込金額
        /// </summary>
        public decimal Zeikomi { get { return this.Hontai + this.Shohizei; } }
        /// <summary>
        /// 課税対象額
        /// </summary>
        public decimal KazeiTaisho { get; set; }
    }
}
