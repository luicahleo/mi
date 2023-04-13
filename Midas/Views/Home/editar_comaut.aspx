<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.comunidad_autonoma oCCAA = (MIDAS.Models.comunidad_autonoma)ViewData["EditarCCAA"];
                       

            if (ViewData["pais"] != null)
            {
                ddlPais.DataSource = ViewData["pais"];
                ddlPais.DataValueField = "id_pais";
                ddlPais.DataTextField = "nombre_pais";
                ddlPais.DataBind();

                ddlPais.SelectedIndex = 0;

                if (Session["ddlPais"] != null)
                {
                    ddlPais.SelectedValue = Session["ddlPais"].ToString();
                    Session.Remove("ddlPais");
                }
            }

            if (oCCAA != null)
            {
                hdnIdPais.Value = oCCAA.id_pais.ToString();
                txtCodPais.Text = oCCAA.id_pais.ToString();
                txtDescripcion.Text = oCCAA.nombre_comunidad;

                ViewData["idCCAA"] = oCCAA.id_pais;


            }
            
            if (Session["EdicionCCAAMensaje"] != null)
            {
                MIDAS.Models.comunidad_autonoma CCAAErroneo = (MIDAS.Models.comunidad_autonoma)Session["CCAAErroneo"];
                hdnIdPais.Value = CCAAErroneo.id_comunidad_autonoma.ToString();
                txtCodPais.Text = CCAAErroneo.id_comunidad_autonoma.ToString();
                txtDescripcion.Text = CCAAErroneo.nombre_comunidad;
                Session.Remove("CCAAErroneo");

                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionCCAAMensaje"].ToString() + "' });", true);
                Session["EdicionCCAAMensaje"] = null;
            }
        }
               

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - CCAA </title>
	
     <script type="text/javascript">


    $(document).ready(function() {
    
             $("form").submit(function() { 
                    var val = $("input[type=submit][clicked=true]").attr("id")
                    
                    if(val=="GuardarSector")
                        $("#hdFormularioEjecutado").val("GuardarCCAA");
            });
            
            $("form input[type=submit]").click(function() {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

           
        
    });

       
    </script>

</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>País <small>Edición de la comunidad autónoma</small></h3>
				</div>
			</div>
			<!-- /page header -->




			<!-- Form vertical (default) -->
						<form role="form" action="#" runat="server">
						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos del sector</h6></div>
								<div class="panel-body">
									<div class="row">
                                        <asp:HiddenField runat="server" ID="hdnIdPais" />
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>PAÍS:</label>
												<asp:TextBox Visible="false" ID="txtCodPais" Enabled="false" runat="server" class="form-control" ></asp:TextBox>
                                                <asp:DropDownList class="form-control" runat="server" ID="ddlPais">
                                                 </asp:DropDownList>
											</div>
											<div class="form-group">
												<label>NOMBRE:</label>
													<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
											</div>

                                            </div>

										
									</div>

                                    								

								</div>


							</div>

			<!-- /modal with table -->
							

                            <div class="form-actions text-right">
                                        <input id="GuardarCCAA" type="submit" value="Guardar datos" class="btn btn-primary run-first">                                               
                                        <a href="/evr/Home/Territorios" title="Volver" class="btn btn-primary run-first">Volver</a>
									</div>
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
