<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.reuniones oReunion = new MIDAS.Models.reuniones();
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

            Session["ModuloAccionMejora"] = 12;

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

            oReunion = (MIDAS.Models.reuniones)ViewData["reunion"];

            if (oReunion != null)
            {
                Session["idAuditoria"] = oReunion.id;
                Session["ReferenciaAccionMejora"] = oReunion.id;
                txtCodigo.Text = oReunion.cod_reunion;
                if (oReunion.fecha_convocatoria != null)
                    txtFechaConvocatoria.Text = oReunion.fecha_convocatoria.ToString().Replace(" 0:00:00", "");
                if (oReunion.horainicio != null)
                    txtHoraInicio.Text = oReunion.horainicio.ToString();
                if (oReunion.horafin != null)
                    txtHoraFin.Text = oReunion.horafin.ToString();
                txtOrden.Text = oReunion.ordendeldia;
                txtResumenAcuerdos.Text = oReunion.resumen;
                ddlEstado.SelectedValue = oReunion.estado.ToString();
                txtPersonasInvolucradas.Text = oReunion.personasinv;
                txtAsunto.Text = oReunion.asunto;

                if (ViewData["usuariosasignar"] != null)
                {
                    ddlObservadores.DataSource = ViewData["usuariosasignar"];
                    ddlObservadores.DataValueField = "idUsuario";
                    ddlObservadores.DataTextField = "nombre";
                    ddlObservadores.DataBind();
                }
                if (ViewData["usuariosasignados"] != null)
                {
                    grdObservadores.DataSource = ViewData["usuariosasignados"];
                    grdObservadores.DataBind();
                }

                if (ViewData["accionesmejora"] != null)
                {
                    grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                    grdAccionesMejora.DataBind();
                }


                if (ViewData["documentosreunion"] != null)
                {
                    grdDocumentos.DataSource = ViewData["documentosreunion"];
                    grdDocumentos.DataBind();
                }
            }
            else
            {
                txtHoraInicio.Text = "00:00";
                txtHoraFin.Text = "00:00";
            }
        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionReunionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionReunionMensaje"].ToString() + "' });", true);
            Session["EdicionReunionMensaje"] = null;
        }
        if (Session["EdicionReunionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionReunionError"].ToString() + "' });", true);
            Session["EdicionReunionError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtFechaConvocatoria.Enabled = false;
        txtHoraInicio.Enabled = false;
        txtHoraFin.Enabled = false;
        ddlEstado.Enabled = false;
        txtOrden.Enabled = false;
        txtResumenAcuerdos.Enabled = false;

        consulta = 1;
    }
    
    
</script>
<asp:Content ID="headEditarAuditoria" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Reunión </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarReunion")
                    $("#hdFormularioEjecutado").val("GuardarReunion");
                else $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "btnAddObservador")
                    $("#hdFormularioEjecutado").val("btnAddObservador");
                if (val == "btnAddDocumento")
                    $("#hdFormularioEjecutado").val("btnAddDocumento");
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
                Detalle de la reunión<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                                Codigo</label>
                            <asp:TextBox ID="txtCodigo" Width="95%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td  style="width: 20%">
                            <label>
                                Fecha de convocatoria (*)</label>
                            <asp:TextBox ID="txtFechaConvocatoria" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Hora de inicio (*)</label>
                            <asp:TextBox ID="txtHoraInicio" Width="95%" TextMode="Time" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Hora de fin (*)</label>
                            <asp:TextBox ID="txtHoraFin" Width="95%" TextMode="Time" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Estado (*)</label>
                            <asp:DropDownList runat="server" ID="ddlEstado" class="form-control" Width="100%">
                                <asp:ListItem Value="0" Text="En seguimiento"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Cerrada"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </table>

                <table width="100%">
                    <tr>
                        <td class="form-group" colspan="5">
                            <br />
                            <label>
                                Asunto</label>
                            <asp:TextBox ID="txtAsunto" runat="server" class="form-control"></asp:TextBox>
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
                    <tr>
                        <td class="form-group" colspan="5">
                            <br />
                            <label>
                                Orden del día</label>
                            <asp:TextBox ID="txtOrden" Rows="4" TextMode="MultiLine" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-group" colspan="5">
                            <br />
                            <label>
                                Resumen de acuerdos adoptados</label>
                            <asp:TextBox ID="txtResumenAcuerdos" Rows="4" TextMode="MultiLine" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div>
        </div>
    </div>    
    

    <% if (oReunion != null)
       { %>            

             <%--Participantes --%>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title">
                            <i class="icon-user"></i><a name="Observadores">Participantes</a></h6>
                    </div>
                    <div class="panel-body">
                        <%
        if (permisos.permiso == true && oReunion != null)
        {
                        %>
                        <table width="40%">
                            <tr>
                                <td style="padding-right:10px">
                                    <div class="form-group">
                                    <label>
                                        Participantes a asignar:</label>
                                    <asp:DropDownList ID="ddlObservadores" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>                        
                                </td>
                                <td style="padding-top:10px">
                                <input id="btnAddObservador" type="submit" value="Asignar Participante"
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
                                            if (permisos.permiso == true && oReunion != null)
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
        if (permisos.permiso == true && oReunion != null)
        {
                                            %>
                                            <td class="text-center">
                                                <a href="/evr/Auditorias/eliminar_participante/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este participante?')) return false;"
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

    <%--Documentos --%>
    <% if (oReunion != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="despliegue">Documentos</a></h6>
        </div>
        <div class="panel-body">
            <% 
                        if (consulta == 0)
                        { %>
            <table style="width:50%" id="tablaFicheroNuevo">
                                 <tr> 
                                    <td style="padding-right:10px; width:50%">
                                        <div class="form-group">
                                                <label>
                                                    Documento:</label>
                                                    <asp:TextBox ID="txtNombreDoc" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>   
                                    <td style="width:50%; padding-top:10px">
                                        <div class="form-group">
                                        <input type="file" id="file" name="file" style="margin-top:10px" /></div>
                                    </td>       
                                    <td>
                                         <input style="margin-top: 5px" type="submit" class="btn btn-primary" name="Submit"
                id="btnAddDocumento" value="Añadir documento" />
                                    </td>  
                                    </tr>
                                    </table>
                
           
            <% } %>
            <asp:GridView ID="grdDocumentos" runat="server" Visible="false">
            </asp:GridView>
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    Documento
                                </th>
                                <th>
                                    Nombre del fichero
                                </th>
                                <th style="width:45px">
                                        Descarga
                                    </th>
                                    <% 
                        if (consulta == 0)
                        { %>
                                    <th style="width:45px">
                                        Eliminar
                                    </th><% } %>
                            </tr>
                        </thead>
                        <tbody>
                            <% 
                                                foreach (GridViewRow item in grdDocumentos.Rows)
                                                     { %>
                            <tr>
                                <td class="task-desc">
                                        <%= item.Cells[2].Text %>
                                    </td>
                                    <td class="task-desc">
                                        <%= item.Cells[3].Text %>
                                    </td>
                                     <td style="text-align:center" class="task-desc">
                                                    <center>
                                                          <a title="Ver Fichero" href="/evr/actasreunion/ObtenerDocReunion/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td> 
                                    <% 
                        if (consulta == 0)
                        { %>
                                    <td class="text-center">
                                        <a href="/evr/actasreunion/eliminar_docreunion/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;"
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

    <% if (oReunion != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (permisos.permiso == true && oReunion != null)
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
                                                                            <th style="width:60px">
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
            if (oReunion != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (consulta == 0)
            {
        %>
        <input id="GuardarReunion" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/actasreunion/gestionreuniones" title="Volver" class="btn btn-primary run-first">Volver</a>
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
