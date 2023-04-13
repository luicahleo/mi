<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.consultas oConsulta = (MIDAS.Models.consultas)ViewData["consulta"];

            if (oConsulta != null)
            {
                txtConsulta.Text = oConsulta.consulta;
                txtTitulo.Text = oConsulta.titulo;
                txtRespuesta.Text = oConsulta.respuesta;
                if (oConsulta.idusuario != null && oConsulta.idusuario != int.Parse(Session["idusuario"].ToString()))
                {
                    oConsulta.leido = 1;
                    MIDAS.Models.Datos.ActualizarConsulta(oConsulta);
                    txtConsulta.ReadOnly = true;
                    txtTitulo.ReadOnly = true;
                    ddlDestinatario.Enabled = false;
                }
                if (Session["organizacion"] != null)
                {
                    if (Session["perfil"] == "1" || oConsulta.idadmin != int.Parse(Session["organizacion"].ToString()))
                    {
                        txtRespuesta.ReadOnly = true;
                    }
                }
                if (oConsulta.idadmin != 0)
                {
                    ddlDestinatario.SelectedIndex = 1;
                }
            }
            else
            {
                txtRespuesta.ReadOnly = true;
            }
        }             

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Consulta </title>

    <script type="text/javascript">
         $(document).ready(function () {

             $("form").submit(function () {
                 var val = $("input[type=submit][clicked=true]").attr("id")

                 if (val == "GuardarConsulta")
                     $("#hdFormularioEjecutado").val("GuardarConsulta");

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
					<h3>Consulta - <asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
				</div>
			</div>
			<!-- /page header -->

     
            

			<!-- Form vertical (default) -->
						<form method="post" enctype="multipart/form-data" role="form" action="#" runat="server">
                           
                           

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Editar Consulta</h6></div>
                                <div class="panel-body">
                                    
                                     <label>Titulo</label>
                                     <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server"></asp:TextBox>

                                     <br />

                                     <label>Destinatario</label>
                                     <asp:DropDownList CssClass="form-control" ID="ddlDestinatario" runat="server">
                                        <asp:ListItem Value="0" Text="Administradores de la herramienta"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Administradores de la organización"></asp:ListItem>
                                     </asp:DropDownList>

                                     <br />

                                     <label>Consulta</label>
                                     <asp:TextBox CssClass="form-control" ID="txtConsulta" Width="100%" Rows="7" TextMode="MultiLine" runat="server"></asp:TextBox>

                                     <br />
                                    
                                     <label>Respuesta</label>
                                     <asp:TextBox CssClass="form-control" ID="txtRespuesta" Width="100%" Rows="7" TextMode="MultiLine" runat="server"></asp:TextBox>

                                     <br />
			                    </div>  

							</div>

                             
                           



                            
                            <div class="form-actions text-right">
                              
										<input id="GuardarConsulta" type="submit" value="Guardar Consulta" class="btn btn-primary run-first">
                                       
           
									</div>
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
