<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.pais oPais = (MIDAS.Models.pais)ViewData["EditarPais"];

            if (oPais != null)
            {
                hdnIdPais.Value = oPais.id_pais.ToString();
                txtCodPais.Text = oPais.id_pais.ToString(); 
                txtDescripcion.Text = oPais.nombre_pais;

                ViewData["idPais"] = oPais.id_pais;                  


            }

            if (Session["EdicionPaisMensaje"] != null)
            {
                MIDAS.Models.pais paisErroneo = (MIDAS.Models.pais)Session["PaisErroneo"];
                hdnIdPais.Value = paisErroneo.id_pais.ToString();
                txtCodPais.Text = paisErroneo.id_pais.ToString();
                txtDescripcion.Text = paisErroneo.nombre_pais;
                Session.Remove("PaisErroneo");

                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionPaisMensaje"].ToString() + "' });", true);
                Session["EdicionPaisMensaje"] = null;
            }
        }
               

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Paises </title>
	
     <script type="text/javascript">


    $(document).ready(function() {
    
             $("form").submit(function() { 
                    var val = $("input[type=submit][clicked=true]").attr("id")
                    
                    if(val=="GuardarSector")
                        $("#hdFormularioEjecutado").val("GuardarSector");
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
					<h3>País <small>Edición del país</small></h3>
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
                                                <label>NOMBRE:</label>
													<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
												
											</div>
											<div style="display:none" class="form-group">
												<label>COD. PAÍS:</label>
												<asp:TextBox ID="txtCodPais" Enabled="false" runat="server" class="form-control" ></asp:TextBox>
											</div>

                                            </div>

										
									</div>

                                    								

								</div>


							</div>

			<!-- /modal with table -->
							

                            <div class="form-actions text-right">
                                        <input id="GuardarPais" type="submit" value="Guardar datos" class="btn btn-primary run-first">                                               
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
