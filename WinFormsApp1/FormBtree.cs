using Candal;
using System;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormBtree : Form
    {
       BtreeFile<PersonKey1> _btreeFile = null;
        private const string fileName3 = @"c:\DATOS\database.k1";

        public FormBtree()
        {
            InitializeComponent();

            _btreeFile = new BtreeFile<PersonKey1>(fileName3);
        }

        private void FormComplex_Load(object sender, EventArgs e)
        {

        }

        private void buttonPopulate_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            _btreeFile.Open();

            long total = _btreeFile.Header.TotalRecords.Value;
            for (long i = 0; i < total; i++)
            {
                BtreeRecord rec = _btreeFile.Read(i + 1);

                ListViewItem item = new ListViewItem();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(rec.LeftIndex.Value.ToString());
                item.SubItems.Add(rec.RightIndex.Value.ToString());
                item.SubItems.Add(rec.RecordID.Value.ToString());
                item.SubItems.Add(Encoding.UTF8.GetString(rec.FreeKey.Value));

                listView1.Items.Add(item);
            }

            _btreeFile.Close();

            listView1.Items[0].Selected = true; ;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int inx = listView1.SelectedItems[0].Index;
                inx++;

                _btreeFile.Open();

                BtreeRecord rec = _btreeFile.Read(inx);

                FileHeaderMount();
                RecordMount(inx, rec);

                _btreeFile.Close();
            }
        }

        private void FileHeaderMount()
        {
            textBox1.Text = _btreeFile.Header.LastDeleted.Value.ToString();
            textBox2.Text = _btreeFile.Header.ActiveRecords.Value.ToString();
            textBox3.Text = _btreeFile.Header.TotalRecords.Value.ToString();
        }

        private void RecordMount(long id, BtreeRecord rec)
        {
            ClearRecord();

            textBoxRecordID.Text = id.ToString();

            if (_btreeFile.IsKeyFound)
            {
                textBox7.Text = rec.LeftIndex.Value.ToString();
                textBox8.Text = rec.RightIndex.Value.ToString();
                textBox9.Text = rec.RecordID.Value.ToString();
                textBox10.Text = Encoding.UTF8.GetString(rec.FreeKey.Value);
            }
            //else if (_btreeFile.IsRecordDeleted)
            //    textBox7.Text = "delete ref: " + _btreeFile.DeletedRecord.LastDeleted.Value.ToString();
        }

        private void ClearRecord()
        {
            textBoxRecordID.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearRecord();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //ComplexRecord rec = _btreeFile.GetRecordInstance();
            //rec.Age.Value = System.Convert.ToInt16(textBox7.Text);
            //rec.Descr.Value = textBox8.Text;
            //rec.Group2.Address.Value = textBox9.Text;
            //rec.Group2.Group1.Id.Value = System.Convert.ToInt64(textBox10.Text);

            //_btreeFile.Open();

            //if (textBoxRecordID.Text == "")
            //{
            //    _btreeFile.AddRecord(rec);
            //}
            //else
            //{
            //    long id = System.Convert.ToInt64(textBoxRecordID.Text);
            //    _btreeFile.UpdateRecord(id, rec);
            //}

            //_btreeFile.Close();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //_btreeFile.Open();

            //if (textBoxRecordID.Text == "")
            //{ 
            //    long id = System.Convert.ToInt64(textBoxRecordID.Text);
            //    _btreeFile.DeleteRecord(id);
            //}

            //_btreeFile.Close();
        }
    }
}
