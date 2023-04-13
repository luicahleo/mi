<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosConsultas.DataSource = ViewData["consultasrecibidas"];
        DatosConsultas.DataBind();


    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Consultas</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Consultas<small>Consultas registradas en el sistema</small></h3>
				</div>
			</div>
			<!-- /page header -->

                        <form id="form1" runat="server">
                            <asp:GridView ID="DatosConsultas" runat="server" Visible="false">
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
                                                <th>Usuario</th>
                                                <th>F.Consulta</th>
                                                <th>F.Respuesta</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in DatosConsultas.Rows)
                                                     { %>
                                            <tr>
                                            <% if (item.Cells[9].Text != "1")
                                               { %>
                                                <td style="font-weight:bold" class="task-desc">

                                                    <%= item.Cells[1].Text%>
                                                </td>  
                                                <%}
                                               else
                                               { %>
                                                <td class="task-desc">

                                                    <%= item.Cells[1].Text%>
                                                </td> 
                                                  <%} %>
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>
                                                <td style="text-align:center" class="task-desc">
                                                    <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                </td>
                                                <td style="text-align:center" class="task-desc">
                                                <% if (item.Cells[7].Text != "&nbsp;")
                                                   { %>
                                                    <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                    <%} %>
                                                </td>                                                
                                                <td class="text-center">
                                                    <a href="/evr/Home/detalle_consulta/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar la noticia?');" href="/evr/Home/eliminar_consulta/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                </td>
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
                        <% if (Session["organizacion"] != null)
                           { %>
                        <a href="/evr/Home/detalle_consulta/0" title="Nueva Consulta"><button class="btn btn-primary" >Nueva Consulta</button></a>
                        <%} %>
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
