<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.matriz_centro matrizCentro = new MIDAS.Models.matriz_centro();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosVersiones.DataSource = ViewData["versiones"];
            DatosVersiones.DataBind();

            //List<MIDAS.Models.version_matriz> listaVerFin = (List<MIDAS.Models.version_matriz>)ViewData["versiones"];
            //var ultimoListaVerFin = listaVerFin.Last().id;
        }

        if (Session["EliminarSistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EliminarSistema"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }

        if (Session["ExisteVersionMatriz"] != null)
        {
            //ClientScript.RegisterStartupScript(GetType(), "miScript", "prueba();", true);
        }
        else
        {


            //ClientScript.RegisterStartupScript(GetType(), "miScript2", "prueba2();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "miScript2", "prueba2();", true);

        }

    }
</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('../Content/images/pageLoader.gif') 50% 50% no-repeat rgb(249,249,249);
            opacity: .6;
        }
    </style>
    <title>DIMAS</title>

</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="loader"></div>
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Matrices de Riesgos Inherentes <small>Conjunto de matrices</small></h3>
        </div>
    </div>
    <!-- /page header -->
    <form id="form1" runat="server">
        <% //ClientScript.RegisterStartupScript(GetType(), "miScript2", "prueba2();", true);%>

        <asp:GridView ID="DatosVersiones" runat="server" Visible="false">
        </asp:GridView>
    </form>
    <!-- Tasks table -->

    <div class="block">
        <center>
            <div style="width: 95%" class="datatablePedido">
                <table class="table table-bordered" style="font-size: 16px;">
                    <thead>
                        <tr>
                            <th>Version</th>
                            <th>Estado</th>
                            <th>Fecha de última modificación</th>
                            <th>Usuario</th>
                            <%--<th >Fecha última modificación</th>--%>


                            <th style="width: 45px">Editar</th>
                            <th style="width: 45px">Borrar</th>

                        </tr>
                    </thead>
                    <tbody>
                        <% 

                            List<MIDAS.Models.version_matriz> listaVerFin = new List<MIDAS.Models.version_matriz>();
                            listaVerFin = (List<MIDAS.Models.version_matriz>)ViewData["versiones"];
                            var ultimoversionmatrizDefinitiva = 0;
                            bool Existeborrador = false;
                            if (listaVerFin != null && listaVerFin.Count > 0)
                            {
                                if (listaVerFin.Where(x => x.estado == 1).Select(x => x.id).ToList().Count!=0) {
                                    Existeborrador = true;
                                }
                                ultimoversionmatrizDefinitiva = listaVerFin.Where(x => x.estado == 0).Select(x => x.id).LastOrDefault();
                            }

                            foreach (GridViewRow item in DatosVersiones.Rows)
                            {

                        %>
                        <tr>

                            <td class="text-center">
                                <%= item.Cells[2].Text %>
                            </td>
                            <td class="text-center">
                                <%= (item.Cells[5].Text == "0")?"FINALIZADA":"BORRADOR"
                                %>
                            </td>
                            <td class="text-center">
                                <%= item.Cells[4].Text %>
                            </td>
                            <td class="task-desc">
                                <%= item.Cells[6].Text %>
                            </td>


                            <%--   <%= item.Cells[2].Text %>--%>
                            <td class="text-center">
                                
                                <%
                                    matrizCentro = MIDAS.Models.Datos.ObtenerMatrizCentro(int.Parse(item.Cells[1].Text), int.Parse(item.Cells[0].Text));
                                    if (item.Cells[5].Text == "1")
                                    {
                                        if (matrizCentro != null)
                                        {%>
                                <a href="/evr/Riesgos/CrearMatrizDesde/<%=item.Cells[0].Text%>" title="Crear Matriz" onclick="CrearMatriz();"><i class="icon-pencil"></i></a>
                                <%}
                                    }
                                    else if (item.Cells[5].Text == "0" && !Existeborrador)
                                    {
                                        if (ultimoversionmatrizDefinitiva != null && ultimoversionmatrizDefinitiva != 0)
                                        {
                                            if (matrizCentro != null && item.Cells[0].Text == ultimoversionmatrizDefinitiva.ToString())
                                            {%>
                                            <a href="/evr/Riesgos/CrearMatrizDesde/<%=item.Cells[0].Text%>" title="Crear Matriz" onclick="CrearMatriz();"><i class="icon-pencil"></i></a>
                                        <%}
                                                }

                                    }%>
                            </td>

                            <td class="text-center">
                                <% if (item.Cells[5].Text == "1") {%>
                                    <a onclick="if(!confirm('¿Está seguro de que desea eliminar esta version?')) return false;" href="/evr/Riesgos/eliminar_version/<%=item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove Eliminar"></i></a>
                                <% }%>
                            </td>


                        </tr>
                        <% }%>
                    </tbody>
                </table>
            </div>
        </center>
    </div>
    <!-- /tasks table -->

    <div style="text-align: right">
        <%--     comentado para que los no administradores puedan crear--%>
        <%--                   <% if ((user.perfil == 1 || user.perfil == 3) &&DatosVersiones.Rows.Count<1)
                           { %>
                         <a  href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>
                        <% } %>--%>

        <%--alternativa--%>
        <% if (DatosVersiones.Rows.Count < 1)
            { %>
        <a href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>
        <% } %>
        <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    <p>
        <br />
    </p>
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->
    <script type="text/javascript" language="JavaScript">

        $(document).ready(function () {

            $("#MenuMatrizRiesgos").css('color', 'black');
            $("#MenuMatrizRiesgos").css('background-color', '#ebf1de');
            $("#MenuMatrizRiesgos").css('font-weight', 'bold');
            $('.loader').hide();


            $('.Eliminar').click(function () {

            });
        });

        function CrearMatriz() {
            $('.loader').show();
        }


        function prueba() {
            alert("hola lista matri");
        }
        function prueba2() {
            alert("hola lista matri   2");
        }


        //    if (array.length > 0) {

        //        $.ajax({
        //            url: "CrearMatrizDesdeAnterior",
        //            data: { id: idVersion },
        //            type: "post",
        //            success: function (datos) {
        //                if (datos != null) {

        //                }


        //            }
        //        });

        //    }
        //}
    </script>
</asp:Content>
