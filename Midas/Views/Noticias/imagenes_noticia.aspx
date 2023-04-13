<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    protected void Page_Load(object sender, EventArgs e)
    {
        DatosPedidos.DataSource = ViewData["imagenesnoticia"];
        DatosPedidos.DataBind();

    }
</script>
<script type="text/javascript">

    $(document).ready(function () {
        if ($('#' + '<%= hdnCambiado.ClientID %>').val() == '1') {
            window.close();
        }


    });

    function copiarAlPortapapeles(cadena) {

        // Crea un campo de texto "oculto"
        var aux = document.createElement("input");

        // Asigna el contenido del elemento especificado al valor del campo
        aux.setAttribute("value", cadena);

        // Añade el campo a la página
        document.body.appendChild(aux);

        // Selecciona el contenido del campo
        aux.select();

        // Copia el texto seleccionado
        document.execCommand("copy");

        // Elimina el campo de la página
        document.body.removeChild(aux);

        $.jGrowl('', { theme: 'growl-success', header: 'Imagen copiada al portapapeles' });
    }
</script>
</head>
<body>
    <br />
    <center><h2>Imágenes adjuntas</h2></center>
    <br />
    <form  enctype="multipart/form-data" method="post"  id="Form1" runat="server">
    <center>
    <div style="padding:15px">
       <center>
       <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-upload"></i>Subir ficheros</h6>
        </div>
        <div class="panel-body">
        <center>
        <br />
        <input type="file" id="file" name="file" />
        <br />
        <input id="subir" class="btn btn-primary run-first" type="submit" value="Subir" />
        <br />

        <br />
        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
    </asp:GridView>
    <div class="block">
        <div class="datatablePedido">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>

                            Enlace del fichero
      
                        </th>      
                        <th  style="width:50px">
                        <center>
                            Copiar
                        </center>
                        </th>                  
                        <th  style="width:50px">
                        <center>
                            Ver
                            </center>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (GridViewRow item in DatosPedidos.Rows)
                       { %>
                    <tr>                        
                        <td class="text">
     
                            <%=  
                                item.Cells[2].Text
                                
                                %>
 
                        </td>   
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Copiar ruta" onclick="copiarAlPortapapeles('<%=item.Cells[2].Text %>'); return false;"><i class="icon-copy"></i></a>
                            </center>
                        </td>                        
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Ver Fichero" onclick="window.open(this.href, this.target, 'width=400,height=400'); return false;" target="_blank" href="<%=item.Cells[2].Text %>");"><i class="icon-search2"></i></a>
                            </center>
                        </td>      
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div></div></center></div></div></center>
    </div>

    </center>
    <br />
    <asp:HiddenField runat="server" ID="hdnCambiado" Value="0" />
    <input  id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />


    </form>
</body>
</html>


