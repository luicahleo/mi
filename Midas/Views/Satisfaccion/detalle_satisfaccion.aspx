<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.satisfaccion oSatisfaccion = new MIDAS.Models.satisfaccion();
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

            Session["ModuloAccionMejora"] = 6;

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

            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }

            if (ViewData["responsables"] != null)
            {
                ddlResponsable.DataSource = ViewData["responsables"];
                ddlResponsable.DataValueField = "idUsuario";
                ddlResponsable.DataTextField = "nombre";
                ddlResponsable.DataBind();
            }

            if (ViewData["stakeholders"] != null)
            {
                ddlStakeholder.DataSource = ViewData["stakeholders"];
                ddlStakeholder.DataValueField = "id";
                ddlStakeholder.DataTextField = "denominacionn3";
                ddlStakeholder.DataBind();
            }

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();

                ddlAnio.Items.Insert(0, itemAnio);
            }

            oSatisfaccion = (MIDAS.Models.satisfaccion)ViewData["detallesatisfaccion"];

            if (oSatisfaccion != null)
            {
                columnaDesplegable.Visible = false;
                Session["idSatisfaccion"] = oSatisfaccion.id;
                Session["ReferenciaAccionMejora"] = oSatisfaccion.id;
                txtAnio.Text = oSatisfaccion.codigo;
                if (oSatisfaccion.fecharealizacion != null)
                    txtFRealizacion.Text = oSatisfaccion.fecharealizacion.ToString().Replace(" 0:00:00", "");
                ddlResponsable.SelectedValue = oSatisfaccion.responsable.ToString();
                ddlStakeholder.SelectedValue = oSatisfaccion.stakeholder.ToString();
                txtConclusiones.Text = oSatisfaccion.conclusiones;
                txtPersonasInvolucradas.Text = oSatisfaccion.personasinv;
                if (oSatisfaccion.informe != null)
                    lblPlanificacionInicial.Text = oSatisfaccion.informe.Replace(Server.MapPath("~/Satisfaccion") + "\\" + oSatisfaccion.id + "\\Informe\\", "");
            }
            else
            {
                columnaTextbox.Visible = false;
            }
        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }

        if (Session["EdicionSatisfaccionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionSatisfaccionMensaje"].ToString() + "' });", true);
            Session["EdicionSatisfaccionMensaje"] = null;
        }
        if (Session["EdicionSatisfaccionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionSatisfaccionError"].ToString() + "' });", true);
            Session["EdicionSatisfaccionError"] = null;
        }
    }

    public void desactivarCampos()
    {
        txtAnio.Enabled = false;
        txtFRealizacion.Enabled = false;
        ddlResponsable.Enabled = false;
        ddlStakeholder.Enabled = false;
        txtConclusiones.Enabled = false;
        txtPersonasInvolucradas.Enabled = false;
        consulta = 1;
    }

</script>
<asp:Content ID="headEditarSatisfaccion" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Satisfacción </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarSatisfaccion")
                    $("#hdFormularioEjecutado").val("GuardarSatisfaccion");
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
            <h3>Detalle de satisfacción<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                            <td runat="server" id="columnaDesplegable" class="form-group" style="width: 10%">
                                <label>
                                    Año (*)</label>
                                <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                                </asp:DropDownList>
                            </td>
                            <td runat="server" id="columnaTextbox" class="form-group" style="width: 10%">
                                <label>
                                    Codigo</label>
                                <asp:TextBox ID="txtAnio" Width="92%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <label>
                                    Parte interesada (*)</label>
                                <asp:DropDownList runat="server" ID="ddlStakeholder" class="form-control" Width="95%"></asp:DropDownList>
                            </td>
                            <td style="width: 10%">
                                <label>
                                    Responsable (*)</label>
                                <asp:DropDownList runat="server" ID="ddlResponsable" class="form-control" Width="95%"></asp:DropDownList>
                            </td>
                            <td class="form-group" style="width: 10%">
                                <center>
                            <label>
                                Fecha Realización (*)</label>
                            <asp:TextBox ID="txtFRealizacion" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="width: 100%; padding-top: 15px">
                                <label>
                                    Personas involucradas</label>
                                <asp:TextBox ID="txtPersonasInvolucradas" Rows="3" TextMode="MultiLine" Width="100%" runat="server"
                                    class="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table width="100%">
                        <tr>
                            <td class="form-group" style="width: 50%; padding-bottom: 20px">
                                <center>
                            <label>
                                Informe</label>
                            <input type="file" id="FilePInicial" name="file" style="margin-top:10px" /></center>
                            </td>
                        </tr>
                        <tr style="padding-top: 20px">
                            <td>
                                <% if (oSatisfaccion != null && oSatisfaccion.informe != null)
                                    { %>
                                <center>
                            <a title="Ver Informe" href="/evr/satisfaccion/ObtenerInforme/<%=oSatisfaccion.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblPlanificacionInicial" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                                <% } %>
                            </td>
                        </tr>
                    </table>

                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                <label>Conclusiones (*)</label>
                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtConclusiones" runat="server" class="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <% if (oSatisfaccion != null && oSatisfaccion.id != 0)
            { %>
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
            </div>
            <div class="panel-body">
                <%
                    if (permisos.permiso == true && oSatisfaccion != null)
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
                if (oSatisfaccion != null)
                {
            %>
            <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
            <%} %>
            <%
                if (consulta == 0)
                {
            %>
            <input id="GuardarSatisfaccion" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
            <% } %>
            <a href="/evr/satisfaccion/satisfaccion" title="Volver" class="btn btn-primary run-first">Volver</a>
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
