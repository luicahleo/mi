<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    List<MIDAS.Models.VISTA_ListarCentrales> centros = new List<MIDAS.Models.VISTA_ListarCentrales>();
    //List<MIDAS.Models.Vista_listar> centros = new List<MIDAS.Models.VISTA_ListarCentrales>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            centros = MIDAS.Models.Datos.ListarCentros();
            DatosPedidos.DataSource = ViewData["centrales"];
            DatosPedidos.DataBind();
            DatosProvincias.DataSource = ViewData["provincias"];
            DatosProvincias.DataBind();

            DatosComunidades.DataSource = ViewData["comunidades"];
            DatosComunidades.DataBind();
        }

        if (Session["EditarCentralesResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarCentralesResultado"].ToString() + "' });", true);
            Session.Remove("EditarCentralesResultado");
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
            <h3>INSTALACIONES <small>Instalaciones registrados en el sistema</small></h3>
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
                            <%--<th style="width: 45px">Siglas</th>--%>
                            <th>Instalación</th>
                            <%--                                                <th style="width:95px">Unidad de negocio</th>
                                                <th style="width:95px">Com. Autónoma</th>--%>
                            <th style="width: 60px">Zona</th>
                            <th style="width: 60px">Agrupacion</th>
                            <th style="width: 60px">Direccion</th>
                            <th style="width: 60px">Localización</th>
                            <th style="width: 60px">Tecnologia</th>
                            <th style="width: 100px">I.Centro</th>
                            <th style="width: 100px">I.Logo</th>
                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>

                            <th style="width: 45px">Editar</th>
                            <th style="width: 45px">Borrar</th>
                            <% } %>
                        </tr>
                    </thead>
                    <tbody>
                        <% 


                            List<MIDAS.Models.comunidad_autonoma> comunidades = (List<MIDAS.Models.comunidad_autonoma>)ViewData["comunidades"];
                            List<MIDAS.Models.provincia> provincias = (List<MIDAS.Models.provincia>)ViewData["provincias"];
                            List<MIDAS.Models.centros> centros = (List<MIDAS.Models.centros>)ViewData["centrales"];
                            List<MIDAS.Models.VISTA_ListaCentroZona> centroZona = (List<MIDAS.Models.VISTA_ListaCentroZona>)ViewData["listaCentrosZonas"];
                            List<MIDAS.Models.VISTA_ListaCentroZonaAgrupacion> centroZonaAgrupacion = (List<MIDAS.Models.VISTA_ListaCentroZonaAgrupacion>)ViewData["listaCentrosZonasAgrupacion"];

                            // List<MIDAS.Models.areas_imagenes> imagenes_areas =  (List<MIDAS.Models.areas_imagenes>)ViewData["imagenesAreas"];

                            foreach (GridViewRow item in DatosPedidos.Rows)
                            { %>
                        <tr>

                            <td class="task-desc">
                                <%= item.Cells[2].Text %>
                            </td>
                            <td class="task-desc">
                                <% var cz = centroZona.Where(x => x.id_centro == int.Parse(item.Cells[0].Text)).Select(x => x.zona_nombre).FirstOrDefault();  %>
                                <%= cz  %>
                            </td>

                            <%var cza = centroZonaAgrupacion.Where(x => x.id_centro == int.Parse(item.Cells[0].Text)).Select(x => x.nombre_agrupacion).FirstOrDefault(); %>

                            <td class="task-desc">
                                <%= cza %> 
                            </td>

                            <td class="task-desc"><%= item.Cells[7].Text%>
                            </td>
                            <td class="task-desc"><%= item.Cells[8].Text%>
                            </td>
                            <td>
                                <% 

                                    var tc = MIDAS.Models.Datos.ObtenerTecnologiaCentro(int.Parse(item.Cells[0].Text)); %>
                                <%= tc.nombre  %>
                            </td>

                            <td class="task-desc">
                                <%  

                                    if (!string.IsNullOrEmpty(item.Cells[6].Text) && item.Cells[6].Text != "&nbsp;")
                                    { %>
                                <img src=" <%= item.Cells[6].Text%>" style="width: 80px; height: 80px;" />
                                <%} %>
                            </td>
                            <td class="task-desc">
                                <%  

                                    if (!string.IsNullOrEmpty(item.Cells[9].Text) && item.Cells[9].Text != "&nbsp;")
                                    { %>
                                <img src=" <%= item.Cells[9].Text%>" style="width: 80px; height: 20px;" />
                                <%} %>
                            </td>
                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <td class="text-center">
                                <a href="/evr/Configuracion/editar_centro/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                            <td class="text-center">
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta central?')) return false;" href="/evr/Configuracion/eliminar_centro/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
        <a href="/evr/Configuracion/editar_centro/0" title="Nueva Central">
            <button class="btn btn-primary">Nueva Instalación</button></a>
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

