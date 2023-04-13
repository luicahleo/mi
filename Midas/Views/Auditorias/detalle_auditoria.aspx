<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.auditorias oAuditoria = new MIDAS.Models.auditorias();
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

            Session["ModuloAccionMejora"] = 2;

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

            if (ViewData["centros"] != null)
            {
                ddlCentral.DataSource = ViewData["centros"];
                ddlCentral.DataValueField = "id";
                ddlCentral.DataTextField = "nombre";
                ddlCentral.DataBind();
            }

            

            oAuditoria = (MIDAS.Models.auditorias)ViewData["auditoria"];

            if (oAuditoria != null)
            {
                Session["idAuditoria"] = oAuditoria.id;
                Session["ReferenciaAccionMejora"] = oAuditoria.id;
                ddlCentral.SelectedValue = oAuditoria.idCentral.ToString();
                if (oAuditoria.fechainicio != null)
                    txtFechaInicio.Text = oAuditoria.fechainicio.ToString().Replace(" 0:00:00", "");
                if (oAuditoria.fechafin != null)
                    txtFechaFin.Text = oAuditoria.fechafin.ToString().Replace(" 0:00:00", "");
                ddlTipo.SelectedValue = oAuditoria.tipo.ToString();
                txtComentario.Text = oAuditoria.comentario;
                if (oAuditoria.programa != null)
                    lblProgramaAuditoria.Text = oAuditoria.programa.Replace(Server.MapPath("~/Auditorias") + "\\" + oAuditoria.id + "\\Programa\\", "");
                if (oAuditoria.informe != null)
                    lblInformeAuditoria.Text = oAuditoria.informe.Replace(Server.MapPath("~/Auditorias") + "\\" + oAuditoria.id + "\\Informe\\", "");


                if (ViewData["referenciales"] != null)
                {
                    ddlReferenciales.DataSource = ViewData["referenciales"];
                    ddlReferenciales.DataValueField = "id";
                    ddlReferenciales.DataTextField = "nombre";
                    ddlReferenciales.DataBind();
                }
                
                if (ViewData["referencialesasignados"] != null)
                {
                    grdReferenciales.DataSource = ViewData["referencialesasignados"];
                    grdReferenciales.DataBind();
                }

                if (ViewData["usuariosasignar"] != null)
                {
                    ddlObservadores.DataSource = ViewData["usuariosasignar"];
                    ddlObservadores.DataValueField = "idUsuario";
                    ddlObservadores.DataTextField = "nombre";
                    ddlObservadores.DataBind();
                }

                if (ViewData["auditoresasignar"] != null)
                {
                    ddlAuditor.DataSource = ViewData["auditoresasignar"];
                    ddlAuditor.DataValueField = "id";
                    ddlAuditor.DataTextField = "nombre";
                    ddlAuditor.DataBind();
                }
                
                if (ViewData["usuariosasignados"] != null)
                {
                    grdObservadores.DataSource = ViewData["usuariosasignados"];
                    grdObservadores.DataBind();
                }

                if (ViewData["auditores"] != null)
                {
                    grdAuditores.DataSource = ViewData["auditores"];
                    grdAuditores.DataBind();
                }


            }

        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionAuditoriaMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionAuditoriaMensaje"].ToString() + "' });", true);
            Session["EdicionAuditoriaMensaje"] = null;
        }
        if (Session["EdicionAuditoriaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionAuditoriaError"].ToString() + "' });", true);
            Session["EdicionAuditoriaError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        ddlCentral.Enabled = false;
        txtFechaInicio.Enabled = false;
        txtFechaFin.Enabled = false;
        ddlTipo.Enabled = false;
        txtComentario.Enabled = false;
        consulta = 1;
    }
    
    
</script>
<asp:Content ID="headEditarAuditoria" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Auditoría </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarAuditoria")
                    $("#hdFormularioEjecutado").val("GuardarAuditoria");
                else $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "btnAddReferencial")
                    $("#hdFormularioEjecutado").val("btnAddReferencial");
                if (val == "btnAddAuditor")
                    $("#hdFormularioEjecutado").val("btnAddAuditor");
                if (val == "btnAddObservador")
                    $("#hdFormularioEjecutado").val("btnAddObservador");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

        });         
    </script>
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            display: none;
            position: absolute;
            top: 60%;
            left: 25%;
            width: 60%;
            height: 500px;
            padding: 16px;
            border: 5px solid #41b9e6;
            background-color: white;
            z-index: 1002;
            overflow: auto;
            border-radius: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>
                Detalle de la auditoría<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td class="form-group" style="width: 50%">
                            <label>
                                Central</label>
                            <asp:DropDownList Width="95%" ID="ddlCentral" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td  style="width: 10%">
                            <label>
                                Fecha inicio</label>
                            <asp:TextBox ID="txtFechaInicio" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 10%">
                            <label>
                                Fecha fin</label>
                            <asp:TextBox ID="txtFechaFin" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%">
                            <label>
                                Tipo</label>
                            <asp:DropDownList Width="95%" ID="ddlTipo" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="Interna"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Externa"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </table>
                    <br />

                    <table width="100%">
                <tr>
                    <td class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Plan de auditoría</label>
                            <input type="file" id="fileProgramaAuditoria" name="file" style="margin-top:10px" /></center>
                        </td>      
                        <td class="form-group" style="width: 50%"><center>
                        <label>
                                Informe de auditoría</label>
                                <input type="file" id="fileInformeAuditoria" name="file" style="margin-top:10px" /></center>
                        </td>   
                    </tr>
                    <tr style="padding-top:20px">
                        <td>
                            <% if (oAuditoria != null && oAuditoria.programa != null)
                               { %>
                               <center>
                            <a title="Ver Plan Auditoría" href="/evr/Auditorias/ObtenerProgramaAuditoria/<%=oAuditoria.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblProgramaAuditoria" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                        <td>
                        <% if (oAuditoria != null && oAuditoria.informe != null)
                           { %>
                           <center>
                            <a title="Ver Informe Auditoría" href="/evr/Auditorias/ObtenerInformeAuditoria/<%=oAuditoria.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblInformeAuditoria" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <br />

                <table width="100%">
                    <tr>
                        <td class="form-group" colspan="5">
                            <br />
                            <label>
                                Comentario</label>
                            <asp:TextBox ID="txtComentario" Rows="4" TextMode="MultiLine" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div>
        </div>
    </div>    
    

    <% if (oAuditoria != null)
       { %>
    <%--Referenciales --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="despliegue">Referenciales</a></h6>
        </div>
        <div class="panel-body">        
            <%
        if (permisos.permiso == true && oAuditoria != null)
        {
            %>
            <table width="40%">
                <tr>
                    <td style="padding-right:10px">
                        <div class="form-group">
                        <label>
                            Referenciales a asignar:</label>
                        <asp:DropDownList ID="ddlReferenciales" Width="100%" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>                        
                    </td>
                    <td style="padding-top:10px">
                    <input id="btnAddReferencial" type="submit" value="Asignar Referencial"
                            class="btn btn-primary run-first" />
                    </td>
                </tr>
            </table>
            <% } %>
            <asp:GridView ID="grdReferenciales" runat="server" Visible="false">
            </asp:GridView>
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    Nombre
                                </th>
                                <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                                %>
                                <th style="width: 45px">
                                    Eliminar
                                </th>
                                <%
        } %>
                            </tr>
                        </thead>
                        <tbody>
                            <% 
        foreach (GridViewRow item in grdReferenciales.Rows)
        { %>
                            <tr>
                                <td class="task-desc">
                                    <%= item.Cells[3].Text%>
                                </td>                                
                                <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                                %>
                                <td class="text-center">
                                    <a href="/evr/Auditorias/eliminar_referencialasociado/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este referencial?')) return false;"
                                        title="Eliminar"><i class="icon-remove"></i></a>
                                </td>
                                <% } %>
                            </tr>
                            <% }%>
                        </tbody>
                    </table>
                </div>
            </center>
            <br />
        </div>
    </div>


                <%--EquipoAuditor --%>
                <div style="height:100%" class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title">
                            <i class="icon-user"></i><a name="Auditores">Equipo auditor</a></h6>
                    </div>
                    <div class="panel-body">
                        <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                        %>
                        <table width="40%">
                            <tr>
                                <td  style="padding-right:10px">
                                    <label>
                                        Nombre del auditor</label>
                                    <asp:DropDownList ID="ddlAuditor" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td  style="padding-top:10px">
                                    <input id="btnAddAuditor" type="submit" value="Agregar Auditor"
                                     class="btn btn-primary run-first" />
                                </td>
                            </tr>
                        </table>
                        <% } %>
                        <asp:GridView ID="grdAuditores" runat="server" Visible="false">
                        </asp:GridView>
                        <center>
                            <div style="width: 95%" class="datatablePedido">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nombre
                                            </th>
                                            <th style="width: 45px">
                                                Cualificación
                                            </th>
                                            <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                                            %>
                                            <th style="width: 45px">
                                                Eliminar
                                            </th>
                                            <%
        } %>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% 
                                            foreach (GridViewRow item in grdAuditores.Rows)
        { %>
                                        <tr>
                                            <td class="task-desc">
                                                <%= item.Cells[2].Text%>
                                            </td>             
                                            <td class="text-center">
                                            <% if (item.Cells[4].Text != "&nbsp;")
                                               { %>
                                                    <a title="Obtener cualificación" href="/evr/configuracion/ObtenerCualificacion/<%= item.Cells[1].Text %>" title="Ver cualificación"><i class="icon-download"></i></a>
                                                <% } %>
                                                </td>                   
                                            <%
                                            if (permisos.permiso == true && oAuditoria != null)
                                            {
                                            %>
                                            <td class="text-center">
                                                <a href="/evr/Auditorias/eliminar_auditor/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este auditor?')) return false;"
                                                    title="Eliminar"><i class="icon-remove"></i></a>
                                                
                                            </td>
                                            <% } %>
                                        </tr>
                                        <% }%>
                                    </tbody>
                                </table>
                            </div>
                        </center>
                        <br />
                    </div>
                </div>
            

             <%--Observadores --%>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title">
                            <i class="icon-user"></i><a name="Observadores">Observadores</a></h6>
                    </div>
                    <div class="panel-body">
                        <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                        %>
                        <table width="40%">
                            <tr>
                                <td style="padding-right:10px">
                                    <div class="form-group">
                                    <label>
                                        Observadores a asignar:</label>
                                    <asp:DropDownList ID="ddlObservadores" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>                        
                                </td>
                                <td style="padding-top:10px">
                                <input id="btnAddObservador" type="submit" value="Asignar Observador"
                                        class="btn btn-primary run-first" />
                                </td>
                            </tr>
                        </table>
                        <% } %>
                        <asp:GridView ID="grdObservadores" runat="server" Visible="false">
                        </asp:GridView>
                        <center>
                            <div style="width: 95%" class="datatablePedido">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nombre
                                            </th>
                                            <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                                            %>
                                            <th style="width: 45px">
                                                Eliminar
                                            </th>
                                            <%
        } %>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% 
        foreach (GridViewRow item in grdObservadores.Rows)
        { %>
                                        <tr>
                                            <td class="task-desc">
                                                <%= item.Cells[2].Text%>
                                            </td>                                
                                            <%
        if (permisos.permiso == true && oAuditoria != null)
        {
                                            %>
                                            <td class="text-center">
                                                <a href="/evr/Auditorias/eliminar_observadorasociado/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este observador?')) return false;"
                                                    title="Eliminar"><i class="icon-remove"></i></a>
                                            </td>
                                            <% } %>
                                        </tr>
                                        <% }%>
                                    </tbody>
                                </table>
                            </div>
                        </center>
                        <br />
                    </div>
                </div>

    <% } %>

    <% if (oAuditoria != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (permisos.permiso == true && oAuditoria != null)
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
            if (oAuditoria != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (consulta == 0)
            {
        %>
        <input id="GuardarAuditoria" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Auditorias/auditorias" title="Volver" class="btn btn-primary run-first">Volver</a>
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
