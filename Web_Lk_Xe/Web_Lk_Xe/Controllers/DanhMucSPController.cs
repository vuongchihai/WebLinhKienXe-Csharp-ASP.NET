using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Lk_Xe.Models;

namespace Web_Lk_Xe.Controllers
{
    public class DanhMucSPController : Controller
    {
        Data_LKXeDataContext dl = new Data_LKXeDataContext();
       
        // GET: DanhMuSanPham
        public ActionResult DanhMucSanPham()
        {
            List<LOAISP> dsDM = dl.LOAISPs.ToList();
            return PartialView(dsDM);
        }
    }
}