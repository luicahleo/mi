<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.riesgos_medidas oMedida;
    MIDAS.Models.medidas_apartados oMedidas_apartados;
    MIDAS.Models.tipos_riesgos oTtipos_riesgos;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
            DatosImagenesRiesgos.DataSource = ViewData["imagenesRiesgos"];
            DatosImagenesRiesgos.DataBind();
        }
        oMedida = (MIDAS.Models.riesgos_medidas)ViewData["EditarMedida"];

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

        if (ViewData["apartadoSeleccionado"] != null)
        {
            oMedidas_apartados = (MIDAS.Models.medidas_apartados)ViewData["apartadoSeleccionado"];

            ddlApartados.SelectedIndex = oMedidas_apartados.id;
        }
        if (ViewData["tipoRiesgoSeleccionado"] != null)
        {
            //visualizamos el ddlRiesgos
            ddlRiesgos.CssClass = ddlRiesgos.CssClass.Replace("visibleOff", "visibleOn");

            oTtipos_riesgos = (MIDAS.Models.tipos_riesgos)ViewData["tipoRiesgoSeleccionado"];

            ddlRiesgos.SelectedIndex = oTtipos_riesgos.id;
        }

        if (ViewData["EditarMedida"] != null)
        {
            //visualizamos la descripcion
            string descripcionBase = oMedida.descripcion.ToString();



            string[] stringSeparators = new string[] { "[SALTO]" };
            string[] lines = descripcionBase.Split(stringSeparators, StringSplitOptions.None);

            string descripcionFinal = "";

            if (descripcionBase.Contains("[SALTO]")) {
                int contador = 1;
                foreach (string s in lines) {
                    if (contador == 1) {
                        if(contador!=lines.Length)
                            descripcionFinal += s + "\n";
                        else
                            descripcionFinal += s;
                        //< br />
                    }
                    else {
                        if(contador!=lines.Length)
                            descripcionFinal += " -" + s + "\n";
                        else
                            descripcionFinal += " -" + s;
                        //< br />
                    }
                    contador++;
                }
            }
            else {
                descripcionFinal = descripcionBase;
            }
            txtNombre.Text = descripcionFinal;
            //txtNombre.Text = oMedida.descripcion;

            if (oMedida.imagen != null)
            {
                var rutaFichero = oMedida.imagen;
                rutaFichero = rutaFichero.Replace("..", "");

                if (oMedida.imagen_grande == 0)
                {
                    //agregarIcono.Checked = true;
                    imagenPrevisualizacionIC.ImageUrl = ("~/" + rutaFichero);
                }
                else if (oMedida.imagen_grande == 1)
                {

                }
            }
        }

        if (ViewData["apartados"] != null)
        {

            ddlApartados.DataSource = ViewData["apartados"];
            ddlApartados.DataValueField = "id";
            ddlApartados.DataTextField = "nombre";
            ddlApartados.DataBind();

            ddlRiesgos.DataSource = ViewData["riesgos"];
            ddlRiesgos.DataValueField = "id";
            ddlRiesgos.DataTextField = "codigo";
            ddlRiesgos.DataBind();

            if (oMedida != null)
            {
                string descripcionBase = oMedida.descripcion.ToString();



                string[] stringSeparators = new string[] { "[SALTO]" };
                string[] lines = descripcionBase.Split(stringSeparators, StringSplitOptions.None);

                string descripcionFinal = "";

                if (descripcionBase.Contains("[SALTO]")) {
                    int contador = 1;
                    foreach (string s in lines) {
                        if (contador == 1) {
                            if(contador!=lines.Length)
                                descripcionFinal += s + "\n";
                            else
                                descripcionFinal += s;
                            //< br />
                        }
                        else {
                            if(contador!=lines.Length)
                                descripcionFinal += " -" + s + "\n";
                            else
                                descripcionFinal += " -" + s;
                            //< br />
                        }
                        contador++;
                    }
                }
                else {
                    descripcionFinal = descripcionBase;
                }
                txtNombre.Text = descripcionFinal;
                //txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "\n");
                int apartado = 0;
                int.TryParse(oMedida.id_apartado.ToString(), out apartado);
                if (apartado > 0)
                {
                    ddlApartados.SelectedIndex = apartado - 1/*MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado)*/;
                }
                int riesgos = 0;
                int.TryParse(oMedida.id_riesgo.ToString(), out riesgos);

            }
        }

        if (!IsPostBack)
        {
            if (ViewData["EditarMedida"] != null)
            {
                oMedida = (MIDAS.Models.riesgos_medidas)ViewData["EditarMedida"];
                if (oMedida != null)
                {
                    ddlApartados.SelectedIndex = int.Parse(oMedida.id_apartado.ToString());
                    ddlRiesgos.SelectedIndex = int.Parse(oMedida.id_riesgo.ToString());

                    hdIdMedida.Value = oMedida.id.ToString();
                    string descripcionBase = oMedida.descripcion.ToString();



                string[] stringSeparators = new string[] { "[SALTO]" };
                string[] lines = descripcionBase.Split(stringSeparators, StringSplitOptions.None);

                string descripcionFinal = "";

                if (descripcionBase.Contains("[SALTO]")) {
                    int contador = 1;
                    foreach (string s in lines) {
                        if (contador == 1) {
                            if(contador!=lines.Length)
                                descripcionFinal += s + "\n";
                            else
                                descripcionFinal += s;
                            //< br />
                        }
                        else {
                            if(contador!=lines.Length)
                                descripcionFinal += " -" + s + "\n";
                            else
                                descripcionFinal += " -" + s;
                            //< br />
                        }
                        contador++;
                    }
                }
                else {
                    descripcionFinal = descripcionBase;
                }
                    txtNombre.Text = descripcionFinal;
                    if (oMedida.imagen != null && oMedida.imagen != "")
                    {
                        if (oMedida.imagen_grande == 0)
                        {
                            chkIC.Visible = true;
                            imagenPrevisualizacionIC.ImageUrl = "../" + oMedida.imagen;
                        }
                        else
                        {
                            chkIC.Visible = false;
                        }
                        if (oMedida.imagen_grande == 1)
                        {
                            chkIG.Visible = true;
                            imagenPrevisualizacionIG.ImageUrl = "../" + oMedida.imagen;
                        }
                        else
                        {
                            chkIG.Visible = false;
                        }
                    }
                }
            }
        }

        if (Session["ErrorForm"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["ErrorForm"].ToString() + "' });", true);
            Session.Remove("ErrorForm");

            if (ViewData["apartado"] != null)
            {
                ddlApartados.SelectedIndex = int.Parse(ViewData["apartado"].ToString());
            }
            if (ViewData["riesgo"] != null)
            {
                ddlRiesgos.SelectedIndex = int.Parse(ViewData["riesgo"].ToString());
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

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/editar_riesgo_medida.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/editar_riesgo_medida.css") %>" rel="stylesheet" />

    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server" method="post" enctype="multipart/form-data">
        <asp:GridView ID="DatosImagenesRiesgos" runat="server" Visible="false">
        </asp:GridView>
        <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
        <asp:HiddenField runat="server" ID="hdIdMedida" ClientIDMode="Static" />
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-office"></i>Datos de medida asociada a riesgo</h6>
                <asp:HiddenField runat="server" ID="hdnIdMedida" ClientIDMode="Static" />
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    APARTADOS:</label>&nbsp;
                                <asp:DropDownList ID="ddlApartados"
                                    CssClass="form-control"
                                    AppendDataBoundItems="true"
                                    runat="server"
                                    ClientIDMode="Static">
                                    <asp:ListItem Value="0">&lt;Seleccione un Item&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <asp:TextBox ID="apartadoOtro" runat="server" CssClass="form-control" placeholder="OTROS" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="padding-right: 15px">
                            <asp:Panel class="form-group" runat="server" ID="panelddlRiesgos" ClientIDMode="Static">
                                <label>RIESGOS:</label>&nbsp;
                                    <asp:DropDownList ID="ddlRiesgos"
                                        CssClass="form-control"
                                        runat="server"
                                        AppendDataBoundItems="true"
                                        ClientIDMode="Static">
                                        <asp:ListItem Value="0">&lt;Seleccione un Item&gt;</asp:ListItem>
                                    </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 15px">
                            <asp:Panel class="form-group" runat="server" ID="paneltxtNombre" ClientIDMode="Static">
                                <label>
                                    DESCRIPCION:</label>
                                <asp:TextBox ID="txtNombre" runat="server"
                                    class="form-control"
                                    TextMode="MultiLine"
                                    Columns="100"
                                    Rows="12"
                                    required="required"
                                    ClientIDMode="Static"></asp:TextBox>
                            </asp:Panel>
                        </td>
                    </tr>

                    <% #region icono e Imagen Grande%>



                    <tr>
                        <td style="padding-right: 15px">
                            <asp:Panel CssClass="col-md-5" ID="panelIC" runat="server" Visible="True">
                                <br />
                                <%--<asp:CheckBox ID="chkIC" class="icon-remove" runat="server" 
                                    ClientIDMode="Static" 
                                    ToolTip="Eliminar Imagen Icono" 
                                    Visible="False" 
                                    id-medida=""
                                    />--%>
                                <asp:Panel runat="server" id="chkIC" Visible="false">
                                    <% if (ViewData["EditarMedida"] != null) {
                                       MIDAS.Models.riesgos_medidas medidaAux = (MIDAS.Models.riesgos_medidas)ViewData["EditarMedida"];
                                            Session["idmedida"] = medidaAux.id;
                                        %>
                                <a href="/evr/Configuracion/eliminar_icono/<%= medidaAux.id %>" title="Eliminar Icono"><i class="icon-remove"></i></a>
                                <%}%>
                                </asp:Panel>
                                
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIC"
                                    ImageUrl="../../Content/images/icono.png"
                                    AlternateText="Añadir Icono"
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
                                    Text="Eliminar Imagen Grande"
                                    ToolTip="Eliminar Imagen Grande"
                                    Visible="False" 
                                    data-id-medida ="1000"/>--%>
                                <asp:Panel runat="server" ID="chkIG" Visible="false">
                                    <% if (ViewData["EditarMedida"] != null) {
                                       MIDAS.Models.riesgos_medidas medidaAux = (MIDAS.Models.riesgos_medidas)ViewData["EditarMedida"];
                                            Session["idmedida"] = medidaAux.id;
                                        %>
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta imagen?')) return false;" href="/evr/Configuracion/eliminar_ImagenGrande/<%= medidaAux.id %>" title="Eliminar Imagen"><i class="icon-remove"></i></a>
                                <%}%>
                                </asp:Panel>
                                
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIG"
                                    ImageUrl="../../Content/images/icono.png"
                                    AlternateText="Añadir Imagen"
                                    runat="server" Width="100px"
                                    ClientIDMode="Static" />
                                <br />
                                <input type='file'
                                    id="seleccionArchivoIG"
                                    onchange="archivoIG();"
                                    name="seleccionArchivoIG"
                                    accept=".jpg,.jpeg"
                                    clientidmode="Static"
                                    />
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>

                    <% #endregion%>
                </table>
            </div>
        </div>
        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>
        <!-- Tasks table -->

        <!-- /tasks table -->

        <!-- /modal with table -->
        <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit" runat="server" clientidmode="Static">
            <a href="/evr/configuracion/riesgos_medidas" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>

    </form>
    <!-- /form vertical (default) -->

    <%-- <div class="container py-4">
        <button class="btn btn-primary" data-toggle="modal" data-target="#mi-modal">
            Abrir banco de imagenes</button>
        <div class="modal fade" id="mi-modal" data-backdrop="static">
            <div class="modal-dialog" style="overflow-y: scroll; max-height:85%;  margin-top: 50px; margin-bottom:50px;">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Banco de imagenes Medida General</h5>
                        <button class="btn btn-close" data-dismiss="modal">X</button>
                    </div>

                    <div class="modal-body">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Nombre</th>
                                    <th style="width: 100px">Imagen</th>
                                    <% if (user.perfil == 1 || user.perfil == 3)
                                        { %>

                                    <th style="width: 45px">Seleccionar</th>
                                    <% } %>
                                </tr>
                            </thead>
                            <tbody>
                                <%
                                    foreach (GridViewRow item in DatosImagenesRiesgos.Rows)
                                    { %>
                                <tr>
                                    <td class="task-desc">
                                    <%var nombre = item.Cells[5].Text.Replace("../Content/images/medidas/", ""); %>

                                        <%= nombre %>
                                    </td>
                                    <td class="task-desc">
                                        <img src=" <%=  "../" + item.Cells[5].Text%>" style="width: 80px; height: 80px;" />
                                    </td>
                                    <td class="task-desc">
                                        <input class="form-check-input" type="checkbox" value="">
                                    </td>
                                </tr>
                                <% }%>
                            </tbody>

                        </table>

                    </div>

                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                        <button class="btn btn-primary">Guardar</button>
                    </div>

                </div>
            </div>
        </div>
    </div>--%>

    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
