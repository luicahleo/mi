<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.documento_historico oDocumentoH = new MIDAS.Models.documento_historico();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosVersiones.DataSource = ViewData["versiones"];
            DatosVersiones.DataBind();

            MIDAS.Models.documento_historico doch = MIDAS.Models.Datos.ObtenerUltimoDocumentoHistorico(int.Parse(Session["CentralElegida"].ToString()));

            if (doch != null)
            {
                Session["DocuhistoricoLastVersion"] = doch.id;
            }



            if (doch != null)
            {
                Session["DocuhistoricoLastVersion"] = doch.id;
            }

            var listaDoc = MIDAS.Models.Datos.ListaDocumentoHistoricoDefinitivo(int.Parse(Session["CentralElegida"].ToString()));
            datosDocumentosH.DataSource = listaDoc;
            datosDocumentosH.DataBind();




            if (ViewData["textoUltimoBorador"] != null)
            {
                textooculto.Value = ViewData["textoUltimoBorador"].ToString();
            }
        }

        if (Session["EliminarSistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EliminarSistema"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }

        if (Session["GenerarDocumento"] != null)
        {

            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["GenerarDocumento"].ToString() + "' });", true);
            Session["GenerarDocumento"] = null;

        }

        if (TempData["Notification"] != null)
        {
            var tempData = TempData["Notification"].ToString();
            var lista = tempData.Split(',');

            string js = string.Format("MostrarMensaje('{0}','{1}','{2}');", lista[0], lista[1], lista[2]);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", js, true);
        }
    }
</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/DocumentoRiesgo/GenerarDocumentoRiesgo.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/DocumentoRiesgo/GenerarDocumentoRiesgo.css") %>" rel="stylesheet" />

    <div class="loader"></div>
    <form action="#" runat="server" enctype="multipart/form-data" id="formularioDocumento" method="post">
        <asp:GridView ID="datosDocumentosH" runat="server" Visible="false">
        </asp:GridView>
        <!-- Page header -->
        <div class="page-header">
            <div class="page-title">
                <h3>Documentos de Riesgos <small>Generar el Documento de Riesgos para la instalación</small></h3>
            </div>
        </div>
        <!-- /page header -->

        <asp:GridView ID="DatosVersiones" runat="server" Visible="false">
        </asp:GridView>
        <asp:HiddenField ID="textooculto" runat="server" ClientIDMode="Static"></asp:HiddenField>

        <div class="block">
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="width: 5%">Revisión</th>
                                <th style="width: 80%">Descripción: Control de Cambios</th>
                                <%--  <th>Tamaño</th>--%>
                                <th style="width: 42%">Fecha ultima modificacion</th>
                                <th style="width: 42%">Usuario</th>
                                <% if (user.perfil == 1 || user.perfil == 3)
                                    { %>
                                <th style="width: 5%">Descargar</th>
                                <% } %>
                            </tr>
                        </thead>

                        <tbody>
                            <%foreach (GridViewRow item in datosDocumentosH.Rows)
                                { %>

                            <tr>
                                <td>
                                    <center><%= item.Cells[10].Text %></center>
                                </td>
                                <td>
                                        <%= item.Cells[11].Text %>
                                </td>
                                <td><%= item.Cells[5].Text %></td>
                                <td><%= item.Cells[6].Text %></td>
                                <td><% if (item.Cells[7].Text == "1" && item.Cells[0].Text == Session["DocuhistoricoLastVersion"].ToString())
                                        {%>
                                    <center>
                                        <a><%--<i id="descargarDocumento" id_documento="<%=item.Cells[0].Text%>" class="icon-download"  onclick="DescargarDocumento()"></i>--%></a>
                                        <a href="../DescargarDocumento?id_documento=<%=item.Cells[0].Text%>"><i id="descargarDocumento" id_documento="<%=item.Cells[0].Text%>" class="icon-download" ></i></a>

                                        <%-- <button id="btnDescarga" class="btn btn-primary" style="all: unset; cursor: pointer;" ClientIDMode="Static">
                                            <a href="#"><i class="icon-download"></i></a>
                                        </button>--%>
                                    </center>
                                    <%--<center>
                                        <a href="/evr/DocumentoRiesgos/GenerarDocumentoRiesgos" title="Descargar" ><i class="icon-download"></i></a>
                                    </center>--%>
                                    <%}
                                        else
                                        { %>

                                    <%}%>
                                </td>

                            </tr>

                            <%} %>
                        </tbody>


                    </table>
                </div>

                <table width="100%">
                    <tr>
                        <td style="width: 20%" class="margenes">
                            <center>
                                <div id="botonVerComentarios" class="btn btn-primary" style="border-radius: 20px; width: 65%; cursor: none; background-color: #41b9e6 !important; border-color: #41b9e6 !important">
                                    <table style="width: 100%; height: 30px">

                                        <tr>
                                            <td>
                                                <center>
                                                    <span style="font-size: 13px">
                                                        <label id="label5">Generar Documento de Riesgos</label>
                                                    </span>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <tr id="filaComentarios">
                        <td class="margenes">
                            <center>
                                <div style="width: 60%" class="datatablePedido">
                                    <div style="text-align: left">
                                        <label>Descripción: Control de Cambios </label>
                                    </div>

                                    <textarea id="texto_descripcion" style="width: 100%;" name="Text1" cols="150" rows="10" onchange="activarboton()" style="white-space: pre-line;"></textarea>

                                    <div style="text-align: right">
                                        <a id="botonBorrador" onclick="generarBorrador()" title="Borrador" class="btn btn-primary run-first">Visualizar Borrador</a>
                                        <%if (ViewData["Haymatrizborrador"] != null && ViewData["Haymatrizborrador"].ToString() != "0")
                                            { %>
                                        <a id="botonDefinitivo" onclick="generarDefinitivo()" title="Definitivo" class="btn btn-primary run-first">Generar Versión Definitiva</a>
                                        <%}
                                            else
                                            {%>
                                        <button id="botonDefinitivo" onclick="generarDefinitivo()" title="Definitivo" class="btn btn-primary run-first" disabled="disabled">Generar Versión Definitiva</button>
                                        <br />
                                        <br />
                                        <label style="color: red">Para generar la Versión Definitiva es necesario tener una matriz de riesgos en estado borrador.</label>
                                        <%}%>
                                    </div>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="width: 20%" class="margenes">
                            <center>
                                <a class="btn btn-primary" style="border-radius: 20px; width: 65%">
                                    <table style="width: 100%; height: 30px">
                                        <tr>
                                            <td>
                                                <center>
                                                    <span style="font-size: 13px">
                                                        <asp:Label ID="label3" runat="server" Text="Generar Mapa de Riesgos" />
                                                    </span>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </a>
                            </center>
                        </td>
                    </tr>--%>
                </table>

                <br />

            </center>
        </div>
        <!-- Tasks table -->

        <div style="text-align: right">
            <% if ((user.perfil == 1 || user.perfil == 3) && DatosVersiones.Rows.Count < 1)
                { %>

            <% } %>
            <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>
        <p>
            <br />
        </p>
        <!-- Footer -->
        <div class="footer clearfix">
            <div class="pull-left"></div>
        </div>
    </form>
    <!-- /footer -->
</asp:Content>
