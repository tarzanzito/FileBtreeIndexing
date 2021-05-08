using Candal;
using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        SequencialFile<ComplexRecord> _seqFile = null;

        public Form1()
        {
            InitializeComponent();
            _seqFile = new SequencialFile<ComplexRecord>(fileName3);
        }

        private const string fileName3 = @"c:\DATOS\databaseComplex.dat";

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

        }

        private void buttonLoadRegs_Click(object sender, EventArgs e)
        {
            // create
            _seqFile.Create();

            _seqFile.Open();

            ComplexRecord rec1 = new ComplexRecord();
            rec1.Age.Value = 21;
            rec1.Descr.Value = "Description 1";
            rec1.Group2.Address.Value = "Road 1";
            rec1.Group2.Group1.Id.Value = 11;
            rec1.Group2.Group1.Name.Value = "Alfa";

            //adds
            long recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 22;
            rec1.Descr.Value = "Description2";
            rec1.Group2.Address.Value = "Road 2";
            rec1.Group2.Group1.Id.Value = 12;
            rec1.Group2.Group1.Name.Value = "Bravo";

            recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 23;
            rec1.Descr.Value = "Description 3";
            rec1.Group2.Address.Value = "Road 3";
            rec1.Group2.Group1.Id.Value = 13;
            rec1.Group2.Group1.Name.Value = "Charley"; ;

            recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 24;
            rec1.Descr.Value = "Description 4";
            rec1.Group2.Address.Value = "Road 4";
            rec1.Group2.Group1.Id.Value = 14;
            rec1.Group2.Group1.Name.Value = "Delta"; ;

            recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 25;
            rec1.Descr.Value = "Description 5";
            rec1.Group2.Address.Value = "Road 5";
            rec1.Group2.Group1.Id.Value = 15;
            rec1.Group2.Group1.Name.Value = "Echo";

            recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 26;
            rec1.Descr.Value = "Description 6";
            rec1.Group2.Address.Value = "Road 6";
            rec1.Group2.Group1.Id.Value = 16;
            rec1.Group2.Group1.Name.Value = "Foxtrot";

            recordID = _seqFile.AddRecord(rec1);

            rec1.Age.Value = 27;
            rec1.Descr.Value = "Description 7";
            rec1.Group2.Address.Value = "Road 7";
            rec1.Group2.Group1.Id.Value = 17;
            rec1.Group2.Group1.Name.Value = "Golf";

            recordID = _seqFile.AddRecord(rec1);

            //updates
            rec1.Age.Value = 31;
            rec1.Descr.Value = "Description 31";
            rec1.Group2.Address.Value = "Road 31";
            rec1.Group2.Group1.Id.Value = 1;
            rec1.Group2.Group1.Name.Value = "Zulu";

            recordID = _seqFile.UpdateRecord(3, rec1);

            //deletes
            recordID = _seqFile.DeleteRecord(2);
            recordID = _seqFile.DeleteRecord(4);

            ////adds
            //rec1.Age.Value = 32;
            //rec1.Descr.Value = "Description 32";
            //rec1.Group2.Address.Value = "Road 32";
            //rec1.Group2.Group1.Id.Value = 2;
            //rec1.Group2.Group1.Name.Value = "Yankee";

            //recordID = _seqFile.AddRecord(rec1);


            //rec1.Age.Value = 33;
            //rec1.Descr.Value = "Description 33";
            //rec1.Group2.Address.Value = "Road 33";
            //rec1.Group2.Group1.Id.Value = 3;
            //rec1.Group2.Group1.Name.Value = "Wiskey";

            //recordID = _seqFile.AddRecord(rec1);

            _seqFile.Close();

            buttonPopulate_Click(sender, e);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int inx = listView1.SelectedItems[0].Index;

                _seqFile.Open();
                ComplexRecord rec = _seqFile.ReadRecord(inx + 1);

                textBox1.Text = _seqFile.Header.LastDeleted.Value.ToString();
                textBox2.Text = _seqFile.Header.ActiveRecords.Value.ToString();
                textBox3.Text = _seqFile.Header.TotalRecords.Value.ToString();

                if (_seqFile.IsRecordFound)
                {
                    textBox4.Text = "Age: "+ rec.Age.Value.ToString() + Environment.NewLine
                    + "Descr: "+ rec.Descr.Value.ToString() + Environment.NewLine
                    + "Group2.Address: " + rec.Group2.Address.Value.ToString() + Environment.NewLine
                    + "Group2.Group1.Id: " + rec.Group2.Group1.Id.Value.ToString() + Environment.NewLine
                    + "Group2.Group1.Name: " + rec.Group2.Group1.Name.Value.ToString() + Environment.NewLine;


                }
                else if (_seqFile.IsRecordDeleted)
                    textBox4.Text = "delete next: " + _seqFile.DeletedRecord.LastDeleted.Value.ToString();


                _seqFile.Close();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
        }
    }
}
