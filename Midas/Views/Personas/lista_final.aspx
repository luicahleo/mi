<%@ Import Namespace="System" %>
<%@ Import Namespace="MIDAS.Models" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<script runat="server">    
    //VISTA_ObtenerUsuario user = new VISTA_ObtenerUsuario();
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (ViewData["listaFinal"] != null)
            {
                DatosListaFinal.DataSource = ViewData["listaFinal"];
                DatosListaFinal.DataBind();
            }
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
    <%--<script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Personas/analisis_datos.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Personas/analisis_datos.css") %>" rel="stylesheet" />--%>

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Lista Final <small>Lista final de empleados seleccionados.</small></h3>
        </div>
    </div>
    <!-- /page header -->

    <form id="form1" runat="server">
        <asp:GridView ID="DatosListaFinal" runat="server" Visible="false">
        </asp:GridView>

        <asp:ScriptManager runat="server"></asp:ScriptManager>

    </form>

    <!-- Tasks table -->
    <div class="block">
        <center>
            <div style="width: 95%" class="datatablePedido">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="width: 20%">Nombre</th>
                            <th style="width: 10%">DNI</th>
                            <th style="width: 10%">Perfil de Riesgo</th>
                            <th style="width: 20%">Actividad Funcional</th>

                           

                            <th style="width: 5%">Asignar</th>
                            <th style="width: 5%">Borrar</th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (GridViewRow item in DatosListaFinal.Rows)
                            { %>
                        <tr>

                            <td class="task-desc">
                                <%= item.Cells[6].Text %>
                            </td>

                            <td class="task-desc"><%= item.Cells[5].Text%>
                            </td>
                            <td class="task-desc"><%= item.Cells[3].Text%>
                            </td>
                            <td class="task-desc"><%= item.Cells[4].Text%>
                            </td>

                            
                            <td class="text-center">
                                <a href="/evr/Encuesta/asignar_empleado/<%= item.Cells[0].Text %>" title="Asignar"><i class="icon-arrow-right4"></i></a>
                            </td>
                            <td class="text-center">
                                <a href="/evr/Personas/eliminar_persona_listaFinal/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                            </td>


                        </tr>
                        <% }%>
                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->





    <!-- /footer -->

</asp:Content>
