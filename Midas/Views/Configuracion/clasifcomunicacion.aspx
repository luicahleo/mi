<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["clasifcomunicacion"];
        DatosPedidos.DataBind();
        
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["EditarClasificacionesComunicacionResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarClasificacionesComunicacionResultado"].ToString() + "' });", true);
            Session["EditarClasificacionesComunicacionResultado"] = null;
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
					<h3>Clasificaciones de comunicación<small>Clasificaciones de comunicación registrados en el sistema</small></h3>
				</div>
			</div>
			<!-- /page header -->

                        <form id="form1" runat="server">
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
					                            <th>Tipo</th>                                         
                                                 <% if (user.perfil == 1 || user.perfil == 3)
                                                    { %>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Baja</th>
                                                <%} %>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>                                             
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/configuracion/editar_clasifcomunicacion/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/configuracion/eliminar_clasifcomunicacion/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        
                        <div style="text-align:right">
                        <% if (user.perfil == 1 || user.perfil == 3)
                           { %>
                        <a href="/evr/configuracion/editar_clasifcomunicacion/0" title="Nueva clasificación de comunicación"><button class="btn btn-primary" >Nueva clasificación de comunicación</button></a>
                        <% } %>
                        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
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
