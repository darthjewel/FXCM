using System;
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

        public Form1()
        {
            InitializeComponent();
        }
    }
}
