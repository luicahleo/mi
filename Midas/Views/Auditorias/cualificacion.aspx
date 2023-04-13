<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cualificación</title>
    <link href="/Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/londinium-theme.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="/Content/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&amp;subset=latin,cyrillic-ext"
        rel="stylesheet" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.4/jquery.js"></script>
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            if (ViewData["auditor"] != null)
            {
                MIDAS.Models.auditores auditor = (MIDAS.Models.auditores)ViewData["auditor"];

                lblNombreAccion.Text = auditor.nombre;
            }

            if (ViewData["cualificacionauditor"] != null)
            {
                grdCualificacion.DataSource = ViewData["cualificacionauditor"];
                grdCualificacion.DataBind();
            }

        }
    </script>
    <script type="text/javascript">

    </script>
</head>
<body>
    <br />
    <center>
        <h2>
            Asociar documento al auditor - <asp:Label ID="lblNombreAccion" runat="server" Text=""></asp:Label></h2>
    </center>
    <br />
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <center>
        <div style="width:100%">
        <%
                                        if (user.perfil == 1 || user.perfil == 3)
                                        {
                                    %>
             <table style="width:50%" id="tablaFicheroNuevo">
                                 <tr> 
                                    <td style="padding-right:10px; width:50%">
                                        <div class="form-group">
                                                <label>
                                                    Documento:</label>
                                                    <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>   
                                    <td style="width:50%">
                                        <div class="form-group">
                                        <input type="file" id="file" name="file" style="margin-top:10px" /></div>
                                    </td>         
                                    </tr>
                                    </table>
                                    <% } %>
                
            <input style="margin-top: 5px" type="submit" class="btn btn-primary" name="Submit"
                id="Submit" value="Añadir documento" />
            <asp:Label Style="color: Red; font-weight: bold" runat="server" Visible="false" ID="lblError"></asp:Label><br />
            <div class="panel-body">
                <asp:GridView ID="grdCualificacion" runat="server" Visible="false">
                </asp:GridView>
                <center>
                    <div style="width: 95%" class="datatablePedido">
                        <table  class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>
                                        Documento
                                    </th>
                                    <th>
                                        Nombre del fichero
                                    </th>
                                    <th style="width:45px">
                                        Descarga
                                    </th>
                                    <%
                                        if (user.perfil == 1 || user.perfil == 3)
                                        {
                                    %>
                                    <th style="width:45px">
                                        Eliminar
                                    </th><% } %>
                                </tr>
                            </thead>
                            <tbody>
                                <% 
                                    foreach (GridViewRow item in grdCualificacion.Rows)
                                    { %>
                                <tr>
                                    <td class="task-desc">
                                        <%= item.Cells[2].Text %>
                                    </td>
                                    <td class="task-desc">
                                        <%= item.Cells[3].Text %>
                                    </td>
                                     <td style="text-align:center" class="task-desc">
                                                    <center>
                                                          <a title="Ver Fichero" href="/evr/Auditorias/ObtenerCualificacion/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td> 
                                    <%
                                        if (user.perfil == 1 || user.perfil == 3)
                                        {
                                    %>
                                    <td class="text-center">
                                        <a href="/evr/Auditorias/eliminar_cualificacion/<%= item.Cells[0].Text %>" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;"
                                            title="Eliminar"><i class="icon-remove"></i></a>
                                    </td>
                                    <% } %>
                                </tr>
                                <% }%>
                            </tbody>
                        </table>
                    </div>
                </center>
            </div>
        </div>
    </center>
    <br />
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    </form>
</body>
</html>