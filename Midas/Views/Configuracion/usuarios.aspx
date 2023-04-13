<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["usuarios"];
        DatosPedidos.DataBind();

        if (Session["EditarUsuarioResultado"] == "GUARDADOUSUARIO")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Usuario Guardado.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EditarUsuarioResultado"] == "ELIMINADOUSUARIO")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Usuario Desactivado.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);
        else if (Session["EditarUsuarioResultado"] == "HABILITADOUSUARIO")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Usuario Habilitado.', { theme: 'growl-success', header: 'ALTA!' });", true);
        Session["EditarUsuarioResultado"] = null;
        
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (Session["CentralElegida"] != null)
            {
                MIDAS.Models.centros cent = MIDAS.Models.Datos.ObtenerCentro(int.Parse(Session["CentralElegida"].ToString()));
                
                if (cent.tipo != 4)
                    lblNombreCentro.Text = cent.nombre;
                else
                    lblNombreCentro.Text = "Todos los centros";
            }
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
					<h3>Usuarios - <asp:Label runat="server" ID="lblNombreCentro"></asp:Label> <small>Usuarios registrados en el sistema</small></h3>
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
					                            <th>Usuario</th>
                                                <th>F. Registro</th>
                                                <th>Perfil</th>                                                
                                                <th>Estado</th>
                                                 <% if (user.perfil == 1)
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
                                                <td style="text-align:center" class="task-desc">
                                                    <%= (DateTime.Parse(item.Cells[4].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Year).ToString()%>
                                                </td>
                                                <td style="text-align:center" class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>
                                                
                                                <% 
                                                   if (item.Cells[5].Text == "true")
                                                   { %>
                                                <td class="task-desc">
                                                <center>
                                                    <label style="color:Red;">Inactivo</label>
                                                    </center>
                                                </td>
                                                <% }
                                                   else
                                                   { %>
                                                <td class="task-desc">
                                                <center>
                                                    <label style="color:Green;">Activo</label>         
                                                    </center>                                           
                                                </td>
                                                <%} %>
                                                <% if (user.perfil == 1)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/Configuracion/editar_usuario/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <% 
                                                    if (item.Cells[5].Text == "true")
                                                    { %>

                                                    <a onclick="return confirm('¿Está seguro que desea habilitar el usuario?');" href="/evr/Configuracion/hab_usuario/<%= item.Cells[0].Text %>" title="Habilitar"><i class="icon-checkmark"></i></a>
                                                    <% }
                                                    else
                                                    { %>

                                                         <a onclick="return confirm('¿Está seguro que desea desactivar el usuario?');" href="/evr/Configuracion/eliminar_usuario/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>

                                                       <% } %>

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
                        <a href="/evr/Configuracion/editar_usuario/0" title="Nuevo Usuario"><button class="btn btn-primary" >Nuevo Usuario</button></a>
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
