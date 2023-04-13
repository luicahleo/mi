<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">   
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.tipodocumento oTipoDoc;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["tecnologias"] != null)
        {
            ddlTecnologia.DataSource = ViewData["tecnologias"];
            ddlTecnologia.DataValueField = "id";
            ddlTecnologia.DataTextField = "nombre";
            ddlTecnologia.DataBind();
            
            ListItem opcionvacia = new ListItem();
            opcionvacia.Value = "0";
            opcionvacia.Text = "---";
            ddlTecnologia.Items.Insert(0, opcionvacia);
        }

        if (!IsPostBack)
        {
            oTipoDoc = (MIDAS.Models.tipodocumento)ViewData["EditarTipoDoc"];
            if (oTipoDoc != null)
            {
                hdnIdDocumento.Value = oTipoDoc.id.ToString();
                txtNombre.Text = oTipoDoc.tipo.ToString();
                ddlNivel.SelectedValue = oTipoDoc.nivel.ToString();
                ddlTecnologia.SelectedValue = oTipoDoc.tecnologia.ToString();
                if (oTipoDoc.nivel != 2)
                    divTecnologia.Style.Add("display", "none");
                ViewData["idTipoDoc"] = oTipoDoc.id;
            }
            else
            {

                divTecnologia.Style.Add("display","none");
                hdnIdDocumento.Value = "0";
            }

            if (Session["TipoDocErroneo"] != null)
            {
                oTipoDoc = (MIDAS.Models.tipodocumento)Session["TipoDocErroneo"];
                hdnIdDocumento.Value = oTipoDoc.id.ToString();
                txtNombre.Text = oTipoDoc.tipo.ToString();
                ddlNivel.SelectedValue = oTipoDoc.nivel.ToString();
                ddlTecnologia.SelectedValue = oTipoDoc.tecnologia.ToString();
                if (oTipoDoc.nivel != 2)
                    divTecnologia.Style.Add("display", "none");

                Session.Remove("TipoDocErroneo");
            }


        }

        if (Session["EdicionTipoDocMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionTipoDocMensaje"].ToString() + "' });", true);
            Session["EdicionTipoDocMensaje"] = null;
        }

        if (Session["EdicionTipoDocError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionTipoDocError"].ToString() + "' });", true);
            Session["EdicionTipoDocError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Tipo Doc. </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarCentral")
                    $("#hdFormularioEjecutado").val("GuardarCentral");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_ddlNivel").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlNivel').val();

                if (perfilseleccionado == 2) {
                    $("#ctl00_MainContent_divTecnologia").show();
                }
                else {
                    $("#ctl00_MainContent_divTecnologia").hide();
                }
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
                Edición de tipo de documento
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
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                    <label>
                        NIVEL:</label>&nbsp;
                    <asp:DropDownList ID="ddlNivel" CssClass="form-control" runat="server">
                        <asp:ListItem Value="0">---</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                    </asp:DropDownList>
                </div>
                    </td>
                    <td style="padding-right:15px;width:20%">
                        <div runat="server" id="divTecnologia" class="form-group">
                    <label>
                        TECNOLOGIA:</label>&nbsp;
                    <asp:DropDownList ID="ddlTecnologia" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                    </td>                  
                    </tr>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarTipoDoc" type="submit" value="Guardar datos" class="btn btn-primary run-first">
        <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
        <a href="/evr/configuracion/Documentos" title="Volver" class="btn btn-primary run-first">Volver</a>
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
