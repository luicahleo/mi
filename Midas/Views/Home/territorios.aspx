<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["paises"];
        DatosPedidos.DataBind();

        gvComunidades.DataSource = ViewData["comunidades"];
        gvComunidades.DataBind();

        gvProvincias.DataSource = ViewData["provincias"];
        gvProvincias.DataBind();

        gvLocalidades.DataSource = ViewData["localidades"];
        gvLocalidades.DataBind();

        if (Session["EdicionPaisMensaje"] == "GUARDADOPAIS")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Pais Creado.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionPaisMensaje"] == "ELIMINADOPAIS")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('País Eliminado.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        if (Session["EdicionCCAAMensaje"] == "GUARDADOCCAA")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Comunidad Creada.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionCCAAMensaje"] == "ELIMINADOCCAA")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Comunidad Eliminada.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        if (Session["EdicionProvinciaMensaje"] == "GUARDADOPROVINCIA")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Provincia Creada.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionProvinciaMensaje"] == "ELIMINADOPROVINCIA")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Provincia Eliminada.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        if (Session["EdicionLocalidadMensaje"] == "GUARDADOLOCALIDAD")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Localidad Creada.', { theme: 'growl-success', header: 'GUARDADO!' });", true);
        else if (Session["EdicionLocalidadMensaje"] == "ELIMINADOLOCALIDAD")
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('Localidad Eliminada.', { theme: 'growl-success', header: 'ELIMINADO!' });", true);

        Session["EdicionPaisMensaje"] = null;
        Session["EdicionCCAAMensaje"] = null;
        Session["EdicionProvinciaMensaje"] = null;
        Session["EdicionLocalidadMensaje"] = null;
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Paises</h3>
				</div>
			</div>
			<!-- /page header -->

                        <form id="form1" runat="server">
                            <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>
                            <asp:GridView ID="gvComunidades" runat="server" Visible="false">
                            </asp:GridView>
                            <asp:GridView ID="gvProvincias" runat="server" Visible="false">
                            </asp:GridView>
                            <asp:GridView ID="gvLocalidades" runat="server" Visible="false">
                            </asp:GridView>
                        </form>


				        <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:85%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                                <%--<th style="width:20px"></th>--%>
                                                <th>País</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr>
                                                <%--<td class="task-desc">
                                                    <input id="Radio<%= item.Cells[0].Text %>" type="radio" />
                                                </td>--%>
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_pais/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_pais/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_pais/0" title="Nuevo País"><button class="btn btn-primary" >Nuevo País</button></a>
                        </div>
                        <p>
                            <br />
                        </p>
                        <div class="page-header">
				            <div class="page-title">
					            <h3>Comunidades Autónomas</h3>
				            </div>
			            </div>

                         <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:85%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                               <th>País</th>
                                                <th>C. Autónoma</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% foreach (GridViewRow item in gvComunidades.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_comaut/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_comaut/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_comaut/0" title="Nueva Comunidad"><button class="btn btn-primary" >Nueva Comunidad</button></a>
                        </div>
                        <p>
                            <br />
                        </p>

                        <div class="page-header">
				            <div class="page-title">
					            <h3>Provincias</h3>
				            </div>
			            </div>

                         <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:85%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                               <%-- <th style="width:20px"></th>--%>
                                                <th>País</th>
                                                <th>Com. Autónoma</th>
                                                <th>Provincia</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% foreach (GridViewRow item in gvProvincias.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[4].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_provincia/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_provincia/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_provincia/0" title="Nueva Provincia"><button class="btn btn-primary" >Nueva Provincia</button></a>
                        </div>
                        <p>
                            <br />
                        </p>

                        <div class="page-header">
				            <div class="page-title">
					            <h3>Localidades</h3>
				            </div>
			            </div>

                         <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:85%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                               <%-- <th style="width:20px"></th>--%>
                                               <th>País</th>
                                               <th>Comunidad Autónoma</th>
                                                <th>Provincia</th>
                                                <th>Localidad</th>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% foreach (GridViewRow item in gvLocalidades.Rows)
                                                     { %>
                                            <tr>
                                            <td class="task-desc">
                                                    <%= item.Cells[6].Text %>
                                                </td>
                                            <td class="task-desc">
                                                    <%= item.Cells[5].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[4].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/editar_localidad/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a href="/evr/Home/eliminar_localidad/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
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
                        <a href="/evr/Home/editar_localidad/0" title="Nueva Localidad"><button class="btn btn-primary" >Nueva Localidad</button></a>
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
