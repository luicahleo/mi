
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Menú Riesgos</h3>
				</div>
			</div>
			<!-- /page header -->
            <br />
            <center>
            <table width="100%">

                <tr  style="width:20%;padding:20px">
                 <td style="width:20%;" >
                    </td>
                    </tr>
                    <tr>
                    <td style="width:20%" >
                        
                    <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;margin: 20px;" href="/evr/Riesgos/matriz_riesgo/0" >
                            <table style="width:100%; height:90px;" >
                                <tr>
                                  <%--  <td>
                                        <center>
                                            <i style="font-size:45px" class="icon-user"></i>
                                        </center>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label1" runat="server" Text="CREAR MATRIZ DE RIESGOS – DESDE MATRICES TIPO" /></span>
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
                    <td style="width:20%">
                        <center>
                        <a class="btn btn-primary" style="border-radius:20px; width:85%;margin: 20px;" href="/evr/Riesgos/lista_matrices">
                            <table style="width:100%; height:90px">
                                <tr>
                                    <%--<td>
                                        <center>
                                            <i style="font-size:45px" class="icon-factory"></i>
                                        </center>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <span style="font-size:13px"><asp:Label ID="label5" runat="server" Text="MODIFICAR MATRIZ EXISTENTE" /></span>
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


</asp:Content>
