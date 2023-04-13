<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["auditores"];
        DatosPedidos.DataBind();
        
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        //if (Session["EditarAuditoresResultado"] != null)
        //{
        //    Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAuditoresResultado"].ToString() + "' });", true);
        //    Session["EditarAuditoresResultado"] = null;
        //}
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Auditores<small>Auditores registrados en el sistema</small></h3>
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
					                            <th>Nombre</th>    
                                                <th>Empresa</th>                                         
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
                                                    <%= item.Cells[1].Text %>
                                                </td>   
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>                                            
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/configuracion/editar_auditor/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/configuracion/eliminar_auditor/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/configuracion/editar_auditor/0" title="Nuevo auditor"><button class="btn btn-primary" >Nuevo auditor</button></a>
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
