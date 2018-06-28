using GoPS.CustomFilters;
using GoPS.Models;
using GoPS.Models.Metadata;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoPS.Classes;
using GoPS.Filters;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ReportesController : Controller
    {
        private GoPSEntities db = new GoPSEntities();
        public new byte[] Content { get; set; }
        public string MimeType { get; set; }
        private Type ReportType { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Type
        {
            WORD,
            EXCEL,
            PDF
        }
        
            // GET: Reportes
            [HasPermission("Reportes")]
        public ActionResult Index()
        {
            return View();
        }
        [HasPermission("Reportes")]
        public ActionResult EstadodeCuenta()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View();
        }

        [HttpPost]
        [HasPermission("Reportes")]
        [ValidateAntiForgeryToken]
        public ViewResult EstadodeCuenta(ParametrosEdoCta model)
        { 
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);         
            return View(model);
            
        }

        [HasPermission("Reportes")]
        public ActionResult ServXColonia()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaSelectList();
            return View();
        }

        [HttpPost]
        [HasPermission("Reportes")]
        [ValidateAntiForgeryToken]
        public ViewResult ServXColonia(ParametrosServColonia model)
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            ObtenerGeografiaSelectList(model.ID_Estado, model.ID_Ciudad, model.ID_Colonia);            
            return View(model);
        }

        [HasPermission("Reportes")]
        public ActionResult ClientesHabituales()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            //ObtenerGeografiaSelectList();
            return View();
        }

        [HttpPost]
        [HasPermission("Reportes")]
        [ValidateAntiForgeryToken]
        public ViewResult ClientesHabituales(ParametrosCH model)
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
           // ObtenerGeografiaSelectList(model.ID_Estado, model.ID_Ciudad, model.ID_Colonia);
            return View(model);

        }

        [HasPermission("Reportes")]
        public ActionResult ClientesAbonados()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);            
            return View();
        }

        [HttpPost]
        [HasPermission("Reportes")]
        [ValidateAntiForgeryToken]
        public ViewResult ClientesAbonados(ParametrosCA model)
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View(model);

        }

        [HasPermission("Reportes")]
        public ActionResult AbonadosADetalle()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View();
        }

        [HttpPost]
        [HasPermission("Reportes")]
        [ValidateAntiForgeryToken]
        public ViewResult AbonadosADetalle(ParametrosCA model)
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.MostrarAfiliados = ID_Afiliados.Count > 1;
            int ID_Afiliado = ID_Afiliados.Count == 1 ? ID_Afiliados.FirstOrDefault() : 0;
            ViewBag.ID_Afiliado = new SelectList(db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Afiliado", "Nombre", ID_Afiliado);
            return View(model);

        }

        public string FileExtension
        {
            get
            {
                string extension;
                switch (ReportType)
                {
                    case Type.EXCEL:
                        extension = "xls";
                        break;

                    case Type.WORD:
                        extension = "doc";
                        break;

                    default:
                        extension = "pdf";
                        break;
                }

                return extension;
            }
        }

        public ActionResult GeneraEstadoCuenta(ReportesController.Type reportType, string rfc=null, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null)
        {
            string extension="pdf";
            ServerReport lr = new ServerReport();
            ReportParameterCollection rpc = new ReportParameterCollection();
            ReportParameter Inicio = new ReportParameter("Inicio", fecha_inicio.Value.Year.ToString() + "-" +
                fecha_inicio.Value.Month.ToString() + "-" + fecha_inicio.Value.Day);
            ReportParameter Fin = new ReportParameter("Fin", fecha_fin.Value.Year.ToString() + "-" +
                fecha_fin.Value.Month.ToString() + "-" + fecha_fin.Value.Day);
            ReportParameter RFC = new ReportParameter("RFC", rfc);
            ReportParameter idafiliado = new ReportParameter("idafiliado", id_afiliado.ToString());
            switch (reportType)
            {
                case Type.EXCEL:
                    extension = "xls";
                    break;

                case Type.WORD:
                    extension = "doc";
                    break;

                default:
                    extension = "pdf";
                    break;
            }

            Uri gopplus = new Uri(@"http://52.176.55.203/ReportServer");
            rpc.Add(Inicio);
            rpc.Add(Fin);
            rpc.Add(RFC);
            rpc.Add(idafiliado);           
            lr.ReportServerUrl = gopplus;
            
            IReportServerCredentials irsc = new CustomReportCredentials("gopplus", "Gopplus2017++", "puertaapuerta");
            lr.ReportServerCredentials = irsc;
            lr.ReportPath = @"/Reportes/EdoCta";
            lr.SetParameters(rpc);            
             string deviceInfo = $@"<DeviceInfo>
                                      <OutputFormat>{ReportType}</OutputFormat>
                                      <PageWidth>27.94cm</PageWidth>
                                      <PageHeight>21.59cm</PageHeight>
                                      <MarginTop>1cm</MarginTop>
                                      <MarginLeft>1cm</MarginLeft>
                                      <MarginRight>1cm</MarginRight>
                                      <MarginBottom>1cm</MarginBottom>
                                   </DeviceInfo>";

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            Content = lr.Render(
                                reportType.ToString(),
                                deviceInfo,
                                out mimeType,
                                out encoding,
                                out fileNameExtension,
                                out streams,
                                out warnings);

            MimeType = mimeType;
            if (download)
            {
                return File(this.Content, this.MimeType, $"EdoCtaGoPPlus{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{fileNameExtension}");
            }
            return File(this.Content, this.MimeType);
        }

        public ActionResult GeneraSrvxColonia(ReportesController.Type reportType, string idcolonia = null, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null)
        {
            string extension = "pdf";
            ServerReport lr = new ServerReport();
            ReportParameterCollection rpc = new ReportParameterCollection();
            ReportParameter Inicio = new ReportParameter("Fecha_Inicio", fecha_inicio.Value.Year.ToString() + "-" +
                fecha_inicio.Value.Month.ToString() + "-" + fecha_inicio.Value.Day);
            ReportParameter Fin = new ReportParameter("Fecha_Fin", fecha_fin.Value.Year.ToString() + "-" +
                fecha_fin.Value.Month.ToString() + "-" + fecha_fin.Value.Day);
            ReportParameter ic = new ReportParameter("idcolonia", idcolonia.ToString());
            ReportParameter idafiliado = new ReportParameter("idafiliado", id_afiliado.ToString());
            switch (reportType)
            {
                case Type.EXCEL:
                    extension = "xls";
                    break;

                case Type.WORD:
                    extension = "doc";
                    break;

                default:
                    extension = "pdf";
                    break;
            }

            Uri gopplus = new Uri(@"http://52.176.55.203/ReportServer");
            rpc.Add(Inicio);
            rpc.Add(Fin);
            rpc.Add(ic);
            rpc.Add(idafiliado);
            lr.ReportServerUrl = gopplus;

            IReportServerCredentials irsc = new CustomReportCredentials("gopplus", "Gopplus2017++", "puertaapuerta");
            lr.ReportServerCredentials = irsc;
            lr.ReportPath = @"/Reportes/SrvxColonia2";
            lr.SetParameters(rpc);
            string deviceInfo = $@"<DeviceInfo>
                                      <OutputFormat>{ReportType}</OutputFormat>
                                      <PageWidth>27.94cm</PageWidth>
                                      <PageHeight>21.59cm</PageHeight>
                                      <MarginTop>1cm</MarginTop>
                                      <MarginLeft>1cm</MarginLeft>
                                      <MarginRight>1cm</MarginRight>
                                      <MarginBottom>1cm</MarginBottom>
                                   </DeviceInfo>";

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            Content = lr.Render(
                                reportType.ToString(),
                                deviceInfo,
                                out mimeType,
                                out encoding,
                                out fileNameExtension,
                                out streams,
                                out warnings);

            MimeType = mimeType;
            if (download)
            {
                return File(this.Content, this.MimeType, $"SrvxColoniaGoPPlus{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{fileNameExtension}");
            }
            return File(this.Content, this.MimeType);
        }

        public ActionResult GeneraClientesHabituales(ReportesController.Type reportType, string idcolonia = null, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null)
        {
            string extension = "pdf";
            ServerReport lr = new ServerReport();
            ReportParameterCollection rpc = new ReportParameterCollection();
            ReportParameter Inicio = new ReportParameter("Fecha_Inicio", fecha_inicio.Value.Year.ToString() + "-" +
                fecha_inicio.Value.Month.ToString() + "-" + fecha_inicio.Value.Day);
            ReportParameter Fin = new ReportParameter("Fecha_Fin", fecha_fin.Value.Year.ToString() + "-" +
                fecha_fin.Value.Month.ToString() + "-" + fecha_fin.Value.Day);            
            ReportParameter idafiliado = new ReportParameter("idafiliado", id_afiliado.ToString());
            switch (reportType)
            {
                case Type.EXCEL:
                    extension = "xls";
                    break;

                case Type.WORD:
                    extension = "doc";
                    break;

                default:
                    extension = "pdf";
                    break;
            }

            Uri gopplus = new Uri(@"http://52.176.55.203/ReportServer");
            rpc.Add(Inicio);
            rpc.Add(Fin);            
            rpc.Add(idafiliado);
            lr.ReportServerUrl = gopplus;

            IReportServerCredentials irsc = new CustomReportCredentials("gopplus", "Gopplus2017++", "puertaapuerta");
            lr.ReportServerCredentials = irsc;
            lr.ReportPath = @"/Reportes/ClientesHabituales";
            lr.SetParameters(rpc);
            string deviceInfo = $@"<DeviceInfo>
                                      <OutputFormat>{ReportType}</OutputFormat>
                                      <PageWidth>27.94cm</PageWidth>
                                      <PageHeight>21.59cm</PageHeight>
                                      <MarginTop>1cm</MarginTop>
                                      <MarginLeft>1cm</MarginLeft>
                                      <MarginRight>1cm</MarginRight>
                                      <MarginBottom>1cm</MarginBottom>
                                   </DeviceInfo>";

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            Content = lr.Render(
                                reportType.ToString(),
                                deviceInfo,
                                out mimeType,
                                out encoding,
                                out fileNameExtension,
                                out streams,
                                out warnings);

            MimeType = mimeType;
            if (download)
            {
                return File(this.Content, this.MimeType, $"LClientesHabituales{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{fileNameExtension}");
            }
            return File(this.Content, this.MimeType);
        }
        
        public ActionResult GeneraClientesAbonados(ReportesController.Type reportType, string idcolonia = null, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null)
        {
            string extension = "pdf";
            ServerReport lr = new ServerReport();
            ReportParameterCollection rpc = new ReportParameterCollection();
            ReportParameter Inicio = new ReportParameter("Fecha_Inicio", fecha_inicio.Value.Year.ToString() + "-" +
                fecha_inicio.Value.Month.ToString() + "-" + fecha_inicio.Value.Day);
            ReportParameter Fin = new ReportParameter("Fecha_Fin", fecha_fin.Value.Year.ToString() + "-" +
                fecha_fin.Value.Month.ToString() + "-" + fecha_fin.Value.Day);            
            ReportParameter idafiliado = new ReportParameter("idafiliado", id_afiliado.ToString());
            switch (reportType)
            {
                case Type.EXCEL:
                    extension = "xls";
                    break;

                case Type.WORD:
                    extension = "doc";
                    break;

                default:
                    extension = "pdf";
                    break;
            }

            Uri gopplus = new Uri(@"http://52.176.55.203/ReportServer");
            rpc.Add(Inicio);
            rpc.Add(Fin);            
            rpc.Add(idafiliado);
            lr.ReportServerUrl = gopplus;

            IReportServerCredentials irsc = new CustomReportCredentials("gopplus", "Gopplus2017++", "puertaapuerta");
            lr.ReportServerCredentials = irsc;
            lr.ReportPath = @"/Reportes/ClientesAbonados";
            lr.SetParameters(rpc);
            string deviceInfo = $@"<DeviceInfo>
                                      <OutputFormat>{ReportType}</OutputFormat>
                                      <PageWidth>27.94cm</PageWidth>
                                      <PageHeight>21.59cm</PageHeight>
                                      <MarginTop>1cm</MarginTop>
                                      <MarginLeft>1cm</MarginLeft>
                                      <MarginRight>1cm</MarginRight>
                                      <MarginBottom>1cm</MarginBottom>
                                   </DeviceInfo>";

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            Content = lr.Render(
                                reportType.ToString(),
                                deviceInfo,
                                out mimeType,
                                out encoding,
                                out fileNameExtension,
                                out streams,
                                out warnings);

            MimeType = mimeType;
            if (download)
            {
                return File(this.Content, this.MimeType, $"LClientesAbonados{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{fileNameExtension}");
            }
            return File(this.Content, this.MimeType);
        }
        public ActionResult GeneraDetalleAbonados(ReportesController.Type reportType, string idcolonia = null, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null)
        {
            string extension = "pdf";
            ServerReport lr = new ServerReport();
            ReportParameterCollection rpc = new ReportParameterCollection();
            ReportParameter Inicio = new ReportParameter("Fecha_Inicio", fecha_inicio.Value.Year.ToString() + "-" +
                fecha_inicio.Value.Month.ToString() + "-" + fecha_inicio.Value.Day);
            ReportParameter Fin = new ReportParameter("Fecha_Fin", fecha_fin.Value.Year.ToString() + "-" +
                fecha_fin.Value.Month.ToString() + "-" + fecha_fin.Value.Day);
            ReportParameter idafiliado = new ReportParameter("idafiliado", id_afiliado.ToString());            
            switch (reportType)
            {
                case Type.EXCEL:
                    extension = "xls";
                    break;

                case Type.WORD:
                    extension = "doc";
                    break;

                default:
                    extension = "pdf";
                    break;
            }

            Uri gopplus = new Uri(@"http://52.176.55.203/ReportServer");
            rpc.Add(Inicio);
            rpc.Add(Fin);
            rpc.Add(idafiliado);
            lr.ReportServerUrl = gopplus;

            IReportServerCredentials irsc = new CustomReportCredentials("gopplus", "Gopplus2017++", "puertaapuerta");
            lr.ReportServerCredentials = irsc;
            lr.ReportPath = @"/Reportes/DetalleAbonados";
            lr.SetParameters(rpc);
            string deviceInfo = $@"<DeviceInfo>
                                      <OutputFormat>{ReportType}</OutputFormat>
                                      <PageWidth>27.94cm</PageWidth>
                                      <PageHeight>21.59cm</PageHeight>
                                      <MarginTop>1cm</MarginTop>
                                      <MarginLeft>1cm</MarginLeft>
                                      <MarginRight>1cm</MarginRight>
                                      <MarginBottom>1cm</MarginBottom>
                                   </DeviceInfo>";

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            Content = lr.Render(
                                reportType.ToString(),
                                deviceInfo,
                                out mimeType,
                                out encoding,
                                out fileNameExtension,
                                out streams,
                                out warnings);

            MimeType = mimeType;
            if (download)
            {
                return File(this.Content, this.MimeType, $"LClientesAbonados{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{fileNameExtension}");
            }
            return File(this.Content, this.MimeType);
        }

        private void ObtenerGeografiaSelectList()
        {
            ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");
            ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
        }

        private void ObtenerGeografiaSelectList(int? id_estado, int? id_ciudad, int? id_colonia)
        {
            Estados estado = id_estado.HasValue ? db.Estados.Find(id_estado.Value) : null;
            Ciudades ciudad = id_ciudad.HasValue ? db.Ciudades.Find(id_ciudad.Value) : null;
            Colonias colonia = id_colonia.HasValue ? db.Colonias.Find(id_colonia.Value) : null;

            if (estado == null)
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre");            
            else
                ViewBag.ID_Estado = new SelectList(db.Estados.OrderBy(o => o.Nombre), "ID_Estado", "Nombre", estado.ID_Estado);

            if(ciudad == null)
                if(estado == null)
                    ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
                else
                    ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion).Take(0), "ID_Ciudad", "Poblacion");
            else
                if(estado == null)
                    ViewBag.ID_Ciudad = new SelectList(db.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);
                else
                    ViewBag.ID_Ciudad = new SelectList(estado.Ciudades.OrderBy(o => o.Poblacion), "ID_Ciudad", "Poblacion", ciudad.ID_Ciudad);

            if (colonia == null)
                if(ciudad == null)
                    ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
                else
                    ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre).Take(0), "ID_Colonia", "Nombre");
            else
                if(ciudad == null)
                    ViewBag.ID_Colonia = new SelectList(db.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
                else
                ViewBag.ID_Colonia = new SelectList(ciudad.Colonias.OrderBy(o => o.Nombre), "ID_Colonia", "Nombre", colonia.ID_Colonia);
        }



        /*public ActionResult RGeneral(Bll.Report.Type reportType, bool download = false, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? id_afiliado = null, int? id_estado = null, int? id_ciudad = null, int? id_colonia = null, int? id_tipovehiculo = null)
        {
            //List<ReportesDespachos_Result> result = db.ReportesDespachos(fecha_inicio, fecha_fin, id_afiliado, id_estado, id_ciudad, id_colonia).ToList();

            //var report = new Bll.Report(Server.MapPath("~/Reportes/Despachos.rdlc"), "MyataSet", result, reportType);
            var report=0;
           /* if (download)
            {
                return File(report.Content, report.MimeType, $"DespachosGoPPlus{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{report.FileExtension}");
            }
            return File(report.Content, report.MimeType);
            List<RP_SP_ESTADODECUENTA_Result> result = db.RP_SP_ESTADODECUENTA(fecha_inicio, fecha_fin,rfc, id_afiliado).ToList();

            var report = new Bll.Report(Server.MapPath("~/Reportes/EdoCta.rdlc"), "MyataSet", null, reportType);

            if (download)
            {
                return File(report.Content, report.MimeType, $"EdoCtaGoPPlus{DateTime.Now.ToString("yyyy-MM-dd-HH'h'mm")}.{report.FileExtension}");
            }
            return File(report.Content, report.MimeType);
        }*/




    }
}
public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
{

    // local variable for network credential.
    private string _UserName;
    private string _PassWord;
    private string _DomainName;
    public CustomReportCredentials(string UserName, string PassWord, string DomainName)
    {
        _UserName = UserName;
        _PassWord = PassWord;
        _DomainName = DomainName;
    }
    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get
        {
            return null;  // not use ImpersonationUser
        }
    }
    public System.Net.ICredentials NetworkCredentials
    {
        get
        {

            // use NetworkCredentials
            return new System.Net.NetworkCredential(_UserName, _PassWord, _DomainName);
        }
    }
    public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string user, out string password, out string authority)
    {

        // not use FormsCredentials unless you have implements a custom autentication.
        authCookie = null;
        user = password = authority = null;
        return false;
    }

}
