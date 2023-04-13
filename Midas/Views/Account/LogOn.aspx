<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Logeo.Master" Inherits="System.Web.Mvc.ViewPage" %>


<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session.Remove("usuario");
            Session.Remove("CentralElegida");
        }
    }

</script>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS - EVR</title>

</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm())
        { %>
    <script type="text/javascript">


        $(document).ready(function () {

            if ($("#errores").html() != '') {
                $("#Section1").css("overflow", "visible");
            }

        });

        function MostrarPassword() {

            document.getElementById('password').type = 'text';
            document.getElementById('ojover').style.display = 'block';
            document.getElementById('ojoocultar').style.display = 'none';
        }

        function OcultarPassword() {

            document.getElementById('password').type = 'password';
            document.getElementById('ojover').style.display = 'none';
            document.getElementById('ojoocultar').style.display = 'block';
        }

    </script>

    <style>
        @import "bourbon";

        *, *:after, *:before {
            box-sizing: border-box;
        }

        body {
            @include display(flex);
            @include align-content(center);
            color: #353535;
            min-height: 100vh;
            background-size: cover;
            background-image: url(Imagenes/main.jpg);
            background-op font-family: 'Open Sans', sans-serif;
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
            &:last-child

        {
            margin-bottom: 0;
        }

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
        <table width="50%">
            <tr>
                <td>
                    <center>
                        <div class="topcornerleft">
                            <br />
                            <br />
                            <asp:Image ID="Image1" Height="60px" ImageUrl="~/Content/images/logo_endesa.png" runat="server" />
                        </div>
                    </center>
                    <br />
                    <br />
                    <h4 class="text-center" style="color: white; font-weight: bold;">DIMAS - EVR</h4>
                    <h4 class="text-center" style="color: red; font-weight: bold;" >EVR PREPRODUCCIÓN</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <!-- Login wrapper -->
                        <br />
                        <div class="topcorner-seleccion">
                            <dl>
                                <dt style="background-color: #0555FA; height: 30px; border-top-right-radius: 25px; border-top-left-radius: 25px" class="popup-header">

                                    <p>
                                        <a href="#Section1">
                                            <asp:Literal ID="Literal2" runat="server" Text="Identificación" /></a>
                                    </p>


                                </dt>
                                <dd id="Section1">
                                    <div class="well">
                                        <div style="margin-top: 15px" class="form-group has-feedback">
                                            <label style="float: left">
                                                <asp:Literal runat="server" Text="Usuario" /></label>
                                            <%= Html.TextBox("username", null, new { @class = "form-control", @placeholder = "Indique usuario" })%>
                                            <i class="icon-users form-control-feedback"></i>
                                            <%= Html.ValidationMessage("username") %>
                                        </div>

                                        <div class="form-group has-feedback">
                                            <label style="float: left">
                                                <asp:Literal ID="Literal1" runat="server" Text="Password" /></asp:Literal></label>
                                            <%= Html.Password("password", null, new { @class = "form-control", @placeholder = "Indique contraseña" })%>
                                            <i class="icon-eye form-control-feedback" onclick="OcultarPassword()" id="ojover" style="display: none;"></i>
                                            <i class="icon-eye-blocked form-control-feedback" onclick="MostrarPassword()" id="ojoocultar" style="display: block;"></i>
                                            <%--<i class="icon-lock form-control-feedback"></i>--%>
                                            <%= Html.ValidationMessage("password")%>
                                            <div id="errores"><%= Html.ValidationMessage("_FORM")%></div>
                                        </div>
                                        <center>
                                            <!--
                    <div style="color:Black" id="capsWarning" <%--style="display:none;"--%>>Los datos de identificación son sensibles a mayúsculas y minúsculas.</div>-->
                                        </center>
                                        <br />
                                        <div class="row form-actions">
                                            <div class="wrap">
                                                <button id="ingresar" type="submit" class="clicker">Acceder</button>
                                                <div class="circle angled"></div>
                                            </div>
                                        </div>
                                    </div>
                                </dd>
                            </dl>
                        </div>
                </td>
            </tr>
        </table>
    </center>



    <% } %>
</asp:Content>
