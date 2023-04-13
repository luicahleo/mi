<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Ocurrió un problema al realizar su solicitud.</h3>
				</div>
			</div>
			<!-- /page header -->
			
            <div class="callout callout-danger fade in">
				<h5><%=ViewData["Error"].ToString()%></h5>
				<p>Favor corroborar informacion.</p>
			</div>

</asp:Content>
