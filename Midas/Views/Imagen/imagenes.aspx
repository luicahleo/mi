<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage"  %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    List<MIDAS.Models.medidas_generales_imagenes> imagenes = new List<MIDAS.Models.medidas_generales_imagenes>();
    //List<MIDAS.Models.Vista_listar> imagenes = new List<MIDAS.Models.VISTA_ListarCentrales>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            imagenes = MIDAS.Models.Datos.ListarMedidasGeneralesImagenes();
            DatosImagenesGenerales.DataSource = ViewData["imagenesGenerales"];
            DatosImagenesGenerales.DataBind();
            DatosImagenesRiesgos.DataSource = ViewData["imagenesRiesgos"];
            DatosImagenesRiesgos.DataBind();
            DatosImagenesPreventivas.DataSource = ViewData["imagenesPreventivas"];
            DatosImagenesPreventivas.DataBind();
            

            //DatosTecnologiasCentros.DataSource = ViewData["tecCentros"];
            //DatosTecnologiasCentros.DataBind();

        }

        if (Session["EditarCentralesResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarCentralesResultado"].ToString() + "' });", true);
            Session["EditarCentralesResultado"] = null;
        }
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Banco de imagenes</h3>
        </div>
    </div>
    <!-- /page header -->
    <form id="form1" runat="server">
        <asp:GridView ID="DatosImagenesGenerales" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosImagenesRiesgos" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosImagenesPreventivas" runat="server" Visible="false">
        </asp:GridView>
        <%--<asp:GridView ID="DatosTecnologiasCentros" runat="server" Visible="true">
        </asp:GridView>--%>
    </form>
    <!-- Tasks table -->
    <div class="block">
        <center>
            <div style="width: 95%" class="datatablePedido">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th style="width: 100px">Imagen</th>
                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>

                            <th style="width: 45px">Editar</th>
                            <th style="width: 45px">Borrar</th>
                            <% } %>
                        </tr>
                    </thead>
                    <tbody>
                           
                       <%
                           
                            foreach (GridViewRow item in DatosImagenesGenerales.Rows)
                            { %>
                            <tr>
                                <td class="task-desc">
                                <%var nombre = item.Cells[2].Text.Replace("../Content/images/medidas/medidasgenerales/", ""); %>

                                    <%= nombre %>
                                </td>
                                <td class="task-desc">
                                    <img src=" <%= item.Cells[2].Text%>" style="width: 80px; height: 80px;" />
                                </td>
                             </tr>
                         <% }%>



                         <%
                            foreach (GridViewRow item in DatosImagenesRiesgos.Rows)
                            { %>
                            <tr>
                                <td class="task-desc">
                                <%var nombre = item.Cells[5].Text.Replace("../Content/images/medidas/", ""); %>

                                    <%= nombre %>
                                </td>
                                <td class="task-desc">
                                    <img src=" <%= item.Cells[5].Text%>" style="width: 80px; height: 80px;" />
                                </td>
                             </tr>
                         <% }%>


                         <%
                            foreach (GridViewRow item in DatosImagenesPreventivas.Rows)
                            { %>
                            <tr>
                                <td class="task-desc">
                                <%var nombre = item.Cells[2].Text.Replace("../Content/images/medidas/medidaspreventivas/", ""); %>

                                    <%= nombre %>
                                </td>
                                <td class="task-desc">
                                    <img src=" <%= item.Cells[2].Text%>" style="width: 80px; height: 80px;" />
                                </td>
                             </tr>
                         <% }%>
                        <tr>
                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->

    <div style="text-align: right">
        <% if (user.perfil == 1 || user.perfil == 3)
            { %>
        <a href="/evr/Imagen/editar_imagen/0" title="Nueva Imagen">
            <button class="btn btn-primary">Nueva Imagen</button></a>
        <% } %>
        <a href="/evr/Configuracion/menu" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    <p>
        <br />
    </p>
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->


</asp:Content>

