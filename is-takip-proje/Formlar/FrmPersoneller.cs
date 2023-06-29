using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using is_takip_proje.Entity;

namespace is_takip_proje.Formlar
{
    public partial class FrmPersoneller : Form
    {
        public FrmPersoneller()
        {
            InitializeComponent();
        }
        DbisTakipEntities db = new DbisTakipEntities();

        // Personeller tablosunu listelemek için kullan
        void Personeller()
        {
            var degerler = from x in db.TblPersonel
                           select new
                           {
                               x.ID,
                               x.Ad,
                               x.Soyad,
                               x.Mail,
                               x.Telefon,
                               x.Departman
                           };
            gridControl1.DataSource = degerler.ToList();
        }
        private void FrmPersoneller_Load(object sender, EventArgs e)
        {
            Personeller();

            // Foreach yardımıyla database'den gelen veriyi departmanlar adında bir listede tut
            var departmanlar = (from x in db.TblDepartmanlar
                                select new
                                {
                                    x.ID,
                                    x.Ad,
                                }).ToList(); ;

            // LookUpEdit compenentinin değerini id görüntülenecek kısmını ad olarak tanımla ve listeyi veri kaynağı olarak ver
            LookUpDepartmanlar.Properties.ValueMember = "ID";
            LookUpDepartmanlar.Properties.DisplayMember = "Ad";
            LookUpDepartmanlar.Properties.DataSource = departmanlar;
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            Personeller();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            // t adında bir tablo türet
            TblPersonel t = new TblPersonel();
            
            // t tablosuna kayıt edilecek verilerin yolunu göster
            t.Ad = TxtAd.Text;
            t.Soyad = TxtSoyAd.Text;
            t.Mail = TxtEmail.Text;
            t.Gorsel = TxtGorsel.Text;
            t.Departman = int.Parse(LookUpDepartmanlar.EditValue.ToString());

            // Database'e veriyi işle değişiklikleri kayıt et
            db.TblPersonel.Add(t);
            db.SaveChanges();

            // İşlemin başarılı olduğunu kullanıcıya göster  
            XtraMessageBox.Show("Personel başarılı bir şekilde kayıt edildi.", 
                "Bilgi",MessageBoxButtons.OK, MessageBoxIcon.Information);

            Personeller();
        }
    }
}
