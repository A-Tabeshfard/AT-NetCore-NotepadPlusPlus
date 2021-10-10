﻿using AT_Notepad.WFA.NetCore.Common.Extensions;
using AT_Notepad.WFA.NetCore.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AT_Notepad.WFA.NetCore.UI
{
    public partial class FrmReplace : Form
    {
        #region Field(s)

        private FrmMain _frmMain;

        #endregion

        #region Constructor

        private FrmReplace()
        {
            InitializeComponent();
        }

        public FrmReplace(FrmMain frmMain)
            : this()
        {
            _frmMain = frmMain;
        }

        #endregion

        #region Private Method(s)

        private void UpdateButtons()
        {
            btnFindNext.Enabled =
                btnReplace.Enabled =
                btnReplaceAll.Enabled = (txtFindWhat.Text.Length > 0);
        }

        #endregion

        #region Public Method(s)

        public void Triggered()
        {
            txtFindWhat.Focus();
        }

        #endregion

        #region Event(s) ==> Form

        private void FrmReplace_Load(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void FrmReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        #endregion

        #region Event(s) ==> btnFindNext

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            string SearchText = txtFindWhat.Text;
            bool isMatchCase = checkBoxMachCase.Checked;
            bool isSearchDown = true;

            if (!_frmMain.FindAndSelect(SearchText, isMatchCase, isSearchDown))
            {
                MessageBox.Show(this, CONST.CannotFindMessage.FormatUsingObject(new
                {
                    SearchText = SearchText
                }), "Notepad");
            }
        }

        #endregion

        #region Event(s) ==> btnReplace

        private void btnReplace_Click(object sender, EventArgs e)
        {
            string SearchText = txtFindWhat.Text;
            string ReplaceWithText = txtReplaceWith.Text;
            bool MatchCase = checkBoxMachCase.Checked;
            var eStringComparison = MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
            var bSearchDown = true;

            if (_frmMain.SelectedText.Equals(SearchText))
            {
                _frmMain.SelectedText = ReplaceWithText;
            }

            if (!_frmMain.FindAndSelect(SearchText, MatchCase, bSearchDown))
            {
                MessageBox.Show(this, CONST.CannotFindMessage.FormatUsingObject(new
                {
                    SearchText = SearchText
                }), "Notepad");
            }
        }

        #endregion

        #region Event(s) ==> btnReplaceAll

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            string Content = _frmMain.Content;
            string SearchText = txtFindWhat.Text;
            bool MatchCase = checkBoxMachCase.Checked;

            var Indexes = Helper.GetIndexes(Content, SearchText, MatchCase);

            if (!Indexes.Any())
            {
                MessageBox.Show(this, CONST.CannotFindMessage.FormatUsingObject(new
                {
                    SearchText = SearchText
                }), "Notepad");
                return;
            }

            StringBuilder Builder = new StringBuilder();
            string ReplaceWith = txtReplaceWith.Text;

            {
                int LastIndex = -1;
                foreach (int Index in Indexes)
                {
                    if (Index != 0)
                    {
                        #region TakeStart

                        int TakeStart;

                        if (LastIndex == -1)
                            TakeStart = 0;
                        else
                            TakeStart = LastIndex + SearchText.Length;

                        #endregion

                        int TakeEnd = Index - 1;
                        int Length = (TakeEnd - TakeStart) + 1;

                        string InBetween = Content.Substring(TakeStart, Length);
                        Builder.Append(InBetween);
                    }

                    Builder.Append(ReplaceWith);
                    LastIndex = Index;
                }

                {
                    #region TakeStart

                    int TakeStart;

                    if (LastIndex == -1)
                        TakeStart = 0;
                    else
                        TakeStart = LastIndex + SearchText.Length;

                    #endregion

                    int TakeEnd = Content.Length - 1;
                    int Length = (TakeEnd - TakeStart) + 1;
                    Builder.Append(Content.Substring(TakeStart, Length));
                }
            }

            _frmMain.Content = Builder.ToString();
        }

        #endregion

        #region Event(s) ==> btnCancel

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion

        #region Event(s) ==> txtFindWhat

        private void txtFindWhat_Enter(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            Sender.SelectAll();
        }

        private void txtFindWhat_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        #endregion

        #region Event(s) ==> txtReplaceWith

        private void txtReplaceWith_Enter(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            Sender.SelectAll();
        }

        #endregion
    }
}