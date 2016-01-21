// *******************************************************************************
// System  : TestEditorPlus
// Title   : 書籍選択
// Version : 1.0.0.0
// -------------------------------------------------------------------------------
// Create  : @16/01/12 NGUYỄN MINH THÔNG Create New
// *******************************************************************************

using StudyLinkZ.Core.Helpers;
using StudyLinkZ.TEP.Classes;
using System;
using System.Collections.Generic;
using System.Windows;

namespace StudyLinkZ.TEP.UI.CustomControls
{
    /// <summary>
    /// Interaction logic for S00901
    /// </summary>
    public partial class S00901
    {
        #region Properties and Variables ============================

        /// <summary>
        /// Titles of form.
        /// </summary>
        private string formTitle = "教科・書籍選択";

        /// <summary>
        /// current Offset
        /// </summary>
        private int currentOffset;

        /// <summary>
        /// list Book
        /// </summary>
        private List<Book> listBook;

        /// <summary>
        /// count list book
        /// </summary>
        private int count;

        /// <summary>
        /// total count
        /// </summary>
        private int totalCount;

        #endregion Properties and Variables ============================

        #region Public methods ======================================

        /// <summary>
        /// Initializes a new instance of the S00901 class.
        /// </summary>
        public S00901()
        {
            InitializeComponent();
            cboCategory.SelectionChanged += this.CboCategorySelectionChanged;

            this.currentOffset = 0;

            this.GetBookList();
        }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// </summary>
        /// <value>Is First Load</value>
        public bool FirstLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return this.formTitle;
            }
        }

        #endregion Public methods ======================================

        /// <summary>
        /// Get Book List
        /// </summary>
        private void GetBookList()
        {
            try
            {
                var bookCollection = new Book_Collection(1);
                bookCollection.GetBook_List(string.Empty, this.currentOffset, 10, 1, 0);

                this.count = bookCollection.count;
                this.totalCount = bookCollection.total_count;

                this.listBook = bookCollection.listBook.FindAll(b => b.subject_name.Equals("英語"));

                this.count = this.listBook.Count;
            }
            catch (Exception ex)
            {
                // If there is any problem in calling API (timeout, network…) or API returns the unexpected response
                Common.TepMessageBox(
                string.Format(Properties.Resources.MSG_S037_E0001, "書籍一覧取得", "errorcode", ex.Message),
                this.formTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

                this.count = 0;
                this.totalCount = 0;
                this.listBook = new List<Book>();
            }

            this.lstBook.ItemsSource = this.count == 0 ? new List<Book>() : this.listBook;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the cboCategory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void CboCategorySelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.currentOffset = 0;
            this.GetBookList();
        }

        /// <summary>
        /// lstBook Preview Mouse Wheel
        /// </summary>
        /// <param name="sender">The parent form</param>
        /// <param name="e">The parent form send event</param>
        private void LstBookPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (this.currentOffset - 10 >= 0)
                {
                    this.currentOffset -= 10;
                    this.GetBookList();
                }
            }
            else if (e.Delta <= 0)
            {
                if (this.currentOffset + 10 < this.totalCount)
                {
                    this.currentOffset += 10;
                    this.GetBookList();
                }
            }
        }
    }
}