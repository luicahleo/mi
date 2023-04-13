<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.evento_seguridad oEvento = new MIDAS.Models.evento_seguridad();
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

            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

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

            if (ViewData["tiposeventoseg"] != null)
            {
                ddlTipo.DataSource = ViewData["tiposeventoseg"];
                ddlTipo.DataValueField = "id";
                ddlTipo.DataTextField = "tipo";
                ddlTipo.DataBind();
            }

            if (ViewData["severidadeseventoseg"] != null)
            {
                ddlSeveridad.DataSource = ViewData["severidadeseventoseg"];
                ddlSeveridad.DataValueField = "id";
                ddlSeveridad.DataTextField = "severidad";
                ddlSeveridad.DataBind();
            }

            if (ViewData["tiposeveneventoeventoseg"] != null)
            {
                ddlTipoEvento_te.DataSource = ViewData["tiposeveneventoeventoseg"];
                ddlTipoEvento_te.DataValueField = "id";
                ddlTipoEvento_te.DataTextField = "tipo";
                ddlTipoEvento_te.DataBind();
            }

            if (ViewData["subtiposeveneventoeventoseg"] != null)
            {
                ddlSubtipoEvento_te.DataSource = ViewData["subtiposeveneventoeventoseg"];
                ddlSubtipoEvento_te.DataValueField = "id";
                ddlSubtipoEvento_te.DataTextField = "subtipo";
                ddlSubtipoEvento_te.DataBind();
            }

            if (ViewData["fotoseventoseg"] != null)
            {
                grdFotos.DataSource = ViewData["fotoseventoseg"];
                grdFotos.DataBind();
            }

            Session["ModuloAccionMejora"] = 15;
            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }

            if (ViewData["eventosegdocs"] != null)
            {
                grdDocumentos.DataSource = ViewData["eventosegdocs"];
                grdDocumentos.DataBind();
            }

            oEvento = (MIDAS.Models.evento_seguridad)ViewData["eventoseguridad"];

            if (oEvento != null)
            {
                Session["idEventoSeg"] = oEvento.id;
                Session["ReferenciaAccionMejora"] = oEvento.id;
                
                ddlTipo.SelectedValue = oEvento.tipo.ToString();
                ddlSeveridad.SelectedValue = oEvento.severidad.ToString();
                ddlPersonalAfectado.SelectedValue = oEvento.personalafectado.ToString();
                ddlOrganizacion.SelectedValue = oEvento.organizacion.ToString();
                txtPais.Text = "Iberia";
                if (oEvento.fecha != null)
                    txtFechaEvento.Text = oEvento.fecha.ToString().Replace(" 0:00:00", "");
                if (oEvento.hora != null)
                    txtHora.Text = oEvento.hora.ToString();
                txtUnidadNegocio.Text = oEvento.unidadnegocio;
                ddlCentral.SelectedValue = oEvento.idcentral.ToString();
                txtCompENEL.Text = oEvento.compania;
                if (oEvento.baja != null)
                    ddlBaja.SelectedValue = oEvento.baja.ToString();
                if (oEvento.fechabaja != null)
                    txtFechaBaja.Text = oEvento.fechabaja.ToString().Replace(" 0:00:00", "");
                if (oEvento.horaaccidente != null)
                    txtHoraJornada.Text = oEvento.horaaccidente.ToString();
                
                //Informacion evento
                txtRegion_ie.Text = oEvento.ie_region;
                txtLocalizacion_ie.Text = oEvento.ie_localizacion;
                txtDescripcion_ie.Text = oEvento.ie_descripcion;
                
                //Informacion personal
                txtNombre_pa.Text = oEvento.pa_nombre;
                txtApellido_pa.Text = oEvento.pa_apellido;
                ddlGenero_pa.SelectedValue = oEvento.pa_genero.ToString();
                txtEdad_pa.Text = oEvento.pa_edad.ToString();
                txtPuesto_pa.Text = oEvento.pa_puesto;
                txtNacionalidad_pa.Text = oEvento.pa_nacionalidad;
                txtAntiguedadEmpresa.Text = oEvento.pa_antiguedadempresa;
                txtAntiguedadCategoria.Text = oEvento.pa_antiguedadcategoria;
                if (oEvento.pa_tipocontrato != null)
                    ddlContrato.SelectedValue = oEvento.pa_tipocontrato.ToString();
                if (oEvento.pa_accidentesant != null)
                    ddlAccidentesAnt.SelectedValue = oEvento.pa_accidentesant.ToString();
                if (oEvento.pa_numacc != null)
                    txtNumAccidentes.Text = oEvento.pa_numacc.ToString();
                if (oEvento.pa_diasbaja != null)
                    txtDiasBaja.Text = oEvento.pa_diasbaja.ToString();
                if (oEvento.pa_fechamodformacion != null)
                    txtFechaBaja.Text = oEvento.pa_fechamodformacion.ToString().Replace(" 0:00:00", "");
                txtNombreUltimaForm.Text = oEvento.pa_nombreultformacion;
                
                //Informacion daño
                txtInformeMedico_id.Text = oEvento.id_informemedico;
                txtDiasConvalescencia_id.Text = oEvento.id_diasconv_primerpron;
                if (oEvento.id_reqasistencia != null)
                    ddlAsistenciaENDESA.SelectedValue = oEvento.id_reqasistencia.ToString();
                txtAsistenciaEn.Text = oEvento.id_asistenciaen;
                txtPersonalSanitario.Text = oEvento.id_personalsanitario;
                txtNaturalezaLesion.Text = oEvento.id_naturalezalesion;
                txtLocalizacionAnatomica.Text = oEvento.id_localizacionanatomica;
                txtAgenteLesion.Text = oEvento.id_agentelesion;
                if (oEvento.id_envmutua != null)
                    ddlEnvioMutua.SelectedValue = oEvento.id_envmutua.ToString();
                txtNombreMutua.Text = oEvento.id_mutua;
                txtLocalidadMutua.Text = oEvento.id_localidadmutua;
                if (oEvento.id_envcs != null)
                    ddlEnvioCS.SelectedValue = oEvento.id_envcs.ToString();
                txtNombreCS.Text = oEvento.id_centrosanitario;
                txtLocalidadCS.Text = oEvento.id_localidadcs;
                txtMandoDirecto.Text = oEvento.id_mandodirecto;
                txtTestigo1.Text = oEvento.id_testigo1;
                txtTestigo2.Text = oEvento.id_testigo2;
                txtTestigo3.Text = oEvento.id_testigo3;
                
                //Tipo Evento
                ddlTipoEvento_te.SelectedValue = oEvento.te_tipo.ToString();
                ddlSubtipoEvento_te.SelectedValue = oEvento.te_subtipo.ToString();
                ddlCategorizacion_te.SelectedValue = oEvento.te_categorizacion.ToString();
                txtCausa_te.Text = oEvento.te_causa;
                txtAccionesInm_te.Text = oEvento.te_accionesinm;
                
                //Información Contratista
                txtNombreEmpresa_ic.Text = oEvento.ic_nombreempresa;
                txtActividad_ic.Text = oEvento.ic_actividad;
                txtPersonaRef_ic.Text = oEvento.ic_personaref;
                txtTelefono_ic.Text = oEvento.ic_telefono;
                txtEmail_ic.Text = oEvento.ic_email;
                if (oEvento.ic_empcontratista != null)
                    ddlContratista_ic.SelectedValue = oEvento.ic_empcontratista.ToString();
                txtContratistaPrincipal_ic.Text = oEvento.ic_subcontrata;
                txtPersonalSanitario_ic.Text = oEvento.ic_personalsanit;
                txtDomicilio_ic.Text = oEvento.ic_domicilio;
                txtCIF_ic.Text = oEvento.ic_cif;
                txtLocalidad_ic.Text = oEvento.ic_localidad;
                
                //Información Adicional
                txtHorario_ia.Text = oEvento.ia_horario;
                txtDesde_ia.Text = oEvento.ia_desde;
                txtHacia_ia.Text = oEvento.ia_hacia;
                txtLugar_ia.Text = oEvento.ia_lugar;
                txtMedio_ia.Text = oEvento.ia_medio;
                txtPropiedad_ia.Text = oEvento.ia_propiedadmediotransporte;
                txtCausa_ia.Text = oEvento.ia_causa;

                if (permisos.permiso != true || oEvento.idcentral != centroseleccionado.id)
                    desactivarCampos();

            }

        }


            txtNombre_pa.Enabled = false;
            txtApellido_pa.Enabled = false;

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }


        if (Session["EdicionEventoSegMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEventoSegMensaje"].ToString() + "' });", true);
            Session["EdicionEventoSegMensaje"] = null;
        }
        if (Session["EdicionEventoSegError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionEventoSegError"].ToString() + "' });", true);
            Session["EdicionEventoSegError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        ddlTipo.Enabled = false;
        ddlSeveridad.Enabled = false;
        ddlPersonalAfectado.Enabled = false;
        ddlOrganizacion.Enabled = false;
        txtPais.Enabled = false;
        txtFechaEvento.Enabled = false;
        txtHora.Enabled = false;
        txtUnidadNegocio.Enabled = false;
        ddlCentral.Enabled = false;
        txtCompENEL.Enabled = false;
        ddlBaja.Enabled = false;
        txtFechaBaja.Enabled = false;
        txtHoraJornada.Enabled = false;

        //Informacion evento
        txtRegion_ie.Enabled = false;
        txtLocalizacion_ie.Enabled = false;
        txtDescripcion_ie.Enabled = false;

        //Informacion personal
        txtNombre_pa.Enabled = false;
        txtApellido_pa.Enabled = false;
        ddlGenero_pa.Enabled = false;
        txtEdad_pa.Enabled = false;
        txtPuesto_pa.Enabled = false;
        txtNacionalidad_pa.Enabled = false;
        txtAntiguedadEmpresa.Enabled = false;
        txtAntiguedadCategoria.Enabled = false;
        ddlContrato.Enabled = false;
        ddlAccidentesAnt.Enabled = false;
        txtNumAccidentes.Enabled = false;
        txtDiasBaja.Enabled = false;
        txtFechaUltimoModulo.Enabled = false;
        txtNombreUltimaForm.Enabled = false;

        //Informacion daño
        txtInformeMedico_id.Enabled = false;
        txtDiasConvalescencia_id.Enabled = false;
        ddlAsistenciaENDESA.Enabled = false;
        txtAsistenciaEn.Enabled = false;
        txtPersonalSanitario.Enabled = false;
        txtNaturalezaLesion.Enabled = false;
        txtLocalizacionAnatomica.Enabled = false;
        txtAgenteLesion.Enabled = false;
        ddlEnvioMutua.Enabled = false;
        txtNombreMutua.Enabled = false;
        txtLocalidadMutua.Enabled = false;
        ddlEnvioCS.Enabled = false;
        txtNombreCS.Enabled = false;
        txtLocalidadCS.Enabled = false;
        txtMandoDirecto.Enabled = false;
        txtTestigo1.Enabled = false;
        txtTestigo2.Enabled = false;
        txtTestigo3.Enabled = false;

        //Tipo Evento
        ddlTipoEvento_te.Enabled = false;
        ddlSubtipoEvento_te.Enabled = false;
        ddlCategorizacion_te.Enabled = false;
        txtCausa_te.Enabled = false;
        txtAccionesInm_te.Enabled = false;

        //Información Contratista
        txtNombreEmpresa_ic.Enabled = false;
        txtActividad_ic.Enabled = false;
        txtPersonaRef_ic.Enabled = false;
        txtTelefono_ic.Enabled = false;
        txtEmail_ic.Enabled = false;
        ddlContratista_ic.Enabled = false;
        txtContratistaPrincipal_ic.Enabled = false;
        txtPersonalSanitario_ic.Enabled = false;
        txtDomicilio_ic.Enabled = false;
        txtCIF_ic.Enabled = false;
        txtLocalidad_ic.Enabled = false;

        //Información Adicional
        txtHorario_ia.Enabled = false;
        txtDesde_ia.Enabled = false;
        txtHacia_ia.Enabled = false;
        txtLugar_ia.Enabled = false;
        txtMedio_ia.Enabled = false;
        txtPropiedad_ia.Enabled = false;
        txtCausa_ia.Enabled = false;
    }
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Evento Seguridad </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                switch (val) {
                    case "GuardarEventoSeg":
                        $("#hdFormularioEjecutado").val("GuardarEventoSeg");
                        break;
                    case "btnSubirDocumento":
                        $("#hdFormularioEjecutado").val("SubirDocumento");
                        break;
                    case "btnSubirArchivo":
                        $("#hdFormularioEjecutado").val("SubirArchivo");
                        break;
                    case "ctl00_MainContent_btnImprimirReg":
                        $("#hdFormularioEjecutado").val("btnImprimirReg");
                        break;
                    case "ctl00_MainContent_btnImprimir":
                        $("#hdFormularioEjecutado").val("btnImprimir");
                        break;
                    default:
                        $("#hdFormularioEjecutado").val("Recarga");
                        break;
                }

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
                Detalle del evento de seguridad/salud<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                <i class="icon-pencil"></i>Información General</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Tipo de evento</label>
                            <asp:DropDownList ID="ddlTipo" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Severidad</label>
                            <asp:DropDownList ID="ddlSeveridad" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Personal afectado</label>
                            <asp:DropDownList ID="ddlPersonalAfectado" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value = "1" Text="Empleado Enel"></asp:ListItem>
                                <asp:ListItem Value = "2" Text="Empleado Contratista"></asp:ListItem>
                                <asp:ListItem Value = "3" Text="Tercera Parte"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Organizacion</label>
                            <asp:DropDownList ID="ddlOrganizacion" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value = "1" Text="O&M: Operación y Mantenimiento"></asp:ListItem>
                                <asp:ListItem Value = "2" Text="E&C: Ingeniería y Construcción"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-group" style="width: 25%; padding-top:15px">
                            <label>
                                País</label>
                                <asp:TextBox ID="txtPais" ReadOnly="true" Text="Iberia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%; padding-top:15px">
                            <label>
                                Fecha del evento</label>
                                <asp:TextBox ID="txtFechaEvento" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        
                        <td class="form-group" style="width: 25%; padding-top:15px">
                            <label>
                                Hora</label>
                            <asp:TextBox ID="txtHora" Width="95%" TextMode="Time" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%; padding-top:15px">
                            <label>
                                Unidad de negocio</label>
                                <asp:TextBox ID="txtUnidadNegocio" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top:15px">
                            <label>
                                Central</label>
                            <asp:DropDownList ID="ddlCentral" Width="97%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>

                        <td colspan="2" style="padding-top:15px">
                            <label>
                                    Compañía ENEL</label>
                                    <asp:TextBox ID="txtCompENEL" Width="97%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>            
                <br />
                <label>Otra información relevante:</label>
                <br />
                <table width="60%">
                    <tr>
                        <td class="form-group" style="width:10%; padding-top:15px">
                            <label>
                                ¿Tiene baja?</label>
                            <asp:DropDownList ID="ddlBaja" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width:10%; padding-top:15px"">
                            <label>
                                Fecha baja</label>
                                <asp:TextBox ID="txtFechaBaja" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td  class="form-group" style="width:25%; padding-top:15px">
                            <label>
                                Hora de jornada de trabajo en la que se produjo el accidente</label>
                            <asp:TextBox ID="txtHoraJornada" Width="20%" TextMode="Time" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>


    <% if (oEvento != null)
       { %>
    <%--Información del evento --%>
    <div id="seccion2" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información del evento</a></h6>                
        </div>
        <div class="panel-body">
                <center>
            <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Región</label>
                                <asp:TextBox ID="txtRegion_ie" Width="97%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Localización</label>
                                <asp:TextBox ID="txtLocalizacion_ie" Width="97%" runat="server" class="form-control"></asp:TextBox>
                        </td>                      
                        
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top:15px">
                            <label>
                                Descripción del evento</label>
                                <asp:TextBox ID="txtDescripcion_ie" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </center>
            <br />
        </div>
    </div>     

    <%--Información personal --%>
    <div id="seccion3" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información del personal afectado</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                          <td class="form-group" style="width:33%">
                            <label>
                                Nombre</label>
                                <asp:TextBox ID="txtNombre_pa" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                 
                          <td class="form-group" style="width:33%">
                            <label>
                                Apellido</label>
                                <asp:TextBox ID="txtApellido_pa" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%">
                            <label>
                                Género</label>
                                <asp:DropDownList ID="ddlGenero_pa" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value = "1" Text="Masculino/Male"></asp:ListItem>
                                <asp:ListItem Value = "2" Text="Femenino/Female"></asp:ListItem>
                            </asp:DropDownList>
                          </td>                     
                    </tr>
                    <tr>
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Edad</label>
                                <asp:TextBox ID="txtEdad_pa" Width="95%" TextMode="Number" runat="server" class="form-control"></asp:TextBox>
                          </td>                 
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Puesto</label>
                                <asp:TextBox ID="txtPuesto_pa" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Nacionalidad</label>
                                <asp:TextBox ID="txtNacionalidad_pa" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                     
                    </tr> 
                </table>
                </center>
                <br />
                    <label>Otra información relevante:</label>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Antigüedad en la empresa</label>
                                <asp:TextBox ID="txtAntiguedadEmpresa" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Antigüedad en la categoría</label>
                                <asp:TextBox ID="txtAntiguedadCategoria" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Contrato de trabajo</label>
                                <asp:DropDownList ID="ddlContrato" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="Fijo"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Temporal"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    </tr>
                   <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                ¿Tuvo accidentes anteriormente?</label>
                                <asp:DropDownList ID="ddlAccidentesAnt" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <table>
                                <tr>
                                    <td style="width:50%">
                                        <label>
                                        ¿Cuántos?</label>
                                        <asp:TextBox ID="txtNumAccidentes" Width="95%" TextMode="Number" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td style="width:50%">
                                        <label>
                                        ¿Días de baja?</label>
                                        <asp:TextBox ID="txtDiasBaja" Width="95%" TextMode="Number" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                        </td>
                   </tr>
                   <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Fecha último módulo formación recibido</label>
                                <asp:TextBox ID="txtFechaUltimoModulo" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Indicar nombre de última formación recibida</label>
                                <asp:TextBox ID="txtNombreUltimaForm" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                        </td>
                   </tr>
                </table>
                
                
            <br />
        </div>
    </div> 

    <%--Información daño --%>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información del daño</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                          <td class="form-group" style="width:100%">
                            <label>
                                Informe médico</label>
                                <asp:TextBox ID="txtInformeMedico_id" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                          </td>                                
                    </tr>
                    <tr>
                          <td class="form-group" style="width:100%; padding-top:15px">
                            <label>
                                Días convalescencia/Primer pronóstico</label>
                                <asp:TextBox ID="txtDiasConvalescencia_id" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                          </td>                                   
                    </tr>                  
                   
                </table></center>
                <br />
                    <label>Otra información relevante:</label>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                ¿Requiere asistencia sanitaria inicial en Applus?</label>
                                <asp:DropDownList ID="ddlAsistenciaENDESA" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                </asp:DropDownList>
                                
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Asistencia en:</label>
                                <asp:TextBox ID="txtAsistenciaEn" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Personal sanitario que lo atendió</label>
                                <asp:TextBox ID="txtPersonalSanitario" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Naturaleza de la lesión</label>
                                <asp:TextBox ID="txtNaturalezaLesion" Width="95%" runat="server" class="form-control"></asp:TextBox>
                                
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Localización anatómica</label>
                                <asp:TextBox ID="txtLocalizacionAnatomica" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Agente de la lesión</label>
                                <asp:TextBox ID="txtAgenteLesion" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                ¿Enviado a mutua?</label>
                                <asp:DropDownList ID="ddlEnvioMutua" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                </asp:DropDownList>
                                
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Nombre de la mutua</label>
                                <asp:TextBox ID="txtNombreMutua" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Localidad</label>
                                <asp:TextBox ID="txtLocalidadMutua" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                ¿Enviado a Centro Sanitario?</label>
                                <asp:DropDownList ID="ddlEnvioCS" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                </asp:DropDownList>
                                
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Nombre del centro sanitario</label>
                                <asp:TextBox ID="txtNombreCS" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Localidad</label>
                                <asp:TextBox ID="txtLocalidadCS" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>

                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width:25%; padding-top:15px">
                            <label>
                                Mando directo del accidentado</label>
                                <asp:TextBox ID="txtMandoDirecto" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:25%; padding-top:15px">
                            <label>
                                Testigo 1</label>
                                <asp:TextBox ID="txtTestigo1" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:25%; padding-top:15px">
                            <label>
                                Testigo 2</label>
                                <asp:TextBox ID="txtTestigo2" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width:25%; padding-top:15px">
                            <label>
                                Testigo 3</label>
                                <asp:TextBox ID="txtTestigo3" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            <br />
        </div>
    </div> 

    <%--Tipo evento --%>
    <div id="Div2" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Tipo de evento, primer análisis de causas y acciones inmediatas</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                          <td class="form-group" style="width:33%">
                            <label>
                                Tipo</label>
                            <asp:DropDownList ID="ddlTipoEvento_te" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                          </td>   
                          <td class="form-group" style="width:33%">
                            <label>
                                Subtipo</label>
                            <asp:DropDownList ID="ddlSubtipoEvento_te" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                          </td> 
                          <td class="form-group" style="width:33%">
                            <label>
                                Categorización</label>
                            <asp:DropDownList ID="ddlCategorizacion_te" Width="95%" runat="server" class="form-control">
                                <asp:ListItem Value="1" Text="Estructuras"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Comportamientos"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Organización"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Otros factores"></asp:ListItem>
                            </asp:DropDownList>
                          </td>                              
                    </tr>
                    <tr>
                          <td colspan="3" class="form-group" style="width:100%; padding-top:15px">
                            <label>
                                Causa</label>
                                <asp:TextBox ID="txtCausa_te" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                          </td>                                   
                    </tr>                      
                    <tr>
                          <td colspan="3" class="form-group" style="width:100%; padding-top:15px">
                            <label>
                                Acciones inmediatas</label>
                                <asp:TextBox ID="txtAccionesInm_te" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                          </td>                                   
                    </tr>                   
                   
                </table></center>
            <br />
        </div>
    </div> 

    <%--Información contratista --%>
    <div id="Div3" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información contratista (sólo para eventos del Contratista)</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                          <td class="form-group" style="width:50%">
                            <label>
                                Nombre de la empresa</label>
                            <asp:TextBox ID="txtNombreEmpresa_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:50%">
                            <label>
                                Actividad</label>
                            <asp:TextBox ID="txtActividad_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                         
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Persona de referencia</label>
                            <asp:TextBox ID="txtPersonaRef_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Teléfono</label>
                            <asp:TextBox ID="txtTelefono_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Email</label>
                            <asp:TextBox ID="txtEmail_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                               
                    </tr>                   
                   
                </table>
                </center>
                <br />
                <label>Otra información relevante:</label><br />
                <center>
                <table width="100%">
                    <tr>
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                ¿Empresa contratista principal</label>
                                <asp:DropDownList ID="ddlContratista_ic" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="Si"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                En caso de subcontrata, indicar contratista principal</label>
                            <asp:TextBox ID="txtContratistaPrincipal_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Personal Sanitario que lo atendió</label>
                            <asp:TextBox ID="txtPersonalSanitario_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                               
                    </tr>                   
                    <tr>
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Domicilio de la empresa</label>
                                <asp:TextBox ID="txtDomicilio_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                CIF de la empresa</label>
                            <asp:TextBox ID="txtCIF_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Localidad</label>
                            <asp:TextBox ID="txtLocalidad_ic" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                               
                    </tr>  
                </table>

                </center>
            <br />
        </div>
    </div>
   

   <%--Información adicional --%>
    <div id="Div4" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información adicional (sólo para accidentes in itinere)</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                          <td class="form-group" style="width:33%">
                            <label>
                                Horario habitual de trabajo</label>
                            <asp:TextBox ID="txtHorario_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%">
                            <label>
                                Desplazamiento desde</label>
                            <asp:TextBox ID="txtDesde_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%">
                            <label>
                                Desplazamiento hacia</label>
                            <asp:TextBox ID="txtHacia_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                             
                    </tr>
                    <tr>
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Lugar del accidente</label>
                            <asp:TextBox ID="txtLugar_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Medio de transporte</label>
                            <asp:TextBox ID="txtMedio_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>   
                          <td class="form-group" style="width:33%; padding-top:15px">
                            <label>
                                Propiedad del medio de transporte</label>
                            <asp:TextBox ID="txtPropiedad_ia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                          </td>                               
                    </tr>    
                    <tr>
                        <td colspan="3" style="padding-top:15px">
                            <label>
                                Causa</label>
                                <asp:TextBox ID="txtCausa_ia" Rows="4" TextMode="MultiLine" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>          
                   
                </table></center>
            <br />
        </div>
    </div>

    <% } %>


    <% if (oEvento != null)
       { %>

    <div id="divFotos" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-camera"></i><a name="especifico">Fotos adjuntas</a></h6>                
        </div>
        <div class="panel-body">
            <br />
                        <% 
                            List<MIDAS.Models.evento_seguridad_foto> listaFotos = (List<MIDAS.Models.evento_seguridad_foto>)ViewData["fotoseventoseg"];
                            if (permisos.permiso == true && oEvento != null && (listaFotos == null || (listaFotos != null && listaFotos.Count<8)))
                           { %>
                            <table width="80%">
                                <tr>
                                    <td style="width:60%" class="form-group">
                                    <label>
                                        Título</label>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                    <label>
                                        Fichero</label>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-group">
                                        <asp:TextBox ID="txtNombreFichero" CssClass="form-control"
                                                    runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                        <input type="file" id="file1" name="file" />
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                        <input id="btnSubirDocumento" type="submit" value="Subir documento" class="btn btn-primary run-first"/>  
                                    </td>
                                </tr>
                            </table>
                        
                            <br />
                            <% } %>

                            <asp:GridView ID="grdFotos" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Titulo      
                                                                            </th>  
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdFotos.Rows)
                                                                        { %>
                                                                        <tr>         
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>                                                                             
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar esta foto?')) return false;" href="/evr/Comunicacion/eliminar_fotoeventoseg/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
        </div>
    </div>
    <% } %>

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
                                                                            <% if (user.perfil == 1)
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

            <%--Documentos --%>
        <% if (oEvento != null)
       { %>

    <div id="divArchivos" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-file"></i><a name="especifico">Documentos relacionados</a></h6>                
        </div>
        <div class="panel-body">
            <br />
                        <% 
                            List<MIDAS.Models.evento_seguridad_documentos> documentos = (List<MIDAS.Models.evento_seguridad_documentos>)ViewData["eventosegdocs"];
                            if (permisos.permiso == true && oEvento != null && (documentos == null || (documentos != null && documentos.Count<8)))
                           { %>
                            <table width="80%">
                                <tr>
                                    <td style="width:60%" class="form-group">
                                    <label>
                                        Título</label>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                    <label>
                                        Fichero</label>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-group">
                                        <asp:TextBox ID="documentoName" CssClass="form-control"
                                                    runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                        <input type="file" id="file" name="file" />
                                    </td>
                                    <td class="form-group" style="padding-left:15px">
                                        <input id="btnSubirArchivo" type="submit" value="Subir documento" class="btn btn-primary run-first"/>  
                                    </td>
                                </tr>
                            </table>
                        
                            <br />
                            <% } %>

                            <asp:GridView ID="grdDocumentos" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Titulo      
                                                                            </th>  
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdDocumentos.Rows)
                                                                        { %>
                                                                        <tr>         
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>                                                                             
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar esta foto?')) return false;" href="/evr/Comunicacion/eliminar_docseg/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
        </div>
    </div>
    <% } %>

    <div class="form-actions text-right">
        <%
            if (oEvento != null)
            {
        %>
        <asp:Button ID="btnImprimirReg" runat="server" class="btn btn-primary run-first" Text="Imprimir Registro" />
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <% 
                        if (consulta == 0)
                        { %>
        <input id="GuardarEventoSeg" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
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
