using System;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using ToDoApp.Areas.Zadania.Models;
using ToDoApp.Services;
using PagedList;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;

namespace ToDoApp.Areas.Zadania.Controllers
{
    public class ZadanieController : Controller
    {
        public ZadanieController()
        {
            _zadanieRepository = new ZadanieRepository(new ZadaniaContext());
        }
        private IZadanieRepository _zadanieRepository;
        public ZadanieController(IZadanieRepository zadanieRepository)
        {
            _zadanieRepository = zadanieRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Listing");

        }

        public ActionResult Listing(string Temat, string Czynnosc, string Opis, string Status, string Priorytet, string ProcentZakonczenia, string DataRozpoczecia, string DataZakonczenia, string sortOrder, int? page, int pageSize = 10)
        {


            var zadania = _zadanieRepository.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TematSortParam = String.IsNullOrEmpty(sortOrder) ? "temat_desc" : "";
            ViewBag.CzynnoscSortParam = sortOrder == "Czynnosc" ? "czynnosc_desc" : "Czynnosc";
            ViewBag.OpisSortParam = sortOrder == "Opis" ? "opis_desc" : "Opis";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.PriorytetSortParam = sortOrder == "Priorytet" ? "priorytet_desc" : "Priorytet";
            ViewBag.ProcentZakonczeniaSortParam = sortOrder == "ProcentZakonczenia" ? "procentZakonczenia_desc" : "ProcentZakonczenia";
            ViewBag.DataRozpoczeciaSortParam = sortOrder == "DataRozpoczecia" ? "dataRozpoczecia_desc" : "DataRozpoczecia";
            ViewBag.DataZakonczeniaSortParam = sortOrder == "DataZakonczenia" ? "dataZakonczenia_desc" : "DataZakonczenia";

            if (zadania.Any())
            {
                if (!string.IsNullOrEmpty(Temat))
                {
                    zadania = zadania.Where(s => s.Temat != null && s.Temat.ToLower().Contains(Temat.Trim().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    if (int.TryParse(Status, out var status))
                        zadania = zadania.Where(s => s.Status == status).ToList();
                }
                if (!string.IsNullOrEmpty(Czynnosc))
                {
                    zadania = zadania.Where(s => s.Czynnosc != null && s.Czynnosc.ToLower().Contains(Czynnosc.Trim().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(DataRozpoczecia))
                {
                    if (DateTime.TryParse(DataRozpoczecia, out var dataRozpoczecia))
                    {
                        zadania = zadania.Where(s => s.DataRozpoczecia != null && s.DataRozpoczecia >= dataRozpoczecia).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(DataZakonczenia))
                {
                    var dataZakonczenia = DateTime.MinValue;
                    if (DateTime.TryParse(DataZakonczenia, out dataZakonczenia))
                    {
                        zadania = zadania.Where(s => s.DataZakonczenia != null && s.DataZakonczenia <= dataZakonczenia).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(Priorytet))
                {
                    if (int.TryParse(Priorytet, out var priorytet))
                        zadania = zadania.Where(s => s.Priorytet == priorytet).ToList();
                }
                if (!string.IsNullOrEmpty(ProcentZakonczenia))
                {
                    if (int.TryParse(ProcentZakonczenia, out var procentZakonczenia))
                        zadania = zadania.Where(s => s.ProcentZakonczenia == procentZakonczenia).ToList();
                }
                if (!string.IsNullOrEmpty(Opis))
                {
                    zadania = zadania.Where(s => s.Opis != null && s.Opis.ToLower().Contains(Opis.Trim().ToLower())).ToList();
                }
            }




            switch (sortOrder)
            {
                case "Temat":
                    zadania = zadania.OrderBy(s => s.Temat);
                    break;
                case "temat_desc":
                    zadania = zadania.OrderByDescending(s => s.Temat);
                    break;
                case "Status":
                    zadania = zadania.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    zadania = zadania.OrderByDescending(s => s.Status);
                    break;
                case "Czynnosc":
                    zadania = zadania.OrderBy(s => s.Czynnosc);
                    break;
                case "czynnosc_desc":
                    zadania = zadania.OrderByDescending(s => s.Czynnosc);
                    break;
                case "DataRozpoczecia":
                    zadania = zadania.OrderBy(s => s.DataRozpoczecia);
                    break;
                case "dataRozpoczecia_desc":
                    zadania = zadania.OrderByDescending(s => s.DataRozpoczecia);
                    break;
                case "DataZakonczenia":
                    zadania = zadania.OrderBy(s => s.DataZakonczenia);
                    break;
                case "dataZakonczenia_desc":
                    zadania = zadania.OrderByDescending(s => s.DataZakonczenia);
                    break;
                case "Priorytet":
                    zadania = zadania.OrderBy(s => s.Priorytet);
                    break;
                case "ProcentZakonczenia":
                    zadania = zadania.OrderBy(s => s.ProcentZakonczenia);
                    break;
                case "procentZakonczenia_desc":
                    zadania = zadania.OrderByDescending(s => s.ProcentZakonczenia);
                    break;
                case "Opis":
                    zadania = zadania.OrderBy(s => s.Opis);
                    break;
                case "opis_desc":
                    zadania = zadania.OrderByDescending(s => s.Opis);
                    break;
                default:
                    zadania = zadania.OrderByDescending(s => s.Priorytet);
                    break;
            }
            ViewBag.psize = pageSize;
            ViewBag.PageSize = new List<SelectListItem>()
            {
             new SelectListItem() { Value="10", Text= "10" },
             new SelectListItem() { Value="20", Text= "20" },
             new SelectListItem() { Value="30", Text= "30" },
             new SelectListItem() { Value="40", Text= "40" },
             new SelectListItem() { Value="50", Text= "50" },
            };

            int pageNumber = page ?? 1;
            return View(zadania.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToDoModel zadanie)
        {

            if (!string.IsNullOrEmpty(zadanie.Temat))
            {
                var temat = _zadanieRepository.GetAll().Any(x => x.Temat.Trim().ToLower() == zadanie.Temat.Trim().ToLower());
                if (temat)
                {
                    ModelState.AddModelError("Temat", "Temat już instnieje");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _zadanieRepository.Add(zadanie);
                    return RedirectToAction("Listing");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return View(zadanie);
        }

        public ActionResult Edit(int id)
        {

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var zadanie = _zadanieRepository.GetById(id);
            if (zadanie == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", zadanie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ToDoModel zadanie)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    zadanie.Identifier = id;
                    _zadanieRepository.Edit(zadanie);
                    return RedirectToAction("Listing");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return View(zadanie);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var zadanie = _zadanieRepository.GetById(id);
            if (zadanie == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", zadanie);

        }



        public ActionResult Delete(int id)
        {

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var zadanie = _zadanieRepository.GetById(id);
            if (zadanie == null)
            {
                return HttpNotFound();
            }

            try
            {
                _zadanieRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return RedirectToAction("Listing");
        }
        public ActionResult Tiles(string Temat, string Czynnosc, string Opis, string Status, string Priorytet, string ProcentZakonczenia, string DataRozpoczecia, string DataZakonczenia, string sortOrder, int? page, int pageSize = 10)
        {
            var zadania = _zadanieRepository.GetAll();

            if (zadania.Any())
            {
                if (!string.IsNullOrEmpty(Temat))
                {
                    zadania = zadania.Where(s => s.Temat != null && s.Temat.ToLower().Contains(Temat.Trim().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    if (int.TryParse(Status, out var status))
                        zadania = zadania.Where(s => s.Status == status).ToList();
                }
                if (!string.IsNullOrEmpty(Czynnosc))
                {
                    zadania = zadania.Where(s => s.Czynnosc != null && s.Czynnosc.ToLower().Contains(Czynnosc.Trim().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(DataRozpoczecia))
                {
                    if (DateTime.TryParse(DataRozpoczecia, out var dataRozpoczecia))
                    {
                        zadania = zadania.Where(s => s.DataRozpoczecia != null && s.DataRozpoczecia >= dataRozpoczecia).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(DataZakonczenia))
                {
                    var dataZakonczenia = DateTime.MinValue;
                    if (DateTime.TryParse(DataZakonczenia, out dataZakonczenia))
                    {
                        zadania = zadania.Where(s => s.DataZakonczenia != null && s.DataZakonczenia <= dataZakonczenia).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(Priorytet))
                {
                    if (int.TryParse(Priorytet, out var priorytet))
                        zadania = zadania.Where(s => s.Priorytet == priorytet).ToList();
                }
                if (!string.IsNullOrEmpty(ProcentZakonczenia))
                {
                    if (int.TryParse(ProcentZakonczenia, out var procentZakonczenia))
                        zadania = zadania.Where(s => s.ProcentZakonczenia == procentZakonczenia).ToList();
                }
                if (!string.IsNullOrEmpty(Opis))
                {
                    zadania = zadania.Where(s => s.Opis != null && s.Opis.ToLower().Contains(Opis.Trim().ToLower())).ToList();
                }
            }
            ViewBag.psize = pageSize;
            ViewBag.PageSize = new List<SelectListItem>()
            {
             new SelectListItem() { Value="10", Text= "10" },
             new SelectListItem() { Value="20", Text= "20" },
             new SelectListItem() { Value="30", Text= "30" },
             new SelectListItem() { Value="40", Text= "40" },
             new SelectListItem() { Value="50", Text= "50" },
            };

            int pageNumber = page ?? 1;
            return View(zadania.OrderBy(s => s.Priorytet).ToPagedList(pageNumber, pageSize));
        }
        protected string RenderRazorViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
        public void ExportToExcel()
        {
            string Filename = "Excel_" + DateTime.Now.ToString("dd_MM_yyy_hh_mm") + ".xls";
            string FolderPath = HttpContext.Server.MapPath("/App_Data/");
            string FilePath = System.IO.Path.Combine(FolderPath, Filename);

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            string HtmlResult = RenderRazorViewToString("~/Areas/Zadania/Views/Zadanie/ExportToExcel.cshtml", _zadanieRepository.GetAll());       
            byte[] ExcelBytes = Encoding.ASCII.GetBytes(HtmlResult);           
            using (Stream file = System.IO.File.OpenWrite(FilePath))
            {
                file.Write(ExcelBytes, 0, ExcelBytes.Length);
            }

            
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(Filename));
            Response.WriteFile(FilePath);
            Response.End();
            Response.Flush();
        }

        public EmptyResult ExportToPdf()
        {
            string Filename = "PDF_" + DateTime.Now.ToString("dd_MM_yyy_hh_mm") + ".pdf";

            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            document.Open();

            var html = RenderRazorViewToString("~/Areas/Zadania/Views/Zadanie/ExportToPDF.cshtml", _zadanieRepository.GetAll());

            var worker = new HTMLWorker(document);

            var stringReader = new StringReader(html);
            worker.Parse(stringReader);

            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(Filename));
            Response.BinaryWrite(output.ToArray());

            return new EmptyResult();
        }



    }
}