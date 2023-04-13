<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.stakeholders_nivel3 oStakeholder;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["listashn2"] != null)
        {
            ddlSHN2.DataSource = ViewData["listashn2"];
            ddlSHN2.DataValueField = "id";
            ddlSHN2.DataTextField = "denominacionn2";
            ddlSHN2.DataBind();
        }


        if (!IsPostBack)
        {
            oStakeholder = (MIDAS.Models.stakeholders_nivel3)ViewData["EditarStakeholderN3"];
            if (oStakeholder != null)
            {
                hdnIdDocumento.Value = oStakeholder.id.ToString();
                txtNombre.Text = oStakeholder.denominacion.ToString();
                ddlSHN2.SelectedValue = oStakeholder.idnivel2.ToString();
                txtNecesidades.Text = oStakeholder.necesidades;
                if (oStakeholder.parteinteresada != null)
                    ddlRelevante.SelectedValue = oStakeholder.parteinteresada.ToString();
                
            }
        }

        if (Session["EdicionStakeholderN3Mensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionStakeholderN3Mensaje"].ToString() + "' });", true);
            Session["EdicionStakeholderN3Mensaje"] = null;
        }

        if (Session["EdicionStakeholderN3Error"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionStakeholderN3Error"].ToString() + "' });", true);
            Session["EdicionStakeholderN3Error"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parte interesada N3 </title>
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
                </div>  </div>

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
                                Nivel 2 Parte Interesada</label>
                            <asp:DropDownList Width="95%" ID="ddlSHN2" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    <td style="width:40%; padding-top:6px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            Denominación:</label>
                        <asp:TextBox Width="95%" ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>     
                    <td class="form-group" style="width: 50%">
                            <label>
                               Relevancia</label>
                            <asp:DropDownList Width="100%" ID="ddlRelevante" runat="server" class="form-control">
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                            </asp:DropDownList>
                        </td>                             
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label>
                            Necesidades:</label>
                            <asp:TextBox ID="txtNecesidades" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
            </table>
            <br />
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarStakeholder" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/stakeholders" title="Volver" class="btn btn-primary run-first">Volver</a>
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
