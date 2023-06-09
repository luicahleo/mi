<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.revision_energetica oRevision = new MIDAS.Models.revision_energetica();
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

            Session["ModuloAccionMejora"] = 7;

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

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();

                ddlAnio.Items.Insert(0, itemAnio);
            }

            oRevision = (MIDAS.Models.revision_energetica)ViewData["revision"];

            if (oRevision != null)
            {
                Session["idRevision"] = oRevision.id;
                Session["ReferenciaAccionMejora"] = oRevision.id;
                txtAnio.Text = oRevision.codigo;
                if (oRevision.fechaplanificacion != null)
                    txtFPlanificacion.Text = oRevision.fechaplanificacion.ToString().Replace(" 0:00:00", "");
                if (oRevision.fecharevision != null)
                    txtFRevision.Text = oRevision.fecharevision.ToString().Replace(" 0:00:00", "");
                ddlResponsable.SelectedValue = oRevision.responsable.ToString();
                columnaDesplegable.Visible = false;
                if (oRevision.planificacionenergetica != null)
                    lblPlanificacionInicial.Text = oRevision.planificacionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + oRevision.id + "\\Planificacion\\", "");
                if (oRevision.revisionenergetica != null)
                    lblPlanificacionEjecutada.Text = oRevision.revisionenergetica.Replace(Server.MapPath("~/RevEnergetica") + "\\" + oRevision.id + "\\Revision\\", "");
                txtConclusiones.Text = oRevision.conclusiones;
                txtPersonasInvolucradas.Text = oRevision.personasinv;
                
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

        if (Session["EdicionRevisionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionRevisionMensaje"].ToString() + "' });", true);
            Session["EdicionRevisionMensaje"] = null;
        }
        if (Session["EdicionRevisionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionRevisionError"].ToString() + "' });", true);
            Session["EdicionRevisionError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtAnio.Enabled = false;
        txtFPlanificacion.Enabled = false;
        txtFRevision.Enabled = false;
        ddlResponsable.Enabled = false;
        txtConclusiones.Enabled = false;
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Revisi�n </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarRevision")
                    $("#hdFormularioEjecutado").val("GuardarRevision");
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
                Detalle de la revisi�n<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                                A�o (*)</label>
                            <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="columnaTextbox" class="form-group" style="width: 10%">
                            <label>
                                Codigo</label>
                            <asp:TextBox ID="txtAnio" Width="92%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                             <label>
                                Responsable (*)</label>
                            <asp:DropDownList runat="server" ID="ddlResponsable" class="form-control" Width="95%"></asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 10%"><center>
                            <label>
                                Fecha Planificaci�n (*)</label>
                            <asp:TextBox ID="txtFPlanificacion" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width: 10%"><center>
                            <label>
                                Fecha Revisi�n (*)</label>
                            <asp:TextBox ID="txtFRevision" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
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
                    <td class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Planificaci�n energ�tica</label>
                            <input type="file" id="FilePInicial" name="file" style="margin-top:10px" /></center>
                        </td>      
                        <td class="form-group" style="width: 50%"><center>
                        <label>
                                Revisi�n energ�tica</label>
                                <input type="file" id="filePEjecutada" name="file" style="margin-top:10px" /></center>
                        </td>   
                    </tr>
                    <tr style="padding-top:20px">
                        <td>
                            <% if (oRevision != null && oRevision.planificacionenergetica != null)
                               { %>
                               <center>
                            <a title="Ver Planificaci�n" href="/evr/revenergetica/ObtenerPlanificacion/<%=oRevision.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblPlanificacionInicial" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                        <td>
                        <% if (oRevision != null && oRevision.revisionenergetica != null)
                           { %>
                           <center>
                            <a title="Ver Revisi�n" href="/evr/revenergetica/ObtenerRevision/<%=oRevision.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblPlanificacionEjecutada" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td>
												<label>Conclusiones</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtConclusiones" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <% if (oRevision != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if ((user.perfil == 1 || user.perfil == 3) && oRevision != null)
        {
            %>
            <table width="100%">
                <tr>
                    <td>
                        <a href="/evr/accionmejora/detalle_accion/0" title="Nueva Acci�n Mejora" class="btn btn-primary run-first">Nueva Acci�n Mejora</a>
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
                                                                                A�o      
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
                                                                                <a title="Eliminar acci�n de mejora" onclick="if(!confirm('�Est� seguro de que desea eliminar este registro de emergencia?')) return false;" href="/evr/accionmejora/eliminar_accionmejora/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
            if (oRevision != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (consulta == 0) 
            {
        %>
        <input id="GuardarRevision" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/revenergetica/revisiones" title="Volver" class="btn btn-primary run-first">Volver</a>
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
