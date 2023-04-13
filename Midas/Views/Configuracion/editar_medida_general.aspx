<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.medidas_generales oMedida;
    MIDAS.Models.medidas_generales_imagenes oImagen;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
            DatosImagenesGenerales.DataSource = ViewData["imagenesGenerales"];
            DatosImagenesGenerales.DataBind();
            DatosBancoIcono.DataSource = ViewData["banco_icono"];
            DatosBancoIcono.DataBind();
        }
        oMedida = (MIDAS.Models.medidas_generales)ViewData["EditarMedida"];

        if (oMedida != null)
        {
            //hdnIdMedida.Value = oMedida.id.ToString();
            //ImagenGrande.Checked=MIDAS.Models.Datos.EsImagenGrandeMedidaGeneral(oMedida.id);
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

        if (ViewData["apartados"] != null)
        {
            ddlApartados.DataSource = ViewData["apartados"];
            ddlApartados.DataValueField = "id";
            ddlApartados.DataTextField = "descripcion";


            ddlApartados.DataBind();

            if (oMedida != null)
            {

                int apartado = 0;
                int.TryParse(oMedida.id_apartado_generales.ToString(), out apartado);
                if (apartado > 0)
                {
                    ddlApartados.SelectedIndex = apartado - 1/*MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado)*/;
                }
            }
        }

        if (!IsPostBack)
        {
            oMedida = (MIDAS.Models.medidas_generales)ViewData["EditarMedida"];

            if (oMedida != null)
            {
                int apartado = 0;
                int.TryParse(oMedida.id_apartado_generales.ToString(), out apartado);
                ddlApartados.SelectedValue = MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado);
                //txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "\n");
                ViewData["idMedida"] = oMedida.id;
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
            if (ViewData["descripcion"] != null)
            {
                string descripcionBase = ViewData["descripcion"].ToString();



                string[] stringSeparators = new string[] { "[SALTO]" };
                string[] lines = descripcionBase.Split(stringSeparators, StringSplitOptions.None);

                string descripcionFinal = "";

                if (descripcionBase.Contains("[SALTO]"))
                {
                    int contador = 1;
                    foreach (string s in lines)
                    {
                        if (contador == 1)
                        {
                            descripcionFinal += s + "\n";
                            //< br />
                        }
                        else
                        {

                            descripcionFinal += " -" + s + "\n";
                            //< br />
                        }
                        contador++;
                    }
                }
                else
                {
                    descripcionFinal = descripcionBase;
                }
                txtNombre.Text = descripcionFinal;
            }
        }

        if (ViewData["EditarMedida"] != null)
        {
            oMedida = (MIDAS.Models.medidas_generales)ViewData["EditarMedida"];
            oImagen = (MIDAS.Models.medidas_generales_imagenes)ViewData["EditarImagen"];

            ddlApartados.SelectedValue = oMedida.id_apartado_generales.ToString();

            txtNombre.Visible = true;


            string descripcionBase = oMedida.descripcion.ToString();



            string[] stringSeparators = new string[] { "[SALTO]" };
            string[] lines = descripcionBase.Split(stringSeparators, StringSplitOptions.None);

            string descripcionFinal = "";

            if (descripcionBase.Contains("[SALTO]"))
            {
                int contador = 1;
                foreach (string s in lines)
                {
                    if (contador == 1)
                    {
                        if (contador != lines.Length)
                            descripcionFinal += s + "\n";
                        else
                            descripcionFinal += s;
                        //< br />
                    }
                    else
                    {
                        if (contador != lines.Length)
                            descripcionFinal += " -" + s + "\n";
                        else
                            descripcionFinal += " -" + s;
                        //< br />
                    }
                    contador++;
                }
            }
            else
            {
                descripcionFinal = descripcionBase;
            }
            txtNombre.Text = descripcionFinal;


            //txtNombre.Text = oMedida.descripcion.ToString();
            txtNombre.Text = descripcionFinal;
            GuardarUsuario.Disabled = false;

            if (oImagen.rutaImagen != null)
            {
                //obtener ruta de imagen
                var rutaFichero = oImagen.rutaImagen;
                rutaFichero = rutaFichero.Replace("..", "");
                imagenPrevisualizacionIcono.ImageUrl = ("~/" + rutaFichero);
            }
        }
               
    }

</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>
</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/editar_medida_general.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/editar_medida_general.css") %>" rel="stylesheet" />


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

        <% //ClientScript.RegisterStartupScript(GetType(), "miScript3", "prueba();", true);%>
        <asp:GridView ID="DatosImagenesGenerales" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosBancoIcono" runat="server" Visible="false">
        </asp:GridView>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>APARTADOS:</label>&nbsp;
                                    <asp:DropDownList ID="ddlApartados"
                                        CssClass="form-control"
                                        runat="server"
                                        Height="18px"
                                        Width="300px"
                                        AppendDataBoundItems="true"
                                        ClientIDMode="Static">
                                        <asp:ListItem Value="0">&lt;Seleccione un Item&gt;</asp:ListItem>
                                    </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 15px">
                        <br />
                        <label>
                            DESCRIPCION DE LA MEDIDA:</label>
                        <asp:TextBox ID="txtNombre" runat="server"
                            class="form-control"
                            TextMode="MultiLine"
                            Columns="100"
                            Rows="12"
                            Visible="true"
                            required="required"
                            ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <% #region icono e Imagen Grande%>
                <tr>
                    <td style="padding-right: 15px">
                        <br />
                        <asp:Panel runat="server" CssClass="form-group">
                            <div class="col-md-4 ">
                                <%if (oImagen != null)
                                    {
                                        if (oImagen.id_medida_general != 0)
                                        {
                                %>
                                <a href="/evr/Configuracion/eliminarImagenMedidas/<%= oImagen.id_medida_general %>" title="Eliminar Icono"><i class="icon-remove"></i></a>
                                <%      }
                                    }%>
                                <br />
                                <br />
                                <asp:Image ID="imagenPrevisualizacionIcono"
                                    ImageUrl="../../Content/images/iconoParaCargar.jpg"
                                    AlternateText="Imagen icono"
                                    runat="server"
                                    Width="80px"
                                    CssClass="prevIcono"
                                    ClientIDMode="Static" />
                                <%-- <input type='file'
                                    id="seleccionArchivoIcono"
                                    onchange="archivoIcono();"
                                    name="seleccionArchivoIcono" />--%>
                                <br />
                            </div>
                        </asp:Panel>
                    </td>
                    <br />
                    <td>
                        <input id="nombreIcono" type="text" name="name" value="" clientidmode="Static" runat="server" style="display: none;" />

                    </td>
                </tr>
                <% #endregion%>
            </table>
        </div>

        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>

        <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit" runat="server">
            <a href="/evr/configuracion/medidas_generales" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>
    </form>

    <div class="container py-4">
        <button class="btn btn-primary" data-toggle="modal" data-target="#mi-modal">
            Abrir banco de imagenes</button>
        <div class="modal fade" id="mi-modal" data-backdrop="static">
            <div class="modal-dialog" style="overflow-y: scroll; max-height: 85%; margin-top: 50px; margin-bottom: 50px;">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Banco de imagenes Medida General</h5>
                        <button class="btn btn-close" data-dismiss="modal">X</button>
                    </div>

                    <div class="modal-body">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Descripcion</th>
                                    <th style="width: 100px">Icono</th>
                                    <% if (user.perfil == 1 || user.perfil == 3)
                                        { %>

                                    <th style="width: 45px">Seleccionar</th>
                                    <% } %>
                                </tr>
                            </thead>
                            <tbody>
                                <%
                                    foreach (GridViewRow item in DatosBancoIcono.Rows)
                                    { %>
                                <tr>
                                    <td class="task-desc">
                                        <%=item.Cells[2].Text %>
                                    </td>
                                    <td class="task-desc">
                                        <img src=" <%= "../" +  item.Cells[3].Text%>" style="width: 80px; height: 80px;" />
                                    </td>
                                    <td class="task-desc">
                                        <input class="form-check-input single-checkbox" type="checkbox" value="" id="chk_<%=item.Cells[1].Text%>" data-codificacion="<%=item.Cells[1].Text%>" onclick="seleccionarImagen(this);">
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
    </div>

    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->

</asp:Content>
