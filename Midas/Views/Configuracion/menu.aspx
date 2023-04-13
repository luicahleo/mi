<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Configuración - Maestros</h3>
				</div>
			</div>
			<!-- /page header -->
            <br />
            <center>
            <table width="100%">
                <tr>
                    <td style="width:20%">
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/centros">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-factory"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label5" runat="server" Text="Instalaciones" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/Usuarios">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-user"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label1" runat="server" Text="Usuarios" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                     <td style="width:20%" >
                         <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;" href="/evr/Configuracion/tipos_riesgos">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-shield"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label2" runat="server" Text="Tipos de Riesgo" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    
                    </td>
                     <td style="width:20%" >
                         <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/editar_informacion_general">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-book2"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label3" runat="server" Text="Introducción Riesgos Inherentes" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>                    
                    </td>
                     <td style="width:20%" >
                         <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/medidas_generales">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-upload"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label6" runat="server" Text="Medidas Preventivas Generales" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    
                    </td>
                     <td style="width:20%" hidden >
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/carga_maestros">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="progress"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label4" runat="server" Text="Cargar Maestro" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>


<%--                     <td style="width:20%" >
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Configuracion/editar_centro">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-factory"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label3" runat="server" Text="Usuarios" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>--%>
                    <%--<td style="width:20%" >
                     <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%" href="/evr/Procesos/gestion_procesos">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-book"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label3" runat="server" Text="Procesos" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;" href="/evr/Configuracion/stakeholders">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-globe"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label6" runat="server" Text="Partes" /></span>
                                            <br />
                                            <span style="font-size:13px"><asp:Label ID="label7" runat="server" Text="interesadas" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;" href="/evr/Configuracion/parametros">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-file-excel"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label26" runat="server" Text="Hoja Datos" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/indicadores">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-rulers"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label10" runat="server" Text="Indicadores" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/tiposaccmejora">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-stats-up"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label9" runat="server" Text="Acciones Mejora" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/ambitos">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-search3"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label8" runat="server" Text="Ámbitos" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/referenciales">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-bookmark3"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label4" runat="server" Text="Referenciales" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/auditores">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-users2"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label13" runat="server" Text="Auditores" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>--%>
                </tr>
                <tr>
                      <td style="width:20%" >
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;margin-top:20px""  href="/evr/Configuracion/riesgos_medidas">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-upload"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label8" runat="server" Text="Medidas Preventivas por Riesgo" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%; margin-top:20px" href="/evr/Configuracion/medidas_preventivas">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-upload"></i>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label9" runat="server" Text="Medidas Preventivas por" /></span><br />
                                            <span style="font-size:13px"><asp:Label ID="label7" runat="server" Text="Situaciones de Riesgo" /></span>
                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>
                    <td style="width:20%" >
                    </td>
                    <td style="width:20%" >
                    </td>
                    <td style="width:20%" >
                    </td>
                    <td style="width:20%" >
                    </td>
                </tr>

                <tr>
                    <td style="width:20%" hidden>
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;margin-top:20px""  href="/evr/Configuracion/personas">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-file-excel"><i style="font-size:45px" class="icon-arrow3"></i></i><i style="font-size:45px" class="icon-database"></i>

                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label10" runat="server" Text="Cargar Maestro" /></span> <br />
                                            <span style="font-size:13px"><asp:Label ID="label11" runat="server" Text="Lista empleados" /></span>

                                        </center>
                                    </td>
                                </tr>
                            </table>                        
                        </a>
                        </center>
                    </td>



                </tr>

            </table></center>

			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->

<script>
    $(document).ready(function () {
        $("#MenuConfiguracion").css('color', 'black');
        $("#MenuConfiguracion").css('background-color', '#ebf1de');
        $("#MenuConfiguracion").css('font-weight', 'bold');
    });
</script>
</asp:Content>
