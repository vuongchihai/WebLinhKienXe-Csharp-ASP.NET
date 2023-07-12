using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;
namespace Web_Lk_Xe.Controllers
{
    public class  UserController : Controller
    {
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
        // GET: User
        public ActionResult Account()
        {
            return View();
        }
        [HttpPost]
        public JsonResult createAccount(string tenTk, string pass, string tkh)
        {
          
            TAIKHOAN checkTk = dl.TAIKHOANs.FirstOrDefault(t => t.TENDN == tenTk);
            if (checkTk != null)
            {
                return Json(new { code = 200, msg = "Đăng ký thất bại, Tài khoản đã tồn tại" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                KHACHHANG kh = new KHACHHANG();
                TAIKHOAN tk = new TAIKHOAN();
                tk.TENDN = tenTk;
                tk.PASS = pass;
                dl.TAIKHOANs.InsertOnSubmit(tk);
                kh.MAKH = "";
                kh.TENKH = tkh;
                kh.TENDN = tenTk;
                dl.KHACHHANGs.InsertOnSubmit(kh);
                dl.SubmitChanges();
                return Json(new {code =500, msg = "Đăng ký thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult loginAccount(string tenTk, string pass)
        {
            TAIKHOAN checkdn = dl.TAIKHOANs.FirstOrDefault(t => t.TENDN == tenTk && t.PASS == pass);
            
            
            if (checkdn != null)
            {
                Session["ttDangNhap"] = checkdn;
                KHACHHANG chechkh = dl.KHACHHANGs.FirstOrDefault(t => t.TENDN == checkdn.TENDN);
                Session["ttkh"] = chechkh;


                // Lay so luong san pham co trong gio khach hang dang nhap
                List<GIOHANG> ghKDN = dl.GIOHANGs.Where(t => t.MAKH == chechkh.MAKH).ToList();
                
                Session["gh_DangNhap"] = ghKDN.Count();

             

                return Json(new { code = 500, msg = "Đăng nhập thành công" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { code = 200, msg = "Đăng nhập thất bại"}, JsonRequestBehavior.AllowGet);

            }
        }

        // Hien thi me menu tai khoan
        public ActionResult menuAccount()
        {
            return PartialView();
        }


        // Cap nhat thong tin tai khoan
        public ActionResult capNhatTtAccount()
        {
            return View();
        }

        //Load du lieu
       [HttpGet]
        public JsonResult loadTtAccount()
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["ttkh"];
                KHACHHANG khs;
                khs = dl.KHACHHANGs.FirstOrDefault(t => t.MAKH == kh.MAKH);

                return Json(new
                {
                    code = 500,
                    tenkh = khs.TENKH,
                    Email = khs.TENDN,
                    sdt = khs.SDT,
                    gt = khs.GIOTINH,

                    msg = "Load thông tin thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { code = 200, msg = "Load thông tin thất bại" }, JsonRequestBehavior.AllowGet);

            }




        }


        [HttpPost]
        public JsonResult XlcapNhatAccount(string tenkh, string sdt, string gt)
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["ttkh"];
                KHACHHANG khs = new KHACHHANG();
                khs = dl.KHACHHANGs.FirstOrDefault(t => t.MAKH == kh.MAKH);
                if(khs != null)
                {
                    khs.TENKH = tenkh;
                    khs.SDT = sdt;
                    khs.GIOTINH = gt;
                    dl.SubmitChanges();
                }
                Session["ttkh"] = khs;
                return Json(new { code = 500, msg = "Cập nhật tài khoản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { code = 200, msg = "Cập nhật tài khoản thất bại" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult xlDoiPass(string PassOld, string PassNew)
        {
            TAIKHOAN tkcheck = (TAIKHOAN)Session["ttDangNhap"];
            if(PassOld == tkcheck.PASS)
            {
                TAIKHOAN doiPass = new TAIKHOAN();
                doiPass = dl.TAIKHOANs.FirstOrDefault(t => t.TENDN == tkcheck.TENDN);
                doiPass.PASS = PassNew;
                dl.SubmitChanges();
                return Json(new { code = 500, msg = "Cập nhật mật khẩu mới thành công" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { code = 200, msg = "Mật khẩu cũ không đúng" }, JsonRequestBehavior.AllowGet);

            }
        }

        
        public ActionResult XemDonHang()
        {
            return View();
        }



        public ActionResult DangXuat()
        {
           Session.Clear();
           return RedirectToAction("Index", "Home");
        }
    }
}