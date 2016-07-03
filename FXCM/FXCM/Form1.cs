﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fxcore2;


namespace FXCM
{

    public partial class Form1 : Form
    {
        private O2GSession mSession = null;
        private MysessionStatusListener statusListener = null;
        private O2GSessionDescriptorCollection descs = null;
        private O2GTableManager man = null;
        private TableListener tbTableListener = null;
        private ATableListener atl = null;
        private DataSet ds;

        private void AppendText(string text)
        {

            if (this.displayBox.InvokeRequired)
            {
              this.Invoke(new Action<string>(AppendText), new object[] { text });
            }
            else
            {
                this.displayBox.Text += text;
            }
        }
        private void AppendTextNL(string text)
        {

            if (this.displayBox.InvokeRequired)
            {
               this.Invoke(new Action<string>(AppendTextNL), new object[] { text });
            }
            else
            {
                this.displayBox.Text += text+Environment.NewLine;
            }
        }
        public Form1()
        {
            InitializeComponent();
            mSession = O2GTransport.createSession();
            statusListener = new MysessionStatusListener();
            mSession.subscribeSessionStatus(statusListener);
            mSession.useTableManager(O2GTableManagerMode.Yes, null);
            statusListener.PropertyChange += new MysessionStatusListener.PropertyChangeHandler(PropertyHasChanged);
            tbTableListener = new TableListener();
          //  load_records();
            tbTableListener.table = ds.Tables[0];
          //  dataGridView1.DataSource = tbTableListener.table;
            atl = new ATableListener();
        }
    }
}
