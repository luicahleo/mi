<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros oCentro;
    MIDAS.Models.areanivel1 oArea;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
        }
        oCentro = (MIDAS.Models.centros)ViewData["EditarCentro"];

        if (oCentro != null)
        {
            hdnIdCentral.Value = oCentro.id.ToString();
        }

        if (Session["EditarAreaResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAreaResultado"].ToString() + "' });", true);
            Session.Remove("EditarAreaResultado");
        }

        if (Session["EditarAreaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAreaError"].ToString() + "' });", true);
            Session.Remove("EditarAreaError");
        }

        //if (ViewData["ubicaciones"] != null)
        //{
        //    ddlProvincia.DataSource = ViewData["ubicaciones"];
        //    ddlProvincia.DataValueField = "id_provincia";
        //    ddlProvincia.DataTextField = "nombre_provincia";
        //    ddlProvincia.DataBind();
        //}

        if (ViewData["unidades"] != null)
        {
            ddlTipo.DataSource = ViewData["unidades"];
            ddlTipo.DataValueField = "id";
            ddlTipo.DataTextField = "nombre";
            ddlTipo.DataBind();
        }

        // Insercción código - Rafael Ortega - 26/07/2022
        if (ViewData["agrupaciones"] != null)
        {
            ddlAgrupacion.DataSource = ViewData["agrupaciones"];
            ddlAgrupacion.DataValueField = "id";
            ddlAgrupacion.DataTextField = "nombre";
            ddlAgrupacion.DataBind();
        }

        if (ViewData["zonas"] != null)
        {
            ddlZonas.DataSource = ViewData["zonas"];
            ddlZonas.DataValueField = "id";
            ddlZonas.DataTextField = "nombre";
            ddlZonas.DataBind();
        }

        //Doget
        if (!IsPostBack)
        {
            oCentro = (MIDAS.Models.centros)ViewData["EditarCentro"];

            if (oCentro != null)
            {
                hdnIdCentral.Value = oCentro.id.ToString();
                //txtSiglas.Text = oCentro.siglas.ToString();
                txtNombre.Text = oCentro.nombre.ToString();
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                //ddlProvincia.SelectedValue = oCentro.provincia.ToString();

                if (oCentro.direccion != null) direccion.Text = oCentro.direccion.ToString();
                if (oCentro.coordenadas != null) coordenadas.Text = oCentro.coordenadas.ToString();

                List<MIDAS.Models.VISTA_ListaCentroTecnologia> eTecnologias = (List<MIDAS.Models.VISTA_ListaCentroTecnologia>)ViewData["VISTAListaCentroTecnologia"];
                List<MIDAS.Models.VISTA_ListaCentroZona> eZonas = (List<MIDAS.Models.VISTA_ListaCentroZona>)ViewData["VistaListarCentrosZonas"];
                List<MIDAS.Models.VISTA_ListaCentroZonaAgrupacion> eAgrupaciones = (List<MIDAS.Models.VISTA_ListaCentroZonaAgrupacion>)ViewData["VISTAListaCentroZonaAgrupacion"];

                ddlTipo.SelectedValue = (eTecnologias.Where(x => x.id_centro == oCentro.id).Select(x => x.id_tecnologia).FirstOrDefault()).ToString();

                var zonaDato = (eZonas.Where(x => x.id_centro == oCentro.id).Select(x => x.id_zona).FirstOrDefault()).ToString();
                if (zonaDato != "")
                {
                    ddlZonas.SelectedValue = zonaDato;
                    panelDdlZonas.CssClass = "visibleOn";

                }
                else
                {
                    RequiredFieldValidatorZona.Enabled = false;
                }
                var agrupacionDato = (eAgrupaciones.Where(x => x.id_centro == oCentro.id).Select(x => x.codigo).FirstOrDefault()).ToString();
                if (agrupacionDato != "")
                {
                    ddlAgrupacion.SelectedValue = agrupacionDato;
                    panelDdlAgrupacion.CssClass = panelDdlZonas.CssClass.Replace("visibleOff", "visibleOn");

                }
                else
                {
                    RequiredFieldValidatorAgrupacion.Enabled = false;
                }

                if (oCentro.rutaImagen != null && oCentro.rutaImagen != "")
                {
                    imagenPrevisualizacionIC.ImageUrl = oCentro.rutaImagen.Replace("..", "~");
                }
                if (oCentro.rutaImagenLogo != null && oCentro.rutaImagenLogo != "")
                {
                    imagenPrevisualizacionIL.ImageUrl = oCentro.rutaImagenLogo.Replace("..", "~");
                }


                ViewData["idCentro"] = oCentro.id;

                GuardarUsuario.Disabled = false;
            }

            if (Session["CentroErroneo"] != null)
            {
                oCentro = (MIDAS.Models.centros)Session["CentroErroneo"];
                hdnIdCentral.Value = oCentro.id.ToString();
                txtNombre.Text = oCentro.nombre.ToString();
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                //ddlProvincia.SelectedValue = oCentro.ubicacion.ToString();

                Session.Remove("CentroErroneo");
            }

            if (Session["ErrorForm"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["ErrorForm"].ToString() + "' });", true);
                Session["ErrorForm"] = null;
            }

            if (Session["RepoblarCampos"] != null)
            {

                var rDatos = (FormCollection)Session["RepoblarCampos"];

                txtNombre.Text = rDatos["ctl00$MainContent$txtNombre"].ToString();
                //ddlTipo.SelectedIndex = int.Parse(rDatos["ctl00$MainContent$ddlTipo"].ToString());
                ddlTipo.SelectedValue = rDatos["ctl00$MainContent$ddlTipo"].ToString();
                //ddlProvincia.SelectedValue = rDatos["ctl00$MainContent$ddlProvincia"].ToString();
                direccion.Text = rDatos["ctl00$MainContent$direccion"].ToString();
                coordenadas.Text = rDatos["ctl00$MainContent$coordenadas"].ToString();

                if (rDatos["ctl00$MainContent$ddlTipo"] == "7" || rDatos["ctl00$MainContent$ddlTipo"] == "8" || rDatos["ctl00$MainContent$ddlTipo"] == "9")
                {
                    if (rDatos["ctl00$MainContent$ddlZonas"] != "0" && rDatos["ctl00$MainContent$ddlZonas"] != null)
                    {
                        if (ViewData["listadoZonas"] != null)
                        {
                            ddlZonas.DataSource = ViewData["listadoZonas"];
                            ddlZonas.DataValueField = "id";
                            ddlZonas.DataTextField = "nombre";
                            ddlZonas.DataBind();
                        }

                        ddlZonas.SelectedValue = rDatos["ctl00$MainContent$ddlZonas"].ToString();
                        panelDdlZonas.CssClass = "visibleOn";
                    }

                    if (rDatos["ctl00$MainContent$ddlAgrupacion"] != "0" && rDatos["ctl00$MainContent$ddlAgrupacion"] != null)
                    {
                        if (ViewData["listadoAgrupacion"] != null)
                        {
                            ddlAgrupacion.DataSource = ViewData["listadoAgrupacion"];
                            ddlAgrupacion.DataValueField = "id";
                            ddlAgrupacion.DataTextField = "nombre";
                            ddlAgrupacion.DataBind();
                        }

                        ddlAgrupacion.SelectedValue = rDatos["ctl00$MainContent$ddlAgrupacion"].ToString();
                        panelDdlAgrupacion.CssClass = "visibleOn";
                    }
                }
                Session.Remove("RepoblarCampos");
            }
        }
    }
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>

</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/editar_centro.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/editar_centro.css") %>" rel="stylesheet" />

    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form id="form2" runat="server" enctype="multipart/form-data">
        <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-office"></i>Datos de central</h6>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <asp:HiddenField runat="server" ID="hdnIdCentral" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hdnIdArea" />
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    NOMBRE: <span style="color: red;">*</span>
                                </label>
                                <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" ErrorMessage="Debe ingresar un nombre para la Instalación" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                                <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator6" runat="server"
                                    ControlToValidate="txtNombre" ErrorMessage="El texto no puede contener caracteres especiales como son:  \ / * : ? ¿  < > |"
                                    ValidationExpression="[a-zA-Z0-9áéíóúÁÉÍÓÚ._ ]*">
                                </asp:RegularExpressionValidator>

                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    TECNOLOGÍAS: <span style="color: red;">*</span></label>
                                <asp:DropDownList ID="ddlTipo" CssClass="form-control" runat="server" AppendDataBoundItems="true" ClientIDMode="Static" onchange="seleccionTecnologia()">
                                    <asp:ListItem Value="0">&lt;Seleccione una tecnologia&gt;</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true"
                                    InitialValue="0" ControlToValidate="ddlTipo" ErrorMessage="Debe seleccionar una tecnología"
                                    ForeColor="Red" />

                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    DIRECCION:</label>&nbsp;
                    <asp:TextBox ID="direccion" CssClass="form-control" runat="server">
                    </asp:TextBox>
                            </div>
                        </td>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    COORDENADAS:</label>&nbsp;
                    <asp:TextBox ID="coordenadas" CssClass="form-control" runat="server">
                    </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 15px">
                            <asp:Panel class="form-group visibleOff" runat="server" ID="panelDdlZonas" ClientIDMode="Static">
                                <label>
                                    ZONA: <span style="color: red;">*</span></label>&nbsp;
                                <asp:DropDownList ID="ddlZonas" CssClass="form-control" runat="server" onchange="seleccionZona()" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorZona" runat="server" SetFocusOnError="true"
                                    InitialValue="0" ControlToValidate="ddlZonas" ErrorMessage="Debe seleccionar una zona"
                                    ForeColor="Red" ClientIDMode="Static" CssClass="ocultar" />
                            </asp:Panel>
                        </td>
                        <td style="padding-right: 15px">
                            <asp:Panel ID="panelDdlAgrupacion" class="form-group visibleOff" runat="server" ClientIDMode="Static">
                                <label>
                                    AGRUPACIÓN: <span style="color: red;">*</span></label>&nbsp;
                                <asp:DropDownList ID="ddlAgrupacion" CssClass="form-control " runat="server" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAgrupacion" runat="server" SetFocusOnError="true"
                                    InitialValue="0" ControlToValidate="ddlAgrupacion" ErrorMessage="Debe seleccionar una agrupación"
                                    ForeColor="Red" ClientIDMode="Static" CssClass="ocultar" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <!-- Insercción código - Rafael Ortega - 22/07/2022 -->
                    <tr>
                        <td>
                            <asp:Panel CssClass="col-md-5" ID="panelIC" runat="server" Visible="True">
                                <br />
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIC" ImageUrl="../../Content/images/icono.png" AlternateText="Imagen centro" ClientIDMode="Static" runat="server" Width="100px" ToolTip="Imagen Central" /><span style="color: red;">*</span>
                                <br />
                                <input type='file' id="seleccionArchivoIC" onchange="archivoIC();" name="seleccionArchivoIC" accept=".jpg,.jpeg" clientidmode="Static" title="Imagen Central" />
                                <br />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel CssClass="col-md-5" ID="panel1" runat="server">
                                <br />
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIL" ImageUrl="../../Content/images/icono.png" AlternateText="Imagen Logo" ClientIDMode="Static" runat="server" Width="100px" ToolTip="Imagen Logotipo" /><span style="color: red;">*</span>
                                <br />
                                <input type='file' id="seleccionArchivoIL" onchange="archivoIL();" name="seleccionArchivoIL" accept=".jpg,.jpeg" clientidmode="Static" title="Imagen Logotipo" />
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>

        <!-- /modal with table -->
        <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" runat="server" clientidmode="Static">
            <a href="/evr/configuracion/centros" title="Volver" class="btn btn-primary run-first">Volver</a>
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
