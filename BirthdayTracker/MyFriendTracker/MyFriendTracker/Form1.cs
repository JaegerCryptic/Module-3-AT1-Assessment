///-------------------------------------------------------------------------------------------------
///	Author: Kyle Kent
/// 
/// Student Number: 465510139
///	
/// Purpose: Track peoples upcoming birthdays and their ideal present
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;

namespace MyFriendTracker
{
    ///-------------------------------------------------------------------------------------------------
    /// @class  BirthdayTracker
    ///
    /// @brief  A birthday tracker.
    ///
    /// @date   28/08/2018
    ///-------------------------------------------------------------------------------------------------

    public partial class BirthdayTracker : Form
    {
        // Call GetTable() function



        /// @brief  The table
        DataTable table =  Friends.GetTable();
        

        /// @brief  The find row
        DataTable findRow = Friends.GetTable();


        /// @brief  The find month
        DataTable findMonth = Friends.GetTable();



        /// @brief  The position
        int pos = 0;

        ///-------------------------------------------------------------------------------------------------
        /// @fn public BirthdayTracker()
        ///
        /// @brief  Default constructor
        ///
        /// @date   28/08/2018
        ///-------------------------------------------------------------------------------------------------

        public BirthdayTracker()
        {
            InitializeComponent();

            // Get row information through index
            ShowData(table, pos);

            // Display list of all brithdays
            lstFriends.DataSource = table;
            SizeDGV(lstFriends);
            lstFriends.ClearSelection();
            lstFriends.Rows[pos].Selected = true;
            lstFriends.CurrentCell = lstFriends[0, pos];

        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn void SizeDGV(DataGridView dgv)
        ///
        /// @brief  Size dgv
        ///
        /// @date   28/08/2018
        ///
        /// @param  dgv The dgv.
        ///-------------------------------------------------------------------------------------------------

        void SizeDGV(DataGridView dgv)
        {
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn public void ShowData(DataTable dgv, int index)
        ///
        /// @brief  Shows the data
        ///
        /// @date   28/08/2018
        ///
        /// @param  dgv     The dgv.
        /// @param  index   Zero-based index of the.
        ///-------------------------------------------------------------------------------------------------

        public void ShowData(DataTable dgv, int index)
        {
            txtPersonName.Text = Convert.ToString(dgv.Rows[index][0]);
            txtLikes.Text = Convert.ToString(dgv.Rows[index][1]);
            txtDislikes.Text = Convert.ToString(dgv.Rows[index][2]);
            txtBDay.Text = Convert.ToString(dgv.Rows[index][3]);
            txtBMonth.Text = Convert.ToString(dgv.Rows[index][4]);
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn public static string UpperCase(string s)
        ///
        /// @brief  Upper case
        ///
        /// @date   28/08/2018
        ///
        /// @param  s   The string.
        ///
        /// @return A string.
        ///-------------------------------------------------------------------------------------------------

        public static string UpperCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnExit_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnExit for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the application?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // save all records upon closing the application.

                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                                   Select(column => column.ColumnName).
                                                   ToArray();

                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in table.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText("MyFriendData.csv", sb.ToString());

                // close application upon user click or pressing the key Esc.
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                // if user selects "no" nothing is done
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnNew_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnNew for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnNew_Click(object sender, EventArgs e)
        {
            // Take user typed values and store them for addition into datatable
            string person = txtPersonName.Text.ToString();
            string likes = txtLikes.Text.ToString();
            string dislikes = txtDislikes.Text.ToString();
            int bDay;
            int bMonth;


            if (person.Any(char.IsUpper) && int.TryParse(txtBDay.Text, out bDay) && int.TryParse(txtBMonth.Text, out bMonth))
            {
                bDay = Convert.ToInt32(txtBDay.Text);
                bMonth = Convert.ToInt32(txtBMonth.Text);

                if (bMonth >= 1 && bMonth <= 12)
                {
                    table.Rows.Add(person, likes, dislikes, bDay, bMonth);

                    // Update and refresh datagrid to view the results
                    lstFriends.DataSource = table;
                    lstFriends.Update();
                    lstFriends.Refresh();

                    pos = table.Rows.Count - 1;
                    ShowData(table, pos);
                    lstFriends.ClearSelection();
                    lstFriends.Rows[pos].Selected = true;
                    lstFriends.CurrentCell = lstFriends[0, pos];
                }
             
            }
            else
            {
                MessageBox.Show("Invalid Format. Make sure your entries are valid", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnSave_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnSave for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newPerson = txtPersonName.Text.ToString();
            string newLikes = txtLikes.Text.ToString();
            string newDislikes = txtDislikes.Text.ToString();
            int newBDay;
            int newBMonth;

            if (newPerson.Any(char.IsUpper) && int.TryParse(txtBDay.Text, out newBDay) && int.TryParse(txtBMonth.Text, out newBMonth))
            {
                newBDay = Convert.ToInt32(txtBDay.Text);
                newBMonth = Convert.ToInt32(txtBMonth.Text);

                if (newPerson.Any(char.IsUpper) && newBMonth >= 1 && newBMonth <= 12)
                {
                    if(lstFriends.DataSource == table)
                    {
                        // Checking if text boxes are = to table row
                        table.Rows[pos]["Person"] = newPerson;
                        table.Rows[pos]["Likes"] = newLikes;
                        table.Rows[pos]["Dislikes"] = newDislikes;
                        table.Rows[pos]["Day"] = newBDay;
                        table.Rows[pos]["Month"] = newBMonth;
                    }
                    else if (lstFriends.DataSource == findRow)
                    {
                        string person = findRow.Rows[pos]["Person"].ToString();

                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["Person"].ToString() == person)
                            {
                                dr["Person"] = newPerson;
                                dr["Likes"] = newLikes;
                                dr["Dislikes"] = newDislikes;
                                dr["Day"] = newBDay;
                                dr["Month"] = newBMonth;


                                lstFriends.DataSource = table;
                                lstFriends.Update();
                                lstFriends.Refresh();

                                ShowData(table, 0);
                                lstFriends.ClearSelection();
                                lstFriends.Rows[pos].Selected = true;
                                lstFriends.CurrentCell = lstFriends[0, 0];
                            }
                        }
                    }
                    else
                    {
                        string person = findMonth.Rows[pos]["Person"].ToString();

                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["Person"].ToString() == person)
                            {
                                dr["Person"] = newPerson;
                                dr["Likes"] = newLikes;
                                dr["Dislikes"] = newDislikes;
                                dr["Day"] = newBDay;
                                dr["Month"] = newBMonth;

                                lstFriends.DataSource = table;
                                lstFriends.Update();
                                lstFriends.Refresh();

                                ShowData(table, 0);
                                lstFriends.ClearSelection();
                                lstFriends.Rows[pos].Selected = true;
                                lstFriends.CurrentCell = lstFriends[0, 0];
                            }
                        }
                    }
                    // Saving additions

                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                                       Select(column => column.ColumnName).
                                                       ToArray();

                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in table.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }

                    File.WriteAllText("MyFriendData.csv", sb.ToString());

                    // Saving deletions
                    string deletePersons = txtPersonName.Text;

                    List<String> lines = new List<string>();
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("MyFriendData.csv");

                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    lines.RemoveAll(l => l.Contains(deletePersons));
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid Format. Make sure your entries are valid.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnDelete_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnDelete for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete selected friend record.

            // User confirms choice
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this person?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string person = Convert.ToString(txtPersonName.Text);


                // removes selected row from table
                for (int i = table.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = table.Rows[i];
                    
                    // if the value is found in table, the row is deleted
                    if (dr[0].ToString() == person)
                    {
                        table.Rows.Remove(dr);
                        table.AcceptChanges();

                        lstFriends.DataSource = table;
                        lstFriends.Update();
                        lstFriends.Refresh();

                        pos = 0;

                        ShowData(table, 0);
                        lstFriends.ClearSelection();
                        lstFriends.Rows[pos].Selected = true;
                        lstFriends.CurrentCell = lstFriends[0, pos];

                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                // if user selects "no" nothing is done
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnFind_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnFind for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnFind_Click(object sender, EventArgs e)
        {
            
            // Find existing friend record. 
            string search = txtFind.Text;

            var filtered = table.AsEnumerable()
            .Where(r => r.Field<String>("Person").Contains(UpperCase(search)));


            if (!filtered.Any())
            {
                MessageBox.Show("No Results Found", "Empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                findRow = filtered.CopyToDataTable();
                lstFriends.DataSource = findRow;
                ShowData(findRow, pos = 0);
                SizeDGV(lstFriends);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnLast_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnLast for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnLast_Click(object sender, EventArgs e)
        {
            // Find last friend record.
            if(lstFriends.DataSource == table)
            {
                pos = table.Rows.Count - 1;

                ShowData(table, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if(lstFriends.DataSource == findRow)
            {
                pos = findRow.Rows.Count - 1;

                ShowData(findRow, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else
            {
                pos = findMonth.Rows.Count - 1;

                ShowData(findMonth, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }

            
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnNext_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnNext for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Find next friend record.
            pos++;
            if(lstFriends.DataSource == table && pos < lstFriends.Rows.Count)
            {
                ShowData(table, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if(lstFriends.DataSource == findRow && pos < lstFriends.Rows.Count)
            {
                ShowData(findRow, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if (pos < lstFriends.Rows.Count && lstFriends.DataSource == findMonth)
            {
                ShowData(findMonth, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else
            {
                MessageBox.Show("This is the last entry.");
                pos = lstFriends.Rows.Count - 1;
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnPrevious_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnPrevious for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // Find previous friend record.
            pos--;
            if (pos >= 0 && lstFriends.DataSource == table)
            {
                ShowData(table,pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if(pos >= 0 && lstFriends.DataSource == findRow)
            {
                ShowData(findRow, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if (pos >= 0 && lstFriends.DataSource == findMonth)
            {
                ShowData(findMonth, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else
            {
                MessageBox.Show("This is the first entry.");
                pos = 0;
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnFirst_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnFirst for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnFirst_Click(object sender, EventArgs e)
        {
            // Find first friend record.
            pos = 0;
           

            if (lstFriends.DataSource == table)
            {
                ShowData(table,pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else if (lstFriends.DataSource == findRow)
            {
                ShowData(findRow,pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
            else
            {
                ShowData(findMonth, pos);
                lstFriends.ClearSelection();
                lstFriends.Rows[pos].Selected = true;
                lstFriends.CurrentCell = lstFriends[0, pos];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// @fn private void btnBirthdaysInMonth_Click(object sender, EventArgs e)
        ///
        /// @brief  Event handler. Called by btnBirthdaysInMonth for click events
        ///
        /// @date   28/08/2018
        ///
        /// @param  sender  Source of the event.
        /// @param  e       Event information.
        ///-------------------------------------------------------------------------------------------------

        private void btnBirthdaysInMonth_Click(object sender, EventArgs e)
        {
           
            int search;

            if (int.TryParse(txtBinarySearch.Text, out search))
            {
                search = Convert.ToInt32(txtBinarySearch.Text);

                if (search >= 1 && search <= 12)
                {

                    var filtered = table.AsEnumerable()
                    .Where(r => r.Field<int>("Month") == search);

                    var result = filtered;

                    if (!filtered.Any())
                    {
                        MessageBox.Show("No Results Found", "Empty",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        findMonth = filtered.CopyToDataTable();
                        lstFriends.DataSource = findMonth;
                        ShowData(findMonth, pos = 0);
                        SizeDGV(lstFriends);
                        lstFriends.ClearSelection();
                        lstFriends.Rows[pos].Selected = true;
                        lstFriends.CurrentCell = lstFriends[0, pos];
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Entry. Make sure all entries are valid.", "Invalid Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Invalid Entry. Make sure all entries are valid.", "Invalid Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BirthdayTracker_Load(object sender, EventArgs e)
        {

        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// @class  Friends
    ///
    /// @brief  A friends.
    ///
    /// @date   28/08/2018
    ///-------------------------------------------------------------------------------------------------

    public class Friends : Form
    {
        ///-------------------------------------------------------------------------------------------------
        /// @fn public static DataTable GetTable()
        ///
        /// @brief  Gets the table
        ///
        /// @date   28/08/2018
        ///
        /// @return The table.
        ///-------------------------------------------------------------------------------------------------

        public static DataTable GetTable()
        {
            // Initialize datatable for use
            DataTable csvTable = new DataTable();

            // Add appropriate columns
            csvTable.Columns.Add("Person", typeof(string)).SetOrdinal(0);
            csvTable.Columns.Add("Likes", typeof(string)).SetOrdinal(1);
            csvTable.Columns.Add("Dislikes", typeof(string)).SetOrdinal(2);
            csvTable.Columns.Add("Day", typeof(int)).SetOrdinal(3);
            csvTable.Columns.Add("Month", typeof(int)).SetOrdinal(4);

            // Set file path for CSV file
            string filePath = "MyFriendData.csv";

            //Read CSV file
            StreamReader streamReader = new StreamReader(filePath);
            string[] totalData = new string[File.ReadAllLines(filePath).Length];

            totalData = streamReader.ReadLine().Split(',');

            while (!streamReader.EndOfStream)
            {
                totalData = streamReader.ReadLine().Split(',');
                csvTable.Rows.Add(totalData[0], totalData[1], totalData[2], totalData[3], totalData[4]);
            }
            streamReader.Close();

            return csvTable;
        }
    }
}

