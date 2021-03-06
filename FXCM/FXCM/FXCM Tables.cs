﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fxcore2;
using System.Data;

namespace FXCM
{
    class FXCM_Tables
    {
    }
    public partial class Form1 : Form
    {
        public void PropertyHasChanged(object sender, PropertyChangeEventArgs data)
        {
            if (data.PropertyName == "Name")
            {
                try
                {
                    AppendTextNL("The Application status : " + data.NewValue + " !"); //cross thread
                    if((O2GSessionStatusCode)data.NewValue == O2GSessionStatusCode.Connected)
                    {
                        man = mSession.getTableManager();
                        AppendTextNL(man.getStatus().ToString());
                    }
                }
                catch (Exception oval)
                {

                    Console.WriteLine(oval.Message);
                }

                //   Console.WriteLine("The Application connected!");
                //"New value: '" + (string)data.NewValue + "' was: '" + (string)data.OldValue + "'");

            }
        }
        public class PropertyChangeEventArgs : EventArgs
        {
            public string PropertyName { get; internal set; }
            public object OldValue { get; internal set; }
            public object NewValue { get; internal set; }

            public PropertyChangeEventArgs(string propertyName, object oldValue, object newValue)
            {
                this.PropertyName = propertyName;
                this.OldValue = oldValue;
                this.NewValue = newValue;
            }
        }
        public class MysessionStatusListener : IO2GSessionStatus
        {
            public string error;
            private O2GSessionStatusCode _status;

            public O2GSessionStatusCode status
            {
                get { return _status; }
                set
                {
                    if (this._status != value)
                    {
                        Object old = this._status;
                        _status = value;
                        //     OnPropertyChange(this, new PropertyChangeEventArgs("Name", old, value));
                    }
                }

            }

            public void onLoginFailed(string error)
            {
                this.error = error;

            }

            public void onSessionStatusChanged(O2GSessionStatusCode status)
            {
                this.status = status;

                if (PropertyChange != null)
                {
                    PropertyChangeEventArgs data = new PropertyChangeEventArgs("Name", status, status);
                    // Call the Event
                    PropertyChange(this, data);
                }
            }

            // Delegate
            public delegate void PropertyChangeHandler(object sender, PropertyChangeEventArgs data);

            // The event
            public event PropertyChangeHandler PropertyChange;

            public void OnPropertyChange(object sender, PropertyChangeEventArgs data)
            {
                // Check if there are any Subscribers
                if (PropertyChange != null)
                {
                    // Call the Event
                    PropertyChange(this, data);
                }
            }
        }

        public class ATableListener : IO2GTableListener
        {
            public DataTable table;

            public void onAdded(string rowID, O2GRow rowData)
            {
                throw new NotImplementedException();
            }

            public void onChanged(string rowID, O2GRow rowData)
            {
                throw new NotImplementedException();
            }

            public void onDeleted(string rowID, O2GRow rowData)
            {
                throw new NotImplementedException();
            }

            public void onStatusChanged(O2GTableStatus status)
            {
                throw new NotImplementedException();
            }
        }

        public class TableListener : IO2GTableListener
        {
            public DataTable table;

            public TableListener()
            {
                table = new DataTable();
                this.table.Columns.Add("Instrument", typeof(string));
                this.table.Columns.Add("Bid", typeof(double));
                this.table.Columns.Add("Ask", typeof(double));
                table.Rows.Add("Instrument", 0, 0);
            }
            public void onAdded(string rowID, O2GRow rowData)
            {
                Console.WriteLine("added");

            }

            public void onChanged(string rowID, O2GRow rowData)
            {
                O2GOfferRow offerRow = (O2GOfferRow)rowData;
                if (offerRow != null)
                {
                    try
                    {

                        DataRow[] customerRow = table.Select(@"instrument= '" + offerRow.Instrument + "'");
                        customerRow[0]["Instrument"] = offerRow.Instrument;
                        customerRow[0]["Bid"] = offerRow.Bid;
                        customerRow[0]["Ask"] = offerRow.Ask;

                    }
                    catch (Exception oval)
                    {

                        //   Console.WriteLine(oval.Message);
                    }

                }
            }

            public void onDeleted(string rowID, O2GRow rowData)
            {
                Console.WriteLine("deleted");
            }

            public void onStatusChanged(O2GTableStatus status)
            {
                Console.WriteLine("status changed");
            }
        }
        public void load_account_data(O2GTableManager tableManager, ATableListener tbTableListener)
        {
            O2GTableManagerStatus ts = tableManager.getStatus();
            if (ts == O2GTableManagerStatus.TablesLoaded)
            {
                O2GAccountsTable accountsTable = (O2GAccountsTable)tableManager.getTable(O2GTableType.Accounts);

                //   display.Text += accountsTable.ToString();
                O2GAccountTableRow accountTableRow = null;
                O2GTableIterator tableIterator = new O2GTableIterator(); //{0, 0, null};
                while (accountsTable.getNextRow(tableIterator, out accountTableRow))
                {
                    AppendText(
                        "Account ID : " + accountTableRow.AccountID + Environment.NewLine
                       + "Account Name : " + accountTableRow.AccountName + Environment.NewLine
                       + "Balance : " + accountTableRow.Balance + Environment.NewLine
                       + "Equity : " + accountTableRow.Equity.ToString() + Environment.NewLine
                       + "Usable Margin : " + accountTableRow.UsableMargin.ToString() + Environment.NewLine
                       + "Used Margin : " + accountTableRow.UsedMargin.ToString() + Environment.NewLine);

                }

                //    accountsTable.subscribeUpdate(O2GTableUpdateType.Update, tbTableListener);
            }
            else
            {
                AppendTextNL("cannot get account data :" + ts.ToString());

            }
        }
    }

}