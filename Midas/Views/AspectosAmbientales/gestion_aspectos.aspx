<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Aspectos</title>
    <script type="text/javascript">


        $(document).ready(function () {
            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            $("#ctl00_MainContent_divN3").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "btnAddAspecto")
                    $("#hdFormularioEjecutado").val("btnAddAspecto");
                if (val == "ctl00_MainContent_btnAddAspectoParametro")
                    $("#hdFormularioEjecutado").val("btnAddAspectoParametro");
                if (val == "ctl00_MainContent_btnAddAspectoFoco")
                    $("#hdFormularioEjecutado").val("btnAddAspectoFoco");
                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");
                if (val == "ctl00_MainContent_btnImprimirCatalogo")
                    $("#hdFormularioEjecutado").val("btnImprimirCatalogo");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN3").show();
                $("#btnN3").addClass('active');
            });


        });

        
       
    </script>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }           
            
            for (int i = 2017; i < DateTime.Now.Year -1; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }
            ListItem todos = new ListItem();
            todos.Value = "0";
            todos.Text = "Actual";
            ddlAnio.Items.Insert(0, todos);    

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (user.perfil == 2)
                permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
            else
            {
                permisos.idusuario = user.idUsuario;
                permisos.idcentro = centroseleccionado.id;
                permisos.permiso = true;
            } 

            if (ViewData["aspectosvaloradosP"] != null)
            {
                grdParametros.DataSource = ViewData["aspectosvaloradosP"];
                grdParametros.DataBind();
            }

            if (ViewData["aspectosvaloradosF"] != null)
            {
                grdFocos.DataSource = ViewData["aspectosvaloradosF"];
                grdFocos.DataBind();
            }

            if (ViewData["aspectosAplicablesP"] != null)
            {
                ddlAspectosParametro.DataSource = ViewData["aspectosAplicablesP"];
                ddlAspectosParametro.DataValueField = "id";
                ddlAspectosParametro.DataTextField = "NombreCompleto";
                ddlAspectosParametro.DataBind();
            }

            if (ViewData["aspectosAplicablesF"] != null)
            {
                ddlAspectosFoco.DataSource = ViewData["aspectosAplicablesF"];
                ddlAspectosFoco.DataValueField = "id";
                ddlAspectosFoco.DataTextField = "NombreCompleto";
                ddlAspectosFoco.DataBind();
            }

            if (Session["EditarAspectosResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAspectosResultado"].ToString() + "' });", true);
                Session.Remove("EditarAspectosResultado");
            }

            if (Session["EditarAspectosError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAspectosError"].ToString() + "' });", true);
                Session.Remove("EditarAspectosError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <table width="100%">
    <tr>
        <td style="width:30%">
            <h3>Aspectos Ambientales</h3>
        </td>
        <td style="width:70%; padding-bottom:10px">
            <div style="text-align:right">
                                        <a  class="btn btn-primary run-first" href="/evr/AspectosAmbientales/ObtenerCriterios" style="width:200px; height:34px; background-color:#ff0f64; border-color:#ff0f64" id="A1" title="Descargar criterios de evaluación">Descargar criterios de evaluación</a>
                                      <asp:Button ID="btnImprimirCatalogo" runat="server" class="btn btn-primary run-first" style="height:34px" Text="Catálogo Asp.Ambientales" />
                                      <a class="btn btn-primary run-first" target="_blank" onclick="window.open(this.href, this.target, 'width=650,height=650'); return false;" href="http://novotecsevilla.westeurope.cloudapp.azure.com/evr/AspectosAmbientales/ayuda"><i class="icon-info"></i> <asp:Literal ID="Literal1" runat="server" Text="Mapa de aspectos" /></a>
                                      <a class="btn btn-primary run-first" href="/evr/aspectosambientales/parametros"><i class="icon-cog"></i> <asp:Literal ID="Literal4" runat="server" Text="Parámetros de evaluación" /></a>
                                    </div>
        </td>
        </tr>
    </table>
    
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
     
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Aspectos</h6>
                        </div>

                            <asp:GridView ID="grdParametros" runat="server" Visible="false">
                            </asp:GridView>
                            <asp:GridView ID="grdFocos" runat="server" Visible="false">
                            </asp:GridView>

                        <div runat="server" class="panel-body">
                        <center>
                        <br />
                        <table width="50%">
                                        <tr>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN1" style="width:230px; margin-right:10px" type="button" value="Evaluaciones por parámetro" class="btn btn-primary run-first" /></center>
                                            </td>             
                          
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN3" style="width:230px" type="button" value="Evaluaciones por foco" class="btn btn-primary run-first"/></center>
                                            </td>

                                        </tr>
                        </table><br />
                        </center>
                        
                        <div runat="server" id="divN1">
                        <table width="50%">
                            <tr>
                                <td style="padding-right:10px">
                                    <div runat="server" class="form-group">
                                    <label>
                                        Aspectos a planificar:</label>
                                    <asp:DropDownList ID="ddlAspectosParametro" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>                        
                                </td>
                                <td style="padding-top:10px">
                                <input id="btnAddAspectoParametro" type="submit" runat="server" value="Asignar Aspecto"
                                        class="btn btn-primary run-first" />
                                </td>
                            </tr>
                        </table>

                        <div style="width:100%" class="datatablePedido">
									<center>
                                    <table class="table table-bordered">
					                    <thead>
					                        <tr> 
                                                <th>Código</th>   
                                                <th>Categoría</th>   
                                                <th>Grupo</th>   
                                                <th>Identificación</th>   
                                                <th>Unidad</th>                                                 
                                                <th width="45px">Significancia</th>
                                                <th width="45px">Queja/Denuncia</th>
					                            <% if (permisos.permiso == true)
                                                    { %>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
                                                <%} %>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in grdParametros.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[21].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[19].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>   
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>     
                                                <% 
                                                    if (item.Cells[16].Text == "1")
                                                   { %> 
                                                <td style="background-color:Red; color:White" class="task-desc">
                                                    Significativo
                                                        
                                                </td>     
                                                <% }
                                                    else if (item.Cells[16].Text == "0")
                                                   { %> 
                                                <td style="background-color:Green; color:White" class="task-desc">
                                                    No Significativo
                                                        
                                                </td>     
                                                <% }
                                                   else
                                                   { %>      
                                                        <td class="task-desc">
                                                    
                                                </td>   
                                                   <% } %>
                                                <% 
                                                    if (item.Cells[23].Text == "1")
                                                   { %> 
                                                <td style="background-color:Red; color:White" class="task-desc">
                                                    Si
                                                        
                                                </td>     
                                                <% }                                                   
                                                   else
                                                   { %>      
                                                        <td class="task-desc">
                                                    No
                                                </td>   
                                                   <% } %>                               
                                                <% if (permisos.permiso == true)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/aspectosambientales/detalle_aspecto/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/aspectosambientales/eliminar_aspecto/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                </td>
                                                <% 
                                                   } %>
                                            </tr>
                                            <% }%>
					                            
					                    </tbody>
					                </table>
							        </center>
                                    </div>
                                </div>

                                <div runat="server" id="divN3">
                        <table width="100%">
                            <tr>
                                <td style="padding-right:10px; width:40%">

                                    <label>
                                        Aspectos a planificar:</label>
                                    <asp:DropDownList ID="ddlAspectosFoco" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>                
                                </td>
                                <td style="width:20%">
                                    <label>
                                        Nombre foco</label>
                                    <asp:TextBox ID="txtNombreFoco" runat="server" Width="95%" class="form-control"></asp:TextBox>
                                </td>
                                <td style="width:13%">
                                    <label>
                            Medición en continuo</label>
                                <asp:DropDownList runat="server" ID="ddlContinuo" class="form-control"
                                    Width="90%">
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                                <td style="padding-top:10px">
                                <input id="btnAddAspectoFoco" type="submit" runat="server" value="Asignar Aspecto"
                                        class="btn btn-primary run-first" />
                                </td>
                            </tr>
                        </table>

                        <div style="width:100%" class="datatablePedido">
									<center>
                                    <table class="table table-bordered">
					                    <thead>
					                        <tr> 
                                                <th>Código</th>   
                                                <th>Categoría</th>   
                                                <th>Grupo</th>   
                                                <th>Identificación</th>   
                                                <th>Unidad</th>                                                 
                                                <th width="45px">Significancia</th>
                                                <th width="45px">Queja/Denuncia</th>
					                            <% if (permisos.permiso == true)
                                                    { %>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Eliminar</th>
                                                <%} %>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in grdFocos.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[21].Text %>
                                                </td>  
                                                <td class="task-desc">
                                                    <%= item.Cells[19].Text %>
                                                </td>   
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>    
                                                <td class="task-desc">
                                                    <%= item.Cells[22].Text %>
                                                </td>
                                                <td class="task-desc">
                                                    <%= item.Cells[3].Text %>
                                                </td>     
                                                <% 
                                                    if (item.Cells[16].Text == "1")
                                                   { %> 
                                                <td style="background-color:Red; color:White" class="task-desc">
                                                    Significativo
                                                        
                                                </td>     
                                                <% }
                                                    else if (item.Cells[16].Text == "0")
                                                   { %> 
                                                <td style="background-color:Green; color:White" class="task-desc">
                                                    No Significativo
                                                        
                                                </td>     
                                                <% }
                                                   else
                                                   { %>      
                                                        <td class="task-desc">
                                                    
                                                </td>   
                                                   <% } %>
                                                <% 
                                                    if (item.Cells[23].Text == "1")
                                                   { %> 
                                                <td style="background-color:Red; color:White" class="task-desc">
                                                    Si
                                                        
                                                </td>     
                                                <% }                                                   
                                                   else
                                                   { %>      
                                                        <td class="task-desc">
                                                    No
                                                </td>   
                                                   <% } %>                                       
                                                <% if (permisos.permiso == true)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/aspectosambientales/detalle_aspecto/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/aspectosambientales/eliminar_aspecto/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                </td>
                                                <% 
                                                   } %>
                                            </tr>
                                            <% }%>
					                            
					                    </tbody>
					                </table>
							        </center>
                                    </div>
                                </div>

                                 </div>      
                                 <br />                                                                                                
                                 </div>

                                 <% if (permisos.permiso == true)
                                       { %>
                                       <div style="text-align:right">
                                        <table style="float:right;">
                                            <tr>
                                    <td style="padding-right:5px; padding-top:5px ; width:50px">
                                                  <label>Año:</label>
                                                </td>
                                                <td style="padding-right:5px; width:80px">
                                                     <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                                                     </asp:DropDownList>
                                                </td>
                                        <td  style="padding-right:5px">
                                    <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
                                            </td>
                                                </tr>
                                            </table>
                                    </div>
                                    <% } %>    
    </form>
</asp:Content>
