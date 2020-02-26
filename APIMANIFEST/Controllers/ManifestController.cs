using Common;
using Domain;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ScrapySharp.Extensions;
using Service.Implementations.Manifest;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            int tableId = 0;
            int trId = 0;
            string startDate = Convert.ToDateTime("10/02/2020").ToString("dd/MM/yyyy");//.ToShortDateString("dd/MM/yyyy");
            string endDate = Convert.ToDateTime("10/02/2020").ToString("dd/MM/yyyy"); //.ToShortDateString();
            string url = "http://www.aduanet.gob.pe/cl-ad-itconsmanifiesto/manifiestoITS01Alias?accion=consultaManifiesto&fec_inicio=" + startDate + "&fec_fin=" + endDate + "&cod_terminal=0000&tamanioPagina=20";

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
                                        manifestSDDetailObj.DAT_DATETRANSMISSIONDOCUMENT = Convert.ToDateTime(td[12].InnerHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Trim());
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

            List<TBL_ADU_TRACK> trackList = new List<TBL_ADU_TRACK>();
            TBL_ADU_WEBTRACKING webTrackingObj;
            TBL_ADU_TRACK trackObj;
            string masterGuidePrefix = "";

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disabled-gpu");
            options.AddArgument("--user-agent=Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/60.0.3112.50 Safari/537.36");
            ChromeDriver chromeDriver;
            //IWebElement originElement, destinationElement, pieceElement, weightElement, statusElement,element = null;
            foreach (var item in manifestSDDetailList)
            {
                #region DESTINOS DE VUELOS
                webTrackingsList = _manifestService.getWebsTracking();
                masterGuidePrefix = item.VCH_DIRECTMASTERGUIDE.Substring(0, 3);
                webTrackingObj =  webTrackingsList.Where(x => x.CHR_PREFIX == masterGuidePrefix).FirstOrDefault();
                if (webTrackingObj != null)
                {
                    IReadOnlyCollection<IWebElement> resumeList = Array.Empty<IWebElement>();
                    IReadOnlyCollection<IWebElement> awbDetails = Array.Empty<IWebElement>();
                    IWebElement originElement = null;
                    IWebElement destinationElement = null;
                    IWebElement pieceElement = null;
                    IWebElement weightElement = null;
                    IWebElement statusElement = null;
                    IWebElement element = null;

                    switch (webTrackingObj.CHR_PREFIX)
                    {

                        case "001":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                element = chromeDriver.FindElementById("airwayBills0.awbNumber");
                                Thread.Sleep(3000);
                                element.SendKeys(item.VCH_DIRECTMASTERGUIDE.Substring(3, 8));
                                element = chromeDriver.FindElementByName("track10Search");
                                element.Click();
                                Thread.Sleep(12000);
                                element = chromeDriver.FindElementByCssSelector("li.awbDetailsHref a");
                                element.Click();
                                Thread.Sleep(10000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("div.awbShipmentDescriptionDetails div.shipData table tr:nth-child(2) td");
                            }
                            catch (Exception)
                            {
                                if (chromeDriver.PageSource.ToString().Contains("Access Denied"))
                                {
                                    try
                                    {
                                        Thread.Sleep(600000);
                                        //chromeDriver.Quit();
                                        chromeDriver.Navigate().GoToUrl(url);
                                        element = chromeDriver.FindElementById("airwayBills0.awbNumber");
                                        Thread.Sleep(3000);
                                        element.SendKeys(item.VCH_DIRECTMASTERGUIDE.Substring(3, 8));
                                        element = chromeDriver.FindElementByName("track10Search");
                                        element.Click();
                                        Thread.Sleep(10000);
                                        element = chromeDriver.FindElementByCssSelector("li.awbDetailsHref a");
                                        element.Click();
                                        Thread.Sleep(10000);
                                        awbDetails = chromeDriver.FindElementsByCssSelector("div.awbShipmentDescriptionDetails div.shipData table tr:nth-child(2) td");
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                            }
                            if (awbDetails.Count() > 0 && awbDetails.First().Text != null)
                            {
                                trackObj = new TBL_ADU_TRACK();
                                trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                trackObj.INT_PIECES = Convert.ToInt32(awbDetails.First().Text);
                                trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.ElementAt(2).Text == "L" ? Convert.ToDouble(awbDetails.ElementAt(1).Text) / 2.205 : Convert.ToDouble(awbDetails.ElementAt(1).Text)); //Guardado en kilos;
                                trackObj.NUM_VOLUME = Convert.ToDecimal(awbDetails.ElementAt(3).Text.Replace("MC", "").Trim());
                                trackObj.VCH_ORIGIN = awbDetails.ElementAt(4).Text;
                                trackObj.VCH_DESTINATION = awbDetails.ElementAt(5).Text;
                                trackObj.VCH_CONNECTION = awbDetails.ElementAt(6).Text;
                                trackList.Add(trackObj);
                            }
                            chromeDriver.Quit();
                            break;
                        case "006":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?awbNumber={item.VCH_DIRECTMASTERGUIDE}&timeZoneOffset=300";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(10000);
                                IReadOnlyCollection<IWebElement> flighDetailsList = chromeDriver.FindElementsByCssSelector("div.dc-bigger-font a span.dc-airport-code");
                                IReadOnlyCollection<IWebElement> productDetailsList = chromeDriver.FindElementsByCssSelector("ul.panel-group li:nth-child(2) div.panel-body div.ng-scope div.col-md-4 p");
                                if (flighDetailsList.Count() > 0 && productDetailsList.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = flighDetailsList.First().Text;
                                    trackObj.VCH_DESTINATION = flighDetailsList.Last().Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(productDetailsList.ElementAt(0).Text.Replace("Pieces\r\n", "").Trim());
                                    trackObj.VCH_SERVICE = productDetailsList.ElementAt(1).Text.Replace("Product\r\n", "").Trim();
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(Convert.ToDouble(productDetailsList.ElementAt(2).Text.Replace("Weight\r\n", "").Replace("lb", "").Trim()) / 2.205); //Guardado en kilos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "014":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}/?s_acn={masterGuidePrefix}&s_sref={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(10000);
                                IReadOnlyCollection<IWebElement> tableRows = chromeDriver.FindElementsByCssSelector("div#track-cargo-resp table tbody tr td");
                                IWebElement captionElement = chromeDriver.FindElementByCssSelector("div#track-cargo-resp table caption.main-info span.pull-right");
                                if (tableRows.Count() > 0 && captionElement != null)
                                {
                                    string[] elements = captionElement.Text.Split(Convert.ToChar("→"));
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = elements.First();
                                    trackObj.VCH_DESTINATION = elements.Last();
                                    trackObj.INT_PIECES = Convert.ToInt32(tableRows.ElementAt(0).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(tableRows.ElementAt(1).Text); //Guardado en kilos;
                                    trackObj.VCH_STATUS = tableRows.ElementAt(2).Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();

                            break;
                        case "016":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?id={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}&pfx={masterGuidePrefix}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(10000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("ul.listClass li");
                                if (awbDetails.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_CONFIRMATION = awbDetails.ElementAt(1).Text;
                                    trackObj.VCH_STATUS = awbDetails.ElementAt(2).Text;
                                    trackObj.VCH_ORIGIN = awbDetails.ElementAt(3).Text;
                                    trackObj.VCH_DESTINATION = awbDetails.ElementAt(4).Text;
                                    trackObj.VCH_SERVICE = awbDetails.ElementAt(5).Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(6).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(Convert.ToDouble(awbDetails.ElementAt(7).Text) / 2.205); //Guardado en kilos;
                                    trackObj.NUM_VOLUME = Convert.ToDecimal(Convert.ToDouble(awbDetails.ElementAt(8).Text) / 35.315); //Volumen en metros cubicos
                                    trackObj.VCH_PRODUCT = awbDetails.ElementAt(9).Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                            case "044": case "230":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?AWBPrefix={masterGuidePrefix}&AWBNo={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                originElement = chromeDriver.FindElementById("lblOrigin");
                                destinationElement = chromeDriver.FindElementById("lblDestination");
                                pieceElement = chromeDriver.FindElementById("lblPcs");
                                weightElement = chromeDriver.FindElementById("lblGrossWt");
                                statusElement = chromeDriver.FindElementById("lblLatestActivity");
                                if (originElement != null && destinationElement != null && pieceElement != null && weightElement != null && statusElement != null)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = originElement.Text;
                                    trackObj.VCH_STATUS = statusElement.Text;
                                    trackObj.VCH_DESTINATION = destinationElement.Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(pieceElement.Text.Replace("P", "").Trim());
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(weightElement.Text.Replace("Kgs", "").Trim()); //Guardado en kilos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "057": case "074":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}/{masterGuidePrefix}-{item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}";
                            chromeDriver.Navigate().GoToUrl(url);
                            Thread.Sleep(20000);

                            IWebElement headerElement = null;
                            IWebElement productElement = null;
                            try
                            {
                                headerElement = chromeDriver.FindElementByCssSelector("afkl-detail-header div.row div.col-12 h1");
                                productElement = chromeDriver.FindElementByCssSelector("div#right-part ul.caret li:nth-child(4)").Text.Contains("Protegido") ?
                                                    chromeDriver.FindElementByCssSelector("div#right-part ul.caret li:nth-child(3)") :
                                                    chromeDriver.FindElementByCssSelector("div#right-part ul.caret li:nth-child(4)");

                                if (headerElement != null && productElement != null)
                                {
                                    string[] elements = headerElement.Text.Split(' ');
                                    string[] detailElements = productElement.Text.Split(' ');
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = elements.First();
                                    trackObj.VCH_DESTINATION = elements.Last();
                                    trackObj.INT_PIECES = Convert.ToInt32(detailElements.First());
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(detailElements.ElementAt(2)); //Guardado en kilos;
                                    trackObj.NUM_VOLUME = Convert.ToDecimal(detailElements.ElementAt(4).Replace(",", ".")); //Guardado en metros cubicos;
                                    trackList.Add(trackObj);
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "075":
                        case "125":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                element = chromeDriver.FindElementById("awb");
                                Thread.Sleep(3000);
                                element.SendKeys($"{masterGuidePrefix}-{item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}");
                                element = chromeDriver.FindElementByCssSelector("input[type='submit']");
                                element.Click();
                                Thread.Sleep(7000);
                                element = chromeDriver.FindElementByTagName("iframe");
                                element = chromeDriver.SwitchTo().Frame(element).FindElement(By.CssSelector("div.container div.subtitleDetail"));
                                if (element != null)
                                {
                                    string[] elementDetails = element.Text.Replace("(", "").Replace(")", "").Split(' ');
                                    string[] destinationElements = elementDetails.ElementAt(4).Split('\r');
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.INT_PIECES = Convert.ToInt32(destinationElements.ElementAt(1).Replace("\n", "").Trim());
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(elementDetails.ElementAt(7).Replace(",", ".")); //Guardado en kilos;
                                    trackObj.VCH_ORIGIN = elementDetails.ElementAt(1);
                                    trackObj.VCH_DESTINATION = destinationElements.First();
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "139":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                element = chromeDriver.FindElementByName("ALP");
                                element.SendKeys(masterGuidePrefix);
                                element = chromeDriver.FindElementByName("AWB");
                                element.SendKeys(item.VCH_DIRECTMASTERGUIDE.Substring(3, 8));
                                Thread.Sleep(3000);
                                element = chromeDriver.FindElementByClassName("btn-trackguide");
                                element.Click();
                                Thread.Sleep(7000);
                                resumeList = chromeDriver.FindElementsByCssSelector("div#printArea p.or-ds");
                                awbDetails = chromeDriver.FindElementsByCssSelector("div#printArea div.col-md-12");
                            }
                            catch (Exception)
                            {
                            }
                            if (resumeList.Count() > 0 && awbDetails.Count() > 0)
                            {
                                string[] awbElements = awbDetails.ElementAt(1).Text.Replace("Detalles de la guía:", "").Replace("Piezas", "").Replace("Kg.", "").Trim().Split('/');
                                trackObj = new TBL_ADU_TRACK();
                                trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                trackObj.INT_PIECES = Convert.ToInt32(awbElements.First());
                                trackObj.NUM_WEIGHT = Convert.ToDecimal(awbElements.Last()); //Guardado en kilos;
                                trackObj.VCH_ORIGIN = resumeList.First().Text;
                                trackObj.VCH_DESTINATION = resumeList.Last().Text;
                                trackObj.VCH_STATUS = awbDetails.Last().Text.Replace("ÚLTIMO ESTATUS:", "").Trim();
                                trackList.Add(trackObj);
                            }
                            chromeDriver.Quit();
                            break;
                        case "144":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}/{item.VCH_DIRECTMASTERGUIDE}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                awbDetails = chromeDriver.FindElementsByCssSelector("div#tab-0 b");
                                IWebElement trackStatus = chromeDriver.FindElementByCssSelector("div#tab-0 table.tracking-summary tbody tr:nth-child(3) td:first-child");
                                if (awbDetails.Count() > 0 && trackStatus != null)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(1).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.ElementAt(2).Text.Replace("kg", "").Trim()); //Guardado en kilos;
                                    trackObj.VCH_ORIGIN = awbDetails.ElementAt(4).Text;
                                    trackObj.VCH_DESTINATION = awbDetails.Last().Text;
                                    trackObj.VCH_STATUS = trackStatus.Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "145":
                        case "045":
                            chromeDriver = new ChromeDriver(options);
                            url = "https://www.track-trace.com/aircargo"; //URL TRACK TRACE
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(3000);
                                element = chromeDriver.FindElementByCssSelector("div.tingle-modal-box__footer button:first-child");
                                element.Click();
                                Thread.Sleep(3000);
                                element = chromeDriver.FindElementById("number");
                                element.SendKeys(item.VCH_DIRECTMASTERGUIDE);
                                element = chromeDriver.FindElementByName("commit");
                                element.Click();
                                Thread.Sleep(7000);
                                element = chromeDriver.FindElementByTagName("iframe");
                                awbDetails = chromeDriver.SwitchTo().Frame(element).FindElements(By.CssSelector("table.caja1:first-child tbody p"));
                                if (awbDetails.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = awbDetails.ElementAt(1).Text;
                                    trackObj.VCH_PRODUCT = awbDetails.ElementAt(3).Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(5).Text);
                                    trackObj.VCH_DESTINATION = awbDetails.ElementAt(9).Text;
                                    trackObj.VCH_SERVICE = awbDetails.ElementAt(11).Text;
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.ElementAt(13).Text.Replace("kg", "").Trim()); //Guardado en kilos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "180":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?awbNO={item.VCH_DIRECTMASTERGUIDE}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(7000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("div#react-tabs-1 div.form-group div.card div.card-body");
                                if (awbDetails.Count() > 0)
                                {
                                    string[] firstElements = awbDetails.First().Text.Split(' ');
                                    string[] secondElements = awbDetails.ElementAt(1).Text.Split('-');

                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.INT_PIECES = Convert.ToInt32(firstElements.First());
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(firstElements.ElementAt(3)); //Guardado en kilos;
                                    trackObj.VCH_ORIGIN = secondElements.First();
                                    trackObj.VCH_DESTINATION = secondElements.Last();
                                    trackObj.VCH_PRODUCT = awbDetails.ElementAt(2).Text;
                                    trackObj.VCH_SERVICE = awbDetails.Last().Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "369":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?pe={masterGuidePrefix}&se={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(10000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("table#tblAWBInfo tbody tr:nth-child(2) td");
                                if (awbDetails.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = awbDetails.First().Text;
                                    trackObj.VCH_DESTINATION = awbDetails.ElementAt(1).Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(2).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.Last().Text); //Guardado en kilos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "489":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                Thread.Sleep(3000);
                                element = chromeDriver.FindElementById("P857_AWBNUM");
                                element.SendKeys(item.VCH_DIRECTMASTERGUIDE.Substring(3, 8));
                                element = chromeDriver.FindElementById("P857_TRACK");
                                element.Click();
                                Thread.Sleep(7000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("table#report_R25280730414418592 tr:last-child table tr:last-child td font");
                                if (awbDetails.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = awbDetails.ElementAt(2).Text;
                                    trackObj.VCH_DESTINATION = awbDetails.ElementAt(3).Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(4).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.ElementAt(5).Text); //Guardado en kilos;
                                    trackObj.NUM_VOLUME = Convert.ToDecimal(awbDetails.ElementAt(6).Text); //Guardado en metros cubicos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "605":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?awbnumber={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}&airlineprefix={masterGuidePrefix}&lang=es";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                awbDetails = chromeDriver.FindElementsByCssSelector("table#tblAWBInfo tbody tr:nth-child(2) td");
                                originElement = chromeDriver.FindElementById("lblOriginT");
                                destinationElement = chromeDriver.FindElementById("lblDestinationT");
                                pieceElement = chromeDriver.FindElementById("lblTotalPieces");
                                weightElement = chromeDriver.FindElementById("lblTotalWeight");
                                if (originElement != null && destinationElement != null && pieceElement != null && weightElement != null)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = originElement.Text.Split(' ').First();
                                    trackObj.VCH_DESTINATION = destinationElement.Text.Split(' ').First();
                                    trackObj.INT_PIECES = Convert.ToInt32(pieceElement.Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(weightElement.Text.Replace(",", ".")); //Guardado en kilos;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;

                        case "729":
                        case "530":
                        case "202":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                element = chromeDriver.FindElementByName("txtPrefix");
                                element.SendKeys(masterGuidePrefix);
                                element = chromeDriver.FindElementByName("txtNumber");
                                element.SendKeys(item.VCH_DIRECTMASTERGUIDE.Substring(3, 8));
                                Thread.Sleep(3000);
                                element = chromeDriver.FindElementByCssSelector("input[type='submit']");
                                element.Click();
                                Thread.Sleep(7000);
                                awbDetails = chromeDriver.FindElementsByCssSelector("tr#trackShiptable1row00 td");
                                if (awbDetails.Count() > 0)
                                {
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = awbDetails.ElementAt(2).Text;
                                    trackObj.VCH_DESTINATION = awbDetails.ElementAt(3).Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(awbDetails.ElementAt(4).Text);
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(awbDetails.ElementAt(5).Text); //Guardado en kilos;
                                    trackObj.NUM_VOLUME = Convert.ToDecimal(awbDetails.ElementAt(6).Text); //Guardado en metros cubicos;
                                    trackObj.VCH_PRODUCT = awbDetails.ElementAt(7).Text;
                                    trackObj.VCH_SERVICE = awbDetails.ElementAt(8).Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
                            break;
                        case "996":
                            chromeDriver = new ChromeDriver(options);
                            url = $"{webTrackingObj.VCH_LINK}?prefix={masterGuidePrefix}&Serial={item.VCH_DIRECTMASTERGUIDE.Substring(3, 8)}";
                            chromeDriver.Navigate().GoToUrl(url);
                            try
                            {
                                awbDetails = chromeDriver.FindElementsByCssSelector("table table tr:last-child td");
                                originElement = chromeDriver.FindElementByCssSelector("table table tr:nth-child(5) td:nth-child(3)");
                                destinationElement = chromeDriver.FindElementByCssSelector("table table tr:nth-child(6) td:nth-child(3)");
                                if (awbDetails.Count() > 0 && originElement != null && destinationElement != null)
                                {
                                    string[] weightAndPieces = awbDetails.ElementAt(7).Text.Split('/');
                                    trackObj = new TBL_ADU_TRACK();
                                    trackObj.NUM_MANIFESTSHIPDETDOCID = item.DEC_MANIFESTSHIPDETDOCID;
                                    trackObj.VCH_DIRECTMASTERGUIDE = item.VCH_DIRECTMASTERGUIDE;
                                    trackObj.VCH_ORIGIN = originElement.Text;
                                    trackObj.VCH_DESTINATION = destinationElement.Text;
                                    trackObj.INT_PIECES = Convert.ToInt32(weightAndPieces.First());
                                    trackObj.NUM_WEIGHT = Convert.ToDecimal(weightAndPieces.Last()); //Guardado en kilos;
                                    trackObj.VCH_STATUS = awbDetails.ElementAt(8).Text;
                                    trackList.Add(trackObj);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            chromeDriver.Quit();
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

            //INSERTA EL TRACKING DE VUELOS
            _manifestService.insertTracks(ref trackList);

            #endregion


            return Json(_manifestService.callStoreProcedureManifest());
        }
    }
}
