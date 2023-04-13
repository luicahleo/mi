<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["noticias"];
        DatosPedidos.DataBind();

        if (Session["usuario"] != null)
        {
            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }   

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

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
					<h3>Noticias<small>Noticias registradas en el sistema</small></h3>
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
                                                <th>Titulo</th>         
                                                <th style="width:150px">Central</th>                                       
                                                <th style="width:120px">Fecha</th>
                                                <th style="width:120px">F. Expiración</th>    
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   { %>                                              
                                                <th style="width:80px">Editar</th>
                                                <% } %>
                                                <th style="width:80px">Consultar</th>
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   { %>  
                                                <th style="width:45px">Eliminar</th>
                                                <% } %>
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
                                                <% 
                                                   if (item.Cells[4].Text == "&nbsp;")
                                                   { %>
                                                <td style="text-align:center" class="task-desc">
                                                    Sede Central Madrid
                                                </td>
                                                <% }
                                                   else
                                                   { %>
                                                   <td style="text-align:center" class="task-desc">
                                                    <%= centroseleccionado.nombre%>
                                                </td>
                                                <% } %>

                                                <td style="text-align:center" class="task-desc">
                                                    <%= (DateTime.Parse(item.Cells[3].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[3].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[3].Text).Year).ToString()%>
                                                </td>
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   { %>  
                                                <td style="text-align:center" class="task-desc">
                                                    <% if (item.Cells[5].Text != "&nbsp;")
                                                       { %>
                                                    <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                    <% } %>
                                                </td><% } %>
                                                <td class="text-center">
                                                <% 
                                                   if ((item.Cells[4].Text == "&nbsp;" && user.perfil == 1 && centroseleccionado.tipo == 4) || ((user.perfil == 1 ||user.perfil == 3) && centroseleccionado.id.ToString() == item.Cells[4].Text))
                                                   { %>  
                                                    <a href="/evr/noticias/editar_noticia/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                    <% } %>
                                                </td>
                                                <td class="text-center">
                                                    <a target="_blank" onclick="window.open(this.href, this.target, 'width=650,height=450'); return false;" href="http://novotecsevilla.westeurope.cloudapp.azure.com/evr/Noticias/leer_noticia_popup/<%= item.Cells[0].Text %>" title="Leer"><i class="icon-search"></i></a>
                                                </td>
                                               <% if (user.perfil == 1 || user.perfil == 3)
                                                  { %>  
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar la noticia?');" href="/evr/noticias/eliminar_noticia/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <%--<form id="UploadForm" action="/completa/Home/editar_pedido/0" method="post">--%>
                       <%-- <input id="NuevaCentral" type="submit" value="Nueva Central" class="btn btn-primary run-first" />--%>
                        <%--</form>--%>
                        <a href="/evr/noticias/editar_noticia/0" title="Nuevo Usuario"><button class="btn btn-primary" >Nueva Noticia</button></a>
                         <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
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
