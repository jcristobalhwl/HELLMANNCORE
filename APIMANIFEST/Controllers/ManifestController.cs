using Common;
using Domain;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScrapySharp.Extensions;
using Service.Implementations.Manifest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
namespace APIMANIFEST.Controllers
{
    public class ManifestController : ApiController
    {
        private readonly ManifestService _manifestService;
        public ManifestController()
        {
            _manifestService = new ManifestService();
        }
        public JsonResult<ResponseBase<TBL_MAN_MANIFEST>> GetData()
        {

            var options = new ChromeOptions();
            options.AddArguments("--disabled-gpu");
            var chromeDriver = new ChromeDriver(options);
            chromeDriver.Navigate().GoToUrl("https://www.unitedcargo.com/OurNetwork/TrackingCargo1512/Tracking.jsp?id=35710113&pfx=016");

            var tables = chromeDriver.FindElements(By.CssSelector("span"));



            int tableId = 0;
            int trId = 0;
            string startDate = Convert.ToDateTime("10/02/2020").ToString("dd/MM/yyyy");//.ToShortDateString("dd/MM/yyyy");
            string endDate = Convert.ToDateTime("10/02/2020").ToString("dd/MM/yyyy"); //.ToShortDateString();
            string url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiesto&fec_inicio=" + startDate + "&fec_fin=" + endDate + "&cod_terminal=0000&tamanioPagina=100000";

            //string url = "https://www.deltacargo.com/Cargo/trackShipment?awbNumber=00623405023&timeZoneOffset=300";
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
            HtmlDocument document = htmlWeb.Load(url);
            List<HtmlNode> td;
            List<TBL_ADU_MANIFEST> manifestList = new List<TBL_ADU_MANIFEST>();
            TBL_ADU_MANIFEST manifestObj;
            HtmlNode anchorNode;
            HtmlNode evalua;
            foreach (HtmlNode table in document.DocumentNode.CssSelect("table"))
            {
                if (tableId == 7)
                {
                    trId = 0;
                    foreach (var tr in table.CssSelect("tr"))
                    {
                        if (trId > 0)
                        {
                            td = new List<HtmlNode>();
                            td = tr.CssSelect("td").ToList();
                            manifestObj = new TBL_ADU_MANIFEST();
                            anchorNode = td[0].CssSelect("a").First();
                            manifestObj.VCH_MANIFESTNUMBER = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            manifestObj.DAT_DEPARTUREDATE = Convert.ToDateTime(td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                            manifestObj.VCH_TERMINAL = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            manifestObj.VCH_AIRLINE = td[3].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            manifestObj.VCH_SHIPMENTPORT = td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            manifestObj.VCH_FLIGHTNUMBER = td[5].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            manifestObj.CHR_REGIME = "E";
                            manifestObj.CHR_VIA = "A";
                            manifestObj.BIT_COMPLETED = false;
                            manifestObj.DAT_DOWNLOADDATE = DateTime.Now;
                            manifestList.Add(manifestObj);
                        }
                        trId++;
                    }
                    manifestList.GroupBy(x => x.VCH_MANIFESTNUMBER).Select(x => x.First()).ToList();
                    _manifestService.insertManifest(ref manifestList);
                }
                tableId++;
            }
            #region CONSULTA DE MANIFIESTOS DE SALIDA
            TBL_ADU_MANIFESTSHIPMENTDOC manifestSDObj = new TBL_ADU_MANIFESTSHIPMENTDOC();
            List<TBL_ADU_MANIFESTSHIPMENTDOC> manifestSDList = new List<TBL_ADU_MANIFESTSHIPMENTDOC>();
            TBL_ADU_MANIFESTSHIPMENTDETAILDOC manifestSDDetailObj = new TBL_ADU_MANIFESTSHIPMENTDETAILDOC();
            List<TBL_ADU_MANIFESTSHIPMENTDETAILDOC> manifestSDDetailList = new List<TBL_ADU_MANIFESTSHIPMENTDETAILDOC>();
            foreach (var item in manifestList)
            {
                url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuia&viat=4&CG_cadu=235&CMc1_Anno=00" + item.VCH_MANIFESTNUMBER.Substring(0, 2) + "&CMc1_Numero=" + item.VCH_MANIFESTNUMBER.Substring(3) + "&CMc1_Terminal=0000&tamanioPagina=100000";
                htmlWeb = new HtmlWeb();
                htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                document = htmlWeb.Load(url);

                while (document.ParsedText.Length <= 1500)
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuia&viat=4&CG_cadu=235&CMc1_Anno=00" + item.VCH_MANIFESTNUMBER.Substring(0, 2) + "&CMc1_Numero=" + item.VCH_MANIFESTNUMBER.Substring(3) + "&CMc1_Terminal=0000&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                }
                evalua = document.DocumentNode.CssSelect("td").First();
                while (evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION ADUANERA POR FECHA SALIDA"
                        || evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION DE CARGA AEREA")
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuia&viat=4&CG_cadu=235&CMc1_Anno=00" + item.VCH_MANIFESTNUMBER.Substring(0, 2) + "&CMc1_Numero=" + item.VCH_MANIFESTNUMBER.Substring(3) + "&CMc1_Terminal=0000&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                    evalua = document.DocumentNode.CssSelect("td").First();
                }

                tableId = 0;
                foreach (HtmlNode tabla in document.DocumentNode.CssSelect("table"))
                {
                    #region DATOS CABECERA


                    if (tableId == 3)
                    {
                        manifestSDObj = new TBL_ADU_MANIFESTSHIPMENTDOC();
                        manifestSDObj.NUM_MANIFESTID = item.NUM_MANIFESTID;
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId > 0)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                switch (trId)
                                {
                                    case 1:
                                        manifestSDObj.VCH_MANIFESTNUMBER = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Replace("  ", " ").Trim();
                                        manifestSDObj.VCH_MANIFESTNUMBER = manifestSDObj.VCH_MANIFESTNUMBER.Substring(0, 13) + manifestSDObj.VCH_MANIFESTNUMBER.Substring(13).Trim();
                                        manifestSDObj.INT_DETAILSNUMBER = Convert.ToInt32(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        break;
                                    case 2:
                                        manifestSDObj.DAT_DEPARTUREDATE = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDObj.DEC_GROSSWEIGHT = Convert.ToDecimal(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        break;
                                    case 3:
                                        manifestSDObj.VCH_AIRLINE = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDObj.VCH_NATIONALITY = td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        break;
                                    case 4:
                                        manifestSDObj.VCH_FLIGHTNUMBER = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDObj.DEC_PACKAGES = Convert.ToInt32(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        break;
                                    case 5:
                                        manifestSDObj.VCH_FINALDATEBOARD = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        if (manifestSDObj.VCH_FINALDATEBOARD != "" && manifestSDObj.VCH_FINALDATEBOARD.Length > 15)
                                        {
                                            if (manifestSDObj.VCH_FINALDATEBOARD.Trim().Substring(0, 1).Any(x => char.IsNumber(x)))
                                            {
                                                manifestSDObj.VCH_FINALDATEBOARD = manifestSDObj.VCH_FINALDATEBOARD.Substring(0, 10) + " " + manifestSDObj.VCH_FINALDATEBOARD.Substring(10).Trim();
                                                if (manifestSDObj.VCH_FINALDATEBOARD.Split(' ')[1].Length == 4)
                                                {
                                                    manifestSDObj.VCH_FINALDATEBOARD = manifestSDObj.VCH_FINALDATEBOARD.Split(' ')[0] + " " + manifestSDObj.VCH_FINALDATEBOARD.Split(' ')[1].Substring(0, 2) + ":" + manifestSDObj.VCH_FINALDATEBOARD.Split(' ')[1].Substring(2, 2);
                                                }
                                            }
                                            else
                                            {
                                                manifestSDObj.VCH_FINALDATEBOARD = "";
                                            }
                                        }

                                        manifestSDObj.VCH_DATEAUTHORIZATIONBOARD = td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        break;
                                    case 6:
                                        manifestSDObj.VCH_MANIFESTTRANSMISSIONDATE = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        break;
                                }
                            }
                            trId++;
                        }
                        manifestSDList.Add(manifestSDObj);
                    }

                    #endregion

                    #region DATOS DETALLE

                    if (tableId == 10)
                    {
                        trId = 0;
                        manifestSDDetailObj = new TBL_ADU_MANIFESTSHIPMENTDETAILDOC();
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId > 0)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                //if (!td[0].InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim().Contains("No hay informaci"))
                                if (td.Count > 1)
                                {

                                    if (trId % 2 != 0)
                                    {
                                        manifestSDDetailObj.NUM_MANIFESTID = item.NUM_MANIFESTID;
                                        anchorNode = td[0].CssSelect("a").First();
                                        manifestSDDetailObj.VCH_AIRGUIDE = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDDetailObj.VCH_DIRECTMASTERGUIDE = td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        if (manifestSDDetailObj.VCH_DIRECTMASTERGUIDE.Length > 20)
                                        {
                                            anchorNode = td[1].CssSelect("a").First();
                                            manifestSDDetailObj.VCH_DIRECTMASTERGUIDE = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        }

                                        anchorNode = td[2].CssSelect("a").First();
                                        manifestSDDetailObj.VCH_DETAIL = Convert.ToInt32(anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.VCH_TERMINALCODE = td[3].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDDetailObj.DEC_WEIGHTORIGIN = Convert.ToDecimal(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.DEC_PACKAGEORIGIN = Convert.ToInt32(td[5].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.DEC_MANIFESTEDWEIGHT = Convert.ToDecimal(td[6].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.DEC_MANIFESTEDPACKAGE = Convert.ToInt32(td[7].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.DEC_WEIGHTRECEIVED = Convert.ToDecimal(td[8].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.DEC_PACKAGERECEIVED = Convert.ToInt32(td[9].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        manifestSDDetailObj.VCH_CONSIGNEE = td[10].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDDetailObj.VCH_SHIPPER = td[11].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDDetailObj.VCH_DATETRANSMISSIONDOCUMENT = td[12].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    }
                                    else
                                    {
                                        manifestSDDetailObj.VCH_DESCRIPTION = td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                        manifestSDDetailList.Add(manifestSDDetailObj);
                                        manifestSDDetailObj = new TBL_ADU_MANIFESTSHIPMENTDETAILDOC();
                                    }


                                }
                            }

                            trId++;
                        }
                    }

                    #endregion

                    tableId++;
                }
            }
            _manifestService.insertManifestShipmentDocument(ref manifestSDList);
            _manifestService.insertManifestShipmentDetailDocument(ref manifestSDDetailList);
            
            #endregion

            #region RECORRE LA CONSULTA DE MANIFIESTOS DE SALIDA
            string manifestYear = "";
            string manifestNumber = "";
            TBL_ADU_ADUANADESTINATION aduanaDestinationObj = new TBL_ADU_ADUANADESTINATION();
            List<TBL_ADU_ADUANADESTINATION> aduanaDestinationList = new List<TBL_ADU_ADUANADESTINATION>();

            TBL_ADU_MASTERINFORMATION masterInformationObj = new TBL_ADU_MASTERINFORMATION();
            List<TBL_ADU_MASTERINFORMATION> masterInformationList = new List<TBL_ADU_MASTERINFORMATION>();

            TBL_ADU_WAREDESCRIPTION wareDescriptionObj = new TBL_ADU_WAREDESCRIPTION();
            List<TBL_ADU_WAREDESCRIPTION> wareDescriptionList = new List<TBL_ADU_WAREDESCRIPTION>();
            List<TBL_ADU_WEBTRACKING> webTrackingsList = new List<TBL_ADU_WEBTRACKING>();
            TBL_ADU_WEBTRACKING webTrackingObj;
            string masterGuidePrefix = "";
            foreach (var item in manifestSDDetailList)
            {
                #region DESTINOS DE VUELOS
                webTrackingsList = _manifestService.getWebsTracking();
                masterGuidePrefix = item.VCH_DIRECTMASTERGUIDE.Substring(0, 3);
                webTrackingObj =  webTrackingsList.Where(x => x.CHR_PREFIX == masterGuidePrefix).FirstOrDefault();
                if (webTrackingObj != null)
                {
                    switch (webTrackingObj.CHR_PREFIX)
                    {
                        case "001":
                            //url = $"{webTrackingObj.VCH_LINK}/"  + item.VCH_MANIFESTNUMBER.Substring(0, 2) + "&CMc1_Numero=" + item.VCH_MANIFESTNUMBER.Substring(3) + "&CMc1_Terminal=0000&tamanioPagina=100000";
                            //htmlWeb = new HtmlWeb();
                            //htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                            //document = htmlWeb.Load(url);

                            //Problema POST

                            break;
                        case "006":
                            //url = $"{webTrackingObj.VCH_LINK}?awbNumber={item.VCH_DIRECTMASTERGUIDE}&timeZoneOffset=300";
                            //htmlWeb = new HtmlWeb();
                            //htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                            //document = htmlWeb.Load(url);
                            //Problema JAVASCRIPT

                            break;
                        case "014":
                            //url = $"{webTrackingObj.VCH_LINK}/?s_acn=014&s_sref={item.VCH_DIRECTMASTERGUIDE.Substring(3,8)}";
                            //htmlWeb = new HtmlWeb();
                            //htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                            //document = htmlWeb.Load(url);
                            //var tables = document.DocumentNode.CssSelect("table");
                            //tableId = 0;
                            //foreach (HtmlNode table in document.DocumentNode.CssSelect("table"))
                            //{
                            //    if (tableId == 0)
                            //    {

                            //    }
                            //    tableId++;
                            //}

                            //Problema JAVASCRIPT

                            break;
                        case "016":
                          
                            //url = $"{webTrackingObj.VCH_LINK}?id={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}&pfx=016";
                            //htmlWeb = new HtmlWeb();
                            //htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                            //document = htmlWeb.Load(url);
                            //var result = document.GetElementbyId("dispTable0");
                            //break;

                        case "044":

                            break;
                        case "057":

                            break;
                        case "074":

                            break;
                        case "075":

                            break;
                        case "139":

                        case "125":

                            break;
                        case "144":

                            break;
                        case "145":

                            break;
                        case "180":

                            break;
                        case "230":

                            break;
                        case "369":

                            break;
                        case "489":

                            break;
                        case "530":

                            break;
                        case "605":
                            break;

                        case "729":

                            break;
                        case "996":

                            break;
                        default:
                            break;
                    }
                }
                else
                {

                }
                #endregion
                #region DESTINACIONES ADUANERAS POR CONOCIMIENTO/GUIA
                manifestYear = "";
                manifestYear = "20" + manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(0, 2);
                manifestNumber = "";
                manifestNumber = manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(3).PadLeft(5, '+');
                url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuiaDUA&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&tamanioPagina=100000";
                htmlWeb = new HtmlWeb();
                htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                document = htmlWeb.Load(url);

                while (document.ParsedText.Length <= 1500)
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuiaDUA&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                }
                evalua = document.DocumentNode.CssSelect("td").First();
                while (evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION ADUANERA POR FECHA SALIDA"
                        || evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION DE CARGA AEREA")
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiestoGuiaDUA&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                    evalua = document.DocumentNode.CssSelect("td").First();
                }

                tableId = 0;
                aduanaDestinationObj = new TBL_ADU_ADUANADESTINATION();
                foreach (HtmlNode tabla in document.DocumentNode.CssSelect("table"))
                {
                    if (tableId == 3)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId == 1)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                anchorNode = td[1].CssSelect("b").First();

                                manifestNumber = "";
                                manifestNumber = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            }
                            trId++;
                        }
                    }

                    if (tableId == 10)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId > 0)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                if (td.Count > 1)
                                {
                                    aduanaDestinationObj = new TBL_ADU_ADUANADESTINATION();
                                    aduanaDestinationObj.DEC_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    aduanaDestinationObj.VCH_MANIFEST = manifestNumber;
                                    aduanaDestinationObj.INT_SEC = Convert.ToInt32(td[0].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    aduanaDestinationObj.VCH_DUANUMBER = td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    aduanaDestinationObj.VCH_DATENUMBERING = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    aduanaDestinationObj.VCH_DATEDATED = td[3].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    if ((td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim()).Any(x => char.IsNumber(x)))
                                    {
                                        aduanaDestinationObj.INT_DETAIL = Convert.ToInt32(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    }

                                    aduanaDestinationObj.VCH_AGENT = td[5].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    aduanaDestinationObj.NUM_WEIGHTREQUESTED = Convert.ToDecimal(td[6].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    aduanaDestinationObj.INT_PACKAGEREQUESTED = Convert.ToInt32(td[7].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());

                                    aduanaDestinationList.Add(aduanaDestinationObj);
                                }
                            }

                            trId++;
                        }
                    }


                    tableId++;
                }

                #endregion

                #region INFORMACION DEL DOCUMENTO DE TRANSPORTE MASTER

                manifestYear = "";
                manifestYear = "20" + manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(0, 2);
                manifestNumber = "";
                manifestNumber = manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(3).PadLeft(5, '+');
                url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleMasterExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&tamanioPagina=100000";
                htmlWeb = new HtmlWeb();
                htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                document = htmlWeb.Load(url);

                while (document.ParsedText.Length <= 1500)
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleMasterExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                }
                evalua = document.DocumentNode.CssSelect("td").First();
                while (evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION ADUANERA POR FECHA SALIDA"
                        || evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION DE CARGA AEREA")
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleMasterExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                    evalua = document.DocumentNode.CssSelect("td").First();
                }

                tableId = 0;
                masterInformationObj = new TBL_ADU_MASTERINFORMATION();
                foreach (HtmlNode tabla in document.DocumentNode.CssSelect("table"))
                {
                    if (tableId == 3)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId == 1)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                anchorNode = td[1].CssSelect("b").First();

                                manifestNumber = "";
                                manifestNumber = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            }
                            trId++;
                        }
                    }

                    if (tableId == 10)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId > 0)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                if (td.Count > 1)
                                {
                                    masterInformationObj = new TBL_ADU_MASTERINFORMATION();
                                    masterInformationObj.DEC_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    masterInformationObj.VCH_MANIFEST = manifestNumber;
                                    masterInformationObj.INT_DETAILNUMBER = Convert.ToInt32(td[0].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.VCH_TERMINALCODE = td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    masterInformationObj.VCH_SHIPMENTPORT = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    masterInformationObj.DEC_WEIGHTORIGIN = Convert.ToDecimal(td[3].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.DEC_PACKAGEORIGIN = Convert.ToDecimal(td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.DEC_MANIFESTEDWEIGHT = Convert.ToDecimal(td[5].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.DEC_MANIFESTEDPACKAGE = Convert.ToDecimal(td[6].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.DEC_WEIGHTRECEIVED = Convert.ToDecimal(td[7].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.DEC_PACKAGERECEIVED = Convert.ToDecimal(td[8].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    masterInformationObj.VCH_CONSIGNEE = td[9].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    masterInformationObj.VCH_SHIPPER = td[10].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    masterInformationObj.VCH_DESTINATIONPORT = td[11].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    masterInformationObj.VCH_TRANSMISSIONDATE = td[12].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();

                                    masterInformationList.Add(masterInformationObj);
                                }
                            }

                            trId++;
                        }
                    }

                    tableId++;
                }

                #endregion

                #region DESCRIPCION DE MERCANCIAS DEL DOCUMENTO DE TRANSPORTE
                manifestYear = "";
                manifestYear = "20" + manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(0, 2);
                manifestNumber = "";
                manifestNumber = manifestList.Where(x => x.NUM_MANIFESTID == item.NUM_MANIFESTID).Select(j => j.VCH_MANIFESTNUMBER).FirstOrDefault().Substring(3).PadLeft(5, '+');
                url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleConocimientoEmbarqueExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&CMc2_NumDet=" + item.VCH_DETAIL.ToString().PadLeft(3, '+') + "&tamanioPagina=100000";
                htmlWeb = new HtmlWeb();
                htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                document = htmlWeb.Load(url);

                while (document.ParsedText.Length <= 1500)
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleConocimientoEmbarqueExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&CMc2_NumDet=" + item.VCH_DETAIL.ToString().PadLeft(3, '+') + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                }
                evalua = document.DocumentNode.CssSelect("td").First();
                while (evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION ADUANERA POR FECHA SALIDA"
                        || evalua.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() == "MANIFIESTOS DE EXPORTACION DE CARGA AEREA")
                {
                    url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultarDetalleConocimientoEmbarqueExportacion&CG_cadu=235&CMc2_Anno=" + manifestYear + "&CMc2_Numero=" + manifestNumber + "&CMc2_numcon=" + item.VCH_AIRGUIDE + "&CMc2_numconm=" + item.VCH_DIRECTMASTERGUIDE + "&CMc2_NumDet=" + item.VCH_DETAIL.ToString().PadLeft(3, '+') + "&tamanioPagina=100000";
                    htmlWeb = new HtmlWeb();
                    htmlWeb.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");
                    document = htmlWeb.Load(url);
                    evalua = document.DocumentNode.CssSelect("td").First();
                }

                tableId = 0;
                wareDescriptionObj = new TBL_ADU_WAREDESCRIPTION();
                foreach (HtmlNode tabla in document.DocumentNode.CssSelect("table"))
                {

                    if (tableId == 3)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId == 1)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                anchorNode = td[1].CssSelect("b").First();

                                manifestNumber = "";
                                manifestNumber = anchorNode.InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                            }
                            trId++;
                        }
                    }

                    if (tableId == 7)
                    {
                        trId = 0;
                        foreach (var tr in tabla.CssSelect("tr"))
                        {
                            if (trId > 1)
                            {
                                td = new List<HtmlNode>();
                                td = tr.CssSelect("td").ToList();

                                if (td.Count > 1)
                                {
                                    wareDescriptionObj = new TBL_ADU_WAREDESCRIPTION();
                                    wareDescriptionObj.DEC_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    wareDescriptionObj.VCH_MANIFEST = manifestNumber;
                                    wareDescriptionObj.DEC_PACKAGES = Convert.ToDecimal(td[0].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    wareDescriptionObj.DEC_GROSSWEIGHT = Convert.ToDecimal(td[1].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    if (td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim() != "")
                                    {
                                        //wareDescriptionObj.empaque = Convert.ToInt32(td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                        wareDescriptionObj.VCH_PACKING = td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    }
                                    //wareDescriptionObj.empaque = Convert.ToInt32(td[2].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
                                    wareDescriptionObj.VCH_SHIPPER = td[3].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    wareDescriptionObj.VCH_CONSIGNEE = td[4].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();
                                    wareDescriptionObj.VCH_MARKSANDNUMBERS = td[5].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Replace("<br>", "").Trim();
                                    wareDescriptionObj.VCH_DESCRIPTION = td[6].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim();

                                    wareDescriptionList.Add(wareDescriptionObj);
                                }
                            }
                            trId++;
                        }

                    }
                    tableId++;
                }
                #endregion
            }

            //INSERTA DESTINACIONES ADUANERAS
            _manifestService.insertAduanaDestinations(ref aduanaDestinationList);

            //INSERTA INFORMACION MASTER
            _manifestService.insertMasterInformation(ref masterInformationList);

            //INSERTA DESCRIPCION DE MERCANCIA
            _manifestService.insertWareDescription(ref wareDescriptionList);

            #endregion


            return null;
        }
    }
}
