<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.partes oParte = new MIDAS.Models.partes();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }

            if (user.perfil == 2)
                permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
            else
            {
                permisos.idusuario = user.idUsuario;
                permisos.idcentro = centroseleccionado.id;
                permisos.permiso = true;
            }

            Session["ModuloAccionMejora"] = 16;
            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }

            oParte = (MIDAS.Models.partes)ViewData["parte"];

            if (oParte != null)
            {
                Session["idParte"] = oParte.id;
                txtCodigo.Text = oParte.idcomunicacion;
                txtEmpresa.Text = oParte.empresa;
                txtInstalacion.Text = oParte.instalacion;
                txtTrabajo.Text = oParte.trabajo;
                txtDetalle.Text = oParte.detalle;
                txtAccionesCorrectoras.Text = oParte.accionescorrectoras;
                txtAccionesPrevistas.Text = oParte.accionesprevistas;
                txtCumplimentadoPor.Text = oParte.cumplimentadopor;
                if (oParte.cumplimentadofecha != null)
                    txtCumplimentadoFecha.Text = oParte.cumplimentadofecha.ToString().Replace(" 0:00:00", "");
                txtEntregadoPor.Text = oParte.entregadopor;
                if (oParte.entregadofecha != null)
                    txtEntregadoFecha.Text = oParte.entregadofecha.ToString().Replace(" 0:00:00", "");
                txtRecibidoPor.Text = oParte.recibidounidadorg;
                if (oParte.recibidofecha != null)
                    txtRecibidoFecha.Text = oParte.recibidofecha.ToString().Replace(" 0:00:00", "");
                txtResueltoPor.Text = oParte.resueltopor;
                if (oParte.resueltofecha != null)
                    txtResueltoFecha.Text = oParte.resueltofecha.ToString().Replace(" 0:00:00", "");
                txtObservaciones.Text = oParte.observaciones;
                txtAsunto.Text = oParte.asunto;

                txtEmpresa.ReadOnly = true;
                txtInstalacion.ReadOnly = true;
                txtTrabajo.ReadOnly = true;
                txtDetalle.ReadOnly = true;
                txtAccionesCorrectoras.ReadOnly = true;
                txtCumplimentadoPor.ReadOnly = true;
                txtCumplimentadoFecha.ReadOnly = true;
                txtEntregadoPor.ReadOnly = true;
                txtEntregadoFecha.ReadOnly = true;

                //if (centroseleccionado.tipo != 4 && oParte.idcentral == centroseleccionado.id)
                //    desactivarCampos();
                //if (centroseleccionado.tipo == 4 && oParte.idcentral != centroseleccionado.id)
                //    desactivarCampos();                  

                
            }
            else
            {
                divSegundaParte.Visible = false;
            }
        }
        

        //if (user.perfil != 1 && user.perfil != 3)
        //{
        //    desactivarCampos();
        //}                                    

        if (Session["EdicionParteMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionParteMensaje"].ToString() + "' });", true);
            Session["EdicionParteMensaje"] = null;
        }
        if (Session["EdicionParteError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionParteError"].ToString() + "' });", true);
            Session["EdicionParteError"] = null;
        }   
    }

    //public void desactivarCampos()
    //{
    //    txtCodigo.Enabled = false;
    //    txtEmpresa.ReadOnly = true;
    //    txtInstalacion.ReadOnly = true;
    //    txtTrabajo.ReadOnly = true;
    //    txtDetalle.ReadOnly = true;
    //    txtAccionesCorrectoras.ReadOnly = true;
    //    txtAccionesPrevistas.ReadOnly = true;
    //    txtCumplimentadoPor.ReadOnly = true;
    //    txtCumplimentadoFecha.ReadOnly = true;
    //    txtEntregadoPor.ReadOnly = true;
    //    txtEntregadoFecha.ReadOnly = true;
    //    txtRecibidoPor.ReadOnly = true;
    //    txtRecibidoFecha.ReadOnly = true;
    //    txtResueltoPor.ReadOnly = true;
    //    txtResueltoFecha.ReadOnly = true;
    //    txtObservaciones.ReadOnly = true;
    //    consulta = 1;
    //}
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Parte </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarParte")
                    $("#hdFormularioEjecutado").val("GuardarParte");
				else if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");											   
                else $("#hdFormularioEjecutado").val("Recarga");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });                       

        });      
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>
                Detalle del parte<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">

    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <%--Datos generales --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-pencil"></i>Datos Generales</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Nº Comunicación</label>
                            <asp:TextBox ID="txtCodigo" Enabled="false" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 40%">
                            <label>
                                Asunto</label>
                            <asp:TextBox ID="txtAsunto" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td colspan="2" style="width: 70%">
                            <label>
                                Empresa</label>
                            <asp:TextBox ID="txtEmpresa" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 50%">
                            <label>
                                Instalación/equipo</label>
                            <asp:TextBox ID="txtInstalacion" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 50%">
                            <label>
                                Trabajo</label>
                            <asp:TextBox ID="txtTrabajo" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 100%">
                            <label>
                                Detalle del riesgo observado</label>
                            <asp:TextBox ID="txtDetalle" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>                        
                    </tr>
                    <tr>
                        <td class="form-group" style="width: 100%">
                        <br />
                            <label>
                                Acciones correctoras propuestas</label>
                            <asp:TextBox ID="txtAccionesCorrectoras" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>                    
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 70%">
                            <label>
                                Cumplimentado por</label>
                            <asp:TextBox ID="txtCumplimentadoPor" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                            <label>
                                Fecha</label>
                            <asp:TextBox ID="txtCumplimentadoFecha" Width="95%" runat="server"  class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-group" style="width: 70%">
                        <br />
                            <label>
                                Entregado a</label>
                            <asp:TextBox ID="txtEntregadoPor" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                        <br />
                            <label>
                               Fecha</label>
                            <asp:TextBox ID="txtEntregadoFecha" Width="95%" runat="server"  class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <br />
                <div runat="server" id="divSegundaParte">
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 100%">
                        <br />
                            <label>
                                Acciones previstas</label>
                            <asp:TextBox ID="txtAccionesPrevistas" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                            <br />
                        </td>
                    </tr>                    
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 70%">
                            <label>
                                Recibido por Unidad Organizativa</label>
                            <asp:TextBox ID="txtRecibidoPor" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                            <label>
                               Fecha</label>
                            <asp:TextBox ID="txtRecibidoFecha" Width="95%" runat="server"  class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 70%">
                            <label>
                                Resuelto por</label>
                            <asp:TextBox ID="txtResueltoPor" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                            <label>
                               Fecha</label>
                            <asp:TextBox ID="txtResueltoFecha" Width="95%" runat="server"  class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table width="100%">
                <tr>
                        <td class="form-group" style="width: 100%">
                        <br />
                            <label>
                                Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 70%">
                            <label>
                                Motivo de desestimación de la propuesta</label>
                            <asp:TextBox ID="txtMotivoDesestimacion" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                            <label>
                               Fecha</label>
                            <asp:TextBox ID="txtFechaDesestimacion" Width="95%" runat="server"  class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                </div>
            </div>
        </div>
    </div>

    <% if (oParte != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
                if (permisos.permiso == true && oParte != null)
        {
            %>
            <table width="100%">
                <tr>
                    <td>
                        <a href="/evr/accionmejora/detalle_accion/0" title="Nueva Acción Mejora" class="btn btn-primary run-first">Nueva Acción Mejora</a>
                    </td>
                </tr>
            </table>
            <% } %>
            <asp:GridView ID="grdAccionesMejora" runat="server" Visible="false">
            </asp:GridView>
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="width:50px">
                                                                                Año      
                                                                            </th> 
                                                                            <th style="width:120px">
                                                                                Tipo      
                                                                            </th>  
                                                                            <th>
                                                                                Asunto      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha apertura
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha cierre
                                                                            </th>  
                                                                            <th  style="width:150px">
                                                                                Estado      
                                                                            </th>                  
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% }
                                                                               else
                                                                               { %>
                                                                                <th  style="width:50px">
                                                                                Consultar
                                                                            </th>
                                                                               <% } %>
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdAccionesMejora.Rows)
                                                                        { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[3].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[4].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[4].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[5].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                            <% 
                                                                            if (permisos.permiso == true)
                                                                            { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                            else
                                                                            { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar acción de mejora" onclick="if(!confirm('¿Está seguro de que desea eliminar este registro de emergencia?')) return false;" href="/evr/accionmejora/eliminar_accionmejora/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            
                                                                        </tr>
                                                                        <% } %>
                        </tbody>
                    </table>
                </div>
            </center>
            <br />
        </div>
    </div>
    <% } %>

    <div class="form-actions text-right">

		<asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />																								

        <input id="GuardarParte" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <a href="/evr/Comunicacion/gestion_comunicacion" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
