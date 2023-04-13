<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.VISTA_ObtenerUsuario oUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (ViewData["centrosasignables"] != null)
        {
            ddlCentros.DataSource = ViewData["centrosasignables"];
            ddlCentros.DataValueField = "id";
            ddlCentros.DataTextField = "nombre";
            ddlCentros.DataBind();
        }

        if (ViewData["unidades"] != null)
        {
            ddlTipo.DataSource = ViewData["unidades"];
            ddlTipo.DataValueField = "id";
            ddlTipo.DataTextField = "nombre";
            ddlTipo.DataBind();
        }

        if (ViewData["centrosasignados"] != null)
        {
            DatosCentros.DataSource = ViewData["centrosasignados"];
            DatosCentros.DataBind();
        }

        //if (user.perfil == 1)
        //{
        //    ListItem perfilgeneral1 = new ListItem();
        //    perfilgeneral1.Value = "5";
        //    perfilgeneral1.Text = "Lector General";
        //    ddlPerfil.Items.Insert(0, perfilgeneral1);

        //    ListItem perfilgeneral2 = new ListItem();
        //    perfilgeneral2.Value = "2";
        //    perfilgeneral2.Text = "Gestor General";
        //    ddlPerfil.Items.Insert(0, perfilgeneral2);

        //    ListItem perfilgeneral3 = new ListItem();
        //    perfilgeneral3.Value = "1";
        //    perfilgeneral3.Text = "Administrador General";
        //    ddlPerfil.Items.Insert(0, perfilgeneral3);

        //}
        //else
        //{
        //    txtLogin.ReadOnly = true;
        //    txtPassword.ReadOnly = true;
        //    txtRepetir.ReadOnly = true;
        //    ddlPerfil.Enabled = false;
        //    txtNombre.ReadOnly = true;
        //    txtMail.ReadOnly = true;
        //    txtPuesto.ReadOnly = true;

        //}


        if (!IsPostBack)
        {
            oUsuario = (MIDAS.Models.VISTA_ObtenerUsuario)ViewData["EditarUsuario"];
            if (oUsuario != null)
            {
                hdnIdUsuario.Value = oUsuario.idUsuario.ToString();
                txtLogin.Text = oUsuario.nombre.ToString();
                txtPassword.Attributes.Add("value", oUsuario.password.ToString());
                txtRepetir.Attributes.Add("value", oUsuario.password.ToString());
                ddlPerfil.SelectedValue = oUsuario.perfil.ToString();
                txtNombre.Text = oUsuario.nombreap.ToString();
                txtMail.Text = oUsuario.mail.ToString();
                txtTelefono.Text = oUsuario.telefono;
                txtPuesto.Text = oUsuario.puesto;
                if (oUsuario.idUnidad.ToString() == "0")
                {
                    ddlTipo.SelectedValue = "0";
                }
                else
                {
                    ddlTipo.SelectedValue = oUsuario.idUnidad.ToString();
                }

                //ddlTipo.SelectedValue = oUsuario.idUnidad.ToString();
                //txtExpira.Text = oUsuario.fecha_registro.ToString().Replace(" 0:00:00", "");
                unidadNegocio.Value =  oUsuario.idUnidad.ToString();
                ViewData["idUsuario"] = oUsuario.idUsuario;
            }
            else
            {
                //extender.Visible = false;
                //txtExpira.Text = DateTime.Now.Date.ToString().Replace(" 0:00:00", "");
                hdnIdUsuario.Value = "0";
            }

            if (Session["UsuarioErroneo"] != null)
            {
                oUsuario = (MIDAS.Models.VISTA_ObtenerUsuario)Session["UsuarioErroneo"];
                hdnIdUsuario.Value = oUsuario.idUsuario.ToString();
                txtLogin.Text = oUsuario.nombre.ToString();
                txtPassword.Attributes.Add("value", oUsuario.password.ToString());
                txtRepetir.Attributes.Add("value", oUsuario.password.ToString());
                ddlPerfil.SelectedValue = oUsuario.perfil.ToString();
                txtNombre.Text = oUsuario.nombreap.ToString();
                txtMail.Text = oUsuario.mail.ToString();
                txtTelefono.Text = oUsuario.telefono.ToString();
                txtPuesto.Text = oUsuario.puesto.ToString();

                Session.Remove("UsuarioErroneo");
            }


        }

        if (Session["EdicionUsuarioMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionUsuarioMensaje"].ToString() + "' });", true);
            Session["EdicionUsuarioMensaje"] = null;
        }

        if (Session["EdicionUsuarioError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionUsuarioError"].ToString() + "' });", true);
            Session["EdicionUsuarioError"] = null;
        }


    }


</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Usuario </title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarUsuario")
                    $("#hdFormularioEjecutado").val("GuardarUsuario");
                if (val == "AsignarCentro")
                    $("#hdFormularioEjecutado").val("AsignarCentro");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_ddlPerfil").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlPerfil').val();

                if (perfilseleccionado == 2) {
                    $("#panelcentrales").show();
                }
                else {
                    $("#panelcentrales").hide();
                }
            });

            if ($('#ctl00_MainContent_unidadNegocio').val() == null || $('#ctl00_MainContent_unidadNegocio').val() == "0" || $('#ctl00_MainContent_unidadNegocio').val() == "") {
                $('#ctl00_MainContent_ddlTipo').prepend($('<option />', {
                    text: 'No',
                    value: 0,
                    selected: true
                }));
                console.log("entra en el if con valor " + $('#ctl00_MainContent_unidadNegocio').val());
            } else {

                $('#ctl00_MainContent_ddlTipo').prepend($('<option />', {
                    text: 'No',
                    value: 0
                }));
                console.log("entra en el else con valor " + $('#ctl00_MainContent_unidadNegocio').val());

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
                Edición del usuario
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
                <i class="icon-user"></i>Datos del usuario</h6>
        </div>
        <div class="panel-body">
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdUsuario" />
                        <label>
                            LOGIN:</label>
                        <asp:TextBox ID="txtLogin" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            CONTRASEÑA:</label>
                        <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            REPETIR CONTRASEÑA:</label>
                        <asp:TextBox ID="txtRepetir" TextMode="Password" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                    <label>
                        PERFIL</label>&nbsp;
                    <asp:DropDownList ID="ddlPerfil" CssClass="form-control" runat="server">
                        <asp:ListItem Value="1">Administrador</asp:ListItem>
                        <asp:ListItem Value="2">Usuario</asp:ListItem>
                    </asp:DropDownList>
                </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            E-MAIL:</label>
                        <asp:TextBox ID="txtMail" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            TELÉFONO/EXTENSIÓN:</label>
                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            PUESTO/CARGO:</label>
                        <asp:TextBox ID="txtPuesto" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:15px">
                        <div class="form-group">
                    <label>
                        RESPONSABLE:</label>&nbsp;
                    <asp:DropDownList ID="ddlTipo" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                            <asp:HiddenField ID="unidadNegocio" Value="0" runat="server"/>
                </div>
                    </td>
                    </tr>
            </table>
            
        </div>
    </div>

    <div class="form-actions text-right">
    <% if (user.perfil == 1)
       { %>
        <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first"/>
        <% } %>
        <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
        <a href="/evr/Configuracion/Usuarios" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>

    <% if ((oUsuario != null && oUsuario.idUsuario != 0) && (ddlPerfil.SelectedValue == "2"))
       { %>
    <div id="panelcentrales" class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Selección de centrales</h6>
        </div>
        <div class="panel-body">

                <table width="50%">
                    <tr>
                        <td>
                            <div class="form-group">
                                <label>
                                    CENTRALES A ASIGNAR:</label>
                                <asp:DropDownList ID="ddlCentros" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td style="padding-left:15px">
                            <div class="form-group">
                                <label>
                                    PERMISO:</label>
                                <asp:DropDownList ID="ddlPermiso" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Text="Lectura"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Escritura"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td  style="padding-left:15px">
                            <div class="form-group">
                                <input style="margin-top: 12px" id="AsignarCentro" type="submit" value="Asignar Central"
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
                                            Central
                                        </th>    
                                        <th>
                                            Tipo
                                        </th> 
                                        <th>
                                            Ubicación
                                        </th>       
                                        <th>
                                            Permiso
                                        </th>                              
                                        <th style="width: 45px">
                                            Eliminar
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
                                        <td class="task-desc">
                                            <%= item.Cells[3].Text %>
                                        </td>    
                                        <td class="task-desc">
                                            <%= item.Cells[4].Text %>
                                        </td>   
                                        <td class="task-desc">
                                            <%= item.Cells[8].Text %>
                                        </td>                                      
                                        <td class="text-center">
                                            <a onclick="return confirm('¿Está seguro que desea eliminar la asociación con el centro?');"
                                                href="/evr/Configuracion/eliminar_usuariocentro/<%= item.Cells[0].Text %>" title="Eliminar"><i
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
    
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
