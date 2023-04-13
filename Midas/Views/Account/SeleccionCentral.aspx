<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Logeo.Master" Inherits="System.Web.Mvc.ViewPage" %>


<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

        if (ViewData["tecnologias"] != null)
        {
            ddlTecnologias.DataSource = ViewData["tecnologias"];
            ddlTecnologias.DataValueField = "id";
            ddlTecnologias.DataTextField = "nombre";
            ddlTecnologias.DataBind();

        }
        Session["DescripcionCentro"] = null;
        Session["VersionMatriz"] = null;

        //if (ViewData["centros"] != null)
        //{
        //    ddlCentros.DataSource = ViewData["centros"];
        //    ddlCentros.DataValueField = "id";
        //    ddlCentros.DataTextField = "nombre";
        //    ddlCentros.DataBind();
        //}
        //imgCentral.ImageUrl = "~/Imagenes/Central/" + ddlCentros.SelectedValue + ".jpg";
    }
    
</script>
<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">


    <title>DIMAS - EVR</title>

</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm())
        { %>


    <style>
        /*  @import "bourbon";*/

        *, *:after, *:before {
            box-sizing: border-box;
        }

        body {
            @include display(flex);
            @include align-content(center);
            color: #353535;
            min-height: 100vh;
            font-family: 'Open Sans', sans-serif;
            font-size: 14px;
            text-align: center;
            background-color: lightgreen;
        }

        .container {
            padding: 60px 80px;
            background-color: white;
            box-shadow: 0 0 4px 1px #BBB;
            margin: auto;
            text-align: center;
        }

        .wrap {
            position: relative;
            width: 80px;
            height: 80px;
            margin: 20px auto 30px auto;
        }

            .wrap .last-child {
                margin-bottom: 0;
            }


        .clicker {
            background-color: white;
            outline: none;
            font-weight: 600;
            position: absolute;
            cursor: pointer;
            padding: 0;
            border: none;
            height: 64px;
            width: 64px;
            left: 8px;
            top: 8px;
            border-radius: 100px;
            z-index: 2;
        }

            .clicker:active {
                transform: translate(0, 1px);
                height: 63px;
                box-shadow: 0px 1px 0 0 rgb(190,190,190) inset;
            }

        .circle {
            position: relative;
            border-radius: 40px;
            width: 80px;
            height: 80px;
            z-index: 1;
        }

            .circle.third {
                border-radius: 0;
            }

        .clicker.faster:focus + .circle, .clicker.faster:active + .circle {
            animation: rotator linear .4s infinite;
        }

        .clicker.fast:focus + .circle, .clicker.fast:active + .circle {
            animation: rotator linear .5s infinite;
        }

        .clicker:focus + .circle, .clicker:active + .circle {
            animation: rotator linear .8s infinite;
        }

        @keyframes rotator {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }


        .angled {
            background-image: linear-gradient(45deg, white 0%, white 30%, #0555FA 30%, #0555FA 70%, white 70%, white 100%);
        }

            .angled.second {
                background-image: linear-gradient( white 0%, white 30%, rgb(250,160,120) 30%, rgb(250,160,120) 70%, white 70%, white 100%);
            }

            .angled.third {
                background-image: linear-gradient(45deg, white 0%, white 30%, rgb(130,230,135) 30%, rgb(130,230,135) 70%, white 70%, white 100%);
            }
    </style>

    <center>
        <br />
        <br />

        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                        <div class="topcornerleft">
                            <asp:Image ID="Image1" Height="60px" ImageUrl="~/Content/images/logo_endesa.png" runat="server" />
                        </div>
                    </center>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <br />
                        <h3>DIMAS - EVR</h3>
                    </center>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <!-- Login wrapper -->
                        <form runat="server">
                            <div class="topcorner-seleccion">
                                <dl>
                                    <dt style="background-color: #0555FA; height: 30px; border-top-right-radius: 25px; border-top-left-radius: 25px" class="popup-header">

                                        <p>
                                            <a href="#Section1">
                                                <asp:Literal ID="Literal2" runat="server" Text="Selección de centro" /></a>
                                        </p>

                                    </dt>
                                    <dd id="Section1">
                                        <div class="well">
                                            <div style="margin-top: 15px" class="form-group has-feedback">
                                                <asp:DropDownList ID="ddlTecnologias" CssClass="form-control" runat="server" ClientIDMode="Static" onchange="seleccionTecnologia()">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlZonas" CssClass="form-control" runat="server" ClientIDMode="Static" onchange="seleccionZona()">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlAgrupacion" CssClass="form-control" runat="server" ClientIDMode="Static" onchange="seleccionAgrupacion()">
                                                </asp:DropDownList>
                                  <%--              <asp:DropDownList ID="ddlTipoCentral" CssClass="form-control" runat="server" ClientIDMode="Static" onchange="ObtenerListaCentros()">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="ddlCentros" CssClass="form-control" ClientIDMode="Static" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <center>
                                                <!--
                    <div style="color:Black" id="capsWarning" <%--style="display:none;"--%>>Los datos de identificación son sensibles a mayúsculas y minúsculas.</div>-->
                                            </center>
                                            <br />
                                            <div class="row form-actions">
                                                <div class="wrap">
                                                    <button name="submit" value="Acceder" id="ingresar" type="submit" class="clicker">Acceder</button>
                                                    <div class="circle angled"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </dd>
                                </dl>
                            </div>
                        </form>
                </td>

                <td>
                    <asp:Image Style="width: 100%; margin-top: 25px; height: 400px; margin-left: -70px; opacity: 0.7; border-radius: 15px; border: 5px solid #0555FA" ID="imgCentral" ImageUrl="~/Imagenes/Central/1.jpg" runat="server" />
                </td>
            </tr>
        </table>
    </center>



    <% } %>
    <script>

    


        function seleccionTecnologia() {
          
            var idTipo = $("#ddlTecnologias").val();
            if (idTipo == "7" || idTipo == "8" || idTipo == "9" ) {

                ObtenerZonas();
                $("#ddlCentros").hide();
                $("#ddlZonas").show();
                $("#ddlAgrupacion").hide();
            } else {
                ObtenerCentrosPorTecnologias();
                $("#ddlCentros").show();
                $("#ddlZonas").hide();
                $("#ddlAgrupacion").hide();
            }

        }

        function seleccionZona() {
            var idTipo = $("#ddlTecnologias").val();
            if (idTipo == "7") {
                ObtenerAgrupacion();        
                $("#ddlAgrupacion").show();
            } else {
                ObtenerCentrosPorZonas();
                $("#ddlAgrupacion").hide();
                $("#ddlCentros").show();
            }
        }
        function seleccionAgrupacion() {       
            ObtenerCentrosPorAgrupacion();
            $("#ddlCentros").show();
        }


        function ObtenerZonas(){
            
                var idTipo = $("#ddlTecnologias").val();

                $.ajax({
                    url: "ObtenerZonas", //Your path should be here
                    data: { idTipo: idTipo },
                    type: "post",
                    success: function (datos) {
                        if (datos != null) {
                            $("#ddlZonas").find('option').remove();
                            $.each(datos, function (i, datos) {
                                $("#ddlZonas").append('<option value="' + datos.Value + '">' +
                                    datos.Text + '</option>');
                            });
                        } else {
                            $("#ddlZonas").find('option').remove();
                        }

                    }
                });
            
        }

        function ObtenerCentrosPorZonas() {

            var idTipo = $("#ddlTecnologias").val();
            var idZona = $("#ddlZonas").val();
            $.ajax({
                url: "ObtenerCentrosZonas", //Your path should be here
                data: { idTipo: idTipo, idZona:idZona },
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        $("#ddlCentros").find('option').remove();
                        $.each(datos, function (i, datos) {
                            $("#ddlCentros").append('<option value="' + datos.Value + '">' +
                                datos.Text + '</option>');
                        });
                    } else {
                        $("#ddlCentros").find('option').remove();
                    }

                }
            });
        }
        function ObtenerAgrupacion() {
            var idTipo = $("#ddlTecnologias").val();
            var idZona = $("#ddlZonas").val();
            $.ajax({
                url: "ObtenerAgrupacion", //Your path should be here
                data: { idTipo: idTipo,idZona: idZona  },
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        $("#ddlAgrupacion").find('option').remove();
                        $.each(datos, function (i, datos) {
                            $("#ddlAgrupacion").append('<option value="' + datos.Value + '">' +
                                datos.Text + '</option>');
                        });
                    } else {
                        $("#ddlAgrupacion").find('option').remove();
                    }

                }
            });

        }

        
        function ObtenerCentrosPorAgrupacion() {
            var idTipo = $("#ddlTecnologias").val();
            var idAgrupacion = $("#ddlAgrupacion").val();

            $.ajax({
                url: "ObtenerCentrosAgrupacion", //Your path should be here
                data: { idTipo: idTipo, idAgrupacion: idAgrupacion},
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        $("#ddlCentros").find('option').remove();
                        $.each(datos, function (i, datos) {
                            $("#ddlCentros").append('<option value="' + datos.Value + '">' +
                                datos.Text + '</option>');
                        });
                    } else {
                        $("#ddlCentros").find('option').remove();
                    }

                }
            });
        }

        function ObtenerCentrosPorTecnologias() {
            var idTipo = $("#ddlTecnologias").val();

            $.ajax({
                url: "ObtenerCentrosPorTecnologias", //Your path should be here
                data: { idTipo: idTipo },
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        $("#ddlCentros").find('option').remove();
                        $("#ddlCentros").append('<option value="0">Seleccione Centro</option>');
                        $.each(datos, function (i, datos) {
                            $("#ddlCentros").append('<option value="' + datos.Value + '">' +
                                datos.Text + '</option>');
                        });
                    } else {
                        $("#ddlCentros").find('option').remove();
                    }

                }
            });
        }



        //function ObtenerListaAgrupacion() {
        //    var idTipo = $("#ddlTecnologias").val();

        //    $.ajax({
        //        url: "ObtenerCentrosPorTecnologias", //Your path should be here
        //        data: { idTipo: idTipo },
        //        type: "post",
        //        success: function (datos) {
        //            if (datos != null) {
        //                $("#ddlTipoCentral").find('option').remove();
        //                $.each(datos, function (i, datos) {
        //                    $("#ddlTipoCentral").append('<option value="' + datos.Value + '">' +
        //                        datos.Text + '</option>');
        //                });
        //            } else {
        //                $("#ddlTipoCentral").find('option').remove();
        //            }

        //        }
        //    });
        //}


        //function ObtenerListaCentros() {
        //    var idTipo = $("#ddlTipoCentral").val();

        //    $.ajax({
        //        url: "ObtenerCentros", //Your path should be here
        //        data: { idTipo: idTipo },
        //        type: "post",
        //        success: function (datos) {
        //            if (datos != null) {
        //                $("#ddlCentros").find('option').remove();
        //                $.each(datos, function (i, datos) {
        //                    $("#ddlCentros").append('<option value="' + datos.Value + '">' +
        //                        datos.Text + '</option>');
        //                });
        //            } else {
        //                $("#ddlCentros").find('option').remove();
        //            }

        //        }
        //    });
        //}

    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#ddlCentros").hide();
            $("#ddlZonas").hide();
            $("#ddlAgrupacion").hide();
            seleccionTecnologia();
        });

        $(window).on("load", function () {
       
            seleccionTecnologia();
           
        });
    </script>

</asp:Content>




