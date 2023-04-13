<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.requisitoslegales oRequisito = new MIDAS.Models.requisitoslegales();
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

            Session["ModuloAccionMejora"] = 8;

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

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();

                ddlAnio.Items.Insert(0, itemAnio);
            }

            if (ViewData["ambitos"] != null)
            {
                ddlAmbito.DataSource = ViewData["ambitos"];
                ddlAmbito.DataValueField = "id";
                ddlAmbito.DataTextField = "nombre_ambito";
                ddlAmbito.DataBind();
            }

            oRequisito = (MIDAS.Models.requisitoslegales)ViewData["requisito"];

            if (oRequisito != null)
            {
                Session["idRequisito"] = oRequisito.id;
                Session["ReferenciaAccionMejora"] = oRequisito.id;
                txtDenominacion.Text = oRequisito.denominacion;
                txtAnio.Text = oRequisito.codigo;
                if (oRequisito.fecharegistro != null)
                    txtFRegistro.Text = oRequisito.fecharegistro.ToString().Replace(" 0:00:00", "");
                columnaDesplegable.Visible = false;
                if (oRequisito.informeevaluacion != null)
                    lblInformeEvaluacion.Text = oRequisito.informeevaluacion.Replace(Server.MapPath("~/Requisitos") + "\\" + oRequisito.id + "\\", "");
                txtNumRequisitos.Text = oRequisito.numrequisitos.ToString();
                txtCumple.Text = oRequisito.cumple.ToString();
                txtTramite.Text = oRequisito.tramite.ToString();
                txtNoCumple.Text = oRequisito.nocumple.ToString();
                txtObservacion.Text = oRequisito.observacion.ToString();
                txtNoProcede.Text = oRequisito.noprocede.ToString();
                txtNoVerificado.Text = oRequisito.noverificado.ToString();
                ddlAmbito.SelectedValue = oRequisito.ambito.ToString();
            }
            else
            {
                columnaTextbox.Visible = false;
                txtNumRequisitos.Text = "0";
                txtCumple.Text = "0";
                txtTramite.Text = "0";
                txtNoCumple.Text = "0";
                txtObservacion.Text = "0";
                txtNoProcede.Text = "0";
                txtNoVerificado.Text = "0";
            }
        }

        if (permisos.permiso != true)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionRequisitoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionRequisitoMensaje"].ToString() + "' });", true);
            Session["EdicionRequisitoMensaje"] = null;
        }
        if (Session["EdicionRequisitoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionRequisitoError"].ToString() + "' });", true);
            Session["EdicionRequisitoError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtDenominacion.Enabled = false;
        txtAnio.Enabled = false;
        txtFRegistro.Enabled = false;
        txtNumRequisitos.Enabled = false;
        txtCumple.Enabled = false;
        txtTramite.Enabled = false;
        txtNoCumple.Enabled = false;
        txtObservacion.Enabled = false;
        txtNoProcede.Enabled = false;
        txtNoVerificado.Enabled = false;
        ddlAmbito.Enabled = false;
        consulta = 1;
    }
    
    
</script>
<asp:Content ID="headEditarRequisito" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Requisito </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarRequisito")
                    $("#hdFormularioEjecutado").val("GuardarRequisito");
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
                Detalle de la evaluación<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                <i class="icon-balance"></i>Datos Generales</h6>
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
                                Codigo (*)</label>
                            <asp:TextBox ID="txtAnio" Width="92%" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 35%">
                            <label>
                                Denominación (*)</label>
                            <asp:TextBox ID="txtDenominacion" Width="98%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 15%">
                            <label>
                                Ámbito (*)</label>
                            <asp:DropDownList Width="95%" ID="ddlAmbito" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="form-group" style="width: 10%"><center>
                            <label>
                                Fecha Registro (*)</label>
                            <asp:TextBox ID="txtFRegistro" style="text-align:center" Width="95%" runat="server" class="datepicker form-control"></asp:TextBox></center>
                        </td>
                    </tr>
                </table>


        <br />
        <table width="100%">
                <tr>
                    <td colspan="4" class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Evaluación de cumplimiento de requisitos</label>
                            <input type="file" id="FilePInicial" name="file" style="margin-top:10px" /></center>
                        </td>       
                    </tr>
                    <tr  style="padding-top:20px">
                        <td colspan="4">
                            <% if (oRequisito != null && oRequisito.informeevaluacion != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Requisitos/ObtenerInformeEvaluacion/<%=oRequisito.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblInformeEvaluacion" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <%--Evaluación--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i>Evaluación</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                    <tr>
                        <td runat="server" id="Td1" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                N.Requisitos</label>
                            <asp:TextBox ID="txtNumRequisitos" TextMode="Number" Width="92%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td runat="server" id="Td2" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                Cumple</label>
                            <asp:TextBox ID="txtCumple" TextMode="Number" Width="92%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td  runat="server" id="Td3" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                En trámite</label>
                            <asp:TextBox ID="txtTramite" TextMode="Number" Width="98%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td  runat="server" id="Td4" class="form-group" style="width: 14%; text-align:center"><center>
                            <label>No cumple</label>
                            <asp:TextBox ID="txtNoCumple" TextMode="Number"  Width="92%" runat="server" class="form-control"></asp:TextBox></center>
                        </td>
                        <td runat="server" id="Td5" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                Observación</label>
                            <asp:TextBox ID="txtObservacion" TextMode="Number" Width="92%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td runat="server" id="Td6" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                No procede</label>
                            <asp:TextBox ID="txtNoProcede" TextMode="Number" Width="92%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td  runat="server" id="Td7" class="form-group" style="width: 14%; text-align:center">
                            <label>
                                No verificado</label>
                            <asp:TextBox ID="txtNoVerificado" TextMode="Number" Width="98%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>


            </div>
        </div>
    </div>

     <% if (oRequisito != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-stats-up"></i><a name="despliegue">Acciones Mejora</a></h6>
        </div>
        <div class="panel-body">
            <%
        if (permisos.permiso == true && oRequisito != null)
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
            if (oRequisito != null)
            {
        %>
        <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
        <%} %>
        <%
            if (permisos.permiso == true)
            {
        %>
        <input id="GuardarRequisito" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Requisitos/requisitos_legales" title="Volver" class="btn btn-primary run-first">Volver</a>
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
