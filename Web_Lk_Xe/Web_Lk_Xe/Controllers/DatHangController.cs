using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;
namespace Web_Lk_Xe.Controllers
{
    public class DatHangController : Controller
    {
        // GET: DatHang
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        public ActionResult DatHang()
        {
            KHACHHANG kh = (KHACHHANG)Session["ttkh"];
            List<DONVIVANCHUYEN> dsdv = dl.DONVIVANCHUYENs.ToList();
            List<GIOHANG> dsGio = dl.GIOHANGs.Where(t => t.MAKH == kh.MAKH).ToList();
            ViewBag.dsdv = dsdv;
            ViewBag.TongTien = dsGio.Sum(t => t.SOLUONGMU * t.SANPHAM.DONGIA);
            return View(dsGio);
        }

        public JsonResult XlDatHang(string tenNN, string sdtNN, string diaChiNN, string dvVC)
        {
            
            KHACHHANG kh = (KHACHHANG)Session["ttkh"];
            KHACHHANG khs = new KHACHHANG();
            khs = dl.KHACHHANGs.FirstOrDefault(t => t.MAKH == kh.MAKH);
            if (khs != null)
            {
                

                // Tao hoa don
                HOADON hd = new HOADON();
                hd.MAHD = "";
                hd.NGAYMUA = DateTime.Now;
                hd.TINHTRANG = "Đang chờ xử lý";
                hd.MAKH = khs.MAKH;
                hd.DIACHIGH = diaChiNN;
                hd.SDT = sdtNN;
                hd.MADV = dvVC;
                hd.ptTT = "Thanh Toán Khi Nhận Hàng";
                List<GIOHANG> dsgh = dl.GIOHANGs.Where(t => t.MAKH == kh.MAKH).ToList();
                hd.thanhTien = dsgh.Sum(t => t.SANPHAM.DONGIA * t.SOLUONGMU);
                dl.HOADONs.InsertOnSubmit(hd);
                dl.SubmitChanges();

                List<HOADON> dsHdTmp = dl.HOADONs.ToList();
                HOADON tmpHd = new HOADON();
                foreach(var i in dsHdTmp)
                {
                    tmpHd = i;
                }   
                
                foreach (var i in dsgh)
                {
                    CTHD cthd = new CTHD();
                    cthd.MAHD = tmpHd.MAHD;
                    cthd.MASP = i.MASP;
                    cthd.SOLUONGMUA = i.SOLUONGMU;
                    cthd.TONGTIEN = 0;
                    dl.CTHDs.InsertOnSubmit(cthd);

                }
               
                dl.SubmitChanges();
                foreach(var i in  dsgh)

                {
                    dl.GIOHANGs.DeleteOnSubmit(i);
                }
                dl.SubmitChanges();
                Session["gh_DangNhap"] = 0;
                return Json(new { code = 500, msg = "Đặt Hàng Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 200, msg = "Đặt Hàng Thất Bại" }, JsonRequestBehavior.AllowGet);

            }

        }

        

       
    }

   



}