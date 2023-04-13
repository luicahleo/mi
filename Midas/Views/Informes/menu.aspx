<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    protected void Page_Load(object sender, EventArgs e)
    {

        for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }

        if (Session["ImpresionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["ImpresionError"].ToString() + "' });", true);
            Session["ImpresionError"] = null;
        }  
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Gestión de informes</h3>
				</div>
			</div>
			<!-- /page header -->
            <br />
            <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
            <center>
                <table style="width:100px">
                    <tr>
                        <td>
                            <div class="form-group">
                            <center>
                                <label>Año </label></center>
                                <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                                </asp:DropDownList>

                            </div>
                        </td>
                    </tr>
                </table>
                </center>
              
            <br />
            <asp:HiddenField ID="hdnInformeSeleccionado" runat="server" />
                <center>
            <table width="100%">
                <tr>
                    <td onclick="$('#ctl00_MainContent_hdnInformeSeleccionado').val('PlanificacionPreventiva');" style="width:50%; text-align:center; padding: 10px">

                        <button style="width:250px; height:100px; border-radius:10px" class="btn btn-primary">      
                            <table width="100%">
                                <tr>
                                    <td>
                                        <i style="font-size:45px" class="icon-file-excel"></i>
                                    </td>
                                    <td>
                                        <span style="font-size:13px; vertical-align:middle; margin-top:15px">Planificación Preventiva</span>
                                    </td>
                                </tr>
                            </table>         
                             
                            
                        </button>

                        

                    </td>
                </tr>

            </table></center>
            </form>
			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->


</asp:Content>
