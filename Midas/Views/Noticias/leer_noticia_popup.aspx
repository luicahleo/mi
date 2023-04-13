<%@ Page Title="" Language="C#" ValidateRequest="false"
    Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Midas-Noticia</title>
<link href="/evr/Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="/evr/Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/evr/Content/css/londinium-theme.css" rel="stylesheet" type="text/css" />
    <link href="/evr/Content/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="/evr/Content/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&amp;subset=latin,cyrillic-ext"
        rel="stylesheet" type="text/css" />
    <script src="http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.4/jquery.js"></script>
    <link href="/evr/dist/summernote.css" rel="stylesheet"/>
    <script src="/dist/summernote.js"></script>
    <script type="text/javascript" src="/evr/Content/js/jquery.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/charts/sparkline.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/uniform.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/select2.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/inputmask.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/autosize.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/inputlimit.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/listbox.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/multiselect.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/validate.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/tags.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/switch.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/uploader/plupload.full.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/uploader/plupload.queue.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/wysihtml5/wysihtml5.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/forms/wysihtml5/toolbar.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/daterangepicker.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/fancybox.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/moment.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/jgrowl.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/datatables.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/colorpicker.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/fullcalendar.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/timepicker.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/plugins/interface/collapsible.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/evr/Content/js/application.js"></script>
    <script type="text/javascript" src="/evr/Content/js/jquery.session.js"></script>

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
        //imgCabecera.ImageUrl = "http://novotecsevilla.westeurope.cloudapp.azure.com/evr/cabeceras" + "/" + notic.id + "/" + notic.cabecera;
          imgCabecera.ImageUrl = "http://localhost:62348/evr/cabeceras" + "/" + notic.id + "/" + notic.cabecera;
    }

</script>

</head>
<body>
    <!-- Page header -->
    <br />
    <div style="border:none" class="panel panel-default">
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
            <% if (notic != null && notic.cabecera != null)
                                                       { %>
                                                       <center>
                                                        <asp:Image style="max-width:255px" ID="imgCabecera"  runat="server" />
                                                        </center>
                                                    <% } %>
            <br />
           <span> <%=  notic.texto.Replace("\r\n", "<br/>") %></span>
            <br />
        </div>
        </div>

    <!-- /page header -->
    <!-- Page tabs -->
   
        

   
    

</body>
</html>
