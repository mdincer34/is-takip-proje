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
    public partial class FrmDepartmanlar : Form
    {
        public FrmDepartmanlar()
        {
            InitializeComponent();
            Listele();
        }

        DbisTakipEntities db = new DbisTakipEntities();
        void Listele()
        {
            var degerler = (from x in db.TblDepartmanlar
                            select new
                            {
                                x.ID,
                                x.Ad
                            }).ToList();
            gridControl1.DataSource = degerler;
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            TblDepartmanlar t = new TblDepartmanlar();
            t.Ad = TxtAd.Text;
            db.TblDepartmanlar.Add(t);
            db.SaveChanges();
            XtraMessageBox.Show("Departman başarılı bir şekilde kayıt edildi",
                "Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Listele();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (TxtID.Text != "")
            {
                int id = int.Parse(TxtID.Text);
                var deger = db.TblDepartmanlar.Find(id);
                db.TblDepartmanlar.Remove(deger);
                db.SaveChanges();
                XtraMessageBox.Show("Departman silme işlemi başarılı bir şekilde gerçekleşti",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Listele();
            }
            else
            {
                XtraMessageBox.Show("Lütfen silmek istediğiniz departmana ait ID değerini giriniz.",
                    "Departman ID boş olamaz!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            TxtID.Text = gridView1.GetFocusedRowCellValue("ID").ToString();
            TxtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = int.Parse(TxtID.Text);
            var deger = db.TblDepartmanlar.Find(id);
            deger.Ad = TxtAd.Text;
            db.SaveChanges();
            XtraMessageBox.Show("Departman güncelleme işlemi başarılı bir şekilde gerçekleşti",
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Listele();
        }
    }
}
