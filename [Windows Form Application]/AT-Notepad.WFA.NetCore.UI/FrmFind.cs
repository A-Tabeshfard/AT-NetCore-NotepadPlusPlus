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
    public partial class FrmFind : Form
    {
        #region Field(s)

        private readonly FrmMain _frmMain;

        #endregion

        #region Constructor

        private FrmFind()
        {
            InitializeComponent();
        }

        public FrmFind(FrmMain frmMain)
            : this()
        {
            _frmMain = frmMain;
        }


        #endregion

        #region Private Method(s)

        private void UpdateFindNextButton()
        {
            btnFindNext.Enabled = (txtFindWhat.Text.Length > 0);
        }

        #endregion

        #region public Method(s)

        public void Triggered()
        {
            txtFindWhat.Focus();
        }

        public new void Show(IWin32Window window = null)
        {
            txtFindWhat.Focus();
            txtFindWhat.SelectAll();

            if (window == null)
                base.Show();
            else
                base.Show(window);
        }

        #endregion

        #region Event(s) ==> Form

        private void FrmFind_Load(object sender, EventArgs e)
        {
            UpdateFindNextButton();
        }

        private void FrmFind_FormClosing(object sender, FormClosingEventArgs e)
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
            bool isRadioDown = radioDown.Checked;

            if (!_frmMain.FindAndSelect(SearchText, isMatchCase, isRadioDown))
            {
                MessageBox.Show(this, CONST.CannotFindMessage.FormatUsingObject(new
                {
                    SearchText = SearchText
                }), "Notepad");
            }
        }

        #endregion

        #region Event(s) ==> btnCancel

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion

        #region Event(s) ==> txtFindWhat

        private void txtFindWhat_TextChanged(object sender, EventArgs e)
        {
            UpdateFindNextButton();
        }

        private void txtFindWhat_Enter(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            Sender.SelectAll();
        }

        #endregion
    }
}