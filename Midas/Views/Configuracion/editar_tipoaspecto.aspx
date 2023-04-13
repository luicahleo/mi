<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.aspecto_tipo oTipo;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }


        if (!IsPostBack)
        {
            oTipo = (MIDAS.Models.aspecto_tipo)ViewData["EditarAspecto"];
            if (oTipo != null)
            {
                hdnIdAspecto.Value = oTipo.id.ToString();
                txtCodAspecto.Text = oTipo.Codigo;
                ddlGrupo.SelectedValue = oTipo.Grupo.ToString();
                txtIdentificacion.Text = oTipo.Nombre;
                txtUnidad.Text = oTipo.Unidad.ToString();
                txtDescripcion.Text = oTipo.Descripcion;
                txtImpacto.Text = oTipo.Impacto;
                ddlPeligroso.SelectedValue = oTipo.RE_Peligroso.ToString();
                if (oTipo.relativoMwhb == true)
                {
                    rdbRelativo.SelectedValue = "Mwhb";
                }
                if (oTipo.relativohAnioGE == true)
                {
                    rdbRelativo.SelectedValue = "hAnioGE";
                }
                if (oTipo.relativokmAnio == true)
                {
                    rdbRelativo.SelectedValue = "kmAnio";
                }
                if (oTipo.relativom3Hora == true)
                {
                    rdbRelativo.SelectedValue = "m3Hora";
                }
                if (oTipo.relativohfuncAnio == true)
                {
                    rdbRelativo.SelectedValue = "hfuncAnio";
                }
            }
        }

        if (Session["EdicionTipoAspectoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionTipoAspectoMensaje"].ToString() + "' });", true);
            Session["EdicionTipoAspectoMensaje"] = null;
        }

        if (Session["EdicionTipoAspectoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionTipoAspectoError"].ToString() + "' });", true);
            Session["EdicionTipoAspectoError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Tipo Acc.Mejora </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarTipoAspecto")
                    $("#hdFormularioEjecutado").val("GuardarTipoAspecto");

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
                Edición de tipos de aspectos
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
                    <td style="padding-right:15px; width:10%">
                    <div class="form-group">
                        <label>
                            Código</label>
                        <asp:TextBox ID="txtCodAspecto" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td> 
                    <td style="padding-right:15px; width:40%">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdAspecto" />
                        <label>
                            Grupo</label>
                            <asp:DropDownList AutoPostBack="true" runat="server" ID="ddlGrupo" class="form-control" Width="100%">
                                <asp:ListItem Value="1" Text="Emisiones reguladas"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Emisiones no reguladas\Otras emisiones (CO2)"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Emisiones no reguladas\Otras emisiones (GE)"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Emisiones no reguladas\Otras emisiones (Vehículos y maquinaria)"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Emisiones no reguladas\Otras emisiones (Material pulverulento)"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Vertidos regulados"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Vertidos no regulados"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Residuos"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Consumos\Combustibles"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Consumos\Agua"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Consumos\Electricidad"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Consumos\Sustancias químicas"></asp:ListItem>
                                <asp:ListItem Value="13" Text="Ruidos y vibraciones\Emitido por la instalación"></asp:ListItem>
                                <asp:ListItem Value="14" Text="Ruidos y vibraciones\Emitido en vertedero"></asp:ListItem>
                                <asp:ListItem Value="15" Text="Ruidos y vibraciones\Emitido por maquinaria (aire libre)"></asp:ListItem>
                                <asp:ListItem Value="16" Text="Ruidos y vibraciones\Emitido por vehículos"></asp:ListItem>
                                <asp:ListItem Value="17" Text="Otros\Visibilidad de la instalación"></asp:ListItem>
                                <asp:ListItem Value="18" Text="Otros\Emisión lumínica"></asp:ListItem>
                                <asp:ListItem Value="19" Text="Otros\Uso del suelo"></asp:ListItem>
                                <asp:ListItem Value="20" Text="Otros\Depósito en vertedero"></asp:ListItem>
                                <asp:ListItem Value="21" Text="Otros\Compactación corona perimetral"></asp:ListItem>
                                <asp:ListItem Value="22" Text="Otros\Olores"></asp:ListItem>
                                <asp:ListItem Value="23" Text="Potenciales"></asp:ListItem>
                                <asp:ListItem Value="24" Text="Indirectos"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    </td>  
                    <% if ( ddlGrupo.SelectedValue != "17" && ddlGrupo.SelectedValue != "18" && ddlGrupo.SelectedValue != "19" && ddlGrupo.SelectedValue != "20" && ddlGrupo.SelectedValue != "21" && ddlGrupo.SelectedValue != "22")
                       {%>
                    <td style="padding-right:15px; width:30%">
                    
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="HiddenField2" />
                        <% if (ddlGrupo.SelectedValue == "1" || ddlGrupo.SelectedValue == "2" || ddlGrupo.SelectedValue == "3" || ddlGrupo.SelectedValue == "4" || ddlGrupo.SelectedValue == "5")
                           {    %>
                        <label>
                            Parámetro del foco/contaminante</label>
                        <% } %>
                        <% if (ddlGrupo.SelectedValue == "6" || ddlGrupo.SelectedValue == "7")
                           {    %>
                        <label>
                            Parámetro del vertido</label>
                        <% } %>
                        <% if (ddlGrupo.SelectedValue == "9" || ddlGrupo.SelectedValue == "10" || ddlGrupo.SelectedValue == "11" || ddlGrupo.SelectedValue == "12")
                           {    %>
                        <label>
                            Identificación del consumo</label>
                        <% } %>
                        <% if (ddlGrupo.SelectedValue == "8" || ddlGrupo.SelectedValue == "23" || ddlGrupo.SelectedValue == "24")
                           {    %>
                        <label>
                            Identificación del aspecto</label>
                        <% } %>
                        <% if (ddlGrupo.SelectedValue == "13" || ddlGrupo.SelectedValue == "14" || ddlGrupo.SelectedValue == "15" || ddlGrupo.SelectedValue == "16")
                           { %>
                        <label>
                            Identificación del punto de medida</label>
                        <% } %>
                        <asp:TextBox ID="txtIdentificacion" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>    
                    <% } %>
                    <td style="padding-right:15px; width:10%">
                    <div class="form-group">
                        <label>
                            Unidad</label>
                        <asp:TextBox ID="txtUnidad" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>       
                    <% if (ddlGrupo.SelectedValue == "8")
                       { %>
                    <td style="padding-right:15px; width:10%">
                    <div class="form-group">
                        <label>
                            Peligroso</label>
                        <asp:DropDownList runat="server" ID="ddlPeligroso" class="form-control" Width="100%">
                                <asp:ListItem Value="1" Text="Peligroso"></asp:ListItem>
                                <asp:ListItem Value="2" Text="No peligroso"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    </td>      
                    <% } %>              
                    </tr>
            </table>
            <table width="100%">
                <tr>
                <td>
                    <label>
                            Impacto</label>
                    
                        <asp:TextBox ID="txtImpacto" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td style="padding-top:20px">
                    <label>
                            Descripción</label>
                    
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                </table>
                <br />
                <% if (ddlGrupo.SelectedValue != "23" && ddlGrupo.SelectedValue != "24")
                   { %>
                <table width="100%">
                <tr>
                <td>
                    <label>Parámetro para la ponderación</label>
                    </td>
                </tr>
                <tr>
                <td>
                        <asp:RadioButtonList RepeatDirection="Horizontal" style="margin-top:10px" ID="rdbRelativo" runat="server">  
                            <asp:ListItem Value="Mwhb" Text="Producción MWhb"></asp:ListItem>
                            <asp:ListItem style="margin-left:20px" Value="hAnioGE" Text="Horas/Año de Grupo Electrógeno"></asp:ListItem>
                            <asp:ListItem style="margin-left:20px" Value="kmAnio" Text="Km/Año"></asp:ListItem>
                            <asp:ListItem style="margin-left:20px" Value="m3Hora" Text="m3/Hora"></asp:ListItem>
                            <asp:ListItem style="margin-left:20px" Value="hfuncAnio" Text="Horas de funcionamiento/Año"></asp:ListItem>
                        </asp:RadioButtonList>

                </td>
                </tr>
            </table>
            <% } %>
            <br />

        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarTipoAspecto" type="submit" value="Guardar datos" class="btn btn-primary run-first"/>        
        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
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
