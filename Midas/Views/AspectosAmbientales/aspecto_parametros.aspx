<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    MIDAS.Models.aspecto_tipo oTipoAspecto;
    MIDAS.Models.aspecto_valoracion oValoracion;
    MIDAS.Models.aspecto_parametros oParametros;
    MIDAS.Models.aspecto_parametro_valoracion oParametroValoracion;
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
        
        oTipoAspecto = (MIDAS.Models.aspecto_tipo)ViewData["tipoaspecto"];
        oValoracion = (MIDAS.Models.aspecto_valoracion)ViewData["valoracionaspecto"];
        oParametroValoracion = (MIDAS.Models.aspecto_parametro_valoracion)ViewData["valoracionparametro"];

        string cadenamagnitud1 = string.Empty;
        string cadenamagnitud2 = string.Empty;
        string cadenamagnitud3 = string.Empty;

        if (oTipoAspecto.Grupo == 13)
        {
            cadenamagnitud1 = "Funcionamiento de la Central < 4.320 h";
            cadenamagnitud2 = "Funcionamiento de la Central entre 4.320 y 7.200 h";
            cadenamagnitud3 = "Funcionamiento de la Central > 7.200 h";
        }
        
        if (cadenamagnitud1 != string.Empty)
        {
            ListItem itemMagnitud1 = new ListItem();
            itemMagnitud1.Value = "1";
            itemMagnitud1.Text = cadenamagnitud1;
            ddlMagnitud.Items.Add(itemMagnitud1);
            ListItem itemMagnitud2 = new ListItem();
            itemMagnitud2.Value = "2";
            itemMagnitud2.Text = cadenamagnitud2;
            ddlMagnitud.Items.Add(itemMagnitud2);
            ListItem itemMagnitud3 = new ListItem();
            itemMagnitud3.Value = "3";
            itemMagnitud3.Text = cadenamagnitud3;
            ddlMagnitud.Items.Add(itemMagnitud3);
        }

        string cadenanaturaleza1 = string.Empty;
        string cadenanaturaleza2 = string.Empty;
        string cadenanaturaleza3 = string.Empty;
        if (oTipoAspecto.Grupo == 6)
        {
            cadenanaturaleza1 = "Vertido de aguas pluviales";
            cadenanaturaleza2 = "Vertido de aguas sanitarias";
            cadenanaturaleza3 = "Vertido de aguas de proceso";
        }

        if (oTipoAspecto.Grupo == 9)
        {
            cadenanaturaleza1 = "Gas natural o biomasa";
            cadenanaturaleza2 = "Fuel, Gasóleo y/o Carbón con bajo contenido azufre";
            cadenanaturaleza3 = "Gasoil / Fuel / Carbón / Diésel";
        }

        if (oTipoAspecto.Grupo == 10)
        {
            cadenanaturaleza1 = "Agua de mar";
            cadenanaturaleza2 = "Aguas Superficiales (rio, pantano, embalse,...) o de red";
            cadenanaturaleza3 = "Aguas subterráneas";
        }


        if (oTipoAspecto.Grupo == 12)
        {
            cadenanaturaleza1 = "No Peligrosa";
            cadenanaturaleza2 = "-";
            cadenanaturaleza3 = "Peligrosa";
        }

        if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
        {
            cadenanaturaleza1 = "Polígono Industrial";
            cadenanaturaleza2 = "Zona Urbana / No Protegida";
            cadenanaturaleza3 = "Zona Sensible / Protegida";
        }        

        if (cadenanaturaleza1 != string.Empty)
        {
            ListItem itemnaturaleza1 = new ListItem();
            itemnaturaleza1.Value = "1";
            itemnaturaleza1.Text = cadenanaturaleza1;
            ddlNaturaleza.Items.Add(itemnaturaleza1);
            ListItem itemnaturaleza2 = new ListItem();
            itemnaturaleza2.Value = "2";
            itemnaturaleza2.Text = cadenanaturaleza2;
            ddlNaturaleza.Items.Add(itemnaturaleza2);
            ListItem itemnaturaleza3 = new ListItem();
            itemnaturaleza3.Value = "3";
            itemnaturaleza3.Text = cadenanaturaleza3;
            ddlNaturaleza.Items.Add(itemnaturaleza3);
        }

        string cadenaorigen1 = string.Empty;
        string cadenaorigen2 = string.Empty;
        string cadenaorigen3 = string.Empty;
        if (oTipoAspecto.Grupo == 1)
        {
            cadenaorigen1 = "Sistemas de reducción de emisiones o depuración en origen";
            cadenaorigen2 = "Sistemas de reducción de emisiones o depuración a posteriori";
            cadenaorigen3 = "Emisión libre";
        }
        
        if (oTipoAspecto.Grupo == 6)
        {
            cadenaorigen1 = "Vertido a red de saneamiento";
            cadenaorigen2 = "Vertido al mar / aguas superficiales";
            cadenaorigen3 = "Vertido a aguas subterráneas";
        }

        if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
        {
            cadenaorigen1 = "Polígono Industrial";
            cadenaorigen2 = "Zona Urbana / No Protegida";
            cadenaorigen3 = "Zona Sensible / Protegida";
        }
        
        if (cadenaorigen1 != string.Empty)
        {
            ListItem itemOrigen1 = new ListItem();
            itemOrigen1.Value = "1";
            itemOrigen1.Text = cadenaorigen1;
            ddlOrigen.Items.Add(itemOrigen1);
            ListItem itemOrigen2 = new ListItem();
            itemOrigen2.Value = "2";
            itemOrigen2.Text = cadenaorigen2;
            ddlOrigen.Items.Add(itemOrigen2);
            ListItem itemOrigen3 = new ListItem();
            itemOrigen3.Value = "3";
            itemOrigen3.Text = cadenaorigen3;
            ddlOrigen.Items.Add(itemOrigen3);
        }

        if (oParametroValoracion != null)
        {

            txtObservaciones.Text = oParametroValoracion.observaciones;
            
            if (oParametroValoracion.mes1 != null)
                txtEne.Text = oParametroValoracion.mes1.ToString();
            if (oParametroValoracion.mes2 != null)
                txtFeb.Text = oParametroValoracion.mes2.ToString();
            if (oParametroValoracion.mes3 != null)
                txtMar.Text = oParametroValoracion.mes3.ToString();
            if (oParametroValoracion.mes4 != null)
                txtAbr.Text = oParametroValoracion.mes4.ToString();
            if (oParametroValoracion.mes5 != null)
                txtMay.Text = oParametroValoracion.mes5.ToString();
            if (oParametroValoracion.mes6 != null)
                txtJun.Text = oParametroValoracion.mes6.ToString();
            if (oParametroValoracion.mes7 != null)
                txtJul.Text = oParametroValoracion.mes7.ToString();
            if (oParametroValoracion.mes8 != null)
                txtAgo.Text = oParametroValoracion.mes8.ToString();
            if (oParametroValoracion.mes9 != null)
                txtSep.Text = oParametroValoracion.mes9.ToString();
            if (oParametroValoracion.mes10 != null)
                txtOct.Text = oParametroValoracion.mes10.ToString();
            if (oParametroValoracion.mes11 != null)
                txtNov.Text = oParametroValoracion.mes11.ToString();
            if (oParametroValoracion.mes12 != null)
                txtDic.Text = oParametroValoracion.mes12.ToString();


            txtReferencia.Text = oParametroValoracion.referencia.ToString();
            if (oParametroValoracion.referenciasup != null)
                txtRefSuperior.Text = oParametroValoracion.referenciasup.ToString();
            txtMed1.Text = oParametroValoracion.anio1.ToString();
            txtMed2.Text = oParametroValoracion.anio2.ToString();
            txtMed3.Text = oParametroValoracion.anio3.ToString();
            txtMed4.Text = oParametroValoracion.anio4.ToString();
            txtMed5.Text = oParametroValoracion.anio5.ToString();
            txtMed6.Text = oParametroValoracion.anio6.ToString();

            txtDia.Text = oParametroValoracion.RU_Dia.ToString();
            txtRefDia.Text = oParametroValoracion.RU_DiaRef.ToString();
            txtTarde.Text = oParametroValoracion.RU_Tarde.ToString();
            txtRefTarde.Text = oParametroValoracion.RU_TardeRef.ToString();
            txtNoche.Text = oParametroValoracion.RU_Noche.ToString();
            txtRefNoche.Text = oParametroValoracion.RU_NocheRef.ToString();

            oParametros = (MIDAS.Models.aspecto_parametros)ViewData["EditarParametros"];

            if (oParametros != null)
            {
                if (oTipoAspecto.relativoMwhb == true && oParametros.Mwhb1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.Mwhb1.ToString();
                    txtRelativoAnio2.Text = oParametros.Mwhb2.ToString();
                    txtRelativoAnio3.Text = oParametros.Mwhb3.ToString();
                    txtRelativoAnio4.Text = oParametros.Mwhb4.ToString();
                    txtRelativoAnio5.Text = oParametros.Mwhb5.ToString();
                    txtRelativoAnio6.Text = oParametros.Mwhb6.ToString();
                }
                if (oTipoAspecto.relativom3Hora == true && oParametros.m3Hora1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.m3Hora1.ToString();
                    txtRelativoAnio2.Text = oParametros.m3Hora2.ToString();
                    txtRelativoAnio3.Text = oParametros.m3Hora3.ToString();
                    txtRelativoAnio4.Text = oParametros.m3Hora4.ToString();
                    txtRelativoAnio5.Text = oParametros.m3Hora5.ToString();
                    txtRelativoAnio6.Text = oParametros.m3Hora6.ToString();
                }
                if (oTipoAspecto.relativokmAnio == true && oParametros.kmAnio1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.kmAnio1.ToString();
                    txtRelativoAnio2.Text = oParametros.kmAnio2.ToString();
                    txtRelativoAnio3.Text = oParametros.kmAnio3.ToString();
                    txtRelativoAnio4.Text = oParametros.kmAnio4.ToString();
                    txtRelativoAnio5.Text = oParametros.kmAnio5.ToString();
                    txtRelativoAnio6.Text = oParametros.kmAnio6.ToString();
                }

                if (oTipoAspecto.relativohfuncAnio == true && oParametros.hfuncAnio1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.hfuncAnio1.ToString();
                    txtRelativoAnio2.Text = oParametros.hfuncAnio2.ToString();
                    txtRelativoAnio3.Text = oParametros.hfuncAnio3.ToString();
                    txtRelativoAnio4.Text = oParametros.hfuncAnio4.ToString();
                    txtRelativoAnio5.Text = oParametros.hfuncAnio5.ToString();
                    txtRelativoAnio6.Text = oParametros.hfuncAnio6.ToString();
                }

                if (oTipoAspecto.relativohAnioGE == true && oParametros.hAnioGE1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.hAnioGE1.ToString();
                    txtRelativoAnio2.Text = oParametros.hAnioGE2.ToString();
                    txtRelativoAnio3.Text = oParametros.hAnioGE3.ToString();
                    txtRelativoAnio4.Text = oParametros.hAnioGE4.ToString();
                    txtRelativoAnio5.Text = oParametros.hAnioGE5.ToString();
                    txtRelativoAnio6.Text = oParametros.hAnioGE6.ToString();
                }

                if (oTipoAspecto.relativonumtrab == true && oParametros.numtrabAnio1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.numtrabAnio1.ToString();
                    txtRelativoAnio2.Text = oParametros.numtrabAnio2.ToString();
                    txtRelativoAnio3.Text = oParametros.numtrabAnio3.ToString();
                    txtRelativoAnio4.Text = oParametros.numtrabAnio4.ToString();
                    txtRelativoAnio5.Text = oParametros.numtrabAnio5.ToString();
                    txtRelativoAnio6.Text = oParametros.numtrabAnio6.ToString();
                }

                if (oTipoAspecto.relativom3aguadeitsalada == true && oParametros.m3aguadesaladaAnio1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.m3aguadesaladaAnio1.ToString();
                    txtRelativoAnio2.Text = oParametros.m3aguadesaladaAnio2.ToString();
                    txtRelativoAnio3.Text = oParametros.m3aguadesaladaAnio3.ToString();
                    txtRelativoAnio4.Text = oParametros.m3aguadesaladaAnio4.ToString();
                    txtRelativoAnio5.Text = oParametros.m3aguadesaladaAnio5.ToString();
                    txtRelativoAnio6.Text = oParametros.m3aguadesaladaAnio6.ToString();
                }

                if (oTipoAspecto.relativotrabcantera == true && oParametros.m3aguadesaladaAnio1 != null)
                {
                    txtRelativoAnio1.Text = oParametros.trabcanteraAnio1.ToString();
                    txtRelativoAnio2.Text = oParametros.trabcanteraAnio2.ToString();
                    txtRelativoAnio3.Text = oParametros.trabcanteraAnio3.ToString();
                    txtRelativoAnio4.Text = oParametros.trabcanteraAnio4.ToString();
                    txtRelativoAnio5.Text = oParametros.trabcanteraAnio5.ToString();
                    txtRelativoAnio6.Text = oParametros.trabcanteraAnio6.ToString();
                }

                if (oTipoAspecto.relativono == true)
                {
                    filaProduccion.Visible = false;
                }
            }
            
            txtVariacion.ReadOnly = true;
            txtAcercamiento.ReadOnly = true;
            txtVariacion.Text = oParametroValoracion.variacion.ToString();
            txtAcercamiento.Text = oParametroValoracion.acercamiento.ToString();


            decimal total = 0;
            if (oTipoAspecto.Grupo == 8)
            {
                total = MIDAS.Models.Datos.CalcularTotalResiduosCentral(centroseleccionado.id);
                txtTotal.Text = total.ToString();
                txtMagnitudRel.Text = oParametroValoracion.acercamiento.ToString();
            }
            if (oTipoAspecto.Grupo == 9)
            {
                total = MIDAS.Models.Datos.CalcularTotalConsumoCombustibleCentral(centroseleccionado.id);
                txtTotal.Text = total.ToString();
                txtMagnitudRel.Text = oParametroValoracion.acercamiento.ToString();
            }

            if (oTipoAspecto.Grupo == 12)
            {
                total = MIDAS.Models.Datos.CalcularTotalConsumoSustanciasCentral(centroseleccionado.id);
                txtTotal.Text = total.ToString();
                txtMagnitudRel.Text = oParametroValoracion.acercamiento.ToString();
            }

            ddlMagnitud.SelectedValue = oParametroValoracion.magnitud.ToString();
            ddlNaturaleza.SelectedValue = oParametroValoracion.naturaleza.ToString();
            ddlOrigen.SelectedValue = oParametroValoracion.origen.ToString();

            txtMagnitud.Text = oParametroValoracion.resmagnitud.ToString();
            txtNaturaleza.Text = oParametroValoracion.resnaturaleza.ToString();
            txtOrigen.Text = oParametroValoracion.resorigen.ToString();

            if (oParametroValoracion.significancia1 == 1)
            {
                txtSignificancia1.Text = "Significativo";
                txtSignificancia1.BackColor = System.Drawing.Color.Red;
                txtSignificancia1.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia1 == 0)
            {
                txtSignificancia1.Text = "No Significativo";
                txtSignificancia1.BackColor = System.Drawing.Color.Green;
                txtSignificancia1.ForeColor = System.Drawing.Color.White;
            }

            if (oParametroValoracion.significancia2 == 1)
            {
                txtSignificancia2.Text = "Significativo";
                txtSignificancia2.BackColor = System.Drawing.Color.Red;
                txtSignificancia2.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia2 == 0)
            {
                txtSignificancia2.Text = "No Significativo";
                txtSignificancia2.BackColor = System.Drawing.Color.Green;
                txtSignificancia2.ForeColor = System.Drawing.Color.White;
            }

            if (oParametroValoracion.significancia3 == 1)
            {
                txtSignificancia3.Text = "Significativo";
                txtSignificancia3.BackColor = System.Drawing.Color.Red;
                txtSignificancia3.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia3 == 0)
            {
                txtSignificancia3.Text = "No Significativo";
                txtSignificancia3.BackColor = System.Drawing.Color.Green;
                txtSignificancia3.ForeColor = System.Drawing.Color.White;
            }

            if (oParametroValoracion.significancia4 == 1)
            {
                txtSignificancia4.Text = "Significativo";
                txtSignificancia4.BackColor = System.Drawing.Color.Red;
                txtSignificancia4.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia4 == 0)
            {
                txtSignificancia4.Text = "No Significativo";
                txtSignificancia4.BackColor = System.Drawing.Color.Green;
                txtSignificancia4.ForeColor = System.Drawing.Color.White;
            }

            if (oParametroValoracion.significancia5 == 1)
            {
                txtSignificancia5.Text = "Significativo";
                txtSignificancia5.BackColor = System.Drawing.Color.Red;
                txtSignificancia5.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia5 == 0)
            {
                txtSignificancia5.Text = "No Significativo";
                txtSignificancia5.BackColor = System.Drawing.Color.Green;
                txtSignificancia5.ForeColor = System.Drawing.Color.White;
            }

            if (oParametroValoracion.significancia6 == 1)
            {
                txtSignificancia6.Text = "Significativo";
                txtSignificancia6.BackColor = System.Drawing.Color.Red;
                txtSignificancia6.ForeColor = System.Drawing.Color.White;
            }
            if (oParametroValoracion.significancia6 == 0)
            {
                txtSignificancia6.Text = "No Significativo";
                txtSignificancia6.BackColor = System.Drawing.Color.Green;
                txtSignificancia6.ForeColor = System.Drawing.Color.White;
            }
        }

        if (Session["EdicionAspectoParametrosMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionAspectoParametrosMensaje"].ToString() + "' });", true);
            Session["EdicionAspectoParametrosMensaje"] = null;
        }

        if (Session["EdicionAspectoParametrosError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionAspectoParametrosError"].ToString() + "' });", true);
            Session["EdicionAspectoParametrosError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Parámetros del aspecto </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                $("#hdFormularioEjecutado").val("GuardarAspectoValoracion");
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
                Parámetros
            </h3>
            <% 
                if (oValoracion.foco == 1)
                { %>
                <label>-Si es en continuo: Introducir el dato medio mensual de concentración</label><br />
                <label>-Si no es en continuo: Introducir el dato puntual de concentración en los meses en los que haya habido medición</label>
                <br /><br />

                <% } %>
        </div> 
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-search"></i>Datos generales</h6>
        </div>
        <div class="panel-body" style="padding-top:0px">
        <br />
            <table style="width:100%">
                <tr>
                    <td>
                        <label>
                            Observaciones</label>
                        <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" Rows="4" runat="server"
                            class="form-control"></asp:TextBox>
                            <br />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <% if (oTipoAspecto.Grupo != 13 && oTipoAspecto.Grupo != 14 && oTipoAspecto.Codigo != "AD-CO-5" && oTipoAspecto.Codigo != "AD-CO-6")
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-cog"></i><%= oParametroValoracion.nombre%>
                <% if (oTipoAspecto.Grupo == 1)
                    {%>
                 (mg/Nm3) 
                <% } %>
                <% 
                    if (oTipoAspecto.Grupo == 12)
                    {%>
                 (kg)
                <% } %>

            </h6>
        </div>
        <div class="panel-body">
        <br />
            <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Ene</label>
                        <asp:TextBox ID="txtEne" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                     <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Feb</label>
                        <asp:TextBox ID="txtFeb" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Mar</label>
                        <asp:TextBox ID="txtMar" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Abr</label>
                        <asp:TextBox ID="txtAbr" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                May</label>
                        <asp:TextBox ID="txtMay" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Jun</label>
                        <asp:TextBox ID="txtJun" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Jul</label>
                        <asp:TextBox ID="txtJul" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Ago</label>
                        <asp:TextBox ID="txtAgo" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Sep</label>
                        <asp:TextBox ID="txtSep" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Oct</label>
                        <asp:TextBox ID="txtOct" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Nov</label>
                        <asp:TextBox ID="txtNov" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                        <td class="form-group" style="width: 8%">
                        <center>
                            <label>
                                Dic</label>
                        <asp:TextBox ID="txtDic" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>   
                    </tr>
                </table><br />
            </div>
        </div>
        <% } %>
              
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-rulers"></i>Medición</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <asp:HiddenField runat="server" ID="hdnIdValoracion" />
            </div>

            <table style="width:100%; border: 1px solid #41b9e6">
                <tr runat="server" id="filaProduccion">
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                    <center>
                        <label>
                            Ponderación 
                        </label>
                        <label>
                                        <% 
                                        if (oTipoAspecto.relativoMwhb == true)
                                        { %>
                                        (Mwhb)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativohAnioGE == true)
                                        { %>
                                        (Horas/Año de Grupo Elec.)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativohfuncAnio== true)
                                        { %>
                                        (Horas de func./Año)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativokmAnio == true)
                                        { %>
                                        (Km/Año)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativom3Hora == true)
                                        { %>
                                        (m3/Hora)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativonumtrab == true)
                                        { %>
                                        (num.trabajadores)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativom3aguadeitsalada == true)
                                        { %>
                                        (m3 agua desalada)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativotrabcantera == true)
                                        { %>
                                        (trabajadores cantera)
                                        <% } %>
                                    </label></center>
                    </td>
                        <td style="border: 1px solid #41b9e6; padding:15px">
                                    <table runat="server" width="100%" id="tablaProduccion">
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-6 %></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-5 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-4 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-3 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-2 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-1 %></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio1" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio2" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio3" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio4" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio5" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio6" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr></table>
                    </td>
                </tr>
                <tr runat="server" id="filaDatosCantidad">
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Datos Cantidad
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                         <table runat="server" width="100%" id="Table2">
                                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-6 %></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-5 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-4 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-3 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-2 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-1 %></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed1" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Cálculos
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding-bottom:15px; padding-left:10px; padding-right:10px">

                            <% if (oTipoAspecto.Grupo != 7 && oTipoAspecto.Grupo != 8)
                               { %>
                            <table style="width: 55%; margin-top:10px; margin-left:15px">
                                <tr>
                                    <% if (oTipoAspecto.Grupo != 2 && oTipoAspecto.Grupo != 3 && oTipoAspecto.Grupo != 4 && oTipoAspecto.Grupo != 5 && oTipoAspecto.Grupo != 9 && oTipoAspecto.Grupo != 10 && oTipoAspecto.Grupo != 11 && oTipoAspecto.Grupo != 12
                                           && oTipoAspecto.Grupo != 13 && oTipoAspecto.Grupo != 14
                                           && oTipoAspecto.Grupo != 15 && oTipoAspecto.Grupo != 16
                                           && oTipoAspecto.Grupo != 17 && oTipoAspecto.Grupo != 18 && oTipoAspecto.Grupo != 19 && oTipoAspecto.Grupo != 22 &&
                                           oTipoAspecto.Grupo != 23 && oTipoAspecto.Grupo != 24)
                                       { %>
                                    <td style="padding-right: 20px; width: 1%">
                                        <label>
                                        <% if (oTipoAspecto.Grupo == 6 && oParametroValoracion.nombre == "PH")
                                           { %>
                                           V.Referencia Inferior
                                       <%}
                                           else
                                           { %>
                                            V.Referencia
                                            <% } %>
                                            </label>
                                        <asp:TextBox ID="txtReferencia" Width="110px" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <% } %>
                                    <% if (oTipoAspecto.Grupo == 6 && oParametroValoracion.nombre == "PH")
                                       { %>
                                    <td style="padding-right: 20px; width: 7%">
                                        <label>
                                            V.Referencia Superior</label>
                                        <asp:TextBox ID="txtRefSuperior" Width="110px" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <% } %>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                                       { %>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Día</label>
                                        <asp:TextBox ID="txtRefDia" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Tarde</label>
                                        <asp:TextBox ID="txtRefTarde" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Noche</label>
                                        <asp:TextBox ID="txtRefNoche" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <% } %>
                                </tr>
                            </table>
                            <% } %>

                        <table runat="server" width="100%" id="Table3">
                                    <tr>
                            <td style="width: 10%; padding-top: 10px; ">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <label>
                                        Variación(%)
                                    </label>
                                    <% } %>
                                <% if (oTipoAspecto.Grupo == 8)
                               { %>
                                    <label>
                                        Magnitud Relativa(%)</label><% }
                                        %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <label>
                                        Acercamiento(%)</label><% }
                                        %>
                                        <% if (oTipoAspecto.Grupo == 8)
                               { %>
                                    <label>
                                        Total Residuos (t)</label><% }
                                        %>
                                        <% if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                               { %>
                                    <label>
                                        Magnitud Relativa(%)</label><% }
                                        %>
                                       
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                     <% if (oTipoAspecto.Grupo == 9)
                                    { %>
                                    <label>
                                        Total Consumo combustibles (t)</label><% }
                                        %>
                                        <% if (oTipoAspecto.Grupo == 12)
                                    { %>
                                    <label>
                                        Total Consumo combustibles+sustancias</label><% }
                                        %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Día</label>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Tarde</label>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Noche</label>
                                    <%} %></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <asp:TextBox ID="txtVariacion" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                     
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                                   { %>
                                        <asp:TextBox ID="txtMagnitudRel" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                        <% } %>
                                        <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <asp:TextBox ID="txtAcercamiento" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    
                                      <% if (oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                               { %>
                                    <asp:TextBox ID="txtTotal" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtDia" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtTarde" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtNoche" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                        </tr>

                        </table>
                
                            <table width="100%">
                <tr>
                    <td style="width: 30%;">
                        <center>
                            <%
                        if (oTipoAspecto.Grupo == 13)
                        {                                  
                            %>
                            <label>
                                Horas de funcionamiento de la central</label>
                            <% } %>                         
                        </center>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <center>
                                                
                        <%
                                 if (oTipoAspecto.Grupo == 9)
                                { %>
                                <label>Tipo de Combustible</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 10)
                                { %>
                                <label>Origen del agua</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 12)
                                { %>
                                <label>Tipo de sustancia</label>
                            <% }   %>
                           

                        </center>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <center>                            
                            <% 
                            if (oTipoAspecto.Grupo == 1)
                            { %>
                            <label>
                                Sistema reducción emisiones</label>
                            <% } %>
                            <% 
                            if (oTipoAspecto.Grupo == 6)
                            { %>
                            <label>
                                Destino del vertido</label>
                            <% } %>
                            <% 
                            if (oTipoAspecto.Grupo == 13)
                            { %>
                            <label>
                                Ubicación de la central</label>
                            <% } %>
                        </center>
                    </td>
                </tr>
                
                <tr>

                    <td style="width: 30%; padding-top: 10px">
                        <%  if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                            {
                        %>
                        <asp:DropDownList runat="server" ID="ddlMagnitud" class="form-control" Width="95%">
                        </asp:DropDownList>
                        <% } %>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <%  if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                       { %>
                        <center>
                            <asp:DropDownList runat="server" ID="ddlNaturaleza" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </center>
                        <% } %>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                       { %>
                        <center>
                            <asp:DropDownList runat="server" ID="ddlOrigen" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </center>
                        <% } %>
                    </td>
                </tr>
            </table>
                    
                    </td>
                </tr>

                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Evaluación
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                        <table runat="server" width="100%" id="Table4">
                            <tr>
                                <td>
                                    <center>
                                        <label>
                                            Magnitud(M)
                                        </label>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <label>
                                            Nat/Pel/A(N)
                                        </label>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <label>
                                            Origen/Destino(D)
                                        </label>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtMagnitud" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtNaturaleza" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtOrigen" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Significancia
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                        <table runat="server" width="100%" id="Table5">
                                    <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-6 %></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-5 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-4 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-3 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-2 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-1 %></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia1" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia2" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia3" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia4" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia5" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia6" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
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
