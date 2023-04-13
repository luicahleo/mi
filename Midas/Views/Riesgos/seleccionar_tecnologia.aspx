<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        Tecnologias.DataSource = ViewData["tecnologias"];
        Tecnologias.DataBind();
        TecnologiasCentro.DataSource = ViewData["tecnologias_centro"];
        TecnologiasCentro.DataBind();

        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            List<string> tecnologiasMarcar = new List<string>();
            foreach (GridViewRow item in TecnologiasCentro.Rows)
            {
                tecnologiasMarcar.Add(item.Cells[0].Text);
            }

        }

        if (Session["TiposRiesgosResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["TiposRiesgosResultado"].ToString() + "' });", true);
            Session["TiposRiesgosResultado"] = null;
        }
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS - EVR</title>

</asp:Content>


<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Riesgos/seleccionar_tecnologia.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Riesgos/seleccionar_tecnologia.css") %>" rel="stylesheet" />

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Seleccione Tecnología<small>Seleccionar maestro de matrices por tecnología para iniciar la matriz de riesgos</small></h3>
        </div>
        <div class="loader"></div>
    </div>
    <!-- /page header -->
    <% using (Html.BeginForm("seleccionar_tecnologia", "Riesgos"))
                                         {%>
    <%--  <form id="sss"  runat="server" action="seleccionar_tecnologia" method="post">--%>
    <asp:GridView ID="Tecnologias" runat="server" Visible="false">
    </asp:GridView>
    <asp:GridView ID="TecnologiasCentro" runat="server" Visible="false">
    </asp:GridView>

    <div>
    </div>

    <!-- Tasks table -->
    <div class="block">
        <center>
            <div class="block">
                <center>
                    <div style="width: 95%" class="datatablePedido">
                        <table id="TABLA_TECNOLOGIAS" class="table table-bordered">
                            <thead>
                                <tr>
                                    <th style="width: 20%">Seleccionar</th>
                                    <th>Tecnología</th>

                                </tr>
                            </thead>
                            <tbody>

                                <% 
                                     List<string> tecnologiasMarcar = new List<string>();
                                     foreach (GridViewRow item in TecnologiasCentro.Rows)
                                     {
                                         tecnologiasMarcar.Add(item.Cells[0].Text);
                                     }


                                     foreach (GridViewRow item in Tecnologias.Rows)
                                     {

                                %>
                                <tr>

                                    <td class="task-desc" style="text-align: center;">
                                        <%if (tecnologiasMarcar.Contains(item.Cells[0].Text))
                     {

                                        %>

                                        <input type="checkbox" id="<%= item.Cells[0].Text %>" style="transform: scale(2);" checked name="checkbox_<%= item.Cells[0].Text %>" />

                                        <%}
        else
        {

                                        %>

                                        <input type="checkbox" id="<%= item.Cells[0].Text %>" style="transform: scale(2);" name="checkbox_<%= item.Cells[0].Text %>" />
                                        <% }%>
                                    </td>
                                    <td class="task-desc" id="<%= item.Cells[1].Text %>">

                                        <img id="IMG_<%=item.Cells[1].Text%>" style="width: 40px; height: 40px;" src="<%=item.Cells[2].Text%>" enctype="multipart/form-data">
                                        <label style="font-size: 18px"><%= item.Cells[1].Text %></label>

                                    </td>


                                </tr>
                                <% }%>
                            </tbody>
                        </table>
                    </div>
                </center>
            </div>
        </center>
    </div>

    <!-- /tasks table -->


    <p>
        <br />
    </p>
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="form-actions text-right">
            <input type="submit" value="Crear Matriz" title="Crear Matriz" onclick="CrearMatriz()" class="btn btn-primary run-first" />
            <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->
    <%} %>
    <%--   </form>--%>
</asp:Content>
