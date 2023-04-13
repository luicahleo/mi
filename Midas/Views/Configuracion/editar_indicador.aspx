<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.indicadores oIndicador;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["parametros"] != null)
        {
            ddlOperador1.DataSource = ViewData["parametros"];
            ddlOperador1.DataValueField = "id";
            ddlOperador1.DataTextField = "indicador";
            ddlOperador1.DataBind();

            ddlOperador2.DataSource = ViewData["parametros"];
            ddlOperador2.DataValueField = "id";
            ddlOperador2.DataTextField = "indicador";
            ddlOperador2.DataBind();

            ListItem itemOperador2 = new ListItem();
            itemOperador2.Value = "0";
            itemOperador2.Text = "---";
            ddlOperador2.Items.Insert(0, itemOperador2);

            ddlOperador3.DataSource = ViewData["parametros"];
            ddlOperador3.DataValueField = "id";
            ddlOperador3.DataTextField = "indicador";
            ddlOperador3.DataBind();

            ListItem itemOperador3 = new ListItem();
            itemOperador3.Value = "0";
            itemOperador3.Text = "---";
            ddlOperador3.Items.Insert(0, itemOperador3);
        }

        if (ViewData["centralesAsignables"] != null)
        {
            lstCentralesAsignar.DataSource = ViewData["centralesAsignables"];
            lstCentralesAsignar.DataValueField = "id";
            lstCentralesAsignar.DataTextField = "nombre";
            lstCentralesAsignar.DataBind();
        }
        if (ViewData["centralesAsignadas"] != null)
        {
            lstCentralesAsignadas.DataSource = ViewData["centralesAsignadas"];
            lstCentralesAsignadas.DataValueField = "id";
            lstCentralesAsignadas.DataTextField = "nombre";
            lstCentralesAsignadas.DataBind();
        }

        if (ViewData["tecnologias"] != null)
        {
            ddlTecnologia.DataSource = ViewData["tecnologias"];
            ddlTecnologia.DataValueField = "id";
            ddlTecnologia.DataTextField = "nombre";
            ddlTecnologia.DataBind();
        }
        ListItem tecnologiaVacio = new ListItem();
        tecnologiaVacio.Value = "0";
        tecnologiaVacio.Text = "--- (Especificar)";
        ddlTecnologia.Items.Insert(0, tecnologiaVacio);


        if (!IsPostBack)
        {
            oIndicador = (MIDAS.Models.indicadores)ViewData["EditarIndicador"];
            if (oIndicador != null)
            {
                hdnIdIndicador.Value = oIndicador.Id.ToString();
                txtNombre.Text = oIndicador.Nombre;
                txtDescripcion.Text = oIndicador.Descripcion;
                txtMetodo.Text = oIndicador.MetodoMedicion;
                txtUnidad.Text = oIndicador.Unidad;
                ddlTendencia.SelectedValue = oIndicador.tendencia.ToString();
                ddlTecnologia.SelectedValue = oIndicador.tecnologia.ToString();
                if (oIndicador.Operador1Constante == null)
                {
                    ddlTipoOperador1.SelectedIndex = 0;
                    operador1List.Style.Add("display", "block");
                    operador1Text.Style.Add("display", "none");
                    ddlOperador1.SelectedValue = oIndicador.Operador1.ToString();
                }
                else
                {
                    ddlTipoOperador1.SelectedIndex = 1;
                    operador1List.Style.Add("display", "none");
                    operador1Text.Style.Add("display", "block");
                    txtOperador1.Text = oIndicador.Operador1Constante.ToString();
                }
                if (oIndicador.Operador2Constante == null)
                {
                    ddlTipoOperador2.SelectedIndex = 0;
                    operador2List.Style.Add("display", "block");
                    operador2Text.Style.Add("display", "none");
                    ddlOperador2.SelectedValue = oIndicador.Operador2.ToString();
                }
                else
                {
                    ddlTipoOperador2.SelectedIndex = 1;
                    operador2List.Style.Add("display", "none");
                    operador2Text.Style.Add("display", "block");
                    txtOperador2.Text = oIndicador.Operador2Constante.ToString();
                }
                if (oIndicador.Operador3Constante == null)
                {
                    ddlTipoOperador3.SelectedIndex = 0;
                    operador3List.Style.Add("display", "block");
                    operador3Text.Style.Add("display", "none");
                    ddlOperador3.SelectedValue = oIndicador.Operador3.ToString();
                }
                else
                {
                    ddlTipoOperador3.SelectedIndex = 1;
                    operador3List.Style.Add("display", "none");
                    operador3Text.Style.Add("display", "block");
                    txtOperador3.Text = oIndicador.Operador3Constante.ToString();
                }


                if (oIndicador.Operacion1 != null)
                    ddlOperacion1.SelectedValue = oIndicador.Operacion1.Trim().ToString();
                if (oIndicador.Operacion2 != null)
                    ddlOperacion2.SelectedValue = oIndicador.Operacion2.Trim().ToString();
                ddlFrecuenciaExp.SelectedValue = oIndicador.Periodicidad.ToString();

                if (oIndicador.especifico != null)
                {
                    ddlEspecifico.SelectedValue = oIndicador.especifico.ToString();

                    if (oIndicador.especifico == 1)
                    {
                        tablaCentros.Visible = true;
                        tablaCentros.Style.Add("display", "block");
                    }

                    if (oIndicador.especifico == 2)
                    {
                        tablaCentros.Style.Add("display", "none");
                    }
                }
                else
                {
                    tablaCentros.Style.Add("display", "none");
                }

                if (oIndicador.ValorNumerico != null)
                {


                    if (oIndicador.ValorNumerico == false)
                    {
                        tablaFormula.Style.Add("display", "block");
                        ddlValorNumerico.SelectedIndex = 1;
                    }

                    if (oIndicador.ValorNumerico == true)
                    {
                        tablaFormula.Style.Add("display", "none");
                        ddlValorNumerico.SelectedIndex = 0;
                    }
                }
                else
                {
                    tablaFormula.Style.Add("display", "none");
                    ddlValorNumerico.SelectedIndex = 0;
                }
            }
            else
            {
                operador1Text.Style.Add("display", "none");
                operador2Text.Style.Add("display", "none");
                operador3Text.Style.Add("display", "none");
                tablaCentros.Style.Add("display", "none");
                tablaFormula.Style.Add("display", "none");
                ddlValorNumerico.SelectedIndex = 0;
            }
        }

        if (Session["EdicionIndicadorMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionIndicadorMensaje"].ToString() + "' });", true);
            Session["EdicionIndicadorMensaje"] = null;
        }

        if (Session["EdicionIndicadorError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionIndicadorError"].ToString() + "' });", true);
            Session["EdicionIndicadorError"] = null;
        }


    }
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Indicador </title>
    <script type="text/javascript">


        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();
			


            $('#<%= ddlTipoOperador1.ClientID %>').change(function () {
                var selectedValue = $("#<%=ddlTipoOperador1.ClientID%> option:selected").val();
                console.log(selectedValue);
                if (selectedValue == 1) {
                    $('#ctl00_MainContent_operador1List').hide();
                    $('#ctl00_MainContent_operador1Text').show();
                } else {
                    $('#ctl00_MainContent_operador1Text').hide();
                    $('#ctl00_MainContent_operador1List').show();
                }
            });

            $('#<%= ddlTipoOperador2.ClientID %>').change(function () {
                var selectedValue = $("#<%=ddlTipoOperador2.ClientID%> option:selected").val();
                console.log(selectedValue);
                if (selectedValue == 1) {
                    $('#ctl00_MainContent_operador2List').hide();
                    $('#ctl00_MainContent_operador2Text').show();
                } else {
                    $('#ctl00_MainContent_operador2Text').hide();
                    $('#ctl00_MainContent_operador2List').show();
                }
            });

            $('#<%= ddlTipoOperador3.ClientID %>').change(function () {
                var selectedValue = $("#<%=ddlTipoOperador3.ClientID%> option:selected").val();
                console.log(selectedValue);
                if (selectedValue == 1) {
                    $('#ctl00_MainContent_operador3List').hide();
                    $('#ctl00_MainContent_operador3Text').show();
                } else {
                    $('#ctl00_MainContent_operador3Text').hide();
                    $('#ctl00_MainContent_operador3List').show();
                }
            });
			   

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarIndicador")
                    $("#hdFormularioEjecutado").val("GuardarIndicador");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $('#btnAsignarCentro').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna central.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstCentralesAsignadas').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarCentralesAsignadas();
                e.preventDefault();
            });

            $('#btnNoAsignarCentro').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignadas option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna central.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstCentralesAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarCentralesAsignadas();
                e.preventDefault();
            });



            $('#btnAsignarTecnologia').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstTecnologiasAsignar option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna tecnología.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstTecnologiasAsignadas').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarTecnologiasAsignadas();
                e.preventDefault();
            });

            $('#btnNoAsignarTecnologia').click(function (e) {
                var selectedOpts = $('#ctl00_MainContent_lstTecnologiasAsignadas option:selected');
                if (selectedOpts.length == 0) {
                    alert("No ha seleccionado ninguna tecnología.");
                    e.preventDefault();
                }

                $('#ctl00_MainContent_lstTecnologiasAsignar').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                comprobarTecnologiasAsignadas();
                e.preventDefault();
            });

            $("#ctl00_MainContent_ddlEspecifico").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlEspecifico').val();

                if (perfilseleccionado == 1) {
                    $("#ctl00_MainContent_tablaCentros").show();
                    $("#ctl00_MainContent_tablaTecnologias").hide();
                }
                if (perfilseleccionado == 2) {
                    $("#ctl00_MainContent_tablaCentros").hide();
                    $("#ctl00_MainContent_tablaTecnologias").show();
                }
                if (perfilseleccionado == 0) {
                    $("#ctl00_MainContent_tablaCentros").hide();
                    $("#ctl00_MainContent_tablaTecnologias").hide();
                }
            });

            $("#ctl00_MainContent_ddlValorNumerico").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlValorNumerico').val();

                if (perfilseleccionado == 0) {
                    $("#ctl00_MainContent_tablaFormula").hide();
                }
                if (perfilseleccionado == 1) {
                    $("#ctl00_MainContent_tablaFormula").show();
                }
            });

            function comprobarCentralesAsignadas() {
                var selectedOpts = $('#ctl00_MainContent_lstCentralesAsignadas');
                var centrosseleccionados = '';
                for (var i = 0; i < selectedOpts[0].length; i++) {
                    centrosseleccionados = centrosseleccionados + selectedOpts[0].children[i].value + ";";
                }
                $("#ctl00_MainContent_hdnCentrosSeleccionados").val(centrosseleccionados);

            }
        });

        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
		<div class="page-title">
            <h3>
                Edición de Indicador
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
        <asp:HiddenField runat="server" ID="hdnIdIndicador" />
            <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 60%">

                            <label>
                                Nombre</label>
                        <asp:TextBox ID="txtNombre" Width="98%" runat="server" class="form-control"></asp:TextBox>

                        </td>
                    <td style="width:40%">

                    
                        <label>
                            Método de medición</label>
                        <asp:TextBox ID="txtMetodo" Width="100%" runat="server" class="form-control"></asp:TextBox>

                    </td>      
                    </tr>
                </table>
                <br />
            <table style="width:100%">
                <tr>
                    
                            <td style="width:60%">
                                    <table width="100%" style="margin-top:-13px">
                                <tr>
                                    <td style="width:50%">
                                        <br />
                                        <label>
                                            Tipo de medición</label>
                                        <asp:DropDownList ID="ddlValorNumerico" style="width:95%" runat="server" class="form-control">
                                            <asp:ListItem Value="0" Text="Valor numérico"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Fórmula"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:50%">
                                        <br />
                                        <label>
                                            Tendencia de valor de referencia</label>
                                        <asp:DropDownList ID="ddlTendencia" style="width:96%" runat="server" class="form-control">
                                            <asp:ListItem Value="0" Text="Mínimo"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Máximo"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            </td>           
                            <td class="form-group" style="width: 10%">

                                <label>Unidad</label>
                                <asp:TextBox ID="txtUnidad" Width="95%" runat="server" class="form-control"></asp:TextBox>

                                </td>   
                                <td style="width:20%">
                                    <label>Tecnología</label>
                                    <asp:DropDownList Width="98%" CssClass="form-control" ID="ddlTecnologia" runat="server"> 
                                                            </asp:DropDownList>  
                            </td>         
                            <td  style="width:10%">
                                            <label>Frec.Explotación</label>
                                                        <asp:DropDownList Width="98%" CssClass="form-control" ID="ddlFrecuenciaExp" runat="server"> 
                                                        <asp:ListItem Value="Mensual" Text="Mensual"></asp:ListItem>
                                                            <asp:ListItem Value="Bimestral" Text="Bimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Trimestral" Text="Trimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Cuatrimestral" Text="Cuatrimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Semestral" Text="Semestral"></asp:ListItem>
                                                            <asp:ListItem Value="Anual" Text="Anual"></asp:ListItem>
                                                                                </asp:DropDownList>  
                                        </td>         
                    </tr>
                    <tr>
                        <td>
                            <div id="tablaFormula" runat="server" class="form-group"><br />
                                <table style="table-layout:fixed">
                                    <tr>
                                        <td style="width:20%">
                                            <label>Operador 1</label>
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            <label>Operación 1</label>
                                        </td>
                                        <td style="width:20%">
                                            <label>Operador 2</label>
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            <label>Operación 2</label>
                                        </td>
                                        <td style="width:20%">
                                             <label>Operador 3</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; padding-bottom:5px">
                                            <asp:DropDownList ID="ddlTipoOperador1" runat="server" class="form-control">
                                             <asp:ListItem Value="0" Text="Parámetro"></asp:ListItem>
                                             <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                                          </asp:DropDownList>
                                            
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            
                                            
                                        </td>
                                        <td style="width:20%; padding-bottom:5px">
                                            <asp:DropDownList ID="ddlTipoOperador2" runat="server" class="form-control">
                                             <asp:ListItem Value="0" Text="Parámetro"></asp:ListItem>
                                             <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                                          </asp:DropDownList>
                                            
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            
                                            
                                        </td>
                                        <td style="width:20%; padding-bottom:5px">
                                          <asp:DropDownList ID="ddlTipoOperador3" runat="server" class="form-control">
                                             <asp:ListItem Value="0" Text="Parámetro"></asp:ListItem>
                                             <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                                          </asp:DropDownList>
                                            
                                        </td>
                                        
                                    </tr>     
                                    <tr>
                                        <td style="width:20%">
                                            <div runat="server" id="operador1List"><asp:DropDownList Width="100%" CssClass="form-control" ID="ddlOperador1" runat="server"> 
                                            </asp:DropDownList></div>
                                            <div runat="server" id="operador1Text">
                                                <asp:TextBox Width="100%" CssClass="form-control" ID="txtOperador1" runat="server" Text="">
                                                </asp:TextBox>
                                            </div>
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            <asp:DropDownList Width="100%" CssClass="form-control" ID="ddlOperacion1" runat="server"> 
                                                <asp:ListItem Value="0" Text=" "></asp:ListItem>
                                                <asp:ListItem Value="+" Text="+"></asp:ListItem>
                                                <asp:ListItem Value="-" Text="-"></asp:ListItem>
                                                <asp:ListItem Value="x" Text="x"></asp:ListItem>
                                                <asp:ListItem Value="/" Text="/"></asp:ListItem>
                                            </asp:DropDownList>  
                                        </td>
                                        <td style="width:20%">
                                            <div runat="server" id="operador2List"><asp:DropDownList Width="100%" CssClass="form-control" ID="ddlOperador2" runat="server"> 
                                            </asp:DropDownList></div>
                                            <div runat="server" id="operador2Text">
                                                <asp:TextBox Width="100%" CssClass="form-control" ID="txtOperador2" runat="server" Text="">
                                                </asp:TextBox>
                                            </div>
                                        </td>
                                        <td  style="padding-left:15px; padding-right:15px; width:10%">
                                            <asp:DropDownList Width="100%" CssClass="form-control" ID="ddlOperacion2" runat="server"> 
                                                <asp:ListItem Value="0" Text=" "></asp:ListItem>
                                                <asp:ListItem Value="+" Text="+"></asp:ListItem>
                                                <asp:ListItem Value="-" Text="-"></asp:ListItem>
                                                <asp:ListItem Value="x" Text="x"></asp:ListItem>
                                                <asp:ListItem Value="/" Text="/"></asp:ListItem>
                                            </asp:DropDownList>  
                                        </td>
                                        <td style="width:20%">
                                            <div runat="server" id="operador3List"><asp:DropDownList Width="100%" CssClass="form-control" ID="ddlOperador3" runat="server"> 
                                            </asp:DropDownList></div>
                                            <div runat="server" id="operador3Text">
                                                <asp:TextBox Width="100%" CssClass="form-control" ID="txtOperador3" runat="server" Text="">
                                                </asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        <br />
                                <label>Descripción</label>
                                <asp:TextBox ID="txtDescripcion"  TextMode="MultiLine" Rows="5" runat="server" class="form-control"></asp:TextBox>

                        </td>
                    </tr>
              </table>
            <br />
        </div>
    </div>

    <%--Específico --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-bookmark"></i><a name="especifico">Centros</a></h6>                
        </div>
        <div class="panel-body">
        <div class="form-group">
                            <label>
                                ¿Especificar centros?</label>
                            <asp:DropDownList ID="ddlEspecifico" style="width:15%" runat="server" class="form-control">
                                <asp:ListItem Value="0" Text="Todos"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Centros"></asp:ListItem>
                            </asp:DropDownList>
                </div>
                <center>
            <table id="tablaCentros" runat="server" style="width:60%">
                <tr>
                    <td style="width:45%"> 
                        <center><label>Centros a asignar</label></center>
                    </td>
                    <td style="width:10%">
                    
                    </td>
                    <td style="width:45%"> 
                        <center><label>Centros asignadas</label></center>
                    </td>
                </tr>
                <tr>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstCentralesAsignar" runat="server">
                        </asp:ListBox></center>
                    </td>
                    <td>
                    <center>
                        <input id="btnAsignarCentro" style="margin-top:5px;width:70px" type="button" value=">" class="btn btn-primary run-first" />
                        <input id="btnNoAsignarCentro" style="margin-top:5px;width:70px" type="button" value="<" class="btn btn-primary run-first" />
                    </center>
                    </td>
                    <td class="form-group"> 
                        <center><asp:ListBox SelectionMode="Multiple" style="width:250px" Rows="10" ID="lstCentralesAsignadas" runat="server">
                        </asp:ListBox></center>
                        <asp:HiddenField ID="hdnCentrosSeleccionados" runat="server" Value="" />
                    </td>
                </tr>
            </table></center>
            <br />
        </div>
    </div>
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarIndicador" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <a href="/evr/configuracion/indicadores" title="Volver" class="btn btn-primary run-first">Volver</a>
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
