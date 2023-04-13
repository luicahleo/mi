<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["medidas"];
            DatosPedidos.DataBind();
        }

        if (Session["EditarCentralesResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarCentralesResultado"].ToString() + "' });", true);
            Session["EditarCentralesResultado"] = null;
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
            <h3>Medidas Preventivas Generales <small>Medidas Preventivas Generales registradas en el sistema</small></h3>
        </div>
    </div>
    <!-- /page header -->
    <form id="form1" runat="server">
        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosProvincias" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosComunidades" runat="server" Visible="false">
        </asp:GridView>
    </form>
    <!-- Tasks table -->
    <div class="block">
        <center>
            <div style="width: 95%" class="datatablePedido">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="width: 60px">Apartado</th>
                            <th>Descripcion</th>
                            <%--  <th>Tamaño</th>--%>
                            <th style="width: 100px">Icono</th>
                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <th style="width: 45px">Editar</th>
                            <th style="width: 45px">Borrar</th>
                            <% } %>
                        </tr>
                    </thead>
                    <tbody>
                        <% 
                            List<MIDAS.Models.medidas_generales> medidas = (List<MIDAS.Models.medidas_generales>)ViewData["medidas"];

                            foreach (GridViewRow item in DatosPedidos.Rows)
                            { %>
                        <tr>
                            <td class="task-desc">
                                <%= MIDAS.Models.Datos.obtenerNombreApartadoGenerales(int.Parse(item.Cells[5].Text)) %>
                            
                            </td>
                            <td class="task-desc">
                                <%
                                     string[] stringSeparators = new string[] { "[SALTO]" };
                                     string[] lines = item.Cells[1].Text.Split(stringSeparators, StringSplitOptions.None);
                                     if (item.Cells[1].Text.Contains("[SALTO]"))
                                     {
                                         int contador = 1;
                                         foreach (string s in lines)
                                         {
                                             if (contador == 1)
                                             {%>
                                <%=s%><br />
                                <% }
                                    else
                                    {%>
                                <%=" -" +s%><br />
                                <%

                                    } %>
                                <%contador++;
                                        }
                                    }
                                    else
                                    {%>
                                <%=item.Cells[1].Text %>
                                <%} %>
                                                                                               
                              
                            </td>

                            <td class="task-desc">
                                <%  

                                    if (!string.IsNullOrEmpty(MIDAS.Models.Datos.obtenerImagenMedidasGenerales(int.Parse(item.Cells[0].Text))))
                                    { %>
                                         <img src=" <%= MIDAS.Models.Datos.obtenerImagenMedidasGenerales(int.Parse(item.Cells[0].Text))  %>" style="width: 60px; height: 60px;" />
                                  <%}
                                     else {%>
                                      <%}   %>
                            </td>

                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <td class="text-center">
                                <a href="/evr/Configuracion/editar_medida_general/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                            <td class="text-center">
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta medida?')) return false;" href="/evr/Configuracion/eliminar_medida_general/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                            </td>
                            <% } %>
                        </tr>
                        <% }%>
                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->

    <div style="text-align: right">
        <% if (user.perfil == 1 || user.perfil == 3)
            { %>
        <a href="/evr/Configuracion/editar_medida_general/0" title="Nueva Medida">
            <button class="btn btn-primary">Nueva Medida</button></a>
        <% } %>
        <%-- <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>--%>
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

