<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">   
    MIDAS.Models.indicadores_hojadedatos consultaParametros = new MIDAS.Models.indicadores_hojadedatos();
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (!IsPostBack)
        {
            if (ViewData["centrosasignables"] != null)
            {
                ddlCentros.DataSource = ViewData["centrosasignables"];
                ddlCentros.DataValueField = "id";
                ddlCentros.DataTextField = "nombre";
                ddlCentros.DataBind();
            }

            if (ViewData["centrosasignados"] != null)
            {
                DatosCentros.DataSource = ViewData["centrosasignados"];
                DatosCentros.DataBind();
            }
            
            consultaParametros = (MIDAS.Models.indicadores_hojadedatos)ViewData["consultaparametro"];
            if (consultaParametros != null)
            {
                if (ViewData["consultaparametro"] != null)
                {                    
                    txtParametro.Text = consultaParametros.indicador;
                    ddlPeriodicidad.SelectedValue = consultaParametros.periodicidad;
                    txtUnidad.Text = consultaParametros.unidad;
                }
                
                                
            }            

            if (Session["TipoDocErroneo"] != null)
            {
                Session.Remove("TipoDocErroneo");
            }
        }

        if (Session["EdicionParametroMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionParametroMensaje"].ToString() + "' });", true);
            Session["EdicionParametroMensaje"] = null;
        }

        if (Session["EdicionParametroError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionParametroError"].ToString() + "' });", true);
            Session["EdicionParametroError"] = null;
        }
    }    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parámetro. </title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarParametro")
                    $("#hdFormularioEjecutado").val("GuardarParametro");   
                if (val == "AsignarCentro")
                    $("#hdFormularioEjecutado").val("AsignarCentro");
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
                Edición del parámetro
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
                <i class="icon-bookmark"></i>Datos Generales</h6>
        </div>
        

        <div class="panel-body">
            <table width="100%">
                <tr>
                    <td style="width:60%">
                        <label>Parámetro</label>
                        <asp:TextBox ID="txtParametro" Width="98%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                    <td  style="width:30%">
                        <label>Periodicidad</label>
                        <asp:DropDownList Width="98%" CssClass="form-control" ID="ddlPeriodicidad" runat="server"> 
                                        <asp:ListItem Value="Mensual" Text="Mensual"></asp:ListItem>
                                        <asp:ListItem Value="Trimestral" Text="Trimestral"></asp:ListItem>                                        
                                        <asp:ListItem Value="Semestral" Text="Semestral"></asp:ListItem>
                                        <asp:ListItem Value="Anual" Text="Anual"></asp:ListItem>
                        </asp:DropDownList>  
                    </td>
                    <td  style="width:10%">
                        <label>Unidad</label>
                        <asp:TextBox ID="txtUnidad" Width="98%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>    

    <% if ((user != null && user.idUsuario != 0) && (consultaParametros != null && consultaParametros.id != 0))
       { %>
    <div id="panelcentrales" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Selección de centros</h6>
        </div>
        <div class="panel-body">

                <table width="50%">
                    <tr>
                        <td>
                            <div class="form-group">
                                <label>
                                    CENTROS A ASIGNAR:</label>
                                <asp:DropDownList ID="ddlCentros" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td  style="padding-left:15px">
                            <div class="form-group">
                                <input style="margin-top: 12px" id="AsignarCentro" type="submit" value="Asignar Centro"
                                    class="btn btn-primary run-first" />
                            </div>
                        </td>
                    </tr>

                </table>
                    
                    

            </div>
            <div class="row">
                <asp:GridView ID="DatosCentros" runat="server" Visible="false">
                </asp:GridView>
                <div class="block">
                    <center>
                        <div style="width: 95%" class="datatablePedido">
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th style="width: 70px">
                                            Siglas
                                        </th>
                                        <th>
                                            Centro
                                        </th>  
                                        <th style="width: 45px">
                                            Baja
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% 
                                        foreach (GridViewRow item in DatosCentros.Rows)
                                        { %>
                                    <tr>
                                        <td style="text-align: center" class="task-desc">
                                            <%= item.Cells[1].Text %>
                                        </td>
                                        <td class="task-desc">
                                            <%= item.Cells[2].Text %>
                                        </td> 
                                        <td class="text-center">
                                            <a onclick="return confirm('¿Está seguro que desea dar de baja la asociación con el centro?');"
                                                href="/evr/configuracion/eliminar_parametrocentro/<%= item.Cells[0].Text %>" title="Baja"><i
                                                    class="icon-remove"></i></a>
                                        </td>
                                    </tr>
                                    <% }%>
                                </tbody>
                            </table>
                        </div>
                    </center>
                </div>
            </div>
        </div>

    <% } %>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarParametro" type="submit" value="Guardar datos" class="btn btn-primary run-first">
        <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
        <a href="/evr/configuracion/parametros" title="Volver" class="btn btn-primary run-first">Volver</a>
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
