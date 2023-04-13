<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["indicadores"];
        DatosPedidos.DataBind();
        
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        
                

        if (Session["EditarIndicadoresResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarIndicadoresResultado"].ToString() + "' });", true);
            Session["EditarIndicadoresResultado"] = null;
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
					<h3>Indicadores<small>Catálogo de indicadores registrados en el sistema</small></h3>
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
                                                <th>Tecnología</th>   
                                                <th>Centro</th>
                                                <th>Unidad</th>    
                                                <th>Proceso</th>        
                                                <th>Activo</th>                            
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
                                                    <%= item.Cells[8].Text %>
                                                </td>    
                                                <td class="task-desc">
                                                    <%= item.Cells[10].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[4].Text %>
                                                </td>
                                                
                                                <td class="task-desc">
                                                    <%= item.Cells[7].Text %>
                                                </td>
                                                <% if (item.Cells[6].Text == "Activo")
                                                   { %>
                                                <td style="color:Green; font-weight:bold" class="task-desc">
                                                    <%= item.Cells[6].Text %>
                                                </td>         
                                                <% }
                                                else
                                                { %>   
                                                    <td style="color:Red; font-weight:bold" class="task-desc">
                                                    <%= item.Cells[6].Text %>
                                                </td>   
                                                <% } %>                     
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/configuracion/editar_indicador/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <% if (item.Cells[6].Text == "Activo")
                                                   { %>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea desactivar el registro?');" href="/evr/configuracion/eliminar_indicador/<%= item.Cells[0].Text %>" title="Desactivar"><i class="icon-remove"></i></a>
                                                </td>
                                                <% }
                                                   else
                                                   { %>
                                                   <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea activar el registro?');" href="/evr/configuracion/activar_indicador/<%= item.Cells[0].Text %>" title="Activar"><i class="icon-checkmark"></i></a>
                                                </td>

                                                <% }
                                                   } %>
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
                        <a href="/evr/configuracion/editar_indicador/0" title="Nuevo tipo"><button class="btn btn-primary" >Nuevo Indicador</button></a>
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
