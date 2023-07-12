using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Lk_Xe.Models
{
    public class gioHang
    {
        public List<iSanPham> dsLk;

        // khoi tao gio hang
        public gioHang()
        {
            dsLk = new List<iSanPham>();
        }
        public void ThemGio(string maLk)
        {
            iSanPham a = new iSanPham(maLk);
            dsLk.Add(a);

        }

        public int DemSL()
        {
            return dsLk.Count();
        }
        //public int TongTien()
        //{
        //    int TongTien = 0;
        //    foreach (ILinhKien i in dsLk)
        //    {
        //        TongTien += i.donGia * i.slMua;
        //    }
        //    return TongTien;
        //}
        //public void Xoa(string malK)
        //{
        //    var i = dsLk.Single(t => t.maLK == malK);
        //    dsLk.Remove(i);
        //}
        //public void XoaH()
        //{
        //    dsLk.Clear();

        //}
        //public void CapNhatSl(string ma, int sl)
        //{
        //    for (int i = 0; i < dsLk.Count; i++)
        //    {
        //        if (dsLk[i].maLK == ma)
        //        {
        //            dsLk[i].slMua = sl;
        //            break;
        //        }
        //    }
        //}
    }
}