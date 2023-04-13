<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<script runat="server">
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.noticias oNoticia = (MIDAS.Models.noticias)ViewData["noticia"];

            if (Session["usuario"] != null)
            {
                if (Session["CentralElegida"] != null)
                {
                    centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                }

                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                if (centroseleccionado.tipo != 4)
                {
                    lblNombreOrg.Text = centroseleccionado.nombre;
                }
                else
                {
                    lblNombreOrg.Text = "Todas las centrales";
                }
            }
            
            if (oNoticia != null)
            {
                txtTexto.Text = oNoticia.texto;
                txtTitulo.Text = oNoticia.titulo;
                Session["noticia"] = oNoticia.id;
            }
        }             

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Compass - Proceso </title>

    <script type="text/javascript">
         $(document).ready(function () {

             $("form").submit(function () {
                 var val = $("input[type=submit][clicked=true]").attr("id")

                 if (val == "GuardarNoticia")
                     $("#hdFormularioEjecutado").val("GuardarNoticia");

             });

             $("form input[type=submit]").click(function () {
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
					<h3>Noticia - <asp:Label runat="server" ID="lblNombreOrg"></asp:Label> <small>Esta noticia aparecerá en la pantalla principal</small></h3>
				</div>
			</div>
			<!-- /page header -->

     
            

			<!-- Form vertical (default) -->
						<form method="post" enctype="multipart/form-data" role="form" action="#" runat="server">
                           
                           

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Editar Noticia</h6></div>
                                <div class="panel-body">
                                    
                                     <label>Titulo</label>
                                     <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server"></asp:TextBox>

                                     <br />

                                     <label>Fecha de expiración</label>
                                     <asp:TextBox CssClass="form-control" ID="txtExpira" Width="140px" TextMode="Date" runat="server"></asp:TextBox>
                                     <br />
                                     <a target="_blank" class="btn btn-primary" onclick="window.open(this.href, this.target, 'width=800,height=600'); return false;" href="http://novotecsevilla.westeurope.cloudapp.azure.com/evr/Noticias/imagenes_noticia"> <asp:Literal ID="Literal1" runat="server" Text="Enlaces a imágenes" /></a>
                                     <br />
                                     <br />
                                    
                                    <FTB:FreeTextBox Width="100%" Language="es-ES" DesignModeCss="~/Content/DesignModeCss.css" ToolbarLayout="FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker;Bold,Italic,Underline,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage;Cut,Copy,Paste,Delete;Undo,Redo" id="txtTexto" runat="Server" />

 
			                    </div>  

							</div>

                             
                           



                            
                            <div class="form-actions text-right">
                              
										<input id="GuardarNoticia" type="submit" value="Actualizar Noticia" class="btn btn-primary run-first"/>                                      
                                        <a href="/Noticias/Noticias" value="Salir" class="btn btn-primary" title="Volver">Volver</a>
									</div>
                                    
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
