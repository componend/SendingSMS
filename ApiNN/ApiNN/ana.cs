using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;
using System.Data.Entity.Migrations;

namespace ApiNN
{
    public partial class ana : Form
    {
        public ana()
        {
            InitializeComponent();
        }

        public void gonder_Click(object sender, EventArgs e)
        {
            //xml içerisinde aşağıdaki gibi değerleri gönderebilirsiniz..
            //<zaman>2014-04-17 08:30:00</zaman>//sms gitmeye başlama zamanı 
            //<zamanasimi>2014-04-17 10:30:00</zamanasimi>//son gönderim deneme zamanı 
            if(String.IsNullOrEmpty(mesaj1.Text) || String.IsNullOrEmpty(numara1.Text))
            {
                return;
            }
            string tur = "Normal";
            string smsNN = "data=<sms><kno>" + kno.Text + "</kno><kulad>" + kad.Text + "</kulad><sifre>" + ksifre.Text + "</sifre>" +
            "<gonderen>" + orjinator.Text + "</gonderen>" +
            "<telmesajlar>" +
            "<telmesaj><tel>" + numara1.Text + "</tel><mesaj>" + mesaj1.Text + "</mesaj></telmesaj>" +
            "</telmesajlar>" +
            "<tur>" + tur + "</tur></sms>";
            var result = XmlPost("http://www.oztekbayi.com/panel/smsgonderNNpost.php", smsNN);
            var res = result.Split(':');
            if (res[0] == "2")
            {
                cevap.Text = res[1];
                return;
            }

            var reportId = res[1];

            using (var db = new MyDb())
            {
                var entity = new SmsMessage
                {
                    Message = mesaj1.Text,
                    ReportId = reportId,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = numara1.Text,
                    Status = Enums.MessageStatus.NotDirty

                };
                db.SmsMessages.Add(entity);
                db.SaveChanges();
                cevap.Text = "Mesaj Gönderildi";
                LoadData();
            }                        
        }
        private string XmlPost(string PostAddress, string xmlData)
        {
            using (WebClient wUpload = new WebClient())
            {
                wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
        }


    
        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            MyDb db = new MyDb();
            var query = db.SmsMessages.ToList();
            dataGridView1.DataSource = query;
        }

        private void ana_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var entity = ((List<SmsMessage>)dataGridView1.DataSource)[e.RowIndex];
            string smsRapor = "data=<smsrapor><kulad>" + kad.Text + "</kulad><sifre>" + ksifre.Text + "</sifre>" +
            "<ozelkod>" + entity.ReportId + "</ozelkod></smsrapor>";
            var result = XmlPost("http://www.oztekbayi.com/panel/smstakippost.php", smsRapor);
            var x = result.Substring(0, result.Length - 5);
            var status = x.Split( );
            var db = new MyDb();

            int updateId = 0;
            string cx = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            updateId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            var message = db.SmsMessages.FirstOrDefault(q => q.Id == updateId);
            if (status[2] == "1")
            {
                message.Status = Enums.MessageStatus.Success;
            }else if (status[2] == "2")
            {
                message.Status = Enums.MessageStatus.Failed;
            }
            else if (status[2]=="3")
            {
                message.Status = Enums.MessageStatus.Pending;
            }
            
            db.SaveChanges();
            LoadData();
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            //using (var db = new MyDb())
                //{
                //    if (status[2]=="1")
                //    {

            //        var query = new SmsMessage
            //        {
            //            Status = Enums.MessageStatus.Success,
            //        };
            //    db.SmsMessages.AddOrUpdate(query);
            //}
            //else if (status[2]=="2")
            //{
            //    var query = new SmsMessage
            //    {
            //        Status = Enums.MessageStatus.Failed,
            //    };
            //    db.SmsMessages.AddOrUpdate(query);
            //}
            //else if (status[2]=="3")
            //{
            //    var query = new SmsMessage
            //    {
            //        Status = Enums.MessageStatus.Pending,
            //    };

            //    db.SmsMessages.AddOrUpdate(query);
            //}
            //    db.SaveChanges();
            //    LoadData();
            //}


        }

        private void kad_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
