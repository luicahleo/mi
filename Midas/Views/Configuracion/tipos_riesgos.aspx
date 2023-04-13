<%@ Page Title="tipos_riesgos" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["tipos_riesgos"];
        DatosPedidos.DataBind();

        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["TiposRiesgosResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAmbitosResultado"].ToString() + "' });", true);
            Session["TiposRiesgosResultado"] = null;
        }

        if (TempData["Notification"] != null)
        {
            var tempData = TempData["Notification"].ToString();
            var lista = tempData.Split(',');

            string js = string.Format("MostrarMensaje('{0}','{1}');", lista[0], lista[1]);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", js, true);
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
            <h3>Tipos de Riesgos<small>Tipos de riesgos registrados en el sistema.</small></h3>
        </div>
    </div>
    <!-- /page header -->

    <form id="formRiesgos" runat="server">
        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
           
        </asp:GridView>
    </form>
    <!-- Tasks table -->
    <div class="block">
        <center>
            <div style="width:95%" class="datatablePedido">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Tipo de Riesgo</th>
                            <% if (user.perfil == 1 || user.perfil == 3)
                            { %>
                            <th>Definición</th>
                            <th style="width:45px">Editar</th>               
                           <%-- <th style="width:45px">Eliminar</th>--%>
                            <%} %>
                        </tr>
                    </thead>
                    <tbody>
                        <%
                        foreach (GridViewRow item in DatosPedidos.Rows)
                        { %>
                        <tr>
                            <td class="task-desc">
                                <%= item.Cells[1].Text %>
                            </td>
                           
                               <td class="task-desc">
                               <%=item.Cells[2].Text %>
                                </td>
                             <% if (user.perfil == 1 || user.perfil == 3)
                                {
                             %>
                            <td class="text-center">
                                <a href="/evr/Configuracion/editar_tipos_riesgos/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                           <%-- <td class="text-center">
                                <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/Configuracion/tipos_riesgos/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                            </td>--%>
                            <% } %>
                        </tr>
                        <% }%>

                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->

    <div style="text-align:right">
<%--        <% if (user.perfil == 1 || user.perfil == 3)
        { %>
        <a href="/evr/Configuracion/editar_ambito/0" title="Nuevo tipo"><button class="btn btn-primary">Nuevo Tipo de Riesgo</button></a>
        <% } %>--%>
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




