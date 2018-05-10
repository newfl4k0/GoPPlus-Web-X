using GoPS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoPS.Classes
{
    public class Utilities
    {
        private GoPSEntities db = new GoPSEntities();
        //private Entities ent = new Entities();

        public Utilities() { }

        public Dictionary<int, string> ObtenerDias()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "01");
            dict.Add(2, "02");
            dict.Add(3, "03");
            dict.Add(4, "04");
            dict.Add(5, "05");
            dict.Add(6, "06");
            dict.Add(7, "07");
            dict.Add(8, "08");
            dict.Add(9, "09");
            dict.Add(10, "10");
            dict.Add(11, "11");
            dict.Add(12, "12");
            dict.Add(13, "13");
            dict.Add(14, "14");
            dict.Add(15, "15");
            dict.Add(16, "16");
            dict.Add(17, "17");
            dict.Add(18, "18");
            dict.Add(19, "19");
            dict.Add(20, "20");
            dict.Add(21, "21");
            dict.Add(22, "22");
            dict.Add(23, "23");
            dict.Add(24, "24");
            dict.Add(25, "25");
            dict.Add(26, "26");
            dict.Add(27, "27");
            dict.Add(28, "28");
            dict.Add(29, "29");
            dict.Add(30, "30");
            dict.Add(31, "31");
            return dict;
        }

        public List<CheckBoxListItem> ObtenerDiasList(string dias, bool festivos)
        {
            List<char> diasChecked = String.IsNullOrEmpty(dias) ? new List<char>() : dias.ToCharArray().ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<char, string> dayNames = ObtenerNombresDias(festivos);
            foreach (KeyValuePair<char, string> day in dayNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = day.Key.ToString(),
                    Display = day.Value,
                    IsChecked = diasChecked.Contains(day.Key),
                    IsLastChecked = diasChecked.Count > 0 ? (day.Key == diasChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        private Dictionary<char, string> ObtenerNombresDias(bool festivos)
        {
            Dictionary<char, string> dict = new Dictionary<char, string>();
            dict.Add('1', "Lunes");
            dict.Add('2', "Martes");
            dict.Add('3', "Miercoles");
            dict.Add('4', "Jueves");
            dict.Add('5', "Viernes");
            dict.Add('6', "Sábado");
            dict.Add('7', "Domingo");
            if(festivos)
                dict.Add('8', "Festivos");
            return dict;
        }

        public Dictionary<string, string> ObtenerHoras()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("00:00:00", "00:00 am");
            dict.Add("01:00:00", "01:00 am");
            dict.Add("02:00:00", "02:00 am");
            dict.Add("03:00:00", "03:00 am");
            dict.Add("04:00:00", "04:00 am");
            dict.Add("05:00:00", "05:00 am");
            dict.Add("06:00:00", "06:00 am");
            dict.Add("07:00:00", "07:00 am");
            dict.Add("08:00:00", "08:00 am");
            dict.Add("09:00:00", "09:00 am");
            dict.Add("10:00:00", "10:00 am");
            dict.Add("11:00:00", "11:00 am");
            dict.Add("12:00:00", "12:00 pm");
            dict.Add("13:00:00", "01:00 pm");
            dict.Add("14:00:00", "02:00 pm");
            dict.Add("15:00:00", "03:00 pm");
            dict.Add("16:00:00", "04:00 pm");
            dict.Add("17:00:00", "05:00 pm");
            dict.Add("18:00:00", "06:00 pm");
            dict.Add("19:00:00", "07:00 pm");
            dict.Add("20:00:00", "08:00 pm");
            dict.Add("21:00:00", "09:00 pm");
            dict.Add("22:00:00", "10:00 pm");
            dict.Add("23:00:00", "11:00 pm");
            return dict;
        }

        public List<CheckBoxListItem> ObtenerEstatusList(string estatus)
        {
            List<int> estatusChecked = String.IsNullOrEmpty(estatus) ? new List<int>() : estatus.Split(',').Select(Int32.Parse).ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<int, string> estatusNames = db.Estatus.ToDictionary(d => d.ID_Estatus, d => d.Nombre);
            foreach (KeyValuePair<int, string> est in estatusNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = est.Key.ToString(),
                    Display = est.Value,
                    IsChecked = estatusChecked.Contains(est.Key),
                    IsLastChecked = estatusChecked.Count > 0 ? (est.Key == estatusChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        public List<CheckBoxListItem> ObtenerTiposVehiculosList(string tiposvehiculos)
        {
            List<int> tiposvehiculosChecked = String.IsNullOrEmpty(tiposvehiculos) ? new List<int>() : tiposvehiculos.Split(',').Select(Int32.Parse).ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<int, string> tiposvehiculosNames = db.TiposVehiculos.ToDictionary(d => d.ID_TipoVehiculo, d => d.Nombre);
            foreach (KeyValuePair<int, string> tipo in tiposvehiculosNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = tipo.Key.ToString(),
                    Display = tipo.Value,
                    IsChecked = tiposvehiculosChecked.Contains(tipo.Key),
                    IsLastChecked = tiposvehiculosChecked.Count > 0 ? (tipo.Key == tiposvehiculosChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        public List<RadioButtonListItem> ObtenerRadioButtonEstatusList(string estatus)
        {
            int estatusChecked = Int32.Parse(estatus);
            List<RadioButtonListItem> radioButtonListItems = new List<RadioButtonListItem>();
            Dictionary<int, string> estatusNames = db.Estatus_Reserva.Where(r=>r.Orden!=0).OrderBy(r => r.Orden).ToDictionary(d => d.ID_Estatus_Reserva, d => d.Nombre);
            radioButtonListItems.Add(new RadioButtonListItem()
            {
                ID = "0",
                Display = "Todos",
                IsChecked = estatusChecked == 0
            });
            foreach (KeyValuePair<int, string> est in estatusNames)
            {
                radioButtonListItems.Add(new RadioButtonListItem()
                {
                    ID = est.Key.ToString(),
                    Display = est.Value,
                    IsChecked = estatusChecked == est.Key
                });
            }
            return radioButtonListItems;
        }

        public List<CheckBoxListItem> ObtenerCheckBoxRolesList(string roles)
        {
            List<string> rolesChecked = String.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<string, string> rolesNames = db.AspNetRoles.ToDictionary(d => d.Id, d => d.Name);
            foreach (KeyValuePair<string, string> rol in rolesNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = rol.Key.ToString(),
                    Display = rol.Value,
                    IsChecked = rolesChecked.Contains(rol.Key),
                    IsLastChecked = rolesChecked.Count > 0 ? (rol.Key == rolesChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        public List<RadioButtonListItem> ObtenerRadioButtonRolesList(string roles, string userID)
        {
            bool isSAdmin = String.IsNullOrEmpty(userID) ? false :
                db.AspNetUsers.Find(userID).AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToUpper() == "SADMINISTRADOR";
            List<string> rolesChecked = String.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();
            List<RadioButtonListItem> radioButtonListItems = new List<RadioButtonListItem>();
            IEnumerable<AspNetRoles> rolesList = db.AspNetRoles;
            if (!isSAdmin)
                rolesList = rolesList.Where(r => r.Name.ToUpper() != "SADMINISTRADOR");
            Dictionary<string, string> rolesNames = rolesList.ToDictionary(d => d.Id, d => d.Name);
            foreach (KeyValuePair<string, string> rol in rolesNames)
            {
                radioButtonListItems.Add(new RadioButtonListItem()
                {
                    ID = rol.Key.ToString(),
                    Display = rol.Value,
                    IsChecked = rolesChecked.Contains(rol.Key)
                });
            }
            return radioButtonListItems;
        }

        public List<CheckBoxListItem> ObtenerCheckBoxAfiliadosList(string afiliados, List<int> ID_Afiliados, string userID)
        {
            bool isSAdmin = String.IsNullOrEmpty(userID) ? false :
                db.AspNetUsers.Find(userID).AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToUpper() == "SADMINISTRADOR";
            List<int> afiliadosChecked = String.IsNullOrEmpty(afiliados) ? new List<int>() : afiliados.Split(',').Select(Int32.Parse).ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            IEnumerable<Afiliados> afiliadosList = db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado));
            Dictionary<int, string> afiliadosNames = isSAdmin ?
                                                    afiliadosList.ToDictionary(d => d.ID_Afiliado, d => d.EmpresaNombreRFC) :
                                                    afiliadosList.ToDictionary(d => d.ID_Afiliado, d => d.NombreRFC);
            foreach (KeyValuePair<int, string> afi in afiliadosNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = afi.Key.ToString(),
                    Display = afi.Value,
                    IsChecked = afiliadosChecked.Contains(afi.Key),
                    IsLastChecked = afiliadosChecked.Count > 0 ? (afi.Key == afiliadosChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        public List<RadioButtonListItem> ObtenerRadioButtonAfiliadosList(string afiliados, List<int> ID_Afiliados, string userID)
        {
            bool isSAdmin = String.IsNullOrEmpty(userID) ? false :
                db.AspNetUsers.Find(userID).AspNetUserRoles.FirstOrDefault().AspNetRoles.Name.ToUpper() == "SADMINISTRADOR";
            List<int> afiliadosChecked = String.IsNullOrEmpty(afiliados) ? new List<int>() : afiliados.Split(',').Select(Int32.Parse).ToList();
            List<RadioButtonListItem> radioButtonListItems = new List<RadioButtonListItem>();
            IEnumerable<Afiliados> afiliadosList = db.Afiliados.Where(r => ID_Afiliados.Contains(r.ID_Afiliado));
            Dictionary<int, string> afiliadosNames = isSAdmin ?
                                                    afiliadosList.ToDictionary(d => d.ID_Afiliado, d => d.EmpresaNombreRFC) :
                                                    afiliadosList.ToDictionary(d => d.ID_Afiliado, d => d.NombreRFC);
            foreach (KeyValuePair<int, string> afi in afiliadosNames)
            {
                radioButtonListItems.Add(new RadioButtonListItem()
                {
                    ID = afi.Key.ToString(),
                    Display = afi.Value,
                    IsChecked = afiliadosChecked.Contains(afi.Key)
                });
            }
            return radioButtonListItems;
        }

        public List<CheckBoxListItem> ObtenerPermisosList(string permisos)
        {
            List<int> permisosChecked = String.IsNullOrEmpty(permisos) ? new List<int>() : permisos.Split(',').Select(Int32.Parse).ToList();
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<int, string> permisosNames = db.Permissions.Where(d => d.Able).ToDictionary(d => d.Id, d => d.Description);
            foreach (KeyValuePair<int, string> perm in permisosNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = perm.Key.ToString(),
                    Display = perm.Value,
                    IsChecked = permisosChecked.Contains(perm.Key),
                    IsLastChecked = permisosChecked.Count > 0 ? (perm.Key == permisosChecked.Last()) : false
                });
            }
            return checkBoxListItems;
        }

        public List<CheckBoxListItem> ObtenerTiposVehiculosList()
        {
            List<CheckBoxListItem> checkBoxListItems = new List<CheckBoxListItem>();
            Dictionary<int, string> tiposNames = db.TiposVehiculos.ToDictionary(d => d.ID_TipoVehiculo, d => d.Nombre);
            foreach (KeyValuePair<int, string> day in tiposNames)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = day.Key.ToString(),
                    Display = day.Value,
                    IsChecked = true,
                    IsLastChecked = false
                });
            }
            return checkBoxListItems;
        }

        public bool RolNeedsPicture(string role)
        {
            //List<string> rolesChecked = String.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();
            List<string> idRolesPicture = db.AspNetRoles.Where(a => a.Name.ToUpper() == "DRIVER" || a.Name.ToUpper() == "GUEST").Select(a => a.Id).ToList();
            bool isRolPic = idRolesPicture.Contains(role);
            return isRolPic;
        }

        public bool RolNeedsPosition(string role)
        {
            List<string> idRolesPosition = db.AspNetRoles.Where(a => a.Name.ToUpper() == "SADMINISTRADOR" || a.Name.ToUpper() == "ADMINISTRADOR" || a.Name.ToUpper() == "CENTRAL").Select(a => a.Id).ToList();
            bool isRolPos = idRolesPosition.Contains(role);
            return isRolPos;
        }

        public DateTime ConvertToMexicanDate(DateTime date)
        {
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime tstTime = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, tst);
            return tstTime;
        }

        public bool MoveFile(string filename)
        {
            string tempFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/images/Uploads_Temp/"), filename);
            string file = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/images/Uploads/"), filename);

            if (File.Exists(tempFile) && !File.Exists(file))
            {
                File.Move(tempFile, file);
                File.Delete(tempFile);
            }
            return File.Exists(file);
        }

        public bool RemoveFile(string filename, bool temp)
        {
            string path = temp ? "~/images/Uploads_Temp/" : "~/images/Uploads/";
            string tempFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(path), filename);

            if (File.Exists(tempFile))
                File.Delete(tempFile);

            return !File.Exists(tempFile);
        }

        public string SaveTempFile(HttpPostedFileBase FilePath)
        {
            string fileName = "";
            string folder = "~/images/Uploads_Temp";

            if (FilePath != null)
                if (FilePath.ContentLength > 0)
                {
                    fileName = Path.GetFileNameWithoutExtension(FilePath.FileName) + "-" + 
                                (ConvertToMexicanDate(DateTime.Now).ToString("ddMMyyyyHHmmssfff")) +
                                Path.GetExtension(FilePath.FileName);
                    var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), fileName);
                    FilePath.SaveAs(path);
                }
            return fileName;
        }

        public string SaveFile(HttpPostedFileBase FilePath)
        {
            string fileName = "";
            string folder = "~/images/Uploads";

            if (FilePath != null)
                if (FilePath.ContentLength > 0)
                {
                    fileName = Path.GetFileNameWithoutExtension(FilePath.FileName) + "-" +
                                (ConvertToMexicanDate(DateTime.Now).ToString("ddMMyyyyHHmmssfff")) +
                                Path.GetExtension(FilePath.FileName);
                    var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), fileName);
                    FilePath.SaveAs(path);
                }

            return fileName;
        }

        public string SaveOrMoveFile(string old_filename, HttpPostedFileBase file, bool edit)
        {
            string new_filename;
            if (file != null)
            {
                RemoveFile(old_filename, !edit);
                new_filename = SaveFile(file);
            }
            else
            {
                MoveFile(old_filename);
                new_filename = old_filename;
            }

            return new_filename;
        }

        public string SaveNewTempFile(string old_filename, HttpPostedFileBase file, bool edit)
        {
            string new_filename;
            if (file != null)
            {
                new_filename = SaveTempFile(file);
                if (!String.IsNullOrEmpty(old_filename))
                    RemoveFile(old_filename, !edit);
            }
            else
                new_filename = old_filename;

            return new_filename;
        }
    }
}