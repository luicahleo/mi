<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["personas"];
            DatosPedidos.DataBind();
        }
      
        if (TempData["Notification"] != null)
        {
            var tempData = TempData["Notification"].ToString();
            var lista = tempData.Split(',');

            string js = string.Format("MostrarMensaje('{0}','{1}');", lista[0], lista[1]);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", js, true);
        }
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/personas.js") %>"></script>


    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Lista de Personas <small>Lista general de personas</small></h3>
        </div>
    </div>

    <%--Modal para cargar hoja excel personas--%>

    <div class="container py-4">

        <div class="modal fade" id="mi-modal" data-backdrop="static">
            <div class="modal-dialog" style="overflow-y: scroll; max-height: 85%; margin-top: 50px; margin-bottom: 50px;">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Cargar excel</h5>
                        <button class="btn btn-close" data-dismiss="modal">X</button>
                    </div>

                    <div class="modal-body">

                        <form role="form" action="#" runat="server" method="post" enctype="multipart/form-data">

                            <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>


                            <div class="panel-body">
                                <table style="width: 100%">

                                    <tr>
                                        <td style="padding-right: 15px">
                                            <br />
                                            <asp:Panel runat="server" CssClass="form-group">

                                                <input type='file'
                                                    id="fileExcel"
                                                    name="fileExcel" />
                                                <br />
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                </table>
                            </div>

                            <div class="form-actions text-right">
                                <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit" runat="server">
                                <%--<a href="/evr/configuracion/" title="Volver" class="btn btn-primary run-first">Volver</a>--%>
                            </div>
                        </form>


                    </div>

                    <div class="modal-footer">
                        <%--<button class="btn btn-danger" data-dismiss="modal">Cancelar</button>--%>
                        <%--<button class="btn btn-primary" onclick="enviarDatosExcel()">Cargar</button>--%>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Tasks table -->
    <div class="block">
        <center>
            <div style="width: 95%" class="datatablePedido">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="width: 5%">Nombre y Apellidos</th>
                            <th style="width: 5%">Perfil de Riesgo</th>
                            <th style="width: 10%">Empresa</th>
                            <th style="width: 5%">Centro de Trabajo</th>
                            <th style="width: 10%">Actividad</th>
                            <th style="width: 10%">Unidad Organizativa</th>
                            <th style="width: 10%">Ocupación</th>
                            <th style="width: 10%">Posición</th>

                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <th style="width: 45px">Editar</th>
                            <th style="width: 45px">Borrar</th>
                            <% } %>
                        </tr>
                    </thead>
                    <tbody>

                        <%foreach (GridViewRow item in DatosPedidos.Rows)
                            { %>
                        <tr>
                            <td class="task-desc">
                                <%=item.Cells[5].Text %>
                            </td>
                            <td class="task-desc">
                                <%=item.Cells[2].Text %>
                            </td>

                            <td class="task-desc">
                                <%=item.Cells[6].Text %>
                            </td>
                            <td class="task-desc">
                                <%=item.Cells[7].Text %>
                            </td>
                            <td class="task-desc">
                                <%=item.Cells[8].Text %>
                            </td>
                            <td class="task-desc">
                                <%=item.Cells[9].Text %>
                            </td>
                            <td class="task-desc">
                                <%=item.Cells[12].Text %>
                            </td>
                            <td class="task-desc">
                               <%=item.Cells[11].Text %>
                            </td>

                            <% if (user.perfil == 1 || user.perfil == 3)
                                { %>
                            <td class="text-center" style="width: 5%">
                                <a href="/evr/Configuracion/editar_persona/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                            </td>
                            <td class="text-center" style="width: 5%">
                                <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta medida?')) return false;" href="/evr/Configuracion/eliminar_persona/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                            </td>
                            <% } %>
                        </tr>
                        <% }%>
                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->

    <div style="text-align: right">
        <% if (user.perfil == 1 || user.perfil == 3)
            { %>
        <button class="btn btn-primary" onclick="mostrarModalCargarExcel()">Cargar hoja excel</button>.

        <% } %>
        <%-- <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>--%>
        <a href="/evr/Configuracion/menu" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    <p>
        <br />
    </p>




    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->


</asp:Content>

