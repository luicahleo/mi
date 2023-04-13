<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>



<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());


            //DatosCentro.DataSource =ViewData["informacion_centro"] ;
            //DatosCentro.DataBind();
            //string datos = "";
            //foreach (GridViewRow item in DatosCentro.Rows)
            //{
            //    datos += item.Cells[1].Text;
            //}
            if(ViewData["informacion_centro"] != null)
            {
                string[] fuentes = { "Calibri" };
                string[] tamanos={"3"};
                string[] tamanosTraduccion = { "11" };
                __txt_desc.Text = ViewData["informacion_centro"].ToString();
                __txt_desc.FontFacesMenuList =fuentes;
                __txt_desc.FontSizesMenuList = tamanos;
                __txt_desc.FontSizesMenuNames = tamanosTraduccion;
                LiteralControl datos = new LiteralControl();
                datos.Text = ViewData["informacion_centro"].ToString();
                descripcion.Controls.Add(datos);
            }



        }

        if (Session["EliminarSistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EliminarSistema"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }
    }


</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Descripción de la instalación<small>Información de interés de la planta</small></h3>
        </div>
    </div>

    <!-- /page header -->
    <form role="form" action="#" runat="server" method="post">
        <asp:GridView ID="DatosCentro" runat="server" Visible="false">
        </asp:GridView>
        <div class="block" id="Lectura">
                     <div>
            
        </div>
             <div style="text-align: right">


                    <a id="Editar" value="Editar" class="btn btn-primary run-first">Editar</a>

                    <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
                </div>
            </br>
            <div style="width: 95%;    " >
                <div style="border: blue 2px inset;">
                <asp:Panel ID="descripcion" runat="server" style="margin:22px;">
                </asp:Panel>
                </div> 
                </br>
               
            </div>
             
        </div>

        <!-- Tasks table -->
        <div class="block" id="Edicion" hidden>
            <div>
            <h4>Edición</h4>
        </div>
            <center>
                <div style="text-align: right">

                <br />
                <input id="GuardarArea" type="submit" value="Guardar" class="btn btn-primary run-first" name="submit">
                <%-- <a  href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Guardar</a>--%>
                   <a id="CancelarEdicion" value="Editar" class="btn btn-primary run-first">Cancelar Edición</a>
               
            </div>
                <br />
                
                <div style="width: 95%;" class="">
                    
                    <FTB:FreeTextBox ID="__txt_desc" runat="server" Width="100%" Height="600px" ClientIDMode="Static" PasteMode="Default" DesignModeCss="../../ext/common/freetextbox/freetextbox.css" 
                      
                        ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,
                                         FontBackColorsMenu,FontBackColorPicker|Bold,Italic,Underline,Strikethrough,Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,
                                         JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;|Cut,Copy,Paste,Delete;Undo,Redo,Print|
                                         InsertRule,InsertDate,InsertTime|InsertTable,EditTable;InsertTableRowAfter,InsertTableRowBefore,DeleteTableRow;InsertTableColumnAfter,InsertTableColumnBefore,
                                         DeleteTableColumn|InsertDiv,EditStyle,Preview,SelectAll,WordClean">
                    </FTB:FreeTextBox>

                    <%--<FTB:FreeTextBox ID="__txt_desc" runat="server" Width="100%" ClientIDMode="Static" Height="600px" DesignModeCss="../../ext/common/freetextbox/freetextbox.css"></FTB:FreeTextBox>--%>

                </div>
            </center>
            
        </div>
        <!-- /tasks table -->


        <p>
            <br />
        </p>
        <!-- Footer -->
    </form>
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->

    <script type="text/javascript">
        $(document).ready(function () {

            $("#MenuEditarInformacion").css('color', 'black');
            $("#MenuEditarInformacion").css('background-color', '#ebf1de');
            $("#MenuEditarInformacion").css('font-weight', 'bold'); 
        });

        //$('#btnImage').on('click', function () {
        //    formData = new FormData();
        //    formData.append("aUploadedFile", file);
        //    $.ajax({
        //        type: "POST",
        //        url: url,
        //        data: formData,
        //        cache: false,
        //        contentType: false,
        //        processData: false,
        //        success: function (FileUrl) {
        //            // alert(FileUrl);  
        //            var imgNode = document.createElement("img");
        //            imgNode.src = FileUrl;
        //            $("#Content").summernote("insertNode", imgNode);
        //        },
        //        error: function (data) {
        //            alert(data.responseText);
        //        }
        //    });

        //});
        $('#Editar').on('click', function () {
            $('#Edicion').show();
            $('#Lectura').hide();
        });
        $('#CancelarEdicion').on('click', function () {
            $('#Lectura').show();
            $('#Edicion').hide();
      
        });
        function obtenerDocumento() {
            var iframe = frames["txt_desc_designEditor"];
            if (iframe.contentDocument) {
                doc = iframe.contentWindow.document;
            } else {            
                doc =iframe.contentWindow.document;
            }
            var resulhtml = doc.body.innerHTML;
            var resultext = doc.body.innerText;
        }


    </script>

</asp:Content>
