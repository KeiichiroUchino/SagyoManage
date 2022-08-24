using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using jp.co.jpsys.util;
using jp.co.jpsys.util.print;

namespace Jpsys.SagyoManage.ReportAppendix
{
    //*** s.arimura 2013/05/23 NSKFramework����R�s�[�BPageSetupDialog��PaperSize���擾�ł��Ȃ����ۂ̑΍��p
    /// <summary>
    /// OK�{�^���ƃL�����Z���{�^���̃��x���ɔC�ӂ̕�����
    /// �\���ł���v�����^�I���_�C�A���O��\������N���X�ł��B
    /// CrystalReport for Visual Studio 2008�ň������ۂɎg�p���܂��B
    /// </summary>
    /// <remarks>
    /// �v�����^�y�т��̃v�����^�őI���ł��鋋�����@�y�сA�p���ݒ��
    /// �I�����邽�߂̃_�C�A���O��\������N���X�ł��B
    /// OK�{�^���ƃL�����Z���{�^���ɕ\������镶�����C�ӂɕύX����
    /// �����\�ł��B
    /// �R���X�g���N�^�ɂ���āA�_�C�A���O�\���O�ɑI�����ڂ����O��
    /// �ݒ肷�邱�Ƃ��\�ł��B
    /// �{�N���X�́ACrystalReport for Visual Studio 2008�ň����
    /// �s���ۂɎg�p���邱�Ƃ�z�肵�Ă��܂��B�z��O�̊��Ŏg�p�����ꍇ��
    /// ����ɂ��Ă͕ۏ؂��܂���B
    /// </remarks>
    public partial class PrinterChoiceDialog : Form
    {
        /// <summary>
        /// �v�����^�ݒ�̃w���p�[�N���X��ێ�
        /// </summary>
        private NSKPrinterSettingHelper printerSettingHelper;

        //*** s.arimura �ǉ�
        /// <summary>
        /// �y�[�W�ݒ�_�C�A���O�ɓn��PaperSize�̐ݒ�𖳂��Ƃ��邩�ǂ���
        /// </summary>
        private bool nullToPaperSizeOnPageSetupDialog;

        /// <summary>
        /// PrinterChoiceDialog�N���X�̃C���X�^���X�����������܂��B
        /// </summary>
        /// <remarks>
        /// �{�R���X�g���N�^�ɂď��������ꂽ�C���X�^���X�ɕێ������A
        /// PageSettings�́A���݂̃R���s���[�^�ɐݒ肳��Ă���f�t�H���g
        /// �v�����^�i�ʏ�g���v�����^�j�̐ݒ肪�ݒ肳�ꂽ���̂ɂȂ�܂��B
        /// </remarks>
        public PrinterChoiceDialog()
        {
            InitializeComponent();

            //�w���p�[�N���X�̃C���X�^���X��
            this.printerSettingHelper = new NSKPrinterSettingHelper();

            this.InitPrinterChoiceDialog();
        }
        /// <summary>
        /// PageSettings�N���X�̃C���X�^���X���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="page">PageSettings�N���X�̃C���X�^���X</param>
        /// <param name="nullToPaperSizeOnPageSetupDialog">�y�[�W�ݒ�_�C�A���O�ɓn��PaperSize�̐ݒ�𖳂��Ƃ��邩�ǂ���</param>
        /// <remarks>
        /// PageSettings�N���X�̃C���X�^���X���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B�f�V���A���C�Y���ꂽPageSettings�I�u�W�F�N�g
        /// ����PrinterChoiceDialog�̃C���X�^���X������������ꍇ�Ȃǂɂ��悤���܂��B
        /// </remarks>
        public PrinterChoiceDialog(PageSettings page, bool nullToPaperSizeOnPageSetupDialog)
        {
            InitializeComponent();

            //�w���p�[�N���X�̃C���X�^���X��
            this.printerSettingHelper =
                new NSKPrinterSettingHelper(page);
            this.nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;

            this.InitPrinterChoiceDialog();

            this.PageCopies = page.PrinterSettings.Copies;

            this.nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;
        }

        /// <summary>
        /// �v�����^���Ƌ������@���Ɨp���T�C�Y���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="printerName">�v�����^��</param>
        /// <param name="sourceName">�������@��</param>
        /// <param name="sizeName">�p���T�C�Y��</param>
        /// <remarks>
        /// �v�����^���Ƌ������@���E�p���T�C�Y���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B�w�肵���e���ڂ̒l�ɐݒ肳�ꂽ
        /// PageSettings��ێ����܂��B�������A�C���X�g�[������Ă��Ȃ��v�����^��
        /// �v�����^���ێ����Ȃ��������@����їp���T�C�Y���w�肵���ꍇ�́A��O��
        /// �������܂��B
        /// </remarks>
        public PrinterChoiceDialog(string printerName, string sourceName,
                string sizeName)
        {
            InitializeComponent();

            //�w���p�[�N���X�̃C���X�^���X��
            this.printerSettingHelper =
                new NSKPrinterSettingHelper(printerName, sourceName, sizeName);

            this.InitPrinterChoiceDialog();

        }

        /// <summary>
        /// NSKPrinterSettingHelper�N���X�̃C���X�^���X���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="nskPrinterHelper">NSKPrinterSettingHelper�N���X�̃C���X�^���X</param>
        /// <remarks>
        /// NSKPrinterSettingHelper�N���X�̃C���X�^���X���w�肵�āAPrinterChoiceDialog
        /// �N���X�̃C���X�^���X�����������܂��B���O�Ƀv�����^�̐ݒ�Ȃǂ�ύX����
        /// NSKPrinterSettingHelper�I�u�W�F�N�g����PageSettings�����o���ĕێ����܂��B
        /// </remarks>
        public PrinterChoiceDialog(NSKPrinterSettingHelper nskPrinterHelper)
        {
            InitializeComponent();

            //�w���p�[�N���X�̃C���X�^���X��ݒ�
            this.printerSettingHelper = nskPrinterHelper;

            this.InitPrinterChoiceDialog();

        }

        /// <summary>
        /// �{��ʂ̏��������s���܂��B
        /// </summary>
        private void InitPrinterChoiceDialog()
        {
            //�e�t�H�[��������Ƃ��́A���̃t�H�[���𒆐S�ɕ\������悤�ɕύX
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            this.CreatePrinterCombo();
            this.CreatePagerSourceCombo();
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            this.EnableDuplex();
            this.EnableMono();
            this.EnableCopies();

            this.AllowMargins = true;//�����l�Ƃ��Đݒ肷��(�]���ݒ�\)���K�v�ɉ����ĊO������v���p�e�B�o�R�ŕύX�\
            this.AllowOrientation = true;//�����l�Ƃ��Đݒ肷��(�p�������ݒ�\)���K�v�ɉ����ĊO������v���p�e�B�o�R�ŕύX�\

        }

        #region �v���C�x�[�g���\�b�h

        /// <summary>
        /// ���ʈ���̗L�������𐧌�
        /// </summary>
        private void EnableDuplex()
        {
            //��[���ʈ���֌W�𖳌���
            this.chkUseDuplex.Checked = false;
            this.chkUseDuplex.Enabled = false;
            this.grbDuplexAllign.Enabled = false;


            //�v�����^�ݒ肩�痼�ʂ̉ۂ��Ƃ�
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            PageSettings wk_ps =
                this.printerSettingHelper.StoredPageSettings;
            if (wk_pr.CanDuplex)
            {
                //���ʂɐݒ肳��Ă�����`�F�b�NON
                if (wk_pr.Duplex != Duplex.Simplex)
                {
                    this.chkUseDuplex.Checked = true;

                    //�����������������������擾����
                    switch (wk_pr.Duplex)
                    {
                        case Duplex.Default:
                            //Default�̎��͐�������
                            this.rbtDuplexVertical.Checked = true;
                            break;
                        case Duplex.Horizontal:
                            //����
                            //--���u�����ǂ������f���ĉ��u���̏ꍇ�́A�������t�]
                            if (!wk_ps.Landscape)
                            {
                                //�c�u��
                                this.rbtDuplexVertical.Checked = false;
                                this.rbtDuplexHorizontal.Checked = true;
                            }
                            else
                            {
                                //���u��
                                this.rbtDuplexVertical.Checked = true;
                                this.rbtDuplexHorizontal.Checked = false;
                            }
                            break;
                        case Duplex.Simplex:
                            //���肦�Ȃ��B�B
                            break;
                        case Duplex.Vertical:
                            //����
                            //--���u�����ǂ������f���ĉ��u���̏ꍇ�́A�������t�]
                            if (!wk_ps.Landscape)
                            {
                                //�c�u��
                                this.rbtDuplexVertical.Checked = true;
                                this.rbtDuplexHorizontal.Checked = false;
                            }
                            else
                            {
                                //���u��
                                this.rbtDuplexVertical.Checked = false;
                                this.rbtDuplexHorizontal.Checked = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            //���ʎd�l�ۂ��g�p�\�̏�Ԃ��v�����^�ݒ�̗��ʈ���ۂ���擾
            this.chkUseDuplex.Enabled = wk_pr.CanDuplex;
            this.grbDuplexAllign.Enabled = this.chkUseDuplex.Checked;

        }

        /// <summary>
        /// ��������̗L�������𐧌�
        /// </summary>
        private void EnableMono()
        {
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            //��������g�p�ۂ��J���[����̃T�|�[�g�ۂ���擾
            this.chkUseMono.Enabled = wk_pr.SupportsColor;
            //PageSettings����J���[����̎g�p�L�����擾���āA�t���O���t�]
            this.chkUseMono.Checked = !this.printerSettingHelper.StoredPageSettings.Color;

        }

        /// <summary>
        /// �����̐ݒ�̗L�������𐧌�
        /// </summary>
        private void EnableCopies()
        {
            //��ʏ�Ŏw�肵��������\�����A�O���[�v�{�b�N�X�u�����w��v��Enabled��False�ɂ���
            this.grbPageCopies.Enabled = false;
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            this.nudCopies.Maximum = wk_pr.MaximumCopies;

            //PrinterSettings wk_pr =
            //    this.printerSettingHelper.StoredPageSettings.PrinterSettings;

            //if (wk_pr.MaximumCopies == 1)
            //{
            //    this.nudCopies.Value = 1;
            //    this.grbPageCopies.Enabled = false;
            //}
            //else
            //{
            //    this.grbPageCopies.Enabled = true;
            //    this.nudCopies.Maximum = wk_pr.MaximumCopies;
            //}
        }


        /// <summary>
        /// �v�����^�̃R���{�{�b�N�X���쐬
        /// </summary>
        private void CreatePrinterCombo()
        {
            IList<PrinterNameIndexItem>
                printer_list = this.printerSettingHelper.GetPrinterNameIndexList();

            if (printer_list.Count != 0)
            {
                this.comboInstalledPrinters.DataSource = printer_list;
                this.comboInstalledPrinters.DisplayMember =
                    PrinterNameIndexItem.DisplayMemberString;
                this.comboInstalledPrinters.ValueMember =
                    PrinterNameIndexItem.ValueMemberString;

                //���ݑI������Ă���v�����^���擾����
                string printer_name =
                    this.printerSettingHelper.StoredPageSettings.PrinterSettings.PrinterName;
                for (int i = 0; i < printer_list.Count; i++)
                {
                    if (printer_list[i].PrinterNameString == printer_name)
                    {
                        //�Y�����Ă���v�����^�����݂��Ă�����Acombo�̑I����
                        //�ύX�B
                        this.comboInstalledPrinters.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// �������@�̃R���{�{�b�N�X���쐬
        /// </summary>
        private void CreatePagerSourceCombo()
        {
            IList<PaperSourceNameIndexItem>
                source_list =
                this.printerSettingHelper.GetPaperSourceNameIndexList();

            if (source_list.Count != 0)
            {
                this.comboPrinterPaperSources.DataSource = source_list;
                this.comboPrinterPaperSources.DisplayMember =
                    PaperSourceNameIndexItem.DisplayMemberString;
                this.comboPrinterPaperSources.ValueMember =
                    PaperSourceNameIndexItem.ValueMemberString;

                //���ݑI������Ă��鋋�����@���擾����
                string source_name =
                    this.printerSettingHelper.StoredPageSettings.PaperSource.SourceName;
                for (int i = 0; i < source_list.Count; i++)
                {
                    if (source_list[i].PaperSourceNameString == source_name)
                    {
                        //�Y�����Ă��鋋�����@�����݂��Ă�����Acombo�̑I����
                        //�ύX
                        this.comboPrinterPaperSources.SelectedIndex = i;
                        break;
                    }
                }
            }

            //�������@��ύX�����ꍇ�p���T�C�Y���ύX�����\�������邽�ߍĐݒ���s��
            SetPaperSourceInnerSetting(
                this.comboPrinterPaperSources.Text.Trim());

        }

        /// <summary>
        /// �v�����^�����w�肵�ċ������@�̃R���{�{�b�N�X�̃��X�g��
        /// �w�肵���v�����^�̋������@�̃��X�g���Z�b�g���܂��B
        /// </summary>
        private void ChangePaperSourceComboItemList(string printerName)
        {
            this.printerSettingHelper.SetCurrentPrinter(printerName);
            this.CreatePagerSourceCombo();
        }

        /// <summary>
        /// �������@�����w�肵�āA�v�����^����ێ���������ϐ���
        /// �Z�b�g���܂��B
        /// </summary>
        /// <param name="paperSourceName"></param>
        private void SetPaperSourceInnerSetting(string paperSourceName)
        {
            this.printerSettingHelper.SetCurrentPaperSource(paperSourceName);
        }


        #endregion

        #region �p�u���b�N���\�b�h

        /// <summary>
        /// OK�{�^���ɕ\�����镶�����ݒ肵�܂��B
        /// </summary>
        /// <param name="buttonText">OK�{�^���ɕ\�����镶����</param>
        /// <remarks>
        /// �_�C�A���O��ʂ�OK�{�^���ɕ\������Ă��镶�����ύX���܂��B
        /// OK�{�^�������������DialogResult�l��DialogResult.OK���ݒ�
        /// ����ă_�C�A���O�������܂��B
        /// </remarks>
        public void SetOkButtonDisplayText(string buttonText)
        {
            this.btnOk.Text = buttonText;
        }

        /// <summary>
        /// �L�����Z���{�^���ɕ\�����镶�����ݒ肵�܂��B
        /// </summary>
        /// <param name="buttonText">�L�����Z���{�^���ɕ\�����镶����</param>
        /// <remarks>
        /// �_�C�A���O��ʂ̃L�����Z���{�^���ɕ\������Ă��镶�����ύX���܂��B
        /// �L�����Z���{�^�������������DialogResult�l��DialogResult.Cancel��
        /// �ݒ肳��ă_�C�A���O�������܂��B
        /// </remarks>
        public void SetCancelButtonDisplayText(string buttonText)
        {
            this.btnCancel.Text = buttonText;
        }

        #endregion

        #region �v���p�e�B

        /// <summary>
        /// �{�N���X�Őݒ肳�ꂽPageSettings�N���X�̃C���X�^���X���擾���܂��B
        /// �i�ǂݎ���p�j
        /// </summary>
        public PageSettings PopulatePageSetting
        {
            get
            {
                return this.printerSettingHelper.StoredPageSettings;
            }
        }

        /// <summary>
        /// �{�N���X�Őݒ肳�ꂽNSKPrinterSettingHelper�N���X�̃C���X�^���X��
        /// �擾���܂��B�i�ǂݎ���p�j
        /// </summary>
        public NSKPrinterSettingHelper PopulateNSKPrinterSettingHelper
        {
            get
            {
                return this.printerSettingHelper;
            }
        }

        /// <summary>
        /// ����͈͂ƈ�������̐ݒ�������邩�ǂ������w�肵�܂��B
        /// �K��l��false(�����Ȃ��j�ɐݒ肳��Ă��܂��B
        /// </summary>
        /// <remarks>
        /// ���̃v���p�e�B��true�̎��́A����͈͂ƈ��������ݒ肷��
        /// �����ł���悤�ɂȂ�܂��B�K��l��false�i�����Ȃ��j��
        /// �ݒ肳��Ă��܂��B
        /// </remarks>
        public bool AllowPrintRangeCopiesOption
        {
            get
            {
                return this.plnRangeCopies.Visible;
            }
            set
            {
                this.plnRangeCopies.Visible = value;
            }

        }

        /// <summary>
        /// ����������擾�E�ݒ肵�܂��B
        /// </summary>
        public decimal PageCopies
        {
            get
            {
                return this.nudCopies.Value;
            }
            set
            {
                this.nudCopies.Value = value;
            }
        }

        /// <summary>
        /// �������������ۂɕ��P�ʂň�����邩�ǂ������擾�E�ݒ肵�܂��B
        /// �K��l��true�i���P�ʂň������j�ɐݒ肳��Ă��܂��B
        /// </summary>
        public bool PrintCollated
        {
            get
            {
                return this.chkCollated.Checked;
            }
            set
            {
                this.chkCollated.Checked = value;
            }

        }

        /// <summary>
        /// ����͈͂Łu���ׂāv���I������Ă��邩�ǂ������擾�E�ݒ肵�܂��B
        /// </summary>
        public bool PrintRangeAll
        {
            get { return this.rbtAllRange.Checked; }
            set { this.rbtAllRange.Checked = value; }
        }

        /// <summary>
        /// ����͈͂Łu���݂̂؁[�W�v���I������Ă��邩�ǂ������擾�E�ݒ肵�܂��B
        /// </summary>
        public bool PrintRangeCurrentPage
        {
            get { return this.rbtRangeCurrent.Checked; }
            set { this.rbtRangeCurrent.Checked = value; }
        }

        /// <summary>
        /// ����͈͂Łu�y�[�W�w��v���I������Ă��邩�ǂ������擾�E�ݒ肵�܂��B
        /// </summary>
        public bool PrintRangeSelect
        {
            get { return this.rbtRangeSelect.Checked; }
            set { this.rbtRangeSelect.Checked = value; }
        }

        /// <summary>
        /// ����͈͂̃y�[�W�w��̊J�n�y�[�W���擾�E�ݒ肵�܂��B
        /// </summary>
        public decimal PrintRangeFrom
        {
            get { return this.nudRangeFrom.Value; }
            set { this.nudRangeFrom.Value = value; }

        }

        /// <summary>
        /// ����͈͂̃y�[�W�w��̏I���y�[�W���擾�E�ݒ肵�܂��B
        /// </summary>
        public decimal PrintRangeTo
        {
            get { return this.nudRangeTo.Value; }
            set { this.nudRangeTo.Value = value; }


        }

        /// <summary>
        /// ���ʈ�����g�p���邩�ǂ������擾���܂��B�i�ǂݎ���p�j
        /// </summary>
        public bool PrintUseDuplex
        {
            get { return this.chkUseDuplex.Checked; }
        }

        /// <summary>
        /// ����������g�p���邩�ǂ������擾���܂��B�i�ǂݎ���p�j
        /// </summary>
        public bool PrintUseMono
        {
            get { return this.chkUseMono.Checked; }
        }

        /// <summary>
        /// �y�[�W�ݒ�_�C�A���O �{�b�N�X�̗]���Z�N�V�������L�����ǂ�����\���l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool AllowMargins { get; set; }

        /// <summary>
        /// �y�[�W�ݒ�_�C�A���O �{�b�N�X�̗p�������Z�N�V���� (�������܂��͏c����) ���L�����ǂ�����\���l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public bool AllowOrientation { get; set; }

        #endregion


        private void comboInstalledPrinters_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //�v�����^�[�̃R���{�����[�U�őI������ĕύX���ꂽ�Ƃ��̃C�x���g
            Cursor.Current = Cursors.WaitCursor;
            this.ChangePaperSourceComboItemList(
                this.comboInstalledPrinters.Text.Trim());
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            //�v�����^�[���ύX���ꂽ��A���ʐݒ�Ɣ����ݒ�����t���b�V��
            this.EnableDuplex();
            this.EnableMono();
            this.EnableCopies();//��������̐ݒ���؂�ւ���

        }


        private void comboPrinterPaperSources_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //�������@�����[�U�őI������ĕύX���ꂽ�Ƃ��̃C�x���g
            Cursor.Current = Cursors.WaitCursor;
            SetPaperSourceInnerSetting(
                this.comboPrinterPaperSources.Text.Trim());
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;

        }

        private void btnShowPaperSetting_Click(object sender, EventArgs e)
        {
            //�p���ݒ�̃_�C�A���O��\��
            using (PageSetupDialog psd = new PageSetupDialog())
            {

                psd.AllowMargins = this.AllowMargins;
                psd.AllowOrientation = this.AllowOrientation;

                psd.EnableMetric = true;
                psd.PrinterSettings =
                    (System.Drawing.Printing.PrinterSettings)this.PopulatePageSetting.PrinterSettings.Clone();
                psd.PageSettings = 
                    (System.Drawing.Printing.PageSettings)this.printerSettingHelper.StoredPageSettings.Clone();

                if (this.nullToPaperSizeOnPageSetupDialog)
                {
                    psd.PageSettings.PaperSize = null;
                }
                else
                {
                    //���������Ȃ��ꍇ�ł��Ώۂ̃v�����^�ɑΏۂ̗p���T�C�Y�������݂��Ȃ��ꍇ��
                    //����������

                    bool _nullToPaperSize = true;

                    //PrinterSettings.PaperSizes�����[�v���āA���|�[�g�̐ݒ�ƈ�v����PaperSize���Z�b�g����
                    foreach (PaperSize paperSize in psd.PrinterSettings.PaperSizes)
                    {
                        //���[�U�[��`�T�C�Y���ǂ����̔��f
                        if (paperSize.Kind == PaperKind.Custom)
                        {
                            //�J�X�^���̏ꍇ�͍����ƕ����������ǂ����Ŕ��f����
                            if (paperSize.Height == psd.PageSettings.PaperSize.Height &&
                                paperSize.Width == psd.PageSettings.PaperSize.Width)
                            {
                                //�p�����������ꍇ�͏��������Ȃ�
                                _nullToPaperSize = false;
                                break;
                            }
                        }
                        else
                        {
                            if (paperSize.Kind == psd.PageSettings.PaperSize.Kind)
                            {
                                //�p�����������ꍇ�͏��������Ȃ�
                                _nullToPaperSize = false;
                                break;
                            }
                        }
                    }

                    if (_nullToPaperSize)
                    {
                        //�p���T�C�Y�����݂��Ȃ��ꍇ�ɂ͏���������
                        psd.PageSettings.PaperSize = null;
                    }

                }

                if (psd.ShowDialog(this) == DialogResult.OK)
                {
                    this.printerSettingHelper = new NSKPrinterSettingHelper(psd.PageSettings);
                }

                this.CreatePagerSourceCombo();

                this.textPaperName.Text =
                    this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            }
        }

        private void PrinterChoiceDialog_Load(object sender, EventArgs e)
        {
            //����͈́E�����w��̃p�l�����\���̎��́A���̕���ʂ�����������
            if (!this.plnRangeCopies.Visible)
            {
                this.Height = this.Height - this.plnRangeCopies.Height;
            }

            //�t�H�[�J�X��OK�{�^���ɂ���
            this.btnOk.Focus();
        }

        private void nudRangeFrom_Enter(object sender, EventArgs e)
        {
            //�y�[�W�͈͂��w�肷����͘g�Ƀt�H�[�J�X����������
            //����y�[�W���w�肷�郉�W�I�{�^���̃`�F�b�N���ړ�����
            this.rbtRangeSelect.Checked = true;
        }

        private void nudRangeTo_Enter(object sender, EventArgs e)
        {
            //�y�[�W�͈͂��w�肷����͘g�Ƀt�H�[�J�X����������
            //����y�[�W���w�肷�郉�W�I�{�^���̃`�F�b�N���ړ�����
            this.rbtRangeSelect.Checked = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //��{�̐ݒ蕔��������OK�̎��ɃR�~�b�g���Ă���

            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            PageSettings wk_ps =
                this.printerSettingHelper.StoredPageSettings;

            //--���ʂ̐ݒ�
            if (this.chkUseDuplex.Checked)
            {
                //�����H
                if (this.rbtDuplexVertical.Checked)
                {
                    //���u�����ǂ���
                    if (!wk_ps.Landscape)
                    {
                        wk_pr.Duplex = Duplex.Vertical;
                    }
                    else
                    {
                        wk_pr.Duplex = Duplex.Horizontal;
                    }
                }
                //�����H
                else if (this.rbtDuplexHorizontal.Checked)
                {
                    //���u�����ǂ����H
                    if (!wk_ps.Landscape)
                    {
                        wk_pr.Duplex = Duplex.Horizontal;
                    }
                    else
                    {
                        wk_pr.Duplex = Duplex.Vertical;
                    }
                }
            }
            else
            {
                //���ʂ̃`�F�b�N���������
                wk_pr.Duplex = Duplex.Simplex;
            }

            //����
            wk_ps.Color = !this.chkUseMono.Checked;

            //��x���v���p�e�B���ύX����Ă��Ȃ��ꍇ�́A�����ϐ����ݒ肳��Ă��Ȃ�
            //�ꍇ��������ւ̑Ή���Ńv���p�e�B�[�̊���͐ݒ肷��B
            wk_pr.PrinterName = wk_pr.PrinterName;
            wk_ps.PaperSource = wk_ps.PaperSource;
            wk_ps.PaperSize = wk_ps.PaperSize;
            wk_ps.PrinterResolution = wk_ps.PrinterResolution;
            wk_pr.Copies = (short)Convert.ToInt32(this.nudCopies.Value);
        }

        private void chkUseDuplex_CheckedChanged(object sender, EventArgs e)
        {
            this.grbDuplexAllign.Enabled = this.chkUseDuplex.Checked;
        }

    }
}