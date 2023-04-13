<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.aspecto_parametros oParametros;
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
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
            oParametros = (MIDAS.Models.aspecto_parametros)ViewData["EditarParametros"];
            if (oParametros != null)
            {
                hdnIdParametros.Value = oParametros.id.ToString();
                txtMwhb1.Text = oParametros.Mwhb1.ToString();
                txtMwhb2.Text = oParametros.Mwhb2.ToString();
                txtMwhb3.Text = oParametros.Mwhb3.ToString();
                txtMwhb4.Text = oParametros.Mwhb4.ToString();
                txtMwhb5.Text = oParametros.Mwhb5.ToString();
                txtMwhb6.Text = oParametros.Mwhb6.ToString();

                txtkmAnio1.Text = oParametros.kmAnio1.ToString();
                txtkmAnio2.Text = oParametros.kmAnio2.ToString();
                txtkmAnio3.Text = oParametros.kmAnio3.ToString();
                txtkmAnio4.Text = oParametros.kmAnio4.ToString();
                txtkmAnio5.Text = oParametros.kmAnio5.ToString();
                txtkmAnio6.Text = oParametros.kmAnio6.ToString();

                txtm3Hora1.Text = oParametros.m3Hora1.ToString();
                txtm3Hora2.Text = oParametros.m3Hora2.ToString();
                txtm3Hora3.Text = oParametros.m3Hora3.ToString();
                txtm3Hora4.Text = oParametros.m3Hora4.ToString();
                txtm3Hora5.Text = oParametros.m3Hora5.ToString();
                txtm3Hora6.Text = oParametros.m3Hora6.ToString();

                txtnumtrabAnio1.Text = oParametros.numtrabAnio1.ToString();
                txtnumtrabAnio2.Text = oParametros.numtrabAnio2.ToString();
                txtnumtrabAnio3.Text = oParametros.numtrabAnio3.ToString();
                txtnumtrabAnio4.Text = oParametros.numtrabAnio4.ToString();
                txtnumtrabAnio5.Text = oParametros.numtrabAnio5.ToString();
                txtnumtrabAnio6.Text = oParametros.numtrabAnio6.ToString();

                txtm3aguadesaladaAnio1.Text = oParametros.m3aguadesaladaAnio1.ToString();
                txtm3aguadesaladaAnio2.Text = oParametros.m3aguadesaladaAnio2.ToString();
                txtm3aguadesaladaAnio3.Text = oParametros.m3aguadesaladaAnio3.ToString();
                txtm3aguadesaladaAnio4.Text = oParametros.m3aguadesaladaAnio4.ToString();
                txtm3aguadesaladaAnio5.Text = oParametros.m3aguadesaladaAnio5.ToString();
                txtm3aguadesaladaAnio6.Text = oParametros.m3aguadesaladaAnio6.ToString();

                txttrabcanteraAnio1.Text = oParametros.trabcanteraAnio1.ToString();
                txttrabcanteraAnio2.Text = oParametros.trabcanteraAnio2.ToString();
                txttrabcanteraAnio3.Text = oParametros.trabcanteraAnio3.ToString();
                txttrabcanteraAnio4.Text = oParametros.trabcanteraAnio4.ToString();
                txttrabcanteraAnio5.Text = oParametros.trabcanteraAnio5.ToString();
                txttrabcanteraAnio6.Text = oParametros.trabcanteraAnio6.ToString();
            }
        }

        if (Session["EdicionParametrosMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionParametrosMensaje"].ToString() + "' });", true);
            Session["EdicionParametrosMensaje"] = null;
        }

        if (Session["EdicionParametrosError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionParametrosError"].ToString() + "' });", true);
            Session["EdicionParametrosError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parámetros de evaluación </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                    $("#hdFormularioEjecutado").val("GuardarParametros");
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
                Parámetros de evaluación
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
                <i class="icon-cog"></i>Producción MWhb</h6>
        </div>
        <div class="panel-body">
        <asp:HiddenField runat="server" ID="hdnIdParametros" />
        <br />
            <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txtMwhb1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txtMwhb2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txtMwhb3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txtMwhb4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txtMwhb5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>      
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txtMwhb6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>               
                    </tr>
                </table><br />
            </div>
        </div>
            <br />
            <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i>Km/Año Flota Vehículos</h6>
        </div>
        <div class="panel-body">
                <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txtkmAnio1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txtkmAnio2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txtkmAnio3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txtkmAnio4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txtkmAnio5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txtkmAnio6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>                   
                    </tr>
                </table><br />
            </div>
        </div>
                <br />
                 <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i>m3/Hora (vertido de agua en planta de tratamiento de efluentes)</h6>
        </div>
        <div class="panel-body">
                <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txtm3Hora1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txtm3Hora2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txtm3Hora3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txtm3Hora4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txtm3Hora5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>      
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txtm3Hora6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>                  
                    </tr>
                </table><br />
            </div>

            </div>
                <br />
            <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i>Número de trabajadores</h6>
        </div>
        <div class="panel-body">
                <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txtnumtrabAnio1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txtnumtrabAnio2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txtnumtrabAnio3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txtnumtrabAnio4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txtnumtrabAnio5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                         
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txtnumtrabAnio6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>                   
                    </tr>
                </table>
                <br />
            </div>
            </div>
                <br />
              <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i>Agua desalada (m3)</h6>
        </div>
        <div class="panel-body">
                <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                         
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txtm3aguadesaladaAnio6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>                   
                    </tr>
                </table>
                <br />
            </div>
            </div>
                <br />

                 <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i>Trabajadores Cantera</h6>
        </div>
        <div class="panel-body">
                <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-6 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio1" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 10%">
                     <center>
                            <label>
                                <%= DateTime.Now.Year-5 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-4 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-3 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>    
                        
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-2 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                         
                        <td class="form-group" style="width: 10%">
                        <center>
                            <label>
                                <%= DateTime.Now.Year-1 %></label>
                        <asp:TextBox ID="txttrabcanteraAnio6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>                   
                    </tr>
                </table>
                <br />
            </div>
            </div>
                <br />
        </div>
    </div>


    <!-- /modal with table -->
    <div class="form-actions text-right">
        <% if (permisos.permiso == true)
           { %>
        <input id="GuardarParametros" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/aspectosambientales/gestion_aspectos" title="Volver" class="btn btn-primary run-first">Volver</a>

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
