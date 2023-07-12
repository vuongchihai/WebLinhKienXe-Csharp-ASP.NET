using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;
namespace Web_Lk_Xe.Controllers
{
    public class HienThiSanPhamController : Controller
    {
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        // GET: HienThiSanPham
        public ActionResult HienThiManHinh()
        {
            List<SANPHAM> dsSp = dl.SANPHAMs.Where(t => t.MALOAI == "LP001").ToList();
            return  PartialView(dsSp);
        }

        public ActionResult hTSanPhamTheoDm(string mLoai)
        {

            List<SANPHAM> dsSp = dl.SANPHAMs.Where(t => t.MALOAI == mLoai).ToList();
            LOAISP name = dl.LOAISPs.FirstOrDefault(t => t.MALOAI == mLoai);
            ViewBag.TenLoai = name.TenLoai;
            return View(dsSp);
        }


        public ActionResult xemCTSanPham(string maSp)
        {
            return View();
        }
    }

}