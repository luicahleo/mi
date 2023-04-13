<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    MIDAS.Models.aspecto_tipo oTipoAspecto;
    MIDAS.Models.aspecto_valoracion oValoracion;
    MIDAS.Models.aspecto_parametros oParametros;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        }

        if (user.perfil == 2)
            permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
        else
        {
            permisos.idusuario = user.idUsuario;
            permisos.idcentro = centroseleccionado.id;
            permisos.permiso = true;
        } 

        ddlParametros.DataSource = ViewData["parametros"];
        ddlParametros.DataValueField = "id_parametro";
        ddlParametros.DataTextField = "nombre";
        ddlParametros.DataBind();

        if (ViewData["quejas"] != null)
        {
            List<ListItem> listado = (List<ListItem>)ViewData["quejas"];
            
            foreach (ListItem item in listado)
            {
                ddlQuejaId.Items.Add(item);
            }
        }

        if (ddlParametros.Items.Count == 0)
        {
            ddlParametros.Visible = false;
            txtParametro.Visible = true;
        }

        grdParametros.DataSource = ViewData["parametrosAsignados"];
        grdParametros.DataBind();

        if (!IsPostBack)
        {
            oTipoAspecto = (MIDAS.Models.aspecto_tipo)ViewData["EditarTipoAspecto"];
            if (oTipoAspecto != null)
            {
                hdnIdAspecto.Value = oTipoAspecto.id.ToString();
                txtCodAspecto.Text = oTipoAspecto.Codigo;
                ddlGrupo.SelectedValue = oTipoAspecto.Grupo.ToString();
                txtUnidad.Text = oTipoAspecto.Unidad;

                ListItem itemGrupo = new ListItem();
                itemGrupo.Value = oTipoAspecto.Grupo.ToString();
                if (oTipoAspecto.Grupo == 1)
                    itemGrupo.Text = "Emisiones reguladas" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5)
                    itemGrupo.Text = "Emisiones no reguladas" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 6)
                    itemGrupo.Text = "Vertidos regulados" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 7)
                    itemGrupo.Text = "Vertidos no regulados" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 8)
                    itemGrupo.Text = "Residuos" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 11)
                    itemGrupo.Text = "Consumos" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 10)
                    itemGrupo.Text = "Consumos\\Agua" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 12)
                    itemGrupo.Text = "Consumos\\Sustancias químicas" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16)
                    itemGrupo.Text = "Ruidos y vibraciones" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21 || oTipoAspecto.Grupo == 22)
                    itemGrupo.Text = "Otros" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 23)
                    itemGrupo.Text = "Potenciales" + "\\" + oTipoAspecto.Nombre;
                if (oTipoAspecto.Grupo == 24)
                    itemGrupo.Text = "Indirectos" + "\\" + oTipoAspecto.Nombre;
                ddlGrupo.Items.Add(itemGrupo);
                
                txtImpacto.Text = oTipoAspecto.Impacto;

                string cadenamagnitud1 = string.Empty;
                string cadenamagnitud2 = string.Empty;
                string cadenamagnitud3 = string.Empty;

                if (oTipoAspecto.Grupo == 13)
                {
                    cadenamagnitud1 = "Funcionamiento de la Central < 4.320 h";
                    cadenamagnitud2 = "Funcionamiento de la Central entre 4.320 y 7.200 h";
                    cadenamagnitud3 = "Funcionamiento de la Central > 7.200 h";
                }      
                
                if (oTipoAspecto.Grupo == 17)
                {
                    cadenamagnitud1 = "Visible a menos de 500 m /Visibilidad Penacho Baja (<10 días/año en polígonos, <5 días/año en zona urbana/no protegida, < 2 días/año en zona sensible/protegida)";
                    cadenamagnitud2 = "Visible entre 500 y 1000 m / Visibilidad Penacho Media (entre 10-20 días/año en polígonos, entre 5-10 días/año en zona urbana/no protegida, entre 2-3 días/año en zona sensible/protegida)";
                    cadenamagnitud3 = "Visible a más de 1000 m / Visibilidad Penacho Alta (>20 días/año en polígonos, >10 días/año en zona urbana/no protegida, >3 días/año en zona sensible/protegida)";
                }
                if (oTipoAspecto.Grupo == 18)
                {
                    cadenamagnitud1 = "Más del 90% de las luminarias de bajo consumo";
                    cadenamagnitud2 = "Entre 50%-90 % de las luminarias de bajo consumo";
                    cadenamagnitud3 = "Menos del 50% de las luminarias de bajo consumo";
                }
                if (oTipoAspecto.Grupo == 19)
                {
                    cadenamagnitud1 = "Superficie ocupada < 2 Ha";
                    cadenamagnitud2 = "Superficie ocupada entre 2 y 10 Ha";
                    cadenamagnitud3 = "Superficie ocupada de > 10 Ha";
                }
                if (oTipoAspecto.Grupo == 22)
                {
                    cadenamagnitud1 = "Perceptible dentro de la central";
                    cadenamagnitud2 = "Perceptible en perímetro de la central (hasta 500 metros)";
                    cadenamagnitud3 = "Perceptible a más de 500 metros";                    
                }
                if (oTipoAspecto.Grupo == 23)
                {
                    cadenamagnitud1 = "Suceso improbable";
                    cadenamagnitud2 = "Suceso poco probable";
                    cadenamagnitud3 = "Suceso probable";
                }
                if (oTipoAspecto.Grupo == 24)
                {
                    cadenamagnitud1 = "Actividad llevada a cabo en menos del 25 % de las jornadas anuales";
                    cadenamagnitud2 = "Actividad llevada a cabo entre el 25-75 % de las jornadas anuales";
                    cadenamagnitud3 = "Actividad llevada a cabo en más del 75 % de las jornadas anuales";
                }
                if (cadenamagnitud1 != string.Empty)
                {
                    ListItem itemMagnitud1 = new ListItem();
                    itemMagnitud1.Value = "1";
                    itemMagnitud1.Text = cadenamagnitud1;
                    ddlMagnitud.Items.Add(itemMagnitud1);
                    ListItem itemMagnitud2 = new ListItem();
                    itemMagnitud2.Value = "2";
                    itemMagnitud2.Text = cadenamagnitud2;
                    ddlMagnitud.Items.Add(itemMagnitud2);
                    ListItem itemMagnitud3 = new ListItem();
                    itemMagnitud3.Value = "3";
                    itemMagnitud3.Text = cadenamagnitud3;
                    ddlMagnitud.Items.Add(itemMagnitud3);
                    if (oTipoAspecto.Grupo == 23 && oTipoAspecto.Codigo == "AP-9")
                    {
                        ListItem itemMagnitud4 = new ListItem();
                        itemMagnitud4.Value = "4";
                        itemMagnitud4.Text = "Positivo legionella";
                        ddlMagnitud.Items.Add(itemMagnitud4);
                    }
                }

                string cadenanaturaleza1 = string.Empty;
                string cadenanaturaleza2 = string.Empty;
                string cadenanaturaleza3 = string.Empty;
                if (oTipoAspecto.Grupo == 2)
                {
                    cadenanaturaleza1 = "Centrales Tipo B: Energías Renovables";
                    cadenanaturaleza2 = "Centrales Tipo A: Cogeneración / Ciclos Combinados)";
                    cadenanaturaleza3 = "Centrales Tipo C: Otros combustibles";
                }
                if (oTipoAspecto.Grupo == 3)
                {
                    cadenanaturaleza1 = "Combustible utilizado Gasolina";
                    cadenanaturaleza2 = "Combustible utilizado Diesel";
                    cadenanaturaleza3 = "Combustible utilizado Nafta u otros derivados";
                }

                if (oTipoAspecto.Grupo == 4)
                {
                    cadenanaturaleza1 = "Máquinas / Vehículos eléctricos";
                    cadenanaturaleza2 = "Máquinas / Vehículos híbridos";
                    cadenanaturaleza3 = "Máquinas / Vehículos combustión convencional";
                }

                if (oTipoAspecto.Grupo == 5)
                {
                    cadenanaturaleza1 = "Almacén cerrado";
                    cadenanaturaleza2 = "Almacén abierto con medidas de reducción de emisiones";
                    cadenanaturaleza3 = "Almacén abierto sin medidas de reducción de emisiones";
                }

                if (oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7)
                {
                    cadenanaturaleza1 = "Vertido de aguas pluviales";
                    cadenanaturaleza2 = "Vertido de aguas sanitarias";
                    cadenanaturaleza3 = "Vertido de aguas de proceso";
                }

                if (oTipoAspecto.Grupo == 8)
                {
                    cadenanaturaleza1 = "Residuo Inerte (RI)";
                    cadenanaturaleza2 = "Residuo No Peligroso (RNP)";
                    cadenanaturaleza3 = "Residuo Peligroso (RP)";
                }

                if (oTipoAspecto.Grupo == 9)
                {
                    cadenanaturaleza1 = "Gas natural o biomasa";
                    cadenanaturaleza2 = "Fuel, Gasóleo y/o Carbón con bajo contenido azufre";
                    cadenanaturaleza3 = "Gasoil / Fuel / Carbón / Diésel";
                }

                if (oTipoAspecto.Grupo == 10)
                {
                    cadenanaturaleza1 = "Agua de mar";
                    cadenanaturaleza2 = "Aguas Superficiales (rio, pantano, embalse,...) o de red";
                    cadenanaturaleza3 = "Aguas subterráneas";
                }

                if (oTipoAspecto.Grupo == 11)
                {
                    cadenanaturaleza1 = "Fuentes renovables";
                    cadenanaturaleza2 = "Autoabastecimiento";
                    cadenanaturaleza3 = "Fuentes no renovables";
                }

                if (oTipoAspecto.Grupo == 12)
                {
                    cadenanaturaleza1 = "No Peligrosa";
                    cadenanaturaleza2 = "-";
                    cadenanaturaleza3 = "Peligrosa";
                }

                if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 19)
                {
                    cadenanaturaleza1 = "Polígono Industrial";
                    cadenanaturaleza2 = "Zona Urbana / No Protegida";
                    cadenanaturaleza3 = "Zona Sensible / Protegida";
                }

                if (oTipoAspecto.Grupo == 15)
                {
                    cadenanaturaleza1 = "NPG es 20% menor de 120 dB";
                    cadenanaturaleza2 = "NPG está entre un 40% y un 20% menor de 120 dB";
                    cadenanaturaleza3 = "NPG es 40% mayor de 120 dB";
                }

                if (oTipoAspecto.Grupo == 16)
                {
                    cadenanaturaleza1 = "Flota de vehículos menor o igual a 20";
                    cadenanaturaleza2 = "Flota de vehículos entre 20 y 40";
                    cadenanaturaleza3 = "Flota de vehículos mayor de 40";
                }

                if (oTipoAspecto.Grupo == 17)
                {
                    cadenanaturaleza1 = "Buena conservación o apantallamiento adecuado";
                    cadenanaturaleza2 = "Conservación aceptable o apantallamiento visual suficiente";
                    cadenanaturaleza3 = "Mala conservación o apantallamiento visual insuficiente";
                }

                if (oTipoAspecto.Grupo == 18)
                {
                    cadenanaturaleza1 = "Más del 90% de las luminarias cuentan con proyectores";
                    cadenanaturaleza2 = "Entre 50% -90 % de las luminarias de cuentan con proyectores";
                    cadenanaturaleza3 = "Menos del 50% de las luminarias cuentan con proyectores";
                }

                if (oTipoAspecto.Grupo == 22)
                {
                    cadenanaturaleza1 = "Olor puntual";
                    cadenanaturaleza2 = "Olor discontinuo";
                    cadenanaturaleza3 = "Olor continuo";
                }

                if (oTipoAspecto.Grupo == 23)
                {
                    cadenanaturaleza1 = "Resolución mediante medios propios de las instalaciones";
                    cadenanaturaleza2 = "Necesarios medios externos complementarios";
                    cadenanaturaleza3 = "Requiere la asistencia de protección civil u otro medio externo";
                }
                
                if (cadenanaturaleza1 != string.Empty)
                {
                    ListItem itemnaturaleza1 = new ListItem();
                    itemnaturaleza1.Value = "1";
                    itemnaturaleza1.Text = cadenanaturaleza1;
                    ddlNaturaleza.Items.Add(itemnaturaleza1);
                    ListItem itemnaturaleza2 = new ListItem();
                    itemnaturaleza2.Value = "2";
                    itemnaturaleza2.Text = cadenanaturaleza2;
                    ddlNaturaleza.Items.Add(itemnaturaleza2);
                    ListItem itemnaturaleza3 = new ListItem();
                    itemnaturaleza3.Value = "3";
                    itemnaturaleza3.Text = cadenanaturaleza3;
                    ddlNaturaleza.Items.Add(itemnaturaleza3);
                }

                string cadenaorigen1 = string.Empty;
                string cadenaorigen2 = string.Empty;
                string cadenaorigen3 = string.Empty;
                if (oTipoAspecto.Grupo == 1)
                {
                    cadenaorigen1 = "Sistemas de reducción de emisiones o depuración en origen";
                    cadenaorigen2 = "Sistemas de reducción de emisiones o depuración a posteriori";
                    cadenaorigen3 = "Emisión libre";
                }
                if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5)
                {
                    cadenaorigen1 = "Receptor Sensible > 3 km";
                    cadenaorigen2 = "Receptor Sensible entre 1 y 3 km";
                    cadenaorigen3 = "Receptor Sensible < 1 km";
                }
                if (oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7)
                {
                    cadenaorigen1 = "Vertido a red de saneamiento";
                    cadenaorigen2 = "Vertido al mar / aguas superficiales";
                    cadenaorigen3 = "Vertido a aguas subterráneas";
                }
                if (oTipoAspecto.Grupo == 8)
                {
                    cadenaorigen1 = "Reutilización/Reciclaje";
                    cadenaorigen2 = "Valorización";
                    cadenaorigen3 = "Eliminación";
                }
                if (oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7)
                {
                    cadenaorigen1 = "Vertido a red de saneamiento";
                    cadenaorigen2 = "Vertido al mar / aguas superficiales";
                    cadenaorigen3 = "Vertido a aguas subterráneas";
                }
                if (oTipoAspecto.Grupo == 8)
                {
                    cadenaorigen1 = "Reutilización/Reciclaje";
                    cadenaorigen2 = "Valorización";
                    cadenaorigen3 = "Eliminación";
                }

                if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 17)
                {
                    cadenaorigen1 = "Polígono Industrial";
                    cadenaorigen2 = "Zona Urbana / No Protegida";
                    cadenaorigen3 = "Zona Sensible / Protegida";
                }

                if (oTipoAspecto.Grupo == 16)
                {
                    cadenaorigen1 = "Vehículos Híbridos/Eléctricos > 50%";
                    cadenaorigen2 = "Vehículos Híbridos/Eléctricos entre 25 y 50%";
                    cadenaorigen3 = "Vehículos Híbridos/Eléctricos < 25%";
                }

                if (oTipoAspecto.Grupo == 22)
                {
                    cadenaorigen1 = "Zona aislada";
                    cadenaorigen2 = "Polígono industrial";
                    cadenaorigen3 = "Zona urbana";
                }
                
                if (oTipoAspecto.Grupo == 23)
                {
                    cadenaorigen1 = "Afección a un vector ambiental";
                    cadenaorigen2 = "Afección a dos vectores ambientales";
                    cadenaorigen3 = "Afección a los tres vectores ambientales";
                }
                if (oTipoAspecto.Grupo == 24)
                {
                    cadenaorigen1 = "Proveedor acredita certificación ambiental (ISO 14001/ EMAS)";
                    cadenaorigen2 = "Proveedor acredita cumplimiento de la legislación ambiental (autorizaciones, estudios de evaluación, mediciones, etc.)";
                    cadenaorigen3 = "Proveedor no acredita cumplimiento de la totalidad de la legislación ambiental aplicable (autorizaciones, estudios de evaluación, mediciones, etc.)";
                }
                if (cadenaorigen1 != string.Empty)
                {
                    ListItem itemOrigen1 = new ListItem();
                    itemOrigen1.Value = "1";
                    itemOrigen1.Text = cadenaorigen1;
                    ddlOrigen.Items.Add(itemOrigen1);
                    ListItem itemOrigen2 = new ListItem();
                    itemOrigen2.Value = "2";
                    itemOrigen2.Text = cadenaorigen2;
                    ddlOrigen.Items.Add(itemOrigen2);
                    ListItem itemOrigen3 = new ListItem();
                    itemOrigen3.Value = "3";
                    itemOrigen3.Text = cadenaorigen3;
                    ddlOrigen.Items.Add(itemOrigen3);
                }
            }

            if (ViewData["EditarAspectoValoracion"] != null)
            {
                oValoracion = (MIDAS.Models.aspecto_valoracion)ViewData["EditarAspectoValoracion"];
                hdnIdValoracion.Value = oValoracion.id.ToString();
                if (oValoracion.foco == 1)
                    txtIdentificacion.Text = oValoracion.nombrefoco;
                else
                    txtIdentificacion.Text = oValoracion.identificacion;
                txtReferencia.Text = oValoracion.referencia.ToString();
                txtServicio.Text = oValoracion.IN_ServicioPrestado;
                ddlTipoIndirecto.SelectedValue = oValoracion.IN_TipoActividad.ToString();
                ddlAspecto.SelectedValue = oValoracion.IN_Aspecto.ToString();
                txtDescripcion.Text = oValoracion.descripcion;
                if (oValoracion.queja != null)
                    ddlQueja.SelectedValue = oValoracion.queja.ToString();
                txtObsQueja.Text = oValoracion.quejaobs;
                if (oValoracion.idqueja != null)
                ddlQueja.SelectedValue = oValoracion.idqueja.ToString();
                
                        txtMed1.Text = oValoracion.anio1.ToString();
                        txtMed2.Text = oValoracion.anio2.ToString();
                        txtMed3.Text = oValoracion.anio3.ToString();
                        txtMed4.Text = oValoracion.anio4.ToString();
                        txtMed5.Text = oValoracion.anio5.ToString();
                        txtMed6.Text = oValoracion.anio6.ToString();

                        txtDia.Text = oValoracion.RU_Dia.ToString();
                        txtRefDia.Text = oValoracion.RU_DiaRef.ToString();
                        txtTarde.Text = oValoracion.RU_Tarde.ToString();
                        txtRefTarde.Text = oValoracion.RU_TardeRef.ToString();
                        txtNoche.Text = oValoracion.RU_Noche.ToString();
                        txtRefNoche.Text = oValoracion.RU_NocheRef.ToString();

                        txtDescripcion.Text = oValoracion.descripcion;

                        oParametros = (MIDAS.Models.aspecto_parametros)ViewData["EditarParametros"];

                        if (oParametros != null)
                        {
                            if (oTipoAspecto.relativoMwhb == true && oParametros.Mwhb1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.Mwhb1.ToString();
                                txtRelativoAnio2.Text = oParametros.Mwhb2.ToString();
                                txtRelativoAnio3.Text = oParametros.Mwhb3.ToString();
                                txtRelativoAnio4.Text = oParametros.Mwhb4.ToString();
                                txtRelativoAnio5.Text = oParametros.Mwhb5.ToString();
                                txtRelativoAnio6.Text = oParametros.Mwhb6.ToString();
                            }
                            if (oTipoAspecto.relativom3Hora == true && oParametros.m3Hora1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.m3Hora1.ToString();
                                txtRelativoAnio2.Text = oParametros.m3Hora2.ToString();
                                txtRelativoAnio3.Text = oParametros.m3Hora3.ToString();
                                txtRelativoAnio4.Text = oParametros.m3Hora4.ToString();
                                txtRelativoAnio5.Text = oParametros.m3Hora5.ToString();
                                txtRelativoAnio6.Text = oParametros.m3Hora6.ToString();
                            }
                            if (oTipoAspecto.relativokmAnio == true && oParametros.kmAnio1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.kmAnio1.ToString();
                                txtRelativoAnio2.Text = oParametros.kmAnio2.ToString();
                                txtRelativoAnio3.Text = oParametros.kmAnio3.ToString();
                                txtRelativoAnio4.Text = oParametros.kmAnio4.ToString();
                                txtRelativoAnio5.Text = oParametros.kmAnio5.ToString();
                                txtRelativoAnio6.Text = oParametros.kmAnio6.ToString();
                            }
                            if (oTipoAspecto.Grupo == 16)
                            {
                                txtMed1.Text = oParametros.kmAnio1.ToString();
                                txtMed2.Text = oParametros.kmAnio2.ToString();
                                txtMed3.Text = oParametros.kmAnio3.ToString();
                                txtMed4.Text = oParametros.kmAnio4.ToString();
                                txtMed5.Text = oParametros.kmAnio5.ToString();
                                txtMed6.Text = oParametros.kmAnio6.ToString();
                                txtMed1.ReadOnly = true;
                                txtMed2.ReadOnly = true;
                                txtMed3.ReadOnly = true;
                                txtMed4.ReadOnly = true;
                                txtMed5.ReadOnly = true;
                                txtMed6.ReadOnly = true;
                            }

                            if (oTipoAspecto.relativohfuncAnio == true && oParametros.hfuncAnio1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.hfuncAnio1.ToString();
                                txtRelativoAnio2.Text = oParametros.hfuncAnio2.ToString();
                                txtRelativoAnio3.Text = oParametros.hfuncAnio3.ToString();
                                txtRelativoAnio4.Text = oParametros.hfuncAnio4.ToString();
                                txtRelativoAnio5.Text = oParametros.hfuncAnio5.ToString();
                                txtRelativoAnio6.Text = oParametros.hfuncAnio6.ToString();
                            }

                            if (oTipoAspecto.relativohAnioGE == true && oParametros.hAnioGE1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.hAnioGE1.ToString();
                                txtRelativoAnio2.Text = oParametros.hAnioGE2.ToString();
                                txtRelativoAnio3.Text = oParametros.hAnioGE3.ToString();
                                txtRelativoAnio4.Text = oParametros.hAnioGE4.ToString();
                                txtRelativoAnio5.Text = oParametros.hAnioGE5.ToString();
                                txtRelativoAnio6.Text = oParametros.hAnioGE6.ToString();
                            }

                            if (oTipoAspecto.relativonumtrab == true && oParametros.numtrabAnio1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.numtrabAnio1.ToString();
                                txtRelativoAnio2.Text = oParametros.numtrabAnio2.ToString();
                                txtRelativoAnio3.Text = oParametros.numtrabAnio3.ToString();
                                txtRelativoAnio4.Text = oParametros.numtrabAnio4.ToString();
                                txtRelativoAnio5.Text = oParametros.numtrabAnio5.ToString();
                                txtRelativoAnio6.Text = oParametros.numtrabAnio6.ToString();
                            }

                            if (oTipoAspecto.relativom3aguadeitsalada == true && oParametros.m3aguadesaladaAnio1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.m3aguadesaladaAnio1.ToString();
                                txtRelativoAnio2.Text = oParametros.m3aguadesaladaAnio2.ToString();
                                txtRelativoAnio3.Text = oParametros.m3aguadesaladaAnio3.ToString();
                                txtRelativoAnio4.Text = oParametros.m3aguadesaladaAnio4.ToString();
                                txtRelativoAnio5.Text = oParametros.m3aguadesaladaAnio5.ToString();
                                txtRelativoAnio6.Text = oParametros.m3aguadesaladaAnio6.ToString();
                            }

                            if (oTipoAspecto.relativotrabcantera == true && oParametros.m3aguadesaladaAnio1 != null)
                            {
                                txtRelativoAnio1.Text = oParametros.trabcanteraAnio1.ToString();
                                txtRelativoAnio2.Text = oParametros.trabcanteraAnio2.ToString();
                                txtRelativoAnio3.Text = oParametros.trabcanteraAnio3.ToString();
                                txtRelativoAnio4.Text = oParametros.trabcanteraAnio4.ToString();
                                txtRelativoAnio5.Text = oParametros.trabcanteraAnio5.ToString();
                                txtRelativoAnio6.Text = oParametros.trabcanteraAnio6.ToString();
                            }

                            if (oTipoAspecto.relativono == true)
                            {
                                filaProduccion.Visible = false;
                            }
                        }
                        
                        

                        txtVariacion.ReadOnly = true;
                        txtAcercamiento.ReadOnly = true;
                        txtVariacion.Text = oValoracion.variacion.ToString();
                        txtAcercamiento.Text = oValoracion.acercamiento.ToString();


                        decimal total = 0;
                        if (oTipoAspecto.Grupo == 8)
                        {
                            total = MIDAS.Models.Datos.CalcularTotalResiduosCentral(centroseleccionado.id);
                            txtTotal.Text = oValoracion.acercamiento.ToString();
                        }
                        if (oTipoAspecto.Grupo == 9)
                        {
                            total = MIDAS.Models.Datos.CalcularTotalConsumoCombustibleCentral(centroseleccionado.id);
                            txtTotal.Text = total.ToString();
                            txtMagnitudRel.Text = oValoracion.acercamiento.ToString();
                        }

                        if (oTipoAspecto.Grupo == 12)
                        {
                            total = MIDAS.Models.Datos.CalcularTotalConsumoSustanciasCentral(centroseleccionado.id);
                            txtTotal.Text = total.ToString();
                            txtMagnitudRel.Text = oValoracion.acercamiento.ToString();
                        }
                        
                        
                        

                        string cadenanaturalezaIND1 = string.Empty;
                        string cadenanaturalezaIND2 = string.Empty;
                        string cadenanaturalezaIND3 = string.Empty;
                        if (oTipoAspecto.Grupo == 24 && (oTipoAspecto.id == 40 || oTipoAspecto.id == 41 || oTipoAspecto.id == 42 || oTipoAspecto.id == 43 ))
                        {
                            cadenanaturalezaIND1 = "Residuo Inerte (RI)";
                            cadenanaturalezaIND2 = "Residuo No Peligroso (RNP)";
                            cadenanaturalezaIND3 = "Residuo Peligroso (RP)";
                        }
                        if (oTipoAspecto.Grupo == 24 && oTipoAspecto.id == 48)
                        {
                            cadenanaturalezaIND1 = "Vertidos de pluviales, vertidos de refrigeración o similares";
                            cadenanaturalezaIND2 = "Vertidos de aguas sanitarias y de limpieza general de las instalaciones";
                            cadenanaturalezaIND3 = "Vertidos de aguas con sustancias contaminantes";
                        }
                        if (oTipoAspecto.Grupo == 24 && oTipoAspecto.id == 46)
                        {
                            cadenanaturalezaIND1 = "No utilización de maquinaria generadora de ruido";
                            cadenanaturalezaIND2 = "-";
                            cadenanaturalezaIND3 = "Utilización maquinaria generadora de ruido";
                        }
                        if (oTipoAspecto.Grupo == 24 && (oTipoAspecto.id == 44 || oTipoAspecto.id == 45 || oTipoAspecto.id == 47 || oTipoAspecto.id == 49))
                        {
                            cadenanaturalezaIND1 = "Emisiones Vapor de agua/Polvo o material pulverulento";
                            cadenanaturalezaIND2 = "Emisiones combustión/Grupos Electrógenos";
                            cadenanaturalezaIND3 = "Emisiones CO2";
                        }
                        if (cadenanaturalezaIND1 != string.Empty)
                        {
                            ListItem itemnaturaleza1 = new ListItem();
                            itemnaturaleza1.Value = "1";
                            itemnaturaleza1.Text = cadenanaturalezaIND1;
                            ddlNaturaleza.Items.Add(itemnaturaleza1);
                            ListItem itemnaturaleza2 = new ListItem();
                            itemnaturaleza2.Value = "2";
                            itemnaturaleza2.Text = cadenanaturalezaIND2;
                            ddlNaturaleza.Items.Add(itemnaturaleza2);
                            ListItem itemnaturaleza3 = new ListItem();
                            itemnaturaleza3.Value = "3";
                            itemnaturaleza3.Text = cadenanaturalezaIND3;
                            ddlNaturaleza.Items.Add(itemnaturaleza3);
                        }
                        ddlMagnitud.SelectedValue = oValoracion.magnitud.ToString();
                        ddlNaturaleza.SelectedValue = oValoracion.naturaleza.ToString();
                        ddlOrigen.SelectedValue = oValoracion.origen.ToString();

                        txtMagnitud.Text = oValoracion.resmagnitud.ToString();
                        txtNaturaleza.Text = oValoracion.resnaturaleza.ToString();
                        txtOrigen.Text = oValoracion.resorigen.ToString();

                        if (oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                        {
                            filaProduccion.Visible = false;
                            filaDatosCantidad.Visible = false;
                            txtMed1.Enabled = false;
                            txtMed2.Enabled = false;
                            txtMed3.Enabled = false;
                            txtMed4.Enabled = false;
                            txtMed5.Enabled = false;
                            txtMed6.Enabled = false;
                            txtReferencia.Enabled = false;
                        }

                        if (oValoracion.foco == 1 && oTipoAspecto.Grupo == 12)
                        {
                            txtMed1.ReadOnly = true;
                            txtMed2.ReadOnly = true;
                            txtMed3.ReadOnly = true;
                            txtMed4.ReadOnly = true;
                            txtMed5.ReadOnly = true;
                            txtMed6.ReadOnly = true;

                            List<MIDAS.Models.aspecto_parametro_valoracion> parametros = (List<MIDAS.Models.aspecto_parametro_valoracion>)ViewData["parametrosAsignados"];

                            txtMed1.Text = parametros.Sum(x => x.anio1).ToString();
                            txtMed2.Text = parametros.Sum(x => x.anio2).ToString();
                            txtMed3.Text = parametros.Sum(x => x.anio3).ToString();
                            txtMed4.Text = parametros.Sum(x => x.anio4).ToString();
                            txtMed5.Text = parametros.Sum(x => x.anio5).ToString();
                            txtMed6.Text = parametros.Sum(x => x.anio6).ToString();
                            
                        }

                        if (oValoracion.significancia1 == 1)
                        {
                            txtSignificancia1.Text = "Significativo";
                            txtSignificancia1.BackColor = System.Drawing.Color.Red;
                            txtSignificancia1.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia1 == 0)
                        {
                            txtSignificancia1.Text = "No Significativo";
                            txtSignificancia1.BackColor = System.Drawing.Color.Green;
                            txtSignificancia1.ForeColor = System.Drawing.Color.White;
                        }

                        if (oValoracion.significancia2 == 1)
                        {
                            txtSignificancia2.Text = "Significativo";
                            txtSignificancia2.BackColor = System.Drawing.Color.Red;
                            txtSignificancia2.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia2 == 0)
                        {
                            txtSignificancia2.Text = "No Significativo";
                            txtSignificancia2.BackColor = System.Drawing.Color.Green;
                            txtSignificancia2.ForeColor = System.Drawing.Color.White;
                        }
                
                        if (oValoracion.significancia3 == 1)
                        {
                            txtSignificancia3.Text = "Significativo";
                            txtSignificancia3.BackColor = System.Drawing.Color.Red;
                            txtSignificancia3.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia3 == 0)
                        {
                            txtSignificancia3.Text = "No Significativo";
                            txtSignificancia3.BackColor = System.Drawing.Color.Green;
                            txtSignificancia3.ForeColor = System.Drawing.Color.White;
                        }

                        if (oValoracion.significancia4 == 1)
                        {
                            txtSignificancia4.Text = "Significativo";
                            txtSignificancia4.BackColor = System.Drawing.Color.Red;
                            txtSignificancia4.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia4 == 0)
                        {
                            txtSignificancia4.Text = "No Significativo";
                            txtSignificancia4.BackColor = System.Drawing.Color.Green;
                            txtSignificancia4.ForeColor = System.Drawing.Color.White;
                        }

                        if (oValoracion.significancia5 == 1)
                        {
                            txtSignificancia5.Text = "Significativo";
                            txtSignificancia5.BackColor = System.Drawing.Color.Red;
                            txtSignificancia5.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia5 == 0)
                        {
                            txtSignificancia5.Text = "No Significativo";
                            txtSignificancia5.BackColor = System.Drawing.Color.Green;
                            txtSignificancia5.ForeColor = System.Drawing.Color.White;
                        }

                        if (oValoracion.significancia6 == 1)
                        {
                            txtSignificancia6.Text = "Significativo";
                            txtSignificancia6.BackColor = System.Drawing.Color.Red;
                            txtSignificancia6.ForeColor = System.Drawing.Color.White;
                        }
                        if (oValoracion.significancia6 == 0)
                        {
                            txtSignificancia6.Text = "No Significativo";
                            txtSignificancia6.BackColor = System.Drawing.Color.Green;
                            txtSignificancia6.ForeColor = System.Drawing.Color.White;
                        }

            }
        }

        if (Session["EdicionIndicadorMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionIndicadorMensaje"].ToString() + "' });", true);
            Session["EdicionIndicadorMensaje"] = null;
        }

        if (Session["EdicionIndicadorError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionIndicadorError"].ToString() + "' });", true);
            Session["EdicionIndicadorError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Aspecto </title>
    <script type="text/javascript">


        $(document).ready(function () {
            var tiposeleccionado = $('#ctl00_MainContent_ddlQueja').val();

            if (tiposeleccionado == 1) {
                $("#seccionObs").show();
                $("#seccionQuejas").show();
            }
            else {
                $("#seccionObs").hide();
                $("#seccionQuejas").hide();
            }

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarAspectoValoracion")
                    $("#hdFormularioEjecutado").val("GuardarAspectoValoracion");
                if (val == "ctl00_MainContent_btnAddParametro")
                    $("#hdFormularioEjecutado").val("btnAddParametro");
                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");

            });

            $("#ctl00_MainContent_ddlQueja").change(function () {
                var tiposeleccionado = $('#ctl00_MainContent_ddlQueja').val();


                if (tiposeleccionado == 1) {
                    $("#seccionObs").show();
                    $("#seccionQuejas").show();
                }
                else {
                    $("#seccionObs").hide();
                    $("#seccionQuejas").hide();
                }

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });


        });

        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>
                Edición de Aspecto
            </h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-search"></i>Datos generales</h6>
        </div>
        <div class="panel-body">
            <asp:HiddenField runat="server" ID="hdnIdIndicador" />
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px; width:10%">
                    <div class="form-group">
                        <label>
                            Código</label>
                        <asp:TextBox ID="txtCodAspecto" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td> 
                    <td style="padding-right:15px; width:40%">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdAspecto" />
                        <label>
                            Grupo</label>
                            <asp:DropDownList Enabled="false" runat="server" ID="ddlGrupo" class="form-control" Width="100%">                                
                            </asp:DropDownList>
                    </div>
                    </td>  
                    <% if (oTipoAspecto.Grupo != 17 && oTipoAspecto.Grupo != 18 && oTipoAspecto.Grupo != 19 && oTipoAspecto.Grupo != 21 && oTipoAspecto.Grupo != 22 && oTipoAspecto.Grupo !=23)
                       {%>
                    <td style="padding-right:15px; width:30%">
                    
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="HiddenField2" />
                        <% if (oTipoAspecto.Grupo == 1 ||oTipoAspecto.Grupo == 6 ||oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                           {    %>
                        <label>
                            Identificación del foco</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5)
                           {    %>
                        <label>
                            Identificación de la emisión</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 3)
                           {    %>
                        <label>
                            Identificación del grupo electrógeno</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 7)
                           {    %>
                        <label>
                            Identificación del vertido</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 8)
                           {    %>
                        <label>
                            Identificación del residuo</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12)
                           {    %>
                        <label>
                            Identificación del consumo</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16)
                           {    %>
                        <label>
                            Identificación del origen ruido</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 20)
                           {    %>
                        <label>
                            Identificación del producto depositado</label>
                        <% } %>
                        <% if (oTipoAspecto.Grupo == 24)
                           {    %>
                        <label>
                            Proveedor</label>
                        <% } %>

                        <asp:TextBox ID="txtIdentificacion" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>    
                    <% } %>
                    <td style="padding-right:15px; width:10%">
                    <div class="form-group">
                        <label>
                            Unidad</label>
                        <asp:TextBox ID="txtUnidad" Enabled="false" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>       
            
                    </tr>
            </table>
            
            <% if (oTipoAspecto.Grupo == 24)
               { %>
            <table width="100%">
                <tr>
                    <td style="width: 25%">
                        <label>
                            Servicio prestado</label>
                        <asp:TextBox ID="txtServicio" runat="server" Width="95%" class="form-control"></asp:TextBox>
                    </td>
                    <td style="width: 25%">
                        <label>
                            Tipo de actividad</label>
                        <asp:DropDownList runat="server" ID="ddlTipoIndirecto" class="form-control" Width="95%">
                            <asp:ListItem Value="1" Text="Mantenimiento de Maquinaria / Instalaciones"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Servicios y suministros"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Obras"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Instalaciones Compartidas"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 25%">

                        <asp:DropDownList runat="server" Visible="false" AutoPostBack="true" ID="ddlAspecto" class="form-control"
                            Width="95%">
                            <asp:ListItem Value="1" Text="Generación de residuos peligrosos"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Generación de residuos no peligrosos"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Generación de Residuos de Construcción y Demolición (RCDs)"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Generación de Residuos de Aparatos Eléctricos y Electrónicos (RAEEs)"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Generación de vertidos de aguas residuales"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Emisión de Ruido"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Emisiones a la Atmósfera"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <% } %>
            <br />
            <table width="100%">
                <tr>
                <td>
                    <label>
                            Impacto</label>
                    
                        <asp:TextBox ID="txtImpacto" Enabled="false" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Descripción</label>
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Rows="4" runat="server"
                            class="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-warning"></i>Queja/Denuncia/Incidencia</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">    
                <table width="100%">
                    <tr>
                        <td style="width: 10%">
                            <label>
                                Queja/Denuncia/Incidencia</label>
                            <asp:DropDownList runat="server" ID="ddlQueja" class="form-control"
                                Width="90%">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Sí"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <div id="seccionQuejas">
                                <label>Seleccionar queja:</label>
                                    <asp:DropDownList ID="ddlQuejaId" Width="40%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>      
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <br />
                        <div id="seccionObs">
                        <label>
                            Observaciones</label>
                        <asp:TextBox ID="txtObsQueja" TextMode="MultiLine" Rows="4" runat="server"
                            class="form-control"></asp:TextBox>
                            </div>
                    </td>
                    </tr>
                </table>
            </div> 
        </div>
    </div>

    <% if (oValoracion.foco == 1)
       { %>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-rulers"></i>Parámetros</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">                         
            <table width="100%">
                            <tr>
                                <td style="padding-right:10px; width:20%">

                                    <label>
                                        Parámetros a planificar:</label>
                                    <asp:DropDownList ID="ddlParametros" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>       
                                    <asp:TextBox ID="txtParametro" Visible="false" runat="server" Width="95%" class="form-control"></asp:TextBox>         
                                </td>                                
                                <td style="padding-top:10px">
                                <input id="btnAddParametro" type="submit" runat="server" value="Asignar Parámetro"
                                        class="btn btn-primary run-first" />
                                </td>
                            </tr>
                        </table>
                                                    <asp:GridView ID="grdParametros" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Parámetro    
                                                                            </th> 
                                                                            <th style="width:8%">
                                                                                Magnitud(M)
                                                                            </th>
                                                                            <th style="width:8%">
                                                                                Nat/Pel/A(N)    
                                                                            </th>                                                                             
                                                                            <th style="width:8%">
                                                                                Origen/Destino(D)
                                                                            </th>                    
                                                                            <th style="width:8%">
                                                                                Significancia
                                                                            </th> 
                                                                            <th style="width:8%">
                                                                                Editar
                                                                            </th> 
                                                                            <th style="width:8%">
                                                                                Eliminar
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% foreach (GridViewRow item in grdParametros.Rows)
                                                                           { %>
                                                                        <tr>         
                                                                            <td class="text">     
                                                                                <%= item.Cells[3].Text%>
                                                                            </td>    
                                                                            <td class="text">     
                                                                                <%= item.Cells[34].Text%>
                                                                            </td>  
                                                                            <td class="text">     
                                                                                <%= item.Cells[35].Text%>
                                                                            </td>  
                                                                            <td class="text">     
                                                                                <%= item.Cells[36].Text%>
                                                                            </td>  
                                                                            <% 
                                                                                if (item.Cells[33].Text == "1")
                                                                               { %> 
                                                                            <td style="background-color:Red; color:White" class="task-desc">
                                                                                Significativo
                                                        
                                                                            </td>     
                                                                            <% }
                                                                                else if (item.Cells[33].Text == "0")
                                                                               { %> 
                                                                            <td style="background-color:Green; color:White" class="task-desc">
                                                                                No Significativo
                                                        
                                                                            </td>     
                                                                            <% }
                                                                               else
                                                                               { %>      
                                                                                    <td class="task-desc">
                                                    
                                                                            </td>   
                                                                               <% } %> 
                                                                            <td class="text-center">
                                                                                <a href="/evr/aspectosambientales/aspecto_parametros/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                                            </td>
                                                                            <td class="text-center">
                                                                                <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/aspectosambientales/eliminar_parametro/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                                            </td>
                                                                        </tr>
                                                                        <% } %>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                            <br />
                                                        </div>
                                                       
            </div>
        </div>
    </div>

    <% } %>


    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-rulers"></i>Medición</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <asp:HiddenField runat="server" ID="hdnIdValoracion" />
            </div>

            <table style="width:100%; border: 1px solid #41b9e6">
            <% 
                if (oValoracion.foco != 1 || (oValoracion.foco == 1 && oTipoAspecto.Grupo == 12))
               { %>
                <tr runat="server" id="filaProduccion">
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                    <center>
                        <label>
                            Ponderación 
                        </label>
                        <label>
                                        <% 
                                        if (oTipoAspecto.relativoMwhb == true)
                                        { %>
                                        (Mwhb)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativohAnioGE == true)
                                        { %>
                                        (Horas/Año de Grupo Elec.)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativohfuncAnio== true)
                                        { %>
                                        (Horas de func./Año)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativokmAnio == true)
                                        { %>
                                        (Km/Año)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativom3Hora == true)
                                        { %>
                                        (m3/Hora)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativonumtrab == true)
                                        { %>
                                        (num.trabajadores)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativom3aguadeitsalada == true)
                                        { %>
                                        (m3 agua desalada)
                                        <% } %>
                                        <% 
                                        if (oTipoAspecto.relativotrabcantera == true)
                                        { %>
                                        (trabajadores cantera)
                                        <% } %>
                                    </label></center>
                    </td>
                        <td style="border: 1px solid #41b9e6; padding:15px">
                                    <table runat="server" width="100%" id="tablaProduccion">
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 6%></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 5%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 4%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 3%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-2 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-1 %></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio1" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio2" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio3" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio4" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio5" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtRelativoAnio6" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr></table>
                    </td>
                </tr>
                <tr runat="server" id="filaDatosCantidad">
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Datos Cantidad
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                         <table runat="server" width="100%" id="Table2">
                                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 6%></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 5%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 4%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 3%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 2%></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year - 1%></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed1" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed2" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed3" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed4" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed5" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%;">
                                <center>
                                    <asp:TextBox ID="txtMed6" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Cálculos
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding-bottom:15px; padding-left:10px; padding-right:10px">

                            <% if (oTipoAspecto.Grupo != 6 && oTipoAspecto.Grupo != 7 && oTipoAspecto.Grupo != 8)
                               { %>
                            <table style="width: 55%; margin-top:10px; margin-left:15px">
                                <tr>
                                    <% if (oTipoAspecto.Grupo != 2 && oTipoAspecto.Grupo != 3 && oTipoAspecto.Grupo != 4 && oTipoAspecto.Grupo != 5 && oTipoAspecto.Grupo != 6 && oTipoAspecto.Grupo != 9 && oTipoAspecto.Grupo != 10 && oTipoAspecto.Grupo != 11 && oTipoAspecto.Grupo != 12
                                           && oTipoAspecto.Grupo != 15 && oTipoAspecto.Grupo != 16
                                           && oTipoAspecto.Grupo != 17 && oTipoAspecto.Grupo != 18 && oTipoAspecto.Grupo != 19 && oTipoAspecto.Grupo != 22 &&
                                           oTipoAspecto.Grupo != 23 && oTipoAspecto.Grupo != 24)
                                       { %>
                                    <td style="padding-right: 20px; width: 15%">
                                        <label>
                                            V.Referencia</label>
                                        <asp:TextBox ID="txtReferencia" Width="110px" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <% } %>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                                       { %>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Día</label>
                                        <asp:TextBox ID="txtRefDia" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Tarde</label>
                                        <asp:TextBox ID="txtRefTarde" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <td style="padding-right: 15px; width: 15%">
                                        <label>
                                            Ref.Noche</label>
                                        <asp:TextBox ID="txtRefNoche" runat="server" class="form-control"></asp:TextBox>
                                    </td>
                                    <% } %>
                                </tr>
                            </table>
                            <% } %>

                        <table runat="server" width="100%" id="Table3">
                                    <tr>
                            <td style="width: 10%; padding-top: 10px; ">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <label>
                                        Variación(%)
                                    </label>
                                    <% } %>
                                <% if (oTipoAspecto.Grupo == 8)
                               { %>
                                    <label>
                                        Magnitud Relativa(%)</label><% }
                                        %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <label>
                                        Acercamiento(%)</label><% }
                                        %>
                                        
                                        <% if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                               { %>
                                    <label>
                                        Magnitud Relativa(%)</label><% }
                                        %>
                                       
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 8)
                               { %>
                                    <label>
                                        Total Residuos (t)</label><% }
                                        %>
                                     <% if (oTipoAspecto.Grupo == 9)
                                    { %>
                                    <label>
                                        Total Consumo combustibles+sustancias</label><% }
                                        %>
                                        <% if (oTipoAspecto.Grupo == 12)
                                    { %>
                                    <label>
                                        Total Consumo combustibles+sustancias</label><% }
                                        %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Día</label>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Tarde</label>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <label>
                                        Med.Noche</label>
                                    <%} %></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || oTipoAspecto.Grupo == 12 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <asp:TextBox ID="txtVariacion" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                     
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                                   { %>
                                        <asp:TextBox ID="txtMagnitudRel" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                        <% } %>
                                        <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 14  || oTipoAspecto.Grupo == 20 || oTipoAspecto.Grupo == 21)
                               { %>
                                    <asp:TextBox ID="txtAcercamiento" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    
                                      <% if (oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 12)
                               { %>
                                    <asp:TextBox ID="txtTotal" Enabled="false" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    <% } %>
                                </center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtDia" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtTarde" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                            <td style="width: 10%; padding-top: 10px">
                                <center>
                                    <% if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14)
                               { %>
                                    <asp:TextBox Width="90%" ID="txtNoche" runat="server" class="form-control"></asp:TextBox>
                                    <% } %></center>
                            </td>
                        </tr>

                        </table>
                
                            <table width="100%">
                <tr>
                    <td style="width: 30%;">
                        <center>
                            <%
                        if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 22)
                        {                                  
                            %>
                            <label>
                                Magnitud (M)</label>
                            <% } %>
                          <% if (oTipoAspecto.Grupo == 17)
                        {                                  
                            %>
                            <label>
                                Visibilidad</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 18)
                        {                                  
                            %>
                            <label>
                                % Luminarias bajo consumo</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 19)
                        {                                  
                            %>
                            <label>
                                Superficie ocupada (Ha)</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 22)
                        {                                  
                            %>
                            <label>
                                Perceptibilidad</label>
                            <% } %>
                            <%
                        if (oTipoAspecto.Grupo == 23)
                        {                                  
                            %>
                            <label>
                                Probabilidad (P)</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 24)
                        {  %>
                            <label>
                                Frecuencia (F)</label>
                            <% } %>
                        </center>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <center>
                            <%
                                if (oTipoAspecto.Grupo == 2)
                                { %>
                                <label>Tipo de central</label>
                        <% }   %>
                        <%
                                if (oTipoAspecto.Grupo == 3)
                                { %>
                                <label>Tipo de combustible</label>
                        <% }   %>
                        <%
                                if (oTipoAspecto.Grupo == 4)
                                { %>
                                <label>Tipo de vehículo/maquinaria/equipo</label>
                        <% }   %>
                        <%
                                if (oTipoAspecto.Grupo == 5)
                                { %>
                                <label>Tipo de almacén</label>
                        <% }   %>
                        <%
                                 if (oTipoAspecto.Grupo == 7)
                                { %>
                                <label>Tipo de vertido</label>
                            <% }   %>
                        <%
                                 if (oTipoAspecto.Grupo == 8)
                                { %>
                                <label>Peligrosidad</label>
                            <% }   %>
                        <%
                                 if (oTipoAspecto.Grupo == 9)
                                { %>
                                <label>Tipo de Combustible</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 10)
                                { %>
                                <label>Origen del agua</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 11)
                                { %>
                                <label>Origen del consumo</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 12 && oValoracion.foco != 1)
                                { %>
                                <label>Tipo de sustancia</label>
                            <% }   %>
                            <% if (oTipoAspecto.Grupo == 15)
                        {                                  
                            %>
                            <label>
                                Valor potencia acústica</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 16)
                        {                                  
                            %>
                            <label>
                                Nº Vehículos</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 17)
                        {                                  
                            %>
                            <label>
                                Conservación/Apantallamiento</label>
                            <% } %>
                             <% if (oTipoAspecto.Grupo == 18)
                        {                                  
                            %>
                            <label>
                                % Luminarias con proyectores</label>
                            <% } %>
                             <% if (oTipoAspecto.Grupo == 19)
                        {                                  
                            %>
                            <label>
                                Ubicación de la central</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 22)
                        {                                  
                            %>
                            <label>
                                Continuidad del olor</label>
                            <% } %>
                        <%          
                        if (oTipoAspecto.Grupo == 23) { %>
                            <label>
                                Gravedad (G)</label>
                            <% }
                        if (oTipoAspecto.Grupo == 24) { %>
                            <label>
                                Naturaleza (N)</label>
                            <% }
                        if (oTipoAspecto.Grupo == 70)
                        { %>
                            <label>
                                Naturaleza / Peligrosidad / Acercamiento a límites legales (N)</label><% } %>

                        </center>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <center>
                             <%
                                 if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5)
                                { %>
                                <label>Distancia de receptores sensibles</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 7)
                                { %>
                                <label>Destino del vertido</label>
                            <% }   %>
                            <%
                                 if (oTipoAspecto.Grupo == 8)
                                { %>
                                <label>Tratamiento</label>
                            <% }   %>
                            <% if (oTipoAspecto.Grupo == 16)
                        {                                  
                            %>
                            <label>
                                Tipo vehículos</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 17)
                        {                                  
                            %>
                            <label>
                                Ubicación de la central</label>
                            <% } %>
                            <% if (oTipoAspecto.Grupo == 22)
                        {                                  
                            %>
                            <label>
                                Ubicación de la central</label>
                            <% } %>
                            <%
                                  if (oTipoAspecto.Grupo == 23)
                                  { %>
                            <label>
                                Afección al medio (A)</label>
                            <% }
                                     if (oTipoAspecto.Grupo == 24)
                                       { %>
                            <label>
                                Desempeño ambiental del proveedor (D)</label>
                            <% }
                                       if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 )
                                       { %>
                            <label>
                                Origen / Destino / Sistemas de control (D)</label>
                            <% } %>
                        </center>
                    </td>
                </tr>
                
                <tr>

                    <td style="width: 30%; padding-top: 10px">
                        <%  if (oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                            {
                        %>
                        <asp:DropDownList runat="server" ID="ddlMagnitud" class="form-control" Width="95%">
                        </asp:DropDownList>
                        <% } %>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <%  if (oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 9 || oTipoAspecto.Grupo == 10 || oTipoAspecto.Grupo == 11 || (oTipoAspecto.Grupo == 12 && oValoracion.foco != 1) || oTipoAspecto.Grupo == 15 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 18 || oTipoAspecto.Grupo == 19 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                       { %>
                        <center>
                            <asp:DropDownList runat="server" ID="ddlNaturaleza" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </center>
                        <% } %>
                    </td>
                    <td style="width: 30%; padding-top: 10px">
                        <% if (oTipoAspecto.Grupo == 1 || oTipoAspecto.Grupo == 2 || oTipoAspecto.Grupo == 3 || oTipoAspecto.Grupo == 4 || oTipoAspecto.Grupo == 5 || oTipoAspecto.Grupo == 6 || oTipoAspecto.Grupo == 7 || oTipoAspecto.Grupo == 8 || oTipoAspecto.Grupo == 13 || oTipoAspecto.Grupo == 14 || oTipoAspecto.Grupo == 16 || oTipoAspecto.Grupo == 17 || oTipoAspecto.Grupo == 22 || oTipoAspecto.Grupo == 23 || oTipoAspecto.Grupo == 24)
                       { %>
                        <center>
                            <asp:DropDownList runat="server" ID="ddlOrigen" class="form-control" Width="95%">
                            </asp:DropDownList>
                        </center>
                        <% } %>
                    </td>
                </tr>
            </table>
                    
                    </td>
                </tr>
            <% } %>
                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Evaluación
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                        <table runat="server" width="100%" id="Table4">
                            <tr>
                                <td>
                                    <center>
                                        <label>
                                            Magnitud(M)
                                        </label>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <label>
                                            Nat/Pel/A(N)
                                        </label>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <label>
                                            Origen/Destino(D)
                                        </label>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtMagnitud" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtNaturaleza" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:TextBox ID="txtOrigen" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #41b9e6; width:10%; padding:15px">
                        <center><label>
                            Significancia
                        </label></center>
                    </td>
                    <td style="border: 1px solid #41b9e6; padding:15px">
                        <table runat="server" width="100%" id="Table5">
                                    <tr>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-6 %></label>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-5 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-4 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-3 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-2 %></label></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <label>
                                        <%= DateTime.Now.Year-1 %></label></center>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia1" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox>
                                </center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia2" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia3" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia4" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia5" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                            <td style="width: 10%">
                                <center>
                                    <asp:TextBox ID="txtSignificancia6" ReadOnly="true" Width="90%" runat="server" class="form-control"></asp:TextBox></center>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarAspectoValoracion" type="submit" value="Guardar datos" class="btn btn-primary run-first" />

        <a href="/evr/aspectosambientales/gestion_aspectos" title="Volver" class="btn btn-primary run-first">Volver</a>

    </div>
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
