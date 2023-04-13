<%@ Page Title="" Language="C#" ValidateRequest="false"
    Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Midas-Cambiar contraseña</title>
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

        if (Session["passwordchanged"] != null)
        {
            lblError.Visible = false;
            if (Session["passwordchanged"] == "1")
            {
                hdnCambiado.Value = "1";
                lblError.Visible = false;
                lblExito.Visible = true;
                lblExito.Text = "La contraseña ha sido cambiada correctamente";           
            }
            else if (Session["passwordchanged"] == "2")
            {
                lblError.Visible = true;
                lblError.Text = "El campo contraseña está vacío";
            }
            else if (Session["passwordchanged"] == "3")
            {
                lblError.Visible = true;
                lblError.Text = "Las contraseñas no coinciden";
            }

            Session.Remove("passwordchanged");
        }

    }
</script>
<script type="text/javascript">

    $(document).ready(function () {
        if ($('#' + '<%= hdnCambiado.ClientID %>').val() == '1') {
            window.close();
        }
    });

</script>
</head>
<body>
    <br />
    <center><h2>Cambiar contraseña</h2></center>
    <br />
    <form id="Form1" runat="server">
    <center>
    <div><label>Contraseña </label>&nbsp;
    <br />
        <asp:TextBox TextMode="Password" CssClass="form-control" style="width:40%; text-align:center"  ID="txtPassword" runat="server"></asp:TextBox>
    <br />    
        <label>Repetir Contraseña </label>&nbsp;
    <br />  
        <asp:TextBox TextMode="Password" CssClass="form-control" style="width:40%; text-align:center"  ID="txtRepetir" runat="server"></asp:TextBox>
    <br />  
    <asp:Label style="color:Red; font-weight:bold" runat="server" Visible="false" ID="lblError"></asp:Label><asp:Label style="color:Green; font-weight:bold" runat="server" Visible="false" ID="lblExito"></asp:Label><br /><br />
    <input type="submit" class="btn btn-primary" name="Submit" id="Submit" value="Cambiar Contraseña" />
    </div>
    </center>
    <br />
    <asp:HiddenField runat="server" ID="hdnCambiado" Value="0" />
    <input  id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />


    </form>
</body>
</html>

