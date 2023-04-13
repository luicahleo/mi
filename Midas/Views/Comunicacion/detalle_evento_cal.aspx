<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.evento_calidad oEvento = new MIDAS.Models.evento_calidad();
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

            ListItem itemCentral = new ListItem();
            itemCentral.Value = centroseleccionado.id.ToString();
            itemCentral.Text = centroseleccionado.nombre;
            ddlCentral.Items.Insert(0, itemCentral);

            ListItem itemTecnologia = new ListItem();
            List<MIDAS.Models.tipocentral> tecno = MIDAS.Models.Datos.ObtenerTecnologiaCentral(centroseleccionado.tipo);
            ddlTecnologia.DataSource = tecno;
            ddlTecnologia.DataValueField = "id";
            ddlTecnologia.DataTextField = "nombre";
            ddlTecnologia.DataBind();
            

            if (ViewData["responsables"] != null)
            {
                ddlResponsable.DataSource = ViewData["responsables"];
                ddlResponsable.DataValueField = "idUsuario";
                ddlResponsable.DataTextField = "nombre";
                ddlResponsable.DataBind();
            }

            Session["ModuloAccionMejora"] = 14;
            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }

            oEvento = (MIDAS.Models.evento_calidad)ViewData["eventocalidad"];

            if (oEvento != null)
            {
                Session["idEventoCal"] = oEvento.id;
                txtAsunto.Text = oEvento.asunto;
                ddlTecnologia.SelectedValue = oEvento.tecnologia.ToString();
                ddlCentral.SelectedValue = oEvento.idcentral.ToString();
                txtUnidad.Text = oEvento.unidad.ToString();
                if (oEvento.fechacomienzo != null)
                    txtFechaComienzo.Text = oEvento.fechacomienzo.ToString().Replace(" 0:00:00", "");
                if (oEvento.fechafin != null)
                    txtFechaFin.Text = oEvento.fechafin.ToString().Replace(" 0:00:00", "");
                txtEvento.Text = oEvento.evento;
                txtDescripcion.Text = oEvento.descripcion;
                txtImpacto.Text = oEvento.impacto;
                txtCargo.Text = oEvento.cargo;
                ddlResponsable.SelectedValue = oEvento.persona.ToString();

                if (permisos.permiso != true || oEvento.idcentral != centroseleccionado.id)
                    desactivarCampos();
                  
            }

        }


        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                      
                                    

        if (Session["EdicionEventoCalMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEventoCalMensaje"].ToString() + "' });", true);
            Session["EdicionEventoCalMensaje"] = null;
        }
        if (Session["EdicionEventoCalError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionEventoCalError"].ToString() + "' });", true);
            Session["EdicionEventoCalError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtAsunto.Enabled = false;
        txtCompania.Enabled = false;
        txtPais.Enabled = false;
        ddlTecnologia.Enabled = false;
        ddlCentral.Enabled = false;
        txtUnidad.Enabled = false;
        txtFechaComienzo.Enabled = false;
        txtFechaFin.Enabled = false;
        txtEvento.Enabled = false;
        txtCodEvento.Enabled = false;
        txtDescripcion.Enabled = false;
        txtImpacto.Enabled = false;
        txtCargo.Enabled = false;
        ddlResponsable.Enabled = false;
        consulta = 1;
    }
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Evento Calidad</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarEventoCal")
                    $("#hdFormularioEjecutado").val("GuardarEventoCal");
                else $("#hdFormularioEjecutado").val("Recarga");

                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");
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
                Detalle del evento de calidad<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td class="form-group" colspan="2" style="width: 50%">
                            <label>
                                Asunto</label>
                            <asp:TextBox ID="txtAsunto" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>
                                Compañía</label>
                            <asp:TextBox ID="txtCompania" Width="95%" runat="server" Text="Applus" ReadOnly="true" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                País</label>
                           <asp:TextBox ID="txtPais" Width="95%" runat="server" Text="España" ReadOnly="true" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 33%">
                            <label>
                                Tecnología</label>
                            <asp:DropDownList ID="ddlTecnologia" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 33%">
                            <label>
                                Central</label>
                            <asp:DropDownList ID="ddlCentral" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 33%">
                            <label>
                                Unidad</label>
                            <asp:TextBox ID="txtUnidad" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Fecha comienzo</label>
                            <asp:TextBox ID="txtFechaComienzo" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>
                                Fecha fin</label>
                            <asp:TextBox ID="txtFechaFin" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Evento</label>
                            <asp:TextBox ID="txtEvento" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Cod.Evento</label>
                            <asp:TextBox ID="txtCodEvento"  Width="95%" runat="server" Text="Significativo/Significant" ReadOnly="true" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                                            <td colspan="4" style="padding-top:10px" class="form-group">
												<label>Descripción</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                    <tr>
                                            <td colspan="4" style="padding-top:10px" class="form-group">
												<label>Estimación del Impacto</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtImpacto" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                    <tr>
                        <td class="form-group" colspan="2" style="width: 50%; padding-top:10px">
                            <label>
                                Cargo</label>
                            <asp:TextBox ID="txtCargo" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" colspan="2" style="width: 50%; padding-top:10px">
                            <label>
                                Persona que lo detecta</label>
                            <asp:DropDownList ID="ddlResponsable" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>   

     <% if (oEvento != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
                if (permisos.permiso == true && oEvento != null)
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
        if (centroseleccionado.tipo == 4 && user.perfil == 1)
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
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
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
    <%
            if (oEvento != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <% 
                        if (consulta == 0)
                        { %>
        <input id="GuardarEventoCal" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
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
