<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    List<MIDAS.Models.noticias> listaNoticiasGenerales = new List<MIDAS.Models.noticias>();
    List<MIDAS.Models.noticias> listaNoticiasCentral = new List<MIDAS.Models.noticias>();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    protected void Page_Load(object sender, EventArgs e)
    {          
        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            //imgCentral.ImageUrl = "~/Imagenes/Central/" + centroseleccionado.id + ".jpg";
        }

        if (ViewData["noticiasGenerales"] != null)
            listaNoticiasGenerales = (List<MIDAS.Models.noticias>)ViewData["noticiasGenerales"];
        if (ViewData["noticiasCentral"] != null)
            listaNoticiasCentral = (List<MIDAS.Models.noticias>)ViewData["noticiasCentral"];
    }

</script>
<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page header -->
    <div class="page-header">
        
    </div>
    
    <div class="page-header">
		<div class="page-title">
            <h3>
                Noticias
            </h3>
        </div>  
    </div>

    <% 
        if (listaNoticiasGenerales != null && listaNoticiasGenerales.Count != 0)
       { %>

       <div class="panel panel-default">
								<div   class="panel-heading"> <h6 style="font-size:13px" class="panel-title"><i class="icon-newspaper"></i>Noticias generales</h6></div>
								<div class="panel-body">
                                      <br />
       <%   
           int id = 0;
           foreach (MIDAS.Models.noticias notic in listaNoticiasGenerales)
           {
           %>
       <div  class="panel panel-default">
        <div onclick="if($('#panelnoticia<%= id %>').css('display') == 'none'){$('#panelnoticia<%= id %>').show('slow');}else{$('#panelnoticia<%= id %>').hide('slow');}" style="cursor:pointer; background-color:#5089ff" class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-point-up"></i><%= notic.titulo + " (" + ((DateTime)notic.fecha).Date.ToShortDateString() + ")"%></h6>
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
        <div id="panelnoticia<%= id %>" style="display:none;padding:15px; background-color:White;">
           <span> <%=  notic.texto %></span>
            <br />
        </div>
        </div>
        <%
            id++;
        }
            %>
             <br />
               </div> 
                </div> 
                <%
       } %>
       <br />
                               
       <% 
           if (listaNoticiasCentral != null && listaNoticiasCentral.Count != 0)
       { %>

       <div class="panel panel-default">
								<div   class="panel-heading"> <h6 style="font-size:13px" class="panel-title"><i class="icon-newspaper"></i>Noticias de la central</h6></div>
								<div class="panel-body">
                                      <br />
       <%   
           int id = 0;
           foreach (MIDAS.Models.noticias notic in listaNoticiasCentral)
           {
           %>
       <div  class="panel panel-default">
        <div onclick="if($('#panelnoticiaCentral<%= id %>').css('display') == 'none'){$('#panelnoticiaCentral<%= id %>').show('slow');}else{$('#panelnoticiaCentral<%= id %>').hide('slow');}" style="cursor:pointer; background-color:#5089ff" class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-point-up"></i><%= notic.titulo + " (" + ((DateTime)notic.fecha).Date.ToShortDateString() + ")"%></h6>
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
        <div id="panelnoticiaCentral<%= id %>" style="display:none;padding:15px; background-color:White;">
           <span> <%=  notic.texto %></span>
           <br />
        </div>
        </div>
        <%
            id++;
        }
            %>
             <br />
               </div> 
                </div> 
                <%
       } %>




       
</asp:Content>
