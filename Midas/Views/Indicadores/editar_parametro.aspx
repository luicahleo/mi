<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">   
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    MIDAS.Models.indicadores_hojadedatos_valores oParametro;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        } 
        
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (user.perfil == 2)
            permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
        else
        {
            permisos.idusuario = user.idUsuario;
            permisos.idcentro = centroseleccionado.id;
            permisos.permiso = true;
        } 

        if (!IsPostBack)
        {
            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }

            if (Session["anioImputacion"] != null)
            {
                ddlAnio.SelectedValue = Session["anioImputacion"].ToString();
                Session.Remove("anioImputacion");
            }
            
            oParametro = (MIDAS.Models.indicadores_hojadedatos_valores)ViewData["EditarParametro"];
            if (oParametro != null)
            {
                if (ViewData["consultaparametro"] != null)
                {
                    MIDAS.Models.indicadores_hojadedatos consultaParametros = ((MIDAS.Models.indicadores_hojadedatos)ViewData["consultaparametro"]);
                    
                    MIDAS.Models.procesos proc = MIDAS.Models.Datos.GetDatosProceso(consultaParametros.idproceso);

                    txtParametro.Text = consultaParametros.indicador;
                    txtPeriodicidad.Text = consultaParametros.periodicidad;
                    txtUnidad.Text = consultaParametros.unidad;
                }

                ddlAnio.SelectedValue = oParametro.anio.ToString();
                
                txtMed1.Text = oParametro.valor1.ToString();
                txtMed2.Text = oParametro.valor2.ToString();
                txtMed3.Text = oParametro.valor3.ToString();
                txtMed4.Text = oParametro.valor4.ToString();
                txtMed5.Text = oParametro.valor5.ToString();
                txtMed6.Text = oParametro.valor6.ToString();
                txtMed7.Text = oParametro.valor7.ToString();
                txtMed8.Text = oParametro.valor8.ToString();
                txtMed9.Text = oParametro.valor9.ToString();
                txtMed10.Text = oParametro.valor10.ToString();
                txtMed11.Text = oParametro.valor11.ToString();
                txtMed12.Text = oParametro.valor12.ToString();            
                
                MIDAS.Models.indicadores_hojadedatos hojad = MIDAS.Models.Datos.ObtenerParametroInd(oParametro.CodIndiHojaDatos);

                switch (hojad.periodicidad)
                {
                    case "Anual":
                        txtMed1.ReadOnly = true;
                        txtMed2.ReadOnly = true;
                        txtMed3.ReadOnly = true;
                        txtMed4.ReadOnly = true;
                        txtMed5.ReadOnly = true;
                        txtMed6.ReadOnly = true;
                        txtMed7.ReadOnly = true;
                        txtMed8.ReadOnly = true;
                        txtMed9.ReadOnly = true;
                        txtMed10.ReadOnly = true;
                        txtMed11.ReadOnly = true;
                        break;
                    case "Semestral":
                        txtMed1.ReadOnly = true;
                        txtMed2.ReadOnly = true;
                        txtMed3.ReadOnly = true;
                        txtMed4.ReadOnly = true;
                        txtMed5.ReadOnly = true;
                        txtMed7.ReadOnly = true;
                        txtMed8.ReadOnly = true;
                        txtMed9.ReadOnly = true;
                        txtMed10.ReadOnly = true;
                        txtMed11.ReadOnly = true;
                        break;
                    case "Trimestral":
                        txtMed1.ReadOnly = true;
                        txtMed2.ReadOnly = true;
                        txtMed4.ReadOnly = true;
                        txtMed5.ReadOnly = true;
                        txtMed7.ReadOnly = true;
                        txtMed8.ReadOnly = true;
                        txtMed10.ReadOnly = true;
                        txtMed11.ReadOnly = true;
                        break;
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
                Imputación del parámetro
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
                        <asp:TextBox ID="txtParametro" Enabled="false" Width="98%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                    <td  style="width:30%">
                        <label>Periodicidad</label>
                        <asp:TextBox ID="txtPeriodicidad" Enabled="false" Width="98%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                    <td  style="width:10%">
                        <label>Unidad</label>
                        <asp:TextBox ID="txtUnidad" Enabled="false" Width="98%" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i>Medición</h6>
        </div>
        

        <div class="panel-body">
        <br />
        <div style="padding-left:30px" class="form-group">
                <label>Año de medición</label>
                <asp:DropDownList AutoPostBack="true" runat="server" ID="ddlAnio" class="form-control" Width="10%">
                            </asp:DropDownList>
            </div>

            <table style="width:100%">
                <tr>
                    <td style="width:7%">
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Enero</label>
                        </center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Febrero</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Marzo</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Abril</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Mayo</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Junio</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Julio</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Agosto</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Septiembre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Octubre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Noviembre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Diciembre</label></center>
                    </td>
                </tr>
                <tr>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <label>Medición</label>
                    </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed1" Width="70%" runat="server" class="form-control"></asp:TextBox>
                        </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed2" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed3" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed4" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed5" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed6" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed7" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed8" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed9" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed10" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed11" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed12" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                </tr>
            </table>
            
        </div>
        <br />
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarParametro" type="submit" value="Guardar datos" class="btn btn-primary run-first">
        <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
        <a href="/evr/indicadores/gestion_indicadores" title="Volver" class="btn btn-primary run-first">Volver</a>
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
