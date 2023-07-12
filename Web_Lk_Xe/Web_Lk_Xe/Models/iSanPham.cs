using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Lk_Xe.Models
{
    public class iSanPham
    {
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        public string maSp { set; get; }
        public string tenSp { set; get; }
        public string hinhAnh { set; get; }
        public float donGia { set; get; }
        public int soluongTon { set; get; }
        public int slMua { set; get; }

        public iSanPham(string msp)
        {

            maSp = msp;
            SANPHAM a = dl.SANPHAMs.FirstOrDefault(t => t.MASP == maSp);
            tenSp = a.TENSP;
            hinhAnh = a.HINH;
            donGia = float.Parse(a.DONGIA.ToString());
            soluongTon = int.Parse(a.SOLUONG.ToString());
            slMua = 1;

        }

    }
}