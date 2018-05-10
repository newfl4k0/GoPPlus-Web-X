﻿using GoPS.Models;
using GoPS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GoPS.Classes
{
    public class DBValidaciones
    {
        GoPSEntities db = new GoPSEntities();
        Regex regexRFC = new Regex("^([A-Z@]{3,4})+([0-9]{6})+([A-Z@0-9]{3})$");
        Regex regexEmail = new Regex("^[a-zA-ZñÑ0-9._-]+@[a-zA-ZñÑ0-9._-]+\\.[a-zA-Z]{2,4}$");
        Regex regexFraseNumerosComaPunto = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9,. ]*$");
        Regex regexFraseComaPunto = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ,. ]*$");
        Regex regexFraseNumeros = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9 ]*$");
        Regex regexFrase = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ ]*$");
        Regex regexPalabraNumerosUserName = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9._-]*$");
        Regex regexPalabraNumerosGuionDiagonal = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9/-]*$");
        Regex regexPalabraNumeros = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9]*$");
        Regex regexPalabra = new Regex("^[a-zA-ZáéíóúñÁÉÍÓÚÑ]*$");
        Regex regexNumeros = new Regex("^[0-9]*$");
        Regex regexLatitud = new Regex(@"\-?(90|[0-8]?[0-9]\.[0-9]{0,7})");
        Regex regexLongitud = new Regex(@"\-?(180|(1[0-7][0-9]|[0-9]{0,2})\.[0-9]{0,7})");
        Regex regexnoNumNeg = new Regex(@"^[+]?\d+([.]\d+)?$");
        Regex regexPhone = new Regex(@"^(\+?\d{1,3}[ ]?)?([0-9]{3})?[ ]?([0-9]{3})[ ]?([0-9]{4})$");
       

        public DBValidaciones()
        {
            //if(regexItem.IsMatch(YOUR_STRING))
            
        }

        #region AspNetUsers

        public void ValidarUser(ModelStateDictionary ModelState, RegisterViewModel model, bool isRolPic)
        {
            bool error;
            if (!String.IsNullOrEmpty(model.UserName))
            {
                error = db.AspNetUsers.Where(c => c.Id != model.Id && c.UserName == model.UserName).Count() > 0;
                if (error)
                    ModelState.AddModelError("UserName", "El nombre de usuario ya se encuentra registrado.");
                if (!regexPalabraNumerosUserName.IsMatch(model.UserName))
                    ModelState.AddModelError("UserName", "El nombre de usuario no permite espacios ni caracteres especiales.");
                model.UserName = System.Text.RegularExpressions.Regex.Replace(model.UserName, @"\s+", " ");
                model.UserName = model.UserName.TrimStart();
            }
            else
                ModelState.AddModelError("UserName", "El campo Nombre de Usuario es requerido.");
            if (!String.IsNullOrEmpty(model.Email))
            {
                error = db.AspNetUsers.Where(c => c.Id != model.Id && c.Email == model.Email).Count() > 0;
                if (error)
                    ModelState.AddModelError("Email", "El email del usuario ya se encuentra registrado.");
                if (!regexEmail.IsMatch(model.Email))
                    ModelState.AddModelError("Email", "El formato del email es inválido.");
            }
            else
                ModelState.AddModelError("Email", "El campo Email es requerido.");
            if (!String.IsNullOrEmpty(model.Password) && !String.IsNullOrEmpty(model.ConfirmPassword))
            {
                if (model.Password != model.ConfirmPassword)
                    ModelState.AddModelError("Password", "La contraseña y la confirmación no coinciden.");
            }
            if (model.DateOfBirth != null)
            {
                error = Convert.ToDateTime(model.DateOfBirth) >= DateTime.Now;
                if (error)
                    ModelState.AddModelError("DateOfBirth", "La fecha de nacimiento del usuario no puede ser mayor a la fecha actual.");
                error = DateTime.Now.AddYears(-18) < Convert.ToDateTime(model.DateOfBirth);
                if (error)
                    ModelState.AddModelError("DateOfBirth", "Debe ser mayor de edad para realizar el registro.");
                error = DateTime.Now > Convert.ToDateTime(model.DateOfBirth).AddYears(100);
                if (error)
                    ModelState.AddModelError("DateOfBirth", "La edad límite para realizar el registro es de 100 años.");
            }
            if (isRolPic)
            {
                if (!String.IsNullOrEmpty(model.PicturePath))
                {
                    if (!ImagenEsValida(model.PicturePath))
                    {
                        ModelState.AddModelError("PicturePath", "La extensión de la Imagen debe ser .jpg, .jpeg o .png.");
                    }
                    if (!PesoImagenEsValido(model.Peso_PicturePath))
                    {
                        ModelState.AddModelError("PicturePath", "El peso de la Imagen no es válido.");
                    }
                }
                else
                {
                    ModelState.AddModelError("PicturePath", "El campo Imagen es requerido.");
                }
            }
        }

        #endregion

        #region Afiliados

        public void ValidarAfiliado(ModelStateDictionary ModelState, Afiliados afiliados)
        {
            if (!String.IsNullOrEmpty(afiliados.Nombre))
            {
                if (!regexFraseComaPunto.IsMatch(afiliados.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del afiliado no permite números ni caracteres especiales.");
                afiliados.Nombre = System.Text.RegularExpressions.Regex.Replace(afiliados.Nombre, @"\s+", " ");
                afiliados.Nombre = afiliados.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(afiliados.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(afiliados.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior del afiliado no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(afiliados.RFC))
            {
                if (!regexRFC.IsMatch(afiliados.RFC))
                    ModelState.AddModelError("RFC", "El RFC del afiliado debe ser un RFC válido para el SAT, debe estar conformado por números y letras mayúsculas, mayúsculas, y no permite espacios ni caracteres especiales a excepción del '@'.");
                afiliados.RFC = afiliados.RFC.TrimStart();
            }
            if (afiliados.ID_TipoPago > 0)
            {
                if (db.TiposPagos.Find(afiliados.ID_TipoPago).Nombre.ToUpper() == "LIQUIDACIÓN")
                {
                    if (!afiliados.Cuota_Conductor.HasValue)
                        ModelState.AddModelError("Cuota_Conductor", "El campo Cuota del Conductor es requerido.");
                }
                if (db.TiposPagos.Find(afiliados.ID_TipoPago).Nombre.ToUpper() == "COMISIÓN POR VIAJE")
                {
                    if (!afiliados.Porcentaje_Conductor.HasValue)
                        ModelState.AddModelError("Porcentaje_Conductor", "El campo Porcentaje de Comisión del Conductor es requerido.");
                }
            }
            if(afiliados.ID_Calle > 0)
            {
                int ID_Ciudad = db.Calles.Find(afiliados.ID_Calle).Colonias.ID_Ciudad;
                bool error = db.Afiliados.Where(a => a.ID_Afiliado != afiliados.ID_Afiliado && a.Calles.Colonias.ID_Ciudad == ID_Ciudad && a.ID_Empresa == afiliados.ID_Empresa).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "Esta empresa ya registró una sucursal en la ciudad indicada.");
            }
            if(afiliados.Codigo_Postal > 0)
            {
                if(afiliados.Codigo_Postal.ToString().Length != 5)
                    ModelState.AddModelError("Codigo_Postal", "El campo Código Postal debe tener una longitud de 5 caracteres.");
            }
            else if (afiliados.Codigo_Postal == 0)
            {
                if (!ValidateMaxInt(ModelState, "Codigo_Postal"))
                    ModelState.AddModelError("Codigo_Postal", "El valor del campo Código Postal no es válido.");
            }
            if (afiliados.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
            if (afiliados.Horas_Conductor == 0)
            {
                if (!ValidateMaxInt(ModelState, "Horas_Conductor"))
                    ModelState.AddModelError("Horas_Conductor", "El valor del campo Horas de Trabajo del Conductor no es válido.");
            }
            if ((afiliados.Cuota_Conductor ?? 0) == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Cuota_Conductor"))
                    ModelState.AddModelError("Cuota_Conductor", "El valor del campo Cuota del Conductor no es válido.");
            }
            if ((afiliados.Porcentaje_Conductor ?? 0) == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Porcentaje_Conductor"))
                    ModelState.AddModelError("Porcentaje_Conductor", "El valor del campo Porcentaje de Comisión del Conductor no es válido.");
            }
        }

        #endregion

        #region Bancos

        public void ValidarBanco(ModelStateDictionary ModelState, Bancos bancos)
        {
            if (!String.IsNullOrEmpty(bancos.Nombre))
            {
                bool error = db.Bancos.Where(c => c.ID_Banco != bancos.ID_Banco && c.Nombre == bancos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del banco ya se encuentra registrado.");
                if (bancos.Nombre.Contains("..") || bancos.Nombre.Contains(",,"))
                    ModelState.AddModelError("Nombre", "El nombre del banco no permite signos de puntuación repetidos.");
                if (!regexFraseComaPunto.IsMatch(bancos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del banco no permite números ni caracteres especiales, acepta solamente '.' y ','.");
                bancos.Nombre= System.Text.RegularExpressions.Regex.Replace(bancos.Nombre, @"\s+", " ");
                bancos.Nombre = bancos.Nombre.TrimStart();
            }
        }

        #endregion

        #region Calles

        public void ValidarCalle(ModelStateDictionary ModelState, Calles calles)
        {
            if (!String.IsNullOrEmpty(calles.Nombre))
            {
                bool error = db.Calles.Where(c => c.ID_Calle != calles.ID_Calle && c.Nombre == calles.Nombre && c.ID_Colonia == calles.ID_Colonia).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la calle ya se encuentra registrado para la colonia indicada.");
                if (calles.Nombre.Contains("..") || calles.Nombre.Contains(",,"))
                    ModelState.AddModelError("Nombre", "El nombre de la calle no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(calles.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la calle no permite caracteres especiales, acepta solamente '.' y ','.");
                if (!regexLatitud.IsMatch(calles.Latitud.ToString()))
                    ModelState.AddModelError("Latitud", "El campo Latitud solo admite números enteros y decimales.");
                if(!regexLongitud.IsMatch(calles.Longitud.ToString()))
                    ModelState.AddModelError("Longitud", "El campo Longitud solo admite números enteros y decimales.");
                calles.Nombre=System.Text.RegularExpressions.Regex.Replace(calles.Nombre, @"\s+", " ");
                calles.Nombre = calles.Nombre.TrimStart();
            }
        }

        #endregion

        #region Clientes

        public void ValidarCliente(ModelStateDictionary ModelState, Clientes clientes)
        {
            if (!String.IsNullOrEmpty(clientes.Nombre))
            {
                bool error = db.Clientes.Where(c => c.ID_Cliente != clientes.ID_Cliente && c.Nombre == clientes.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del cliente ya se encuentra registrado.");
                if (!regexFrase.IsMatch(clientes.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del cliente no permite números, caracteres especiales ni signos de puntuación.");
                clientes.Nombre = System.Text.RegularExpressions.Regex.Replace(clientes.Nombre, @"\s+", " ");
                clientes.Nombre = clientes.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(clientes.Numero_Interior_Origen))
                if (!regexPalabraNumeros.IsMatch(clientes.Numero_Interior_Origen))
                    ModelState.AddModelError("Numero_Interior_Origen", "El número interior origen del cliente no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(clientes.Numero_Interior_Destino))
                if (!regexPalabraNumeros.IsMatch(clientes.Numero_Interior_Destino))
                    ModelState.AddModelError("Numero_Interior_Destino", "El número interior destino del cliente no permite espacios ni caracteres especiales.");
            if (clientes.Numero_Exterior_Origen == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior_Origen"))
                    ModelState.AddModelError("Numero_Exterior_Origen", "El valor del campo Número Exterior Origen no es válido.");
            }
            if (clientes.Numero_Exterior_Destino == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior_Destino"))
                    ModelState.AddModelError("Numero_Exterior_Destino", "El valor del campo Número Exterior Destino no es válido.");
            }
        }

        #endregion

        #region ClientesAbonados

        public void ValidarClienteAbonado(ModelStateDictionary ModelState, ClientesAbonados clientesAbonados)
        {
            bool error;
            if (!String.IsNullOrEmpty(clientesAbonados.Nombre))
            {
                error = db.ClientesAbonados.Where(c => c.ID_ClienteAbonado != clientesAbonados.ID_ClienteAbonado && c.Nombre == clientesAbonados.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del cliente abonado ya se encuentra registrado.");
                if (!regexFrase.IsMatch(clientesAbonados.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del cliente abonado no permite números, caracteres especiales ni signos de puntuación.");
                clientesAbonados.Nombre = System.Text.RegularExpressions.Regex.Replace(clientesAbonados.Nombre, @"\s+", " ");
                clientesAbonados.Nombre = clientesAbonados.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(clientesAbonados.Email))
            {
                if (!regexEmail.IsMatch(clientesAbonados.Email))
                    ModelState.AddModelError("Email", "El formato del email es inválido.");
            }
            if (!String.IsNullOrEmpty(clientesAbonados.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(clientesAbonados.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior del cliente abonado no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(clientesAbonados.RFC))
            {
                error = db.ClientesAbonados.Where(c => c.ID_ClienteAbonado != clientesAbonados.ID_ClienteAbonado && c.RFC == clientesAbonados.RFC && c.ID_Afiliado == clientesAbonados.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("RFC", "El RFC del cliente abonado ya se encuentra registrado para el afiliado indicado.");
                if (!regexRFC.IsMatch(clientesAbonados.RFC))
                    ModelState.AddModelError("RFC", "El RFC del cliente abonado debe ser un RFC válido para el SAT, debe estar conformado por números y letras mayúsculas, y no permite espacios ni caracteres especiales a excepción del '@'.");
            }
            if (!String.IsNullOrEmpty(clientesAbonados.Clave_Automatica))
                if (!regexNumeros.IsMatch(clientesAbonados.Clave_Automatica))
                    ModelState.AddModelError("Clave_Automatica", "La clave automática del cliente abonado sólo permite números.");
            if (!String.IsNullOrEmpty(clientesAbonados.Clabe))
                if (!regexNumeros.IsMatch(clientesAbonados.Clabe))
                    ModelState.AddModelError("Clabe", "La clabe del cliente abonado sólo permite números.");
            if (!String.IsNullOrEmpty(clientesAbonados.Observaciones))
            {
                if (clientesAbonados.Observaciones.Contains("..") || clientesAbonados.Observaciones.Contains(",,"))
                    ModelState.AddModelError("Observaciones", "Las observaciones del cliente abonado no permiten signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(clientesAbonados.Observaciones))
                    ModelState.AddModelError("Observaciones", "Las observaciones del cliente abonado no permiten caracteres especiales.");
            }
            if (clientesAbonados.Fecha_Alta != null && clientesAbonados.Fecha_Baja != null)
            {
                error = clientesAbonados.Fecha_Alta >= clientesAbonados.Fecha_Baja;
                if (error)
                    ModelState.AddModelError("Fecha_Baja", "La fecha de baja debe ser mayor a la fecha de alta.");
            }
            if (clientesAbonados.Limite_Credito == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Limite_Credito"))
                    ModelState.AddModelError("Limite_Credito", "El valor del campo Límite de Crédito no es válido.");
            }
            if (clientesAbonados.CP > 0)
            {
                if (clientesAbonados.CP.ToString().Length != 5)
                    ModelState.AddModelError("CP", "El campo CP debe tener una longitud de 5 caracteres.");
            }
            else if (clientesAbonados.CP == 0)
            {
                if (!ValidateMaxInt(ModelState, "CP"))
                    ModelState.AddModelError("CP", "El valor del campo CP no es válido.");
            }
            if (clientesAbonados.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
        }


        #endregion

        #region ClientesHabituales

        public void ValidarClienteHabitual(ModelStateDictionary ModelState, ClientesHabituales clientesHabituales)
        {
            bool error;
            if (!String.IsNullOrEmpty(clientesHabituales.Nombre))
            {
                error = db.ClientesHabituales.Where(c => c.ID_ClienteHabitual != clientesHabituales.ID_ClienteHabitual && c.Nombre == clientesHabituales.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del cliente habitual ya se encuentra registrado.");
                if (!regexFrase.IsMatch(clientesHabituales.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del cliente habitual no permite números, caracteres especiales ni signos de puntuación.");
                clientesHabituales.Nombre = System.Text.RegularExpressions.Regex.Replace(clientesHabituales.Nombre, @"\s+", " ");
                clientesHabituales.Nombre = clientesHabituales.Nombre.TrimStart();
            }
            if (clientesHabituales.Fecha_Alta != null && clientesHabituales.Fecha_Baja != null)
            {
                error = clientesHabituales.Fecha_Alta >= clientesHabituales.Fecha_Baja;
                if (error)
                    ModelState.AddModelError("Fecha_Baja", "La fecha de baja debe ser mayor a la fecha de alta.");
            }
            if (!String.IsNullOrEmpty(clientesHabituales.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(clientesHabituales.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior del cliente habitual no permite espacios ni caracteres especiales.");
            if (clientesHabituales.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
        }

        #endregion

        #region Colonias

        public void ValidarColonia(ModelStateDictionary ModelState, Colonias colonias)
        {
            if (!String.IsNullOrEmpty(colonias.Nombre))
            {
                bool error = db.Colonias.Where(c => c.ID_Colonia != colonias.ID_Colonia && c.Nombre == colonias.Nombre && c.ID_Ciudad == colonias.ID_Ciudad).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la colonia ya se encuentra registrado para la ciudad indicada.");
                if (colonias.Nombre.Contains("..") || colonias.Nombre.Contains(",,"))
                    ModelState.AddModelError("Nombre", "El nombre de la colonia no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(colonias.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la colonia no permite caracteres especiales acepta solamente '.' y ','.");
                if (!regexLatitud.IsMatch(colonias.Latitud.ToString()))
                    ModelState.AddModelError("Latitud", "El campo Latitud solo admite números enteros y decimales.");
                if (!regexLongitud.IsMatch(colonias.Longitud.ToString()))
                    ModelState.AddModelError("Longitud", "El campo Longitud solo admite números enteros y decimales.");
                colonias.Nombre = System.Text.RegularExpressions.Regex.Replace(colonias.Nombre, @"\s+", " ");
                colonias.Nombre = colonias.Nombre.TrimStart();
            }
            if (colonias.Latitud < -90 || colonias.Latitud > 90)
                ModelState.AddModelError("Latitud", "Latitud debe ser desde -90 hasta 90.");
            if (colonias.Longitud < -180 || colonias.Longitud > 180)
                ModelState.AddModelError("Longitud", "Longitud debe ser desde -180 hasta 180.");
        }

        #endregion

        #region Colores

        public void ValidarColor(ModelStateDictionary ModelState, Colores colores)
        {
            if (!String.IsNullOrEmpty(colores.Nombre))
            {
                bool error = db.Colores.Where(c => c.ID_Color != colores.ID_Color && c.Nombre == colores.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del color ya se encuentra registrado.");
                if (!regexFrase.IsMatch(colores.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del color no permite números, caracteres especiales ni signos de puntuación.");
                colores.Nombre = System.Text.RegularExpressions.Regex.Replace(colores.Nombre, @"\s+", " ");
                colores.Nombre = colores.Nombre.TrimStart();
            }
        }

        #endregion

        #region Conductores

        public void ValidarConductor(ModelStateDictionary ModelState, Conductores conductores)
        {
            bool error;
            if (!String.IsNullOrEmpty(conductores.Nombre))
            {
                error = db.Conductores.ToList().Where(c => c.ID_Conductor != conductores.ID_Conductor && c.NombreCompleto == conductores.NombreCompleto && c.ID_Flota == conductores.ID_Flota).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre completo del conductor ya se encuentra registrado para la flota indicada.");
                if (!regexFrase.IsMatch(conductores.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del conductor no permite números, caracteres especiales ni signos de puntuación.");
                conductores.Nombre = System.Text.RegularExpressions.Regex.Replace(conductores.Nombre, @"\s+", " ");
                conductores.Nombre = conductores.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(conductores.Apellido))
            { 
                if (!regexFrase.IsMatch(conductores.Apellido))
                    ModelState.AddModelError("Apellido", "El apellido del conductor no permite números, caracteres especiales ni signos de puntuación.");
                conductores.Apellido = System.Text.RegularExpressions.Regex.Replace(conductores.Apellido, @"\s+", " ");
                conductores.Apellido = conductores.Apellido.TrimStart();
            }
            if (conductores.Fecha_Nacimiento != null)
            {
                error = conductores.Fecha_Nacimiento >= DateTime.Now;
                if (error)
                    ModelState.AddModelError("Fecha_Nacimiento", "La fecha de nacimiento del conductor no puede ser mayor a la fecha actual.");
                error = DateTime.Now.AddYears(-18) < conductores.Fecha_Nacimiento;
                if (error)
                    ModelState.AddModelError("Fecha_Nacimiento", "Debe ser mayor de edad para realizar el registro.");
                error = DateTime.Now > conductores.Fecha_Nacimiento.AddYears(100);
                if (error)
                    ModelState.AddModelError("Fecha_Nacimiento", "La edad límite para realizar el registro es de 100 años.");
            }
            if (!String.IsNullOrEmpty(conductores.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(conductores.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior del conductor no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(conductores.RFC))
            {
                error = db.Conductores.Where(c => c.ID_Conductor != conductores.ID_Conductor && c.RFC == conductores.RFC).Count() > 0;
                if (error)
                    ModelState.AddModelError("RFC", "El RFC del conductor ya se encuentra registrado para el afiliado indicado.");
                //if (!regexRFC.IsMatch(conductores.RFC))
                  //  ModelState.AddModelError("RFC", "El RFC del conductor debe ser un RFC válido para el SAT, debe estar conformado por números y letras mayúsculas, y no permite espacios ni caracteres especiales a excepción del '@'.");
            }
            if (!String.IsNullOrEmpty(conductores.NoLicencia))
                if (!regexPalabraNumeros.IsMatch(conductores.NoLicencia))
                    ModelState.AddModelError("NoLicencia", "El número de licencia del conductor no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(conductores.NoTarjeton))
                if (!regexNumeros.IsMatch(conductores.NoTarjeton))
                    ModelState.AddModelError("NoTarjeton", "El número de tarjetón del conductor sólo permite números.");
            if (conductores.Vigencia_Licencia > 0)
            {
                error = DateTime.Now.Year + 10 < conductores.Vigencia_Licencia;
                if (error)
                    ModelState.AddModelError("Vigencia_Licencia", "El valor del campo Año de Vencimiento (Licencia) no es congruente. Vigencia mayor a 10 años no es válida");
                if(conductores.Vigencia_Licencia < DateTime.Now.Year)
                    ModelState.AddModelError("Vigencia_Licencia", "El valor del campo Año de Vencimiento (Licencia) no es válido. La Vigencia debe ser mayor o igual al año en curso.");

            }
            else if (conductores.Vigencia_Licencia == 0)
            {
                if (!ValidateMaxInt(ModelState, "Vigencia_Licencia"))
                    ModelState.AddModelError("Vigencia_Licencia", "El valor del campo Año de Vencimiento (Licencia) no es válido. La Vigencia debe ser mayor o igual al año en curso.");
            }
            if (conductores.Vigencia_Tarjeton > 0)
            {
                error = DateTime.Now.Year + 10 < conductores.Vigencia_Tarjeton;
                if (error)
                    ModelState.AddModelError("Vigencia_Tarjeton", "El valor del campo Año de Vencimiento (Tarjetón) no es congruente. Vigencia mayor a 10 años no es válida");
                if (conductores.Vigencia_Tarjeton < DateTime.Now.Year)
                    ModelState.AddModelError("Vigencia_Tarjeton", "El valor del campo Año de Vencimiento (Tarjetón) no es válido. La Vigencia debe ser mayor o igual al año en curso.");
            }
            else if (conductores.Vigencia_Tarjeton == 0)
            {
                if (!ValidateMaxInt(ModelState, "Vigencia_Tarjeton"))
                    ModelState.AddModelError("Vigencia_Tarjeton", "El valor del campo Año de Vencimiento (Tarjetón) no es válido. La Vigencia debe ser mayor o igual al año en curso.");
            }
            if (conductores.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_Antidoping))
            {
                if (!ImagenEsValida(conductores.Archivo_Antidoping))
                {
                    ModelState.AddModelError("Archivo_Antidoping", "La extensión del Archivo Antidoping debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_Antidoping))
                {
                    ModelState.AddModelError("Archivo_Antidoping", "El peso del Archivo Antidoping no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_Antidoping", "El campo Archivo Antidoping es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_CartaFianza))
            {
                if (!ImagenEsValida(conductores.Archivo_CartaFianza))
                {
                    ModelState.AddModelError("Archivo_CartaFianza", "La extensión del Archivo Carta Fianza debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_CartaFianza))
                {
                    ModelState.AddModelError("Archivo_CartaFianza", "El peso del Archivo Carta Fianza no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_CartaFianza", "El campo Archivo Carta Fianza es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_Domicilio))
            {
                if (!ImagenEsValida(conductores.Archivo_Domicilio))
                {
                    ModelState.AddModelError("Archivo_Domicilio", "La extensión del Archivo Domicilio debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_Domicilio))
                {
                    ModelState.AddModelError("Archivo_Domicilio", "El peso del Archivo Domicilio no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_Domicilio", "El campo Archivo Domicilio es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_INE))
            {
                if (!ImagenEsValida(conductores.Archivo_INE))
                {
                    ModelState.AddModelError("Archivo_INE", "La extensión del Archivo INE debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_INE))
                {
                    ModelState.AddModelError("Archivo_INE", "El peso del Archivo INE no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_INE", "El campo Archivo INE es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_Licencia))
            {
                if (!ImagenEsValida(conductores.Archivo_Licencia))
                {
                    ModelState.AddModelError("Archivo_Licencia", "La extensión del Archivo No. Licencia debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_Licencia))
                {
                    ModelState.AddModelError("Archivo_Licencia", "El peso del Archivo No. Licencia no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_Licencia", "El campo Archivo No. Licencia es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_NoAntecedentes))
            {
                if (!ImagenEsValida(conductores.Archivo_NoAntecedentes))
                {
                    ModelState.AddModelError("Archivo_NoAntecedentes", "La extensión del Archivo No. Antecedentes debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_NoAntecedentes))
                {
                    ModelState.AddModelError("Archivo_NoAntecedentes", "El peso del Archivo No. Antecedentes no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_NoAntecedentes", "El campo Archivo No. Antecedentes es requerido.");
            }
            if (!String.IsNullOrEmpty(conductores.Archivo_Tarjeton))
            {
                if (!ImagenEsValida(conductores.Archivo_Tarjeton))
                {
                    ModelState.AddModelError("Archivo_Tarjeton", "La extensión del Archivo No. Tarjetón debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(conductores.Peso_Archivo_Tarjeton))
                {
                    ModelState.AddModelError("Archivo_Tarjeton", "El peso del Archivo No. Tarjetón no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Archivo_Tarjeton", "El campo Archivo No. Tarjetón es requerido.");
            }
        }

        private bool PesoImagenEsValido(int size)
        {
            bool valid = (size > 0) && (size <= (1024 * 1024 * 1024));
            return valid;
        }

        private bool ImagenEsValida(string file)
        {
            List<string> extensions = new List<string>() { ".jpg", ".png", ".jpeg" };
            bool valid = extensions.Contains(Path.GetExtension(file).ToLower());
            return valid;
        }

        public bool ValidaAssignUser(ModelStateDictionary ModelState, Conductores conductores)
        {
            if (conductores.UserID_Conductor==null)
            {
                ModelState.AddModelError("UserID_Conductor", "Debe seleccionar una opción de la lista.");
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Cupones

        public void ValidarCupon(ModelStateDictionary ModelState, Cupones cupones, string from="")
        {
            bool error;
            if (!String.IsNullOrEmpty(cupones.Codigo))
            {
                error = db.Cupones.Where(c => c.ID_Cupon != cupones.ID_Cupon && c.Codigo == cupones.Codigo && c.ID_Afiliado == cupones.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Codigo", "El código del cupón ya se encuentra registrado para el afiliado indicado.");
                if (!regexPalabraNumerosGuionDiagonal.IsMatch(cupones.Codigo))
                    ModelState.AddModelError("Codigo", "El código del cupón no permite espacios ni caracteres especiales, excepto - y /.");
            }
            
            if (cupones.Fecha_Inicio != null && from!="edit")
            {
                error = cupones.Fecha_Inicio.Date < new Utilities().ConvertToMexicanDate(DateTime.Now.AddMinutes(-5)).Date;
                if (error)
                    ModelState.AddModelError("Fecha_Inicio", "La fecha de inicio no puede ser menor a la fecha actual.");
            }
            if (cupones.Fecha_Inicio != null && cupones.Fecha_Fin != null)
            {
                error = cupones.Fecha_Inicio >= cupones.Fecha_Fin;
                if (error)
                    ModelState.AddModelError("Fecha_Fin", "La fecha de finalización debe ser mayor a la fecha de inicio.");
            }
            if (!String.IsNullOrEmpty(cupones.Descripcion))
            {
                if (cupones.Descripcion.Contains("..") || cupones.Descripcion.Contains(",,"))
                    ModelState.AddModelError("Descripcion", "La descripción del cupón no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(cupones.Descripcion))
                    ModelState.AddModelError("Descripcion", "La descripción del cupón no permite caracteres especiales, acepta solamente '.' y ','");
                cupones.Descripcion = System.Text.RegularExpressions.Regex.Replace(cupones.Descripcion, @"\s+", " ");
                cupones.Descripcion = cupones.Descripcion.TrimStart();
            }
            if (cupones.Descuento.ToString().Contains("."))
            {
                ModelState.AddModelError("Descuento", "Descuento solo puede contener números enteros.");
            }
            if (cupones.Cantidad <= 0)
            {
                if (!ValidateMaxInt(ModelState, "Cantidad"))
                    ModelState.AddModelError("Cantidad", "El valor del campo Cantidad debe ser mayor a cero, número entero y no mayor a 2,147,483,647.");
            }

            if (cupones.Descuento <= 0)
            {
                if (!ValidateMaxInt(ModelState, "Descuento"))
                    ModelState.AddModelError("Descuento", "El valor del campo Descuento debe ser un entero entre uno y cien.");
            }
            
        }

        #endregion

        #region DatosTickets

        public void ValidarDatosTicket(ModelStateDictionary ModelState, DatosTickets datosTickets)
        {
            if (!String.IsNullOrEmpty(datosTickets.Texto_1))
            {
                if (datosTickets.Texto_1.Contains("..") || datosTickets.Texto_1.Contains(",,"))
                    ModelState.AddModelError("Texto_1", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_1))
                    ModelState.AddModelError("Texto_1", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_2))
            {
                if (datosTickets.Texto_2.Contains("..") || datosTickets.Texto_2.Contains(",,"))
                    ModelState.AddModelError("Texto_2", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_2))
                    ModelState.AddModelError("Texto_2", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_3))
            {
                if (datosTickets.Texto_3.Contains("..") || datosTickets.Texto_3.Contains(",,"))
                    ModelState.AddModelError("Texto_3", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_3))
                    ModelState.AddModelError("Texto_3", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_4))
            {
                if (datosTickets.Texto_4.Contains("..") || datosTickets.Texto_4.Contains(",,"))
                    ModelState.AddModelError("Texto_4", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_4))
                    ModelState.AddModelError("Texto_4", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_5))
            {
                if (datosTickets.Texto_5.Contains("..") || datosTickets.Texto_5.Contains(",,"))
                    ModelState.AddModelError("Texto_5", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_5))
                    ModelState.AddModelError("Texto_5", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_6))
            {
                if (datosTickets.Texto_6.Contains("..") || datosTickets.Texto_6.Contains(",,"))
                    ModelState.AddModelError("Texto_6", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_6))
                    ModelState.AddModelError("Texto_6", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_7))
            {
                if (datosTickets.Texto_7.Contains("..") || datosTickets.Texto_7.Contains(",,"))
                    ModelState.AddModelError("Texto_7", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_7))
                    ModelState.AddModelError("Texto_7", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_8))
            {
                if (datosTickets.Texto_8.Contains("..") || datosTickets.Texto_8.Contains(",,"))
                    ModelState.AddModelError("Texto_8", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_8))
                    ModelState.AddModelError("Texto_8", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_9))
            {
                if (datosTickets.Texto_9.Contains("..") || datosTickets.Texto_9.Contains(",,"))
                    ModelState.AddModelError("Texto_9", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_9))
                    ModelState.AddModelError("Texto_9", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Texto_10))
            {
                if (datosTickets.Texto_10.Contains("..") || datosTickets.Texto_10.Contains(",,"))
                    ModelState.AddModelError("Texto_10", "Este texto no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(datosTickets.Texto_10))
                    ModelState.AddModelError("Texto_10", "Este texto no permite caracteres especiales, acepta solamente '.' y ','.");
            }
            if (!String.IsNullOrEmpty(datosTickets.Imagen))
            {
                if (!ImagenEsValida(datosTickets.Imagen))
                {
                    ModelState.AddModelError("Imagen", "La extensión de la Imagen debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(datosTickets.Peso_Imagen))
                {
                    ModelState.AddModelError("Imagen", "El peso de la Imagen no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Imagen", "El campo Imagen es requerido.");
            }
        }

        #endregion

        #region DiasFestivos

        public void ValidarDiaFestivo(ModelStateDictionary ModelState, DiasFestivos diasFestivos)
        {
            if (!String.IsNullOrEmpty(diasFestivos.Nombre))
            {
                bool error = db.DiasFestivos.Where(c => c.ID_DiaFestivo != diasFestivos.ID_DiaFestivo && c.Nombre == diasFestivos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del día festivo ya se encuentra registrado.");
                if (!regexFrase.IsMatch(diasFestivos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del día festivo no permite números, caracteres especiales ni signos de puntuación.");
                diasFestivos.Nombre = System.Text.RegularExpressions.Regex.Replace(diasFestivos.Nombre, @"\s+", " ");
                diasFestivos.Nombre = diasFestivos.Nombre.TrimStart();
            }
        }

        #endregion

        #region Empresas

        public void ValidarEmpresa(ModelStateDictionary ModelState, Empresas empresas)
        {
            if (!String.IsNullOrEmpty(empresas.Nombre))
            {
                bool error = db.Empresas.Where(c => c.ID_Empresa != empresas.ID_Empresa && c.Nombre == empresas.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la empresa ya se encuentra registrado.");
                if (!regexFraseComaPunto.IsMatch(empresas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la empresa no permite números, caracteres especiales ni signos de puntuación.");
                empresas.Nombre = System.Text.RegularExpressions.Regex.Replace(empresas.Nombre, @"\s+", " ");
                empresas.Nombre = empresas.Nombre.TrimStart();
            }
        }

        #endregion

        #region Estatus

        public void ValidarEstatus(ModelStateDictionary ModelState, Estatus estatus)
        {
            if (!String.IsNullOrEmpty(estatus.Nombre))
            {
                bool error = db.Estatus.Where(c => c.ID_Estatus != estatus.ID_Estatus && c.Nombre == estatus.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del Status ya se encuentra registrado.");
                if (!regexFrase.IsMatch(estatus.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del Status no permite números, caracteres especiales ni signos de puntuación.");
                estatus.Nombre = System.Text.RegularExpressions.Regex.Replace(estatus.Nombre, @"\s+", " ");
                estatus.Nombre = estatus.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(estatus.Imagen))
            {
                if (!ImagenEsValida(estatus.Imagen))
                {
                    ModelState.AddModelError("Imagen", "La extensión de la Imagen debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(estatus.Peso_Imagen))
                {
                    ModelState.AddModelError("Imagen", "El peso de la Imagen no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Imagen", "El campo Imagen es requerido.");
            }
        }

        #endregion

        #region Estatus_Reservas

        public void ValidarEstatusReservas(ModelStateDictionary ModelState, Estatus_Reserva estatus_reserva)
        {
            bool error;
            if (!String.IsNullOrEmpty(estatus_reserva.Nombre))
            {
                error = db.Estatus_Reserva.Where(c => c.ID_Estatus_Reserva != estatus_reserva.ID_Estatus_Reserva && c.Nombre == estatus_reserva.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del estatus ya se encuentra registrado.");
                if (!regexFrase.IsMatch(estatus_reserva.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del estatus no permite números, caracteres especiales ni signos de puntuación.");
                estatus_reserva.Nombre = System.Text.RegularExpressions.Regex.Replace(estatus_reserva.Nombre, @"\s+", " ");
                estatus_reserva.Nombre = estatus_reserva.Nombre.TrimStart();
            }
            error = db.Estatus_Reserva.Where(c => c.ID_Estatus_Reserva != estatus_reserva.ID_Estatus_Reserva && c.Orden == estatus_reserva.Orden).Count() > 0;
            if (error)
                ModelState.AddModelError("Orden", "El número de orden ya se encuentra registrado para otro estatus.");
            else
            {
                if (estatus_reserva.Orden == 0)
                {
                    if (!ValidateMaxInt(ModelState, "Orden"))
                        ModelState.AddModelError("Orden", "El valor del campo Orden no es válido.");
                }
            }
            if (!String.IsNullOrEmpty(estatus_reserva.Descripcion))
            {
                if (estatus_reserva.Descripcion.Contains("..") || estatus_reserva.Descripcion.Contains(",,"))
                    ModelState.AddModelError("Descripcion", "La descripción del estatus no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(estatus_reserva.Descripcion))
                    ModelState.AddModelError("Descripcion", "La descripción del estatus no permite caracteres especiales.");
                estatus_reserva.Descripcion = System.Text.RegularExpressions.Regex.Replace(estatus_reserva.Descripcion, @"\s+", " ");
                estatus_reserva.Descripcion = estatus_reserva.Descripcion.TrimStart();
            }
        }

        #endregion

        #region Flotas

        public void ValidarFlota(ModelStateDictionary ModelState, Flotas flotas)
        {
            bool error;
            if (!String.IsNullOrEmpty(flotas.Nombre))
            {
                error = db.Flotas.Where(c => c.ID_Flota != flotas.ID_Flota && c.Nombre == flotas.Nombre && c.ID_Afiliado == flotas.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la flota ya se encuentra registrado para el afiliado indicado.");
                if (!regexFrase.IsMatch(flotas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la flota no permite números, caracteres especiales ni signos de puntuación.");
                flotas.Nombre = System.Text.RegularExpressions.Regex.Replace(flotas.Nombre, @"\s+", " ");
                flotas.Nombre = flotas.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(flotas.Email))
            {
                if (!regexEmail.IsMatch(flotas.Email))
                    ModelState.AddModelError("Email", "El formato del email es inválido.");
            }
            if (!String.IsNullOrEmpty(flotas.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(flotas.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior de la flota no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(flotas.RFC))
            {
                error = db.Flotas.Where(c => c.ID_Flota != flotas.ID_Flota && c.RFC == flotas.RFC && c.ID_Afiliado == flotas.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("RFC", "El RFC de la flota ya se encuentra registrado para el afiliado indicado.");
                //if (!regexRFC.IsMatch(flotas.RFC))
                  //  ModelState.AddModelError("RFC", "El RFC de la flota debe ser un RFC válido para el SAT, debe estar conformado por números y letras mayúsculas, y no permite espacios ni caracteres especiales a excepción del '@'.");
            }
            if (!String.IsNullOrEmpty(flotas.Clabe))
                if (!regexNumeros.IsMatch(flotas.Clabe))
                    ModelState.AddModelError("Clabe", "La clabe de la flota sólo permite números.");
            if (flotas.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
        }

        public void ValidaFlotaLiquidar(ModelStateDictionary ModelState, Conductores conductores)
        {
            if(conductores.UserID_Conductor==null)
            {
                ModelState.AddModelError("ID_Flota", "Ocurrió un error.");
            }
        }

        #endregion

        #region FormasPago

        public void ValidarFormaPago(ModelStateDictionary ModelState, FormasPago formasPago)
        {
            if (!String.IsNullOrEmpty(formasPago.Nombre))
            {
                bool error = db.FormasPago.Where(c => c.ID_FormaPago != formasPago.ID_FormaPago && c.Nombre == formasPago.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la forma de pago ya se encuentra registrado.");
                if (!regexFrase.IsMatch(formasPago.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la forma de pago no permite números, caracteres especiales ni signos de puntuación.");
                formasPago.Nombre = System.Text.RegularExpressions.Regex.Replace(formasPago.Nombre, @"\s+", " ");
                formasPago.Nombre = formasPago.Nombre.TrimStart();
            }
        }

        #endregion

        #region FrecuenciasPago

        public void ValidarFrecuenciaPago(ModelStateDictionary ModelState, FrecuenciasPago frecuenciasPagos)
        {
            if (!String.IsNullOrEmpty(frecuenciasPagos.Nombre))
            {
                bool error = db.FrecuenciasPago.Where(c => c.ID_FrecuenciaPago != frecuenciasPagos.ID_FrecuenciaPago && c.Nombre == frecuenciasPagos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la frecuencia de pago ya se encuentra registrado.");
                if (!regexFrase.IsMatch(frecuenciasPagos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la frecuencia de pago no permite números, caracteres especiales ni signos de puntuación.");
                frecuenciasPagos.Nombre = System.Text.RegularExpressions.Regex.Replace(frecuenciasPagos.Nombre, @"\s+", " ");
                frecuenciasPagos.Nombre = frecuenciasPagos.Nombre.TrimStart();
            }
        }

        #endregion
    
        #region Marcas

        public void ValidarMarca(ModelStateDictionary ModelState, Marcas marcas)
        {
            if (!String.IsNullOrEmpty(marcas.Nombre))
            {
                bool error = db.Marcas.Where(c => c.ID_Marca != marcas.ID_Marca && c.Nombre == marcas.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la marca ya se encuentra registrado.");
                if (!regexFrase.IsMatch(marcas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la marca no permite números, caracteres especiales ni signos de puntuación.");
                marcas.Nombre = System.Text.RegularExpressions.Regex.Replace(marcas.Nombre, @"\s+", " ");
                marcas.Nombre = marcas.Nombre.TrimStart();
            }
        }

        #endregion

        #region MensajesPredefinidos

        public void ValidarMensajePredefinido(ModelStateDictionary ModelState, MensajesPredefinidos mensajesPredefinidos)
        {
            if (!String.IsNullOrEmpty(mensajesPredefinidos.Texto))
            {
                bool error = db.MensajesPredefinidos.Where(c => c.ID_MensajePredefinido != mensajesPredefinidos.ID_MensajePredefinido && c.Texto == mensajesPredefinidos.Texto).Count() > 0;
                if (error)
                    ModelState.AddModelError("Texto", "El texto del mensaje predefinido ya se encuentra registrado.");
                if (mensajesPredefinidos.Texto.Contains("..") || mensajesPredefinidos.Texto.Contains(",,"))
                    ModelState.AddModelError("Texto", "El texto del mensaje predefinido no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(mensajesPredefinidos.Texto))
                    ModelState.AddModelError("Texto", "El texto del mensaje predefinido no permite caracteres especiales, acepta solamente '.' y ','.");
            }
        }

        #endregion

        #region Modelos

        public void ValidarModelo(ModelStateDictionary ModelState, Modelos modelos)
        {
            if (!String.IsNullOrEmpty(modelos.Nombre))
            {
                bool error = db.Modelos.Where(c => c.ID_Modelo != modelos.ID_Modelo && c.Nombre == modelos.Nombre && c.ID_Marca == modelos.ID_Marca).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del modelo ya se encuentra registrado para la marca indicada.");
                if (!regexFraseNumeros.IsMatch(modelos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del modelo no permite caracteres especiales.");
                modelos.Nombre = System.Text.RegularExpressions.Regex.Replace(modelos.Nombre, @"\s+", " ");
                modelos.Nombre = modelos.Nombre.TrimStart();
            }
        }

        #endregion

        #region Operadores

        public void ValidarOperador(ModelStateDictionary ModelState, Operadores operadores)
        {
            bool error;
            if (!String.IsNullOrEmpty(operadores.Nombre))
            {
                error = db.Operadores.Where(c => c.ID_Operador != operadores.ID_Operador && c.Nombre == operadores.Nombre && c.ID_Afiliado == operadores.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del operador ya se encuentra registrado para el afiliado indicado.");
                if (!regexFrase.IsMatch(operadores.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del operador no permite números, caracteres especiales ni signos de puntuación.");
                operadores.Nombre = System.Text.RegularExpressions.Regex.Replace(operadores.Nombre, @"\s+", " ");
                operadores.Nombre = operadores.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(operadores.Numero_Interior))
                if (!regexPalabraNumeros.IsMatch(operadores.Numero_Interior))
                    ModelState.AddModelError("Numero_Interior", "El número interior del operador no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(operadores.RFC))
            {
                error = db.Operadores.Where(c => c.ID_Operador != operadores.ID_Operador && c.RFC == operadores.RFC && c.ID_Afiliado == operadores.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("RFC", "El RFC del operador ya se encuentra registrado para el afiliado indicado.");
                if (!regexRFC.IsMatch(operadores.RFC))
                    ModelState.AddModelError("RFC", "El RFC del operador debe ser un RFC válido para el SAT, debe estar conformado por números y letras mayúsculas, y no permite espacios ni caracteres especiales a excepción del '@'.");
            }
            if (operadores.Codigo_Postal > 0)
            {
                if (operadores.Codigo_Postal.ToString().Length != 5)
                    ModelState.AddModelError("Codigo_Postal", "El campo Código Postal debe tener una longitud de 5 caracteres.");
            }
            else if (operadores.Codigo_Postal == 0)
            {
                if (!ValidateMaxInt(ModelState, "Codigo_Postal"))
                    ModelState.AddModelError("Codigo_Postal", "El valor del campo Código Postal no es válido.");
            }
            if (operadores.Numero_Exterior == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior"))
                    ModelState.AddModelError("Numero_Exterior", "El valor del campo Número Exterior no es válido.");
            }
            if (operadores.Objetivo_Mensual == 0)
            {
                if (!ValidateMaxInt(ModelState, "Objetivo_Mensual"))
                    ModelState.AddModelError("Objetivo_Mensual", "El valor del campo Objetivo Mensual no es válido.");
            }
        }

        #endregion

        #region ParticionesCalles

        public void ValidarLocalizacion(ModelStateDictionary ModelState, ParticionesCalles particiones)
        {
            bool error = db.ParticionesCalles.Where(x => x.Latitud == particiones.Latitud).Count() > 0;            
            if (error)                
                ModelState.AddModelError("Latitud", "La Latitud especificada ya se encuentra registrada.");
            error = db.ParticionesCalles.Where(x => x.Longitud == particiones.Longitud).Count() > 0;
            if (error)
                ModelState.AddModelError("Longitud", "La Longitud especificada ya se encuentra registrada.");
            if (particiones.Latitud < -90 || particiones.Latitud > 90)
                ModelState.AddModelError("Latitud", "Latitud debe ser desde -90 hasta 90.");
            if (particiones.Longitud < -180 || particiones.Longitud > 180)
                ModelState.AddModelError("Longitud", "Longitud debe ser desde -180 hasta 180.");
            if (!regexLatitud.IsMatch(particiones.Latitud.ToString()))
                ModelState.AddModelError("Latitud", "El campo Latitud solo admite números enteros y decimales.");
            if (!regexLongitud.IsMatch(particiones.Longitud.ToString()))
                ModelState.AddModelError("Longitud", "El campo Longitud solo admite números enteros y decimales.");
        }

        #endregion

        #region Perfiles

        public void ValidarPerfil(ModelStateDictionary ModelState, AspNetRoles perfiles)
        {
            if (!String.IsNullOrEmpty(perfiles.Name))
            {
                bool error = db.AspNetRoles.Where(c => c.Id != perfiles.Id && c.Name == perfiles.Name).Count() > 0;
                if (error)
                    ModelState.AddModelError("Name", "El nombre del perfil ya se encuentra registrado.");
                if (!regexFrase.IsMatch(perfiles.Name))
                    ModelState.AddModelError("Name", "El nombre del perfil no permite números, caracteres especiales ni signos de puntuación.");
                perfiles.Name = System.Text.RegularExpressions.Regex.Replace(perfiles.Name, @"\s+", " ");
                perfiles.Name = perfiles.Name.TrimStart();
            }
        }

        #endregion

        #region Permissions

        public void ValidarPermiso(ModelStateDictionary ModelState, Permissions permisos)
        {
            if (!String.IsNullOrEmpty(permisos.Name))
            {
                bool error = db.Permissions.Where(c => c.Id != permisos.Id && c.Name == permisos.Name).Count() > 0;
                if (error)
                    ModelState.AddModelError("Name", "El nombre del permiso ya se encuentra registrado.");
                if (!regexFrase.IsMatch(permisos.Name))
                    ModelState.AddModelError("Name", "El nombre del permiso no permite números, caracteres especiales ni signos de puntuación.");
                permisos.Name = System.Text.RegularExpressions.Regex.Replace(permisos.Name, @"\s+", " ");
                permisos.Name = permisos.Name.TrimStart();
            }
            if (!String.IsNullOrEmpty(permisos.Description))
            {
                if (permisos.Description.Contains("..") || permisos.Description.Contains(",,"))
                    ModelState.AddModelError("Description", "La descripción del permiso no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(permisos.Description))
                    ModelState.AddModelError("Description", "La descripción del permiso no permite caracteres especiales, acepta solamente '.' y ','");
            }
        }

        #endregion

        #region Positions

        public void ValidarPosition(ModelStateDictionary ModelState, Positions positions)
        {
            if (!String.IsNullOrEmpty(positions.Position))
            {
                bool error = db.Positions.Where(c => c.Id != positions.Id && c.Position == positions.Position).Count() > 0;
                if (error)
                    ModelState.AddModelError("Position", "El nombre de la posición ya se encuentra registrado.");
                if (!regexFrase.IsMatch(positions.Position))
                    ModelState.AddModelError("Position", "El nombre de la posición no permite números, caracteres especiales ni signos de puntuación.");
                positions.Position = System.Text.RegularExpressions.Regex.Replace(positions.Position, @"\s+", " ");
                positions.Position = positions.Position.TrimStart();
            }
        }

        #endregion

        #region PuntosInteres

        public void ValidarPuntoInteres(ModelStateDictionary ModelState, PuntosInteres puntosInteres)
        {
            if (!String.IsNullOrEmpty(puntosInteres.Nombre))
            {
                bool error = db.PuntosInteres.Where(c => c.ID_PuntoInteres != puntosInteres.ID_PuntoInteres && c.Nombre == puntosInteres.Nombre && c.ID_Afiliado == puntosInteres.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del punto de interés ya se encuentra registrado para el afiliado indicado.");
                if (!regexFrase.IsMatch(puntosInteres.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del punto de interés no permite números, caracteres especiales ni signos de puntuación.");
                puntosInteres.Nombre = System.Text.RegularExpressions.Regex.Replace(puntosInteres.Nombre, @"\s+", " ");
                puntosInteres.Nombre = puntosInteres.Nombre.TrimStart();
            }
        }

        #endregion

        #region RazonesLlamadas

        public void ValidarRazonLlamada(ModelStateDictionary ModelState, RazonesLlamadas razonesLlamadas)
        {
            if (!String.IsNullOrEmpty(razonesLlamadas.Nombre))
            {
                bool error = db.RazonesLlamadas.Where(c => c.ID_RazonLlamada != razonesLlamadas.ID_RazonLlamada && c.Nombre == razonesLlamadas.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la razón de llamada ya se encuentra registrado.");
                if (!regexFrase.IsMatch(razonesLlamadas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la razón de llamada no permite números, caracteres especiales ni signos de puntuación.");
                razonesLlamadas.Nombre = System.Text.RegularExpressions.Regex.Replace(razonesLlamadas.Nombre, @"\s+", " ");
                razonesLlamadas.Nombre = razonesLlamadas.Nombre.TrimStart();
            }
        }

        #endregion

        #region RazonesRechazos

        public void ValidarRazonRechazo(ModelStateDictionary ModelState, RazonesRechazos razonesRechazos)
        {
            if (!String.IsNullOrEmpty(razonesRechazos.Nombre))
            {
                bool error = db.RazonesRechazos.Where(c => c.ID_RazonRechazo != razonesRechazos.ID_RazonRechazo && c.Nombre == razonesRechazos.Nombre && c.ID_TipoRechazo == razonesRechazos.ID_TipoRechazo).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la razón de rechazo ya se encuentra registrado para el tipo de rechazo indicado.");
                if (!regexFrase.IsMatch(razonesRechazos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la razón de rechazo no permite números, caracteres especiales ni signos de puntuación.");
                razonesRechazos.Nombre = System.Text.RegularExpressions.Regex.Replace(razonesRechazos.Nombre, @"\s+", " ");
                razonesRechazos.Nombre = razonesRechazos.Nombre.TrimStart();
            }
        }

        #endregion

        #region Reservas

        public bool IsValidReserva(Reservas reservas)
        {
            bool valid = reservas.ID_Cliente.HasValue || reservas.ID_ClienteAbonado.HasValue || reservas.ID_ClienteHabitual.HasValue;
            return valid;
        }

        #endregion

        #region Rutas

        public void ValidarRuta(ModelStateDictionary ModelState, Rutas rutas)
        {
            if (!String.IsNullOrEmpty(rutas.Nombre))
            {
                bool error = db.Rutas.Where(c => c.ID_Ruta != rutas.ID_Ruta && c.Nombre == rutas.Nombre && c.ID_UsuarioAbonado == rutas.ID_UsuarioAbonado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la ruta ya se encuentra registrado para el usuario abonado indicado.");
                if (!regexFrase.IsMatch(rutas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la ruta no permite números, caracteres especiales ni signos de puntuación.");
            }
            if (!String.IsNullOrEmpty(rutas.Numero_Interior_Origen))
                if (!regexPalabraNumeros.IsMatch(rutas.Numero_Interior_Origen))
                    ModelState.AddModelError("Numero_Interior_Origen", "El número interior origen de la ruta no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(rutas.Numero_Interior_Destino))
                if (!regexPalabraNumeros.IsMatch(rutas.Numero_Interior_Destino))
                    ModelState.AddModelError("Numero_Interior_Destino", "El número interior destino de la ruta no permite espacios ni caracteres especiales.");
            if (rutas.Precio == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Precio"))
                    ModelState.AddModelError("Precio", "El valor del campo Precio no es válido.");
            }
            if (rutas.Numero_Exterior_Origen == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior_Origen"))
                    ModelState.AddModelError("Numero_Exterior_Origen", "El valor del campo Número Exterior Origen no es válido.");
            }
            if (rutas.Numero_Exterior_Destino == 0)
            {
                if (!ValidateMaxInt(ModelState, "Numero_Exterior_Destino"))
                    ModelState.AddModelError("Numero_Exterior_Destino", "El valor del campo Número Exterior Destino no es válido.");
            }
        }

        #endregion

        #region Sanciones

        public void ValidarSancion(ModelStateDictionary ModelState, Sanciones sanciones)
        {
            bool error;            
            if (sanciones.Fecha_Inicio != null)
            {
                error = (sanciones.Fecha_Inicio <= new Utilities().ConvertToMexicanDate(DateTime.Now)
                            && sanciones.ID_Sancion == 0);
                if (error)
                    ModelState.AddModelError("Fecha_Inicio", "La fecha de inicio no puede ser menor a la fecha actual.");
            }
            if (sanciones.Fecha_Inicio != null && sanciones.Fecha_Fin != null)
            {
                error = sanciones.Fecha_Inicio >= sanciones.Fecha_Fin;
                if (error)
                    ModelState.AddModelError("Fecha_Fin", "La fecha de finalización debe ser mayor a la fecha de inicio.");
            }
            if (!String.IsNullOrEmpty(sanciones.Observaciones))
            {
                if (sanciones.Observaciones.Contains("..") || sanciones.Observaciones.Contains(",,"))
                    ModelState.AddModelError("Observaciones", "Las observaciones de la sanción no permiten signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(sanciones.Observaciones))
                    ModelState.AddModelError("Observaciones", "Las observaciones de la sanción no permiten caracteres especiales.");
            }
            if (sanciones.ID_Conductor > 0)
            {
                if (db.Conductores.Find(sanciones.ID_Conductor).Flotas.Afiliados.Operadores == null)
                {
                    ModelState.AddModelError("ID_Conductor", "La sucursal a la que pertenece el conductor no posee operadores.");
                }
            }
        }

        #endregion

        #region Seguros

        public void ValidarSeguro(ModelStateDictionary ModelState, Seguros seguros)
        {
            if (!String.IsNullOrEmpty(seguros.Nombre))
            {
                bool error = db.Seguros.Where(c => c.ID_Seguro != seguros.ID_Seguro && c.Nombre == seguros.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del seguro ya se encuentra registrado.");
                if (!regexFrase.IsMatch(seguros.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre del seguro no permite números, caracteres especiales ni signos de puntuación.");
                    seguros.Nombre = System.Text.RegularExpressions.Regex.Replace(seguros.Nombre, @"\s+", " ");
                    seguros.Nombre = seguros.Nombre.TrimStart();
                }
                if (!String.IsNullOrEmpty(seguros.Telefono) && !ModelState.IsValidField("Telefono"))
                {
                    if (!(seguros.Telefono.StartsWith("01800") && seguros.Telefono.Count() == 12))
                    {
                        ModelState.AddModelError("Telefono", "El formato del campo Teléfono no es válido.");
                    }
                }
            }
        }

        #endregion

        #region ServiciosAuxiliares

        public void ValidarServicioAuxiliar(ModelStateDictionary ModelState, ServiciosAuxiliares serviciosAuxiliares)
        {
            if (!String.IsNullOrEmpty(serviciosAuxiliares.Nombre))
            {
                bool error = db.ServiciosAuxiliares.Where(c => c.ID_ServicioAuxiliar != serviciosAuxiliares.ID_ServicioAuxiliar && c.Nombre == serviciosAuxiliares.Nombre && c.ID_Afiliado == serviciosAuxiliares.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del servicio auxiliar ya se encuentra registrado para el afiliado indicado.");
                if (!regexFrase.IsMatch(serviciosAuxiliares.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del servicio auxiliar no permite números, caracteres especiales ni signos de puntuación.");
                serviciosAuxiliares.Nombre = System.Text.RegularExpressions.Regex.Replace(serviciosAuxiliares.Nombre, @"\s+", " ");
                serviciosAuxiliares.Nombre = serviciosAuxiliares.Nombre.TrimStart();
            }
        }

        #endregion

        #region Tarifas

        public void ValidarTarifa(ModelStateDictionary ModelState, Tarifas tarifas)
        {
            if (!String.IsNullOrEmpty(tarifas.Nombre))
            {
                bool error = db.Tarifas.Where(c => c.ID_Tarifa != tarifas.ID_Tarifa && c.Nombre == tarifas.Nombre && c.ID_Afiliado == tarifas.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la tarifa ya se encuentra registrada para el afiliado indicado.");
                if (!regexFrase.IsMatch(tarifas.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la tarifa no permite números, caracteres especiales ni signos de puntuación.");
                tarifas.Nombre = System.Text.RegularExpressions.Regex.Replace(tarifas.Nombre, @"\s+", " ");
                tarifas.Nombre = tarifas.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(tarifas.Descripcion))
            {
                if (tarifas.Descripcion.Contains("..") || tarifas.Descripcion.Contains(",,"))
                    ModelState.AddModelError("Descripcion", "La descripción de la tarifa no permite signos de puntuación repetidos.");
                if (!regexFraseNumerosComaPunto.IsMatch(tarifas.Descripcion))
                    ModelState.AddModelError("Descripcion", "La descripción de la tarifa no permite caracteres especiales, acepta solamente '.' y ','");
            }
            if(tarifas.Tarifa_Minima == 0)
            {
                if(!ValidateMaxDecimal(ModelState, "Tarifa_Minima"))
                    ModelState.AddModelError("Tarifa_Minima", "El valor del campo Tarifa Mínima no es válido.");
            }
            if (tarifas.Precio_Base == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Precio_Base"))
                    ModelState.AddModelError("Precio_Base", "El valor del campo Precio Base no es válido.");
            }
            if (tarifas.Precio_Km == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Precio_Km"))
                    ModelState.AddModelError("Precio_Km", "El valor del campo Precio por Kilómetro no es válido.");
            }
            if (tarifas.Precio_Min == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Precio_Min"))
                {
                    ModelState.AddModelError("Precio_Min", "El valor del campo Precio por Minuto no es válido.");
                }
            }
        }

        private static bool ValidateMaxDecimal(ModelStateDictionary ModelState, string field)
        {
            bool valid = false;
            ModelState state;
            if (ModelState.TryGetValue(field, out state))
            {
                decimal value;
                bool isDecimal = decimal.TryParse(state.Value.AttemptedValue.ToString(), out value);
                if (isDecimal)
                {
                    valid = value <= decimal.Parse("79228162514264337593543950335");
                }
            }
            if (!valid)
                ModelState.Remove(field);
            return valid;
        }

        private static bool ValidateMaxInt(ModelStateDictionary ModelState, string field)
        {
            bool valid = false;
            ModelState state;
            if (ModelState.TryGetValue(field, out state))
            {
                int value;
                bool isDecimal = int.TryParse(state.Value.AttemptedValue.ToString(), out value);
                if (isDecimal)
                {
                    valid = value <= int.MaxValue;
                }
            }
            if (!valid)
                ModelState.Remove(field);
            return valid;
        }

        #endregion

        #region TarjetasCredito

        public void ValidarTarjetaCredito(ModelStateDictionary ModelState, TarjetasCredito tarjetasCredito)
        {
            if (!String.IsNullOrEmpty(tarjetasCredito.Nombre))
            {
                bool error = db.TarjetasCredito.Where(c => c.ID_TarjetaCredito != tarjetasCredito.ID_TarjetaCredito && c.Nombre == tarjetasCredito.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre de la tarjeta de crédito ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tarjetasCredito.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre de la tarjeta de crédito no permite números, caracteres especiales ni signos de puntuación.");
                tarjetasCredito.Nombre = System.Text.RegularExpressions.Regex.Replace(tarjetasCredito.Nombre, @"\s+", " ");
                tarjetasCredito.Nombre = tarjetasCredito.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposAvisos

        public void ValidarTipoAviso(ModelStateDictionary ModelState, TiposAvisos tiposAvisos)
        {
            if (!String.IsNullOrEmpty(tiposAvisos.Nombre))
            {
                bool error = db.TiposAvisos.Where(c => c.ID_TipoAviso != tiposAvisos.ID_TipoAviso && c.Nombre == tiposAvisos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de aviso ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposAvisos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de aviso no permite números, caracteres especiales ni signos de puntuación.");
                tiposAvisos.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposAvisos.Nombre, @"\s+", " ");
                tiposAvisos.Nombre = tiposAvisos.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposPagos

        public void ValidarTipoPago(ModelStateDictionary ModelState, TiposPagos tiposPagos)
        {
            if (!String.IsNullOrEmpty(tiposPagos.Nombre))
            {
                bool error = db.TiposPagos.Where(c => c.ID_TipoPago != tiposPagos.ID_TipoPago && c.Nombre == tiposPagos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de pago ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposPagos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de pago no permite números, caracteres especiales ni signos de puntuación.");
                tiposPagos.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposPagos.Nombre, @"\s+", " ");
                tiposPagos.Nombre = tiposPagos.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposPrioridades

        public void ValidarTipoPrioridad(ModelStateDictionary ModelState, TiposPrioridades tiposPrioridades)
        {
            if (!String.IsNullOrEmpty(tiposPrioridades.Nombre))
            {
                bool error = db.TiposPrioridades.Where(c => c.ID_TipoPrioridad != tiposPrioridades.ID_TipoPrioridad && c.Nombre == tiposPrioridades.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de prioridad ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposPrioridades.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de prioridad no permite números, caracteres especiales ni signos de puntuación.");
                tiposPrioridades.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposPrioridades.Nombre, @"\s+", " ");
                tiposPrioridades.Nombre = tiposPrioridades.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposPuntosInteres

        public void ValidarTipoPuntoInteres(ModelStateDictionary ModelState, TiposPuntosInteres tiposPuntosInteres)
        {
            if (!String.IsNullOrEmpty(tiposPuntosInteres.Nombre))
            {
                bool error = db.TiposPuntosInteres.Where(c => c.ID_TipoPuntoInteres != tiposPuntosInteres.ID_TipoPuntoInteres && c.Nombre == tiposPuntosInteres.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de punto de interés ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposPuntosInteres.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de punto de interés no permite números, caracteres especiales ni signos de puntuación.");
                tiposPuntosInteres.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposPuntosInteres.Nombre, @"\s+", " ");
                tiposPuntosInteres.Nombre = tiposPuntosInteres.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposRechazos

        public void ValidarTipoRechazo(ModelStateDictionary ModelState, TiposRechazos tiposRechazos)
        {
            if (!String.IsNullOrEmpty(tiposRechazos.Nombre))
            {
                bool error = db.TiposRechazos.Where(c => c.ID_TipoRechazo != tiposRechazos.ID_TipoRechazo && c.Nombre == tiposRechazos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de rechazo ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposRechazos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de rechazo no permite números, caracteres especiales ni signos de puntuación.");
                tiposRechazos.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposRechazos.Nombre, @"\s+", " ");
                tiposRechazos.Nombre = tiposRechazos.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposSanciones

        public void ValidarTipoSancion(ModelStateDictionary ModelState, TiposSanciones tiposSanciones)
        {
            if (!String.IsNullOrEmpty(tiposSanciones.Nombre))
            {
                bool error = db.TiposSanciones.Where(c => c.ID_TipoSancion != tiposSanciones.ID_TipoSancion && c.Nombre == tiposSanciones.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de sanción ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposSanciones.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de sanción no permite números, caracteres especiales ni signos de puntuación.");
                tiposSanciones.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposSanciones.Nombre, @"\s+", " ");
                tiposSanciones.Nombre = tiposSanciones.Nombre.TrimStart();
            }
            if (tiposSanciones.Horas_Penalizacion == 0)
            {
                if (!ValidateMaxInt(ModelState, "Horas_Penalizacion"))
                    ModelState.AddModelError("Horas_Penalizacion", "El valor del campo Horas de Penalización no es válido.");
            }
        }

        #endregion

        #region TiposServicios

        public void ValidarTipoServicio(ModelStateDictionary ModelState, TiposServicios tiposServicios)
        {
            if (!String.IsNullOrEmpty(tiposServicios.Nombre))
            {
                bool error = db.TiposServicios.Where(c => c.ID_TipoServicio != tiposServicios.ID_TipoServicio && c.Nombre == tiposServicios.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de servicio ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposServicios.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de servicio no permite números, caracteres especiales ni signos de puntuación.");
                tiposServicios.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposServicios.Nombre, @"\s+", " ");
                tiposServicios.Nombre = tiposServicios.Nombre.TrimStart();
            }
        }

        #endregion

        #region TiposVehiculos

        public void ValidarTipoVehiculo(ModelStateDictionary ModelState, TiposVehiculos tiposVehiculos)
        {
            if (!String.IsNullOrEmpty(tiposVehiculos.Nombre))
            {
                bool error = db.TiposVehiculos.Where(c => c.ID_TipoVehiculo != tiposVehiculos.ID_TipoVehiculo && c.Nombre == tiposVehiculos.Nombre).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del tipo de vehículo ya se encuentra registrado.");
                if (!regexFrase.IsMatch(tiposVehiculos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del tipo de vehículo no permite números, caracteres especiales ni signos de puntuación.");
                tiposVehiculos.Nombre = System.Text.RegularExpressions.Regex.Replace(tiposVehiculos.Nombre, @"\s+", " ");
                tiposVehiculos.Nombre = tiposVehiculos.Nombre.TrimStart();
            }
            if (!String.IsNullOrEmpty(tiposVehiculos.Imagen))
            {
                if (!ImagenEsValida(tiposVehiculos.Imagen))
                {
                    ModelState.AddModelError("Imagen", "La extensión de la Imagen debe ser .jpg, .jpeg o .png.");
                }
                if (!PesoImagenEsValido(tiposVehiculos.Peso_Imagen))
                {
                    ModelState.AddModelError("Imagen", "El peso de la Imagen no es válido.");
                }
            }
            else
            {
                ModelState.AddModelError("Imagen", "El campo Imagen es requerido.");
            }
        }

        #endregion

        #region Turnos

        public void ValidarTurno(ModelStateDictionary ModelState, Turnos turnos)
        {
            if (!String.IsNullOrEmpty(turnos.Nombre))
            {
                bool error = db.Turnos.Where(c => c.ID_Turno != turnos.ID_Turno && c.Nombre == turnos.Nombre && c.ID_Afiliado == turnos.ID_Afiliado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del turno ya se encuentra registrado para el afiliado indicado.");
                if (!regexFrase.IsMatch(turnos.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del turno no permite números, caracteres especiales ni signos de puntuación.");
                turnos.Nombre = System.Text.RegularExpressions.Regex.Replace(turnos.Nombre, @"\s+", " ");
                turnos.Nombre = turnos.Nombre.TrimStart();
            }
        }

        #endregion

        #region UsuariosAbonados

        public void ValidarUsuarioAbonado(ModelStateDictionary ModelState, UsuariosAbonados usuariosAbonados)
        {
            if (!String.IsNullOrEmpty(usuariosAbonados.Nombre))
            {
                bool error = db.UsuariosAbonados.Where(c => c.Nombre != usuariosAbonados.Nombre && c.ID_ClienteAbonado == usuariosAbonados.ID_ClienteAbonado).Count() > 0;
                if (error)
                    ModelState.AddModelError("Nombre", "El nombre del usuario abonado ya se encuentra registrado para el cliente abonado indicado.");
                if (!regexFrase.IsMatch(usuariosAbonados.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre del usuario abonado no permite números, caracteres especiales ni signos de puntuación.");
                usuariosAbonados.Nombre = System.Text.RegularExpressions.Regex.Replace(usuariosAbonados.Nombre, @"\s+", " ");
                usuariosAbonados.Nombre = usuariosAbonados.Nombre.TrimStart();
            }
            if (usuariosAbonados.Limite_Credito == 0)
            {
                if (!ValidateMaxDecimal(ModelState, "Limite_Credito."))
                    ModelState.AddModelError("Limite_Credito.", "El valor del campo Límite de Crédito no es válido.");
            }
        }

        #endregion

        #region Vehiculos

        public void ValidarVehiculo(ModelStateDictionary ModelState, Vehiculos vehiculos)
        {
            bool error;
            if (!String.IsNullOrEmpty(vehiculos.Matricula))
            {
                error = db.Vehiculos.Where(c => c.ID_Vehiculo != vehiculos.ID_Vehiculo && c.Matricula == vehiculos.Matricula).Count() > 0;
                if (error)
                    ModelState.AddModelError("Matricula", "La matrícula del vehículo ya se encuentra registrada.");
                if (!regexPalabraNumeros.IsMatch(vehiculos.Matricula))
                    ModelState.AddModelError("Matricula", "La matrícula del vehículo no permite espacios ni caracteres especiales.");
            }
            if (!String.IsNullOrEmpty(vehiculos.NoLicencia))
            {
                error = db.Vehiculos.Where(c => c.ID_Vehiculo != vehiculos.ID_Vehiculo && c.NoLicencia == vehiculos.NoLicencia).Count() > 0;
                if (error)
                    ModelState.AddModelError("NoLicencia", "El número de licencia del vehículo ya se encuentra registrado.");
                if (!regexPalabraNumeros.IsMatch(vehiculos.NoLicencia))
                    ModelState.AddModelError("NoLicencia", "El número de licencia del vehículo no permite espacios ni caracteres especiales.");
            }
            if (!String.IsNullOrEmpty(vehiculos.NoTarjeton))
                if (!regexNumeros.IsMatch(vehiculos.NoTarjeton))
                    ModelState.AddModelError("NoTarjeton", "El número de tarjetón del vehículo sólo permite números.");
            if (!String.IsNullOrEmpty(vehiculos.NoTransporte))
                if (!regexNumeros.IsMatch(vehiculos.NoTransporte))
                    ModelState.AddModelError("NoTransporte", "El número de transporte ejecutivo del vehículo sólo permite números.");
            if (!String.IsNullOrEmpty(vehiculos.NoPermiso))
                if (!regexPalabraNumeros.IsMatch(vehiculos.NoPermiso))
                    ModelState.AddModelError("NoPermiso", "El número de permiso del vehículo no permite espacios ni caracteres especiales.");
            if (!String.IsNullOrEmpty(vehiculos.Version))
                if (!regexFraseNumeros.IsMatch(vehiculos.Version))
                    ModelState.AddModelError("Version", "La versión del vehículo no permite caracteres especiales.");
            if (!String.IsNullOrEmpty(vehiculos.Poliza))
            {
                error = db.Vehiculos.Where(c => c.ID_Vehiculo != vehiculos.ID_Vehiculo && c.Poliza == vehiculos.Poliza).Count() > 0;
                if (error)
                    ModelState.AddModelError("Poliza", "La póliza del vehículo ya se encuentra registrada.");
                if (!regexPalabraNumeros.IsMatch(vehiculos.Poliza))
                    ModelState.AddModelError("Poliza", "La póliza del vehículo no permite espacios ni caracteres especiales.");
            }
            if (!String.IsNullOrEmpty(vehiculos.Serie))
            {
                error = db.Vehiculos.Where(c => c.ID_Vehiculo != vehiculos.ID_Vehiculo && c.Serie == vehiculos.Serie).Count() > 0;
                if (error)
                    ModelState.AddModelError("Serie", "La serie del vehículo ya se encuentra registrada.");
                if (!regexPalabraNumeros.IsMatch(vehiculos.Serie))
                    ModelState.AddModelError("Serie", "La serie del vehículo no permite espacios ni caracteres especiales.");
            }
            if (vehiculos.Fecha_Alta != null && vehiculos.Fecha_Baja != null)
            {
                error = vehiculos.Fecha_Alta >= vehiculos.Fecha_Baja;
                if (error)
                    ModelState.AddModelError("Fecha_Baja", "La fecha de baja debe ser mayor a la fecha de alta.");
            }
            if (vehiculos.KmInicial > 0 && vehiculos.KmFinal > 0)
            {
                error = vehiculos.KmInicial >= vehiculos.KmFinal;
                if (error)
                    ModelState.AddModelError("KmFinal", "El kilometraje final debe ser mayor al kilometraje inicial.");
            }
            if (vehiculos.KmInicial == 0)
            {
                if (!ValidateMaxInt(ModelState, "KmInicial"))
                    ModelState.AddModelError("KmInicial", "El valor del campo Kilometraje Inicial no es válido.");
            }
            if (vehiculos.KmFinal == 0)
            {
                if (!ValidateMaxInt(ModelState, "KmFinal"))
                    ModelState.AddModelError("KmFinal", "El valor del campo Kilometraje Final no es válido.");
            }
            if (!String.IsNullOrEmpty(vehiculos.Telefono_Seguro) && !ModelState.IsValidField("Telefono_Seguro"))
            {
                if (!(vehiculos.Telefono_Seguro.StartsWith("01800") && vehiculos.Telefono_Seguro.Count() == 12))
                {
                    ModelState.AddModelError("Telefono_Seguro", "El formato del campo Teléfono Seguro no es válido.");
                }
            }
            if (vehiculos.Vigencia_Permiso > 0)
            {
                error = DateTime.Now.Year + 10 < vehiculos.Vigencia_Permiso;
                if (error)
                    ModelState.AddModelError("Vigencia_Permiso", "El valor del campo Vigencia de Permiso no es congruente.");
                if(vehiculos.Vigencia_Permiso < DateTime.Now.Year)
                    ModelState.AddModelError("Vigencia_Permiso", "El valor del campo Vigencia de Permiso no es válido. La Vigencia debe ser mayor o igual al año en curso.");
            }
            else if (vehiculos.Vigencia_Permiso == 0)
            {
                if (!ValidateMaxInt(ModelState, "Vigencia_Permiso"))
                    ModelState.AddModelError("Vigencia_Permiso", "El valor del campo Vigencia de Permiso no es válido.");
            }
            if (vehiculos.RevistaMecanica == 0)
            {
                if (!ValidateMaxInt(ModelState, "RevistaMecanica"))
                    ModelState.AddModelError("RevistaMecanica", "El valor del campo Revista Mecánica no es válido.");
            }
        }

        #endregion

    }
}