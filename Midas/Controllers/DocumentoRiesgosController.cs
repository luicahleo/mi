using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIDAS.Models;


/*librerias openXM*/
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using System.IO;
using ColorXML = DocumentFormat.OpenXml.Wordprocessing.Color;
using SPS = DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections;
using V = DocumentFormat.OpenXml.Vml;
using System.Text;
using PWT = OpenXmlPowerTools;
using convertidor = OpenXmlPowerTools.HtmlConverter;
using SYSPCK = System.IO.Packaging;
using ConvertidorPDF = PdfSharp.Pdf;
using ConvertidorHTML = TheArtOfDev.HtmlRenderer.PdfSharp;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using MIDAS.Helpers;

namespace MIDAS.Controllers
{
    public class DocumentoRiesgosController : Controller
    {
        // GET: DocumentoRiesgos
        string docxMIMEType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public ActionResult Index()
        {
            return View();
        }

        public Paragraph obtenerimagenMedidaRiesgo(WordprocessingDocument wordDoc, string imagen)
        {
            var rutalocalPrueba = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), imagen);

            ImagePart imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
            string rutalocal = Server.MapPath("~/");
            var existeImagen = System.IO.File.Exists(rutalocal + imagen);
            if (!existeImagen)
            {
                imagen = "/Content/images/medidas/imagenNoDisponibleIcono.png";
            }
            //string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas";
            using (FileStream flujoimagen = new FileStream(rutalocal + imagen, FileMode.Open))

            {
                imagePart.FeedData(flujoimagen);
            }
            return AddImageToBody(wordDoc, wordDoc.MainDocumentPart.GetIdOfPart(imagePart), 40, 30);
        }
        public Paragraph obtenerimagenMedidaRiesgoImagen(WordprocessingDocument wordDoc, string imagen, int alto, int ancho)
        {
            var rutalocalPrueba = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas"), imagen);
            string rutalocal = Server.MapPath("~/");
            var existeImagen = System.IO.File.Exists(rutalocal + imagen);
            if (!existeImagen)
            {
                imagen = "/Content/images/medidas/imagenNoDisponible.png";
            }
            ImagePart imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
            // string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas";
            using (FileStream flujoimagen = new FileStream(rutalocal + imagen, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                imagePart.FeedData(flujoimagen);
                var imagenActual = Image.FromFile(rutalocal + imagen);
                alto = imagenActual.Height;
                ancho = imagenActual.Width;

            }
            return AddImageToBody(wordDoc, wordDoc.MainDocumentPart.GetIdOfPart(imagePart), alto, ancho);
        }
        public Paragraph obtenerimagenMedidaPreventivasImagen(WordprocessingDocument wordDoc, string imagen)
        {

            var rutalocalPrueba = System.IO.Path.Combine(Server.MapPath("~/Content/images/medidas/medidaspreventivas"), imagen);
            ImagePart imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
            string rutalocal = Server.MapPath("~/");
            // string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas";
            using (FileStream flujoimagen = new FileStream(rutalocal + imagen, FileMode.Open))
            {
                imagePart.FeedData(flujoimagen);
            }
            return AddImageToBody(wordDoc, wordDoc.MainDocumentPart.GetIdOfPart(imagePart), 570, 400);
        }
        public Paragraph obtenerimagenEndesa(WordprocessingDocument wordDoc)
        {
            var rutalocal = Server.MapPath("~/Imagenes/endesa.PNG");

            ImagePart imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Png);
            //string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas/Imagenes/endesa.PNG";
            // C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas/Imagenes/Central/
            using (FileStream flujoimagen = new FileStream(rutalocal, FileMode.Open))


            {
                imagePart.FeedData(flujoimagen);
            }
            return AddImageToBody(wordDoc, wordDoc.MainDocumentPart.GetIdOfPart(imagePart), 40, 30);
        }


        public void ApplyHeader(WordprocessingDocument doc)
        {
            // Get the main document part.
            MainDocumentPart mainDocPart = doc.MainDocumentPart;
            HeaderPart headerPart1 = mainDocPart.AddNewPart<HeaderPart>("r97");
            Header header1 = new Header();
            Paragraph paragraph1 = new Paragraph() { };
            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = "Header stuff";
            run1.Append(text1);
            paragraph1.Append(run1);
            header1.Append(paragraph1);
            headerPart1.Header = header1;

            SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
            if (sectionProperties1 == null)
            {
                sectionProperties1 = new SectionProperties() { };
                mainDocPart.Document.Body.Append(sectionProperties1);
            }
            HeaderReference headerReference1 = new HeaderReference() { Type = HeaderFooterValues.Default, Id = "r97" };
            sectionProperties1.InsertAt(headerReference1, 0);
        }
        //private static Drawing BuildImage(string imageRelationshipID, string imageName,int pixelWidth, int pixelHeight)
        //{
        //    int emuWidth = (int)(pixelWidth * EMU_PER_PIXEL);
        //    int emuHeight = (int)(pixelHeight * EMU_PER_PIXEL);
        //    Drawing drawing = new Drawing();
        //    d.Wordprocessing.Inline inline = new d.Wordprocessing.Inline { DistanceFromTop = 0, DistanceFromBottom = 0, DistanceFromLeft = 0, DistanceFromRight = 0 };
        //    d.Wordprocessing.Anchor anchor = new d.Wordprocessing.Anchor();
        //    d.Wordprocessing.SimplePosition simplePos = new d.Wordprocessing.SimplePosition { X = 0, Y = 0 };
        //    d.Wordprocessing.Extent extent = new d.Wordprocessing.Extent { Cx = emuWidth, Cy = emuHeight };
        //    d.Wordprocessing.DocProperties docPr = new d.Wordprocessing.DocProperties { Id = 1, Name = imageName };
        //    d.Graphic graphic = new d.Graphic();
        //    // We don’t have to hard code a URI anywhere else in the document but if we don’t do it here 
        //    // we end up with a corrupt document.
        //    d.GraphicData graphicData = new d.GraphicData { Uri = GRAPHIC_DATA_URI };
        //    d.Pictures.Picture pic = new d.Pictures.Picture();
        //    d.Pictures.NonVisualPictureProperties nvPicPr = new d.Pictures.NonVisualPictureProperties();
        //    d.Pictures.NonVisualDrawingProperties cNvPr = new d.Pictures.NonVisualDrawingProperties { Id = 2, Name = imageName };
        //    d.Pictures.NonVisualPictureDrawingProperties cNvPicPr = new d.Pictures.NonVisualPictureDrawingProperties();
        //    d.Pictures.BlipFill blipFill = new d.Pictures.BlipFill();
        //    d.Blip blip = new d.Blip { Embed = imageRelationshipID };
        //    d.Stretch stretch = new d.Stretch();
        //    d.FillRectangle fillRect = new d.FillRectangle();
        //    d.Pictures.ShapeProperties spPr = new d.Pictures.ShapeProperties();
        //    d.Transform2D xfrm = new d.Transform2D();
        //    d.Offset off = new d.Offset { X = 0, Y = 0 };
        //    d.Extents ext = new d.Extents { Cx = emuWidth, Cy = emuHeight };
        //    d.PresetGeometry prstGeom = new d.PresetGeometry { Preset = d.ShapeTypeValues.Rectangle };
        //    d.AdjustValueList avLst = new d.AdjustValueList();
        //    xfrm.Append(off);
        //    xfrm.Append(ext);
        //    prstGeom.Append(avLst);
        //    stretch.Append(fillRect);
        //    spPr.Append(xfrm);
        //    spPr.Append(prstGeom);
        //    blipFill.Append(blip);
        //    blipFill.Append(stretch);
        //    nvPicPr.Append(cNvPr);
        //    nvPicPr.Append(cNvPicPr);
        //    pic.Append(nvPicPr);
        //    pic.Append(blipFill);
        //    pic.Append(spPr);
        //    graphicData.Append(pic);
        //    graphic.Append(graphicData);
        //    inline.Append(extent);
        //    inline.Append(docPr);
        //    inline.Append(graphic);
        //    drawing.Append(inline);
        //    return drawing;
        //}
        public Header encabezad(string centro, string version, WordprocessingDocument wordoc)
        {
            Header h = new Header();
            //Paragraph p = new Paragraph();
            //Run r = new Run();
            //Table tablaEncabezado = new Table();
            //h.Append(obtenerimagenEndesa(wordoc));
            ////p.Append(r);
            //r = new Run();
            //RunProperties rPr = new RunProperties();
            //TabChar tab = new TabChar();
            //Bold b = new Bold();
            //ColorXML color = new ColorXML { Val = "006699" };
            //FontSize sz = new FontSize { Val = "25" };
            //Text t = new Text { Text = centro };
            //var centerJustification = new Justification() { Val = JustificationValues.Center };
            //rPr.Append(b);
            //rPr.Append(color);
            //rPr.Append(sz);
            //rPr.Append(centerJustification);
            //r.Append(rPr);
            //r.Append(tab);
            //r.Append(t);
            //p.Append(r);
            //h.Append(p);
            //h.Append(construirTablaCabecera(centro, version,wordoc));
            return h;
            //Header h = new Header();
            //Paragraph p = new Paragraph();
            //// Run r = new Run();
            //Table tablaEncabezado = new Table();
            //TableProperties tblProp = new TableProperties(
            //             new TableBorders(
            //                 new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
            //                 new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
            //             )
            //         );

            //tablaEncabezado.AppendChild<TableProperties>(tblProp);
            //TableRow tr1 = new TableRow();
            //TableRow tr2 = new TableRow();


            //TableCell tr1c1 = new TableCell(obtenerimagenEndesa(wordoc));
            //TableCell tr1c2 = new TableCell(new Paragraph(new Run(new Text() { Text = centro })));
            //TableCell tr2c1 = new TableCell(new Paragraph(new Run(new Text() { Text = centro })));
            //TableCell tr2c2 = new TableCell(new Paragraph(new Run(new Text() { Text = centro })));
            //tr1.Append(tr1c1);
            //tr1.Append(tr1c2);
            //tr2.Append(tr2c1);
            //tr2.Append(tr2c2);
            //tablaEncabezado.Append(tr1);
            //tablaEncabezado.Append(tr2);
            //p.Append(tablaEncabezado);
            //h.Append(p);
            //return h;

        }
        private static Paragraph GeneratePicHeader(string relationshipId, int ancho, int alto)
        {
            // Define the reference of the image.
            Size size = new Size(ancho, alto);

            Int64Value width = 80 * 9525;
            Int64Value height = 40 * 9525;



            var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent() { Cx = width, Cy = height },
                        new DW.EffectExtent()
                        {
                            LeftEdge = 0L,
                            TopEdge = 0L,
                            RightEdge = 0L,
                            BottomEdge = 0L
                        },
                        new DW.DocProperties()
                        {
                            Id = (UInt32Value)1U,
                            Name = "NIS Logo"
                        },
                        new DW.NonVisualGraphicFrameDrawingProperties(
                            new A.GraphicFrameLocks() { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties()
                                        {
                                            Id = (UInt32Value)0U,
                                            Name = "nis.png"
                                        },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension()
                                                {
                                                    Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                })
                                        )
                                        {
                                            Embed = relationshipId,
                                            CompressionState =
                                                A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(
                                            new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0L, Y = 92000L },
                                            new A.Extents() { Cx = 990000L, Cy = 392000L }),
                                        new A.PresetGeometry(
                                            new A.AdjustValueList()
                                        )
                                        { Preset = A.ShapeTypeValues.Rectangle }))
                            )
                            { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                    )
                    {
                        DistanceFromTop = (UInt32Value)0U,
                        DistanceFromBottom = (UInt32Value)0U,
                        DistanceFromLeft = (UInt32Value)0U,
                        DistanceFromRight = (UInt32Value)0U,
                        EditId = "50D07946"
                    });

            var header = new Header();
            Paragraph paragraph = new Paragraph();
            var run = new Run();

            run.Append(element);
            paragraph.Append(run);
            // header.Append(paragraph);
            return paragraph;
        }

        public Paragraph obtenerNumeracionCabecera()
        {

            return new Paragraph(
                    new ParagraphProperties(
                     new ParagraphStyleId() { Val = "Header" },
                     new Justification() { Val = JustificationValues.Center },
                    new FontSize() { Val = "18" }),

                    new Run(
                     new Text() { Text = "Página ", Space = SpaceProcessingModeValues.Preserve }),
                    new Run(
                     new SimpleField() { Instruction = "Page" }),
                    new Run(
                     new Text() { Text = " de ", Space = SpaceProcessingModeValues.Preserve }),
                    new Run(
                     new SimpleField() { Instruction = "NUMPAGES" })
                       );

        }




        private void InsertCustomWatermark(WordprocessingDocument package, string p)
        {
            SetWaterMarkPicture(p);
            MainDocumentPart mainDocumentPart1 = package.MainDocumentPart;
            if (mainDocumentPart1 != null)
            {
                mainDocumentPart1.DeleteParts(mainDocumentPart1.HeaderParts);
                HeaderPart headPart1 = mainDocumentPart1.AddNewPart<HeaderPart>();
                GenerateHeaderPart1Content(headPart1);
                string rId = mainDocumentPart1.GetIdOfPart(headPart1);
                ImagePart image = headPart1.AddNewPart<ImagePart>("image/jpeg", "rId999");
                GenerateImagePart1Content(image);
                IEnumerable<SectionProperties> sectPrs = mainDocumentPart1.Document.Body.Elements<SectionProperties>();
                foreach (var sectPr in sectPrs)
                {
                    sectPr.RemoveAllChildren();
                    sectPr.PrependChild(new HeaderReference() { Id = rId });
                }
            }
            else
            {
            }
        }
        private void GenerateHeaderPart1Content(HeaderPart headerPart1)
        {
            Header header1 = new Header();
            Paragraph paragraph2 = new Paragraph();
            Run run1 = new Run();
            Picture picture1 = new Picture();
            V.Shape shape1 = new V.Shape() { Id = "WordPictureWatermark75517470", Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:415.2pt;height:456.15pt;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin", OptionalString = "_x0000_s2051", AllowInCell = false, Type = "#_x0000_t75" };
            V.ImageData imageData1 = new V.ImageData() { Gain = "19661f", BlackLevel = "22938f", Title = "??", RelationshipId = "rId999" };
            shape1.Append(imageData1);
            picture1.Append(shape1);
            run1.Append(picture1);
            paragraph2.Append(run1);
            header1.Append(paragraph2);
            headerPart1.Header = header1;
        }
        private void GenerateImagePart1Content(ImagePart imagePart1)
        {
            System.IO.Stream data = GetBinaryDataStream(imagePart1Data);
            imagePart1.FeedData(data);
            data.Close();
        }
        private string imagePart1Data = "";
        private System.IO.Stream GetBinaryDataStream(string base64String)
        {
            return new System.IO.MemoryStream(System.Convert.FromBase64String(base64String));
        }
        public void SetWaterMarkPicture(string file)
        {
            FileStream inFile;
            byte[] byteArray;
            try
            {
                inFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                byteArray = new byte[inFile.Length];
                long byteRead = inFile.Read(byteArray, 0, (int)inFile.Length);
                inFile.Close();
                imagePart1Data = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
        public ActionResult GenerarDocumentoBorrador(string descrDoc)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return RedirectToAction("GenerarDocumento", "DocumentoRiesgos", new { descrDoc = descrDoc, esborrador = true });
        }

        public ActionResult GenerarDocumentoBorradorCritico(string descrDoc)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return RedirectToAction("GenerarDocumentoCritico", "DocumentoRiesgos", new { descrDoc = descrDoc, esborrador = true });
        }

        public ActionResult GenerarDocumentoDefinitivoCritico(string descrDoc)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }

            return RedirectToAction("GenerarDocumento", "DocumentoRiesgos", new { descrDoc = descrDoc, esborrador = false });


        }

        public ActionResult GenerarDocumentoDefinitivo(string descrDoc)
        {
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }

            return RedirectToAction("GenerarDocumento", "DocumentoRiesgos", new { descrDoc = descrDoc, esborrador = false });


        }


        public ActionResult GenerarDocumentoRiesgosCriticos()
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                ViewData["textoUltimoBoradorCriticos"] = "";
                if (Datos.ObtenerUltimaversionDocumentoCriticos(centroseleccionado.id) != 0)
                {
                    int ultimaversion = Datos.ObtenerUltimaversionDocumentoCriticos(centroseleccionado.id);

                    ViewData["textoUltimoBoradorCriticos"] = Datos.ObtenerUltimaversionTextoDocumentoCriticos(ultimaversion);
                }
                List<version_matriz> matrices = Datos.listarMatrizVersion(centroseleccionado.id);
                if (matrices != null)
                    ViewData["Haymatrizborrador"] = matrices.Where(x => x.estado == 1).Select(x => x.id).ToList().Count();
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }


        public ActionResult GenerarDocumentoRiesgos()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                ViewData["textoUltimoBorador"] = "";

                var docu = Datos.ObtenerUltimaversionDocumento(centroseleccionado.id);

                if (docu != 0)
                {
                    int ultimaversion = Datos.ObtenerUltimaversionDocumento(centroseleccionado.id);

                    ViewData["textoUltimoBorador"] = Datos.ObtenerUltimaversionTextoDocumento(ultimaversion);
                }

                List<version_matriz> matrices = Datos.listarMatrizVersion(centroseleccionado.id);
                if (matrices != null)
                {
                    ViewData["Haymatrizborrador"] = matrices.Where(x => x.estado == 1).Select(x => x.id).ToList().Count();
                }

            }
            catch (Exception ex)
            {
                new EscribirLog("Error: " +
                            ex.Message, true, this.ToString(), "GenerarDocumentoRiesgos");
                return RedirectToAction("LogOn", "Account");
            }
            return View();

        }

        [HttpPost]
        public ActionResult GenerarDocumentoRiesgos(FormCollection collection)
        {
            try
            {
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("LogOn", "Account");
                }

                var doc = Datos.ObtenerDocumentoHistoricoRutaDescarga(centroseleccionado.id);

                if (doc != null)
                {
                    doc.ruta = doc.ruta.Replace("..", "~");

                    Response.ContentType = "application/msexcel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + doc.nombre);
                    Response.TransmitFile(doc.ruta + doc.nombre);
                    Response.End();
                }


                //ViewData["textoUltimoBorador"] = "";
                //if (Datos.ObtenerUltimaversionDocumento(centroseleccionado.id) != 0)
                //{
                //    int ultimaversion = Datos.ObtenerUltimaversionDocumento(centroseleccionado.id);

                //    ViewData["textoUltimoBorador"] = Datos.ObtenerUltimaversionTextoDocumento(ultimaversion);
                //}

            }
            catch (Exception Ex)
            {
                return RedirectToAction("LogOn", "Account");
            }
            //return RedirectToAction("GenerarDocumentoRiesgos", "DocumentoRiesgos");
            return View();
        }


        public ActionResult GenerarDocumento(string descrDoc, bool esborrador)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            try
            {
                string nombredocumento = "";
                if (descrDoc != null)
                {
                    nombredocumento = descrDoc;
                }
                centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                //string random = Datos.GeneraNumeroRandom();

                string version = "";

                if (!esborrador)
                {
                    version = Datos.ObtenerDocumentoHistoricoFinal(centroseleccionado.id).Count.ToString();
                    //var Centro = Datos.InsertarMatrizVersion(Datos.obtenerUltimaVersionCentro(centroseleccionado.id), Session["usuario"].ToString());
                    var Centro = Datos.finalizarMatrizVersion(Datos.obtenerUltimaVersionCentro(centroseleccionado.id), Session["usuario"].ToString());
                }
                else
                {
                    version = (Datos.ObtenerDocumentoHistoricoBorrador(centroseleccionado.id).Count).ToString();
                }


                string ficheroresultado = "";
                //  using (var wordDocument = WordprocessingDocument.Create(@"c:\Users\Public\Documents\DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx",
                //    WordprocessingDocumentType.Document, true))
                using (MemoryStream stream = new MemoryStream())
                {
                    using (var wordDocument = WordprocessingDocument.Create(stream,
                        WordprocessingDocumentType.Document, true))
                    {


                        //    using (var wordDocument = WordprocessingDocument.Create(Server.MapPath("~/Content/ficheros/" + "DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx"),
                        //    WordprocessingDocumentType.Document, true))
                        //{
                        SPS.PageSetup pruea = new SPS.PageSetup() { Draft = true };

                        MainDocumentPart mp = wordDocument.AddMainDocumentPart();
                        var documento = new Document();
                        var body = new Body();
                        var body2 = new Body();
                        string imagenblanco = "../Content/images/medidas/imagenNoDisponible.png";
                        //string imagenblancoCentro = "../Content/images/centros/imagenNoDisponibleCentro.png";
                        //string imagenblancoLogo = "../Content/images/centros/logos/imagenNoDisponibleLogo.png";


                        //PORTADA
                        TitlePage pag = new TitlePage() { Val = true };
                        body.Append(titular("RIESGOS INHERENTES Y MEDIDAS PREVENTIVAS", "44", "", "centrar", "1", "", 3));
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));
                        body.Append(titular(centroseleccionado.nombre, "44", "", "centrar", "1", "298af0", 3));
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));
                        ImagePart imagePart = wordDocument.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);
                        string rutalocal = Server.MapPath("~/Imagenes/Central/");
                        string rutaImagenCentro = "";
                        if (centroseleccionado.rutaImagen != null)
                        {

                            var ruta = centroseleccionado.rutaImagen.Replace("..", "");
                            string rutaServer = Server.MapPath("~/");
                            string rutaImagen = Path.Combine(rutaServer + ruta);

                            if (System.IO.File.Exists(rutaImagen))
                            {
                                rutaImagenCentro = Server.MapPath(centroseleccionado.rutaImagen);
                            }
                            else
                            {
                                rutaImagenCentro = Server.MapPath(imagenblanco);

                            }
                        }
                        else
                        {
                            rutaImagenCentro = rutalocal + "central_estandar.BMP";
                        }
                        //string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas/Imagenes/Central/";
                        using (FileStream flujoimagen = new FileStream(/*rutalocal + "aspontes.BMP"*/rutaImagenCentro, FileMode.Open))
                        {
                            imagePart.FeedData(flujoimagen);
                        }
                        body.Append(AddImageToBody(wordDocument, wordDocument.MainDocumentPart.GetIdOfPart(imagePart), 600, 600));
                        body.Append(pag);
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                        //CABECERA
                        Paragraph cabeceraNumeracion = obtenerNumeracionCabecera();


                        HeaderPart hp = mp.AddNewPart<HeaderPart>();
                        string headerRelationshipID = mp.GetIdOfPart(hp);
                        //Nueva seccion
                        SectionProperties sectPr = new SectionProperties();
                        HeaderReference headerReference = new HeaderReference();
                        headerReference.Id = headerRelationshipID;
                        headerReference.Type = HeaderFooterValues.Default;
                        sectPr.Append(headerReference);
                        body.Append(sectPr);

                        hp.Header = encabezad("Documento de Riesgos Inherentes: " + centroseleccionado.nombre, "Revision" + nombredocumento, wordDocument);
                        var newHeaderPart = mp.AddNewPart<HeaderPart>();
                        var imgPart = hp.AddImagePart(ImagePartType.Png, "rId999");

                        string rutaIcono = "";
                        if (centroseleccionado.rutaImagenLogo != null)
                        {

                            var ruta = centroseleccionado.rutaImagenLogo.Replace("..", "");
                            string rutaServer = Server.MapPath("~/");
                            string rutaImagen = Path.Combine(rutaServer + ruta);

                            if (System.IO.File.Exists(rutaImagen))
                            {
                                rutaIcono = Server.MapPath(centroseleccionado.rutaImagenLogo);
                            }
                            else
                            {
                                rutaIcono = Server.MapPath(imagenblanco);
                            }
                        }
                        else
                        {
                            rutaIcono = Server.MapPath("~/Imagenes/endesa.PNG");
                        }

                        int iWidth = 0;
                        int iHeight = 0;
                        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(rutaIcono))
                        {
                            iWidth = bmp.Width;
                            iHeight = bmp.Height;
                        }

                        //string rutaIcono = Server.MapPath("~/Imagenes/endesa.PNG");
                        using (FileStream flujo = new FileStream(rutaIcono, FileMode.Open))
                        {
                            imgPart.FeedData(flujo);
                        }

                        //CONTROL DE CAMBIOS
                        //
                        documentos_riesgos documentoNuevo = new documentos_riesgos();
                        documentoNuevo.id_centro = centroseleccionado.id;
                        documentoNuevo.fechageneracion = DateTime.Now;
                        documentoNuevo.descripcion = nombredocumento;
                        documentoNuevo.revision = NumeroRevision(centroseleccionado.id);

                        documentoNuevo.id = Datos.ObtenerUltimaversionDocumento(centroseleccionado.id);

                        if (documentoNuevo.id != 0)
                        {
                            var docRies = Datos.ObtenerDocumentosRiesgosPorId(documentoNuevo.id);
                            if (docRies != null && docRies.esborrador == false)
                            {
                                documentoNuevo.id = 0;
                                Datos.ActualizarDocumentoRiesgos(documentoNuevo, esborrador);
                            }
                            else
                            {
                                Datos.ActualizarDocumentoRiesgos(documentoNuevo, esborrador);
                            }
                        }
                        else
                        {
                            Datos.ActualizarDocumentoRiesgos(documentoNuevo, esborrador);
                        }






                        //if (esborrador)
                        //{
                        //Datos.ActualizarDocumentoRiesgos(documentoNuevo, esborrador);
                        //}
                        //else
                        //{
                        //    Datos.ActualizarDocumentoRiesgos(documentoNuevo, esborrador);
                        //}



                        var imagePartID = hp.GetIdOfPart(imgPart);

                        //CODIGO SOBRANTE
                        var rId = mp.GetIdOfPart(hp);
                        var headerRef = new HeaderReference { Id = rId };
                        var sectionProps = new SectionProperties();
                        HeaderReference headerReference2 = new HeaderReference();
                        headerRef.Id = headerRelationshipID;
                        headerRef.Type = HeaderFooterValues.Default;
                        sectionProps.Append(headerRef);
                        sectionProps.RemoveAllChildren<HeaderReference>();
                        sectionProps.Append(headerRef);
                        sectionProps.Append(new Justification() { Val = JustificationValues.Center });
                        body.Append(sectionProps);
                        //CODIGO SOBRANTE

                        hp.Header.Append(construirTablaCabecera(GeneratePicHeader(imagePartID, iWidth, iHeight),
                            centroseleccionado.nombre, version, wordDocument, cabeceraNumeracion));
                        //hp.Header.Append(new Paragraph( new Run(new Break() { Type = BreakValues.TextWrapping })));
                        hp.Header.Append(new Paragraph(new Run("")));
                        hp.Header.Save();

                        //INDICE

                        var indice = titular("ÍNDICE", "25", "", "centrar", "1", "", 1);
                        body.Append(indice);

                        var indice0 = titular("0. CONTROL DE CAMBIOS", "25", "", "", "1", "", 1);
                        body.Append(indice0);

                        var indice1 = titular("1. INTRODUCCIÓN", "25", "", "", "1", "", 1);
                        body.Append(indice1);

                        var indice2 = titular("2. DESCRIPCIÓN DE LA INSTALACIÓN", "25", "", "", "1", "", 1);
                        body.Append(indice2);

                        var indice3 = titular("3. IDENTIFICACIÓN DE RIESGOS", "25", "", "", "1", "", 1);
                        body.Append(indice3);

                        var indice4 = titularIdentado("3.1 RIESGOS TIPO", "25", "", "", "1", "", 1);
                        body.Append(indice4);

                        var indice5 = titularIdentado("3.2. MATRIZ DE RIESGOS", "25", "", "", "1", "", 1);
                        body.Append(indice5);

                        var indice6 = titular("4. MEDIDAS PREVENTIVAS", "25", "", "", "1", "", 1);
                        body.Append(indice6);

                        var indice7 = titularIdentado("4.1 MEDIDAS PREVENTIVAS GENERALES", "25", "", "", "1", "", 1);
                        body.Append(indice7);

                        var indice8 = titularIdentado("4.2. MEDIDAS PREVENTIVAS ESPECÍFICAS", "25", "", "", "1", "", 1);
                        body.Append(indice8);

                        //var indice9 = titular("4.3. MEDIDAS PREVENTIVAS ESPECÍFICAS RIESGOS CRITICOS", "25", "", "", "1", "", 1);
                        //body.Append(indice9);

                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        //Pagina 0 control cambios
                        var parrafoControlCambios = titular("0. CONTROL DE CAMBIOS", "31", "", "", "1", "", 1);
                        body.Append(parrafoControlCambios);
                        //


                        //TABLA DE CONTROL DE CAMBIOS
                        Table tCambios = construirTabla(centroseleccionado.id);

                        body.Append(tCambios);
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));

                        //INTRODUCCION
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        var titIntroduccion = titular("1. INTRODUCCIÓN", "31", "", "", "1", "", 1);
                        body.Append(titIntroduccion);
                        //PARTE FIJA EN METODOS

                        //body.Append(titular("1.1. OBJETO", "22, "", "", "1", "", 1));
                        //body.Append(titular(textoObjeto(centroseleccionado.nombre), "18", "", "", "", "", 2));
                        //body.Append(titular("1.2. REFERENCIAS: MÉTODO ENDESA", "22, "", "", "1", "", 1));
                        //body.Append(titular(textoEndesa(), "18", "", "", "", "", 2));

                        //List<string> listadescripcion = DameListaDescripcion();
                        //AddBulletList(listadescripcion, wordDocument, body, "·","720");
                        //body.Append(new Break());


                        //DESARROLLO EDICION PARTE FIJA
                        string altChunkIdDescripcion = "altChunkIdDescripcion";
                        AlternativeFormatImportPart afipDescripcion = wordDocument.MainDocumentPart
                                .AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkIdDescripcion);
                        string htmlDescripcion = "<html><head></head><body>";
                        htmlDescripcion += Datos.ObtenerDescripcionGeneral(1).descripcion;
                        htmlDescripcion += "</body></html>";
                        htmlDescripcion = htmlDescripcion.Replace("-INSTALACION-", centroseleccionado.nombre);
                        afipDescripcion.FeedData(new MemoryStream(Encoding.Default.GetBytes(htmlDescripcion)));
                        AltChunk altChunkDescripcion = new AltChunk();
                        altChunkDescripcion.Id = altChunkIdDescripcion;
                        body.Append(altChunkDescripcion);
                        body.Append(new Break());
                        //DESARROLLO EDICION PARTE FIJA


                        /*DESCRIPCION*/
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        var titparrafoDescripcion = titular("2. DESCRIPCIÓN DE LA INSTALACIÓN", "31", "", "", "1", "", 1);
                        // body.Append(new Break());
                        body.Append(titparrafoDescripcion);



                        //body.Append(titular("2.1. DESCRIPCIÓN GENERAL", "22", "", "", "1", "", 1));
                        //   body.Append(new Break());





                        //OBTENER DESCRIPCION CENTRO
                        //var parrafoDescripcion = new Paragraph();
                        //var seccionDescripcion = new Run();
                        //string textoDescripcion = "";
                        //if (Datos.ObtenerInformacionCentral(centroseleccionado.id).descripcion_texto != null)
                        //{
                        //    textoDescripcion = Datos.ObtenerInformacionCentral(centroseleccionado.id).descripcion_texto;
                        //}



                        //textoDescripcion = textoDescripcion.Replace("  ", " ");
                        //textoDescripcion = textoDescripcion.Replace("&nbsp;", "");
                        //textoDescripcion = textoDescripcion.Replace("\r\n", "\n");
                        //textoDescripcion = textoDescripcion.Replace("\r", "\n");
                        //textoDescripcion = textoDescripcion.Replace("-", "");
                        //var textDescripcion = new Text(textoDescripcion.ToString()) { Space = SpaceProcessingModeValues.Preserve };
                        //seccionDescripcion.Append(textDescripcion);
                        //parrafoDescripcion.Append(seccionDescripcion);
                        //body.Append(titular(textoDescripcion, "18", "", "", "", "", 2));
                        //OBTENER DESCRIPCION CENTRO




                        /*PRUEBA1 HTML*/
                        //var parrafoHTMLDescripcion = new Paragraph();
                        //var seccionHTMLDescripcion = new Run();
                        string textoHTMLDescripcion = "";
                        if (Datos.ObtenerInformacionCentral(centroseleccionado.id) != null)
                        {
                            if (Datos.ObtenerInformacionCentral(centroseleccionado.id).descripcion != null)
                            {
                                textoHTMLDescripcion = Datos.ObtenerInformacionCentral(centroseleccionado.id).descripcion;
                            }
                        }



                        //var textHTMLDescripcion = new Text(textoHTMLDescripcion.ToString()) { Space = SpaceProcessingModeValues.Preserve };
                        //seccionHTMLDescripcion.Append(textHTMLDescripcion);
                        //parrafoHTMLDescripcion.Append(seccionHTMLDescripcion);
                        //body.Append(titular(textoHTMLDescripcion, "18", "", "", "", "", 2));
                        /*PRUEBA1 HTML*/
                        //prueba2 html
                        string altChunkId = "AltChunkId";
                        AlternativeFormatImportPart afip = wordDocument.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);
                        string htmlt = "<html><head></head><body>";
                        htmlt += textoHTMLDescripcion;
                        htmlt += "</body></html>";
                        afip.FeedData(new MemoryStream(Encoding.Default.GetBytes(htmlt)));
                        AltChunk altChunk = new AltChunk();
                        altChunk.Id = altChunkId;

                        body.Append(altChunk);

                        //prueba2 html


                        //RIESGOS
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                        var titIdentifRiesgos = titular("3. IDENTIFICACIÓN DE RIESGOS", "31", "", "", "1", "", 1);
                        body.Append(titIdentifRiesgos);
                        var titIdentifRiesgos2 = titular("3.1 RIESGOS TIPO", "22", "", "", "1", "", 1);
                        body.Append(titIdentifRiesgos2);
                        // body.Append(new Break());
                        var tablaRiesgos = GettablaRiesgos(centroseleccionado.id);
                        //body.Append(new Break());
                        body.Append(tablaRiesgos);
                        // body.Append(new Break());



                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        body.Append(new Paragraph(new ParagraphProperties(CreateSectionProperties(PageOrientationValues.Portrait))));
                        //MATRIZ DE RIESGOS
                        var titMatriz = titular("3.2. MATRIZ DE RIESGOS", "22", "", "", "1", "", 1);
                        body.Append(new Break());
                        body.Append(titMatriz);
                        var tablaatriz = GettablaMatriz(centroseleccionado.id);
                        body.Append(tablaatriz);
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        body.Append(new Paragraph(new ParagraphProperties(CreateSectionProperties(PageOrientationValues.Landscape))));

                        //MEDIDAS PREVENTIVAS
                        var titMedidas = titular("4. MEDIDAS PREVENTIVAS", "31", "", "", "1", "", 1);
                        body.Append(titMedidas, CreateSectionProperties(PageOrientationValues.Portrait));

                        //  body.Append(new Break());

                        List<tipo_riesgos_critico> listaRiesgos = Datos.ListarTiposRiesgosCriticos();
                        List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
                        List<medidas_preventivas> listaMedidas = Datos.ListarMedidas()
                            .Where(x => x.id_centro == 0 || x.id_centro == centroseleccionado.id).ToList();
                        List<parametrica_medidas> obtenerSeleccionadas = (List<parametrica_medidas>)Datos.ListarParametricaMedidas()
                            .Where(x => x.id_centro == centroseleccionado.id && x.activo == true).ToList();

                        listaMedidas = (List<medidas_preventivas>)listaMedidas.Where(x => obtenerSeleccionadas
                                    .Select(z => z.id_medida).Contains(x.id)).ToList();

                        List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centroseleccionado.id);
                        //List<medidas_apartados> apartados = Datos.ListarApartados();
                        List<medidas_apartadosV2> apartados = Datos.ListarApartadosV2();
                        List<medidas_apartados_generales> apartadosGenerales = Datos.ListarApartadosGenerales();

                        //List<medidas_generales> medGeneralesCentro = Datos.ListarMedidasGenerales(centroseleccionado.id, 0);

                        List<medidas_generales> medGeneralesCentro = Datos.ListarMedidasGenerales();

                        if (medGeneralesCentro.Count > 0)
                        {
                            var titMedidasge = titular("4.1 MEDIDAS PREVENTIVAS GENERALES", "22", "", "", "1", "", 1);
                            body.Append(titMedidasge);

                            foreach (medidas_apartados_generales item in apartadosGenerales)
                            {
                                var titApartMedGeneral = titular(item.descripcion, "18", "", "", "1", "298af0", 1);
                                body.Append(titApartMedGeneral);
                                foreach (medidas_generales medg in medGeneralesCentro.Where(x => x.id_apartado_generales == item.id))
                                {
                                    string rutaImagen = Datos.obtenerImagenMedidasGenerales(medg.id);

                                    if (!string.IsNullOrEmpty(rutaImagen))
                                    {
                                        rutaImagen = rutaImagen.Replace("..", "");
                                        Table tablamedgen;
                                        //rutaImagen = ("~") + rutaImagen;

                                        string rutaimagenserver = Server.MapPath("~/" + rutaImagen);
                                        if (System.IO.File.Exists(rutaimagenserver))
                                        {
                                            if (medg.descripcion.Contains("[SALTO]"))
                                            {
                                                string saltosdelineaG = medg.descripcion.Replace("[SALTO]", "\n");
                                                string[] lineasG = saltosdelineaG.Split('\n');
                                                List<string> listaSaltosG = new List<string>();
                                                foreach (string itemSaltos in lineasG)
                                                {
                                                    listaSaltosG.Add(itemSaltos);
                                                }
                                                tablamedgen = construirFilaBullet(listaSaltosG, wordDocument, rutaImagen);
                                                body.Append(titular("", "", "", "", "", "", 2));
                                                body.Append(tablamedgen);

                                            }
                                            else
                                            {
                                                Paragraph MedidaGeneral = titular(medg.descripcion, "18", "", "", "", "", 2);
                                                tablamedgen = construirFila(MedidaGeneral, wordDocument, rutaImagen);
                                                body.Append(titular("", "", "", "", "", "", 2));
                                                body.Append(tablamedgen);
                                            }
                                        }
                                        else
                                        {

                                            Paragraph MedidaGeneral = titular(medg.descripcion, "18", "", "", "", "", 2);
                                            tablamedgen = construirFila(MedidaGeneral, wordDocument, imagenblanco);
                                            body.Append(titular("", "", "", "", "", "", 2));
                                            body.Append(tablamedgen);
                                        }


                                    }
                                    else
                                    {
                                        string saltosdelineamedg = medg.descripcion.Replace("[SALTO]", "\n");
                                        string[] lineasmedg = saltosdelineamedg.Split('\n');
                                        List<string> listaSaltosmedG = new List<string>();
                                        int contador = 1;

                                        foreach (string itemmedg in lineasmedg)
                                        {
                                            if (contador > 1)
                                            {
                                                listaSaltosmedG.Add(itemmedg);
                                            }
                                            else
                                            {
                                                AddBulletList(new List<string> { itemmedg }, wordDocument, body, "·", "720");
                                            }

                                            contador++;
                                        }

                                        AddBulletList(listaSaltosmedG, wordDocument, body, "-", "920");
                                        //AddBulletList(new List<string>() { medg.descripcion }, wordDocument, body, "·", "720");

                                    }


                                }
                            }
                        }

                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));
                        body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        var titMedGeneralesRiesgo = titular("4.2. MEDIDAS PREVENTIVAS ESPECÍFICAS", "22", "", "", "1", "", 1);
                        body.Append(titMedGeneralesRiesgo);

                        foreach (tipos_riesgos riesgo in Datos.ListarTiposRiesgosInerentesMedidas(centroseleccionado.id))
                        {

                            var titRiesgo = titular(riesgo.codigo.ToUpper(), "22", "052673", "", "1", "ffffff", 1);

                            body.Append(titRiesgo);


                            var titvarSitRiesgo = titular("Situaciones de Riesgo:", "20", "", "", "1", "", 2);
                            body.Append(titvarSitRiesgo);

                            foreach (riesgos_situaciones situacion in listasituaciones.Where(x => x.id_tipo_riesgo == riesgo.id))
                            {
                                bool contieneMedidaslistado = false;
                                if (listaMedidas.Where(x => x.id_situacion == situacion.id).Count() > 0)
                                {
                                    contieneMedidaslistado = true;
                                }

                                if (contieneMedidaslistado)
                                {
                                    var varSitRiesgo = titular(" " + riesgo.id + "." + situacion.descripcion, "20", "", "", "", "", 2);
                                    body.Append(varSitRiesgo);
                                }

                            }



                            foreach (medidas_apartadosV2 apart in apartados)
                            {
                                bool contieneApartado = false;
                                List<riesgos_medidas> medidasRiesgofiltradas2 = medidasRiesgo.Where(x => (x.id_centro == centroseleccionado.id || x.id_centro == 0) && x.id_riesgo == riesgo.id && x.id_apartado == apart.id).ToList();
                                foreach (riesgos_medidas r in medidasRiesgofiltradas2)
                                {
                                    if (r.id_apartado == apart.id)
                                    {
                                        contieneApartado = true;
                                    }
                                }


                                if (contieneApartado)
                                {

                                    var titApartGeneral = titular(apart.nombre, "20", "", "", "1", "298af0", 1);
                                    body.Append(titApartGeneral);

                                    foreach (riesgos_medidas medidar in medidasRiesgofiltradas2)
                                    {
                                        if (medidar.imagen_grande == null || medidar.imagen_grande == 0)
                                        {

                                            Paragraph MedidaGeneral = titular(medidar.descripcion, "20", "", "", "", "", 2);
                                            string rutaimagen = medidar.imagen;
                                            if (!string.IsNullOrEmpty(rutaimagen))
                                            {
                                                rutaimagen = rutaimagen.Replace("..", "");
                                                Table table = new Table();
                                                if (medidar.descripcion.Contains("[SALTO]"))
                                                {
                                                    string saltosdelinea = medidar.descripcion.Replace("[SALTO]", "\n");
                                                    string[] lineas = saltosdelinea.Split('\n');
                                                    List<string> listaSaltos = new List<string>();
                                                    foreach (string item in lineas)
                                                    {
                                                        listaSaltos.Add(item);
                                                    }
                                                    table = construirFilaBullet(listaSaltos, wordDocument, rutaimagen);

                                                }
                                                else
                                                {
                                                    table = construirFila(MedidaGeneral, wordDocument, rutaimagen);
                                                }

                                                body.Append(table);
                                                body.Append(titular("", "", "", "", "", "", 2));
                                            }
                                            else
                                            {
                                                string saltosdelinea = medidar.descripcion.Replace("[SALTO]", "\n");
                                                string[] lineas = saltosdelinea.Split('\n');
                                                List<string> listaSaltos = new List<string>();
                                                int contador = 1;
                                                foreach (string item in lineas)
                                                {
                                                    if (contador > 1)
                                                    {
                                                        listaSaltos.Add(item);
                                                    }
                                                    else
                                                    {
                                                        AddBulletList(new List<string> { item }, wordDocument, body, "·", "720");
                                                    }

                                                    contador++;
                                                }

                                                AddBulletList(listaSaltos, wordDocument, body, "-", "920");

                                            }
                                        }
                                        else
                                        {
                                            string rutaimagenGrande = medidar.imagen;
                                            if (rutaimagenGrande != null) rutaimagenGrande = rutaimagenGrande.Replace("..", "");

                                            string saltosdelinea = medidar.descripcion.Replace("[SALTO]", "\n");
                                            string[] lineas = saltosdelinea.Split('\n');
                                            List<string> listaSaltos = new List<string>();
                                            foreach (string item in lineas)
                                            {
                                                listaSaltos.Add(item);
                                            }
                                            if (rutaimagenGrande == null)
                                            {
                                                AddBulletList(listaSaltos, wordDocument, body, "·", "720");
                                            }
                                            else
                                            {
                                                //var textoPrueba = titular("IMAGENES:", "20", "", "", "1", "", 2);
                                                //body.Append(textoPrueba);
                                                string rutaimagenserver = Server.MapPath("~/" + rutaimagenGrande);
                                                if (System.IO.File.Exists(rutaimagenserver))
                                                {
                                                    if (!string.IsNullOrEmpty(medidar.descripcion))
                                                    {
                                                        //body.Append(construirFilaImagenGrande(listaSaltos, wordDocument, body, rutaimagenGrande));
                                                        body.Append(construirFilaImagenGrandeHorizontal(listaSaltos, wordDocument, body, rutaimagenGrande));
                                                        body.Append(titular("", "", "", "", "", "", 2));
                                                    }
                                                    else
                                                    {
                                                        body.Append(obtenerimagenMedidaRiesgoImagen(wordDocument, rutaimagenGrande, 600, 800));
                                                        body.Append(titular("", "", "", "", "", "", 2));
                                                    }
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(medidar.descripcion))
                                                    {
                                                        body.Append(construirFilaImagenGrande(listaSaltos, wordDocument, body, imagenblanco));
                                                        body.Append(titular("", "", "", "", "", "", 2));
                                                    }
                                                    else
                                                    {
                                                        body.Append(obtenerimagenMedidaRiesgoImagen(wordDocument, imagenblanco, 600, 800));
                                                        body.Append(titular("", "", "", "", "", "", 2));
                                                    }
                                                }

                                            }

                                        }


                                    }
                                }
                            }


                            //body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));

                            var listaSitu = listasituaciones.Where(x => x.id_tipo_riesgo == riesgo.id);

                            foreach (riesgos_situaciones situacion in listaSitu)
                            {
                                bool contieneMedidas = false;
                                if (listaMedidas.Where(x => x.id_situacion == situacion.id).Count() > 0)
                                {
                                    contieneMedidas = true;
                                }

                                if (contieneMedidas)
                                {

                                    var varSitRiesgo = titular((riesgo.id + "." + situacion.descripcion).ToUpper(), "21", "", "", "1", "", 1);
                                    body.Append(varSitRiesgo);

                                    List<string> listaMedidasCadena = new List<string>();
                                    var listaPersonalizada = listaMedidas.Where(x => x.id_situacion == situacion.id && (x.id_centro == 0 || x.id_centro == centroseleccionado.id));
                                    foreach (medidas_preventivas medidaspre in listaPersonalizada)
                                    {
                                        string imagenen_medidas_preventivas = Datos.ObtenerImagenSituacionMedida(medidaspre.id);
                                        List<submedidas_preventivas> listaSubMedidas = Datos.ListarSubMedidas(medidaspre.id);
                                                                                            
                                        if (string.IsNullOrEmpty(imagenen_medidas_preventivas) || imagenen_medidas_preventivas.Trim() == "")
                                        {
                                            if (listaSubMedidas.Count > 0)
                                            {
                                                List<string> listasubMedidasCadena = new List<string>();
                                                AddBulletList(new List<string>() { medidaspre.descripcion }, wordDocument, body, "·", "720");
                                                foreach (submedidas_preventivas submedd in listaSubMedidas)
                                                {
                                                    listasubMedidasCadena.Add(submedd.descripcion);
                                                }
                                                AddBulletList(listasubMedidasCadena, wordDocument, body, "-", "920");
                                                body.Append(titular("", "", "", "", "", "", 2));

                                            }
                                            else
                                            {
                                                listaMedidasCadena.Add(medidaspre.descripcion);
                                            }

                                        }
                                        else
                                        {
                                            if (Datos.EsImagenGrandeMedida(medidaspre.id))
                                            {

                                                string saltosdelinea = medidaspre.descripcion.Replace("[SALTO]", "\n");
                                                string[] lineas = saltosdelinea.Split('\n');
                                                List<string> listaSaltos = new List<string>();
                                                foreach (string item in lineas)
                                                {
                                                    listaSaltos.Add(item);
                                                }

                                                if (!string.IsNullOrEmpty(medidaspre.descripcion))
                                                {

                                                    //body.Append(construirFilaImagenGrande(listaSaltos, wordDocument, body, imagenen_medidas_preventivas.Replace("..", "")));
                                                    body.Append(construirFilaImagenGrandeHorizontal(listaSaltos, wordDocument, body, imagenen_medidas_preventivas.Replace("..", "")));
                                                    body.Append(titular("", "", "", "", "", "", 2));
                                                }
                                                else
                                                {
                                                    body.Append(obtenerimagenMedidaRiesgoImagen(wordDocument, imagenen_medidas_preventivas.Replace("..", ""), 600, 800));
                                                    body.Append(titular("", "", "", "", "", "", 2));
                                                }
                                            }
                                            else
                                            {
                                                if (listaSubMedidas.Count > 0)
                                                {
                                                    List<string> listasubMedidasCadena = new List<string>();
                                                    listasubMedidasCadena.Add(medidaspre.descripcion);
                                                    foreach (submedidas_preventivas submedd in listaSubMedidas)
                                                    {
                                                        listasubMedidasCadena.Add(submedd.descripcion);
                                                    }
                                                    Table table = construirFilaBullet(listasubMedidasCadena, wordDocument, imagenen_medidas_preventivas.Replace("..", ""));
                                                    body.Append(table);
                                                    body.Append(titular("", "", "", "", "", "", 2));
                                                }
                                                else
                                                {
                                                    Paragraph medidaDescripcion = titular(medidaspre.descripcion, "20", "", "", "", "", 2);
                                                    Table table = construirFila(medidaDescripcion, wordDocument, imagenen_medidas_preventivas.Replace("..", ""));
                                                    body.Append(table);
                                                    body.Append(titular("", "", "", "", "", "", 2));

                                                }
                                            }


                                        }
                                    }
                                    if (listaMedidasCadena.Count > 0)
                                    {
                                        AddBulletList(listaMedidasCadena, wordDocument, body, "·", "720");
                                    }
                                    //body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.TextWrapping })));
                                }

                            }

                            //Salto de Pagina
                            body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        }

                        //FIN DEL DOCUMENTO
                        documento.Append(body);

                        //if (documentSettingsPart != null)
                        //{
                        //    var documentProtection = documentSettingsPart.Settings.Elements<DocumentProtection>().FirstOrDefault();

                        //    if (documentProtection != null)
                        //    {
                        //        documentProtection.Enforcement = true;
                        //    }
                        //    else
                        //    {
                        //        documentProtection = new DocumentProtection() { Edit = DocumentProtectionValues.Comments, Enforcement = true, CryptographicProviderType = CryptProviderValues.RsaFull, CryptographicAlgorithmClass = CryptAlgorithmClassValues.Hash, CryptographicAlgorithmType = CryptAlgorithmValues.TypeAny, CryptographicAlgorithmSid = 4, CryptographicSpinCount = (UInt32Value)100000U, Hash = "2krUoz1qWd0WBeXqVrOq81l8xpk=", Salt = "9kIgmDDYtt2r5U2idCOwMA==" };
                        //        documentSettingsPart.Settings.Append(documentProtection);
                        //    }
                        //}

                        // InsertCustomWatermark(wordDocument, stream);
                        wordDocument.MainDocumentPart.Document = documento;
                        //WordprocessingDocument a = (WordprocessingDocument)wordDocument.SaveAs("C:\\users\\public\\temp.docx");

                        //BLOQUEAR DOCUMENTO

                        mp.AddNewPart<DocumentSettingsPart>();
                        DocumentSettingsPart documentSettingsPart = wordDocument.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().FirstOrDefault();
                        DocumentProtection documentProtection = new DocumentProtection();
                        documentProtection.Edit = DocumentProtectionValues.None;
                        Settings conf = new Settings();
                        documentSettingsPart.Settings = conf;
                        documentSettingsPart.Settings.Append(documentProtection);
                        mp.DocumentSettingsPart.Settings.Save();

                        //  BLOQUEAR DOCUMENTO

                        if (!esborrador)
                        {
                            documento.Save();
                            wordDocument.SaveAs(Server.MapPath("~/Content/ficheros/" + "DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx")).Close();

                            documento_historico oDocumento = new documento_historico();
                            oDocumento.nombre = ("DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx");
                            oDocumento.tipo = ".docx";
                            oDocumento.version = version;
                            oDocumento.estado = 1;
                            oDocumento.fechaUltimaModificacion = DateTime.Now.ToString();
                            oDocumento.usuario = Session["usuario"].ToString();
                            oDocumento.descarga = esborrador ? 0 : 1;
                            oDocumento.ruta = "../Content/ficheros/";
                            oDocumento.id_centro = centroseleccionado.id;
                            oDocumento.revision = NumeroRevision(centroseleccionado.id);
                            oDocumento.descripcion = nombredocumento;

                            var id_oDocumento = Datos.InsertarDocumentoHistorico(oDocumento);
                        }
                        else
                        {

                            documento_historico oDocumento = new documento_historico();
                            oDocumento.nombre = ("DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx");
                            oDocumento.tipo = ".docx";
                            oDocumento.version = version;
                            oDocumento.estado = 1;
                            oDocumento.fechaUltimaModificacion = DateTime.Now.ToString();
                            oDocumento.usuario = Session["usuario"].ToString();
                            oDocumento.descarga = esborrador ? 0 : 1;
                            oDocumento.ruta = "../Content/ficheros/";
                            oDocumento.id_centro = centroseleccionado.id;
                            oDocumento.revision = NumeroRevision(centroseleccionado.id);
                            oDocumento.descripcion = nombredocumento;

                            var id_oDocumento = Datos.InsertarDocumentoHistorico(oDocumento);

                        }





                        wordDocument.Dispose();

                        stream.Dispose();
                        stream.Close();

                    }



                    stream.Dispose();
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DocmentoRiesgosXML_" + centroseleccionado.nombre + "__" + version + ".docx");


                }

            }
            catch (Exception ex)
            {

                new EscribirLog("Error: " +
                            ex.Message, true, this.ToString(), "GenerarDocumento");
                Session["GenerarDocumento"] = "Se ha producido un error al generar el documento.";
                //return RedirectToAction("Principal", "Home");
                return RedirectToAction("GenerarDocumentoRiesgos", "DocumentoRiesgos");
            }

        }



        private string ParseDOCX(FileInfo fileInfo)
        {
            try
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(fileInfo.FullName);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(byteArray, 0, byteArray.Length);
                    using (WordprocessingDocument wDoc =
                                              WordprocessingDocument.Open(memoryStream, true))
                    {
                        int imageCounter = 0;


                        PWT.WmlToHtmlConverterSettings settings = new PWT.WmlToHtmlConverterSettings()
                        {
                            AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                            FabricateCssClasses = true,
                            CssClassPrefix = "pt-",
                            RestrictToSupportedLanguages = false,
                            RestrictToSupportedNumberingFormats = false,
                            ImageHandler = imageInfo =>
                            {
                                ++imageCounter;
                                string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                                ImageFormat imageFormat = null;
                                if (extension == "png") imageFormat = ImageFormat.Png;
                                else if (extension == "gif") imageFormat = ImageFormat.Gif;
                                else if (extension == "bmp") imageFormat = ImageFormat.Bmp;
                                else if (extension == "jpeg") imageFormat = ImageFormat.Jpeg;
                                else if (extension == "tiff")
                                {
                                    extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                }
                                else if (extension == "x-wmf")
                                {
                                    extension = "wmf";
                                    imageFormat = ImageFormat.Wmf;
                                }

                                if (imageFormat == null) return null;

                                string base64 = null;
                                try
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        imageInfo.Bitmap.Save(ms, imageFormat);
                                        var ba = ms.ToArray();
                                        base64 = System.Convert.ToBase64String(ba);
                                    }
                                }
                                catch (System.Runtime.InteropServices.ExternalException)
                                { return null; }

                                ImageFormat format = imageInfo.Bitmap.RawFormat;
                                ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders()
                                                          .First(c => c.FormatID == format.Guid);
                                string mimeType = codec.MimeType;

                                string imageSource =
                                       string.Format("data:{0};base64,{1}", mimeType, base64);

                                XElement img = new XElement(PWT.Xhtml.img,
                                      new XAttribute(PWT.NoNamespace.src, imageSource),
                                      imageInfo.ImgStyleAttribute,
                                      imageInfo.AltText != null ?
                                           new XAttribute(PWT.NoNamespace.alt, imageInfo.AltText) : null);
                                return img;
                            }
                        };

                        XElement htmlElement = PWT.WmlToHtmlConverter.ConvertToHtml(wDoc, settings);
                        var html = new XDocument(new XDocumentType("html", null, null, null), htmlElement);
                        var htmlString = html.ToString(SaveOptions.DisableFormatting);
                        return htmlString;
                    }
                }
            }
            catch
            {
                return "File contains corrupt data";
            }
        }
        public Shading colorFondo(string color)
        {
            Shading shading = new Shading()
            {
                Color = "auto",
                Fill = color,
                Val = ShadingPatternValues.Clear
            };
            return shading;
        }

        private static Paragraph AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, int ancho, int alto)
        {
            // Define the reference of the image.
            Size size = new Size(ancho, alto);

            Int64Value width = size.Width * 9525;
            Int64Value height = size.Height * 9525;

            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = width, Cy = height },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = false }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            return new Paragraph(new Run(element), new ParagraphProperties(new Justification() { Val = JustificationValues.Center }));
        }
        private static Paragraph AddImageToBodyImagenGrande(WordprocessingDocument wordDoc, string relationshipId, int ancho, int alto)
        {
            // Define the reference of the image.
            Size size = new Size(ancho, alto);


            Int64Value width = size.Width * 9525;
            Int64Value height = size.Height * 4752;

            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = width, Cy = height },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = false }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = width, Cy = height }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            return new Paragraph(new Run(element), new ParagraphProperties(new Justification() { Val = JustificationValues.Center }));
        }

        public Table construirTabla(int centro)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                           )
                       );

            table.AppendChild<TableProperties>(tblProp);
            TableRow trCabecera = new TableRow(
                        //new TableRowProperties(
                        //    new TableRowHeight() { Val = Convert.ToUInt32("1") }),
                        //new TableCellWidth() { Width = "3", Type = TableWidthUnitValues.Auto },
                        new TableCell(
                            new TableCellProperties(

                                new Shading { Fill = "87b9ed" },
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                                ),
                           titular("Revisión/Fecha", "21", "", "centrar", "1", "", 2)

                        ),
                        new TableCell(
                            new TableCellProperties(
                                new Shading { Fill = "87b9ed" },
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center },
                                new TableCellMargin() { }
                                ), new ParagraphProperties(new Indentation() { Left = "100" }),
                           titular("Modificaciones", "21", "", "", "1", "", 2)

                        )
                        );


            table.Append(trCabecera);

            List<documentos_riesgos> lista = Datos.ObtenerDocumentosRiesgos(centro);
            int i = 0;
            foreach (documentos_riesgos riesgo in lista)
            {


                TableRow tr = new TableRow();
                TableCell tc1 = new TableCell();
                tc1.Append(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }, new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "31" }, new Justification() { Val = JustificationValues.Center }));
                //tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text("Rev ." + i.ToString()), new Break(), new Text(riesgo.fechageneracion.ToString("dd/MM/yyyy")))));
                tc1.Append(titular("Rev ." + i.ToString(), "21", "", "centrar", "", "", 2));
                tc1.Append(titular(riesgo.fechageneracion.ToString("dd/MM/yyyy"), "21", "", "centrar", "", "", 2));
                tr.Append(tc1);
                TableCell tc2 = new TableCell();
                tc2.Append(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }, new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }, new Justification() { Val = JustificationValues.Center }));

                string saltosdelinea = riesgo.descripcion.Replace("-salto-", "\n");
                string[] lineas = saltosdelinea.Split('\n');
                if (lineas.Count() > 1)
                {
                    foreach (string linea in lineas)
                    {
                        tc2.Append(new ParagraphProperties(new Indentation() { Left = "100" }), titular(linea, "20", "", "", "", "", 2));
                        //tc2.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(linea))));
                    }
                }
                else
                {

                    tc2.Append(new ParagraphProperties(new Indentation() { Left = "100" }), titular(saltosdelinea, "20", "", "", "", "", 2));
                    //tc2.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(saltosdelinea))));
                }
                tr.Append(tc2);
                table.Append(tr);
                i++;
            }
            return table;
        }
        public Table construirFila(Paragraph medida, WordprocessingDocument wordDocument, string rutaimagen)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                           ),
                           new TableCellMarginDefault(
                                new TopMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new StartMargin() { Width = "200", Type = TableWidthUnitValues.Dxa },
                                new BottomMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new EndMargin() { Width = "50", Type = TableWidthUnitValues.Dxa })
                       );
            TableCell celdaimagen = new TableCell(new ParagraphProperties(
                new Justification() { Val = JustificationValues.Center },
               new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }
                ), obtenerimagenMedidaRiesgo(wordDocument, rutaimagen));
            TableCellProperties prpceldaImagen = new TableCellProperties(new TableCellWidth() { Width = "40" },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

            celdaimagen.Append(prpceldaImagen);
            table.AppendChild<TableProperties>(tblProp);
            table.Append(new TableRow(celdaimagen,
                                new TableCell(new TableCellProperties(new TableCellVerticalAlignment()
                                { Val = TableVerticalAlignmentValues.Center }),
                                    new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Center }

                                ), medida)));
            return table;
        }
        public Table construirFilaBullet(List<string> listaCadena, WordprocessingDocument wordDocument, string rutaimagen)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                           ),
                           new TableCellMarginDefault(
                                new TopMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new StartMargin() { Width = "200", Type = TableWidthUnitValues.Dxa },
                                new BottomMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new EndMargin() { Width = "50", Type = TableWidthUnitValues.Dxa })
                       );
            TableCell celdaimagen = new TableCell(new ParagraphProperties(
                new Justification() { Val = JustificationValues.Center },
               new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }
                ), obtenerimagenMedidaRiesgo(wordDocument, rutaimagen));
            TableCellProperties prpceldaImagen = new TableCellProperties(new TableCellWidth() { Width = "40" },
                        new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

            celdaimagen.Append(prpceldaImagen);
            table.AppendChild<TableProperties>(tblProp);
            TableCell celdaTexto = new TableCell(new TableCellProperties(new TableCellVerticalAlignment()
            { Val = TableVerticalAlignmentValues.Center }),
                                    new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Center }

                                ));
            if (listaCadena.Count > 1)
            {
                if (listaCadena[0].Trim() != "")
                {
                    celdaTexto.Append(titular(listaCadena[0], "18", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
                }

                listaCadena.RemoveAt(0);
                AddBulletListTabla(ListOfStringToRunList(listaCadena), wordDocument, celdaTexto, ".", "720");
            }
            else
            {
                celdaTexto.Append(titular(listaCadena[0], "18", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
            }


            table.Append(new TableRow(celdaimagen, celdaTexto));

            //AddBulletListTabla(ListOfStringToRunList(listaCadena), wordDocument, celdaTexto, ".", "720");
            //table.Append(new TableRow(celdaimagen, celdaTexto));
            return table;
        }

        public Table construirFilaImagenGrande(List<string> medidas, WordprocessingDocument wordDocument, Body body, string rutaimagen)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                           ),
                           new TableCellMarginDefault(
                                new TopMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new StartMargin() { Width = "200", Type = TableWidthUnitValues.Dxa },
                                new BottomMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new EndMargin() { Width = "50", Type = TableWidthUnitValues.Dxa })
                       );
            TableCell celdaimagen = new TableCell(new ParagraphProperties(
                new Justification() { Val = JustificationValues.Center },
               new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }
                ), obtenerimagenMedidaRiesgoImagen(wordDocument, rutaimagen, 220, 220));
            TableCellProperties prpceldaImagen = new TableCellProperties(new TableCellWidth() { Width = "40" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

            celdaimagen.Append(prpceldaImagen);
            table.AppendChild<TableProperties>(tblProp);

            TableCell celdaTexto = new TableCell(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                    new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Left }

                                ));
            var indentation = new Indentation() { Left = "260", Hanging = "360", Right = "260" };
            //celdaTexto.Append(new Paragraph(new Run(new Text(medidas[0]))), new ParagraphProperties(new Indentation() { Left = "160", Hanging = "160", Right = "160" }));
            if (medidas.Count > 1)
            {
                celdaTexto.Append(titular(medidas[0], "20", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
                medidas.RemoveAt(0);
                AddBulletListTabla(ListOfStringToRunList(medidas), wordDocument, celdaTexto, ".", "720");
            }
            else
            {
                celdaTexto.Append(titular(medidas[0], "20", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
            }


            table.Append(new TableRow(celdaimagen, celdaTexto));


            return table;
        }

        public Table construirFilaImagenGrandeHorizontal(List<string> medidas, WordprocessingDocument wordDocument, Body body, string rutaimagen)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                           ),
                           new TableCellMarginDefault(
                                new TopMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new StartMargin() { Width = "200", Type = TableWidthUnitValues.Dxa },
                                new BottomMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                                new EndMargin() { Width = "50", Type = TableWidthUnitValues.Dxa })
                       );

            var rowImagenGrande = new TableRow();

            TableCell celdaimagen = new TableCell(new ParagraphProperties(
                new Justification() { Val = JustificationValues.Center },
               new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }
                ), obtenerimagenMedidaRiesgoImagen(wordDocument, rutaimagen, 220, 220));
            TableCellProperties prpceldaImagen = new TableCellProperties(new TableCellWidth() { Width = "40" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

            celdaimagen.Append(prpceldaImagen);
            rowImagenGrande.Append(celdaimagen);
            table.AppendChild<TableProperties>(tblProp);

            var rowTexto = new TableRow();
            TableCell celdaTexto = new TableCell(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                    new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Left }

                                ));
            var indentation = new Indentation() { Left = "260", Hanging = "360", Right = "260" };
            //celdaTexto.Append(new Paragraph(new Run(new Text(medidas[0]))), new ParagraphProperties(new Indentation() { Left = "160", Hanging = "160", Right = "160" }));
            if (medidas.Count > 1)
            {
                celdaTexto.Append(titular(medidas[0], "20", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
                medidas.RemoveAt(0);
                AddBulletListTabla(ListOfStringToRunList(medidas), wordDocument, celdaTexto, ".", "720");
            }
            else
            {
                celdaTexto.Append(titular(medidas[0], "20", "", "", "", "", 2), new ParagraphProperties(new Indentation() { Left = "260", Hanging = "260", Right = "260" }));
            }

            rowTexto.Append(celdaTexto);
            table.Append(rowTexto);
            table.Append(rowImagenGrande);


            return table;
        }

        public ParagraphProperties EstilosTextoCabecera()
        {
            return new ParagraphProperties(
                 new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto },
                    new Justification() { Val = JustificationValues.Center },
                    new Spacing() { Val = 10 }
                );
        }
        //public ParagraphProperties EstilosTextoCabeceraFontSize()
        //{
        //    return new ParagraphProperties(
        //         new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto },
        //            new Justification() { Val = JustificationValues.Center },
        //            new Spacing() { Val = 10 },
        //            new FontSize { Val = "18" },
        //            new FontSizeComplexScript { Val = "18" },
        //        new Run(
        //            new RunProperties(
        //            new FontSize { Val = "18" },
        //            new FontSizeComplexScript { Val = "18" })
        //            )
        //        );
        //}
        public TableCellProperties EstilosCeldasCabecera()
        {
            return new TableCellProperties(
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                );
        }

        public Table construirTablaCabecera(Paragraph image, string centro, string version, WordprocessingDocument wordoc,
                                                Paragraph pagina)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                           new TableBorders(
                               new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                               new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct },
                               new TextAlignment() { Val = VerticalTextAlignmentValues.Center }
                           ),

                           new TableJustification() { Val = TableRowAlignmentValues.Center }

                       );

            table.AppendChild<TableProperties>(tblProp);
            //filas
            TableRow tr1 = new TableRow(new TableRowProperties(new TableRowHeight() { Val = Convert.ToUInt32("1") }));
            TableRow tr2 = new TableRow(new TableRowProperties(new TableRowHeight() { Val = Convert.ToUInt32("1") }));
            TableRow tr3 = new TableRow(new TableRowProperties(new TableRowHeight() { Val = Convert.ToUInt32("1") }));

            ImagePart imagePart = wordoc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);

            string rutalocal = Server.MapPath("~/Imagenes/endesa.PNG");
            // string rutalocal = "C://Users/jose.pinto/Documents/REPOSITORIO/REPOSITORIO_NOVOTEC/DIMASST/Midas/Imagenes/endesa.PNG";
            using (FileStream flujoimagen = new FileStream(rutalocal, FileMode.Open))
            {
                imagePart.FeedData(flujoimagen);
            }

            var centroId = Datos.ObtenerCentroPorNombre(centro);
            int numeroRevision = 0;
            if (NumeroRevision(centroId.id) != 0)
            {
                numeroRevision = NumeroRevision(centroId.id);
            }

            TableCell CellCabecera1 = new TableCell(EstilosTextoCabecera(), image);
            TableCell CellCabecera2 = new TableCell(EstilosTextoCabecera(), titular("RIESGOS INHERENTES Y MEDIDAS PREVENTIVAS", "23", "", "centrar", "1", "", 2));

            TableCell CellCabecera3 = new TableCell(EstilosTextoCabecera(), new Paragraph(
                                                                                            new Run(new RunProperties(new FontSize() { Val = "22" }),
                                                                                            new Text("Revisión: " + numeroRevision))));

            TableCell tc1 = new TableCell(new Paragraph());
            TableCell tc2 = new TableCell(new Paragraph());
            TableCell tc3 = new TableCell(EstilosTextoCabecera(), new Paragraph(
                                                new Run(new RunProperties(new FontSize() { Val = "22" })
                                                , new Run(new Text(DateTime.Now.ToString("dd/MM/yyyy"))))));
            TableCell tc4 = new TableCell(new Paragraph());
            TableCell tc5 = new TableCell(EstilosTextoCabecera(), new Paragraph(new Run(new Text(centro))));

            //Run run = new Run();
            //Text txt = new Text("txt");

            //RunProperties runProps = new RunProperties();
            //FontSize fontSize = new Fontsize() { Val = "18" }; // font size 9

            //runProps.Append(fontSize);

            //run.Append(runProps);
            //run.Append(txt);

            //para.Append(run);

            TableCell tc6 = new TableCell(EstilosTextoCabecera(), pagina);



            //UNIR todas las celdas de la primera columna
            CellCabecera1.TableCellProperties = new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            CellCabecera1.TableCellProperties.VerticalMerge = new VerticalMerge { Val = MergedCellValues.Restart };
            tc1.TableCellProperties = new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            tc1.TableCellProperties.VerticalMerge = new VerticalMerge { Val = MergedCellValues.Continue };
            tc4.TableCellProperties = new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            tc4.TableCellProperties.VerticalMerge = new VerticalMerge { Val = MergedCellValues.Continue };

            //unir celdas de la segunda columna
            CellCabecera2.TableCellProperties = new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            CellCabecera2.TableCellProperties.VerticalMerge = new VerticalMerge { Val = MergedCellValues.Restart };
            tc2.TableCellProperties = new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            tc2.TableCellProperties.VerticalMerge = new VerticalMerge { Val = MergedCellValues.Continue };

            tr1.Append(CellCabecera1);
            tr1.Append(CellCabecera2);
            tr1.Append(CellCabecera3);
            tr2.Append(tc1);
            tr2.Append(tc2);
            tr2.Append(tc3);
            tr3.Append(tc4);
            tr3.Append(tc5);
            tr3.Append(tc6);
            table.Append(tr1);
            table.Append(tr2);
            table.Append(tr3);

            return table;
        }

        private static int NumeroRevision(int centroId)
        {
            List<documentos_riesgos> lista = Datos.ObtenerDocumentosRiesgos(centroId);
            var ultimo = new documentos_riesgos();
            int numeroRevision = 0;

            if (lista.Count > 0)
            {
                ultimo = lista.Last();
                numeroRevision = lista.IndexOf(ultimo);
            }

            return numeroRevision;
        }

        public string MarcaMatriz(List<matriz_centro> matriz, int idar, int nivel, int riesgo)
        {
            string resultado = "-";
            bool esactivo = false;
            switch (nivel)
            {
                case 1:
                    foreach (matriz_centro activos in matriz)
                    {
                        if (activos.id_areanivel1 == idar && activos.id_riesgo == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;
                case 2:
                    foreach (matriz_centro activos in matriz)
                    {
                        if (activos.id_areanivel2 == idar && activos.id_riesgo == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }
                    break;
                case 3:
                    foreach (matriz_centro activos in matriz)
                    {
                        if (activos.id_areanivel3 == idar && activos.id_riesgo == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;
                case 4:
                    foreach (matriz_centro activos in matriz)
                    {
                        if (activos.id_areanivel4 == idar && activos.id_riesgo == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;

                default:
                    resultado = "-";
                    break;

            }
            return resultado;
        }

        public string MarcaMatrizCritico(List<matriz_centro_critico> matriz, int idar, int nivel, int riesgo)
        {
            string resultado = "-";
            bool esactivo = false;
            switch (nivel)
            {
                case 1:
                    foreach (matriz_centro_critico activos in matriz)
                    {
                        if (activos.id_areanivel1 == idar && activos.id_riesgoCritico == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;
                case 2:
                    foreach (matriz_centro_critico activos in matriz)
                    {
                        if (activos.id_areanivel2 == idar && activos.id_riesgoCritico == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }
                    break;
                case 3:
                    foreach (matriz_centro_critico activos in matriz)
                    {
                        if (activos.id_areanivel3 == idar && activos.id_riesgoCritico == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;
                case 4:
                    foreach (matriz_centro_critico activos in matriz)
                    {
                        if (activos.id_areanivel4 == idar && activos.id_riesgoCritico == riesgo)
                        {
                            esactivo = true;
                        }
                    }
                    if (esactivo)
                    {
                        resultado = "X";
                    }
                    else
                    {
                        resultado = "-";
                    }

                    break;

                default:
                    resultado = "-";
                    break;

            }
            return resultado;
        }

        public Table GettablaMatrizCritico(int centro)
        {
            //TABLA DE RIESGOS

            Table tablaRiesgos = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new CantSplit() { Val = OnOffOnlyValues.On }
                )
            );

            tablaRiesgos.AppendChild<TableProperties>(tblProp);
            TableHeader Cabecera = new TableHeader();

            TableRow trCabecera = new TableRow();
            TableCell CellCabecera1 = new TableCell();
            ColorXML colorle = new ColorXML() { Val = "ffffff" };

            //CellCabecera1.Append(new TableCellProperties(new Shading { Fill = "87b9ed" }));
            CellCabecera1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));

            //CellCabecera1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Áreas - Riesgos"))));
            CellCabecera1.Append(titular("Áreas - Riesgos", "21", "", "izquierda", "1", "", 2));
            trCabecera.Append(CellCabecera1);


            List<tipo_riesgos_critico> listaRiesgos = Datos.ListarTiposRiesgosCriticos();
            foreach (tipo_riesgos_critico riesgo in listaRiesgos)
            {

                TableCell CabeceraRiesgo = new TableCell();
                CabeceraRiesgo.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                CabeceraRiesgo.Append();
                CabeceraRiesgo.Append(titular((riesgo.id > 10) ? riesgo.id.ToString() : " " + riesgo.id, "21", "", "centrar", "1", "", 2));
                trCabecera.Append(CabeceraRiesgo);

            }
            trCabecera.Append(new TableHeader());
            tablaRiesgos.Append(trCabecera);

            List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
            List<medidas_preventivas> listaMedidas = Datos.ListarMedidas();
            List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centro);
            List<medidas_apartadosV2> apartados = Datos.ListarApartadosV2();
            int version = Datos.obtenerUltimaVersionCentro(centro);
            List<matriz_centro_critico> matriz = Datos.listarMatrizCentroUltimaVersionCritico(centro);
            matriz = matriz.Where(x => x.activo == true).ToList();
            int contadorarea1 = 1;
            foreach (areanivel1 area1 in Datos.ListarAreas(version))
            {

                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();


                tc1.Append(new TableCellProperties(new Shading { Fill = "2f5496" }, new Color { Val = "FFFFFFFF" }));
                tc1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                // tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(area1.codigo + "." + area1.nombre))));
                tc1.Append(titular(contadorarea1 + ". " + area1.nombre, "21", "", "izquierda", "1", "FFFFFF", 2));
                tr1.Append(tc1, new CantSplit());
                foreach (tipo_riesgos_critico riesgos in listaRiesgos)
                {
                    TableCell DatoRiesgo1 = new TableCell();
                    DatoRiesgo1.Append(new TableCellProperties(new Shading { Fill = "2f5496" }));
                    DatoRiesgo1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                    DatoRiesgo1.Append(titular(MarcaMatrizCritico(matriz, area1.id, 1, riesgos.id), "21", "", "centrar", "1", "FFFFFF", 2));
                    tr1.Append(DatoRiesgo1);
                }
                tablaRiesgos.Append(tr1);

                int contadorarea2 = 1;
                foreach (areanivel2 area2 in Datos.ListarSistema().Where(x => x.id_areanivel1 == area1.id))
                {

                    TableRow tr2 = new TableRow();
                    TableCell tc2 = new TableCell();

                    tc2.Append(new TableCellProperties(new Shading { Fill = "b4c6e7" }, new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                    tc2.Append(titular(contadorarea1 + "." + contadorarea2 + ". " + area2.nombre, "21", "", "izquierda", "1", "", 2));
                    tr2.Append(tc2, new CantSplit());
                    foreach (tipo_riesgos_critico riesgos in listaRiesgos)
                    {
                        TableCell DatoRiesgo2 = new TableCell();
                        DatoRiesgo2.Append(new TableCellProperties(new Shading { Fill = "b4c6e7" }));
                        DatoRiesgo2.Append(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" },
                        new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                        ));

                        DatoRiesgo2.Append(titular(MarcaMatrizCritico(matriz, area2.id, 2, riesgos.id), "21", "", "centrar", "1", "", 2));
                        tr2.Append(DatoRiesgo2);

                    }

                    tablaRiesgos.Append(tr2);
                    int contadorarea3 = 1;
                    foreach (areanivel3 area3 in Datos.ListarEquipos().Where(x => x.id_areanivel2 == area2.id))
                    {

                        TableRow tr3 = new TableRow();
                        TableCell tc3 = new TableCell();

                        tc3.Append(new TableCellProperties(new Shading { Fill = "f4b083" }));
                        tc3.Append(new TableCellProperties(
                            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                        tc3.Append(titular(contadorarea1 + "." + contadorarea2 + "." + contadorarea3 + ". " + area3.nombre, "21", "", "izquierda", "1", "", 2));
                        tr3.Append(tc3, new CantSplit());

                        foreach (tipo_riesgos_critico riesgos in listaRiesgos)
                        {
                            TableCell DatoRiesgo3 = new TableCell();
                            DatoRiesgo3.Append(new TableCellProperties(new Shading { Fill = "f4b083" }));
                            DatoRiesgo3.Append(new TableCellProperties(
                            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" },
                        new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                            DatoRiesgo3.Append(titular(MarcaMatrizCritico(matriz, area3.id, 3, riesgos.id), "21", "", "centrar", "1", "", 2));
                            tr3.Append(DatoRiesgo3);
                        }
                        tablaRiesgos.Append(tr3);
                        int contadorarea4 = 1;
                        foreach (areanivel4 area4 in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == area3.id))
                        {

                            TableRow tr4 = new TableRow();
                            TableCell tc4 = new TableCell();

                            tc4.Append(new TableCellProperties(new Shading { Fill = "fbe4d5" }));
                            tc4.Append(new TableCellProperties(
                                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                            tc4.Append(titular(contadorarea1 + "." + contadorarea2 + "." + contadorarea3 + "." + contadorarea3 + ". " + area4.nombre, "21", "", "izquierda", "1", "", 2));
                            tr4.Append(tc4, new CantSplit());

                            foreach (tipo_riesgos_critico riesgos in listaRiesgos)
                            {
                                TableCell DatoRiesgo4 = new TableCell();
                                DatoRiesgo4.Append(new TableCellProperties(new Shading { Fill = "fbe4d5" }));
                                DatoRiesgo4.Append(new TableCellProperties(
                                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" },
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                                DatoRiesgo4.Append(titular(MarcaMatrizCritico(matriz, area4.id, 4, riesgos.id), "21", "", "centrar", "1", "", 2));
                                tr4.Append(DatoRiesgo4);
                            }
                            tablaRiesgos.Append(tr4);
                            contadorarea4++;
                        }
                        contadorarea3++;
                    }
                    contadorarea2++;
                }


                contadorarea1++;
            }
            return tablaRiesgos;
        }

        public Table GettablaMatriz(int centro)
        {
            //TABLA DE RIESGOS

            Table tablaRiesgos = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new CantSplit() { Val = OnOffOnlyValues.On }
                )
            );

            tablaRiesgos.AppendChild<TableProperties>(tblProp);
            TableHeader Cabecera = new TableHeader();

            TableRow trCabecera = new TableRow();
            TableCell CellCabecera1 = new TableCell();
            ColorXML colorle = new ColorXML() { Val = "ffffff" };

            //CellCabecera1.Append(new TableCellProperties(new Shading { Fill = "87b9ed" }));
            CellCabecera1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));

            //CellCabecera1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Áreas - Riesgos"))));
            CellCabecera1.Append(titular("Áreas - Riesgos", "18", "", "izquierda", "1", "", 2));
            trCabecera.Append(CellCabecera1);


            List<tipos_riesgos> listaRiesgos = Datos.ListarRiesgos();
            foreach (tipos_riesgos riesgo in listaRiesgos)
            {

                TableCell CabeceraRiesgo = new TableCell();
                CabeceraRiesgo.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                CabeceraRiesgo.Append();
                CabeceraRiesgo.Append(titular((riesgo.id > 10) ? riesgo.id.ToString() : " " + riesgo.id, "18", "", "centrar", "1", "", 2));
                trCabecera.Append(CabeceraRiesgo);

            }
            trCabecera.Append(new TableHeader());
            tablaRiesgos.Append(trCabecera);

            List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
            List<medidas_preventivas> listaMedidas = Datos.ListarMedidas();
            List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centro);
            List<medidas_apartadosV2> apartados = Datos.ListarApartadosV2();
            int version = Datos.obtenerUltimaVersionCentro(centro);
            List<matriz_centro> matriz = Datos.listarMatrizCentroUltimaVersion(centro);
            matriz = matriz.Where(x => x.activo == true).ToList();
            int contadorarea1 = 1;
            int lastTec = 0;

            List<tecnologias> listaTec = Datos.ListarTecnologiasPorVersion(centro, version);
            var listaInversa = listaTec.OrderByDescending(x => x.id);


            //var listaInversa = matriz.GroupBy(x => x.id_tecnologia).Select(grp => grp.First()).OrderByDescending(y => y.id);
            //var listaInversa = matriz.GroupBy(x => x.id_tecnologia).Select(grp => grp.First());

            //var listaAreas = Datos.ListarAreas(version);

            foreach (var listaTecno in listaInversa)            {
                var listaAreas = Datos.ListarAreasPorVersionYTecnologia(version, listaTecno.id);
                foreach (areanivel1 area1 in listaAreas)
                {
                    if (lastTec != area1.id_tecnologia)
                    {
                        #region tecnologia
                        string tecnologia = Datos.ObtenerTecnologia(area1.id_tecnologia).nombre;

                        TableRow trTec = new TableRow();
                        TableCell tcTec = new TableCell();


                        tcTec.Append(new TableCellProperties(new Shading { Fill = "ff0f64" }, new Color { Val = "FFFFFFFF" }));
                        tcTec.Append(new TableCellProperties(
                            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                        // tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(area1.codigo + "." + area1.nombre))));
                        tcTec.Append(titular(tecnologia, "22", "", "centrar", "1", "FFFFFF", 2));
                        trTec.Append(tcTec, new CantSplit());
                        foreach (tipos_riesgos riesgos in listaRiesgos)
                        {
                            TableCell DatoRiesgoTec = new TableCell();
                            DatoRiesgoTec.Append(new TableCellProperties(new Shading { Fill = "ff0f64" }));
                            DatoRiesgoTec.Append(new TableCellProperties(
                            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                            DatoRiesgoTec.Append(titular(" ", "21", "", "centrar", "1", "FFFFFF", 2));
                            trTec.Append(DatoRiesgoTec);
                        }
                        tablaRiesgos.Append(trTec);
                        lastTec = (int)area1.id_tecnologia;
                        #endregion
                    }


                    TableRow tr1 = new TableRow();
                    TableCell tc1 = new TableCell();


                    tc1.Append(new TableCellProperties(new Shading { Fill = "2f5496" }, new Color { Val = "FFFFFFFF" }));
                    tc1.Append(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));

                    // tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(area1.codigo + "." + area1.nombre))));
                    tc1.Append(titular(contadorarea1 + ". " + area1.nombre, "18", "", "izquierda", "1", "FFFFFF", 2));
                    tr1.Append(tc1, new CantSplit());
                    foreach (tipos_riesgos riesgos in listaRiesgos)
                    {
                        TableCell DatoRiesgo1 = new TableCell();
                        DatoRiesgo1.Append(new TableCellProperties(new Shading { Fill = "2f5496" }));
                        DatoRiesgo1.Append(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                        DatoRiesgo1.Append(titular(MarcaMatriz(matriz, area1.id, 1, riesgos.id), "21", "", "centrar", "1", "FFFFFF", 2));
                        tr1.Append(DatoRiesgo1);
                    }
                    tablaRiesgos.Append(tr1);

                    int contadorarea2 = 1;
                    foreach (areanivel2 area2 in Datos.ListarSistema().Where(x => x.id_areanivel1 == area1.id))
                    {

                        TableRow tr2 = new TableRow();
                        TableCell tc2 = new TableCell();

                        tc2.Append(new TableCellProperties(new Shading { Fill = "b4c6e7" }, new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                        tc2.Append(titular(contadorarea1 + "." + contadorarea2 + ". " + area2.nombre, "18", "", "izquierda", "1", "", 2));
                        tr2.Append(tc2, new CantSplit());
                        foreach (tipos_riesgos riesgos in listaRiesgos)
                        {
                            TableCell DatoRiesgo2 = new TableCell();
                            DatoRiesgo2.Append(new TableCellProperties(new Shading { Fill = "b4c6e7" }));
                            DatoRiesgo2.Append(new TableCellProperties(
                            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" },
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                            ));

                            DatoRiesgo2.Append(titular(MarcaMatriz(matriz, area2.id, 2, riesgos.id), "21", "", "centrar", "1", "", 2));
                            tr2.Append(DatoRiesgo2);

                        }

                        tablaRiesgos.Append(tr2);
                        int contadorarea3 = 1;
                        foreach (areanivel3 area3 in Datos.ListarEquipos().Where(x => x.id_areanivel2 == area2.id))
                        {

                            TableRow tr3 = new TableRow();
                            TableCell tc3 = new TableCell();

                            tc3.Append(new TableCellProperties(new Shading { Fill = "f4b083" }));
                            tc3.Append(new TableCellProperties(
                                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                            tc3.Append(titular(contadorarea1 + "." + contadorarea2 + "." + contadorarea3 + ". " + area3.nombre, "18", "", "izquierda", "1", "", 2));
                            tr3.Append(tc3, new CantSplit());

                            foreach (tipos_riesgos riesgos in listaRiesgos)
                            {
                                TableCell DatoRiesgo3 = new TableCell();
                                DatoRiesgo3.Append(new TableCellProperties(new Shading { Fill = "f4b083" }));
                                DatoRiesgo3.Append(new TableCellProperties(
                                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" },
                            new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                                DatoRiesgo3.Append(titular(MarcaMatriz(matriz, area3.id, 3, riesgos.id), "21", "", "centrar", "1", "", 2));
                                tr3.Append(DatoRiesgo3);
                            }
                            tablaRiesgos.Append(tr3);
                            int contadorarea4 = 1;
                            foreach (areanivel4 area4 in Datos.ListarNivelescuatro().Where(x => x.id_areanivel3 == area3.id))
                            {

                                TableRow tr4 = new TableRow();
                                TableCell tc4 = new TableCell();

                                tc4.Append(new TableCellProperties(new Shading { Fill = "fbe4d5" }));
                                tc4.Append(new TableCellProperties(
                                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3400" }));
                                tc4.Append(titular(contadorarea1 + "." + contadorarea2 + "." + contadorarea3 + "." + contadorarea3 + ". " + area4.nombre, "18", "", "izquierda", "1", "", 2));
                                tr4.Append(tc4, new CantSplit());

                                foreach (tipos_riesgos riesgos in listaRiesgos)
                                {
                                    TableCell DatoRiesgo4 = new TableCell();
                                    DatoRiesgo4.Append(new TableCellProperties(new Shading { Fill = "fbe4d5" }));
                                    DatoRiesgo4.Append(new TableCellProperties(
                                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "210" },
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                                    DatoRiesgo4.Append(titular(MarcaMatriz(matriz, area4.id, 4, riesgos.id), "21", "", "centrar", "1", "", 2));
                                    tr4.Append(DatoRiesgo4);
                                }
                                tablaRiesgos.Append(tr4);
                                contadorarea4++;
                            }
                            contadorarea3++;
                        }
                        contadorarea2++;
                    }

                    //foreach (tipos_riesgos riesgos in listaRiesgos)
                    //{
                    //    TableCell DatoRiesgo = new TableCell();
                    //    DatoRiesgo.Append(new TableCellProperties(
                    //    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "240" }));
                    //    DatoRiesgo.Append(new Paragraph(new Run(new Text("X"))));
                    //    tr1.Append(DatoRiesgo);
                    //}
                    //tablaRiesgos.Append(tr1);
                    contadorarea1++;
                }

            }
            return tablaRiesgos;
        }

        public Table GettablaRiesgosCriticos(int centro)
        {
            //TABLA DE RIESGOS

            Table tablaRiesgos = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 }
                )
            );

            tablaRiesgos.AppendChild<TableProperties>(tblProp);


            var centerJustification = new Justification() { Val = JustificationValues.Left };
            //, new Justification() { Val = JustificationValues.Center }
            TableRow trCabecera = new TableRow();

            TableCell CellCabecera1 = new TableCell();
            CellCabecera1.Append(new TableCellProperties(new Shading { Fill = "89b5e0" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
            CellCabecera1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));

            // CellCabecera1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Riesgo"), new Justification() { Val = JustificationValues.Center })));
            CellCabecera1.Append(titular("Riesgo", "21", "", "", "1", "", 2));
            trCabecera.Append(CellCabecera1);
            TableCell CellCabecera2 = new TableCell();
            CellCabecera2.Append(new TableCellProperties(new Shading { Fill = "89b5e0" }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));

            CellCabecera2.Append(titular("Definición", "21", "", "", "1", "", 2));
            trCabecera.Append(CellCabecera2);
            trCabecera.Append(new TableHeader());
            tablaRiesgos.Append(trCabecera);


            List<tipo_riesgos_critico> listaRiesgos = Datos.ListarTiposRiesgosCriticos();
            //List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
            //List<medidas_preventivas> listaMedidas = Datos.ListarMedidas();
            //List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centro);
            //List<medidas_apartados> apartados = Datos.ListarApartados();

            foreach (tipo_riesgos_critico riesgo in listaRiesgos)
            {

                TableRow tr = new TableRow();
                TableCell tc1 = new TableCell();
                tc1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3100" },
                    new Justification() { Val = JustificationValues.Center },
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                //tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(riesgo.codigo))),
                //    new ParagraphProperties(new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }                 
                //    ));
                tc1.Append(titular(riesgo.codigo, "20", "", "izquierda", "", "", 2));
                tr.Append(tc1, new CantSplit());
                TableCell tc2 = new TableCell();
                tc2.Append(new TableCellProperties(new Justification() { Val = JustificationValues.Center },
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                tc2.Append(titular(riesgo.definicion, "20", "", "izquierda", "", "", 2));
                tr.Append(tc2, new CantSplit());
                tablaRiesgos.Append(tr);

            }
            return tablaRiesgos;
        }

        public Table GettablaRiesgos(int centro)
        {
            //TABLA DE RIESGOS

            Table tablaRiesgos = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 14 }
                )
            );

            tablaRiesgos.AppendChild<TableProperties>(tblProp);


            var centerJustification = new Justification() { Val = JustificationValues.Left };
            //, new Justification() { Val = JustificationValues.Center }
            TableRow trCabecera = new TableRow();

            TableCell CellCabecera1 = new TableCell();
            CellCabecera1.Append(new TableCellProperties(new Shading { Fill = "89b5e0" },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
            CellCabecera1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));

            // CellCabecera1.Append(new Paragraph(new Run(new RunProperties(new Bold()), new Text("Riesgo"), new Justification() { Val = JustificationValues.Center })));
            CellCabecera1.Append(titular("Riesgo", "20", "", "", "1", "", 2));
            trCabecera.Append(CellCabecera1);
            TableCell CellCabecera2 = new TableCell();
            CellCabecera2.Append(new TableCellProperties(new Shading { Fill = "89b5e0" },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));

            CellCabecera2.Append(titular("Definición", "20", "", "", "1", "", 2));
            trCabecera.Append(CellCabecera2);
            trCabecera.Append(new TableHeader());
            tablaRiesgos.Append(trCabecera);


            List<tipos_riesgos> listaRiesgos = Datos.ListarRiesgos();
            //List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
            //List<medidas_preventivas> listaMedidas = Datos.ListarMedidas();
            //List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centro);
            //List<medidas_apartados> apartados = Datos.ListarApartados();

            foreach (tipos_riesgos riesgo in listaRiesgos)
            {

                TableRow tr = new TableRow();
                TableCell tc1 = new TableCell();
                tc1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3100" },
                    new Justification() { Val = JustificationValues.Center },
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                //tc1.Append(new Paragraph(new Run(new RunProperties(new FontSize() { Val = "20" }), new Text(riesgo.codigo))),
                //    new ParagraphProperties(new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }                 
                //    ));
                tc1.Append(titular(riesgo.codigo, "20", "", "izquierda", "", "", 2));
                tr.Append(tc1, new CantSplit());
                TableCell tc2 = new TableCell();
                tc2.Append(new TableCellProperties(new Justification() { Val = JustificationValues.Center },
                    new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                tc2.Append(titular(riesgo.definicion, "20", "", "izquierda", "", "", 2));
                tr.Append(tc2, new CantSplit());
                tablaRiesgos.Append(tr);

            }
            return tablaRiesgos;
        }
        public Paragraph titular(string valor)
        {
            var parrafo = new Paragraph();
            var Propiedades = new ParagraphProperties();

            FontSize fontSize1 = new FontSize() { Val = "12" };

            var runprop = new RunProperties();
            runprop.FontSize = fontSize1;

            Propiedades.Append(runprop);

            var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            var centerJustification = new Justification() { Val = JustificationValues.Distribute };
            Propiedades.Append(paragraphStyleId);
            Propiedades.Append(centerJustification);
            var run = new Run();
            var text = new Text(valor);
            run.Append(text);
            parrafo.Append(Propiedades);
            parrafo.Append(run);
            return parrafo;
        }


        //ESTO HAY QUE PASARLO A BBDD, Y DAR LA POSIBILIDAD AL USUARIO DE MODIFICARLO; PERO EN EL FUTURO
        public string textoObjeto(string nombrecentro)
        {
            return "La Ley 31/ 1995, de Prevención de Riesgos Laborales establece, en su artículo 24, párrafo 2, que 'El empresario titular del centro de trabajo adoptará las medidas necesarias para que aquellos otros empresarios que desarrollen actividades en su centro de trabajo reciban la información y las instrucciones adecuadas, en relación con los riesgos existentes en el centro de trabajo y con las medidas de prevención y protección correspondientes, así como sobre las medidas de emergencia a aplicar, para su traslado a sus respectivos trabajadores." +
            " Para dar cumplimiento a este requisito legal desarrollado de acuerdo al RD 171 / 2004, se establece un listado general de todos los riesgos a los que podrían estar sometidos los trabajadores que realicen tareas en la instalación - " + nombrecentro + "." +
            " En función de las zonas de trabajo, se establece una matriz de riesgos en la que se especifican los riesgos asociados a la instalación en condiciones normales de operación del proceso productivo, en cada una de estas zonas. Además, para cada uno de los riesgos identificados, se presentan las acciones preventivas que deben seguirse para minimizar estos riesgos.";

        }
        public string textoEndesa()
        {
            return "Para la elaboración del presente documento se ha seguido lo indicado en la Metodología de Evaluación de Riesgos de Endesa, en la que establece la identificación de riesgos existentes en una instalación en la que se realizan determinadas actividades como una matriz. " +
"El proceso seguido para la elaboración de la matriz de riesgos de\n un centro es el siguiente:";


        }
        public List<string> DameListaDescripcion()
        {


            List<string> lista = new List<string>{ "Listado de 28 riesgos tipo y descripción de sus situaciones de riesgo.",
             "Listado de instalación tipo, en este caso Central Ciclo combinado.",
             "Listado de actividades tipo.",
            "Identificación de riesgos inherentes de cada instalación tipo. Estos riesgos son las posibilidades de daño que la instalación tipo puede ejercer sobre una persona que se encuentre en su proximidad sin realizar ninguna actividad.",
             "Identificación de riesgos generados por la realización de las actividades tipo en las instalaciones tipo.",
             "Construcción de la matriz de riesgos del centro de actividad" };
            return lista;
        }
        void parseTextForOpenXML(Run run, string textualData)
        {
            string[] newLineArray = { Environment.NewLine }; string[] textArray = textualData.Split(newLineArray, StringSplitOptions.None); bool first = true; foreach (string line in textArray) { if (!first) { run.Append(new Break()); } first = false; Text txt = new Text(); txt.Text = line; run.Append(txt); }

        }

        public Paragraph titular(string valor, string tamanofuente, string colorDetras, string centrar,
                                    string negrita, string color, int tipo)
        {
            var parrafo = new Paragraph();
            var Propiedades = new ParagraphProperties();
            var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            var centerJustification = new Justification() { Val = JustificationValues.Both };

            //ESPACIO ENTRE LINEAS
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            if (tipo == 1)
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "120", Before = "240", Line = "240", LineRule = LineSpacingRuleValues.Auto });
            }
            else if (tipo == 2)
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto });
            }
            else
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto });
            }

            parrafo.Append(paragraphProperties1);

            //JUSTIFICACION
            if (!string.IsNullOrEmpty(centrar))
            {
                if (centrar == "centrar") centerJustification = new Justification() { Val = JustificationValues.Center };
                if (centrar == "derecha") centerJustification = new Justification() { Val = JustificationValues.Right };
                if (centrar == "izquierda") centerJustification = new Justification() { Val = JustificationValues.Left };
            }

            //var justifiacionperfecta = new WordPerfectJustification() { Val = true };
            Propiedades.Append(paragraphStyleId);
            Propiedades.Append(centerJustification);
            parrafo.Append(Propiedades);

            var run = new Run();

            /*Estilos de texto*/

            var runprop = new RunProperties();

            if (!string.IsNullOrEmpty(colorDetras))
            {
                runprop.Append(new Shading { Fill = "89b5e0" });
            }
            if (!string.IsNullOrEmpty(color))
            {
                ColorXML colorlera = new ColorXML() { Val = color };
                runprop.Append(colorlera);
            }

            if (!string.IsNullOrEmpty(negrita))
            {
                Bold bold = new Bold();
                runprop.Bold = new Bold();
            }
            if (!string.IsNullOrEmpty(tamanofuente))
            {
                FontSize fontSize1 = new FontSize() { Val = tamanofuente };
                runprop.FontSize = fontSize1;
            }

            run.Append(runprop);
            var text = new Text(valor) { Space = SpaceProcessingModeValues.Default };
            run.Append(text);
            parrafo.Append(run);
            return parrafo;
        }
        public Paragraph titularIdentado(string valor, string tamanofuente, string colorDetras, string centrar,
                                    string negrita, string color, int tipo)
        {
            var parrafo = new Paragraph();
            var Propiedades = new ParagraphProperties();

            var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
            var centerJustification = new Justification() { Val = JustificationValues.Both };

            //ESPACIO ENTRE LINEAS
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            if (tipo == 1)
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "120", Before = "240", Line = "240", LineRule = LineSpacingRuleValues.Auto },
                    new Indentation() { FirstLine = "720" }
                    );
            }
            else if (tipo == 2)
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto });
            }
            else
            {
                paragraphProperties1 = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto }
                    );
            }

            parrafo.Append(paragraphProperties1);

            //JUSTIFICACION
            if (!string.IsNullOrEmpty(centrar))
            {
                if (centrar == "centrar") centerJustification = new Justification() { Val = JustificationValues.Center };
                if (centrar == "derecha") centerJustification = new Justification() { Val = JustificationValues.Right };
                if (centrar == "izquierda") centerJustification = new Justification() { Val = JustificationValues.Left };
            }

            //var justifiacionperfecta = new WordPerfectJustification() { Val = true };
            Propiedades.Append(paragraphStyleId);
            Propiedades.Append(centerJustification);
            parrafo.Append(Propiedades);

            var run = new Run();

            /*Estilos de texto*/

            var runprop = new RunProperties();

            if (!string.IsNullOrEmpty(colorDetras))
            {
                runprop.Append(new Shading { Fill = "89b5e0" });
            }
            if (!string.IsNullOrEmpty(color))
            {
                ColorXML colorlera = new ColorXML() { Val = color };
                runprop.Append(colorlera);
            }

            if (!string.IsNullOrEmpty(negrita))
            {
                Bold bold = new Bold();
                runprop.Bold = new Bold();
            }
            if (!string.IsNullOrEmpty(tamanofuente))
            {
                FontSize fontSize1 = new FontSize() { Val = tamanofuente };
                runprop.FontSize = fontSize1;
            }

            run.Append(runprop);
            var text = new Text(valor) { Space = SpaceProcessingModeValues.Default };
            run.Append(text);
            parrafo.Append(run);
            return parrafo;
        }

        public static SectionProperties CreateSectionProperties(PageOrientationValues orientation)
        {
            // create the section properties
            SectionProperties properties = new SectionProperties();
            // create the height and width
            UInt32Value height = orientation == (PageOrientationValues.Portrait) ? 16839U : 11907U;
            UInt32Value width = orientation != (PageOrientationValues.Portrait) ? 16839U : 11907U;
            // create the page size and insert the wanted orientation
            PageSize pageSize = new PageSize()
            {
                Width = width,
                Height = height,
                Code = (UInt16Value)9U,
                // insert the orientation
                Orient = orientation
            };
            // create the page margin

            PageMargin pageMargin = new PageMargin()
            {
                Top = 2717,
                Right = (UInt32Value)1417U,
                Bottom = 1417,
                Left = (UInt32Value)1417U,
                Header = (UInt32Value)708U,
                Footer = (UInt32Value)708U,
                Gutter = (UInt32Value)0U
            };

            Columns columns = new Columns() { Space = "720" };
            DocGrid docGrid = new DocGrid() { LinePitch = 360 };
            // appen the page size and margin
            properties.Append(pageSize, pageMargin, columns, docGrid);

            return properties;
        }
        //string ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //descripcion_centro descripcion = new descripcion_centro();
        //centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));


        //int inicio = 0;
        //int fin = 0;
        //ObjetoWord.Application objetoAplicacion = new ObjetoWord.Application();
        //ObjetoWord.Document objetoDocumento = objetoAplicacion.Documents.Add();
        //objetoDocumento.Activate();
        //objetoAplicacion.Visible = true;

        ////CONTROL DE CAMBIOS
        ////Definir objeto parrafo
        //ObjetoWord.Paragraph objetoParrafoDescripcion = objetoDocumento.Content.Paragraphs.Add(Type.Missing);

        //objetoParrafoDescripcion.Range.Text = "0.CONTROL DE CAMBIOS";
        //objetoParrafoDescripcion.Range.Font.Bold = 1;
        //objetoParrafoDescripcion.Range.Font.Size = 12;
        //objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //objetoParrafoDescripcion.Range.InsertParagraphAfter();


        //ObjetoWord.Range controlCambios = objetoDocumento.Range(inicio, fin);
        //objetoDocumento.Tables.Add(controlCambios, 2, 2, 1, 1);


        //ObjetoWord.Table tablaCambios = objetoDocumento.Tables[1];
        //objetoDocumento.Tables[1].Range.Font.Size = 8;

        ////  objetoDocumento.Tables[1].set_Style("Table Professional");
        //tablaCambios.Cell(1, 1).Range.Text = "Versión";
        //tablaCambios.Cell(1, 2).Range.Text = "Modificaciones";

        ////PARRAFO DESCRIPCION
        //string textoDescripcion = Datos.ObtenerInformacionCentral(centroseleccionado.id).descripcion_texto;

        //textoDescripcion = textoDescripcion.Replace("&nbsp;", "");
        //textoDescripcion = textoDescripcion.Replace("\r\n", "\n");
        //textoDescripcion = textoDescripcion.Replace("\r", "\n");
        //HtmlString cadehaHtml = new HtmlString(textoDescripcion);


        //objetoParrafoDescripcion.Range.Text = "1.DESCRIPCION DE LA INSTALACION";
        //objetoParrafoDescripcion.Range.Font.Bold = 1;
        //objetoParrafoDescripcion.Range.Font.Size = 12;
        //objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //objetoParrafoDescripcion.Range.InsertParagraphAfter();


        //objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //objetoParrafoDescripcion.Range.Font.Bold = 1;
        //objetoParrafoDescripcion.Range.Font.Size = 8;
        //objetoParrafoDescripcion.Range.Text = textoDescripcion.ToString();
        //objetoParrafoDescripcion.Range.InsertParagraphAfter();




        ////TABLA DE RIESGOS

        //objetoParrafoDescripcion.Range.Text = "3.IDENTIFICACION DE RIESGOS";
        //objetoParrafoDescripcion.Range.Font.Bold = 1;
        //objetoParrafoDescripcion.Range.Font.Size = 12;
        //objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //objetoParrafoDescripcion.Range.InsertParagraphAfter();


        //ObjetoWord.Range localizacionTabla = objetoDocumento.Range(400, 400);
        //objetoDocumento.Tables.Add(localizacionTabla, 28, 2, 1, 1);
        ////objetoDocumento.Tables.Add(objetoParrafoDescripcion.Range, 28, 2, 1, 1);

        ////Tabla de riesgos



        //ObjetoWord.Table tabla = objetoDocumento.Tables[2];

        //List<tipos_riesgos> listaRiesgosTabla = Datos.ListarRiesgos();
        //objetoDocumento.Tables[2].Range.Font.Size = 8;

        ////  objetoDocumento.Tables[1].set_Style("Table Professional");
        //tabla.Cell(1, 1).Range.Text = "Código";
        //tabla.Cell(1, 2).Range.Text = "Descripcion";
        //int contador = 1;
        //foreach (tipos_riesgos item in listaRiesgosTabla)
        //{
        //    contador++;
        //    tabla.Cell(contador, 1).Range.Text = item.codigo;
        //    tabla.Cell(contador, 2).Range.Text = item.definicion;
        //}


        ////MATRIZ RIESGOS

        //ObjetoWord.Range localizacionMatriz = objetoDocumento.Range(3000, 3000);
        //objetoDocumento.Tables.Add(localizacionMatriz,28 , 28, 1, 1);
        //ObjetoWord.Table MatrizRiesgos = objetoDocumento.Tables[3];
        //objetoDocumento.Tables[3].Range.Font.Size = 8;

        //  objetoDocumento.Tables[3].set_Style("Tabla de cuadrícula 5 oscúra - Énfasis 5");
        //MatrizRiesgos.Cell(1, 1).Range.Text = "Tipos Riesgos/ Secciones";
        //MatrizRiesgos.Cell(1, 2).Range.Text = "RIESGOS";


        //objetoParrafoDescripcion.Range.Text = "4.MEDIDAS PREVENTIVAS";
        //objetoParrafoDescripcion.Range.Font.Bold = 1;
        //objetoParrafoDescripcion.Range.Font.Size = 12;
        //objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //objetoParrafoDescripcion.Range.InsertParagraphAfter();

        ////PARRAFO RIESGOS

        //// objetoParrafoDescripcion.Range.Font.Size = 10;
        //List<tipos_riesgos> listaRiesgos = Datos.ListarRiesgos();
        //List<riesgos_situaciones> listasituaciones = Datos.ListarSituaciones();
        //List<medidas_preventivas> listaMedidas = Datos.ListarMedidas();
        //List<riesgos_medidas> medidasRiesgo = Datos.ListarMedidasRiesgo(centroseleccionado.id);
        //List<medidas_apartados> apartados = Datos.ListarApartados(); 

        //foreach (tipos_riesgos riesgo in listaRiesgos)
        //{


        //    objetoParrafoDescripcion = objetoDocumento.Content.Paragraphs.Add(Type.Missing);
        //    objetoParrafoDescripcion.Range.Font.Size = 12;
        //    objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorWhite;
        //    objetoParrafoDescripcion.Range.Font.Bold = 1;
        //    objetoParrafoDescripcion.Range.Text = riesgo.codigo.ToUpper();
        //    objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdBlue;
        //    objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //    objetoParrafoDescripcion.Range.Font.Size = 9;
        //    objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //    objetoParrafoDescripcion.Range.Font.Bold = 0;
        //    objetoParrafoDescripcion.Range.Text = riesgo.definicion;
        //    objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //    objetoParrafoDescripcion.Range.InsertParagraphAfter();



        //    objetoParrafoDescripcion.Range.Text = "4.1.MEDIDAS GENERALES";
        //    objetoParrafoDescripcion.Range.Font.Bold = 1;
        //    objetoParrafoDescripcion.Range.Font.Size = 10;
        //    objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //    objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //    objetoParrafoDescripcion.Range.InsertParagraphAfter();


        //    foreach (medidas_apartados apart in apartados)
        //    {
        //        foreach (riesgos_medidas medidar in medidasRiesgo.Where(x => x.id_centro == centroseleccionado.id && x.id_riesgo==riesgo.id))
        //        {
        //            objetoParrafoDescripcion.Range.Font.Size = 9;
        //            objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //            objetoParrafoDescripcion.Range.Font.Bold = 0;
        //            objetoParrafoDescripcion.Range.Text = apart.nombre;
        //            objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //            objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //        }
        //    }



        //    objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //    objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //    objetoParrafoDescripcion.Range.Font.Size = 11;
        //    objetoParrafoDescripcion.Range.Text = "Situaciones de Riesgo:";
        //    objetoParrafoDescripcion.Range.InsertParagraphAfter();
        //    objetoParrafoDescripcion.Range.Font.Bold = 0;

        //    foreach (riesgos_situaciones situacion in listasituaciones.Where(x => x.id_tipo_riesgo == riesgo.id))
        //    {
        //        objetoParrafoDescripcion.Range.HighlightColorIndex = ObjetoWord.WdColorIndex.wdNoHighlight;
        //        objetoParrafoDescripcion.Range.Font.Color = ObjetoWord.WdColor.wdColorBlack;
        //        objetoParrafoDescripcion.Range.Font.Size = 10;
        //        objetoParrafoDescripcion.Range.Text += riesgo.id + "." + situacion.descripcion;

        //        objetoParrafoDescripcion.Range.Font.Bold = 0;
        //    }
        //    // objetoParrafoDescripcion.Range.InsertBreak();
        //    objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //    foreach (riesgos_situaciones situacion in listasituaciones.Where(x => x.id_tipo_riesgo == riesgo.id))
        //    {
        //        objetoParrafoDescripcion = objetoDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objetoParrafoDescripcion.Range.Font.Size = 12;
        //        objetoParrafoDescripcion.Range.Font.Bold = 1;
        //        objetoParrafoDescripcion.Range.Text = riesgo.id + "." + situacion.descripcion;
        //        objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //        objetoParrafoDescripcion.Range.Font.Size = 10;
        //        objetoParrafoDescripcion.Range.Font.Bold = 1;
        //        objetoParrafoDescripcion.Range.Text = "Medidas Preventivas";
        //        objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //        foreach (medidas_preventivas medidaspre in listaMedidas.Where(x => x.id_situacion == situacion.id))
        //        {
        //            objetoParrafoDescripcion = objetoDocumento.Content.Paragraphs.Add(Type.Missing);
        //            objetoParrafoDescripcion.Range.Font.Size = 7;
        //            objetoParrafoDescripcion.Range.Text += "-" + medidaspre.descripcion;

        //        }
        //        objetoParrafoDescripcion.Range.InsertParagraphAfter();

        //    }
        //}

        ////PARRAFO MEDIDAS


        //objetoDocumento.SaveAs2(ruta + "\\DocumentoRiesgos.docx");
        //objetoDocumento.Close();
        //objetoAplicacion.Quit();
        //Session["GenerarDocumento"] = "Documento Generado correctamente en ." + ruta;

        //}
        //    catch (Exception ex)
        //    {
        //        Session["GenerarDocumento"] = "Se ha producido un error al generar el documento.";
        //        return RedirectToAction("Principal", "Home");
        //    }

        //    return RedirectToAction("Principal", "Home");

        //public ActionResult CreateDocument()
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        using (var wordDocument = WordprocessingDocument.Create(stream,
        //            WordprocessingDocumentType.Document, true))
        //        {
        //            wordDocument.AddMainDocumentPart();

        //            var document = new Document();
        //            var body = new Body();

        //            var paragraph = new Paragraph();

        //            var paragraphProperties = new ParagraphProperties();
        //            var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
        //            var centerJustification = new Justification() { Val = JustificationValues.Center };

        //            paragraphProperties.Append(paragraphStyleId);
        //            paragraphProperties.Append(centerJustification);

        //            var run = new Run();
        //            var text = new Text("Hello world from Open XML SDK!");

        //            run.Append(text);
        //            paragraph.Append(paragraphProperties);
        //            paragraph.Append(run);

        //            body.Append(paragraph);

        //            document.Append(body);
        //            wordDocument.MainDocumentPart.Document = document;
        //             //CreateWordprocessingDocument(@"c:\Users\Public\Documents\Invoice.docx");
        //            wordDocument.Close();
        //        }

        //        return File(stream.ToArray(), docxMIMEType,
        //            "Word Document Basic Example.docx");
        //    }
        //}

        public static void AddHeaderFromTo(string filepathFrom, string filepathTo)
        {
            // Replace header in target document with header of source document.
            using (WordprocessingDocument
                wdDoc = WordprocessingDocument.Open(filepathTo, true))
            {
                MainDocumentPart mainPart = wdDoc.MainDocumentPart;

                // Delete the existing header part.
                mainPart.DeleteParts(mainPart.HeaderParts);

                // Create a new header part.
                DocumentFormat.OpenXml.Packaging.HeaderPart headerPart =
            mainPart.AddNewPart<HeaderPart>();

                // Get Id of the headerPart.
                string rId = mainPart.GetIdOfPart(headerPart);

                // Feed target headerPart with source headerPart.
                using (WordprocessingDocument wdDocSource =
                    WordprocessingDocument.Open(filepathFrom, true))
                {
                    DocumentFormat.OpenXml.Packaging.HeaderPart firstHeader =
            wdDocSource.MainDocumentPart.HeaderParts.FirstOrDefault();

                    wdDocSource.MainDocumentPart.HeaderParts.FirstOrDefault();

                    if (firstHeader != null)
                    {
                        headerPart.FeedData(firstHeader.GetStream());
                    }
                }

                // Get SectionProperties and Replace HeaderReference with new Id.
                IEnumerable<DocumentFormat.OpenXml.Wordprocessing.SectionProperties> sectPrs =
            mainPart.Document.Body.Elements<SectionProperties>();
                foreach (var sectPr in sectPrs)
                {
                    // Delete existing references to headers.
                    sectPr.RemoveAllChildren<HeaderReference>();

                    // Create the new header reference node.
                    sectPr.PrependChild<HeaderReference>(new HeaderReference() { Id = rId });
                }
            }
        }
        public void AddBulletList(List<string> sentences, WordprocessingDocument wordoc, Body body, string separador, string espacio)
        {
            var runList = ListOfStringToRunList(sentences);

            AddBulletList(runList, wordoc, body, separador, espacio);
        }


        public void AddBulletList(List<Run> runList, WordprocessingDocument wordoc, Body body, string separador, string espacio)
        {
            // Introduce bulleted numbering in case it will be needed at some point
            NumberingDefinitionsPart numberingPart = wordoc.MainDocumentPart.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart = wordoc.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");
                Numbering element = new Numbering();
                element.Save(numberingPart);
            }

            var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            var abstractLevel = new Level(new NumberingFormat() { Val = NumberFormatValues.Bullet }, new LevelText() { Val = separador }) { LevelIndex = 0 };
            var abstractNum1 = new AbstractNum(abstractLevel) { AbstractNumberId = abstractNumberId };

            if (abstractNumberId == 1)
            {
                numberingPart.Numbering.Append(abstractNum1);
            }
            else
            {
                AbstractNum lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
                numberingPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
            }


            var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = numberId };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = abstractNumberId };
            numberingInstance1.Append(abstractNumId1);

            if (numberId == 1)
            {
                numberingPart.Numbering.Append(numberingInstance1);
            }
            else
            {
                var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
                numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
            }



            foreach (Run runItem in runList)
            {
                // Create items for paragraph properties
                var numberingProperties = new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = numberId });
                var spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };  // Get rid of space between bullets
                var indentation = new Indentation() { Left = espacio, Hanging = "360" };  // correct indentation 
                var centerJustification = new Justification() { Val = JustificationValues.Both };
                var fuente = new FontSize() { Val = "18" };
                var espacioEntreLineas = new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto };
                RunProperties rpt = new RunProperties();
                rpt.FontSize = fuente;
                runItem.RunProperties = rpt;
                ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                RunFonts runFonts1 = new RunFonts() { Ascii = "Symbol", HighAnsi = "Symbol" };
                paragraphMarkRunProperties1.Append(runFonts1);

                // create paragraph properties
                var paragraphProperties = new ParagraphProperties(numberingProperties, spacingBetweenLines1, indentation, paragraphMarkRunProperties1, centerJustification, espacioEntreLineas);

                // Create paragraph 
                var newPara = new Paragraph(paragraphProperties);

                // Add run to the paragraph
                newPara.AppendChild(runItem);

                // Add one bullet item to the body
                body.AppendChild(newPara);
            }
        }
        public void AddBulletListTabla(List<Run> runList, WordprocessingDocument wordoc, TableCell celda, string separador, string espacio)
        {
            NumberingDefinitionsPart numberingPart = wordoc.MainDocumentPart.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart = wordoc.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");
                Numbering element = new Numbering();
                element.Save(numberingPart);
            }

            var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            var abstractLevel = new Level(new NumberingFormat() { Val = NumberFormatValues.Bullet }, new LevelText() { Val = separador }) { LevelIndex = 0 };
            var abstractNum1 = new AbstractNum(abstractLevel) { AbstractNumberId = abstractNumberId };

            if (abstractNumberId == 1)
            {
                numberingPart.Numbering.Append(abstractNum1);
            }
            else
            {
                AbstractNum lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
                numberingPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
            }


            var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = numberId };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = abstractNumberId };
            numberingInstance1.Append(abstractNumId1);

            if (numberId == 1)
            {
                numberingPart.Numbering.Append(numberingInstance1);
            }
            else
            {
                var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
                numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
            }



            foreach (Run runItem in runList)
            {
                // Create items for paragraph properties
                var numberingProperties = new NumberingProperties(new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = numberId });
                var spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };  // Get rid of space between bullets
                var indentation = new Indentation() { Left = espacio, Hanging = "360", Right = "260" };  // correct indentation 
                var centerJustification = new Justification() { Val = JustificationValues.Both };
                var fuente = new FontSize() { Val = "18" };
                var espacioEntreLineas = new SpacingBetweenLines() { After = "60", Before = "60", Line = "240", LineRule = LineSpacingRuleValues.Auto };
                RunProperties rpt = new RunProperties();
                rpt.FontSize = fuente;
                runItem.RunProperties = rpt;
                ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                RunFonts runFonts1 = new RunFonts() { Ascii = "Symbol", HighAnsi = "Symbol" };
                paragraphMarkRunProperties1.Append(runFonts1);

                // create paragraph properties
                var paragraphProperties = new ParagraphProperties(numberingProperties, spacingBetweenLines1, indentation, paragraphMarkRunProperties1, centerJustification, espacioEntreLineas);

                // Create paragraph 
                var newPara = new Paragraph(paragraphProperties);

                // Add run to the paragraph
                newPara.AppendChild(runItem);

                // Add one bullet item to the body
                celda.AppendChild(newPara);
            }
        }
        private static List<Run> ListOfStringToRunList(List<string> sentences)
        {
            var runList = new List<Run>();
            foreach (string item in sentences)
            {
                var newRun = new Run();
                newRun.AppendChild(new Text(item));
                runList.Add(newRun);
            }

            return runList;
        }

        public FileResult DescargarDocumento(int id_documento)
        {
            //try
            //{
            centros centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

            //documento_historico documento_Historico = new documento_historico();
            var documento_Historico = Datos.ObtenerDocumentoHistoricoPor_id_centro_descarga(id_documento, centroseleccionado.id);
            var ruta = Server.MapPath("~/Content/ficheros/" + documento_Historico.nombre);
            return File(ruta, "application/docx", documento_Historico.nombre);


            //}
            //catch (Exception ex)
            //{
            //}

            //return File("", "application/docx", "");
        }

    }



}