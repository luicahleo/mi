<%@ Page Title="" Language="C#" 
    Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="/Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/londinium-theme.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&amp;subset=latin,cyrillic-ext"
        rel="stylesheet" type="text/css" />
    <script src="http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.4/jquery.js"></script>
    <link href="/dist/summernote.css" rel="stylesheet"/>
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

    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
        
    protected void Page_Load(object sender, EventArgs e)
    {
        List<MIDAS.Models.procesos> listaPadres = new List<MIDAS.Models.procesos>();

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }

        }
    }

</script>


    <style>
        .Rotate-90
        {
          -webkit-transform: rotate(-90deg);
          -moz-transform: rotate(-90deg);
          -ms-transform: rotate(-90deg);
          -o-transform: rotate(-90deg);
          transform: rotate(-90deg);
 
          -webkit-transform-origin: 50% 50%;
          -moz-transform-origin: 50% 50%;
          -ms-transform-origin: 50% 50%;
          -o-transform-origin: 50% 50%;
          transform-origin: 50% 50%;
 
          font-size: 35px;
          font-color:#41b9e6;
          color:#41b9e6;
          width: 100px;
          position: relative;
        }
    </style>
    <script type="text/javascript">


        $(document).ready(function () {

            $("#divFicha").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarElemento")
                    $("#hdFormularioEjecutado").val("GuardarElemento");


            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });


            $('#ctl00_MainContent_ddlNivel').change(function () {
                $("#hdFormularioEjecutado").val("CambiaNivel");
            });


        });

       
    </script>
    <title>DIMAS</title>
</head>
<body>
    <form runat="server" id="form1">
    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
    <!-- Page header -->
    <div class="page-header">


                       
                       <%--e5f6ff--%>
                        <div id="canvas" style="background-color:#FFF; padding:15px; border-radius:25px;">
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>
                              
<% 
                                List<MIDAS.Models.procesos> listaProcesos1 = (List<MIDAS.Models.procesos>)ViewData["procesosE"];
                                List<MIDAS.Models.procesos> listaProcesosHijos = (List<MIDAS.Models.procesos>)ViewData["procesosT"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos1)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <table>
                                                    <%
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    <tr>
                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#ede6e8; min-height:70px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                         
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                     
                                                                                     <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   
                                                                            }
                                                                        %> 

                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                        </tr>
                                                    <% } %>
                                                    </table>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table width="100%">
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; min-height:70px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Estratégicos</div>
                             </td></tr></table>
                             <%--FIN PROCESOS ESTRATEGICOS--%>
                                  <hr style="border-color:#41b9e6;" />

                            <%--INICIO PROCESOS OPERATIVOS--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>
                              
<% 
                                List<MIDAS.Models.procesos> listaProcesos2 = (List<MIDAS.Models.procesos>)ViewData["procesosO"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos2)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <center>
                                                   <table>
                                                   <tr>
                                                    <%
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    
                                                        <td style="width:<%= 100/listaHijos2.Count %>%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#ede6e8; max-width:250px; min-width:250px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px; min-height:70px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                     <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   
                                                                            }
                                                                        %> 

                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                       
                                                    <% } %> </tr>
                                                    </table></center>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table>
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Operativos</div>
                             </td></tr></table>

                                   <hr  style="border-color:#41b9e6;" />

                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>
                              
<% 
                                List<MIDAS.Models.procesos> listaProcesos3 = (List<MIDAS.Models.procesos>)ViewData["procesosS"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos3)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="100%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; margin-top:15px ; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <table width="100%"><tr>
                                                    <%
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    
                                                        <td style="width:<%= 100/listaHijos2.Count %>%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#ede6e8; margin-right:15px; min-width:200px; min-height:70px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px;background-color:#41b9e6; min-height:70px; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                    
                                                                                     <a style="text-decoration:none; color:#000" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   
                                                                            }
                                                                        %> 

                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                        
                                                    <% } %></tr>
                                                    </table>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                               </tr></tr>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos3.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table>
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos3.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>

                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Soporte</div>
                             </td></tr></table>
                    </div>


                    </div>
</form>
   
        

   
    

</body>
</html>
