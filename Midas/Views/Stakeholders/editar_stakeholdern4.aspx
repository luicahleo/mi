<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.stakeholders_nivel4 oStakeholder;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["listashn3"] != null)
        {
            ddlSHN3.DataSource = ViewData["listashn3"];
            ddlSHN3.DataValueField = "id";
            ddlSHN3.DataTextField = "denominacionn3";
            ddlSHN3.DataBind();
        }


        if (!IsPostBack)
        {
            oStakeholder = (MIDAS.Models.stakeholders_nivel4)ViewData["EditarStakeholderN4"];
            if (oStakeholder != null)
            {
                hdnIdDocumento.Value = oStakeholder.id.ToString();
                txtNombre.Text = oStakeholder.denominacion.ToString();
                ddlSHN3.SelectedValue = oStakeholder.idnivel3.ToString();
                txtNecesidades.Text = oStakeholder.necesidades;
                txtRequisitos.Text = oStakeholder.requisitosrel;
            }
        }

        if (Session["EdicionStakeholderN4Mensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionStakeholderN4Mensaje"].ToString() + "' });", true);
            Session["EdicionStakeholderN4Mensaje"] = null;
        }

        if (Session["EdicionStakeholderN4Error"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionStakeholderN4Error"].ToString() + "' });", true);
            Session["EdicionStakeholderN4Error"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parte interesada N4 </title>
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
                                Nivel 3 Parte Interesada</label>
                            <asp:DropDownList Width="95%" ID="ddlSHN3" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    <td style="width:50%; padding-top:6px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            Denominación:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                                  
                    </tr>
                    <tr>
                        <td colspan="2">
                        <br />
                            <label>
                            Requisitos relevantes:</label>
                            <asp:TextBox ID="txtRequisitos" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
        <a href="/evr/stakeholders/stakeholders" value="Salir" class="btn btn-primary" title="Volver">Volver</a>
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
