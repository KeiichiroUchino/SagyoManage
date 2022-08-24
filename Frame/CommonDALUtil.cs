using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.Property;

namespace Jpsys.SagyoManage.Frame
{
    /// <summary>
    /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスです。
    /// </summary>
    public class CommonDALUtil
    {
        #region ユーザ定義

        /// <summary>
        /// ビジネスロジックの操作に使用する認証情報
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// DALのロケーター
        /// </summary>
        private readonly DALLocator Locator;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// (プライベート化)
        /// </summary>
        private CommonDALUtil()
        {
        }

        /// <summary>
        /// 認証アプリケーション情報を指定して本クラスの
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="curAppAuth"></param>
        public CommonDALUtil(AppAuthInfo curAppAuth)
        {
            //認証情報をローカル変数に設定
            this.appAuth = curAppAuth;

            this.Locator = new DALLocator(curAppAuth);
        }

        #endregion

        #region パブリックメソッド

        #region 共通テーブル関係

        /// <summary>
        /// システム名称のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SystemName _systemGlobalName = null;

        public SystemName SystemGlobalName
        {
            get
            {
                if (_systemGlobalName == null)
                {
                    _systemGlobalName = new SystemName();
                }

                return _systemGlobalName;
            }
        }

        /// <summary>
        /// 操作履歴のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private OperateHistory _operateHistory = null;

        public OperateHistory OperateHistory
        {
            get
            {
                if (_operateHistory == null)
                {
                    _operateHistory = new OperateHistory(this.appAuth);
                }

                return _operateHistory;
            }
        }

        /// <summary>
        /// 利用者のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Operator _operator = null;

        public Operator Operator
        {
            get
            {
                if (_operator == null)
                {
                    _operator = new Operator(this.appAuth);
                }

                return _operator;
            }
        }

        ///// <summary>
        ///// 郵便番号のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private PostalZip _postalZip = null;

        //public PostalZip PostalZip
        //{
        //    get
        //    {
        //        if (_postalZip == null)
        //        {
        //            _postalZip = new PostalZip(this.appAuth);
        //        }

        //        return _postalZip;
        //    }
        //}

        /// <summary>
        /// 管理のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SQLServerDAL.SystemProperty _SystemProperty = null;

        public SQLServerDAL.SystemProperty SystemProperty
        {
            get
            {
                if (_SystemProperty == null)
                {
                    _SystemProperty = new SQLServerDAL.SystemProperty(this.appAuth);
                }

                return _SystemProperty;
            }
        }

        ///// <summary>
        ///// メール管理のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private MailKanri _MailKanri = null;

        //public MailKanri MailKanri
        //{
        //    get
        //    {
        //        if (_MailKanri == null)
        //        {
        //            _MailKanri = new MailKanri(this.appAuth);
        //        }

        //        return _MailKanri;
        //    }
        //}

        ///// <summary>
        ///// 消費税率情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private TaxRate _taxRate = null;
        ///// <summary>
        ///// 消費税率情報のデータアクセスオブジェクト
        ///// </summary>
        //public TaxRate TaxRate
        //{
        //    get
        //    {
        //        if (_taxRate == null)
        //        {
        //            _taxRate = new TaxRate(this.appAuth);
        //        }
        //        return _taxRate;
        //    }
        //}

        ///// <summary>
        ///// 権限名称情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private KengenName _kengenName = null;
        ///// <summary>
        ///// 権限名称情報のデータアクセスオブジェクト
        ///// </summary>
        //public KengenName KengenName
        //{
        //    get
        //    {
        //        if (_kengenName == null)
        //        {
        //            _kengenName = new KengenName(this.appAuth);
        //        }
        //        return _kengenName;
        //    }
        //}

        ///// <summary>
        ///// 排他制御のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ExclusiveControl _ExclusiveControl = null;

        //public ExclusiveControl ExclusiveControl
        //{
        //    get
        //    {
        //        if (_ExclusiveControl == null)
        //        {
        //            _ExclusiveControl = new ExclusiveControl(this.appAuth);
        //        }

        //        return _ExclusiveControl;
        //    }
        //}

        #endregion

        ///// <summary>
        ///// 品目大分類情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ItemLBunrui _ItemLBunrui = null;
        ///// <summary>
        ///// 品目大分類情報のデータアクセスオブジェクト
        ///// </summary>
        //public ItemLBunrui ItemLBunrui
        //{
        //    get
        //    {
        //        if (_ItemLBunrui == null)
        //        {
        //            _ItemLBunrui = new ItemLBunrui(this.appAuth);
        //        }
        //        return _ItemLBunrui;
        //    }
        //}

        ///// <summary>
        ///// 品目中分類情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ItemMBunrui _ItemMBunrui = null;
        ///// <summary>
        ///// 品目中大分類情報のデータアクセスオブジェクト
        ///// </summary>
        //public ItemMBunrui ItemMBunrui
        //{
        //    get
        //    {
        //        if (_ItemMBunrui == null)
        //        {
        //            _ItemMBunrui = new ItemMBunrui(this.appAuth);
        //        }
        //        return _ItemMBunrui;
        //    }
        //}

        ///// <summary>
        ///// 発着地大分類情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private PointLBunrui _PointLBunrui = null;
        ///// <summary>
        ///// 発着地大分類情報のデータアクセスオブジェクト
        ///// </summary>
        //public PointLBunrui PointLBunrui
        //{
        //    get
        //    {
        //        if (_PointLBunrui == null)
        //        {
        //            _PointLBunrui = new PointLBunrui(this.appAuth);
        //        }
        //        return _PointLBunrui;
        //    }
        //}

        ///// <summary>
        ///// 発着地中分類情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private PointMBunrui _PointMBunrui = null;
        ///// <summary>
        ///// 発着地中大分類情報のデータアクセスオブジェクト
        ///// </summary>
        //public PointMBunrui PointMBunrui
        //{
        //    get
        //    {
        //        if (_PointMBunrui == null)
        //        {
        //            _PointMBunrui = new PointMBunrui(this.appAuth);
        //        }
        //        return _PointMBunrui;
        //    }
        //}

        /// <summary>
        /// 車両情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Car _Car = null;
        /// <summary>
        /// 車両情報のデータアクセスオブジェクト
        /// </summary>
        public Car Car
        {
            get
            {
                if (_Car == null)
                {
                    _Car = new Car(this.appAuth);
                }
                return _Car;
            }
        }

        /// <summary>
        /// 名称情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Meisho _Meisho = null;
        /// <summary>
        /// 名称情報のデータアクセスオブジェクト
        /// </summary>
        public Meisho Meisho
        {
            get
            {
                if (_Meisho == null)
                {
                    _Meisho = new Meisho(this.appAuth);
                }
                return _Meisho;
            }
        }

        ///// <summary>
        ///// 車種（トラDON補）情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private CarKind _CarKind = null;
        ///// <summary>
        ///// 車種（トラDON補）情報のデータアクセスオブジェクト
        ///// </summary>
        //public CarKind CarKind
        //{
        //    get
        //    {
        //        if (_CarKind == null)
        //        {
        //            _CarKind = new CarKind(this.appAuth);
        //        }
        //        return _CarKind;
        //    }
        //}

        ///// <summary>
        ///// 品目（トラDON補）情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Item _Item = null;
        ///// <summary>
        ///// 品目（トラDON補）情報のデータアクセスオブジェクト
        ///// </summary>
        //public Item Item
        //{
        //    get
        //    {
        //        if (_Item == null)
        //        {
        //            _Item = new Item(this.appAuth);
        //        }
        //        return _Item;
        //    }
        //}

        /// <summary>
        /// 得意先情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Tokuisaki _Tokuisaki = null;
        /// <summary>
        /// 得意先情報のデータアクセスオブジェクト
        /// </summary>
        public Tokuisaki Tokuisaki
        {
            get
            {
                if (_Tokuisaki == null)
                {
                    _Tokuisaki = new Tokuisaki(this.appAuth);
                }
                return _Tokuisaki;
            }
        }

        /// <summary>
        /// 作業場所情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SagyoBasho _SagyoBasho = null;
        /// <summary>
        /// 作業場所情報のデータアクセスオブジェクト
        /// </summary>
        public SagyoBasho SagyoBasho
        {
            get
            {
                if (_SagyoBasho == null)
                {
                    _SagyoBasho = new SagyoBasho(this.appAuth);
                }
                return _SagyoBasho;
            }
        }

        /// <summary>
        /// 契約情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Keiyaku _Keiyaku = null;
        /// <summary>
        /// 契約情報のデータアクセスオブジェクト
        /// </summary>
        public Keiyaku Keiyaku
        {
            get
            {
                if (_Keiyaku == null)
                {
                    _Keiyaku = new Keiyaku(this.appAuth);
                }
                return _Keiyaku;
            }
        }

        /// <summary>
        /// 社員情報のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private Staff _Staff = null;
        /// <summary>
        /// 社員情報のデータアクセスオブジェクト
        /// </summary>
        public Staff Staff
        {
            get
            {
                if (_Staff == null)
                {
                    _Staff = new Staff(this.appAuth);
                }
                return _Staff;
            }
        }

        ///// <summary>
        ///// 発着地グループ（トラDON補）情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private PointGroup _PointGroup = null;
        ///// <summary>
        ///// 発着地グループ（トラDON補）情報のデータアクセスオブジェクト
        ///// </summary>
        //public PointGroup PointGroup
        //{
        //    get
        //    {
        //        if (_PointGroup == null)
        //        {
        //            _PointGroup = new PointGroup(this.appAuth);
        //        }
        //        return _PointGroup;
        //    }
        //}

        ///// <summary>
        ///// 発着地（トラDON）情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Point _Point = null;
        ///// <summary>
        ///// 発着地（トラDON）情報のデータアクセスオブジェクト
        ///// </summary>
        //public Point Point
        //{
        //    get
        //    {
        //        if (_Point == null)
        //        {
        //            _Point = new Point(this.appAuth);
        //        }
        //        return _Point;
        //    }
        //}

        ///// <summary>
        ///// 取引先（トラDON）情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Torihikisaki _Torihikisaki = null;
        ///// <summary>
        ///// 取引先（トラDON）情報のデータアクセスオブジェクト
        ///// </summary>
        //public Torihikisaki Torihikisaki
        //{
        //    get
        //    {
        //        if (_Torihikisaki == null)
        //        {
        //            _Torihikisaki = new Torihikisaki(this.appAuth);
        //        }
        //        return _Torihikisaki;
        //    }
        //}

        ///// <summary>
        ///// 方面情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Homen _Homen = null;
        ///// <summary>
        ///// 方面情報のデータアクセスオブジェクト
        ///// </summary>
        //public Homen Homen
        //{
        //    get
        //    {
        //        if (_Homen == null)
        //        {
        //            _Homen = new Homen(this.appAuth);
        //        }
        //        return _Homen;
        //    }
        //}

        ///// <summary>
        ///// 配色情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Haishoku _Haishoku = null;
        ///// <summary>
        ///// 配色情報のデータアクセスオブジェクト
        ///// </summary>
        //public Haishoku Haishoku
        //{
        //    get
        //    {
        //        if (_Haishoku == null)
        //        {
        //            _Haishoku = new Haishoku(this.appAuth);
        //        }
        //        return _Haishoku;
        //    }
        //}

        ///// <summary>
        ///// トラDON請求部門情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDONClmClass _ToraDONClmClass = null;
        ///// <summary>
        ///// トラDON請求部門情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDONClmClass ToraDONClmClass
        //{
        //    get
        //    {
        //        if (_ToraDONClmClass == null)
        //        {
        //            _ToraDONClmClass = new ToraDONClmClass(this.appAuth);
        //        }
        //        return _ToraDONClmClass;
        //    }
        //}

        ///// <summary>
        ///// トラDON請負情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDONContract _ToraDONContract = null;
        ///// <summary>
        ///// トラDON請負情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDONContract ToraDONContract
        //{
        //    get
        //    {
        //        if (_ToraDONContract == null)
        //        {
        //            _ToraDONContract = new ToraDONContract(this.appAuth);
        //        }
        //        return _ToraDONContract;
        //    }
        //}

        ///// <summary>
        ///// トラDON単位情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDONFig _ToraDONFig = null;
        ///// <summary>
        ///// トラDON単位情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDONFig ToraDONFig
        //{
        //    get
        //    {
        //        if (_ToraDONFig == null)
        //        {
        //            _ToraDONFig = new ToraDONFig(this.appAuth);
        //        }
        //        return _ToraDONFig;
        //    }
        //}

        ///// <summary>
        ///// 請求データ情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private SeikyuData _SeikyuData = null;
        ///// <summary>
        ///// 請求データ情報のデータアクセスオブジェクト
        ///// </summary>
        //public SeikyuData SeikyuData
        //{
        //    get
        //    {
        //        if (_SeikyuData == null)
        //        {
        //            _SeikyuData = new SeikyuData(this.appAuth);
        //        }
        //        return _SeikyuData;
        //    }
        //}

        ///// <summary>
        ///// トラDON管理情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDonSystemProperty _ToraDonSystemProperty = null;
        ///// <summary>
        ///// トラDON管理情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDonSystemProperty ToraDonSystemProperty
        //{
        //    get
        //    {
        //        if (_ToraDonSystemProperty == null)
        //        {
        //            _ToraDonSystemProperty = new ToraDonSystemProperty(this.appAuth);
        //        }
        //        return _ToraDonSystemProperty;
        //    }
        //}

        ///// <summary>
        ///// 得意先グループ情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private TokuisakiGroup _TokuisakiGroup = null;
        ///// <summary>
        ///// 得意先グループ情報のデータアクセスオブジェクト
        ///// </summary>
        //public TokuisakiGroup TokuisakiGroup
        //{
        //    get
        //    {
        //        if (_TokuisakiGroup == null)
        //        {
        //            _TokuisakiGroup = new TokuisakiGroup(this.appAuth);
        //        }
        //        return _TokuisakiGroup;
        //    }
        //}

        ///// <summary>
        ///// 得意先グループ明細情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private TokuisakiGroupMeisai _TokuisakiGroupMeisai = null;
        ///// <summary>
        ///// 得意先グループ明細情報のデータアクセスオブジェクト
        ///// </summary>
        //public TokuisakiGroupMeisai TokuisakiGroupMeisai
        //{
        //    get
        //    {
        //        if (_TokuisakiGroupMeisai == null)
        //        {
        //            _TokuisakiGroupMeisai = new TokuisakiGroupMeisai(this.appAuth);
        //        }
        //        return _TokuisakiGroupMeisai;
        //    }
        //}

        ///// <summary>
        ///// 方面グループ情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private HomenGroup _HomenGroup = null;
        ///// <summary>
        ///// 方面グループ情報のデータアクセスオブジェクト
        ///// </summary>
        //public HomenGroup HomenGroup
        //{
        //    get
        //    {
        //        if (_HomenGroup == null)
        //        {
        //            _HomenGroup = new HomenGroup(this.appAuth);
        //        }
        //        return _HomenGroup;
        //    }
        //}

        ///// <summary>
        ///// 方面グループ明細情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private HomenGroupMeisai _HomenGroupMeisai = null;
        ///// <summary>
        ///// 方面グループ明細情報のデータアクセスオブジェクト
        ///// </summary>
        //public HomenGroupMeisai HomenGroupMeisai
        //{
        //    get
        //    {
        //        if (_HomenGroupMeisai == null)
        //        {
        //            _HomenGroupMeisai = new HomenGroupMeisai(this.appAuth);
        //        }
        //        return _HomenGroupMeisai;
        //    }
        //}

        ///// <summary>
        ///// 車種グループ情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private CarKindGroup _CarKindGroup = null;
        ///// <summary>
        ///// 車種グループ情報のデータアクセスオブジェクト
        ///// </summary>
        //public CarKindGroup CarKindGroup
        //{
        //    get
        //    {
        //        if (_CarKindGroup == null)
        //        {
        //            _CarKindGroup = new CarKindGroup(this.appAuth);
        //        }
        //        return _CarKindGroup;
        //    }
        //}

        ///// <summary>
        ///// 車種グループ明細情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private CarKindGroupMeisai _CarKindGroupMeisai = null;
        ///// <summary>
        ///// 車種グループ明細情報のデータアクセスオブジェクト
        ///// </summary>
        //public CarKindGroupMeisai CarKindGroupMeisai
        //{
        //    get
        //    {
        //        if (_CarKindGroupMeisai == null)
        //        {
        //            _CarKindGroupMeisai = new CarKindGroupMeisai(this.appAuth);
        //        }
        //        return _CarKindGroupMeisai;
        //    }
        //}

        ///// <summary>
        ///// 販路グループ情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private HanroGroup _HanroGroup = null;
        ///// <summary>
        ///// 販路グループ情報のデータアクセスオブジェクト
        ///// </summary>
        //public HanroGroup HanroGroup
        //{
        //    get
        //    {
        //        if (_HanroGroup == null)
        //        {
        //            _HanroGroup = new HanroGroup(this.appAuth);
        //        }
        //        return _HanroGroup;
        //    }
        //}

        ///// <summary>
        ///// 販路グループ明細情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private HanroGroupMeisai _HanroGroupMeisai = null;
        ///// <summary>
        ///// 販路グループ明細情報のデータアクセスオブジェクト
        ///// </summary>
        //public HanroGroupMeisai HanroGroupMeisai
        //{
        //    get
        //    {
        //        if (_HanroGroupMeisai == null)
        //        {
        //            _HanroGroupMeisai = new HanroGroupMeisai(this.appAuth);
        //        }
        //        return _HanroGroupMeisai;
        //    }
        //}

        ///// <summary>
        ///// 郵便番号ワーク情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private WK_PostalZipData _WK_PostalZipData = null;
        ///// <summary>
        ///// 郵便番号ワーク情報のデータアクセスオブジェクト
        ///// </summary>
        //public WK_PostalZipData WK_PostalZipData
        //{
        //    get
        //    {
        //        if (_WK_PostalZipData == null)
        //        {
        //            _WK_PostalZipData = new WK_PostalZipData(this.appAuth);
        //        }
        //        return _WK_PostalZipData;
        //    }
        //}

        ///// <summary>
        ///// 販路情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private Hanro _Hanro = null;
        ///// <summary>
        ///// 販路情報のデータアクセスオブジェクト
        ///// </summary>
        //public Hanro Hanro
        //{
        //    get
        //    {
        //        if (_Hanro == null)
        //        {
        //            _Hanro = new Hanro(this.appAuth);
        //        }
        //        return _Hanro;
        //    }
        //}

        ///// <summary>
        ///// 営業所情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDONBranchOffice _ToraDONBranchOffice = null;
        ///// <summary>
        ///// 営業所情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDONBranchOffice ToraDONBranchOffice
        //{
        //    get
        //    {
        //        if (_ToraDONBranchOffice == null)
        //        {
        //            _ToraDONBranchOffice = new ToraDONBranchOffice(this.appAuth);
        //        }
        //        return _ToraDONBranchOffice;
        //    }
        //}

        ///// <summary>
        ///// 荷主情報のデータアクセスオブジェクトを保持するバッキングフィールド
        ///// </summary>
        //private ToraDONOwner _ToraDONOwner = null;
        ///// <summary>
        ///// 荷主情報のデータアクセスオブジェクト
        ///// </summary>
        //public ToraDONOwner ToraDONOwner
        //{
        //    get
        //    {
        //        if (_ToraDONOwner == null)
        //        {
        //            _ToraDONOwner = new ToraDONOwner(this.appAuth);
        //        }
        //        return _ToraDONOwner;
        //    }
        //}

        /// <summary>
        /// 作業大分類のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SagyoDaiBunrui _sagyoDaiBunrui = null;

        public SagyoDaiBunrui SagyoDaiBunrui
        {
            get
            {
                if (_sagyoDaiBunrui == null)
                {
                    _sagyoDaiBunrui = new SagyoDaiBunrui(this.appAuth);
                }

                return _sagyoDaiBunrui;
            }
        }

        /// <summary>
        /// 作業中分類のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SagyoChuBunrui _sagyoChuBunrui = null;

        public SagyoChuBunrui SagyoChuBunrui
        {
            get
            {
                if (_sagyoChuBunrui == null)
                {
                    _sagyoChuBunrui = new SagyoChuBunrui(this.appAuth);
                }

                return _sagyoChuBunrui;
            }
        }

        /// <summary>
        /// 作業小分類のデータアクセスオブジェクトを保持するバッキングフィールド
        /// </summary>
        private SagyoShoBunrui _sagyoShoBunrui = null;

        public SagyoShoBunrui SagyoShoBunrui
        {
            get
            {
                if (_sagyoShoBunrui == null)
                {
                    _sagyoShoBunrui = new SagyoShoBunrui(this.appAuth);
                }

                return _sagyoShoBunrui;
            }
        }

        #endregion

        #region 各種DALやBLLにアクセスするためのプロパティ

        #region システム名称関連

        #endregion

        #endregion
    }

    /// <summary>
    /// DALを検索してインスタンスを提供します。
    /// </summary>
    internal class DALLocator
    {
        private AppAuthInfo _authInfo;
        public DALLocator(AppAuthInfo authInfo)
        {
            _authInfo = authInfo;
        }

        public T GetOrNew<T>()
        {
            System.Reflection.ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(AppAuthInfo) });

            if (constructor != null)
                return (T)constructor.Invoke(new object[] { _authInfo });

            constructor = typeof(T).GetConstructor(System.Type.EmptyTypes);
            return (T)constructor.Invoke(null);
        }
    }
}
