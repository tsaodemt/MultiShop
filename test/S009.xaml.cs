// *******************************************************************************
// System  : TestEditorPlus
// Title   : 書籍選択
// Version : 1.0.0.0
// -------------------------------------------------------------------------------
// Create  : @16/01/12 NGUYỄN MINH THÔNG Create New
// *******************************************************************************

using DevExpress.Xpf.Core;
using StudyLinkZ.Core.Helpers;
using StudyLinkZ.TEP.Classes;
using StudyLinkZ.TEP.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace StudyLinkZ.TEP.UI.Screens
{
    /// <summary>
    /// Get book to reference or edit selected questions.
    /// </summary>
    public partial class S009
    {
        #region Properties and Variables ============================

        /// <summary>
        /// Mode S044
        /// </summary>
        private int modeS044 = 2;

        /// <summary>
        /// Mode S010
        /// </summary>
        private int modeS010 = 1;

        /// <summary>
        /// Mode of S009
        /// </summary>
        private int mode;

        /// <summary>
        /// The condition data
        /// </summary>
        private TSConditionData conditionData;

        /// <summary>
        /// The system conf
        /// </summary>
        private TSSysCondition sysConf;

        /// <summary>
        /// The TSZ test loader
        /// </summary>
        private TszTestLoader tszTestLoader;

        /// <summary>
        /// UserControl S00902
        /// </summary>
        private S00902 s00902;

        /// <summary>
        /// UserControl S00901
        /// </summary>
        private S00901 s00901;

        /// <summary>
        /// UserControl S044
        /// </summary>
        private S044 s044;

        /// <summary>
        /// UserControl S010
        /// </summary>
        private S010 s010;

        /// <summary>
        /// The folder name z
        /// </summary>
        private string folderNameZ = "Ｚ会作成テスト";

        /// <summary>
        /// The folder name data test
        /// </summary>
        private string folderNameDataTest = "本文データ";

        #endregion Properties and Variables ============================

        #region Public methods ======================================

        /// <summary>
        /// Initializes a new instance of the <see cref="S009"/> class.
        /// </summary>
        public S009(int mode)
        {
            InitializeComponent();

            this.sysConf = TSSysCondition.GetInstance();
            this.sysConf.エディション = TSConstants.Edition.Id.jpnSoc;
            this.sysConf.インストール状況 = TSConstants.runMode.pc;

            this.conditionData = TSConditionData.GetInstance();
            this.conditionData.書籍インストール先 = TSConstants.runMode.pc;

            btnDisplay.Visibility = Visibility.Hidden;
            btnReturns044.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            btnQuote.Visibility = Visibility.Hidden;
            BtnChooseBook.IsEnabled = false;

            this.mode = mode;
            switch (this.mode)
            {
                case 1:
                    this.ShowS00901();
                    break;

                case 2:
                    this.ShowS00902();
                    break;

                default:
                    break;
            }
        }

        #endregion Public methods ======================================

        #region Events ==============================================

        /// <summary>
        /// BTNs the next click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            // Read condition Data of selected book
            this.ReadConditionFile();

            // Read all data of selected book
            this.tszTestLoader = new TszTestLoader(this.conditionData.科目, this.conditionData.書籍コード, this.conditionData.書籍バージョン);

            if (this.mode == this.modeS044)
            {
                // 3.9. Click button Next 次へ
                // Push all data of selected book
                this.ShowS044(this.tszTestLoader);
            }
            else
            {
                // 4.9. Click button Next 次へ
                // Push all data of selected book
                this.ShowS010(this.tszTestLoader);
            }
        }

        /// <summary>
        /// BTNs the choose book click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void BtnChooseBookClick(object sender, RoutedEventArgs e)
        {
            this.ReadConditionFile();

            this.ShowS044(this.tszTestLoader);
        }

        #endregion Events ==============================================

        #region Private and Protected methods =======================

        /// <summary>
        /// Shows the S00901.
        /// </summary>
        private void ShowS00901()
        {
            btnNext.Focus();
            this.s00901 = new S00901();

            tabControl.Items.Add(new DXTabItem { Content = this.s00901 });
        }

        /// <summary>
        /// Shows the S00902.
        /// </summary>
        private void ShowS00902()
        {
            btnNext.Focus();
            this.s00902 = new S00902();

            tabControl.Items.Add(new DXTabItem { Content = this.s00902 });
        }

        /// <summary>
        /// Shows the S0044.
        /// </summary>
        /// <param name="data">The data.</param>
        private void ShowS044(TszTestLoader data)
        {
            var mode = this.s00902.rdoModeZ.IsChecked == true ? 1 : 2;

            this.s044 = new S044(mode, data);
            tabControl.Items.Clear();
            tabControl.Items.Add(new DXTabItem { Content = this.s044 });

            btnDisplay.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Hidden;
            btnReturns044.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Shows the S0044.
        /// </summary>
        /// <param name="data">The data.</param>
        private void ShowS010(TszTestLoader data)
        {
            this.s010 = new S010(data);
            tabControl.Items.Clear();
            tabControl.Items.Add(new DXTabItem { Content = this.s010 });

            btnDisplay.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            btnNext.Visibility = Visibility.Hidden;
            btnReturns044.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Visible;
            btnQuote.Visibility = Visibility.Visible;
            btnReturn.IsEnabled = true;
            BtnChooseBook.IsEnabled = true;
        }

        /// <summary>
        /// Reads the condition file.
        /// </summary>
        private void ReadConditionFile()
        {
            var conditionCtrl = new TSConditionDataCtrl();

            // Filter follow English(英語) category.  Because on screen, there are only 1 categories of book: English(英語)
            var sb = (TSConstants.Subject.Id)Enum.Parse(typeof(TSConstants.Subject.Id), "english", true);

            // Get selected book
            var selectedBook = this.s00902 == null ? (Book)this.s00901.lstBook.SelectedItem : (Book)this.s00902.lstBook.SelectedItem;

            // Read condition data of selected book
            conditionCtrl.LoadCondition(sb, selectedBook.book_id.ToString(), selectedBook.version.ToString());
        }

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="listFile">The list file.</param>
        private void CopyFile(List<string> listFile)
        {
            if (listFile.Count < 1)
            {
                // In case there is no chosen question range in trvQuestionRanges, show the following message
                // Show message box MSG_S033_E0001.
                Common.TepMessageBox(
                    Properties.Resources.MSG_S044_W0001,
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                // Exit function.
                return;
            }

            var fbd = new FolderBrowserDialog();
            var copyToPath = string.Empty;

            // Open the [SaveDialog] control. Choose copy parent destination folder.
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                copyToPath = fbd.SelectedPath;
            }
            else
            {
                return;
            }

            var folderNameDetail = this.s00902.rdoModeZ.IsChecked == true ? this.folderNameZ : this.folderNameDataTest;

            // Create the following child destination folder in destination parent folder
            var newFolderName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMdd"), this.conditionData.書籍名, folderNameDetail);
            copyToPath = System.IO.Path.Combine(copyToPath, newFolderName);
            try
            {
                if (!System.IO.Directory.Exists(copyToPath))
                {
                    System.IO.Directory.CreateDirectory(copyToPath);
                }

                this.conditionData.目的 = TSConstants.TestType.Id.existing;
                var baseFileNameFullPath = TSCommon.QuestionFolderPath(
                    this.conditionData.書籍インストール状況,
                    this.conditionData.科目,
                    this.conditionData.書籍コード,
                    this.conditionData.書籍バージョン,
                    this.conditionData.目的);
                baseFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, "問題");

                foreach (var selFileName in listFile)
                {
                    var selFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, selFileName);
                    var deskTopFilePath = System.IO.Path.Combine(copyToPath, selFileName);
                    System.IO.File.Copy(selFileNameFullPath, deskTopFilePath, true);
                }
            }
            catch (Exception)
            {
                // If there is any error in saving file progress, show the following message
                // Show message box MSG_S033_E0001.
                Common.TepMessageBox(
                    Properties.Resources.MSG_S044_E0005,
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            // After saving successful, show the following message
            Common.TepMessageBox(
                Properties.Resources.MSG_S044_I0003,
                this.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        /// <summary>
        /// BTNs the save click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            // Chosen question ranges in trvQuestionRanges
            var nodeChecked = TSCommon.GetAllNodes(this.s044.trvQuestionRanges.View.Nodes).Where(node => node.IsChecked == true).ToList();

            // List file name
            var result = new List<string>();
            foreach (var node in nodeChecked)
            {
                var value = ((KeyValuePair<string, string>)node.Content).Key;

                // If parent node then continue
                if (!value.Contains(','))
                {
                    continue;
                }

                var content = value.Split(',');
                var fileName = from DataRow row in this.tszTestLoader.Correspondence.Rows
                          where (int)row["目次"] == int.Parse(content[0]) && (int)row["問題"] == int.Parse(content[1])
                          select row["ファイル名"].ToString();

                result.AddRange(fileName.ToList());
            }

            this.CopyFile(result);
        }

        /// <summary>
        /// BTNs the display click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void BtnDisplayClick(object sender, RoutedEventArgs e)
        {
            // Chosen question ranges in trvQuestionRanges
            var nodeChecked = TSCommon.GetAllNodes(this.s044.trvQuestionRanges.View.Nodes).Where(node => node.IsChecked == true).ToList();

            // In case there are >=2 chosen question ranges in trvQuestionRanges, show the following message
            if (nodeChecked.Count < 1 || nodeChecked.Count >= 2)
            {
                // Show message box MSG_S044_W0002.
                Common.TepMessageBox(
                    Properties.Resources.MSG_S044_W0002,
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                // Exit function.
                return;
            }

            var value = ((KeyValuePair<string, string>)nodeChecked[0].Content).Key.Split(',');

            // Read [問題対応表.asc] file in folder
            var tmp = from DataRow row in this.tszTestLoader.Correspondence.Rows
                      where (int)row["目次"] == int.Parse(value[0]) && (int)row["問題"] == int.Parse(value[1])
                      select row["ファイル名"].ToString();

            this.conditionData.目的 = TSConstants.TestType.Id.existing;
            var baseFileNameFullPath = TSCommon.QuestionFolderPath(
                this.conditionData.書籍インストール状況,
                this.conditionData.科目,
                this.conditionData.書籍コード,
                this.conditionData.書籍バージョン,
                this.conditionData.目的);
            baseFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, "問題");

            // Get the corresponding *.rtf file(s) of chosen
            var selFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, tmp.ToList()[0]);

            // Open rtf file of selected question ranges by corresponding software (WordPad by default).
            Process.Start("WordPad.exe", selFileNameFullPath);
        }

        #endregion Private and Protected methods =======================
    }
}