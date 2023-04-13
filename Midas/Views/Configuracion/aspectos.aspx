<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();    
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["aspectos"];
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
					<h3>Listado de Aspectos Ambientales<small>Catálogo de aspectos ambientales registrados en el sistema</small></h3>
				</div>
			</div>
			<!-- /page header -->

                        
                        <form id="form1" runat="server">
                            <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>
                        
				        <!-- Tasks table -->
				        	<div class="block">
                                <center>
					            <div style="width:95%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr> <th>Código</th>   
					                            <th>Grupo</th>   
                                                
                                                <th style="width:190px">Identificación</th>  
                                                <th style="width:120px">Unidad</th>    
              

					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr><td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>   
                                                       
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[4].Text %>
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
                        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
                        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
                        </div>
                        </form>
                        <p>
                            <br />
                        </p>
			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->


</asp:Content>
