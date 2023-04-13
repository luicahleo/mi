<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["indsec"];
        DatosPedidos.DataBind();

        if (Session["EdicionIndicadorMensaje"] == "GUARDADOINDICADOR")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Indicador Guardado.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionIndicadorMensaje"] == "ELIMINADOINDICADOR")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Indicador Eliminado.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        Session["EdicionIndicadorMensaje"] = null;
        Session["EditarPedidoResultado"] = null;
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Indicadores <small>Indicadores por sector</small></h3>
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
					            <div style="width:85%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                                <th>Sector</th>
					                            <th>Titulo</th>
                                                <th style="width:70px">Escala</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[6].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[5].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text + " - " + item.Cells[3].Text%>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_indsec/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_indsec/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_indsec/0" title="Nuevo Indicador"><button class="btn btn-primary" >Nuevo Indicador</button></a>
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
