<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.sector oSector = (MIDAS.Models.sector)ViewData["EditarSector"];

            if (oSector != null)
            {
                hdnIdSector.Value = oSector.id.ToString();
                txtSector.Text = oSector.sector1;
                txtDescripcion.Text = oSector.descripcion;

                ViewData["idSector"] = oSector.id;                  


            }

            if (Session["EdicionSectorMensaje"] != null)
            {
                MIDAS.Models.sector secErroneo = (MIDAS.Models.sector)Session["SectorErroneo"];
                hdnIdSector.Value = secErroneo.id.ToString() ;
                txtSector.Text = secErroneo.sector1;
                txtDescripcion.Text = secErroneo.descripcion;
                Session.Remove("SectorErroneo");             
                
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionSectorMensaje"].ToString() + "' });", true);
                Session["EdicionSectorMensaje"] = null;
            }
        }
        
      
        
        

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Sectores </title>
	
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
					<h3>Sector <small>Edición del sector</small></h3>
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
                                        <asp:HiddenField runat="server" ID="hdnIdSector" />
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>SECTOR:</label>
												<asp:TextBox ID="txtSector" runat="server" class="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
												<label>DESCRIPCIÓN:</label>
													<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
											</div>

                                            </div>

										
									</div>

                                    								

								</div>


							</div>

			<!-- /modal with table -->
							

                            <div class="form-actions text-right">
                                        <input id="GuardarSector" type="submit" value="Guardar datos" class="btn btn-primary run-first">                                               
                                        <a href="/evr/Home/Sectores" title="Volver" class="btn btn-primary run-first">Volver</a>
									</div>
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
