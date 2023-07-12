using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;
namespace Web_Lk_Xe.Controllers
{
    public class GioHangController : Controller
    {
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        // GET: GioHang
        public gioHang taoGio()
        {
            gioHang cart = (gioHang)Session["CART"];
            return cart;
        }
        // Lưu giỏ hàng

        public void LuuGioHang(gioHang cart)
        {
            Session["CART"] = cart;

        }

        // them vao ggio hang
        [HttpPost]
        public JsonResult ChonMua(string msp)
        {
            gioHang cart = taoGio();
            if (cart == null)
            {
                cart = new gioHang();
                cart.ThemGio(msp);
            }
            else
            {
                iSanPham s = cart.dsLk.FirstOrDefault(t => t.maSp == msp);
                if (s == null)
                {
                    cart.ThemGio(msp);
                }
                else
                {
                    s.slMua++;
                }
            }
            //Them vao session neu khach hang khong dang nhap
            LuuGioHang(cart);
            Session["CART"] = cart;
            // Nếu khách hàng đăng nhập thì thêm sản phẩm vào giỏ hàng csdl luôn
            if (Session["ttDangNhap"]!= null)
            {

                
                KHACHHANG kh = (KHACHHANG)Session["ttkh"];
                GIOHANG tesGio = dl.GIOHANGs.FirstOrDefault(t => t.MASP == msp);
                if(tesGio == null)
                {
                   
                    GIOHANG gioCsdl = new GIOHANG();
                    gioCsdl.MASP = msp;
                    gioCsdl.MAKH = kh.MAKH;
                    gioCsdl.SOLUONGMU = 1;
                    dl.GIOHANGs.InsertOnSubmit(gioCsdl);
                    dl.SubmitChanges();
                }
                else
                {
                    GIOHANG gioCapNhat = new GIOHANG();
                    gioCapNhat = dl.GIOHANGs.FirstOrDefault(t => t.MASP == msp);
                    gioCapNhat.SOLUONGMU += 1;
                    dl.SubmitChanges();
                }
                List<GIOHANG> demSoGioCdsl = dl.GIOHANGs.Where(t => t.MAKH == kh.MAKH).ToList();
                Session["gh_DangNhap"] = demSoGioCdsl.Count();
                return Json(new { code = 500, msg = "Thêm giỏ hàng thành công", soLuong = demSoGioCdsl.Count() }, JsonRequestBehavior.AllowGet);

            }    
            return Json(new { code = 500, msg = "Thêm giỏ hàng thành công", soLuong = cart.DemSL() }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult xemCtGio()
        {
            KHACHHANG kh = (KHACHHANG)Session["ttkh"];

            if (kh == null)
            {
                return RedirectToAction("Account", "User");

            }
            else if (Session["gh_DangNhap"].ToString() == "0")
            {
                return RedirectToAction("gioNull", "GioHang");
            }
            return View();
            
        }

        public ActionResult gioNull()
        {
            return View();
        }

        // Gio Hang theo kieu dang nhap roi
        [HttpGet]
        public JsonResult loadGioHang()
        {
            KHACHHANG kh = (KHACHHANG)Session["ttkh"];
            var dsGios = (from l in dl.GIOHANGs.Where(t => t.MAKH == kh.MAKH)
                          select new
                          {
                              maSP = l.MASP,
                              hinhSp = l.SANPHAM.HINH,
                              tenSp = l.SANPHAM.TENSP,
                              giaSp = l.SANPHAM.DONGIA,
                              sl = l.SOLUONGMU
                          }).ToList();

            return Json(new { code = 500, dsGio = dsGios }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult loadTT()
        {
            KHACHHANG kh = (KHACHHANG)Session["ttkh"];

            List<GIOHANG> gh = dl.GIOHANGs.Where(t => t.MAKH == kh.MAKH).ToList();
            var tong = gh.Sum(t => t.SOLUONGMU * t.SANPHAM.DONGIA);
            var tongSL = gh.Sum(t => t.SOLUONGMU);
            return Json(new { code = 500, tong = tong, tongSL = tongSL }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult xoaSpGioHang(string msp)
        {
            GIOHANG ghCheck = dl.GIOHANGs.FirstOrDefault(t => t.MASP == msp);
            dl.GIOHANGs.DeleteOnSubmit(ghCheck);
            dl.SubmitChanges();
            Session["gh_DangNhap"] = 0;
            return Json(new { code = 500, msg="Xóa sản phẩm thành công" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult suaSpGioHang(string msp, string slnew)
        {
            GIOHANG ghCheck = dl.GIOHANGs.FirstOrDefault(t => t.MASP == msp);
            ghCheck.SOLUONGMU = int.Parse(slnew);
            dl.SubmitChanges();
            return Json(new { code = 500, msg = "Thêm số lượng thành công" }, JsonRequestBehavior.AllowGet);

        }
    }
}