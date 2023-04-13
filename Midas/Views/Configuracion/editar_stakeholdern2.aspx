<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.stakeholders_nivel2 oStakeholder;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["listashn1"] != null)
        {
            ddlSHN1.DataSource = ViewData["listashn1"];
            ddlSHN1.DataValueField = "id";
            ddlSHN1.DataTextField = "denominacion";
            ddlSHN1.DataBind();
        }


        if (!IsPostBack)
        {
            oStakeholder = (MIDAS.Models.stakeholders_nivel2)ViewData["EditarStakeholderN2"];
            if (oStakeholder != null)
            {
                hdnIdDocumento.Value = oStakeholder.id.ToString();
                txtNombre.Text = oStakeholder.denominacion.ToString();
                ddlSHN1.SelectedValue = oStakeholder.idnivel1.ToString();
            }
        }

        if (Session["EdicionStakeholderN2Mensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionStakeholderN2Mensaje"].ToString() + "' });", true);
            Session["EdicionStakeholderN2Mensaje"] = null;
        }

        if (Session["EdicionStakeholderN2Error"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionStakeholderN2Error"].ToString() + "' });", true);
            Session["EdicionStakeholderN2Error"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parte interesada N2 </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarStakeholder")
                    $("#hdFormularioEjecutado").val("GuardarStakeholder");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });          


        });

        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
		<div class="page-title">
            <h3>
                Edición de Parte Interesada
            </h3>
        </div> 
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-document"></i>Datos generales</h6>
        </div>
        <div class="panel-body">
            <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 50%">
                            <label>
                                N1 PARTE INTERESADA</label>
                            <asp:DropDownList Width="95%" ID="ddlSHN1" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    <td style="width:50%; padding-top:6px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            DENOMINACIÓN:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                                  
                    </tr>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarStakeholder" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a <a href="/evr/configuracion/stakeholders" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
