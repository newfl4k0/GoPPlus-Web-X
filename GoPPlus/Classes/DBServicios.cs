using GoPS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace GoPS.Classes
{
    public class DBServicios
    {
        
        GoPSEntities db = new GoPSEntities();

        public DBServicios()
        {

        }

        #region Afiliados

        public async Task EliminarAfiliado(int id)
        {
            Afiliados afiliados = db.Afiliados.Find(id);

            await EliminarAfiliado(afiliados);
            db.SaveChanges();
        }

        public async Task EliminarAfiliado(Afiliados afiliados)
        {
            EliminarOperadoresPorAfiliado(afiliados);
            EliminarClientesPorAfiliado(afiliados);
            EliminarClientesAbonadosPorAfiliado(afiliados);
            EliminarClientesHabitualesPorAfiliado(afiliados);
            EliminarGeocercaPorAfiliado(afiliados);
            EliminarCuponesPorAfiliado(afiliados);
            EliminarDatosTicketsPorAfiliado(afiliados);
            EliminarEstatusAfiliadosPorAfiliado(afiliados);
            EliminarFlotasPorAfiliado(afiliados);
            EliminarPuntosInteresPorAfiliado(afiliados);
            EliminarServiciosAuxiliaresPorAfiliado(afiliados);
            EliminarTarifasPorAfiliado(afiliados);
            EliminarTiposVehiculosAfiliadosPorAfiliado(afiliados);
            EliminarTurnosPorAfiliado(afiliados);
            await EliminarUserRoleAfiliadosPorAfiliado(afiliados);

            db.Afiliados.Remove(afiliados);
            db.SaveChanges();
        }

        private void EliminarOperadoresPorAfiliado(Afiliados afiliados)
        {
            foreach (Operadores operadores in afiliados.Operadores.ToList())
                EliminarOperador(operadores);
        }

        private void EliminarClientesPorAfiliado(Afiliados afiliados)
        {
            foreach (Clientes clientes in afiliados.Clientes.ToList())
                EliminarCliente(clientes);
        }

        private void EliminarClientesAbonadosPorAfiliado(Afiliados afiliados)
        {
            foreach (ClientesAbonados clientesAbonados in afiliados.ClientesAbonados.ToList())
                EliminarClienteAbonado(clientesAbonados);
        }

        private void EliminarClientesHabitualesPorAfiliado(Afiliados afiliados)
        {
            foreach (ClientesHabituales clientesHabituales in afiliados.ClientesHabituales.ToList())
                EliminarClienteHabitual(clientesHabituales);
        }

        private void EliminarGeocercaPorAfiliado(Afiliados afiliados)
        {
            foreach (Geocerca geocerca in afiliados.Geocerca.ToList())
                EliminarGeocerca(geocerca);
        }

        private void EliminarCuponesPorAfiliado(Afiliados afiliados)
        {
            foreach (Cupones cupones in afiliados.Cupones.ToList())
                EliminarCupon(cupones);
        }

        private void EliminarDatosTicketsPorAfiliado(Afiliados afiliados)
        {
            foreach (DatosTickets datosTickets in afiliados.DatosTickets.ToList())
                EliminarDatoTicket(datosTickets);
        }

        private void EliminarEstatusAfiliadosPorAfiliado(Afiliados afiliados)
        {
            foreach (Estatus_Afiliados estatus_afiliados in afiliados.Estatus_Afiliados.ToList())
                EliminarEstatusAfiliado(estatus_afiliados);
        }

        private void EliminarFlotasPorAfiliado(Afiliados afiliados)
        {
            foreach (Flotas flotas in afiliados.Flotas.ToList())
                EliminarFlota(flotas);
        }

        private void EliminarPuntosInteresPorAfiliado(Afiliados afiliados)
        {
            foreach (PuntosInteres puntosInteres in afiliados.PuntosInteres.ToList())
                EliminarPuntoInteres(puntosInteres);
        }

        private void EliminarServiciosAuxiliaresPorAfiliado(Afiliados afiliados)
        {
            foreach (ServiciosAuxiliares serviciosAuxiliares in afiliados.ServiciosAuxiliares.ToList())
                EliminarServicioAuxiliar(serviciosAuxiliares);
        }

        private void EliminarTarifasPorAfiliado(Afiliados afiliados)
        {
            foreach (Tarifas tarifas in afiliados.Tarifas.ToList())
                EliminarTarifa(tarifas);
        }

        private void EliminarTiposVehiculosAfiliadosPorAfiliado(Afiliados afiliados)
        {
            foreach (TiposVehiculos_Afiliados tiposvehiculos_afiliados in afiliados.TiposVehiculos_Afiliados.ToList())
                EliminarTiposVehiculosAfiliado(tiposvehiculos_afiliados);
        }

        private void EliminarTurnosPorAfiliado(Afiliados afiliados)
        {
            foreach (Turnos turnos in afiliados.Turnos.ToList())
                EliminarTurno(turnos);
        }

        private async Task EliminarUserRoleAfiliadosPorAfiliado(Afiliados afiliados)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (AspNetUserRoles userRole in afiliados.AspNetUserRoles.ToList())
            {
                userRole.Afiliados.Remove(afiliados);
                db.SaveChanges();
                IdentityResult deletionResult = await userManager.RemoveFromRoleAsync(userRole.UserId, userRole.AspNetRoles.Name);
                //EliminarUsuario(userRole.AspNetUsers.Id);
            }
        }
        #endregion

        #region AspNetUsers
        public async Task EliminarUsuario(string id)
        {
            AspNetUsers usuarios = db.AspNetUsers.Find(id);

            await EliminarUsuario(usuarios);
            db.SaveChanges();
        }

        public async Task EliminarUsuario(AspNetUsers usuarios)
        {
            await EliminarUserRoleAfiliadosPorUsuario(usuarios);

            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = userManager.FindByName(usuarios.UserName);
            userManager.Delete(user);
        }

        private async Task EliminarUserRoleAfiliadosPorUsuario(AspNetUsers usuarios)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (AspNetUserRoles userRole in usuarios.AspNetUserRoles.ToList())
            {
                if (userRole.AspNetRoles.Afiliado)
                {
                    List<Afiliados> afiliadosDelete = userRole.Afiliados.ToList();
                    afiliadosDelete.ForEach(c => userRole.Afiliados.Remove(c));
                    db.SaveChanges();
                }
                IdentityResult deletionResult = await userManager.RemoveFromRoleAsync(userRole.UserId, userRole.AspNetRoles.Name);
            }
        }

        #endregion

        #region Bancos

        public void EliminarBanco(int id)
        {
            Bancos bancos = db.Bancos.Find(id);

            EliminarClientesAbonadosPorBanco(bancos);
            EliminarFlotasPorBanco(bancos);

            db.Bancos.Remove(bancos);
            db.SaveChanges();
        }

        private void EliminarClientesAbonadosPorBanco(Bancos bancos)
        {
            foreach (ClientesAbonados clientesAbonados in bancos.ClientesAbonados.ToList())
                EliminarClienteAbonado(clientesAbonados);
        }

        private void EliminarFlotasPorBanco(Bancos bancos)
        {
            foreach (Flotas flotas in bancos.Flotas.ToList())
                EliminarFlota(flotas);
        }

        #endregion

        #region Calles

        public void EliminarCalle(int id)
        {
            Calles calles = db.Calles.Find(id);

            EliminarCalle(calles);
        }

        public void EliminarCalle(Calles calles)
        {
            EliminarAfiliadosPorCalle(calles);
            EliminarClientesAbonadosPorCalle(calles);
            EliminarClientesPorCalle(calles);
            EliminarClientesHabitualesPorCalle(calles);
            EliminarConductoresPorCalle(calles);
            EliminarDireccionesAlternativasPorCalle(calles);
            EliminarFlotasPorCalle(calles);
            EliminarOperadoresPorCalle(calles);
            EliminarParticionesCallesPorCalle(calles);
            EliminarPuntosInteresPorCalle(calles);
            EliminarRutasPorCalle(calles);

            db.Calles.Remove(calles);
            db.SaveChanges();
        }

        private void EliminarAfiliadosPorCalle(Calles calles)
        {
            db.Afiliados.RemoveRange(calles.Afiliados);
            db.SaveChanges();
        }

        private void EliminarClientesAbonadosPorCalle(Calles calles)
        {
            foreach (ClientesAbonados clientesAbonados in calles.ClientesAbonados.ToList())
                EliminarClienteAbonado(clientesAbonados);
        }

        private void EliminarClientesPorCalle(Calles calles)
        {
            foreach (Clientes clientes in calles.Clientes.ToList())
                EliminarCliente(clientes);

            foreach (Clientes clientes in calles.Clientes1.ToList())
                EliminarCliente(clientes);
        }

        private void EliminarClientesHabitualesPorCalle(Calles calles)
        {
            foreach (ClientesHabituales clientesHabituales in calles.ClientesHabituales.ToList())
                EliminarClienteHabitual(clientesHabituales);
        }

        private void EliminarConductoresPorCalle(Calles calles)
        {
            foreach (Conductores conductores in calles.Conductores.ToList())
                EliminarConductor(conductores);
        }

        private void EliminarDireccionesAlternativasPorCalle(Calles calles)
        {
            db.DireccionesAlternativas.RemoveRange(calles.DireccionesAlternativas);
            db.SaveChanges();
        }

        private void EliminarFlotasPorCalle(Calles calles)
        {
            foreach (Flotas flotas in calles.Flotas.ToList())
                EliminarFlota(flotas);
        }

        private void EliminarOperadoresPorCalle(Calles calles)
        {
            foreach (Operadores operadores in calles.Operadores.ToList())
                EliminarOperador(operadores);
        }

        private void EliminarParticionesCallesPorCalle(Calles calles)
        {
            db.ParticionesCalles.RemoveRange(calles.ParticionesCalles);
            db.SaveChanges();
        }

        private void EliminarPuntosInteresPorCalle(Calles calles)
        {
            db.PuntosInteres.RemoveRange(calles.PuntosInteres);
            db.SaveChanges();
        }

        private void EliminarRutasPorCalle(Calles calles)
        {
            foreach (Rutas rutas in calles.Rutas.ToList())
                EliminarRuta(rutas);

            foreach (Rutas rutas in calles.Rutas1.ToList())
                EliminarRuta(rutas);
        }

        #endregion

        #region Clientes

        public void EliminarCliente(int id)
        {
            Clientes clientes = db.Clientes.Find(id);

            EliminarCliente(clientes);
        }

        public void EliminarCliente(Clientes clientes)
        {
            EliminarDireccionesAlternativasPorCliente(clientes);
            EliminarReservasPorCliente(clientes);
            EliminarUsuarioAppsPorCliente(clientes);

            db.Clientes.Remove(clientes);
            db.SaveChanges();
        }

        private void EliminarUsuarioAppsPorCliente(Clientes clientes)
        {
            db.Usuario_App.RemoveRange(clientes.Usuario_App);
            db.SaveChanges();
        }

        private void EliminarDireccionesAlternativasPorCliente(Clientes clientes)
        {
            db.DireccionesAlternativas.RemoveRange(clientes.DireccionesAlternativas);
            db.SaveChanges();
        }

        private void EliminarReservasPorCliente(Clientes clientes)
        {
            foreach (Reservas reservas in clientes.Reservas.ToList())
                EliminarReserva(reservas);
        }

        #endregion

        #region ClientesAbonados

        public void EliminarClienteAbonado(int id)
        {
            ClientesAbonados clientesAbonados = db.ClientesAbonados.Find(id);

            EliminarClienteAbonado(clientesAbonados);
        }

        public void EliminarClienteAbonado(ClientesAbonados clientesAbonados)
        {
            EliminarDireccionesAlternativasPorClienteAbonado(clientesAbonados);
            EliminarUsuariosAbonadosPorClienteAbonado(clientesAbonados);
            EliminarReservasPorClienteAbonado(clientesAbonados);

            db.ClientesAbonados.Remove(clientesAbonados);
            db.SaveChanges();
        }

        private void EliminarReservasPorClienteAbonado(ClientesAbonados clientesAbonados)
        {
            foreach (Reservas reservas in clientesAbonados.Reservas.ToList())
                EliminarReserva(reservas);
        }

        private void EliminarDireccionesAlternativasPorClienteAbonado(ClientesAbonados clientesAbonados)
        {
            db.DireccionesAlternativas.RemoveRange(clientesAbonados.DireccionesAlternativas);
            db.SaveChanges();
        }

        private void EliminarUsuariosAbonadosPorClienteAbonado(ClientesAbonados clientesAbonados)
        {
            foreach (UsuariosAbonados usuariosAbonados in clientesAbonados.UsuariosAbonados.ToList())
                EliminarUsuarioAbonado(usuariosAbonados);
        }

        #endregion

        #region ClientesHabituales

        public void EliminarClienteHabitual(int id)
        {
            ClientesHabituales clientesHabituales = db.ClientesHabituales.Find(id);

            EliminarClienteHabitual(clientesHabituales);
        }

        public void EliminarClienteHabitual(ClientesHabituales clientesHabituales)
        {
            EliminarDireccionesAlternativasPorClienteHabitual(clientesHabituales);
            EliminarReservasPorClienteHabitual(clientesHabituales);

            db.ClientesHabituales.Remove(clientesHabituales);
            db.SaveChanges();
        }

        private void EliminarReservasPorClienteHabitual(ClientesHabituales clientesHabituales)
        {
            foreach (Reservas reservas in clientesHabituales.Reservas.ToList())
                EliminarReserva(reservas);
        }

        private void EliminarDireccionesAlternativasPorClienteHabitual(ClientesHabituales clientesHabituales)
        {
            db.DireccionesAlternativas.RemoveRange(clientesHabituales.DireccionesAlternativas);
            db.SaveChanges();
        }

        #endregion

        #region Colonias

        public void EliminarColonia(int id)
        {
            Colonias colonias = db.Colonias.Find(id);

            EliminarCallesPorColonia(colonias);

            db.Colonias.Remove(colonias);
            db.SaveChanges();
        }

        private void EliminarCallesPorColonia(Colonias colonias)
        {
            foreach (Calles calles in colonias.Calles.ToList())
                EliminarCalle(calles);
        }

        #endregion

        #region Colores

        public void EliminarColor(int id)
        {
            Colores colores = db.Colores.Find(id);

            EliminarVehiculosPorColor(colores);

            db.Colores.Remove(colores);
            db.SaveChanges();
        }

        private void EliminarVehiculosPorColor(Colores colores)
        {
            foreach (Vehiculos vehiculos in colores.Vehiculos.ToList())
                EliminarVehiculo(vehiculos);
        }

        #endregion

        #region Conductores

        public void EliminarConductor(int id)
        {
            Conductores conductores = db.Conductores.Find(id);

            EliminarConductor(conductores);
        }

        public void EliminarConductor(Conductores conductores)
        {
            EliminarVehiculosConductoresPorConductor(conductores);
            EliminarSancionesPorConductor(conductores);
            EliminarChatsPorConductor(conductores);

            db.Conductores.Remove(conductores);
            db.SaveChanges();
        }

        private void EliminarChatsPorConductor(Conductores conductores)
        {
            db.Chat.RemoveRange(conductores.Chat);
            db.SaveChanges();
        }

        private void EliminarVehiculosConductoresPorConductor(Conductores conductores)
        {
            foreach (Vehiculos_Conductores vehiculos_conductores in conductores.Vehiculos_Conductores.ToList())
                EliminarVehiculoConductor(vehiculos_conductores);
        }

        private void EliminarSancionesPorConductor(Conductores conductores)
        {
            foreach (Sanciones sanciones in conductores.Sanciones.ToList())
                EliminarSancion(sanciones);
        }

        #endregion

        #region Conexiones

        public void EliminarConexion(int id)
        {
            Conexiones conexiones = db.Conexiones.Find(id);

            EliminarConexion(conexiones);
        }

        public void EliminarConexion(Conexiones conexiones)
        {
            EliminarUbicacionesPorConexion(conexiones);

            db.Conexiones.Remove(conexiones);
            db.SaveChanges();
        }

        private void EliminarUbicacionesPorConexion(Conexiones conexiones)
        {
            db.Ubicaciones.RemoveRange(conexiones.Ubicaciones);
            db.SaveChanges();
        }

        #endregion

        #region Cupones

        public void EliminarCupon(int id)
        {
            Cupones cupones = db.Cupones.Find(id);

            EliminarCupon(cupones);
        }

        public void EliminarCupon(Cupones cupones)
        {
            db.Cupones.Remove(cupones);
            db.SaveChanges();
        }

        #endregion

        #region DatosTickets

        public void EliminarDatoTicket(int id)
        {
            DatosTickets datosTickets = db.DatosTickets.Find(id);

            EliminarDatoTicket(datosTickets);
        }

        public void EliminarDatoTicket(DatosTickets datosTickets)
        {
            db.DatosTickets.Remove(datosTickets);
            db.SaveChanges();
        }

        #endregion

        #region Despachos

        public void EliminarDespacho(int id)
        {
            Despachos despachos = db.Despachos.Find(id);

            EliminarDespacho(despachos);
        }

        public void EliminarDespacho(Despachos despachos)
        {
            EliminarChatsPorDespacho(despachos);

            db.Despachos.Remove(despachos);
            db.SaveChanges();
        }

        private void EliminarChatsPorDespacho(Despachos despachos)
        {
            db.Chat.RemoveRange(despachos.Chat);
            db.SaveChanges();
        }

        #endregion

        #region DiasFestivos

        public void EliminarDiaFestivo(int id)
        {
            DiasFestivos diasFestivos = db.DiasFestivos.Find(id);

            db.DiasFestivos.Remove(diasFestivos);
            db.SaveChanges();
        }

        #endregion

        #region Empresas

        public async Task EliminarEmpresa(int id)
        {
            Empresas empresas = db.Empresas.Find(id);

            await EliminarAfiliadosPorEmpresa(empresas);
            EliminarEstatusPorEmpresa(empresas);
            EliminarTiposVehiculosPorEmpresa(empresas);

            db.Empresas.Remove(empresas);
            db.SaveChanges();
        }

        private async Task EliminarAfiliadosPorEmpresa(Empresas empresas)
        {
            foreach (Afiliados afiliados in empresas.Afiliados.ToList())
                await EliminarAfiliado(afiliados);
        }

        private void EliminarEstatusPorEmpresa(Empresas empresas)
        {
            foreach (Estatus estatus in empresas.Estatus.ToList())
                EliminarEstatus(estatus);
        }

        private void EliminarTiposVehiculosPorEmpresa(Empresas empresas)
        {
            foreach (TiposVehiculos tiposVehiculos in empresas.TiposVehiculos.ToList())
                EliminarTipoVehiculo(tiposVehiculos);
        }

        #endregion

        #region Estatus

        public void EliminarEstatus(int id)
        {
            Estatus estatus = db.Estatus.Find(id);

            EliminarEstatus(estatus);
        }

        public void EliminarEstatus(Estatus estatus)
        {
            EliminarEstatusAfiliadosPorEstatus(estatus);
            EliminarCambiosEstatusPorEstatus(estatus);

            db.Estatus.Remove(estatus);
            db.SaveChanges();
        }

        private void EliminarEstatusAfiliadosPorEstatus(Estatus estatus)
        {
            foreach (Estatus_Afiliados estatus_afiliados in estatus.Estatus_Afiliados.ToList())
                EliminarEstatusAfiliado(estatus_afiliados);
        }

        private void EliminarCambiosEstatusPorEstatus(Estatus estatus)
        {
            db.CambiosEstatus.RemoveRange(estatus.CambiosEstatus);
            db.SaveChanges();
            db.CambiosEstatus.RemoveRange(estatus.CambiosEstatus1);
            db.SaveChanges();
        }

        private void EliminarVehiculosConductoresPorEstatus(Estatus estatus)
        {
            foreach (Vehiculos_Conductores vehiculos_conductores in estatus.Vehiculos_Conductores.ToList())
                EliminarVehiculoConductor(vehiculos_conductores);
        }

        #endregion

        #region EstatusAfiliados

        public void EliminarEstatusAfiliado(int id)
        {
            Estatus_Afiliados estatus_afiliados = db.Estatus_Afiliados.Find(id);

            EliminarEstatusAfiliado(estatus_afiliados);
        }

        public void EliminarEstatusAfiliado(Estatus_Afiliados estatus_afiliados)
        {
            
            db.Estatus_Afiliados.Remove(estatus_afiliados);
            db.SaveChanges();
        }

   

        /*        private void EliminarSeguimientosPorEstatusAfiliado(Estatus_Afiliados estatus_afiliados)
                {
                    foreach (Seguimientos seguimientos in estatus_afiliados..ToList())
                        EliminarSeguimiento(seguimientos);
                }*/

        #endregion

        #region EstatusReservas

        public void EliminarEstatusReserva(int id)
        {
            Estatus_Reserva estatus_reserva = db.Estatus_Reserva.Find(id);

            EliminarEstatusReserva(estatus_reserva);
        }

        public void EliminarEstatusReserva(Estatus_Reserva estatus_reserva)
        {
            EliminarReservasPorEstatusReserva(estatus_reserva);

            db.Estatus_Reserva.Remove(estatus_reserva);
            db.SaveChanges();
        }

        private void EliminarReservasPorEstatusReserva(Estatus_Reserva estatus_reserva)
        {
            foreach (Reservas reservas in estatus_reserva.Reservas.ToList())
                EliminarReserva(reservas);
        }

        #endregion

        #region Flotas

        public void EliminarFlota(int id)
        {
            Flotas flotas = db.Flotas.Find(id);

            EliminarFlota(flotas);
        }

        public void EliminarFlota(Flotas flotas)
        {
            EliminarConductoresPorFlota(flotas);
            EliminarVehiculosPorFlota(flotas);

            db.Flotas.Remove(flotas);
            db.SaveChanges();
        }

        private void EliminarConductoresPorFlota(Flotas flotas)
        {
            foreach (Conductores conductores in flotas.Conductores.ToList())
                EliminarConductor(conductores);
        }

        private void EliminarVehiculosPorFlota(Flotas flotas)
        {
            foreach (Vehiculos vehiculos in flotas.Vehiculos.ToList())
                EliminarVehiculo(vehiculos.ID_Vehiculo);
        }

        #endregion

        #region FormasPago

        public void EliminarFormaPago(int id)
        {
            FormasPago formasPago = db.FormasPago.Find(id);

            EliminarClientesAbonadosPorFormaPago(formasPago);
            EliminarDespachosPorFormaPago(formasPago);
            EliminarFlotasPorFormaPago(formasPago);

            db.FormasPago.Remove(formasPago);
            db.SaveChanges();
        }

        private void EliminarClientesAbonadosPorFormaPago(FormasPago formasPago)
        {
            foreach (ClientesAbonados clientesAbonados in formasPago.ClientesAbonados.ToList())
                EliminarClienteAbonado(clientesAbonados);
        }

        private void EliminarDespachosPorFormaPago(FormasPago formasPago)
        {
            foreach (Despachos despachos in formasPago.Despachos.ToList())
                EliminarDespacho(despachos);
        }

        private void EliminarFlotasPorFormaPago(FormasPago formasPago)
        {
            foreach (Flotas flotas in formasPago.Flotas.ToList())
                EliminarFlota(flotas);
        }

        #endregion

        #region FrecuenciasPago

        public async Task EliminarFrecuenciaPago(int id)
        {
            FrecuenciasPago frecuenciasPago = db.FrecuenciasPago.Find(id);

            await EliminarAfiliadosPorFrecuenciaPago(frecuenciasPago);

            db.FrecuenciasPago.Remove(frecuenciasPago);
            db.SaveChanges();
        }

        private async Task EliminarAfiliadosPorFrecuenciaPago(FrecuenciasPago frecuenciasPago)
        {
            foreach (Afiliados afiliados in frecuenciasPago.Afiliados.ToList())
                await EliminarAfiliado(afiliados);
        }

        #endregion

        #region Geocerca

        private void EliminarGeocerca(int id)
        {
            Geocerca geocerca = db.Geocerca.Find(id);

            EliminarGeocerca(geocerca);
        }

        private void EliminarGeocerca(Geocerca geocerca)
        {
            EliminarCoordenadasGeocercaPorGeocerca(geocerca);

            db.Geocerca.Remove(geocerca);
            db.SaveChanges();
        }

        private void EliminarCoordenadasGeocercaPorGeocerca(Geocerca geocerca)
        {
            db.CoordenadasGeocerca.RemoveRange(geocerca.CoordenadasGeocerca);
            db.SaveChanges();
        }

        #endregion

        #region Marcas

        public void EliminarMarca(int id)
        {
            Marcas marcas = db.Marcas.Find(id);

            EliminarModelosPorMarca(marcas);

            db.Marcas.Remove(marcas);
            db.SaveChanges();
        }

        private void EliminarModelosPorMarca(Marcas marcas)
        {
            foreach (Modelos modelos in marcas.Modelos.ToList())
                EliminarModelo(modelos);
        }

        #endregion

        #region MensajesPredefinidos

        public void EliminarMensajePredefinido(int id)
        {
            MensajesPredefinidos mensajesPredefinidos = db.MensajesPredefinidos.Find(id);

            db.MensajesPredefinidos.Remove(mensajesPredefinidos);
            db.SaveChanges();
        }

        #endregion

        #region Modelos

        public void EliminarModelo(int id)
        {
            Modelos modelos = db.Modelos.Find(id);

            EliminarModelo(modelos);
        }

        public void EliminarModelo(Modelos modelos)
        {
            EliminarVehiculosPorModelo(modelos);

            db.Modelos.Remove(modelos);
            db.SaveChanges();
        }

        private void EliminarVehiculosPorModelo(Modelos modelos)
        {
            foreach (Vehiculos vehiculos in modelos.Vehiculos.ToList())
                EliminarVehiculo(vehiculos.ID_Vehiculo);
        }

        #endregion

        #region Operadores

        public void EliminarOperador(int id)
        {
            Operadores operadores = db.Operadores.Find(id);

            EliminarOperador(operadores);
        }

        public void EliminarOperador(Operadores operadores)
        {
            EliminarSancionesPorOperador(operadores);
            EliminarReservasPorOperador(operadores);
            EliminarChatsPorOperador(operadores);

            db.Operadores.Remove(operadores);
            db.SaveChanges();
        }

        private void EliminarSancionesPorOperador(Operadores operadores)
        {
            db.Sanciones.RemoveRange(operadores.Sanciones);
            db.SaveChanges();
            db.Sanciones.RemoveRange(operadores.Sanciones1);
            db.SaveChanges();
        }

        private void EliminarReservasPorOperador(Operadores operadores)
        {
            foreach (Reservas reservas in operadores.Reservas.ToList())
                EliminarReserva(reservas);
        }

        private void EliminarChatsPorOperador(Operadores operadores)
        {
            db.Chat.RemoveRange(operadores.Chat);
            db.SaveChanges();
        }

        #endregion

        #region ParticionesCalles

        public void EliminarParticionCalle(int id)
        {
            ParticionesCalles particionesCalles = db.ParticionesCalles.Find(id);

            EliminarParticionCalle(particionesCalles);
        }

        public void EliminarParticionCalle(ParticionesCalles particionesCalles)
        {
            db.ParticionesCalles.Remove(particionesCalles);
            db.SaveChanges();
        }

        #endregion

        #region Perfiles

       /* public Task EliminarPerfil(int id)
        {
            AspNetRoles perfiles = db.AspNetRoles.Where(k=>k.newId==id).FirstOrDefault();
            //await EliminarPerfil(perfiles);
            db.SaveChanges();
            return db.SaveChanges() ;
        }

        public async Task EliminarPerfil(AspNetRoles perfiles)
        {
            await EliminarUserRoleAfiliadosPorPerfil(perfiles);

            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            var role = roleManager.FindByName(perfiles.Name);
            roleManager.Delete(role);

            db.AspNetRoles.Remove(perfiles);
            db.SaveChanges();
        }*/

        private async Task EliminarUserRoleAfiliadosPorPerfil(AspNetRoles perfiles)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (AspNetUserRoles userRole in perfiles.AspNetUserRoles.ToList())
            {
                if (userRole.AspNetRoles.Afiliado)
                {
                    List<Afiliados> afiliadosDelete = userRole.Afiliados.ToList();
                    afiliadosDelete.ForEach(c => userRole.Afiliados.Remove(c));
                    db.SaveChanges();
                }
                IdentityResult deletionResult = await userManager.RemoveFromRoleAsync(userRole.UserId, userRole.AspNetRoles.Name);
                //EliminarUsuario(userRole.AspNetUsers.Id);
            }

            //db.AspNetUserRoles.RemoveRange(perfiles.AspNetUserRoles);
            //db.SaveChanges();
        }

        #endregion

        #region Permisos

        public void EliminarPermiso(int id)
        {
            Permissions permisos = db.Permissions.Find(id);

            db.Permissions.Remove(permisos);
            db.SaveChanges();
        }

        #endregion

        #region Positions

        public void EliminarPosition(int id)
        {
            Positions positions = db.Positions.Find(id);

            UpdateUsersPorPosition(positions);

            db.Positions.Remove(positions);
            db.SaveChanges();
        }

        private void UpdateUsersPorPosition(Positions positions)
        {
            foreach (AspNetUsers user in positions.AspNetUsers.ToList())
            {
                user.PositionID = null;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        #region PuntosInteres

        public void EliminarPuntoInteres(int id)
        {
            PuntosInteres puntosInteres = db.PuntosInteres.Find(id);

            EliminarPuntoInteres(puntosInteres);
        }

        public void EliminarPuntoInteres(PuntosInteres puntosInteres)
        {
            db.PuntosInteres.Remove(puntosInteres);
            db.SaveChanges();
        }

        #endregion

        #region RazonesLlamadas

        public void EliminarRazonLlamada(int id)
        {
            RazonesLlamadas razonesLlamadas = db.RazonesLlamadas.Find(id);

            db.RazonesLlamadas.Remove(razonesLlamadas);
            db.SaveChanges();
        }

        #endregion

        #region RazonesRechazos

        public void EliminarRazonRechazo(int id)
        {
            RazonesRechazos razonesRechazos = db.RazonesRechazos.Find(id);

            EliminarRazonRechazo(razonesRechazos);
        }

        public void EliminarRazonRechazo(RazonesRechazos razonesRechazos)
        {
            db.RazonesRechazos.Remove(razonesRechazos);
            db.SaveChanges();
        }

        private void EliminarReservasRechazadasPorRazonRechazo(RazonesRechazos razonesRechazos)
        {
            db.Reservas_Rechazadas.RemoveRange(razonesRechazos.Reservas_Rechazadas);
            db.SaveChanges();
        }

        #endregion

        #region Reservas

        public void EliminarReserva(int id)
        {
            Reservas reservas = db.Reservas.Find(id);

            EliminarReserva(reservas);
        }

        public void EliminarReserva(Reservas reservas)
        {
            EliminarDespachosPorReserva(reservas);
            EliminarReservasRechazadasPorReserva(reservas);

            db.Reservas.Remove(reservas);
            db.SaveChanges();
        }

        private void EliminarReservasRechazadasPorReserva(Reservas reservas)
        {
            db.Reservas_Rechazadas.RemoveRange(reservas.Reservas_Rechazadas);
            db.SaveChanges();
        }

        private void EliminarDespachosPorReserva(Reservas reservas)
        {
            foreach (Despachos despachos in reservas.Despachos.ToList())
                EliminarDespacho(despachos);
        }

        #endregion

        #region Rutas

        public void EliminarRuta(int id)
        {
            Rutas rutas = db.Rutas.Find(id);

            EliminarRuta(rutas);
        }

        public void EliminarRuta(Rutas rutas)
        {
            db.Rutas.Remove(rutas);
            db.SaveChanges();
        }

        #endregion

        #region Sanciones

        public void EliminarSancion(int id)
        {
            Sanciones sanciones = db.Sanciones.Find(id);

            EliminarSancion(sanciones);
        }

        public void EliminarSancion(Sanciones sanciones)
        {
            db.Sanciones.Remove(sanciones);
            db.SaveChanges();
        }

        #endregion

        #region Seguimientos

        public void EliminarSeguimiento(int id)
        {
            Seguimientos seguimientos = db.Seguimientos.Find(id);

            EliminarSeguimiento(seguimientos);
        }

        public void EliminarSeguimiento(Seguimientos seguimientos)
        {
            EliminarCambioEstatusPorSeguimiento(seguimientos);

            db.Seguimientos.Remove(seguimientos);
            db.SaveChanges();
        }

        public void EliminarCambioEstatusPorSeguimiento(Seguimientos seguimientos)
        {
            db.CambiosEstatus.RemoveRange(seguimientos.CambiosEstatus);
            db.SaveChanges();
        }

        #endregion

        #region Seguros

        public void EliminarSeguro(int id)
        {
            Seguros seguros = db.Seguros.Find(id);

            EliminarVehiculosPorSeguro(seguros);

            db.Seguros.Remove(seguros);
            db.SaveChanges();
        }

        private void EliminarVehiculosPorSeguro(Seguros seguros)
        {
            foreach (Vehiculos vehiculos in seguros.Vehiculos.ToList())
                EliminarVehiculo(vehiculos.ID_Vehiculo);
        }

        #endregion

        #region ServiciosAuxiliares

        public void EliminarServicioAuxiliar(int id)
        {
            ServiciosAuxiliares serviciosAuxiliares = db.ServiciosAuxiliares.Find(id);

            EliminarServicioAuxiliar(serviciosAuxiliares);
        }

        public void EliminarServicioAuxiliar(ServiciosAuxiliares serviciosAuxiliares)
        {
            db.ServiciosAuxiliares.Remove(serviciosAuxiliares);
            db.SaveChanges();
        }

        #endregion

        #region Tarifas

        public void EliminarTarifa(int id)
        {
            Tarifas tarifas = db.Tarifas.Find(id);

            EliminarTarifa(tarifas);
        }

        public void EliminarTarifa(Tarifas tarifas)
        {
            EliminarTiposVehiculosPorTarifa(tarifas);

            db.Tarifas.Remove(tarifas);
            db.SaveChanges();
        }

        private void EliminarTiposVehiculosPorTarifa(Tarifas tarifas)
        {
            foreach (TiposVehiculos tiposVehiculos in tarifas.TiposVehiculos.ToList())
                EliminarTipoVehiculo(tiposVehiculos);
        }

        #endregion

        #region TarjetasCredito

        public void EliminarTarjetaCredito(int id)
        {
            TarjetasCredito tarjetasCredito = db.TarjetasCredito.Find(id);

            db.TarjetasCredito.Remove(tarjetasCredito);
            db.SaveChanges();
        }

        #endregion

        #region TiposAvisos

        public void EliminarTipoAviso(int id)
        {
            TiposAvisos tiposAvisos = db.TiposAvisos.Find(id);

            EliminarClientesAbonadosPorTipoAviso(tiposAvisos);
            EliminarClientesHabitualesPorTipoAviso(tiposAvisos);

            db.TiposAvisos.Remove(tiposAvisos);
            db.SaveChanges();
        }

        private void EliminarClientesAbonadosPorTipoAviso(TiposAvisos tiposAvisos)
        {
            foreach (ClientesAbonados clientesAbonados in tiposAvisos.ClientesAbonados.ToList())
                EliminarClienteAbonado(clientesAbonados);
        }

        private void EliminarClientesHabitualesPorTipoAviso(TiposAvisos tiposAvisos)
        {
            foreach (ClientesHabituales clientesHabituales in tiposAvisos.ClientesHabituales.ToList())
                EliminarClienteHabitual(clientesHabituales);
        }

        #endregion

        #region TiposPagos

        public async Task EliminarTipoPago(int id)
        {
            TiposPagos tiposPagos = db.TiposPagos.Find(id);

            await EliminarAfiliadosPorTipoPago(tiposPagos);

            db.TiposPagos.Remove(tiposPagos);
            db.SaveChanges();
        }

        private async Task EliminarAfiliadosPorTipoPago(TiposPagos tiposPagos)
        {
            foreach (Afiliados afiliados in tiposPagos.Afiliados.ToList())
                await EliminarAfiliado(afiliados);
        }

        #endregion

        #region TiposPrioridades

        public void EliminarTipoPrioridad(int id)
        {
            TiposPrioridades tiposPrioridades = db.TiposPrioridades.Find(id);

            db.TiposPrioridades.Remove(tiposPrioridades);
            db.SaveChanges();
        }

        #endregion

        #region TiposPuntosInteres

        public void EliminarTipoPuntoInteres(int id)
        {
            TiposPuntosInteres tiposPuntosInteres = db.TiposPuntosInteres.Find(id);

            EliminarPuntosInteresPorTipoPuntoInteres(tiposPuntosInteres);

            db.TiposPuntosInteres.Remove(tiposPuntosInteres);
            db.SaveChanges();
        }

        private void EliminarPuntosInteresPorTipoPuntoInteres(TiposPuntosInteres tiposPuntosInteres)
        {
            db.PuntosInteres.RemoveRange(tiposPuntosInteres.PuntosInteres);
            db.SaveChanges();
        }

        #endregion

        #region TiposRechazos

        public void EliminarTipoRechazo(int id)
        {
            TiposRechazos tiposRechazos = db.TiposRechazos.Find(id);

            EliminarRazonesRechazosPorTipoRechazo(tiposRechazos);

            db.TiposRechazos.Remove(tiposRechazos);
            db.SaveChanges();
        }

        private void EliminarRazonesRechazosPorTipoRechazo(TiposRechazos tiposRechazos)
        {
            db.RazonesRechazos.RemoveRange(tiposRechazos.RazonesRechazos);
            db.SaveChanges();
        }

        #endregion

        #region TiposSanciones

        public void EliminarTipoSancion(int id)
        {
            TiposSanciones tiposSanciones = db.TiposSanciones.Find(id);

            EliminarSancionesPorTipoSancion(tiposSanciones);

            db.TiposSanciones.Remove(tiposSanciones);
            db.SaveChanges();
        }

        private void EliminarSancionesPorTipoSancion(TiposSanciones tiposSanciones)
        {
            db.Sanciones.RemoveRange(tiposSanciones.Sanciones);
            db.SaveChanges();
        }

        #endregion

        #region TiposServicios

        public async Task EliminarTipoServicio(int id)
        {
            TiposServicios tiposServicios = db.TiposServicios.Find(id);

            await EliminarAfiliadosPorTipoServicio(tiposServicios);

            db.TiposServicios.Remove(tiposServicios);
            db.SaveChanges();
        }

        private async Task EliminarAfiliadosPorTipoServicio(TiposServicios tiposServicios)
        {
            foreach (Afiliados afiliados in tiposServicios.Afiliados.ToList())
                await EliminarAfiliado(afiliados);
        }

        #endregion

        #region TiposVehiculos_Afiliados

        public void EliminarTiposVehiculosAfiliado(int id)
        {
            TiposVehiculos_Afiliados tiposvehiculos_afiliados = db.TiposVehiculos_Afiliados.Find(id);

            EliminarTiposVehiculosAfiliado(tiposvehiculos_afiliados);
        }

        public void EliminarTiposVehiculosAfiliado(TiposVehiculos_Afiliados tiposvehiculos_afiliados)
        {
            db.TiposVehiculos_Afiliados.Remove(tiposvehiculos_afiliados);
            db.SaveChanges();
        }

        #endregion

        #region TiposVehiculos

        public void EliminarTipoVehiculo(int id)
        {
            TiposVehiculos tiposVehiculos = db.TiposVehiculos.Find(id);

            EliminarTipoVehiculo(tiposVehiculos);
        }

        public void EliminarTipoVehiculo(TiposVehiculos tiposVehiculos)
        {
            EliminarVehiculosPorTipoVehiculo(tiposVehiculos);
            EliminarReservasPorTipoVehiculo(tiposVehiculos);

            db.TiposVehiculos.Remove(tiposVehiculos);
            db.SaveChanges();
        }
        private void EliminarTiposVehiculosAfiliadosPorEstatus(TiposVehiculos tiposvehiculos)
        {
            foreach (TiposVehiculos_Afiliados tiposvehiculos_afiliados in tiposvehiculos.TiposVehiculos_Afiliados.ToList())
                EliminarTiposVehiculosAfiliado(tiposvehiculos_afiliados);
        }

        private void EliminarVehiculosPorTipoVehiculo(TiposVehiculos tiposVehiculos)
        {
            foreach (Vehiculos vehiculos in tiposVehiculos.Vehiculos.ToList())
                EliminarVehiculo(vehiculos);
        }

        private void EliminarReservasPorTipoVehiculo(TiposVehiculos tiposVehiculos)
        {
            foreach (Reservas reservas in tiposVehiculos.Reservas.ToList())
                EliminarReserva(reservas);
        }

        #endregion

        #region Turnos

        public void EliminarTurno(int id)
        {
            Turnos turnos = db.Turnos.Find(id);

            EliminarTurno(turnos);
        }

        public void EliminarTurno(Turnos turnos)
        {
            EliminarConductoresPorTurno(turnos);
            EliminarOperadoresPorTurno(turnos);

            db.Turnos.Remove(turnos);
            db.SaveChanges();
        }

        private void EliminarConductoresPorTurno(Turnos turnos)
        {
            foreach (Conductores conductores in turnos.Conductores.ToList())
                EliminarConductor(conductores);
        }

        private void EliminarOperadoresPorTurno(Turnos turnos)
        {
            foreach (Operadores operadores in turnos.Operadores.ToList())
                EliminarOperador(operadores);
        }

        #endregion

        #region UsuariosAbonados

        public void EliminarUsuarioAbonado(int id)
        {
            UsuariosAbonados usuariosAbonados = db.UsuariosAbonados.Find(id);

            EliminarUsuarioAbonado(usuariosAbonados);
        }

        public void EliminarUsuarioAbonado(UsuariosAbonados usuariosAbonados)
        {
            EliminarRutasPorUsuarioAbonado(usuariosAbonados);

            db.UsuariosAbonados.Remove(usuariosAbonados);
            db.SaveChanges();
        }

        private void EliminarRutasPorUsuarioAbonado(UsuariosAbonados usuariosAbonados)
        {
            db.Rutas.RemoveRange(usuariosAbonados.Rutas);
            db.SaveChanges();
        }

        #endregion

        #region Vehiculos

        public void EliminarVehiculo(int id)
        {
            Vehiculos vehiculos = db.Vehiculos.Find(id);

            EliminarVehiculo(vehiculos);
        }

        public void EliminarVehiculo(Vehiculos vehiculos)
        {
            EliminarVehiculosConductoresPorVehiculo(vehiculos);

            db.Vehiculos.Remove(vehiculos);
            db.SaveChanges();
        }

        private void EliminarVehiculosConductoresPorVehiculo(Vehiculos vehiculos)
        {
            foreach (Vehiculos_Conductores vehiculos_conductores in vehiculos.Vehiculos_Conductores.ToList())
                EliminarVehiculoConductor(vehiculos_conductores);
        }

        #endregion

        #region Vehiculos_Conductores

        public void EliminarVehiculoConductor(Vehiculos_Conductores vehiculos_conductores)
        {
            EliminarSeguimientosPorVehiculoConductor(vehiculos_conductores);
            EliminarDespachosPorVehiculoConductor(vehiculos_conductores);
            EliminarConexionesPorVehiculoConductor(vehiculos_conductores);
            //EliminarReservasRechazadasPorVehiculoConductor(vehiculos_conductores);

            db.Vehiculos_Conductores.Remove(vehiculos_conductores);
            db.SaveChanges();
        }

        private void EliminarSeguimientosPorVehiculoConductor(Vehiculos_Conductores vehiculos_conductores)
        {
            foreach (Seguimientos seguimientos in vehiculos_conductores.Seguimientos.ToList())
                EliminarSeguimiento(seguimientos);
        }

        private void EliminarDespachosPorVehiculoConductor(Vehiculos_Conductores vehiculos_conductores)
        {
            foreach (Despachos despachos in vehiculos_conductores.Despachos.ToList())
                EliminarDespacho(despachos);
        }

        private void EliminarConexionesPorVehiculoConductor(Vehiculos_Conductores vehiculos_conductores)
        {
            foreach (Conexiones conexiones in vehiculos_conductores.Conexiones.ToList())
                EliminarConexion(conexiones);
        }

        //private void EliminarReservasRechazadasPorVehiculoConductor(Vehiculos_Conductores vehiculos_conductores)
        //{
        //    db.Reservas_Rechazadas.RemoveRange(vehiculos_conductores.Reservas_Rechazadas);
        //    db.SaveChanges();
        //}

        #endregion
    }
}