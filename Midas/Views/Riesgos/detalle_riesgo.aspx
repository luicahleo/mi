<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.Riesgos oRiesgo = new MIDAS.Models.Riesgos();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null)
            {

                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            }

            Session["ModuloAccionMejora"] = 11;

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

            if (ViewData["categorias"] != null)
            {
                ddlCategoria.DataSource = ViewData["categorias"];
                ddlCategoria.DataValueField = "id";
                ddlCategoria.DataTextField = "categoria";
                ddlCategoria.DataBind();
            }

            if (ViewData["tipologias"] != null)
            {
                ddlTipologia.DataSource = ViewData["tipologias"];
                ddlTipologia.DataValueField = "id";
                ddlTipologia.DataTextField = "tipologia";
                ddlTipologia.DataBind();
            }

            if (ViewData["stakeholdersAsignables"] != null)
            {
                lstPartesInteresadasAsignar.DataSource = ViewData["stakeholdersAsignables"];
                lstPartesInteresadasAsignar.DataValueField = "id";
                lstPartesInteresadasAsignar.DataTextField = "denominacionn2";
                lstPartesInteresadasAsignar.DataBind();
            }
            if (ViewData["stakeholdersAsignadas"] != null)
            {
                lstPartesInteresadasAsignadas.DataSource = ViewData["stakeholdersAsignadas"];
                lstPartesInteresadasAsignadas.DataValueField = "id";
                lstPartesInteresadasAsignadas.DataTextField = "denominacionn2";
                lstPartesInteresadasAsignadas.DataBind();
            }

            if (ViewData["valuechain"] != null)
            {
                ddlValueChain.DataSource = ViewData["valuechain"];
                ddlValueChain.DataValueField = "id";
                ddlValueChain.DataTextField = "nombre";
                ddlValueChain.DataBind();
            }
            ListItem itemVC = new ListItem();
            itemVC.Value = "0";
            itemVC.Text = "-";
            ddlValueChain.Items.Insert(0, itemVC);

            if (ViewData["macroprocesos"] != null)
            {
                ddlMacroproceso.DataSource = ViewData["macroprocesos"];
                ddlMacroproceso.DataValueField = "id";
                ddlMacroproceso.DataTextField = "nombre";
                ddlMacroproceso.DataBind();
            }
            ListItem itemMP = new ListItem();
            itemMP.Value = "0";
            itemMP.Text = "-";
            ddlMacroproceso.Items.Insert(0, itemMP);

            if (ViewData["procesos"] != null)
            {
                ddlProceso.DataSource = ViewData["procesos"];
                ddlProceso.DataValueField = "id";
                ddlProceso.DataTextField = "nombre";
                ddlProceso.DataBind();
            }
            ListItem itemP = new ListItem();
            itemP.Value = "0";
            itemP.Text = "-";
            ddlProceso.Items.Insert(0, itemP);
            
            oRiesgo = (MIDAS.Models.Riesgos)ViewData["riesgo"];

            if (oRiesgo != null)
            {
                Session["ReferenciaAccionMejora"] = oRiesgo.Id;
                txtCod.Text = oRiesgo.CodigoRiesgo.ToString();
                ddlTipo.SelectedValue = oRiesgo.Tipo.ToString();
                ddlValueChain.SelectedValue = oRiesgo.idCadenaValor.ToString();
                ddlMacroproceso.SelectedValue = oRiesgo.idMacroproceso.ToString();
                ddlProceso.SelectedValue = oRiesgo.idProceso.ToString();
                txtDescripcion.Text = oRiesgo.Descripcion;
                ddlCategoria.Text = oRiesgo.Categoria.ToString();
                ddlTipologia.Text = oRiesgo.Tipologia.ToString();
                txtFechaCreacion.Text = oRiesgo.fechaCreacion.ToString().Replace(" 0:00:00", "");
                txtFechaModificacion.Text = oRiesgo.fechaModificacion.ToString().Replace(" 0:00:00", "");
                ddlVigente.Text = oRiesgo.vigente.ToString();
                
                ddlProbabilidadRI.SelectedValue = oRiesgo.RI_ProbabilidadOcurrencia;
                ddlImpactoObjetivosRI.SelectedValue = oRiesgo.RI_ImpactoObjetivos;
                ddlImpactoEconomicoRI.SelectedValue = oRiesgo.RI_ImpactoEconomico;
                ddlImpactoProcesosRI.SelectedValue = oRiesgo.RI_ImpactoProcesosNegocio;
                ddlImpactoReputacionalRI.SelectedValue = oRiesgo.RI_ImpactoReputacional;
                ddlImpactoCumplimientoRI.SelectedValue = oRiesgo.RI_ImpactoCumplimiento;
                
                txtImpactoGeneralRI.Text = oRiesgo.RI_ImpactoGeneral;
                txtRelevanciaRI.Text = oRiesgo.RI_RelevanciaRiesgo;
                txtRelevanceValueRI.Text = oRiesgo.RI_ValorRelevanciaRiesgo;
                ddlGestionRiesgoRI.SelectedValue = oRiesgo.RI_GestionRiesgo;

                if (oRiesgo.LimitOOE == true)
                    chkLimitOOE.Checked = true;
                if (oRiesgo.LimitOO == true)
                    chkLimitOO.Checked = true;
                if (oRiesgo.LimitE == true)
                    chkLimitE.Checked = true;
                if (oRiesgo.SinLimit == true)
                    chkSinLimit.Checked = true;
                if (oRiesgo.SinEfectos == true)
                    chkSinEfectos.Checked = true;
                if (oRiesgo.EfectD == true)
                    chkEfectD.Checked = true;
                if (oRiesgo.EfectDI == true)
                    chkEfectDI.Checked = true;

                txtValoracionOportunidad.Text = oRiesgo.ValoracionOportunidad;
                txtGestionOportunidad.Text = oRiesgo.GestionOportunidad;

                txtDescripcionControl.Text = oRiesgo.DescripcionControl;
                txtPropietarioControl.Text = oRiesgo.PropietarioControl;

                ddlProbabilidadRR.SelectedValue = oRiesgo.RR_ProbabilidadOcurrencia;
                ddlImpactoObjetivosRR.SelectedValue = oRiesgo.RR_ImpactoObjetivos;
                ddlImpactoEconomicoRR.SelectedValue = oRiesgo.RR_ImpactoEconomico;
                ddlImpactoProcesosRR.SelectedValue = oRiesgo.RR_ImpactoProcesosNegocio;
                ddlImpactoReputacionalRR.SelectedValue = oRiesgo.RR_ImpactoReputacional;
                ddlImpactoCumplimientoRR.SelectedValue = oRiesgo.RR_ImpactoCumplimiento;

                txtImpactoGeneralRR.Text = oRiesgo.RR_ImpactoGeneral;
                txtRelevanciaRR.Text = oRiesgo.RR_RelevanciaRiesgo;
                txtRelevanceValueRR.Text = oRiesgo.RR_ValorRelevanciaRiesgo;
            }
     
        }

        if (permisos.permiso != true)
        {
            txtCod.ReadOnly = true;
            ddlTipo.Enabled = true;
            ddlValueChain.Enabled = true;
            ddlMacroproceso.Enabled = true;
            ddlProceso.Enabled = true;
            txtDescripcion.ReadOnly = true;
            ddlCategoria.Enabled = false;
            ddlTipologia.Enabled = false;
            txtFechaCreacion.ReadOnly = false;
            txtFechaModificacion.ReadOnly = false;
            ddlVigente.Enabled = false;

            ddlProbabilidadRI.Enabled = true;
            ddlImpactoObjetivosRI.Enabled = true;
            ddlImpactoEconomicoRI.Enabled = true;
            ddlImpactoProcesosRI.Enabled = true;
            ddlImpactoReputacionalRI.Enabled = true;
            ddlImpactoCumplimientoRI.Enabled = true;

            ddlGestionRiesgoRI.Enabled = true;

            chkLimitOOE.Enabled = true;
            chkLimitOO.Enabled = true;
            chkLimitE.Enabled = true;
            chkSinLimit.Enabled = true;
            chkSinEfectos.Enabled = true;
            chkEfectD.Enabled = true;
            chkEfectDI.Enabled = true;

            txtDescripcionControl.ReadOnly = true;
            txtPropietarioControl.ReadOnly = true;

            ddlProbabilidadRR.Enabled = true;
            ddlImpactoObjetivosRR.Enabled = true;
            ddlImpactoEconomicoRR.Enabled = true;
            ddlImpactoProcesosRR.Enabled = true;
            ddlImpactoReputacionalRR.Enabled = true;
            ddlImpactoCumplimientoRR.Enabled = true;

        }
                                                      

        if (Session["EdicionRiesgoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionRiesgoMensaje"].ToString() + "' });", true);
            Session["EdicionRiesgoMensaje"] = null;
        }
        if (Session["EdicionRiesgoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionRiesgoError"].ToString() + "' });", true);
            Session["EdicionRiesgoError"] = null;
        }             

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Riesgo </title>

    <style type="text/css">
        .GradienteMediumHigh
        {
            background: linear-gradient(to right, yellow , red);
        }
        .GradienteMediumLow
        {
            background: linear-gradient(to right, yellow , green);
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            calcularRI();
            calcularRR();
            calcularEvaluacionOportunidad();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")
                calcularRI();
                calcularRR();
                calcularEvaluacionOportunidad();
                if (val == "GuardarRiesgo")
                    $("#hdFormularioEjecutado").val("GuardarRiesgo");
                else $("#hdFormularioEjecutado").val("btnImprimir");



            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });


            //LISTBOXES
            $('#btnAsignarParteInteresada').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstPartesInteresadasAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna parte interesada.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstPartesInteresadasAsignadas').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarPartesInteresadasAsignadas();
                e.preventDefault();
            });

            $('#btnNoAsignarParteInteresada').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstPartesInteresadasAsignadas option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna parte interesada.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstPartesInteresadasAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarPartesInteresadasAsignadas();
                e.preventDefault();
            });

            //EVENTOS RIESGO INHERENTE
            $("#ctl00_MainContent_ddlProbabilidadRI").change(function () {
                calcularRI();
            });

            $("#ctl00_MainContent_ddlImpactoObjetivosRI").change(function () {
                calcularRI();
            });

            $("#ctl00_MainContent_ddlImpactoEconomicoRI").change(function () {
                calcularRI();
            });

            $("#ctl00_MainContent_ddlImpactoProcesosRI").change(function () {
                calcularRI();
            });

            $("#ctl00_MainContent_ddlImpactoReputacionalRI").change(function () {
                calcularRI();
            });

            $("#ctl00_MainContent_ddlImpactoCumplimientoRI").change(function () {
                calcularRI();
            });

            //EVENTOS RIESGO RESIDUAL
            $("#ctl00_MainContent_ddlProbabilidadRR").change(function () {
                calcularRR();
            });

            $("#ctl00_MainContent_ddlImpactoObjetivosRR").change(function () {
                calcularRR();
            });

            $("#ctl00_MainContent_ddlImpactoEconomicoRR").change(function () {
                calcularRR();
            });

            $("#ctl00_MainContent_ddlImpactoProcesosRR").change(function () {
                calcularRR();
            });

            $("#ctl00_MainContent_ddlImpactoReputacionalRR").change(function () {
                calcularRR();
            });

            $("#ctl00_MainContent_ddlImpactoCumplimientoRR").change(function () {
                calcularRR();
            });

            //EVENTOS EVALUACION OPORTUNIDAD
            $("#ctl00_MainContent_chkLimitOOE").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkLimitOO").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkLimitE").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkSinLimit").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkSinEfectos").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkEfectD").change(function () {
                calcularEvaluacionOportunidad();
            });

            $("#ctl00_MainContent_chkEfectDI").change(function () {
                calcularEvaluacionOportunidad();
            });


        });

        function comprobarPartesInteresadasAsignadas() {
            var selectedOpts = $('#ctl00_MainContent_lstPartesInteresadasAsignadas');
            var centrosseleccionados = '';
            for (var i = 0; i < selectedOpts[0].length; i++) {
                centrosseleccionados = centrosseleccionados + selectedOpts[0].children[i].value + ";";
            }
            $("#ctl00_MainContent_hdnPartesInteresadasSeleccionadas").val(centrosseleccionados);

        }

        function comprobarPartesInteresadas() {
            comprobarPartesInteresadasAsignadas();
        }

        function calcularEvaluacionOportunidad() {
            var S1 = $('#ctl00_MainContent_chkLimitOOE').prop('checked');
            var T1 = $('#ctl00_MainContent_chkLimitOO').prop('checked');
            var U1 = $('#ctl00_MainContent_chkLimitE').prop('checked');
            var V1 = $('#ctl00_MainContent_chkSinLimit').prop('checked');
            var W1 = $('#ctl00_MainContent_chkSinEfectos').prop('checked');
            var X1 = $('#ctl00_MainContent_chkEfectD').prop('checked');
            var Y1 = $('#ctl00_MainContent_chkEfectDI').prop('checked');

            if (S1 == true && W1 == true) {
                $('#ctl00_MainContent_txtValoracionOportunidad').val('1');
            }
            else {
                if (S1 == true && X1 == true) {
                    $('#ctl00_MainContent_txtValoracionOportunidad').val('2');
                }
                else {
                    if (S1 == true && Y1 == true) {
                        $('#ctl00_MainContent_txtValoracionOportunidad').val('3');
                    }
                    else {
                        if (T1 == true && W1 == true) {
                            $('#ctl00_MainContent_txtValoracionOportunidad').val('2');
                        }
                        else {
                            if (T1 == true && X1 == true) {
                                $('#ctl00_MainContent_txtValoracionOportunidad').val('4');
                            }
                            else {
                                if (T1 == true && Y1 == true) {
                                    $('#ctl00_MainContent_txtValoracionOportunidad').val('6');
                                }
                                else {
                                    if (U1 == true && W1 == true) {
                                        $('#ctl00_MainContent_txtValoracionOportunidad').val('3');
                                    }
                                    else {
                                        if (U1 == true && X1 == true) {
                                            $('#ctl00_MainContent_txtValoracionOportunidad').val('6');
                                        }
                                        else {
                                            if (U1 == true && Y1 == true) {
                                                $('#ctl00_MainContent_txtValoracionOportunidad').val('9');
                                            }
                                            else {
                                                if (V1 == true && W1 == true) {
                                                    $('#ctl00_MainContent_txtValoracionOportunidad').val('4');
                                                }
                                                else {
                                                    if (V1 == true && X1 == true) {
                                                        $('#ctl00_MainContent_txtValoracionOportunidad').val('8');
                                                    }
                                                    else {
                                                        if (V1 == true && Y1 == true) {
                                                            $('#ctl00_MainContent_txtValoracionOportunidad').val('12');
                                                        }
                                                        else {
                                                            $('#ctl00_MainContent_txtValoracionOportunidad').val('');
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var Z1 = $('#ctl00_MainContent_txtValoracionOportunidad').val();

            if (Z1 == "") {
                $('#ctl00_MainContent_txtGestionOportunidad').val('');
            }
            else {
                if (Z1 >= 6) {
                    $('#ctl00_MainContent_txtGestionOportunidad').val('Viable');
                }
                else {
                    $('#ctl00_MainContent_txtGestionOportunidad').val('No Viable');
                }
            }

        }

        function calcularRI() {
            var I1 = $('#ctl00_MainContent_ddlProbabilidadRI').val();
            if (I1 == 'ALTO') {
                $('#ctl00_MainContent_ddlProbabilidadRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlProbabilidadRI').css('color', 'White');
            }
            if (I1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlProbabilidadRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlProbabilidadRI').css('color', 'Black');
            }
            if (I1 == 'BAJO') {
                $('#ctl00_MainContent_ddlProbabilidadRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlProbabilidadRI').css('color', 'White');
            }
            if (I1 == '0') {
                $('#ctl00_MainContent_ddlProbabilidadRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlProbabilidadRI').css('color', 'Black');
            }

            var J1 = $('#ctl00_MainContent_ddlImpactoObjetivosRI').val();
            if (J1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('color', 'White');
            }
            if (J1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('color', 'Black');
            }
            if (J1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('color', 'White');
            }
            if (J1 == '0') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoObjetivosRI').css('color', 'Black');
            }

            var K1 = $('#ctl00_MainContent_ddlImpactoEconomicoRI').val();
            if (K1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('color', 'White');
            }
            if (K1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('color', 'Black');
            }
            if (K1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('color', 'White');
            }
            if (K1 == '0') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoEconomicoRI').css('color', 'Black');
            }

            var L1 = $('#ctl00_MainContent_ddlImpactoProcesosRI').val();
            if (L1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('color', 'White');
            }
            if (L1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('color', 'Black');
            }
            if (L1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('color', 'White');
            }
            if (L1 == '0') {
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoProcesosRI').css('color', 'Black');
            }

            var M1 = $('#ctl00_MainContent_ddlImpactoReputacionalRI').val();
            if (M1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('color', 'White');
            }
            if (M1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('color', 'Black');
            }
            if (M1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('color', 'White');
            }
            if (M1 == '0') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoReputacionalRI').css('color', 'Black');
            }

            var N1 = $('#ctl00_MainContent_ddlImpactoCumplimientoRI').val();
            if (N1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('color', 'White');
            }
            if (N1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('color', 'Black');
            }
            if (N1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('color', 'White');
            }
            if (N1 == '0') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRI').css('color', 'Black');
            }

            //IMPACTO GENERAL
            if (J1 == '0' && K1 == '0' && L1 == '0' && M1 == '0' && N1 == '0') {
                $('#ctl00_MainContent_txtImpactoGeneralRI').val('-');
                $('#ctl00_MainContent_txtImpactoGeneralRI').css('background-color', 'White');
                $('#ctl00_MainContent_txtImpactoGeneralRI').css('color', 'Black');
            }
            else {
                if (J1 == 'ALTO' || K1 == 'ALTO' || L1 == 'ALTO' || M1 == 'ALTO' || N1 == 'ALTO') {
                    $('#ctl00_MainContent_txtImpactoGeneralRI').val('ALTO');
                    $('#ctl00_MainContent_txtImpactoGeneralRI').css('background-color', 'Red');
                    $('#ctl00_MainContent_txtImpactoGeneralRI').css('color', 'White');
                }
                else {
                    if (J1 == 'MEDIO' || K1 == 'MEDIO' || L1 == 'MEDIO' || M1 == 'MEDIO' || N1 == 'MEDIO') {
                        $('#ctl00_MainContent_txtImpactoGeneralRI').val('MEDIO');
                        $('#ctl00_MainContent_txtImpactoGeneralRI').css('background-color', 'Yellow');
                        $('#ctl00_MainContent_txtImpactoGeneralRI').css('color', 'Black');
                    }
                    else {
                        $('#ctl00_MainContent_txtImpactoGeneralRI').val('BAJO');
                        $('#ctl00_MainContent_txtImpactoGeneralRI').css('background-color', 'Green');
                        $('#ctl00_MainContent_txtImpactoGeneralRI').css('color', 'White');
                    }
                }
            }

            var O1 = $('#ctl00_MainContent_txtImpactoGeneralRI').val();

            //RELEVANCIA RIESGO INHERENTE
            if (I1 == 'BAJO' && O1 == 'BAJO') {
                $('#ctl00_MainContent_txtRelevanciaRI').val('BAJO');
                $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'Green');
                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumHigh');
                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumLow');
                $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'White');
            }
            else {
                if ((I1 == 'BAJO' && O1 == 'MEDIO') || (I1 == 'MEDIO' && O1 == 'BAJO')) {
                    $('#ctl00_MainContent_txtRelevanciaRI').val('MEDIO-BAJO');
                    $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'Green');
                    document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumHigh');
                    document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.add('GradienteMediumLow');
                    $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'Black');
                }
                else {
                    if ((I1 == 'BAJO' && O1 == 'ALTO') || (I1 == 'MEDIO' && O1 == 'MEDIO') || (I1 == 'ALTO' && O1 == 'BAJO')) {                        
                        $('#ctl00_MainContent_txtRelevanciaRI').val('MEDIO');
                        $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'Yellow');
                        document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumHigh');
                        document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumLow');
                        $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'Black');
                    }
                    else
                    {
                        if ((I1 == 'ALTO' && O1 == 'MEDIO') || (I1 == 'MEDIO' && O1 == 'ALTO')) {
                            $('#ctl00_MainContent_txtRelevanciaRI').val('MEDIO-ALTO');
                            $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'Red');
                            document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.add('GradienteMediumHigh');
                            document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumLow');
                            $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'Black');
                        }
                        else {
                            if ((I1 == 'ALTO' && O1 == 'ALTO')) {
                                $('#ctl00_MainContent_txtRelevanciaRI').val('ALTO');
                                $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'Red');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumHigh');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumLow');
                                $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'White');
                            }
                            else {
                                $('#ctl00_MainContent_txtRelevanciaRI').val('-');
                                $('#ctl00_MainContent_txtRelevanciaRI').css('background-color', 'White');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumHigh');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRI").classList.remove('GradienteMediumLow');
                                $('#ctl00_MainContent_txtRelevanciaRI').css('color', 'Black');
                            }
                        }
                    }
                }
            }

            var P1 = $('#ctl00_MainContent_txtRelevanciaRI').val();

            //INHERENT RISK RELEVANCE VALUE
            if (P1 == 'BAJO') {
                $('#ctl00_MainContent_txtRelevanceValueRI').val('1');
            }
            else {
                if (P1 == 'MEDIO-BAJO') {
                    $('#ctl00_MainContent_txtRelevanceValueRI').val('2');
                }
                else {
                    if (P1 == 'MEDIO') {
                        $('#ctl00_MainContent_txtRelevanceValueRI').val('3');
                    }
                    else {
                        if (P1 == 'MEDIO-ALTO') {
                            $('#ctl00_MainContent_txtRelevanceValueRI').val('4');
                        }
                        else {
                            if (P1 == 'ALTO') {
                                $('#ctl00_MainContent_txtRelevanceValueRI').val('5');
                            }
                            else {
                                $('#ctl00_MainContent_txtRelevanceValueRI').val('-');
                            }
                        }
                    }
                }
            }
        }

        function calcularRR() {
            var I1 = $('#ctl00_MainContent_ddlProbabilidadRR').val();
            if (I1 == 'ALTO') {
                $('#ctl00_MainContent_ddlProbabilidadRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlProbabilidadRR').css('color', 'White');
            }
            if (I1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlProbabilidadRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlProbabilidadRR').css('color', 'Black');
            }
            if (I1 == 'BAJO') {
                $('#ctl00_MainContent_ddlProbabilidadRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlProbabilidadRR').css('color', 'White');
            }
            if (I1 == '0') {
                $('#ctl00_MainContent_ddlProbabilidadRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlProbabilidadRR').css('color', 'Black');
            }

            var J1 = $('#ctl00_MainContent_ddlImpactoObjetivosRR').val();
            if (J1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('color', 'White');
            }
            if (J1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('color', 'Black');
            }
            if (J1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('color', 'White');
            }
            if (J1 == '0') {
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoObjetivosRR').css('color', 'Black');
            }

            var K1 = $('#ctl00_MainContent_ddlImpactoEconomicoRR').val();
            if (K1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('color', 'White');
            }
            if (K1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('color', 'Black');
            }
            if (K1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('color', 'White');
            }
            if (K1 == '0') {
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoEconomicoRR').css('color', 'Black');
            }

            var L1 = $('#ctl00_MainContent_ddlImpactoProcesosRR').val();
            if (L1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('color', 'White');
            }
            if (L1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('color', 'Black');
            }
            if (L1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('color', 'White');
            }
            if (L1 == '0') {
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoProcesosRR').css('color', 'Black');
            }

            var M1 = $('#ctl00_MainContent_ddlImpactoReputacionalRR').val();
            if (M1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('color', 'White');
            }
            if (M1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('color', 'Black');
            }
            if (M1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('color', 'White');
            }
            if (M1 == '0') {
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoReputacionalRR').css('color', 'Black');
            }

            var N1 = $('#ctl00_MainContent_ddlImpactoCumplimientoRR').val();
            if (N1 == 'ALTO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('background-color', 'Red');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('color', 'White');
            }
            if (N1 == 'MEDIO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('background-color', 'Yellow');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('color', 'Black');
            }
            if (N1 == 'BAJO') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('background-color', 'Green');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('color', 'White');
            }
            if (N1 == '0') {
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('background-color', 'White');
                $('#ctl00_MainContent_ddlImpactoCumplimientoRR').css('color', 'Black');
            }

            //IMPACTO GENERAL
            if (J1 == '0' && K1 == '0' && L1 == '0' && M1 == '0' && N1 == '0') {
                $('#ctl00_MainContent_txtImpactoGeneralRR').val('-');
                $('#ctl00_MainContent_txtImpactoGeneralRR').css('background-color', 'White');
                $('#ctl00_MainContent_txtImpactoGeneralRR').css('color', 'Black');
            }
            else {
                if (J1 == 'ALTO' || K1 == 'ALTO' || L1 == 'ALTO' || M1 == 'ALTO' || N1 == 'ALTO') {
                    $('#ctl00_MainContent_txtImpactoGeneralRR').val('ALTO');
                    $('#ctl00_MainContent_txtImpactoGeneralRR').css('background-color', 'Red');
                    $('#ctl00_MainContent_txtImpactoGeneralRR').css('color', 'White');
                }
                else {
                    if (J1 == 'MEDIO' || K1 == 'MEDIO' || L1 == 'MEDIO' || M1 == 'MEDIO' || N1 == 'MEDIO') {
                        $('#ctl00_MainContent_txtImpactoGeneralRR').val('MEDIO');
                        $('#ctl00_MainContent_txtImpactoGeneralRR').css('background-color', 'Yellow');
                        $('#ctl00_MainContent_txtImpactoGeneralRR').css('color', 'Black');
                    }
                    else {
                        $('#ctl00_MainContent_txtImpactoGeneralRR').val('BAJO');
                        $('#ctl00_MainContent_txtImpactoGeneralRR').css('background-color', 'Green');
                        $('#ctl00_MainContent_txtImpactoGeneralRR').css('color', 'White');
                    }
                }
            }

            var O1 = $('#ctl00_MainContent_txtImpactoGeneralRR').val();

            //RELEVANCIA RRESGO INHERENTE
            if (I1 == 'BAJO' && O1 == 'BAJO') {
                $('#ctl00_MainContent_txtRelevanciaRR').val('BAJO');
                $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'Green');
                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumHigh');
                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumLow');
                $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'White');
            }
            else {
                if ((I1 == 'BAJO' && O1 == 'MEDIO') || (I1 == 'MEDIO' && O1 == 'BAJO')) {
                    $('#ctl00_MainContent_txtRelevanciaRR').val('MEDIO-BAJO');
                    $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'Green');
                    document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumHigh');
                    document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.add('GradienteMediumLow');
                    $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'Black');
                }
                else {
                    if ((I1 == 'BAJO' && O1 == 'ALTO') || (I1 == 'MEDIO' && O1 == 'MEDIO') || (I1 == 'ALTO' && O1 == 'BAJO')) {
                        $('#ctl00_MainContent_txtRelevanciaRR').val('MEDIO');
                        $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'Yellow');
                        document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumHigh');
                        document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumLow');
                        $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'Black');
                    }
                    else {
                        if ((I1 == 'ALTO' && O1 == 'MEDIO') || (I1 == 'MEDIO' && O1 == 'ALTO')) {
                            $('#ctl00_MainContent_txtRelevanciaRR').val('MEDIO-ALTO');
                            $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'Red');
                            document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.add('GradienteMediumHigh');
                            document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumLow');
                            $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'Black');
                        }
                        else {
                            if ((I1 == 'ALTO' && O1 == 'ALTO')) {
                                $('#ctl00_MainContent_txtRelevanciaRR').val('ALTO');
                                $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'Red');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumHigh');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumLow');
                                $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'White');
                            }
                            else {
                                $('#ctl00_MainContent_txtRelevanciaRR').val('-');
                                $('#ctl00_MainContent_txtRelevanciaRR').css('background-color', 'White');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumHigh');
                                document.getElementById("ctl00_MainContent_txtRelevanciaRR").classList.remove('GradienteMediumLow');
                                $('#ctl00_MainContent_txtRelevanciaRR').css('color', 'Black');
                            }
                        }
                    }
                }
            }

            var P1 = $('#ctl00_MainContent_txtRelevanciaRR').val();

            //INHERENT RRSK RELEVANCE VALUE
            if (P1 == 'BAJO') {
                $('#ctl00_MainContent_txtRelevanceValueRR').val('1');
            }
            else {
                if (P1 == 'MEDIO-BAJO') {
                    $('#ctl00_MainContent_txtRelevanceValueRR').val('2');
                }
                else {
                    if (P1 == 'MEDIO') {
                        $('#ctl00_MainContent_txtRelevanceValueRR').val('3');
                    }
                    else {
                        if (P1 == 'MEDIO-ALTO') {
                            $('#ctl00_MainContent_txtRelevanceValueRR').val('4');
                        }
                        else {
                            if (P1 == 'ALTO') {
                                $('#ctl00_MainContent_txtRelevanceValueRR').val('5');
                            }
                            else {
                                $('#ctl00_MainContent_txtRelevanceValueRR').val('-');
                            }
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Detalle del Riesgo/Oportunidad<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
				</div>
			</div>
			<!-- /page header -->

			<!-- Form vertical (default) -->
						<form enctype="multipart/form-data" method="post" id="Form1" runat="server">
						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Datos Generales</h6></div>
                                <div class="panel-body">
						 			        <div  class="form-group">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width:5%">
                                                            <label>ID</label>
												            <asp:TextBox ID="txtCod" ReadOnly="true" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td style="width:35%; padding-left:20px">
                                                             <label>
                                                                Riesgo/Oportunidad</label>
                                                            <asp:DropDownList runat="server" ID="ddlTipo" class="form-control" Width="95%">
                                                                <asp:ListItem Value="1" Text="Riesgo"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Oportunidad"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width:20%; padding-left:20px" >
                                                            <label>Cadena de Valor</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlValueChain" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                        <td style="width:20%; padding-left:20px" >
                                                            <label>Macroproceso</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlMacroproceso" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                        <td style="width:20%; padding-left:20px" >
                                                            <label>Proceso</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlProceso" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                    </tr>
                                                </table>
                                                
											</div>

                                            <div  class="form-group">
											<div class="form-group">
												<label>Descripción del Riesgo/Oportunidad</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
											</div>

                                            <div class="form-group">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width:30%; padding-right:10px">
                                                            <label>Categoría</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlCategoria" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                        <td style="width:30%; padding-left:10px">
                                                            <label>Tipología</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlTipologia" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                        <td style="width:15%; padding-left:10px">
                                                            <label>Fecha de Creación</label>
												            <asp:TextBox ID="txtFechaCreacion" style="text-align:center" runat="server" class="datepicker form-control"></asp:TextBox>
                                                        </td>
                                                        <td style="width:15%; padding-left:10px">
                                                            <label>Fecha de Modificación</label>
												            <asp:TextBox ID="txtFechaModificacion" style="text-align:center" runat="server" class="datepicker form-control"></asp:TextBox>
                                                        </td>
                                                        <td style="width:10%; padding-left:10px">
                                                            <label>Vigente</label>
                                                            <asp:DropDownList runat="server" ID="ddlVigente" class="form-control">
                                                                <asp:ListItem Value="0" Text="Sí"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
										</div> 
			                    </div>  
							</div>	           

                            <%--Stakeholders --%>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h6 class="panel-title">
                                        <i class="icon-globe"></i><a name="especifico">Partes interesadas</a></h6>                
                                </div>
                                <div class="panel-body">
                                        <center>
                                    <table id="tablaCentros" runat="server" style="width:100%">
                                        <tr>
                                            <td style="width:45%"> 
                                                <center><label>Partes interesadas a asignar</label></center>
                                            </td>
                                            <td style="width:10%">
                    
                                            </td>
                                            <td style="width:45%"> 
                                                <center><label>Partes interesadas asignadas</label></center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="form-group"> 
                                                <center><asp:ListBox SelectionMode="Multiple" style="width:100%" Rows="10" ID="lstPartesInteresadasAsignar" runat="server">
                                                </asp:ListBox></center>
                                            </td>
                                            <td>
                                            <center>
                                            <% 
                                                if (permisos.permiso == true)
                                                { %>
                                                <input id="btnAsignarParteInteresada" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                                                <input id="btnNoAsignarParteInteresada" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                                                <% } %>
                                                </center>
                                            </td>
                                            <td class="form-group"> 
                                                <center><asp:ListBox SelectionMode="Multiple" style="width:100%" Rows="10" ID="lstPartesInteresadasAsignadas" runat="server">
                                                </asp:ListBox></center>
                                                <asp:HiddenField ID="hdnPartesInteresadasSeleccionadas" runat="server" Value="" />
                                            </td>
                                        </tr>
                                    </table></center>                                     
                                    <br />
                                </div>
                            </div>

                            
                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Evaluación del Riesgo Inherente</h6></div>
                                <div class="panel-body">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:33%">
                                                <label>Probabilidad de ocurrencia</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlProbabilidadRI" Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%">
                                                <label>Impacto sobre los objetivos del negocio</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoObjetivosRI"  Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%">
                                                <label>Impacto económico</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoEconomicoRI" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto sobre los procesos del negocio</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoProcesosRI" Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto reputacional</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoReputacionalRI"  Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto sobre el cumplimiento</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoCumplimientoRI" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>                                      
                                        </tr>
                                    </table>

                                    <table width="100%">
                                        <tr>
                                            <td style="width:25%; padding-top:10px">
                                                <label>Impacto general</label>
												<asp:TextBox ID="txtImpactoGeneralRI" ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                            <td style="width:25%; padding-top:10px">
                                                <label>Relevancia del Riesgo Inherente</label>
												<asp:TextBox ID="txtRelevanciaRI"  ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                            <td style="width:25%; padding-top:10px">
                                                <label>Inherent risk relevance value</label>
												<asp:TextBox ID="txtRelevanceValueRI"  ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>    
                                            <td style="width:25%; padding-top:10px">
                                                <label>Gestión del Riesgo/Oportunidad</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlGestionRiesgoRI" runat="server"> 
                                                                <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                                                <asp:ListItem Value="Aceptar" Text="Aceptar"></asp:ListItem>
                                                                <asp:ListItem Value="Mitigar" Text="Mitigar"></asp:ListItem>
                                                                <asp:ListItem Value="Transferir" Text="Transferir"></asp:ListItem>
                                                                <asp:ListItem Value="Evitar" Text="Evitar"></asp:ListItem>
                                                                <asp:ListItem Value="No Aplica" Text="No Aplica"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>     
                                        </tr>
                                    </table><br />

                                </div>                                            
                            </div>

                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Evaluación de la Oportunidad</h6></div>
                                <div class="panel-body">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:14%">
                                                <center>
                                                <label>Limitación econômica, organizativa y operacional</label><br />
                                                <asp:CheckBox ID="chkLimitOOE" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Limitación operativa y organizacional</label><br />
                                                <asp:CheckBox ID="chkLimitOO" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Limitación económica</label><br /><br />
                                                <asp:CheckBox ID="chkLimitE" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Sin limitaciones</label><br /><br />
                                                <asp:CheckBox ID="chkSinLimit" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Sin Efectos</label><br /><br />
                                                <asp:CheckBox ID="chkSinEfectos" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Mejora desempeño</label><br /><br />
                                                <asp:CheckBox ID="chkEfectD" runat="server" />
                                                </center>
                                            </td>
                                            <td style="width:14%">
                                                <center>
                                                <label>Mejora desempeño e Imagen</label><br /><br />
                                                <asp:CheckBox ID="chkEfectDI" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table width="100%">
                                        <tr>
                                            <td style="width:50%; padding-top:10px">
                                                <label>Valoración Oportunidad</label>
												<asp:TextBox ID="txtValoracionOportunidad" ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                            <td style="width:50%; padding-top:10px">
                                                <label>Evaluación Oportunidad</label>
												<asp:TextBox ID="txtGestionOportunidad"  ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />

                                </div>                                            
                            </div>

                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Gestión del Riesgo/Oportunidad</h6></div>
                                <div class="panel-body">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <label>Descripción del control</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcionControl" runat="server" class="form-control" ></asp:TextBox>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Propietario del control</label>
                                                <asp:TextBox ID="txtPropietarioControl" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>
                                    <br />
                                </div>                                            
                            </div>

                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Evaluación del Riesgo Residual</h6></div>
                                <div class="panel-body">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:33%">
                                                <label>Probabilidad de ocurrencia</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlProbabilidadRR" Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%">
                                                <label>Impacto sobre los objetivos del negocio</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoObjetivosRR"  Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%">
                                                <label>Impacto económico</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoEconomicoRR" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto sobre los procesos del negocio</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoProcesosRR" Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto reputacional</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoReputacionalRR"  Width="95%" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto sobre el cumplimiento</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlImpactoCumplimientoRR" runat="server"> 
                                                                <asp:ListItem Value="0" style="background-color:White; color:Black" Text="-"></asp:ListItem>
                                                                <asp:ListItem Value="BAJO" style="background-color:Green; color:White" Text="BAJO"></asp:ListItem>
                                                                <asp:ListItem Value="MEDIO" style="background-color:Yellow; color:Black" Text="MEDIO"></asp:ListItem>
                                                                <asp:ListItem Value="ALTO" style="background-color:Red; color:White" Text="ALTO"></asp:ListItem>
                                                            </asp:DropDownList>  
                                            </td>                                      
                                        </tr>
                                    </table>

                                    <table width="100%">
                                        <tr>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Impacto general</label>
												<asp:TextBox ID="txtImpactoGeneralRR" ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Relevancia del Riesgo Residual</label>
												<asp:TextBox ID="txtRelevanciaRR"  ReadOnly="true" Width="95%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>
                                            <td style="width:33%; padding-top:10px">
                                                <label>Inherent risk relevance value</label>
												<asp:TextBox ID="txtRelevanceValueRR"  ReadOnly="true" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                                            </td>    
                                        </tr>
                                    </table><br />
                                </div>                                            
                            </div>
                          
                            <div class="form-actions text-right">
                                <asp:Button ID="btnImprimir" runat="server"  class="btn btn-primary run-first" Text="Exportar" />
                              <% if (permisos.permiso == true)
                                  {
                                                      %>                                         
										<input id="GuardarRiesgo" onclick="comprobarPartesInteresadas()" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
                                        <% } %>
                                        <a href="/evr/riesgos/gestion_riesgos" title="Volver" class="btn btn-primary run-first">Volver</a>
           
									</div>
                                    
                         </form>				
						<!-- /form vertical (default) -->	
			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->
</asp:Content>
