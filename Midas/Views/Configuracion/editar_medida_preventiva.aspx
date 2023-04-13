<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.medidas_preventivas oMedida;
    MIDAS.Models.medidaspreventivas_imagenes oImagen;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
        }
        oMedida = (MIDAS.Models.medidas_preventivas)ViewData["EditarMedida"];

        if (oMedida != null)
        {
            hdnIdMedida.Value = oMedida.id.ToString();
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

        if (ViewData["situaciones"] != null)
        {
            ddlSituaciones.DataSource = ViewData["situaciones"];
            ddlSituaciones.DataValueField = "id";
            ddlSituaciones.DataTextField = "descripcion";
            ddlSituaciones.DataBind();

            if (oMedida != null)
            {
                int situacion = 0;
                int.TryParse(oMedida.id_situacion.ToString(), out situacion);
                if (situacion > 0)
                {
                    ddlSituaciones.SelectedIndex = situacion - 1/*MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado)*/;
                }
                txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "");
                foreach (MIDAS.Models.submedidas_preventivas submedida in MIDAS.Models.Datos.ListarSubMedidas(oMedida.id))
                {
                    txtNombre.Text += "\n-" + submedida.descripcion;
                }
            }
        }
        if (!IsPostBack)
        {
            if (ViewData["EditarMedida"] != null)
            {
                oMedida = (MIDAS.Models.medidas_preventivas)ViewData["EditarMedida"];
                oImagen = (MIDAS.Models.medidaspreventivas_imagenes)ViewData["EditarImagen"];

                if (oMedida != null)
                {
                    ddlSituaciones.SelectedIndex = oMedida.id_situacion;
                    txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "");
                    foreach (MIDAS.Models.submedidas_preventivas submedida in MIDAS.Models.Datos.ListarSubMedidas(oMedida.id))
                    {
                        txtNombre.Text += "\n-" + submedida.descripcion;
                    }
                    ViewData["idMedida"] = oMedida.id;
                }

                if (oImagen != null)
                {
                    if (oImagen.tamano == false)
                    {
                        imagenPrevisualizacionIC.ImageUrl = "../" + oImagen.rutaImagen;
                    }
                    if (oImagen.tamano == true)
                    {
                        imagenPrevisualizacionIG.ImageUrl = "../" + oImagen.rutaImagen;
                    }
                }
            }
        }
        if (Session["ErrorForm"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["ErrorForm"].ToString() + "' });", true);
            Session.Remove("ErrorForm");

            if (ViewData["situacion"] != null)
            {
                ddlSituaciones.SelectedIndex = int.Parse(ViewData["situacion"].ToString());
            }
            if (ViewData["descripcion"] != null)
            {
                txtNombre.Text = ViewData["descripcion"].ToString();
            }
        }
    }

</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>
</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/editar_medida_preventiva.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/editar_medida_preventiva.css") %>" rel="stylesheet" />

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server" method="post" enctype="multipart/form-data">
        <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-office"></i>Datos de medida preventiva</h6>
                <asp:HiddenField runat="server" ID="hdnIdMedida" ClientIDMode="Static" />
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    SITUACIÓN:</label>&nbsp;
                               <asp:DropDownList ID="ddlSituaciones"
                                   CssClass="form-control"
                                   runat="server"
                                   ClientIDMode="Static"
                                   AppendDataBoundItems="true">
                                   <asp:ListItem Value="0">&lt;Seleccione un Item&gt;</asp:ListItem>
                               </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    DESCRIPCION:</label>
                                <asp:TextBox ID="txtNombre"
                                    runat="server"
                                    class="form-control"
                                    TextMode="MultiLine"
                                    Columns="100"
                                    Rows="12"
                                    ClientIDMode="Static">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 15px">
                            <asp:Panel CssClass="col-md-5" ID="panelIC" runat="server" Visible="True">
                                <br />
                                <%--<asp:CheckBox ID="chkIC" runat="server" ClientIDMode="Static" Text="Cambiar Imagen Icono" ToolTip="Cambiar Imagen Icono" Visible="False" />--%>
                                <%if (oImagen != null)
                                    {
                                        if (oImagen.id != 0 && oImagen.tamano == false)
                                        {
                                %>
                                <a href="/evr/Configuracion/eliminarImagenPrevia/<%= oImagen.id %>" title="Eliminar Icono"><i class="icon-remove"></i></a>
                                <%}
                                    } %>
                                <br />
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIC"
                                    ImageUrl="../../Content/images/icono.png"
                                    AlternateText="Imagen icono"
                                    runat="server"
                                    Width="100px"
                                    ClientIDMode="Static" />
                                <br />
                                <input type='file'
                                    id="seleccionArchivoIC"
                                    onchange="archivoIC();"
                                    name="seleccionArchivoIC"
                                    accept=".jpg,.jpeg"
                                    clientidmode="Static"
                                    title="Imagen Icono" />
                                <br />
                            </asp:Panel>

                        </td>
                        <td style="padding-right: 15px">
                            <asp:Panel CssClass="col-md-5" ID="panel1" runat="server" Visible="True">
                                <br />
                                <%--<asp:CheckBox ID="chkIG"
                                    runat="server"
                                    ClientIDMode="Static"
                                    Text="Cambiar Imagen Grande"
                                    ToolTip="Cambiar Imagen Grande"
                                    Visible="False" />--%>
                                <%if (oImagen != null)
                                    {
                                        if (oImagen.id != 0 && oImagen.tamano == true)
                                        {
                                %>
                                <a href="/evr/Configuracion/eliminarImagenPrevia/<%= oImagen.id %>" title="Eliminar Imagen"><i class="icon-remove"></i></a>
                                <%}
                                    } %>
                                <br />
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIG"
                                    ImageUrl="../../Content/images/icono.png"
                                    AlternateText="Imagen grande"
                                    runat="server" Width="100px"
                                    ClientIDMode="Static" />
                                <br />
                                <input type='file'
                                    id="seleccionArchivoIG"
                                    onchange="archivoIG();"
                                    name="seleccionArchivoIG"
                                    accept=".jpg,.jpeg"
                                    clientidmode="Static" />
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>

        <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit">
            <a href="/evr/configuracion/medidas_preventivas" title="Volver" class="btn btn-primary run-first">Volver</a>
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
