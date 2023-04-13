<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    
    protected void Page_Load(object sender, EventArgs e)
    {
        gvSectores.DataSource = ViewData["sectores"];
        gvSectores.DataBind();

        if (Session["EdicionSectorMensaje"] == "GUARDADOSECTOR")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Sector Guardado.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionSectorMensaje"] == "ELIMINADOSECTOR")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Sector Desactivado.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        Session["EdicionSectorMensaje"] = null;
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Sectores <small>Sectores registrados en el sistema</small></h3>
				</div>
			</div>
			<!-- /page header -->

                        <form id="form1" runat="server">
                            <asp:GridView ID="gvSectores" runat="server" Visible="false">
                            </asp:GridView>
                        </form>


				        <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:95%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
					                            <th>Sector</th>
                                                <th>Descripción</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Baja</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in gvSectores.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>   
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_sector/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_sector/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_sector/0" title="Nuevo Sector"><button class="btn btn-primary" >Nuevo Sector</button></a>
                        <a href="/evr/Home/Principal/0" title="Volver"><button class="btn btn-primary" >Volver</button></a>
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
