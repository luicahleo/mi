<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centros = new MIDAS.Models.centros();
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

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/medidas_preventivas.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/medidas_preventivas.css") %>" rel="stylesheet" />



    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Medidas Preventivas por Situaciones de Riesgo <small>Medidas Preventivas por Situaciones de Riesgo registradas en el sistema</small></h3>
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
                            <th>Riesgo</th>
                            <th>Situacion de Riesgo</th>
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
                            List<MIDAS.Models.medidas_preventivas> medidas = (List<MIDAS.Models.medidas_preventivas>)ViewData["medidas"];
                            List<MIDAS.Models.submedidas_preventivas> submedidas = (List<MIDAS.Models.submedidas_preventivas>)ViewData["SubMedidas"];
                            foreach (GridViewRow item in DatosPedidos.Rows)
                            { %>
                        <tr>
                            <td class="task-desc">
                                <% if (int.Parse(item.Cells[4].Text) == 0)
                                    {%>
                                            Administrador
                                         <%}%>
                                <%else
                                    {
                                        MIDAS.Models.centros centro = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(item.Cells[4].Text));%>
                                <%= centro.nombre %>
                                <%}%>
                               
                            </td>
                            <td class="task-desc">
                                <% MIDAS.Models.riesgos_situaciones situacion = MIDAS.Models.Datos.ObtenerSituacionRiesgoporID(int.Parse(item.Cells[3].Text));
                                    MIDAS.Models.tipos_riesgos Triesgos = MIDAS.Models.Datos.ObtenerTiposRiesgos(situacion.id_tipo_riesgo);
                                    if (Triesgos != null)
                                    {%>
                                <%=Triesgos.codigo %>

                                <%}%>
                                
                            </td>
                            <td class="task-desc">
                                <%if (situacion != null)
                                    {
                                %>
                                <%-- esta columa representa numeroDeRiesgo. numeroSituacionDeRiesgo --%>
                                <%=Triesgos.id%>. <%=situacion.descripcion %>

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

                                <%--<%= item.Cells[2].Text %>--%>

                                <%foreach (MIDAS.Models.submedidas_preventivas submedida in MIDAS.Models.Datos.ListarSubMedidas(int.Parse(item.Cells[0].Text)))
                                    {   %>
                                 </br>
                                <%="-" + submedida.descripcion %>

                                <%} %>
                            </td>
                            

                            <td class="task-desc">
                                <% var medidasPreventivasImagen = MIDAS.Models.Datos.obtenerMedidasPreventivasImagenes(int.Parse(item.Cells[0].Text));
                                    if (medidasPreventivasImagen != null)
                                    {
                                        if (medidasPreventivasImagen.tamano == false)
                                        { %>
                                <img src=" <%= medidasPreventivasImagen.rutaImagen  %>" style="width: 60px; height: 60px;" />
                                <%}
                                        else { }
                                    }%>
                            </td>
                            <td class="task-desc">
                                <% if (medidasPreventivasImagen != null)
                                    {
                                        if (medidasPreventivasImagen.tamano == true)
                                        { %>
                                <img src=" <%= medidasPreventivasImagen.rutaImagen  %>" style="width: 60px; height: 60px;" />
                                <%}
                                        else { }
                                    }%>
                            </td>

                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <td class="text-center">
                                <a href="/evr/Configuracion/editar_medida_preventiva/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                            <td class="text-center">
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta medida?')) return false;" href="/evr/Configuracion/eliminar_medida_preventiva/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
        <a href="/evr/Configuracion/editar_medida_preventiva/0" title="Nueva Medida">
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


