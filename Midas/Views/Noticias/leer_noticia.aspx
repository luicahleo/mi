<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.noticias notic = new MIDAS.Models.noticias();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        }
        notic = (MIDAS.Models.noticias)ViewData["noticia"];
    }

</script>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page header -->
    <br />
    <div class="panel panel-default">
        <div  class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-newspaper"></i><%= notic.titulo + " (" + ((DateTime)notic.fecha).Date.ToShortDateString() + ")"%></h6>
                <% if (notic.organizacion == null)
                   { %>
                        <div style="float:right; padding-right:10px">
                            <h6 class="panel-title">
                                Sede Social Madrid
                            </h6>
                        </div>
                   <% }
                   else
                   { %>
                        <div style="float:right; padding-right:10px">
                            <h6 class="panel-title">
                                <%= centroseleccionado.nombre %>
                            </h6>
                        </div>
                   <% } %>
        </div>
        <div id="demo" style="padding:15px; background-color:White;">
           <span> <%=  notic.texto %></span>
            <br />
        </div>
        </div>
    <br />
    <div style="text-align:right">
    <a href="/Noticias/noticias" class="btn btn-primary" >Volver</a></>
    </div>
    <!-- /page header -->
    <!-- Page tabs -->
   
        

   
    

    <!-- Footer -->
<%--    <div class="footer clearfix">
        <div class="pull-left">
            </div>
    </div>--%>
    <!-- /footer -->
</asp:Content>
