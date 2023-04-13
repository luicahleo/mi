<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.formacion oFormacion = new MIDAS.Models.formacion();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            Session["ModuloAccionMejora"] = 4;

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

            if (ViewData["accionesmejora"] != null)
            {
                grdAccionesMejora.DataSource = ViewData["accionesmejora"];
                grdAccionesMejora.DataBind();
            }

            for (int i = 2008; i <= DateTime.Now.Year +1; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();

                ddlAnio.Items.Insert(0, itemAnio);
            }
            if (ddlAnio.Items.Count > 1)
                ddlAnio.SelectedIndex = 1;

            oFormacion = (MIDAS.Models.formacion)ViewData["formacion"];

            if (oFormacion != null)
            {
                Session["idFormacion"] = oFormacion.id;
                Session["ReferenciaAccionMejora"] = oFormacion.id;
                txtDenominacion.Text = oFormacion.denominacion;
                txtAnio.Text = oFormacion.codigo;
                if (oFormacion.fecha_registro_inicio != null)
                    txtFRegInicio.Text = oFormacion.fecha_registro_inicio.ToString().Replace(" 0:00:00", "");
                if (oFormacion.fecha_registro_ejecutado != null)
                    txtFRegEjecutado.Text = oFormacion.fecha_registro_ejecutado.ToString().Replace(" 0:00:00", "");
                ddlValoracion.SelectedValue = oFormacion.valoracion_eficacia.ToString();
                if (oFormacion.id != 0)
                    columnaDesplegable.Visible = false;
                else
                    columnaTextbox.Visible = false;
                if (oFormacion.planificacion_inicial != null)
                    lblPlanificacionInicial.Text = oFormacion.planificacion_inicial.Replace(Server.MapPath("~/Formacion") + "\\" + oFormacion.id + "\\PlanInicial\\", "");
                if (oFormacion.planificacion_ejecutada != null)
                    lblPlanificacionEjecutada.Text = oFormacion.planificacion_ejecutada.Replace(Server.MapPath("~/Formacion") + "\\" + oFormacion.id + "\\PlanEjecutada\\", "");

                txtCausasNoRealiz.Text = oFormacion.analisiscausasnorealiz;
                txtCausasNoEfectivas.Text = oFormacion.analisiscausasnoefectivas;
                txtObservaciones.Text = oFormacion.observaciones;
                
                txtActEjecutadas.Text = oFormacion.actividadesejecutadas.ToString();
                txtActPlanificadas.Text = oFormacion.actividadesplanificadas.ToString();
                txtHorasCalidad.Text = oFormacion.horascalidad.ToString();
                txtHorasMedioambiente.Text = oFormacion.horasmedioambiente.ToString();
                txtHorasSegySalud.Text = oFormacion.horasseguridadsalud.ToString();
                txtHorasOtrasAreas.Text = oFormacion.horasotrasareas.ToString();
                                
                if (centroseleccionado.tipo == 4 && oFormacion.idcentral != 0)
                    desactivarCampos();
                if (centroseleccionado.tipo != 4 && oFormacion.idcentral == 0)
                    desactivarCampos();
                
            }
            else
            {
                columnaTextbox.Visible = false;
            }
        }
        if (oFormacion != null)
        {
            if (permisos.permiso != true && oFormacion.idcentral != 0)
            {
                desactivarCampos();
            }
            else
            {
                if (oFormacion.idcentral == 0 && centroseleccionado.tipo != 4)
                {
                    desactivarCampos();
                }
            }
        }
        if (Session["EdicionFormacionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionFormacionMensaje"].ToString() + "' });", true);
            Session["EdicionFormacionMensaje"] = null;
        }
        if (Session["EdicionFormacionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionFormacionError"].ToString() + "' });", true);
            Session["EdicionFormacionError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtDenominacion.Enabled = false;
        txtAnio.Enabled = false;
        txtFRegInicio.Enabled = false;
        txtFRegEjecutado.Enabled = false;
        ddlValoracion.Enabled = false;

        txtCausasNoRealiz.Enabled = false;
        txtCausasNoEfectivas.Enabled = false;
        txtObservaciones.Enabled = false;

        txtActEjecutadas.Enabled = false;
        txtActPlanificadas.Enabled = false;
        txtHorasCalidad.Enabled = false;
        txtHorasMedioambiente.Enabled = false;
        txtHorasSegySalud.Enabled = false;
        txtHorasOtrasAreas.Enabled = false;
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Formación </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarFormacion")
                    $("#hdFormularioEjecutado").val("GuardarFormacion");
                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");
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
                Detalle del plan de formación<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <%--Datos generales --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-pencil"></i>Datos Generales</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                    <tr>
                        <td runat="server" id="columnaDesplegable" class="form-group" style="width: 10%">
                            <label>
                                Año (*)</label>
                            <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="columnaTextbox" class="form-group" style="width: 10%">
                            <label>
                                Codigo</label>
                            <asp:TextBox ID="txtAnio" Width="92%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 40%">
                            <label>
                                Denominación (*)</label>
                            <asp:TextBox ID="txtDenominacion" Width="98%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                             <label>
                                Valoración</label>
                            <asp:DropDownList runat="server" ID="ddlValoracion" class="form-control" Width="95%">
                            <asp:ListItem Value="0" Text="---"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Eficaz"></asp:ListItem>
                            <asp:ListItem Value="2" Text="No eficaz"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 15%"><center>
                            <label>
                                Fecha Registro Inicio</label>
                            <asp:TextBox ID="txtFRegInicio" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width: 15%"><center>
                            <label>
                                Fecha Registro Ejecutado</label>
                            <asp:TextBox ID="txtFRegEjecutado" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Act. Ejecutadas</label>
                            <asp:TextBox ID="txtActEjecutadas" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Act. Planificadas</label>
                            <asp:TextBox ID="txtActPlanificadas" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Calidad (horas)</label>
                            <asp:TextBox ID="txtHorasCalidad" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Medioambiente (horas)</label>
                            <asp:TextBox ID="txtHorasMedioambiente" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Seg. y salud (horas)</label>
                            <asp:TextBox ID="txtHorasSegySalud" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td class="form-group" style="width:15%">
                        <center>
                            <label>
                                Otras áreas (horas)</label>
                            <asp:TextBox ID="txtHorasOtrasAreas" Width="70%" TextMode="Number" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                    </tr>
                </table>

        <br />
        <table width="100%">
                <tr>
                    <td class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Planificación Inicial</label>
                            <input type="file" id="FilePInicial" name="file" style="margin-top:10px" /></center>
                        </td>      
                        <td class="form-group" style="width: 50%"><center>
                        <label>
                                Planificación Ejecutada</label>
                                <input type="file" id="filePEjecutada" name="file" style="margin-top:10px" /></center>
                        </td>   
                    </tr>
                    <tr style="padding-top:20px">
                        <td>
                            <% if (oFormacion != null && oFormacion.planificacion_inicial != null)
                               { %>
                               <center>
                            <a title="Ver Planificación Inicial" href="/evr/Formacion/ObtenerPlanInicial/<%=oFormacion.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblPlanificacionInicial" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                        <td>
                        <% if (oFormacion != null && oFormacion.planificacion_ejecutada != null)
                           { %>
                           <center>
                            <a title="Ver Planificación Ejecutada" href="/evr/Formacion/ObtenerPlanEjecutada/<%=oFormacion.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblPlanificacionEjecutada" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <label>
                            Análisis de las causas por las que no se han realizado las acciones formativas</label>
                            <asp:TextBox ID="txtCausasNoRealiz" TextMode="MultiLine" Rows="3" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                            Análisis de causas en caso de acciones no efectivas</label>
                            <asp:TextBox ID="txtCausasNoEfectivas" TextMode="MultiLine" Rows="3" Width="100%" runat="server"
                            class="form-control"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                            Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" Rows="3" Width="100%" runat="server"
                            class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>

     <% if (oFormacion != null && oFormacion.id != 0)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if ((user.perfil == 1 || user.perfil == 3) && oFormacion != null)
        {
            %>
            <table width="100%">
                <tr>
                    <td>
                        <a href="/evr/accionmejora/detalle_accion/0" title="Nueva Acción Mejora" class="btn btn-primary run-first">Nueva Acción Mejora</a>
                    </td>
                </tr>
            </table>
            <% } %>
            <asp:GridView ID="grdAccionesMejora" runat="server" Visible="false">
            </asp:GridView>
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="width:50px">
                                                                                Año      
                                                                            </th> 
                                                                            <th style="width:120px">
                                                                                Tipo      
                                                                            </th>  
                                                                            <th>
                                                                                Asunto      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha apertura
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha cierre
                                                                            </th>  
                                                                            <th  style="width:150px">
                                                                                Estado      
                                                                            </th>                  
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% }
                                                                               else
                                                                               { %>
                                                                                <th  style="width:50px">
                                                                                Consultar
                                                                            </th>
                                                                               <% } %>
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                            </tr>
                        </thead>
                        <tbody>
                            <% 
        foreach (GridViewRow item in grdAccionesMejora.Rows)
        { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[3].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[4].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[4].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[5].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                            <% 
        if (centroseleccionado.tipo == 4 && user.perfil == 1)
        { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
        else
        { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar acción de mejora" onclick="if(!confirm('¿Está seguro de que desea eliminar este registro de emergencia?')) return false;" href="/evr/accionmejora/eliminar_accionmejora/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            
                                                                        </tr>
                                                                        <% } %>
                        </tbody>
                    </table>
                </div>
            </center>
            <br />
        </div>
    </div>
    <% } %>
    
    <div class="form-actions text-right">
        <%
            if (oFormacion != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (consulta == 0) 
            {
        %>
        <input id="GuardarFormacion" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Formacion/plan_formacion" title="Volver" class="btn btn-primary run-first">Volver</a>
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
