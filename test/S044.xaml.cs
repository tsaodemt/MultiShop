using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Data;

namespace StudyLinkZ.TEP.UI.CustomControls
{
    /// <summary>
    /// 本機能では参考資料を閲覧し、資料選択
    /// 参考資料については「Z会作成済みテストもしくは「書籍の本文データ」を指定できる。
    /// </summary>
    public partial class S044
    {
        private const int ModeZ = 1;

        private int Mode;

        private readonly TszTestLoader dataTest;

        /// <summary>
        /// Initializes a new instance of the S038 class.
        /// </summary>
        public S044(int mode, TszTestLoader data)
        {
            InitializeComponent();
            Mode = mode;
            if (this.Mode == ModeZ)
            {
                lblMainTitle.Content = "「Ｚ会作成済のテストを利用する」";
                lblSubTitle.Content = "利用するＺ会作成済テストを一覧から選択してください。その後、「表示」または「保存」ボタンを押してください。";
            }
            else
            {
                lblMainTitle.Content = "「書籍の本文データを利用する」";
                lblSubTitle.Content = "利用する書籍の本文データを一覧から選択してください。その後、「表示」または「保存」ボタンを押してください。";
            }

            dataTest = data;

            InitForm();
        }

        private void btnSelectAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var nodes = trvQuestionRanges.View.Nodes;

            // Traverse through the items inside the trvQuestionRanges
            foreach (var node in nodes)
            {
                // With each item, set the item.Checked = true
                node.IsChecked = true;
            }
        }

        private void btnDeselectAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var nodes = trvQuestionRanges.View.Nodes;

            // Traverse through the items inside the trvQuestionRanges
            foreach (var node in nodes)
            {
                // With each item, set the item.Checked = false
                node.IsChecked = false;
            }
        }

        public void InitForm()
        {
            var questionRanges = this.trvQuestionRanges.View;
            questionRanges.Nodes.Clear();

            foreach (DataRow row in dataTest.Contents.Rows)
            {
                var key = row["番号"].ToString();
                var value = row["階層1"].ToString();
                var tlNode = new TreeListNode(new KeyValuePair<string, string>(key, value));

                foreach (DataRow row1 in dataTest.Problem.Rows)
                {
                    var key1 = key + ',' + row1["番号"];
                    var value1 = row1["階層1"].ToString();
                    var tlNodeChild = new TreeListNode(new KeyValuePair<string, string>(key1, value1));
                    tlNode.Nodes.Add(tlNodeChild);
                }

                questionRanges.Nodes.Add(tlNode);
            }
        }
    }
}