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
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Medidas Preventivas por Riesgos <small>Medidas Preventivas por Riesgos registradas en el sistema</small></h3>
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
                            <th>Tipo de Medida (Administrador / Instalación)</th>
                            <th style="width: 60px">Riesgo</th>
                            <th style="width: 60px">Apartado</th>
                            <th>Descripcion</th>
                            <%--  <th>Tamaño</th>--%>
                            <th style="width: 100px">Icono</th>
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

                            foreach (GridViewRow item in DatosPedidos.Rows)
                            {
                                MIDAS.Models.tipos_riesgos tRiesgo = new MIDAS.Models.tipos_riesgos();
                                tRiesgo = MIDAS.Models.Datos.ObtenerTiposRiesgos(int.Parse(item.Cells[4].Text));
                        %>
                        <tr>
                            <td class="task-desc">
                                <% if (int.Parse(item.Cells[7].Text) == 0)
                                    {%>
                                            Administrador
                                         <%}%>
                                <%else
                                    {
                                        MIDAS.Models.centros centro = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(item.Cells[7].Text));%>
                                <%= centro.nombre %>
                                <%}%>
                               
                            </td>
                            <td style="width: 100px">

                                <%=tRiesgo.codigo %>

                            </td>
                            <td class="task-desc">
                                <%  if (item.Cells[6].Text != "&nbsp;" && item.Cells[6].Text != null)
                                    {%>
                                <%= MIDAS.Models.Datos.obtenerNombreApartadoMedidasRiesgoV2(int.Parse(item.Cells[6].Text)) %>
                                <%} %>
                            </td>

                            <td class="task-desc">
                                <%
                                    string[] stringSeparators = new string[] { "[SALTO]" };
                                    string[] lines = item.Cells[2].Text.Split(stringSeparators, StringSplitOptions.None);
                                    if (item.Cells[2].Text.Contains("[SALTO]"))
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
                                <%=" - " +s%><br />
                                <%

                                    } %>
                                <%contador++;
                                        }
                                    }
                                    else
                                    {%>
                                <%= item.Cells[2].Text %>
                                <%} %>

                                  
                              
                            </td>
                            <%-- Imagen icono --%>
                            <td class="task-desc">
                                <%  
                                    if (!string.IsNullOrEmpty(item.Cells[5].Text) && item.Cells[5].Text != "&nbsp;")
                                    {
                                        if (item.Cells[8].Text == "0")
                                        {
                                %>
                                <img src="<%=item.Cells[5].Text %>" style="width: 60px; height: 60px;" />

                                <%}
                                    } %>
                            </td>
                            <%--Imagen granden--%>
                            <td class="task-desc">
                                <%  
                                    if (!string.IsNullOrEmpty(item.Cells[5].Text) && item.Cells[5].Text != "&nbsp;")
                                    {
                                        if (item.Cells[8].Text == "1")
                                        { %>
                                <img src="<%=item.Cells[5].Text %>" style="width: 60px; height: 60px;" />
                                <%}
                                    } %>
                            </td>
                            <%--   <td class="task-desc">
                                                    <%= item.Cells[4].Text %>
                                                </td>    --%>


                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <td class="text-center">
                                <a href="/evr/Configuracion/editar_riesgo_medida/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                            <td class="text-center">
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta medida?')) return false;" href="/evr/Configuracion/eliminar_riesgo_medida/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
        <a href="/evr/Configuracion/editar_riesgo_medida/0" title="Nueva Medida">
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

