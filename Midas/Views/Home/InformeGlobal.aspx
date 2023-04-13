<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<script runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ViewData["tab"] == null)
            ViewData["tab"] = "Exportar";
    }
    
</script>

<asp:Content ID="InformeGlobalContent" ContentPlaceHolderID="MainContent" runat="server">
    
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Informe Global <small>Informe del estado actual de empresas y trabajadores</small></h3>
				</div>
			</div>
			<!-- /page header -->
			
	        <!-- Page tabs -->
            <div class="tabbable page-tabs">
                <ul class="nav nav-tabs">
                	<% if (ViewData["tab"].ToString() == "Exportar")
                    { %>
                	   <li class="active"><a href="#Exportar" data-toggle="tab"><i class="icon-file"></i> Exportar datos</a></li>
                	<%} %>
                	 
                	 
                    
                </ul>
                <div class="tab-content">

                	<!-- First tab -->
                	<% if (ViewData["tab"].ToString() == "Exportar")
                    { %>
                	    <div class="tab-pane active fade in" id="Exportar">
                	<%}
                    else
                    {%>
                	    <div class="tab-pane fade in" id="Exportar">
                	 <%}%>

				 
                        <% Html.RenderPartial("UserControls/ExportaInformeGlobal"); %>


                	</div>
                	<!-- /first tab -->

            	</div>
        	</div>
        	<!-- /page tabs -->	        


			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left">&copy; 2014. Novotec</div>
			</div>
			<!-- /footer -->

</asp:Content>
