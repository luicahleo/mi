<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.objetivos oObjetivo = new MIDAS.Models.objetivos();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            Session["ModuloAccionMejora"] = 1;

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

            if (ViewData["responsables"] != null)
            {
                ddlResponsable.DataSource = ViewData["responsables"];
                ddlResponsable.DataValueField = "idUsuario";
                ddlResponsable.DataTextField = "nombre";
                ddlResponsable.DataBind();
            }

            if (ViewData["responsables"] != null)
            {
                ddlResponsableAccion.DataSource = ViewData["responsables"];
                ddlResponsableAccion.DataValueField = "idUsuario";
                ddlResponsableAccion.DataTextField = "nombre";
                ddlResponsableAccion.DataBind();
            }

            ListItem aspectoVacio = new ListItem();
            aspectoVacio.Value = "0";
            aspectoVacio.Text = "---";
            ddlAspecto.Items.Insert(0, aspectoVacio);

            if (ViewData["centralesAsignables"] != null)
            {
                lstCentralesAsignar.DataSource = ViewData["centralesAsignables"];
                lstCentralesAsignar.DataValueField = "id";
                lstCentralesAsignar.DataTextField = "nombre";
                lstCentralesAsignar.DataBind();
            }
            if (ViewData["centralesAsignadas"] != null)
            {
                lstCentralesAsignadas.DataSource = ViewData["centralesAsignadas"];
                lstCentralesAsignadas.DataValueField = "id";
                lstCentralesAsignadas.DataTextField = "nombre";
                lstCentralesAsignadas.DataBind();
            }

            if (ViewData["tecnologiasAsignables"] != null)
            {
                lstTecnologiasAsignar.DataSource = ViewData["tecnologiasAsignables"];
                lstTecnologiasAsignar.DataValueField = "id";
                lstTecnologiasAsignar.DataTextField = "nombre";
                lstTecnologiasAsignar.DataBind();
            }
            if (ViewData["tecnologiasAsignadas"] != null)
            {
                lstTecnologiasAsignadas.DataSource = ViewData["tecnologiasAsignadas"];
                lstTecnologiasAsignadas.DataValueField = "id";
                lstTecnologiasAsignadas.DataTextField = "nombre";
                lstTecnologiasAsignadas.DataBind();
            }

            oObjetivo = (MIDAS.Models.objetivos)ViewData["objetivo"];

            if (oObjetivo != null)
            {
                Session["idObjetivo"] = oObjetivo.id;

                Session["ReferenciaAccionMejora"] = oObjetivo.id;

                ddlAmbito.SelectedValue = oObjetivo.ambito.ToString();
                txtNombre.Text = oObjetivo.Nombre;
                txtCodigo.Text = oObjetivo.Codigo;
                txtDescripcionObjetivo.Text = oObjetivo.Descripcion;
                txtPersonasInvolucradas.Text = oObjetivo.personasinv;
                if (oObjetivo.FechaReal != null)
                    txtFReal.Text = oObjetivo.FechaReal.ToString().Replace(" 0:00:00", "");
                if (oObjetivo.FechaEstimada != null)
                    txtFEstimada.Text = oObjetivo.FechaEstimada.ToString().Replace(" 0:00:00", "");
                ddlResponsable.SelectedValue = oObjetivo.Responsable.ToString();
                ddlAspecto.SelectedValue = oObjetivo.idAspecto.ToString();
                txtMetodoMedicion.Text = oObjetivo.metodomedicion;
                txtCoste.Text = oObjetivo.Coste;
                txtMedios.Text = oObjetivo.Medios;
                txtSeguimiento.Text = oObjetivo.Seguimiento;
                //txtComentarios.Text = oObjetivo.Comentarios;
                ddlEstadoObjetivo.SelectedValue = oObjetivo.estado.ToString();

                if (oObjetivo.especifico != null)
                {
                    ddlEspecifico.SelectedValue = oObjetivo.especifico.ToString();

                    if (oObjetivo.especifico == 0)
                    {
                        tablaCentros.Style.Add("display", "none");
                        tablaTecnologias.Style.Add("display", "none");
                    }

                    if (oObjetivo.especifico == 1)
                    {
                        tablaCentros.Style.Add("display", "block"); 
                        tablaTecnologias.Style.Add("display", "none");
                    }

                    if (oObjetivo.especifico == 2)
                    {
                        tablaCentros.Style.Add("display", "none");
                        tablaTecnologias.Style.Add("display", "block"); 
                    }
                }
                else
                {
                    tablaCentros.Style.Add("display", "none");
                    tablaTecnologias.Style.Add("display", "none");
                }
                    
                    
                if (ViewData["despliegue"] != null)
                {
                    grdDespliegue.DataSource = ViewData["despliegue"];
                    grdDespliegue.DataBind();
                }
            }
            else
            {
                tablaCentros.Style.Add("display", "none");
                tablaTecnologias.Style.Add("display", "none");
            }
        }

        if (Session["accionFallida"] != null)
        {
            MIDAS.Models.despliegue desp = (MIDAS.Models.despliegue)Session["accionFallida"];

            if (desp.id != null)
                hdnIdAccion.Value = desp.id.ToString();

            txtActividad.Text = desp.Nombre;
            txtDescripcion.Text = desp.Descripcion;
            txtFechaEstimada.Text = desp.FechaEstimada.ToString().Replace(" 0:00:00", "");
            txtFechaReal.Text = desp.FechaReal.ToString().Replace(" 0:00:00", "");
            txtConsecucion.Text = desp.Porcentaje.ToString();
            ddlResponsable.SelectedValue = desp.Responsable.ToString();
            ddlEstadoAccion.SelectedValue = desp.Estado.ToString();
            txtRecursos.Text = desp.Recursos;
            txtComentariosAccion.Text = desp.Comentarios;
            hdnIdAccionFallida.Value = "1";
        }
        Session.Remove("accionFallida");

        if (permisos.permiso != true)
        {
            txtNombre.Enabled = false;
            txtCodigo.Enabled = false;
            txtDescripcionObjetivo.Enabled = false;
            txtFEstimada.Enabled = false;
            txtFReal.Enabled = false;
            ddlResponsable.Enabled = false;
            txtCoste.Enabled = false;
            txtMedios.Enabled = false;
            txtSeguimiento.Enabled = false;
            ddlEstadoObjetivo.Enabled = false;
            txtPersonasInvolucradas.Enabled = false;
            ddlAmbito.Enabled = false;
            ddlAspecto.Enabled = false;
            txtMetodoMedicion.Enabled = false;
            //txtComentarios.Enabled = false;

            ddlEspecifico.Enabled = false;
        }                                                      

        if (Session["EdicionObjetivoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionObjetivoMensaje"].ToString() + "' });", true);
            Session["EdicionObjetivoMensaje"] = null;
        }
        if (Session["EdicionObjetivoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionObjetivoError"].ToString() + "' });", true);
            Session["EdicionObjetivoError"] = null;
        }   
    }
    
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Objetivo </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            if ($("#" + "<%= hdnIdAccionFallida.ClientID %>").val() == '1') {
                document.getElementById('lightDespliegue').style.display = 'block';
                $("#" + "<%= hdnIdAccionFallida.ClientID %>").val('0')
            }

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarObjetivo")
                    $("#hdFormularioEjecutado").val("GuardarObjetivo");
                else $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "ctl00_MainContent_btnImprimirFC")
                    $("#hdFormularioEjecutado").val("btnImprimirFC");
                if (val == "btnNuevaAccion")
                    $("#hdFormularioEjecutado").val("btnNuevaAccion");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $('#btnAsignarCentro').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna central.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstCentralesAsignadas').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarCentralesAsignadas();
                e.preventDefault();
            });

            $('#btnNoAsignarCentro').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignadas option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna central.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstCentralesAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarCentralesAsignadas();
                e.preventDefault();
            });



            $('#btnAsignarTecnologia').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstTecnologiasAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna tecnología.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstTecnologiasAsignadas').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarTecnologiasAsignadas();
                e.preventDefault();
            });

            $('#btnNoAsignarTecnologia').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstTecnologiasAsignadas option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna tecnología.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstTecnologiasAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarTecnologiasAsignadas();
                e.preventDefault();
            });

            $("#ctl00_MainContent_ddlEspecifico").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlEspecifico').val();

                if (perfilseleccionado == 1) {
                    $("#ctl00_MainContent_tablaCentros").show();
                    $("#ctl00_MainContent_tablaTecnologias").hide();
                }
                if (perfilseleccionado == 2) {
                    $("#ctl00_MainContent_tablaCentros").hide();
                    $("#ctl00_MainContent_tablaTecnologias").show();
                }
                if (perfilseleccionado == 0) {
                    $("#ctl00_MainContent_tablaCentros").hide();
                    $("#ctl00_MainContent_tablaTecnologias").hide();
                }
            });

        });

        

        function comprobarCentralesAsignadas() {
            var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignadas');
            var centrosseleccionados = '';
            for (var i = 0; i < selectedOpts[0].length; i++) {
                centrosseleccionados = centrosseleccionados + selectedOpts[0].children[i].value + ";";
            }
            $("#ctl00_MainContent_hdnCentrosSeleccionados").val(centrosseleccionados);

        }

        function comprobarTecnologiasAsignadas() {
            var selectedOpts = $('#ctl00_MainContent_lstTecnologiasAsignadas');
            var tecnologiasseleccionadas = '';
            for (var i = 0; i < selectedOpts[0].length; i++) {
                tecnologiasseleccionadas = tecnologiasseleccionadas + selectedOpts[0].children[i].value + ";";
            }
            $("#ctl00_MainContent_hdnTecnologiasSeleccionadas").val(tecnologiasseleccionadas);

        }

        function comprobarCentralesyTecnologias() {
            comprobarCentralesAsignadas();
            comprobarTecnologiasAsignadas();
        }

        function modifAccion(id, numaccion, actividad, descripcion, fechaestimada, fechareal, propietario, estado, recursos, comentarioaccion, porcentaje) {
            $("#" + "<%= hdnIdAccion.ClientID %>").val(id);
            $("#" + "<%= txtActividad.ClientID %>").val(actividad);
            $("#" + "<%= ddlResponsableAccion.ClientID %>").val(propietario)
            descripcion = descripcion.replaceAll('<br>', '\r\n');
            $("#" + "<%= txtDescripcion.ClientID %>").val(descripcion);
            recursos = recursos.replaceAll('<br>', '\r\n');
            $("#" + "<%= txtRecursos.ClientID %>").val(recursos);            
            $("#" + "<%= txtConsecucion.ClientID %>").val(porcentaje);
            comentarioaccion = comentarioaccion.replaceAll('<br>', '\r\n');
            $("#" + "<%= txtComentariosAccion.ClientID %>").val(comentarioaccion);
            $("#" + "<%= ddlEstadoAccion.ClientID %>").val(estado).change();
            if (fechaestimada != '') {
                var date = new Date(fechaestimada);
                $("#" + "<%= txtFechaEstimada.ClientID %>").val((("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear()));
            }
            else {
                $("#" + "<%= txtFechaEstimada.ClientID %>").val('');
            }
            if (fechareal != '') {
                date = new Date(fechareal);
                $("#" + "<%= txtFechaReal.ClientID %>").val((("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear()));
            }
            else {
                $("#" + "<%= txtFechaReal.ClientID %>").val('');
            }
            document.getElementById('lightDespliegue').style.display = 'block';
        }

        String.prototype.replaceAll = function (search, replace) {
            if (replace === undefined) {
                return this.toString();
            }
            return this.split(search).join(replace);
        }

        function nuevaAccion() {
            $("#" + "<%= hdnIdAccion.ClientID %>").val(0);
            $("#" + "<%= txtActividad.ClientID %>").val('');
            $("#" + "<%= ddlResponsableAccion.ClientID %>").val('');
            $("#" + "<%= txtDescripcion.ClientID %>").val('');
            $("#" + "<%= ddlEstadoAccion.ClientID %>").val('1');
            $("#" + "<%= txtFechaReal.ClientID %>").val('');
            $("#" + "<%= txtFechaEstimada.ClientID %>").val('');
            $("#" + "<%= txtRecursos.ClientID %>").val('');
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
            <h3>
                Detalle del objetivo<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <%
        if (oObjetivo != null && ( ((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true))
        {
    %>
    <div id="lightDespliegue" style="height: 560px; position: absolute; top: 30%" class="white_content">
        <center>
            <h4>
                Nueva acción</h4>
        </center>
        <br />
        <br />
        <div class="form-group">
            <table width="100%">
                <tr>
                    <td colspan="2">
                    <asp:HiddenField ID="hdnIdAccion" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIdAccionFallida" runat="server" Value="0" />
                        <label >
                            Título (*)</label>
                        <asp:TextBox ID="txtActividad" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; padding-top: 15px">
                        <label>
                            Descripción (*)</label>
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%; padding-top: 15px">
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        F.Estimada (*)</label>
                                    <asp:TextBox ID="txtFechaEstimada" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px;">
                                    <label>
                                        F.Real</label>
                                    <asp:TextBox ID="txtFechaReal" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                    <td style="width: 30%; padding-left: 20px; padding-top: 15px">
                        <label >
                            Consecución % (*)</label>
                        <asp:TextBox ID="txtConsecucion" TextMode="Number" Text="0" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; padding-top: 15px">
                        <label>
                            Responsable (*)</label>
                        <asp:DropDownList runat="server" ID="ddlResponsableAccion" class="form-control" Width="95%"></asp:DropDownList>
                    </td>
                    <td width="50%" style="padding-top: 15px; padding-left: 20px">
                        <label>
                            Estado (*)</label>
                        <asp:DropDownList runat="server" ID="ddlEstadoAccion" class="form-control" Width="100%">
                            <asp:ListItem Value="0" Text="No ejecutado"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Pendiente de ejecutar"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Ejecutado"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; padding-top:15px" colspan="2">
                        <label>
                            Recursos (*)</label>
                        <asp:TextBox ID="txtRecursos" TextMode="MultiLine" Rows="3"  Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; padding-top:15px" colspan="2">
                        <label>
                            Seguimiento</label>
                        <asp:TextBox ID="txtComentariosAccion" TextMode="MultiLine" Rows="3" Width="100%" runat="server"
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
                        <td class="form-group" style="width: 10%">
                            <label>
                                Codigo</label>
                            <asp:TextBox ID="txtCodigo" Width="95%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Ámbito</label>
                            <asp:DropDownList runat="server" ID="ddlAmbito" class="form-control" Width="97%"></asp:DropDownList>
                        </td>
                        <td colspan="2" style="width: 50%">
                            <label>
                                Titulo (*)</label>
                            <asp:TextBox ID="txtNombre" Width="98%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 20%">
                            <label>
                                Responsable (*)</label>
                            <asp:DropDownList runat="server" ID="ddlResponsable" class="form-control" Width="100%"></asp:DropDownList>
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
                <table width="100%">
                    <tr>
                        <td class="form-group" colspan="4">
                            <br />
                            <label>
                                Descripcion (*)</label>
                            <asp:TextBox ID="txtDescripcionObjetivo" Rows="4" TextMode="MultiLine" runat="server" class="form-control" style="margin-bottom:5px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width:50%">
                            <label  style="margin-top: 10px">
                                Aspecto Ambiental (*)</label>
                            <asp:DropDownList runat="server" ID="ddlAspecto" class="form-control" Width="97%"></asp:DropDownList>
                        </td>
                        <td colspan="2" style="width:50%">
                            <label  style="margin-top: 10px">
                                Método de medición (*)</label>
                            <asp:TextBox ID="txtMetodoMedicion" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>               
                    <td class="form-group" style="width: 25%;">
                        <label>
                            Coste (*)</label>
                        <asp:TextBox ID="txtCoste" Width="95%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                    <td class="form-group" style="width: 25%;">
                        <label>
                            Medios (*)</label>
                        <asp:TextBox ID="txtMedios" Width="95%" runat="server" class="form-control"></asp:TextBox>
                    </td>        
                        <td class="form-group" style="width: 10%">
                            <label>
                                Fecha Estimada (*)</label>
                            <asp:TextBox ID="txtFEstimada" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 10%">
                            <label>
                                Fecha Real</label>
                            <asp:TextBox ID="txtFReal" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 30%;">
                        <label>
                            Estado (*)</label>
                            <asp:DropDownList ID="ddlEstadoObjetivo" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="En seguimiento"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Alcanza resultados esperados"></asp:ListItem>
                                <asp:ListItem Value="2" Text="No alcanza resultados esperados"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Desestimado"></asp:ListItem>
                            </asp:DropDownList>
                        </td>   
                    </tr>
                    <tr>
                    <td colspan="5" style="width: 100%; padding-top: 15px">
                        <label>
                            Seguimiento (Indicar fecha del seguimiento, situación actual y medidas a tomar en caso de desviación de lo planificado)</label>
                        <asp:TextBox ID="txtSeguimiento" Rows="4" TextMode="MultiLine" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
                </table>
            </div>
        </div>
    </div>


    <% if (oObjetivo!= null && oObjetivo.Tipo == 0)  
       {%>
    <%--Específico --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Centros</a></h6>                
        </div>
        <div class="panel-body">
        <div class="form-group">
                            <label>
                                ¿Especificar centros?</label>
                            <asp:DropDownList ID="ddlEspecifico" style="width:15%" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                            </asp:DropDownList>
                </div>
                <center>
            <table id="tablaCentros" runat="server" style="width:60%">
                <tr>
                    <td style="width:45%"> 
                        <center><label>Centrales a asignar</label></center>
                    </td>
                    <td style="width:10%">
                    
                    </td>
                    <td style="width:45%"> 
                        <center><label>Centrales asignadas</label></center>
                    </td>
                </tr>
                <tr>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstCentralesAsignar" runat="server">
                        </asp:ListBox></center>
                    </td>
                    <td>
                    <center>
                    <% 
                        if ((user.perfil == 1 || user.perfil == 3) && centroseleccionado.tipo == 4)
                        { %>
                        <input id="btnAsignarCentro" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                        <input id="btnNoAsignarCentro" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                        <% } %>
                        </center>
                    </td>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstCentralesAsignadas" runat="server">
                        </asp:ListBox></center>
                        <asp:HiddenField ID="hdnCentrosSeleccionados" runat="server" Value="" />
                    </td>
                </tr>
            </table></center>

             <center>
            <table id="tablaTecnologias" runat="server" style="width:60%">
                <tr>
                    <td style="width:45%"> 
                        <center><label>Tecnologías a asignar</label></center>
                    </td>
                    <td style="width:10%">
                    
                    </td>
                    <td style="width:45%"> 
                        <center><label>Tecnologías asignadas</label></center>
                    </td>
                </tr>
                <tr>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px; height:70px" Rows="5" ID="lstTecnologiasAsignar" runat="server">
                        </asp:ListBox></center>
                    </td>
                    <td>
                    <center>
                    <% 
                        if ((user.perfil == 1 || user.perfil == 3) && centroseleccionado.tipo == 4)
                       { %>
                        <input id="btnAsignarTecnologia" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                        <input id="btnNoAsignarTecnologia" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                        <% } %>
                        </center>
                    </td>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px; height:70px" Rows="5" ID="lstTecnologiasAsignadas" runat="server">
                        </asp:ListBox></center>
                        <asp:HiddenField ID="hdnTecnologiasSeleccionadas" runat="server" Value="" />
                    </td>
                </tr>
            </table></center>
            <br />
        </div>
    </div>
    <% } %>

    <%--Despliegue --%>
    <% if (oObjetivo != null && oObjetivo.id != 0)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="despliegue">Acciones</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                                    Titulo
                                </th>
                                <th>
                                    Responsable
                                </th>
                                <th style="width: 110px">
                                    Fecha estimada
                                </th>
                                <th style="width: 110px">
                                    Fecha real
                                </th>
                                <th style="width: 150px">
                                    Estado
                                </th>
                                <th style="width: 45px">
                                    Consecución %
                                </th>
                                <th style="width: 45px">
                                    Evidencias
                                </th>
                                <%
        if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
        {
                                %>
                                <th style="width: 45px">
                                    Editar
                                </th>
                                <%
        } %>
                                <%
        if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
        {
                                %>
                                <th style="width: 45px">
                                    Eliminar
                                </th>
                                <%
        } %>
                                <%--                                                <th style="width:45px">Baja</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <% 
        foreach (GridViewRow item in grdDespliegue.Rows)
        { %>
                            <tr>
                                <td class="task-desc">
                                    <%= item.Cells[2].Text%>
                                </td>
                                <td class="task-desc">
                                    <%= item.Cells[3].Text%>
                                </td>
                                <td class="task-desc">
                                    <%= item.Cells[11].Text%>
                                </td>
                                <td style="text-align: center" class="task-desc">
                                <% 
        if (item.Cells[5].Text != "&nbsp;")
        { %>
                                    <%= item.Cells[5].Text.Replace(" 0:00:00", "")%>
                                <%} %>
                                </td>
                                

                                <td style="text-align: center" class="task-desc">
                                <% 
        if (item.Cells[6].Text != "&nbsp;")
        { %>
                                    <%= item.Cells[6].Text.Replace(" 0:00:00", "")%>
                                <% } %>
                                </td>
                                
                                <% 
        if (item.Cells[8].Text == "0")
        { %>
                                <td class="task-desc">
                                   <b>No ejecutado</b>
                                </td>
                                <% }%><% 
                                     
                                                    
        if (item.Cells[8].Text == "2")
        { %>
                                <td style="color: LimeGreen" class="task-desc">
                                <b>
                                    Ejecutado</b>
                                </td>
                                <% } %>
                                <% 
        if (item.Cells[8].Text == "1")
        {
            if (item.Cells[5].Text.Replace(" 0:00:00", "") != "&nbsp;")
            {
                DateTime fecha = DateTime.Parse(item.Cells[5].Text.Replace(" 0:00:00", ""));
                if (fecha < DateTime.Now.Date)
                {
                                %>
                                <td style="color: Red" class="task-desc">
                                <b>
                                    Pendiente de ejecutar</b>
                                </td>
                                <% 
        }
                else
                {
                                %>
                                <td style="color: Orange" class="task-desc">
                                <b>
                                    Pendiente de ejecutar</b>
                                </td>
                                
                                <%
        }



            }
        } %>
                                   <td>
                                   <center>
                                    <%= item.Cells[12].Text%></center>
                                </td>
                                   <td class="text-center">
                                                    <a onclick="window.open(this.href, this.target, 'width=950,height=600'); return false;" href="/evr/Objetivos/evidencias/<%= item.Cells[0].Text %>" title="Ver evidencias"><i class="icon-download"></i></a>
                                                </td>
                                <%
                                    if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
        {
                                %>
                                <td class="text-center">
                                <% 

        string fechaeststr = "";
        string fecharealstr = "";
        if (item.Cells[5].Text != "&nbsp;")
            fechaeststr = ((DateTime.Parse(item.Cells[5].Text.Replace(" 0:00:00", ""))).Month + "-" + (DateTime.Parse(item.Cells[5].Text.Replace(" 0:00:00", ""))).Day + "-" + (DateTime.Parse(item.Cells[5].Text.Replace(" 0:00:00", ""))).Year);
        if (item.Cells[6].Text != "&nbsp;")
            fecharealstr = ((DateTime.Parse(item.Cells[6].Text.Replace(" 0:00:00", ""))).Month + "-" + (DateTime.Parse(item.Cells[6].Text.Replace(" 0:00:00", ""))).Day + "-" + (DateTime.Parse(item.Cells[6].Text.Replace(" 0:00:00", ""))).Year);
                                           
                                         %>
                                    <a id="btnModifAccion" onclick="modifAccion(<%= item.Cells[0].Text%>,'<%= item.Cells[2].Text%>','<%= item.Cells[3].Text%>','<%= item.Cells[4].Text.Replace("\r\n","<br>") %>','<%= fechaeststr %>','<%= fecharealstr %>','<%= item.Cells[7].Text%>','<%= item.Cells[8].Text%>','<%= item.Cells[9].Text.Replace("\r\n","<br>") %>','<%= item.Cells[10].Text.Replace("\r\n","<br>") %>','<%= item.Cells[12].Text %>' );"
                                        data-toggle="modal" role="button" href="#table_modal3" title="Editar acción"><i class="icon-pencil">
                                        </i></a>
                                </td>
                                <% } %>
                                <%
                                    if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
        {
                                %>
                                <td class="text-center">
                                    <a href="/evr/Objetivos/eliminar_accion/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar esta acción?')) return false;"
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

    <% if (oObjetivo != null && oObjetivo.id != 0)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
           <%
               if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                <div class="block">
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
                                                                            <%  if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                                                                            <% if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                                                                                if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                                                                            <% if (((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion)) && permisos.permiso == true)
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
                </div>
            </center>
            <br />
        </div>
    </div>
    <% } %>

    <div class="form-actions text-right">
        <%
            if (oObjetivo != null && oObjetivo.Tipo == 0 && oObjetivo.ambito == 2)
            {
        %>
        <asp:Button ID="btnImprimirFC" runat="server" class="btn btn-primary run-first" Text="Ficha de control" />
        <%} %>
        <%
                                 if (oObjetivo != null)
                                 {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <% } %>
        <%
            if ((oObjetivo == null || ((oObjetivo.Tipo == 0 && centroseleccionado.tipo == 4) || (oObjetivo.Tipo == 1 && centroseleccionado.id == oObjetivo.idorganizacion))) && permisos.permiso == true)
            {
        %>
        <input id="GuardarObjetivo" onclick="comprobarCentralesyTecnologias()" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Objetivos/gestion_objetivos" class="btn btn-primary" >Volver</a></>
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
