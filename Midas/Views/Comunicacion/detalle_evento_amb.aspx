<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.evento_ambiental oEvento = new MIDAS.Models.evento_ambiental();
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


            List<MIDAS.Models.tipocentral> tecno = MIDAS.Models.Datos.ObtenerTecnologiaCentral(centroseleccionado.tipo);

            foreach (MIDAS.Models.tipocentral tipo in tecno)
            {
                ListItem itemTecnologia = new ListItem();

                itemTecnologia.Value = tipo.id.ToString();
                itemTecnologia.Text = tipo.nombre;

                ddlTecnologia.Items.Insert(0, itemTecnologia);
            }
            

            if (ViewData["tiposeventoamb"] != null)
            {
                ddlTipo.DataSource = ViewData["tiposeventoamb"];
                ddlTipo.DataValueField = "id";
                ddlTipo.DataTextField = "tipo";
                ddlTipo.DataBind();
            }

            if (ViewData["documentoseventoamb"] != null)
            {
                grdDocumentos.DataSource = ViewData["documentoseventoamb"];
                grdDocumentos.DataBind();
            }

            if (ViewData["fotoseventoamb"] != null)
            {
                grdFotos.DataSource = ViewData["fotoseventoamb"];
                grdFotos.DataBind();
            }

            Session["ModuloAccionMejora"] = 13;
            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }
            
            if (ViewData["matriceseventoamb"] != null)
            {
                ddlMatrizPrincipal.DataSource = ViewData["matriceseventoamb"];
                ddlMatrizPrincipal.DataValueField = "id";
                ddlMatrizPrincipal.DataTextField = "matriz";
                ddlMatrizPrincipal.DataBind();
            }

            if (ViewData["matriceseventoamb"] != null)
            {
                ddlMatrizSecundaria.DataSource = ViewData["matriceseventoamb"];
                ddlMatrizSecundaria.DataValueField = "id";
                ddlMatrizSecundaria.DataTextField = "matriz";
                ddlMatrizSecundaria.DataBind();
            }

            oEvento = (MIDAS.Models.evento_ambiental)ViewData["eventoambiental"];

            if (oEvento != null)
            {
                Session["idEventoAmb"] = oEvento.id;
                Session["ReferenciaAccionMejora"] = oEvento.id;
                txtFechaEvento.Text = oEvento.fechaevento.ToString().Replace(" 0:00:00", ""); ;
                ddlTipo.SelectedValue = oEvento.tipo.ToString();
                ddlMatrizPrincipal.SelectedValue = oEvento.matrizprincipal.ToString();
                ddlMatrizSecundaria.SelectedValue = oEvento.matrizsecundaria.ToString();
                txtUnidadNegocio.Text = oEvento.unidadnegocio;
                ddlCompInvolucrada.SelectedValue = oEvento.companiainvolucrada.ToString();
                txtEmpContratista.Text = oEvento.empresacontratista;
                
                //Seccion2
                if (oEvento.claseevento_sec2 != null)
                    ddlClaseEvento.SelectedValue = oEvento.claseevento_sec2.ToString();
                txtExtension.Text = oEvento.extension_sec2;
                if (oEvento.impacto_sec2 != null)
                    ddlImpacto.SelectedValue = oEvento.impacto_sec2.ToString();
                txtLocalizacion.Text = oEvento.localizacion_sec2;
                txtDescripcionSec2.Text = oEvento.descripcion_sec2;
                txtCausa.Text = oEvento.causa_sec2;
                txtAccionesInmediatas.Text = oEvento.accionesinmediatas_sec2;
                txtInfoAdicionalSec2.Text = oEvento.infoadicional;
                
                //Seccion3
                txtDescripcionSec3.Text = oEvento.descripcion_sec3;
                txtDemandante.Text = oEvento.demandante_sec3;
                if (oEvento.tipodemantante_sec3 != null)
                    ddlTipoDemandante.SelectedValue = oEvento.tipodemantante_sec3.ToString();
                if (oEvento.tipocriticidad_sec3 != null)
                    ddlCriticidad.SelectedValue = oEvento.tipocriticidad_sec3.ToString();
                txtInfoAdicionalSec3.Text = oEvento.infoadicional;
                
                if (permisos.permiso != true || oEvento.idcentral != centroseleccionado.id)
                    desactivarCampos();
            }
        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                                                

        if (Session["EdicionEventoAmbMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEventoAmbMensaje"].ToString() + "' });", true);
            Session["EdicionEventoAmbMensaje"] = null;
        }
        if (Session["EdicionEventoAmbError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionEventoAmbError"].ToString() + "' });", true);
            Session["EdicionEventoAmbError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtFechaEvento.Enabled = false;
        ddlMatrizPrincipal.Enabled = false;
        ddlMatrizSecundaria.Enabled = false;
        ddlTipo.Enabled = false;
        txtOrganizacion.Enabled = false;
        txtPais.Enabled = false;
        txtUnidadNegocio.Enabled = false;
        ddlCentral.Enabled = false;
        ddlTecnologia.Enabled = false;
        ddlCompInvolucrada.Enabled = false;
        txtCompENEL.Enabled = false;
        txtEmpContratista.Enabled = false;
        //Seccion2
        ddlClaseEvento.Enabled = false;
        txtExtension.Enabled = false;
        ddlImpacto.Enabled = false;
        txtLocalizacion.Enabled = false;
        txtDescripcionSec2.Enabled = false;
        txtCausa.Enabled = false;
        txtAccionesInmediatas.Enabled = false;
        //Seccion3
        txtDescripcionSec3.Enabled = false;
        txtDemandante.Enabled = false;
        ddlTipoDemandante.Enabled = false;
        ddlCriticidad.Enabled = false;
    }
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Evento Ambiental </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();
            var tiposeleccionado = $('#ctl00_MainContent_ddlTipo').val();

            
            if (tiposeleccionado == 1 || tiposeleccionado == 2 || tiposeleccionado == 3) {
                $("#seccion2").show();
                $("#seccion3").hide();
            }
            if (tiposeleccionado == 4 || tiposeleccionado == 5) {
                $("#seccion2").hide();
                $("#seccion3").show();
            }

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarEventoAmb")
                    $("#hdFormularioEjecutado").val("GuardarEventoAmb");
                else $("#hdFormularioEjecutado").val("Recarga");


                if (val == "btnSubirDocumento")
                    $("#hdFormularioEjecutado").val("SubirDocumento");

                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "btnAddDocumento")
                    $("#hdFormularioEjecutado").val("btnAddDocumento");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_ddlTipo").change(function () {
                var tiposeleccionado = $('#ctl00_MainContent_ddlTipo').val();

                if (tiposeleccionado == 1 || tiposeleccionado == 2 || tiposeleccionado == 3) {
                    $("#seccion2").show();
                    $("#seccion3").hide();
                }
                if (tiposeleccionado == 4 || tiposeleccionado == 5) {
                    $("#seccion2").hide();
                    $("#seccion3").show();
                }

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
                Detalle del evento ambiental<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                                Fecha del evento</label>
                                <asp:TextBox ID="txtFechaEvento" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>
                                Tipo de evento</label>
                            <asp:DropDownList ID="ddlTipo" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Matriz Principal</label>
                            <asp:DropDownList ID="ddlMatrizPrincipal" Width="95%" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Matriz Secundaria</label>
                            <asp:DropDownList ID="ddlMatrizSecundaria" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Organización</label>
                                <asp:TextBox ID="txtOrganizacion" ReadOnly="true" Text="O&M: Operación y Mantenimiento" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                País</label>
                                <asp:TextBox ID="txtPais" ReadOnly="true" Text="Iberia" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Unidad de Negocio</label>
                                <asp:TextBox ID="txtUnidadNegocio" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Central/Sede</label>
                                <asp:DropDownList ID="ddlCentral" runat="server" class="form-control">
                                </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Tecnología</label>
                                <asp:DropDownList ID="ddlTecnologia" Width="95%" runat="server" class="form-control">
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Compañia involucrada</label>
                                <asp:DropDownList ID="ddlCompInvolucrada" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="1" Text="Applus"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Contratista"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Compañia Enel</label>
                                <asp:TextBox ID="txtCompENEL" ReadOnly="true" Text="Applus" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Empresa contratista</label>
                                <asp:TextBox ID="txtEmpContratista" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>


    <%--Información adicional incidentes ambientales significativos --%>
    <div id="seccion2" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información adicional Incidentes Ambientales SIgnificativos y Daños ambientales</a></h6>                
        </div>
        <div class="panel-body">
                <center>
            <table style="margin-top: 15px" width="100%">
                    <tr>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Clase de evento</label>
                                <asp:DropDownList ID="ddlClaseEvento" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Derrame de aceite/Oil spill"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Derrame de fuel/Fuel spill"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Derrame sustancia química/Chemicals spill"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Emisión de ruido/Noise emissions"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Vertido de agua/Water emissions"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Emisiones atmosféricas/Air emissions"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Fuego/Fire"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Radiación/Radiation"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Otro/Other"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Extensión, cantidad o volumen</label>
                                <asp:TextBox ID="txtExtension" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Impacto en la opinión pública</label>
                                <asp:DropDownList ID="ddlImpacto" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="No afectado por el evento/Not affected by the event"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Posible interés por las partes interesadas/Possible interest of the stakeholders"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Interés por las partes interesadas/Stakeholder interest"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 25%">
                            <label>
                                Localización del evento</label>
                                <asp:TextBox ID="txtLocalizacion" Width="100%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        
                    </tr>
                </table>
                <table style="margin-top: 15px" width="100%">
                    <tr>
                                            <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Descripción del evento</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcionSec2" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                    <tr>
                                            <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Causa</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtCausa" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                    <tr>
                                            <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Acciones inmediatas</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtAccionesInmediatas" runat="server" class="form-control" ></asp:TextBox>
											</td>
                    </tr>
                    <tr>
                    <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Información adicional</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtInfoAdicionalSec2" runat="server" class="form-control" ></asp:TextBox><br />
											</td>
                                        </tr>
                </table></center>
            <br />
        </div>
    </div>     

    <%--Documentos --%>
    <% if (oEvento != null)
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
                                                          <a title="Ver Fichero" href="/evr/Comunicacion/ObtenerDocEventoAmb/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td> 
                                    <% 
                        if (consulta == 0)
                        { %>
                                    <td class="text-center">
                                        <a href="/evr/Comunicacion/eliminar_doceventoamb/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;"
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

    <%--Información adicional Ligitios y Criticidades --%>
    <div id="seccion3" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Información adicional Litigios y Criticidades</a></h6>                
        </div>
        <div class="panel-body">
                <center>
                <table  width="100%">
                    <tr>
                                            <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Descripción</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcionSec3" runat="server" class="form-control" ></asp:TextBox><br />
											</td>
                                            
                    </tr>
                    <tr>
                        <td class="form-group" style="width: 33%">
                            <label>
                                Demandante</label>
                                <asp:TextBox ID="txtDemandante" Width="95%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td class="form-group" style="width: 33%">
                            <label>
                                Tipo de demandante</label>
                                <asp:DropDownList ID="ddlTipoDemandante" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Público/Public"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Privado/Private"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 33%">
                            <label>
                                Tipo de criticidad</label>
                                <asp:DropDownList ID="ddlCriticidad" Width="95%" runat="server" class="form-control">
                                    <asp:ListItem Value="0" Text="---"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Iniciativa pública/Public"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Iniciativa privada/Private iniciative"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Medida administrativa/Administrative Measure"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Carta de apercibimiento/Warning letter"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Otro/Other"></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                                            <td colspan="3" style="padding-top:10px" class="form-group">
												<label>Información adicional</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtInfoAdicionalSec3" runat="server" class="form-control" ></asp:TextBox><br />
											</td>
                                            
                    </tr>
                    
                   
                </table></center>
            <br />
        </div>
    </div> 

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
                            List<MIDAS.Models.evento_ambiental_foto> listaFotos = (List<MIDAS.Models.evento_ambiental_foto>)ViewData["fotoseventoamb"];
                            if (permisos.permiso == true && oEvento != null && (listaFotos == null || (listaFotos != null && listaFotos.Count < 8)))
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
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar esta foto?')) return false;" href="/evr/Comunicacion/eliminar_fotoeventoamb/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
        <input id="GuardarEventoAmb" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
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
