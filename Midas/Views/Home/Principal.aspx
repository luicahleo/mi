<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    List<MIDAS.Models.noticias> listaNoticiasGenerales = new List<MIDAS.Models.noticias>();
    List<MIDAS.Models.noticias> listaNoticiasCentral = new List<MIDAS.Models.noticias>();
    List<MIDAS.Models.version_matriz> version_Matriz = new List<MIDAS.Models.version_matriz>();

    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            imgCentral.ImageUrl = "~/Imagenes/Central/1.jpg";

        }

        if (ViewData["noticiasGenerales"] != null)
            listaNoticiasGenerales = (List<MIDAS.Models.noticias>)ViewData["noticiasGenerales"];
        if (ViewData["noticiasCentral"] != null)
            listaNoticiasCentral = (List<MIDAS.Models.noticias>)ViewData["noticiasCentral"];


        if (Session["GenerarDocumento"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["GenerarDocumento"].ToString() + "' });", true);
            Session["GenerarDocumento"] = null;
        }

        if (Session["DescripcionCentro"] == null && Session["VersionMatriz"] != null)
        {
            var linea = "Error la matriz sin descripción";
            //Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + "ERROR EN EL REGISTRO, CONTIENE MATRIZ PERO NO DESCRIPCION!!!" + "' });", true);
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + linea + "' });", true);
        }

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
            <h3></h3>
            <br />
        </div>
    </div>
    <asp:Image Style="position: absolute; z-index: -1; width: 100%; opacity: 0.50; margin-top: -15px" ID="imgCentral" runat="server" />
    <table width="100%;">
        <tr>
            <% 
                if (listaNoticiasGenerales != null && listaNoticiasGenerales.Count != 0)
                { %>

            <% if (listaNoticiasCentral == null || listaNoticiasCentral.Count == 0)
                {%>
            <td style="width: 100%; vertical-align: top">
                <% }
                    else
                    { %>
            <td style="width: 50%; vertical-align: top">
                <% } %>
                <div style="width: 98%; background: transparent; border: none; margin-top: -15px" class="panel panel-default">
                    <div class="panel-heading">
                        <h6 style="font-size: 13px" class="panel-title"><i class="icon-newspaper"></i>Noticias</h6>
                    </div>
                    <div class="panel-body">

                        <table width="100%">
                            <%   
                                int id = 0;


                                foreach (MIDAS.Models.noticias notic in listaNoticiasGenerales)
                                {
                            %>

                            <tr style="padding-bottom: 20px; margin: 20px; min-height: 300px;">

                                <td style="width: 100%; height: 100%; vertical-align: top">
                                    <br />
                                    <div style="width: 100%; height: 100%" class="panel panel-default">
                                        <div style="background-color: #5089ff" class="panel-heading">
                                            <h6 class="panel-title">
                                                <i class="icon-newspaper"></i><%= notic.titulo + " (" + ((DateTime)notic.fecha).Date.ToShortDateString() + ")"%></h6>
                                        </div>
                                        <div id="panelnoticia<%= id %>" style="padding: 15px; min-height: 50px; background-color: White;">
                                            <% string imagencabecera;
                                                imagencabecera = "http://localhost:62348/evr/cabeceras" + "/" + notic.id + "/" + notic.cabecera;
                                            %>
                                            <center>
                                                <table width="100%">
                                                    <tr>
                                                        <% if (notic.cabecera != null)
                                                            { %>
                                                        <td>
                                                            <center>
                                                                <img style="max-width: 255px; max-height: 90px" src="<%= imagencabecera %>" /></center>
                                                        </td>
                                                        <% } %>
                                                        <td style="padding-left: 15px; vertical-align: top; text-align: justify; padding-right: 15px">
                                                            <span><% if (notic.texto != null && notic.texto.Count() > 300)
                                                                      { %> <%=  notic.texto.Substring(0, 300)%>... <b><a target="_blank" onclick="window.open(this.href, this.target, 'width=650,height=450'); return false;" href="/evr/Noticias/leer_noticia_popup/<%= notic.id %>" title="Volver">Leer más</a></b> <% } %>
                                                                <% else
                                                                    { %> <%=  notic.texto %> <% } %></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>

                                        </div>
                                    </div>
                                </td>

                            </tr>

                            <%
                                    id++;

                                }


                            %>
                        </table>
                        <br />
                    </div>
                </div>
            </td>
            <%
                } %>

            <% 
                if (listaNoticiasCentral != null && listaNoticiasCentral.Count != 0)
                { %>
            <% if (listaNoticiasGenerales == null || listaNoticiasGenerales.Count == 0)
                {%>
            <td style="width: 100%; vertical-align: top">
                <% }
                    else
                    { %>
            <td style="width: 50%; vertical-align: top">
                <% } %>
                <div style="width: 100%; background: transparent; border: none; margin-top: -15px" class="panel panel-default">
                    <div class="panel-heading">
                        <h6 style="font-size: 13px" class="panel-title"><i class="icon-newspaper"></i>Noticias de la central</h6>
                    </div>
                    <div class="panel-body">

                        <table width="100%">
                            <%   
                                int id = 0;


                                foreach (MIDAS.Models.noticias notic in listaNoticiasCentral)
                                {
                            %>

                            <tr style="padding-bottom: 20px; margin: 20px; min-height: 300px">

                                <td style="width: 50%; height: 100%">
                                    <br />
                                    <div style="width: 100%; height: 100%" class="panel panel-default">
                                        <div style="background-color: #5089ff" class="panel-heading">
                                            <h6 class="panel-title">
                                                <i class="icon-newspaper"></i><%= notic.titulo + " (" + ((DateTime)notic.fecha).Date.ToShortDateString() + ")"%></h6>
                                        </div>
                                        <div id="panelNoticiaCentral" style="padding: 15px; min-height: 50px; background-color: White;">
                                            <% string imagencabecera;
                                                imagencabecera = "http://novotecsevilla.westeurope.cloudapp.azure.com/evr/cabeceras" + "/" + notic.id + "/" + notic.cabecera;
                                            %>
                                            <center>
                                                <table width="100%">
                                                    <tr>
                                                        <% if (notic.cabecera != null)
                                                            { %>
                                                        <td>
                                                            <center>
                                                                <img style="max-width: 255px; max-height: 90px" src="<%= imagencabecera %>" /></center>
                                                        </td>
                                                        <% } %>
                                                        <td style="padding-left: 15px; vertical-align: top; text-align: justify; padding-right: 15px">
                                                            <span><% if (notic.texto != null && notic.texto.Count() > 300)
                                                                      { %> <%=  notic.texto.Substring(0, 300)%>... <b><a target="_blank" onclick="window.open(this.href, this.target, 'width=650,height=450'); return false;" href="http://novotecsevilla.westeurope.cloudapp.azure.com/evr/Noticias/leer_noticia_popup/<%= notic.id %>" title="Volver">Leer más</a></b>  <% } %>
                                                                <% else
                                                                    { %> <%=  notic.texto %> <% } %></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>

                                        </div>
                                    </div>
                                </td>

                            </tr>

                            <%
                                    id++;

                                }


                            %>
                        </table>
                        <br />
                    </div>
                </div>
            </td>
            <%
                } %>
        </tr>
    </table>




</asp:Content>
