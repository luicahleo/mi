<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.comunicacion oComunicacion = new MIDAS.Models.comunicacion();
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

            Session["ModuloAccionMejora"] = 3;

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

            if (ViewData["procesosAsignables"] != null)
            {
                lstProcesosAsignar.DataSource = ViewData["procesosAsignables"];
                lstProcesosAsignar.DataValueField = "id";
                lstProcesosAsignar.DataTextField = "nombre";
                lstProcesosAsignar.DataBind();
            }
            if (ViewData["procesosAsignados"] != null)
            {
                lstProcesosAsignados.DataSource = ViewData["procesosAsignados"];
                lstProcesosAsignados.DataValueField = "id";
                lstProcesosAsignados.DataTextField = "nombre";
                lstProcesosAsignados.DataBind();
            }

            if (ViewData["stakeholders"] != null)
            {
                ddlStakeholder.DataSource = ViewData["stakeholders"];
                ddlStakeholder.DataValueField = "id";
                ddlStakeholder.DataTextField = "denominacionn3";
                ddlStakeholder.DataBind();
            }

            if (ViewData["canales"] != null)
            {
                ddlCanal.DataSource = ViewData["canales"];
                ddlCanal.DataValueField = "id";
                ddlCanal.DataTextField = "canal";
                ddlCanal.DataBind();
            }

            if (ViewData["clasificaciones"] != null)
            {
                ddlClasificacion.DataSource = ViewData["clasificaciones"];
                ddlClasificacion.DataValueField = "id";
                ddlClasificacion.DataTextField = "tipo";
                ddlClasificacion.DataBind();
            }

            if (ViewData["tipos"] != null)
            {
                ddlTipo.DataSource = ViewData["tipos"];
                ddlTipo.DataValueField = "id";
                ddlTipo.DataTextField = "tipo";
                ddlTipo.DataBind();
            }

            if (ViewData["documentoscomunicacion"] != null)
            {
                grdDocumentos.DataSource = ViewData["documentoscomunicacion"];
                grdDocumentos.DataBind();
            }

            oComunicacion = (MIDAS.Models.comunicacion)ViewData["comunicacion"];

            if (oComunicacion != null)
            {
                Session["idComunicacion"] = oComunicacion.id;
                Session["ReferenciaAccionMejora"] = oComunicacion.id;
                txtCodigo.Text = oComunicacion.idcomunicacion;
                ddlTipo.SelectedValue = oComunicacion.tipo.ToString();
                ddlClasificacion.SelectedValue = oComunicacion.clasificacion.ToString();
                ddlStakeholder.SelectedValue = oComunicacion.stakeholder.ToString();
                if (oComunicacion.fechainicio != null)
                    txtFInicio.Text = oComunicacion.fechainicio.ToString().Replace(" 0:00:00", "");
                if (oComunicacion.fechafin != null)
                    txtFFin.Text = oComunicacion.fechafin.ToString().Replace(" 0:00:00", "");
                ddlCanal.SelectedValue = oComunicacion.canal.ToString();
                ddlEficacia.SelectedValue = oComunicacion.eficacia.ToString();
                txtDescripcionCom.Text = oComunicacion.descripcion;
                txtDescripcionRes.Text = oComunicacion.descripcionres;
                txtAsunto.Text = oComunicacion.asunto;
                txtRemitente.Text = oComunicacion.remitente;
                    
                if (ViewData["documentosasociados"] != null)
                {
                    grdDocumentos.DataSource = ViewData["documentosasociados"];
                    grdDocumentos.DataBind();
                }

                if (permisos.permiso != true || oComunicacion.idcentral != centroseleccionado.id)
                    desactivarCampos();
                  
            }

        }
        

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                      
                                    

        if (Session["EdicionComunicacionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionComunicacionMensaje"].ToString() + "' });", true);
            Session["EdicionComunicacionMensaje"] = null;
        }
        if (Session["EdicionComunicacionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionComunicacionError"].ToString() + "' });", true);
            Session["EdicionComunicacionError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtCodigo.Enabled = false;
        ddlTipo.Enabled = false;
        ddlClasificacion.Enabled = false;
        ddlStakeholder.Enabled = false;
        txtFInicio.Enabled = false;
        txtFFin.Enabled = false;
        ddlCanal.Enabled = false;
        ddlEficacia.Enabled = false;
        txtAsunto.Enabled = false;
        txtRemitente.Enabled = false;
        consulta = 1;
    }
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Comunicación </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarComunicacion")
                    $("#hdFormularioEjecutado").val("GuardarComunicacion");
                else $("#hdFormularioEjecutado").val("Recarga");

                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "btnAddDocumento")
                    $("#hdFormularioEjecutado").val("btnAddDocumento");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $('#btnAsignarProceso').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstProcesosAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ningun proceso.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstProcesosAsignados').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarProcesosAsignados();
                e.preventDefault();
            });

            $('#btnNoAsignarProceso').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstProcesosAsignados option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ningún proceso.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstProcesosAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarProcesosAsignados();
                e.preventDefault();
            });           

            

        });

        function comprobarProcesosAsignados() {
            var selectedOpts = $('#ctl00_MainContent_lstProcesosAsignados');
            var procesosseleccionados = '';
            for (var i = 0; i < selectedOpts[0].length; i++) {
                procesosseleccionados = procesosseleccionados + selectedOpts[0].children[i].value + ";";
            }
            $("#ctl00_MainContent_hdnProcesosSeleccionados").val(procesosseleccionados);

        }       
        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>
                Detalle de la comunicación<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td class="form-group" style="width: 10%">
                            <label>
                                ID Comunicación</label>
                            <asp:TextBox ID="txtCodigo" Width="95%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>
                                Remitente</label>
                            <asp:TextBox ID="txtRemitente" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 20%">
                            <label>
                                Tipo</label>
                            <asp:DropDownList ID="ddlTipo" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Clasificación</label>
                            <asp:DropDownList ID="ddlClasificacion" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Parte interesada</label>
                            <asp:DropDownList ID="ddlStakeholder" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 10%">
                            <label>
                                Fecha registro</label>
                            <asp:TextBox ID="txtFInicio" style="text-align:center"  Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td style="width: 20%">
                            <label>
                                Canal</label>
                            <asp:DropDownList ID="ddlCanal" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 30%">
                        <label>
                                Asunto</label>
                            <asp:TextBox ID="txtAsunto" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Eficacia de la acción de comunicacion</label>
                            <asp:DropDownList ID="ddlEficacia" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Eficaz"></asp:ListItem>
                                <asp:ListItem Value="2" Text="No eficaz"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 10%">
                            <label>
                                Fecha cierre</label>
                            <asp:TextBox ID="txtFFin" style="text-align:center" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        <br />
                            <label>
                                Descripción</label>
                            <asp:TextBox ID="txtDescripcionCom" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        <br />
                            <label>
                                Descripción de la resolución</label>
                            <asp:TextBox ID="txtDescripcionRes" TextMode="MultiLine" Rows="5"  Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>

    <% if (oComunicacion != null && oComunicacion.id != 0)
       { %>
    <%--Procesos asociados --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Procesos asociados</a></h6>                
        </div>
        <div class="panel-body">
                <center>
            <table id="tablaCentros" runat="server" style="width:60%">
                <tr>
                    <td style="width:45%"> 
                        <center><label>Procesos a asignar</label></center>
                    </td>
                    <td style="width:10%">
                    
                    </td>
                    <td style="width:45%"> 
                        <center><label>Procesos asignados</label></center>
                    </td>
                </tr>
                <tr>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstProcesosAsignar" runat="server">
                        </asp:ListBox></center>
                    </td>
                    <td>
                    <center>
                    <% 
                        if (consulta == 0)
                        { %>
                        <input id="btnAsignarProceso" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                        <input id="btnNoAsignarProceso" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                        <% } %>
                        </center>
                    </td>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstProcesosAsignados" runat="server">
                        </asp:ListBox></center>
                        <asp:HiddenField ID="hdnProcesosSeleccionados" runat="server" Value="" />
                    </td>
                </tr>
            </table></center>
            <br />
        </div>
    </div>


    <%--Documentos --%>
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
                                                          <a title="Ver Fichero" href="/evr/Comunicacion/ObtenerDocComunicacion/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td> 
                                    <% 
                        if (consulta == 0)
                        { %>
                                    <td class="text-center">
                                        <a href="/evr/Comunicacion/eliminar_doccomunicacion/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;"
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

     <% if (oComunicacion != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (consulta != 1)
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
            if (oComunicacion != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <% 
                        if (consulta == 0)
                        { %>
        <input id="GuardarComunicacion" onclick="comprobarProcesosAsignados();" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
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
