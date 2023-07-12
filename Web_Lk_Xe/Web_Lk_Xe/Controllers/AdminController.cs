using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;
namespace Web_Lk_Xe.Controllers
{
    public class AdminController : Controller
    {


        // GET: Admin
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        public ActionResult Index()
        {

            return View();
        }

        // Quản lý sản phẩm
        public ActionResult QLSanPham()
        {
            List<LOAISP> dsLoai = dl.LOAISPs.ToList();
            return View(dsLoai);
        }
        public JsonResult LoadSanPham()
        {
            var dssp = (from l in dl.SANPHAMs
                        join lSp in dl.LOAISPs on l.MALOAI equals lSp.MALOAI
                        select new
                        {
                            MASP = l.MASP,
                            TENSP = l.TENSP,
                            Hinh = l.HINH,
                            SOLUONG = l.SOLUONG,
                            DONGIA = l.DONGIA,
                            loaiSp = lSp.TenLoai
                        }).ToList();


            return Json(new { code = 500, dssp = dssp }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult loadttSp(string msp)
        {
            var l = dl.SANPHAMs.FirstOrDefault(t => t.MASP == msp);
            return Json(new { code = 500, MASP = l.MASP, tenSP = l.TENSP, Sl = l.SOLUONG, dg = l.DONGIA, loai = l.MALOAI, tenL = l.LOAISP.TenLoai }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SuaSanPham(string masp, string tensp, string hinh, string sl, string dg, string maLoai)
        {
            SANPHAM s = dl.SANPHAMs.FirstOrDefault(t => t.MASP == masp);
            s.TENSP = tensp;
            s.HINH = hinh;
            s.SOLUONG = int.Parse(sl);
            s.DONGIA = float.Parse(dg);
            dl.SubmitChanges();
            return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult xoaSp(string masp)
        {
            CTHD ct = dl.CTHDs.FirstOrDefault(t => t.MASP == masp);
            if (ct != null)
            {
                return Json(new { code = 200, msg = "Tồn tại một hóa đơn nên không thể xóa sản phẩm" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SANPHAM sp = dl.SANPHAMs.FirstOrDefault(t => t.MASP == masp);
                dl.SANPHAMs.DeleteOnSubmit(sp);
                dl.SubmitChanges();
                return Json(new { code = 500, msg = "Xóa sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
            }



        }
        public JsonResult themSP(string masp, string tensp, string hinh, string sl, string dg, string maLoai, string nCC)
        {
            SANPHAM s = new SANPHAM();
            s.MASP = masp;
            s.TENSP = tensp;
            s.HINH = hinh;
            s.SOLUONG = int.Parse(sl);
            s.DONGIA = float.Parse(dg);
            s.MALOAI = maLoai;
            s.MANCC = nCC;
            s.MOTA = "";
            dl.SANPHAMs.InsertOnSubmit(s);
            dl.SubmitChanges();
            return Json(new { code = 500, msg = "Thêm sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
        }

        // end quản lý sản phẩm




        // Quan ly hoa don
        public ActionResult QLHoaDon()
        {

            return View();
        }

        [HttpGet]
        public JsonResult loadHoaDon()
        {
            var hd_Ds = (from l in dl.HOADONs
                        select new
                        {
                            mAHD = l.MAHD,
                            ngayMua = l.NGAYMUA.ToString(),
                            tt = l.TINHTRANG,
                            thanhTien = l.thanhTien,
                            diaChi = l.DIACHIGH,
                            sdt = l.SDT,
                            pt = l.ptTT,
                            makh_hd = l.MAKH,
                            vc_hd = l.MADV,
                            
                        }).ToList();


            return Json(new { code = 500, hd_Ds = hd_Ds }, JsonRequestBehavior.AllowGet);
        }   
        
        public JsonResult loadCTHD(string ma_hd)
        {

            var ct_ds = (from l in dl.CTHDs where l.MAHD == ma_hd
                         join sp in dl.SANPHAMs on l.MASP equals sp.MASP
                         select new
                         {
                             TenSp = sp.TENSP,
                             Hinh = sp.HINH,
                             donGia = sp.DONGIA,
                             sl = sp.SOLUONG,
                             tongCtSp = l.TONGTIEN
                         }).ToList();
            return Json(new { code = 500, ct_ds = ct_ds }, JsonRequestBehavior.AllowGet);

        }



        // sửa hd
        [HttpPost]
        public JsonResult loadTTHD(string mahd)
        {
            var hd_Ds = dl.HOADONs.FirstOrDefault(t => t.MAHD == mahd);

            return Json(new { code = 500, tt = hd_Ds.TINHTRANG }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult suaHD(string mahd, string tt)
        {
            HOADON hd = dl.HOADONs.FirstOrDefault(t => t.MAHD == mahd);
            hd.TINHTRANG = tt;
            dl.SubmitChanges();
            return Json(new { code = 500, msg="Cập nhật trạng thái thành công" }, JsonRequestBehavior.AllowGet);
        }



        //Xóa hóa đơn
        //[HttpPost]
        public JsonResult xoaHd(string maHd)
        {
            List<CTHD> dsCt = dl.CTHDs.Where(t => t.MAHD == maHd).ToList() ;
            dl.CTHDs.DeleteAllOnSubmit(dsCt);
            dl.SubmitChanges();
            HOADON hd = dl.HOADONs.FirstOrDefault(t => t.MAHD == maHd);
            dl.HOADONs.DeleteOnSubmit(hd);
            dl.SubmitChanges();
            return Json(new { code = 500, msg = "Xóa hóa đơn thành công" }, JsonRequestBehavior.AllowGet);

        }




        // Quan ly tai khoan
        public ActionResult QLKhachHang()
        {
            return View();
        }
        public JsonResult LoadKhachHang()
        {
            var dskh = (from l in dl.KHACHHANGs
                        select new
                        {
                            MAKH = l.MAKH,
                            TENKH = l.TENKH,
                            NGAYSINH = l.NGAYSINH,
                            TENDN = l.TENDN,
                            GIOTINH = l.GIOTINH,
                            SDT = l.SDT,
                        }).ToList();

            return Json(new { code = 500, dskh = dskh }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult xemTK(string ma_kh)
        {
            KHACHHANG kh = dl.KHACHHANGs.FirstOrDefault(t => t.MAKH == ma_kh);
            TAIKHOAN tk = dl.TAIKHOANs.FirstOrDefault(t => t.TENDN == kh.TENDN);
            return Json(new { code = 500, tdn = tk.TENDN, pass = tk.PASS }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult suatk(string tks, string pass)
        {
            TAIKHOAN tk = dl.TAIKHOANs.FirstOrDefault(t => t.TENDN == tks);
            tk.PASS = pass;
            dl.SubmitChanges();
            return Json(new { code = 500, msg="Cập nhật thành công"}, JsonRequestBehavior.AllowGet);
        }
    }




}