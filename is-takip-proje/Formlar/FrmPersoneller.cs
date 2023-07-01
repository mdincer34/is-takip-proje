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
                               departman = x.TblDepartmanlar.Ad,
                               x.Durum
                           };
            gridControl1.DataSource = degerler.Where(x=> x.Durum==true).ToList();
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

            // LookUpEdit compenentinin değerini id, görüntülenecek kısmını ad olarak tanımla ve listeyi veri kaynağı olarak ver
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

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TxtID.Text);
            var deger = db.TblPersonel.Find(id);
            deger.Durum = false;
            db.SaveChanges();

            XtraMessageBox.Show("Personel başarılı bir şekilde silindi, silinen personellerin listesinden silinmiş tüm personellere ulaşabilirsiniz...",
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Personeller();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            TxtID.Text = gridView1.GetFocusedRowCellValue("ID").ToString();
            TxtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
            TxtSoyAd.Text = gridView1.GetFocusedRowCellValue("Soyad").ToString();
            TxtEmail.Text = gridView1.GetFocusedRowCellValue("Mail").ToString();
            // TxtGorsel.Text = gridView1.GetFocusedRowCellValue("Gorsel").ToString();
            LookUpDepartmanlar.Text = gridView1.GetFocusedRowCellValue("Departman").ToString();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int x = int.Parse(TxtID.Text);
            var deger = db.TblPersonel.Find(x);
            deger.Ad = TxtAd.Text;
            deger.Soyad = TxtSoyAd.Text;
            deger.Mail = TxtEmail.Text;
            deger.Gorsel = TxtGorsel.Text;
            deger.Departman = int.Parse(LookUpDepartmanlar.EditValue.ToString());
            
            db.SaveChanges();

            XtraMessageBox.Show("Personel güncelleme işlemi başarıyla gerçekleşti.",
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Personeller();
        }
    }
}
