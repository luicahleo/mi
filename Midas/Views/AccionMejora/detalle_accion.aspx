<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.accionesmejora oAccionesMejora = new MIDAS.Models.accionesmejora();
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

            if (ViewData["tiposaccionmejora"] != null)
            {
                ddlTipo.DataSource = ViewData["tiposaccionmejora"];
                ddlTipo.DataValueField = "id";
                ddlTipo.DataTextField = "nombre";
                ddlTipo.DataBind();
            }
            
            if (ViewData["modulos"] != null)
            {
                ddlAntecedente.DataSource = ViewData["modulos"];
                ddlAntecedente.DataValueField = "id";
                ddlAntecedente.DataTextField = "nombre";
                ddlAntecedente.DataBind();
            }
            if (Session["ModuloAccionMejora"] == null)
            {
                ListItem antecedenteVacio = new ListItem();
                antecedenteVacio.Value = "0";
                antecedenteVacio.Text = "---";
                ddlAntecedente.Items.Insert(0, antecedenteVacio);
            }

            if (ViewData["procesos"] != null)
            {
                ddlProceso.DataSource = ViewData["procesos"];
                ddlProceso.DataValueField = "id";
                ddlProceso.DataTextField = "nombre";
                ddlProceso.DataBind();
            }
            
            ListItem procesoVacio = new ListItem();
            procesoVacio.Value = "0";
            procesoVacio.Text = "---";
            ddlProceso.Items.Insert(0, procesoVacio);

            if (ViewData["referencias"] != null)
            {
                ddlReferencia.DataSource = ViewData["referencias"];
                ddlReferencia.DataValueField = "id";
                ddlReferencia.DataTextField = "nombre";
                ddlReferencia.DataBind();
            }

            if (ViewData["referencialesAsignables"] != null)
            {
                lstReferencialesAsignar.DataSource = ViewData["referencialesAsignables"];
                lstReferencialesAsignar.DataValueField = "id";
                lstReferencialesAsignar.DataTextField = "nombre";
                lstReferencialesAsignar.DataBind();
            }
            if (ViewData["referencialesAsignadas"] != null)
            {
                lstReferencialesAsignados.DataSource = ViewData["referencialesAsignadas"];
                lstReferencialesAsignados.DataValueField = "id";
                lstReferencialesAsignados.DataTextField = "nombre";
                lstReferencialesAsignados.DataBind();
            }

            if (ViewData["responsables"] != null)
            {
                ddlResponsable.DataSource = ViewData["responsables"];
                ddlResponsable.DataValueField = "idUsuario";
                ddlResponsable.DataTextField = "nombre";
                ddlResponsable.DataBind();
            }

            if (ViewData["responsables"] != null)
            {
                ddlResponsableAI.DataSource = ViewData["responsables"];
                ddlResponsableAI.DataValueField = "idUsuario";
                ddlResponsableAI.DataTextField = "nombre";
                ddlResponsableAI.DataBind();                
            }

            if (ViewData["responsables"] != null)
            {
                ddlResponsableAccion.DataSource = ViewData["responsables"];
                ddlResponsableAccion.DataValueField = "idUsuario";
                ddlResponsableAccion.DataTextField = "nombre";
                ddlResponsableAccion.DataBind();
                
            }

            if (ViewData["ambitos"] != null)
            {
                ddlAmbito.DataSource = ViewData["ambitos"];
                ddlAmbito.DataValueField = "id";
                ddlAmbito.DataTextField = "nombre_ambito";
                ddlAmbito.DataBind();

                ListItem itemAmbitoGeneral = new ListItem();
                itemAmbitoGeneral.Text = "General";
                itemAmbitoGeneral.Value = "0";
                ddlAmbito.Items.Insert(0, itemAmbitoGeneral);

            }

            if (ViewData["documentosaccionesmejora"] != null)
            {
                grdDocumentos.DataSource = ViewData["documentosaccionesmejora"];
                grdDocumentos.DataBind();
            }

            if (ViewData["acciones"] != null)
            {
                grdDespliegue.DataSource = ViewData["acciones"];
                grdDespliegue.DataBind();
            }

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }

            oAccionesMejora = (MIDAS.Models.accionesmejora)ViewData["accionmejora"];

            if (oAccionesMejora != null)
            {
                if (oAccionesMejora.id != null && oAccionesMejora.id != 0)
                    columnaDesplegable.Visible = false;
                else
                    columnaTextbox.Visible = false;
                Session["idAccionMejora"] = oAccionesMejora.id;

                txtAsunto.Text = oAccionesMejora.asunto;
                txtAnio.Text = oAccionesMejora.codigo;
                if (oAccionesMejora.fecha_apertura != null)
                    txtFApertura.Text = oAccionesMejora.fecha_apertura.ToString().Replace(" 0:00:00", "");
                if (oAccionesMejora.fecha_cierre != null)
                    txtFCierre.Text = oAccionesMejora.fecha_cierre.ToString().Replace(" 0:00:00", "");
                ddlResponsable.SelectedValue = oAccionesMejora.responsable.ToString();
                ddlTipo.SelectedValue = oAccionesMejora.tipo.ToString();
                ddlEstado.SelectedValue = oAccionesMejora.estado.ToString();
                ddlAntecedente.SelectedValue = oAccionesMejora.antecedente.ToString();
                ddlAmbito.SelectedValue = oAccionesMejora.ambito.ToString();
                if ((ddlAntecedente.SelectedIndex != 0 && ddlAntecedente.SelectedValue != "9") || Session["ModuloAccionMejora"] != null)
                {
                    if (oAccionesMejora.referencia != null)
                        ddlReferencia.SelectedValue = oAccionesMejora.referencia.ToString();
                    ddlReferencia.Visible = true;
                    txtReferencia.Visible = false;
                }
                else
                {
                    txtReferencia.Text = oAccionesMejora.referencianoconforme;
                    ddlReferencia.Visible = false;
                    txtReferencia.Visible = true;
                }                
                ddlProceso.SelectedValue = oAccionesMejora.proceso.ToString();
                ddlContratista.SelectedValue = oAccionesMejora.contratista.ToString();
                txtDetectado.Text = oAccionesMejora.detectadopor;
                txtCausa.Text = oAccionesMejora.causas;
                txtDescripcion.Text = oAccionesMejora.descripcion;
                txtPersonasInvolucradas.Text = oAccionesMejora.personasinv;
                
                txtDescripcionAI.Text = oAccionesMejora.ai_descripcion;
                ddlResponsableAI.SelectedValue = oAccionesMejora.ai_responsable.ToString(); 
                if (oAccionesMejora.ai_ffin_prevista != null)
                    txtFFinAI.Text = oAccionesMejora.ai_ffin_prevista.ToString().Replace(" 0:00:00", "");
                if (oAccionesMejora.ai_fcierre != null)
                    txtFCierreAI.Text = oAccionesMejora.ai_fcierre.ToString().Replace(" 0:00:00", "");
                ddlEstadoAI.SelectedValue = oAccionesMejora.ai_estado.ToString();
                txtComentarioAI.Text = oAccionesMejora.ai_comentario;

                if (oAccionesMejora.especifico != null)
                {
                    ddlEspecifico.SelectedValue = oAccionesMejora.especifico.ToString();

                    if (oAccionesMejora.especifico == 1)
                    {
                        tablaReferenciales.Style.Add("display", "block");
                    }
                }
                else
                {
                    tablaReferenciales.Style.Add("display", "none");
                }

                if (ViewData["documentosaccionesmejora"] != null)
                {
                    grdDocumentos.DataSource = ViewData["documentosaccionesmejora"];
                    grdDocumentos.DataBind();
                }
            }
            else
            {
                if (Session["ModuloAccionMejora"] != null)
                {
                ddlReferencia.Visible = true;
                txtReferencia.Visible = false;
                }
                else
                {
                ddlReferencia.Visible = false;
                txtReferencia.Visible = true;
                }
                columnaTextbox.Visible = false;
            }
        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionAccionMejoraMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionAccionMejoraMensaje"].ToString() + "' });", true);
            Session["EdicionAccionMejoraMensaje"] = null;
        }
        if (Session["EdicionAccionMejoraError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionAccionMejoraError"].ToString() + "' });", true);
            Session["EdicionAccionMejoraError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtAsunto.Enabled = false;
        txtAnio.Enabled = false;
        txtFApertura.Enabled = false;
        txtFCierre.Enabled = false;
        ddlResponsable.Enabled = false;
        ddlEstado.Enabled = false;
        ddlAntecedente.Enabled = false;
        txtReferencia.Enabled = false;
        ddlProceso.Enabled = false;
        txtCausa.Enabled = false;
        consulta = 1;
    }    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Accion Mejora </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarAccionMejora")
                    $("#hdFormularioEjecutado").val("GuardarAccionMejora");
                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");
                if (val == "ctl00_MainContent_btnImprimirNC")
                    $("#hdFormularioEjecutado").val("btnImprimirNC");
                if (val == "btnNuevaAccion")
                    $("#hdFormularioEjecutado").val("btnNuevaAccion");
                if (val == "btnAddDocumento")
                    $("#hdFormularioEjecutado").val("btnAddDocumento");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_ddlEspecifico").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlEspecifico').val();

                if (perfilseleccionado == 1) {
                    $("#ctl00_MainContent_tablaReferenciales").show();
                }
                if (perfilseleccionado == 0) {
                    $("#ctl00_MainContent_tablaReferenciales").hide();
                }
            });

            function comprobarReferencialesAsignados() {
                var selectedOpts = $('#ctl00_MainContent_lstReferencialesAsignados');
                var referencialesseleccionados = '';
                for (var i = 0; i < selectedOpts[0].length; i++) {
                    referencialesseleccionados = referencialesseleccionados + selectedOpts[0].children[i].value + ";";
                }
                $("#ctl00_MainContent_hdnReferencialesSeleccionados").val(referencialesseleccionados);

            }

            $('#btnAsignarReferencial').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstReferencialesAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ningún referencial.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstReferencialesAsignados').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarReferencialesAsignados();
                e.preventDefault();
            });

            $('#btnNoAsignarReferencial').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstReferencialesAsignados option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ningún referencial.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstReferencialesAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarReferencialesAsignados();
                e.preventDefault();
            });
        });

            function modifAccion(id, descripcion, fechafin, fechacierre, responsable, estado, comentarioaccion) {
            $("#" + "<%= hdnIdAccion.ClientID %>").val(id);
            $("#" + "<%= ddlResponsableAccion.ClientID %>").val(responsable)
            descripcion = descripcion.replaceAll('<br>', '\r\n');
            $("#" + "<%= txtDescripcionAccion.ClientID %>").val(descripcion);
            comentarioaccion = comentarioaccion.replaceAll('<br>', '\r\n');
            $("#" + "<%= txtComentariosAccion.ClientID %>").val(comentarioaccion);
            $("#" + "<%= ddlEstadoAccion.ClientID %>").val(estado).change();
            if (fechafin != '') {
                var date = new Date(fechafin);
                $("#" + "<%= txtFechaFinAccion.ClientID %>").val((("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear()));
            }
            else {
                $("#" + "<%= txtFechaFinAccion.ClientID %>").val('');
            }
            if (fechacierre != '') {
                date = new Date(fechacierre);
                $("#" + "<%= txtFechaCierreAccion.ClientID %>").val((("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear()));
            }
            else {
                $("#" + "<%= txtFechaCierreAccion.ClientID %>").val('');
            }
            document.getElementById('lightDespliegue').style.display = 'block';
            document.getElementById('fade').style.display = 'block';
        }

        String.prototype.replaceAll = function (search, replace) {
            if (replace === undefined) {
                return this.toString();
            }
            return this.split(search).join(replace);
        }

        function nuevaAccion() {
            $("#" + "<%= hdnIdAccion.ClientID %>").val(0);
            $("#" + "<%= ddlResponsableAccion.ClientID %>").prop('selectedIndex', 0);
            $("#" + "<%= txtDescripcionAccion.ClientID %>").val('');
            $("#" + "<%= ddlEstadoAccion.ClientID %>").prop('selectedIndex', 0);
            $("#" + "<%= txtFechaFinAccion.ClientID %>").val('');
            $("#" + "<%= txtFechaCierreAccion.ClientID %>").val('');
            $("#" + "<%= txtComentariosAccion.ClientID %>").val('');

            document.getElementById('lightDespliegue').style.display = 'block';
            document.getElementById('fade').style.display = 'block';
        }        
       
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
            <h3>Detalle de la acción de mejora<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    
    <%
        if (permisos.permiso == true && oAccionesMejora != null)
        {
    %>
    <div id="lightDespliegue" style="height: 550px; position: absolute; top: 60%" class="white_content">
        <center>
            <h4>
                Nueva acción</h4>
        </center>
        <br />
        <br />
        <div class="form-group">
            <table width="100%">
                <tr>
                    <td colspan="2" style="width: 100%">
                        <label>
                            Descripción</label>
                        <asp:HiddenField ID="hdnIdAccion" runat="server" Value="0" />
                        <asp:TextBox ID="txtDescripcionAccion" TextMode="MultiLine" Rows="5" Width="100%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    
                    <td style="width: 50%; padding-top: 15px">
                        <table>
                            <tr>
                                <td style="width:50%">
                                    <label>
                                        Nº Acción</label>
                                    <asp:TextBox ID="txtNumAccion" TextMode="Number" Width="100%" runat="server" class="form-control"></asp:TextBox>
                                </td>
                                <td style="width:50%; padding-left:10px">
                                    <label>
                                        Estado</label>
                                    <asp:DropDownList runat="server" ID="ddlEstadoAccion" class="form-control" Width="100%">
                                            <asp:ListItem Value="0" Text="Ejecutada"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No ejecutada"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="En Ejecución"></asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>

                        </table>
                        
                    </td>
                    <td style="width: 50%; padding-top: 15px">
                        <label>
                            Responsable</label>
                        <asp:DropDownList runat="server" ID="ddlResponsableAccion" class="form-control" Width="95%"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; padding-top: 15px; padding-right:15px">
                        <label>
                            Fecha Fin Prevista</label>
                        <asp:TextBox ID="txtFechaFinAccion" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                    </td>
                    <td style="width: 50%; padding-top: 15px">
                        <label>
                            Fecha Cierre</label>
                        <asp:TextBox ID="txtFechaCierreAccion" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; padding-top:15px" colspan="2">
                        <label>
                            Seguimiento</label>
                        <asp:TextBox ID="txtComentariosAccion" TextMode="MultiLine" Rows="5" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <br />
        <center>
            <input id="btnNuevaAccion" type="submit" value="Guardar" class="btn btn-primary run-first" />&nbsp;
            <input id="btnCerrarAccion" type="button" value="Cerrar" onclick="document.getElementById('lightDespliegue').style.display='none';document.getElementById('fade').style.display='none'"
                class="btn btn-primary run-first" />
        </center>
    </div>
    <% } %>

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
                        <td runat="server" id="columnaDesplegable" class="form-group" style="width: 5%">
                            <label>Año</label>
                            <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="columnaTextbox" class="form-group" style="width: 5%">
                            <label>Codigo</label>
                            <asp:TextBox ID="txtAnio" Width="92%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 40%">
                            <label>Asunto</label>
                            <asp:TextBox ID="txtAsunto"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 15%">
                             <label>Ámbito</label>
                            <asp:DropDownList runat="server" ID="ddlAmbito" class="form-control" Width="95%"></asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                             <label>Tipo</label>
                            <asp:DropDownList runat="server" ID="ddlTipo" class="form-control" Width="100%"></asp:DropDownList>
                        </td>
                        
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 15%"><center>
                            <label>Fecha Apertura</label>
                            <asp:TextBox ID="txtFApertura" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width: 15%"><center>
                            <label>Fecha Cierre</label>
                            <asp:TextBox ID="txtFCierre" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td runat="server" id="Td1" class="form-group" style="width: 20%">
                            <label>Responsable</label>
                            <asp:DropDownList runat="server" ID="ddlResponsable" class="form-control" Width="95%"></asp:DropDownList>
                        </td>
                        <td runat="server" id="Td3" class="form-group" style="width: 15%">
                            <label>Estado</label>
                            <asp:DropDownList runat="server" ID="ddlEstado" class="form-control" Width="95%">
                                <asp:ListItem Value="0" Text="Abierto"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Cerrado eficaz"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Cerrado no eficaz"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="Td2" class="form-group" style="width: 35%">
                            <label>Proceso</label>
                            <asp:DropDownList runat="server" ID="ddlProceso" class="form-control" Width="100%"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                 <table width="100%">
                    <tr>
                        
                        <td style="width: 10%">
                            <label>Antecedentes</label>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlAntecedente" class="form-control" Width="95%"></asp:DropDownList>
                        </td>
                        <td style="width: 40%">
                            <label>Referencia</label>
                            <asp:DropDownList runat="server" ID="ddlReferencia" class="form-control" Width="98%"></asp:DropDownList>
                            <asp:TextBox ID="txtReferencia"  Width="98%" runat="server" class="form-control" ></asp:TextBox>
                        </td>              
                        <td style="width: 15%">
                            <label>Atribuible a contratista</label>
                            <asp:DropDownList runat="server" ID="ddlContratista" class="form-control" Width="95%">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                            </asp:DropDownList>
                        </td>    
                        <td style="width: 35%">
                            <label>Detectada por</label>
                            <asp:TextBox ID="txtDetectado" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                        </td>           
                    </tr>
                </table>                
                <br />
                <table width="100%">
                    <tr >
                        <td>
							<label>Descripción</label>
                            <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
						</td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <label>
                            Personas involucradas</label>
                            <asp:TextBox ID="txtPersonasInvolucradas" Rows="3" TextMode="MultiLine" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                            <br />
                        </td>
                    </tr>
                    <tr >
                        <td>
							<label>Causa</label>
                            <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtCausa" runat="server" class="form-control" ></asp:TextBox>
						</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

     <% if (oAccionesMejora!= null)  
       {%>
    <%--Específico --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Referenciales</a></h6>                
        </div>
        <div class="panel-body">
        <div class="form-group">
                            <label>
                                ¿Especificar referenciales?</label>
                            <asp:DropDownList ID="ddlEspecifico" style="width:15%" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                            </asp:DropDownList>
                </div>
                <center>
            <table id="tablaReferenciales" runat="server" style="width:60%">
                <tr>
                    <td style="width:45%"> 
                        <center><label>Referenciales a asignar</label></center>
                    </td>
                    <td style="width:10%">
                    
                    </td>
                    <td style="width:45%"> 
                        <center><label>Referenciales asignados</label></center>
                    </td>
                </tr>
                <tr>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstReferencialesAsignar" runat="server">
                        </asp:ListBox></center>
                    </td>
                    <td>
                    <center>
                    <% 
                        if (permisos.permiso == true)
                        { %>
                        <input id="btnAsignarReferencial" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                        <input id="btnNoAsignarReferencial" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                        <% } %>
                        </center>
                    </td>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstReferencialesAsignados" runat="server">
                        </asp:ListBox></center>
                        <asp:HiddenField ID="hdnReferencialesSeleccionados" runat="server" Value="" />
                    </td>
                </tr>
            </table></center>
            <br />
        </div>
    </div>
    <% } %>

    <%--Acción inmediata --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-pencil"></i>Acción Inmediata</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                  <table width="100%">
                    <tr>
                        <td colspan="4">
                            <label>
                                Descripción</label>
                            <asp:TextBox ID="txtDescripcionAI" TextMode="MultiLine" Rows="5"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:15px; width:30%;">
                            <label>Responsable</label>
                            <asp:DropDownList runat="server" ID="ddlResponsableAI" class="form-control" Width="95%"></asp:DropDownList>
                        </td>
                        <td style="padding-top:15px; width:15%;">
                            <label>F. Fin Prevista</label>
                            <asp:TextBox ID="txtFFinAI" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td style="padding-top:15px; width:15%;">
                            <label>F. Cierre</label>
                            <asp:TextBox ID="txtFCierreAI" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td  style="padding-top:15px; width:30%;">
                        <label>Estado</label>
                            <asp:DropDownList runat="server" ID="ddlEstadoAI" class="form-control" Width="95%">
                                <asp:ListItem Value="-1" Text="---"></asp:ListItem>
                                <asp:ListItem Value="0" Text="No Ejecutada"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Ejecutada"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:15px" colspan="4">
                            		<label>Seguimiento</label>
                                    <asp:TextBox TextMode="MultiLine" Rows="5" ID="txtComentarioAI" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                  </table>
            </div>
        </div>
    </div>

    <% if (oAccionesMejora != null && oAccionesMejora.id != 0)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="despliegue">Acciones</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (permisos.permiso == true && oAccionesMejora != null)
        {
            %>
            <table width="100%">
                <tr>
                    <td>
                        <input id="btnAddDespliegue" type="button" value="Nuevo" onclick="nuevaAccion()"
                            class="btn btn-primary run-first" />
                    </td>
                </tr>
            </table>
            <% } %>
            <asp:GridView ID="grdDespliegue" runat="server" Visible="false">
            </asp:GridView>
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    Nº Acción
                                </th>
                                <th>
                                    Descripcción
                                </th>
                                <th>
                                    Responsable
                                </th>
                                <th style="width: 110px">
                                    Fecha Cierre
                                </th>
                                <th style="width: 110px">
                                    Fecha Fin Prevista
                                </th>
                                <th style="width: 150px">
                                    Estado
                                </th>
                                <%
                                    if (permisos.permiso == true && oAccionesMejora != null)
        {
                                %>
                                <th style="width: 45px">
                                    Editar
                                </th>
                                <% } %>
                                <%
                                    if (permisos.permiso == true && oAccionesMejora != null)
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
        foreach (GridViewRow item in grdDespliegue.Rows)
        { %>
                            <tr>
                                <td class="task-desc">
                                    <%= item.Cells[9].Text %>
                                </td>
                                <td class="task-desc">
                                    <%= item.Cells[1].Text%>
                                </td>
                                <td class="task-desc">
                                    <%= item.Cells[2].Text%>
                                </td>
                                <td style="text-align: center" class="task-desc">
                                <% 
        if (item.Cells[3].Text != "&nbsp;")
        { %>
                                    <%= item.Cells[4].Text.Replace(" 0:00:00", "")%>
                                <%} %>
                                </td>
                                

                                <td style="text-align: center" class="task-desc">
                                <% 
        if (item.Cells[4].Text != "&nbsp;")
        { %>
                                    <%= item.Cells[3].Text.Replace(" 0:00:00", "")%>
                                <% } %>
                                </td>
                                
                                <% 
        if (item.Cells[5].Text == "1")
        { %>
                                <td class="task-desc">
                                   <b>No ejecutado</b>
                                </td>
                                <% }%>
                                
                                <% 
        if (item.Cells[5].Text == "2")
        { %>
                                <td class="task-desc">
                                   <b>En ejecución</b>
                                </td>
                                <% }%><% 
                                     
                                                    
        if (item.Cells[5].Text == "0")
        { %>
                                <td style="color: LimeGreen" class="task-desc">
                                <b>
                                    Ejecutado</b>
                                </td>
                                <% } %>

                                <%
                                    if (permisos.permiso == true && oAccionesMejora != null)
        {
                                %>
                                <td class="text-center">
                                <% 

        string fechaFin = "";
        string fechaCierre = "";
        if (item.Cells[3].Text != "&nbsp;")
            fechaFin = ((DateTime.Parse(item.Cells[3].Text.Replace(" 0:00:00", ""))).Month + "-" + (DateTime.Parse(item.Cells[3].Text.Replace(" 0:00:00", ""))).Day + "-" + (DateTime.Parse(item.Cells[3].Text.Replace(" 0:00:00", ""))).Year);
        if (item.Cells[4].Text != "&nbsp;")
            fechaCierre = ((DateTime.Parse(item.Cells[4].Text.Replace(" 0:00:00", ""))).Month + "-" + (DateTime.Parse(item.Cells[4].Text.Replace(" 0:00:00", ""))).Day + "-" + (DateTime.Parse(item.Cells[4].Text.Replace(" 0:00:00", ""))).Year);
                                           
                                         %>
                                    <a id="btnModifAccion" onclick="modifAccion(<%= item.Cells[0].Text%>,'<%= item.Cells[1].Text.Replace("\r\n","<br>") %>','<%= fechaFin %>','<%= fechaCierre %>','<%= item.Cells[7].Text%>','<%= item.Cells[5].Text%>','<%= item.Cells[6].Text.Replace("\r\n","<br>")%>' );"
                                        data-toggle="modal" role="button" href="#table_modal3" title="Editar acción"><i class="icon-pencil">
                                        </i></a>
                                </td>
                                <% } %>
                                <%
                                    if (permisos.permiso == true && oAccionesMejora != null)
        {
                                %>
                                <td class="text-center">
                                    <a href="/evr/AccionMejora/eliminar_accion/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar esta acción?')) return false;"
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
                                                          <a title="Ver Fichero" href="/evr/AccionMejora/ObtenerDocAccMejora/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td> 
                                    <% 
                        if (consulta == 0)
                        { %>
                                    <td class="text-center">
                                        <a href="/evr/AccionMejora/eliminar_docaccmejora/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;"
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
    
    
    <div class="form-actions text-right">
        <%
            if (oAccionesMejora != null && oAccionesMejora.tipo == 1 && oAccionesMejora.ambito == 2)
            {
        %>
        <asp:Button ID="btnImprimirNC" runat="server" class="btn btn-primary run-first" Text="Informe NC" />
        <%} %>
        <%
            if (oAccionesMejora != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (consulta == 0) 
            {
        %>
        <input id="GuardarAccionMejora" onclick="comprobarCentralesAsignadas()" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>

        <%
            if (Session["ModuloAccionMejora"] != null && Session["ReferenciaAccionMejora"] != null)
            {
                int ModuloAccionMejora = int.Parse(Session["ModuloAccionMejora"].ToString());
                int ReferenciaAccionMejora = int.Parse(Session["ReferenciaAccionMejora"].ToString());
                switch (ModuloAccionMejora)
                {
                    //Objetivos
                    case 1:
                        %>
                            <a href="/evr/Objetivos/detalle_objetivo/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Auditorías
                    case 2:
                        %>
                            <a href="/evr/Auditorias/detalle_auditoria/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Comunicación
                    case 3:
                        %>
                            <a href="/evr/Comunicacion/detalle_comunicacion/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Formación
                    case 4:
                        %>
                            <a href="/evr/Formacion/detalle_formacion/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Emergencias
                    case 5:
                        %>
                            <a href="/evr/Emergencias/detalle_emergencia/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Satisfacción
                    case 6:
                        %>
                            <a href="/evr/Satisfaccion/detalle_satisfaccion/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Revisión Energética
                    case 7:
                        %>
                            <a href="/evr/RevEnergetica/detalle_revision/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Requisitos Legales
                    case 8:
                        %>
                            <a href="/evr/Requisitos/detalle_requisito/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Indicadores
                    case 9:
                        %>
                            <a href="/evr/Indicadores/detalle_indicador/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Aspectos Ambientales
                    case 10:
                        %>
                            <a href="/evr/Aspectos/detalle_aspecto/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    //Riesgos y Oportunidades
                    case 11:
                        %>
                            <a href="/evr/Riesgos/detalle_riesgo/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    case 12:
                        %>
                            <a href="/evr/ActasReunion/detalle_reunion/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    case 13:
                        %>
                            <a href="/evr/Comunicacion/detalle_evento_amb/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    case 14:
                        %>
                            <a href="/evr/Comunicacion/detalle_evento_cal/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    case 15:
                        %>
                            <a href="/evr/Comunicacion/detalle_evento_seg/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    case 16:
                        %>
                            <a href="/evr/Comunicacion/detalle_parte/<%= ReferenciaAccionMejora %>" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                    default:
                        %>
                            <a href="/evr/AccionMejora/accionesmejora" title="Volver" class="btn btn-primary run-first">Volver</a>
                        <%
            break;
                }




            }
            else
            {
             %>
             <a href="/evr/AccionMejora/accionesmejora" title="Volver" class="btn btn-primary run-first">Volver</a>
            <%
            } %>
        
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
