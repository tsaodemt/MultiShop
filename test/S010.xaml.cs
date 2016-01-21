// *******************************************************************************
// System  : TestEditorPlus
// Title   : 本文引用機能 - 出題範囲画面
// Version : 1.0.0.0
// -------------------------------------------------------------------------------
// Create  : @16/01/14 NGUYỄN MINH THÔNG Create New
// *******************************************************************************

using StudyLinkZ.Core.Classes.TEP.Editor;
using StudyLinkZ.Core.Helpers;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace StudyLinkZ.TEP.UI.CustomControls
{
    /// <summary>
    /// 本機能では参考資料を閲覧し、資料選択
    /// 参考資料については「Z会作成済みテストもしくは「書籍の本文データ」を指定できる。
    /// </summary>
    public partial class S010
    {
        #region Properties and Variables ============================

        /// <summary>
        /// The data test
        /// </summary>
        private readonly TszTestLoader dataTest;

        /// <summary>
        /// Titles of form.
        /// </summary>
        private string formTitle = "書籍本文データ選択";

        /// <summary>
        /// The condition data
        /// </summary>
        private TSConditionData conditionData;

        #endregion Properties and Variables ============================

        #region Public methods ======================================

        /// <summary>
        /// Initializes a new instance of the <see cref="S010"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public S010(TszTestLoader data)
        {
            InitializeComponent();

            this.conditionData = TSConditionData.GetInstance();
            this.conditionData.書籍インストール先 = TSConstants.runMode.pc;

            this.dataTest = data;

            this.InitForm();
        }

        #endregion Public methods ======================================

        #region Private and Protected methods =======================

        /// <summary>
        /// Initializes the form.
        /// </summary>
        private void InitForm()
        {
            // Clear all node in treeview treQuestionRange
            this.treQuestionRange.Items.Clear();

            // If DS is nothing, show error message below, do nothing and stay at screen S010
            if (this.dataTest.Contents.Rows.Count < 1)
            {
                Common.TepMessageBox(
                    Properties.Resources.MSG_S010_E0001,
                    this.formTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            // Otherwise, execute the processes below for treeview treQuestionRange
            foreach (DataRow row in this.dataTest.Contents.Rows)
            {
                var node = new TreeNode { Index = (int)row["番号"], Text = row["階層1"].ToString() };

                foreach (DataRow rowChild in this.dataTest.Problem.Rows)
                {
                    var key = row["番号"].ToString() + rowChild["番号"].ToString();
                    var nodeChild = new TreeNode { Index = int.Parse(key), Text = rowChild["階層1"].ToString() };
                    node.Items.Add(nodeChild);
                }

                this.treQuestionRange.Items.Add(node);
            }
        }

        /// <summary>
        /// Handles the SelectedItemChanged event of the treQuestionRange control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void TreQuestionRangeSelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            this.conditionData.目的 = TSConstants.TestType.Id.existing;
            var baseFileNameFullPath = TSCommon.QuestionFolderPath(
                this.conditionData.書籍インストール状況,
                this.conditionData.科目,
                this.conditionData.書籍コード,
                this.conditionData.書籍バージョン,
                this.conditionData.目的);
            baseFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, "問題");

            var item = (TreeNode)treQuestionRange.SelectedItem;
            if (item.Items.Count > 0)
            {
                return;
            }

            var value = item.Index.ToString();
            var tmp = from DataRow row in this.dataTest.Correspondence.Rows
                      where (int)row["目次"] == int.Parse(value[0].ToString()) && (int)row["問題"] == int.Parse(value[1].ToString())
                      select row["ファイル名"].ToString();

            var selFileNameFullPath = System.IO.Path.Combine(baseFileNameFullPath, tmp.ToList()[0]);

            if (!System.IO.File.Exists(selFileNameFullPath))
            {
                // If the selected file is not exists, show warning below, do nothing and stay at screen S010.
                Common.TepMessageBox(
                    Properties.Resources.MSG_S010_W0002,
                    this.formTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var textRange = new TextRange(txtPreview.Document.ContentStart, txtPreview.Document.ContentEnd);
            using (var fileStream = new System.IO.FileStream(selFileNameFullPath, System.IO.FileMode.OpenOrCreate))
            {
                textRange.Load(fileStream, DataFormats.Rtf);
            }
        }

        #endregion Private and Protected methods =======================
    }
}