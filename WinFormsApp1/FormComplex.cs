using Candal;
using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormComplex : Form
    {
        SequencialFile<ComplexRecord> _seqFile = null;
        private const string fileName3 = @"c:\DATOS\databaseComplex.dat";

        public FormComplex()
        {
            InitializeComponent();
            _seqFile = new SequencialFile<ComplexRecord>(fileName3);
        }

        private void FormComplex_Load(object sender, EventArgs e)
        {

        }

        private void buttonPopulate_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            _seqFile.Open();
            long total = _seqFile.Header.TotalRecords.Value;
            for (long i = 0; i < total; i++)
            {
                ComplexRecord rec = _seqFile.ReadRecord(i + 1);

                ListViewItem item = new ListViewItem();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(rec.Age.Value.ToString());
                item.SubItems.Add(rec.Descr.Value.ToString());
                item.SubItems.Add(rec.Group2.Address.Value.ToString());
                item.SubItems.Add(rec.Group2.Group1.Id.Value.ToString());
                item.SubItems.Add(rec.Group2.Group1.Name.Value.ToString());

                listView1.Items.Add(item);
            }

            _seqFile.Close();

            listView1.Items[0].Selected = true; ;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int inx = listView1.SelectedItems[0].Index;
                inx++;

                _seqFile.Open();

                ComplexRecord rec = _seqFile.ReadRecord(inx);

                FileHeaderMount();
                RecordMount(inx, rec);

                _seqFile.Close();
            }
        }

        private void FileHeaderMount()
        {
            textBox1.Text = _seqFile.Header.LastDeleted.Value.ToString();
            textBox2.Text = _seqFile.Header.ActiveRecords.Value.ToString();
            textBox3.Text = _seqFile.Header.TotalRecords.Value.ToString();
        }

        private void RecordMount(long id, ComplexRecord rec)
        {
            ClearRecord();

            textBoxRecordID.Text = id.ToString();

            if (_seqFile.IsRecordFound)
            {
                textBox7.Text = rec.Age.Value.ToString();
                textBox8.Text = rec.Descr.Value.ToString();
                textBox9.Text = rec.Group2.Address.Value.ToString();
                textBox10.Text = rec.Group2.Group1.Id.Value.ToString();
                textBox11.Text = rec.Group2.Group1.Name.Value.ToString();
            }
            else if (_seqFile.IsRecordDeleted)
                textBox7.Text = "delete ref: " + _seqFile.DeletedRecord.LastDeleted.Value.ToString();
        }

        private void ClearRecord()
        {
            textBoxRecordID.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearRecord();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ComplexRecord rec = _seqFile.GetRecordInstance();
            rec.Age.Value = System.Convert.ToInt16(textBox7.Text);
            rec.Descr.Value = textBox8.Text;
            rec.Group2.Address.Value = textBox9.Text;
            rec.Group2.Group1.Id.Value = System.Convert.ToInt64(textBox10.Text);
            rec.Group2.Group1.Name.Value = textBox11.Text;

            this._seqFile.Open();

            if (textBoxRecordID.Text == "")
            {
                _seqFile.AddRecord(rec);
            }
            else
            {
                long id = System.Convert.ToInt64(textBoxRecordID.Text);
                _seqFile.UpdateRecord(id, rec);
            }

            this._seqFile.Close();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this._seqFile.Open();

            if (textBoxRecordID.Text == "")
            { 
                long id = System.Convert.ToInt64(textBoxRecordID.Text);
                _seqFile.DeleteRecord(id);
            }

            this._seqFile.Close();
        }
    }
}
