using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Protecta.CrossCuting.Utilities;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;

namespace Protecta.Application.Service.Services.Email
{
    public class EmailService : IEmailService {
        private readonly EmailSettings _configuration;

        public EmailService () {

        }
        private EmailService (EmailSettings config) {
            _configuration = config;
        }
        public async Task<string> SenderEmailPlanilla(string planilla, Mensaje result, string email )
        {
            try
            {
                //if (email != null)
                {
                    string bodyResponse = string.Empty;
                    string subject = "Anulación de planilla";
                    var mm = new MailMessage();
                    MailAddress from = new MailAddress("operaciones_sctr@protectasecurity.pe", "PROTECTA SECURITY");
                    //subject = "Aviso de Rechazo de planilla- Plataforma Conciliaciones";
                    mm.From = from;
                    mm.To.Add(new MailAddress(email));
                    bodyResponse = prepareBody(planilla, result);
                    Console.WriteLine("el bodyResponse : " + bodyResponse);
                    mm.Body = bodyResponse;
                    mm.Subject = subject ;
                    mm.IsBodyHtml = true;
                    //string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    //LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add(av);
                    //lr.ContentId = "Logo";
                    //av.LinkedResources.Add (lr);
                    try
                    {
                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("operaciones_sctr@protectasecurity.pe", "0perac10nesSCTR$$_");
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el exception : " + ex);
                throw ex;
            }
        }
        private string prepareBody(string numeroPlanilla, Mensaje item)
        {
            string[] obs = item.observacion.TrimEnd('.').Split('.');
            string body = "";
            body += $"<center style='width: 100%; table-layout: fixed; background-color: #ffffff; padding-bottom: 40px;font-family: Verdana;'>";
            body += $"<div style='max-width: 600px; background-color: #ffffff; margin: 3px;'><table style='margin: 0 auto; width: 100%; max-width: 600px;'><tr>";
            body += $"<td style='width:75%'><span style='color:#6A2E92;text-align:left'> Protegemos lo que</span><span style ='color:#ed6e00;text-align:left'> más valoras </span></td>";
            body += $"<td style='width:25%'><img src='https://protectasecurity.pe/wp-content/uploads/2020/01/logo_Protecta-Security.png' width='145.25px' height ='32.75px' alt='logo' border='0' ></td>";
            body += $"</tr></table> <div style='background-color: #FF6E00; color : rgb(43, 13, 97) !important; height: 45px;'>";
            body += $"<h2 style='text-align: left; justify-content: left; justify-items: left; padding-left:20px; padding-top: 8px;'> Observaciones planilla SOAT</h2>";
            body += $"</div><div style='margin: 0 auto; width: 100%; max-width: 550px;' >";
            body += $"<h3 style='text-align: left; justify-content: left; justify-items: left; padding-left:5px!important; padding-right: 5px!important; font-family: Verdana; color: gray;'>Estimado Socio de negocio</h3>";
            //variables//
            body += $"<p style='text-align: left; justify-content: left; justify-items: left; padding-left: 20px; font-family: Verdana; color: gray;'> Te informamos que hemos procedido con el rechazo de la planilla N° {numeroPlanilla}, debido a la(s) siguiente(s) observacion(es) </p>";
            for (var i = 0; i < obs.Length; i++)
            {
                body += $"<p style='text-align: left; justify-content: left; justify-items: left; padding-left: 20px; font-family: Verdana; color: gray;'> {i + 1}. {obs[i].ToString()}";
            }
            //variables//
            body += $"<p style='text-align: left; justify-content: left; justify-items: left; padding-left: 20px; font-family: Verdana; color: gray;'> Solicitamos se realicen las correcciones en la Plataforma Digital.</p>";
            body += $"<p style='text-align: left; justify-content: left; justify-items: left; padding-left: 20px; font-family: Verdana; color: gray;'> Muchas Gracias.</p>";
            body += $"<h3 style='text-align: left; justify-content: left; justify-items: left; padding-left: 5px!important; padding-right: 5px!important; font-family: Verdana; color: gray;'>Equipo de Cobranzas.</h3></div>";
            body += $"<div style='background-color: #FF6E00; color : rgb(43, 13, 97);height: 10%;'> <div style='padding-top: 10px;padding-bottom: 10px;'>";
            body += $"<section style='font-size: 14px; text-align: center; padding-left: 20px; padding-right: 20px; padding-top: 9px;'>Este es un email automático, si tienes cualquier tipo de duda ponte en contacto con nosotros a través de nuestro servicio de atención al cliente al e-mail <b> canalcorporativo@protectasecurity.pe </b>, por favor no respondas a este mensaje.</section>";
            body += $"</div></div><div style='height: 100px;' ><div style='padding-top: 15px; color :#FF6E00;'><span> Encuentranos en:</span><table><tr>";
            body += $"<td><a href='https://protectasecurity.pe/' target ='_blank'><img src='https://i.ibb.co/kMmSk7D/web.jpg' alt ='web' border='0'></a ></td >";
            body += $"<td><a href='https://www.youtube.com/channel/UCLDUNm7ULAC4jbie_7S8beQ' target='_blank'><img src='https://i.ibb.co/Wy4p9t7/youtube.jpg' alt='youtube' border='0'></a></td>";
            body += $"<td><a href='https://www.facebook.com/ProtectaSecurity/' target='_blank'><img src='https://i.ibb.co/gmssGnb/facebook.jpg' alt='facebook' border='0'></a></td>";
            body += $"<td><a href='https://www.linkedin.com/company/359787/admin/' target='_blank'><img src='https://i.ibb.co/dKLspzX/likendin.jpg' alt='likendin' border='0'></a></td>";
            body += $"<td><a href='https://www.instagram.com/somosprotectores/' target='_blank'><img src='https://i.ibb.co/vqFNRPw/instagran.jpg' alt='instagran' border='0'></a></td>";
            body += $"</tr></table></div></div>";
            body += $"<table style='width:100%; height:10px; border-spacing:0px! important;'><tr><td style='background-color: #FF6E00; width: 50%;'></td><td style='background-color:rgb(43, 13, 97); width: 50%;'></td></tr></table></div></div></center>";
            return body;
        }
        //Método para enviar correo electrónico al usuario en sesión LAFT
        public string SenderEmailReportGen (string user, string email, string route, string reportId, string message, string startDate, string enDate, string desReport, string desOperType, string sbsFileType) {
            try {
                if (route != null) {
                    string bodyResponse = string.Empty;
                    string subject = string.Empty;
                    subject = "Generación de Reporte SBS - Plataforma LAFT";
                    var mm = new MailMessage ();
                    MailAddress from = new MailAddress ("operaciones_sctr@protectasecurity.pe", "PROTECTA SECURITY");
                    mm.From = from;
                    mm.To.Add (new MailAddress (email));
                    mm.Subject = subject;
                    bodyResponse = ComposeBodyReportGen (user, email, route, reportId, message, startDate, enDate, desReport, desOperType, sbsFileType, bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;
                    string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString (bodyResponse, null, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add (av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add (lr);

                    try {

                        using (SmtpClient smtp = new SmtpClient ("smtp.gmail.com", 587)) {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential ("operaciones_sctr@protectasecurity.pe", "0perac10nesSCTR$$_");
                            smtp.EnableSsl = true;
                            smtp.Send (mm);
                        }

                    } catch (Exception ex) {
                        ExceptionManager.resolve(ex);
                        return null;
                    }
                }
                return email;
            } catch (Exception ex) {
                ExceptionManager.resolve (ex);
                return null;
            }
        }

        public string ComposeBodyReportGen (string user, string email, string route, string reportId, string message, string startDate, string enDate, string desReport, string desOperType, string sbsFileType, string bodyResponse) {
            var reportCountName = "el reporte sbs ";
            var failreports = " no se pudo generar";

            if (route != null) {
                try {
                    if (sbsFileType == "txt") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Se acaba de generar "))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en texto plano "))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport.ToLower () + " con el tipo de operación " + desOperType.ToLower ()))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", "Por favor, puede descargar el reporte desde la plataforma "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", "LAFT"))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", "– opción monitoreo de reportes sbs, consultando el ID: "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", reportId))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Le pedimos que pueda verificarlo en la pantalla de monitoreo de reportes SBS."))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));

                    }
                    if (sbsFileType == "xls") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Se acaba de generar "))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en formato excel "))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport.ToLower () + " con el tipo de operación " + desOperType.ToLower ()))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", "Por favor, puede descargar el reporte desde la plataforma "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", "LAFT"))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", "– opción monitoreo de reportes sbs, consultando el ID: "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", reportId))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Le pedimos que pueda verificarlo en la pantalla de monitoreo de reportes SBS."))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));

                    }

                } catch (Exception ex) {
                    ExceptionManager.resolve (ex);
                    return null;
                }

            } else {
                try {
                    if (sbsFileType == "txt") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Le informamos que " + failreports))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en texto plano"))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport + " con el tipo de operación " + desOperType))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", " Le pedimos porfavor que pueda "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", " contactar a soporte. "))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));

                    }
                    if (sbsFileType == "xls") {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReportGen.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Nombre]", user)
                            .Replace ("[Resultado]", string.Format ("{0}", "Le informamos que " + failreports))
                            .Replace ("[Reporte]", string.Format ("<strong>{0}</strong>", reportCountName + "en excel"))
                            .Replace ("[NombreReporte]", string.Format ("<strong>{0}</strong>", desReport + " con el tipo de operación " + desOperType))
                            .Replace ("[FechaInicio]", string.Format ("<strong>{0}</strong>", startDate))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", enDate))
                            .Replace ("[Respuesta]", string.Format ("{0}", " Le pedimos porfavor que pueda "))
                            .Replace ("[Aplicacion]", string.Format ("<strong>{0}</strong>", " contactar a soporte. "))
                            .Replace ("[Indicacion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[IdProceso]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", " "))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));

                    }

                } catch (Exception ex) {
                    ExceptionManager.resolve (ex);
                    return null;
                }

            }
            return bodyResponse;
        }

        public string ComposeBodyCompRequest (string endDate, string user, string email, string cargo, string bodyResponse) {

            if (user != null) {

                if (user == "Alfredo Chan Way Diaz") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Como parte del conocimiento del cliente, se requiere tu confirmación respecto a si la fuerza de ventas de Comercial Rentas (Vida con Renta Temporal/Renta Total/Ahorro Total, según aplique) ha reportado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));
                    } catch (Exception ex) {
                        throw ex;
                    }
                } else if (user == "Diego Rosell Ramírez Gastón") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Como parte del conocimiento del cliente (contratantes de productos masivos), se requiere tu confirmación si se han presentado/identificado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));
                    } catch (Exception ex) {
                        throw ex;
                    }
                } else if (user == "Yvan Ruiz Portocarrero") {
                    try {
                        string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                        string readText = File.ReadAllText (path);
                        return readText

                            .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                            .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Por favor considerar si en el proceso de Emisión de pólizas (todos los productos de la compañía) si se han presentado/identificado las siguientes situaciones al cierre de "))
                            .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", endDate))
                            .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));
                    } catch (Exception ex) {
                        throw ex;
                    }
                }
            } else {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyRequest.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Mensaje]", string.Format ("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."))
                        .Replace ("[FechaFin]", string.Format ("<strong>{0}</strong>", " "))
                        .Replace ("[Link]", string.Format ("<strong>{0}</strong>", "http://190.216.170.173/ApplicationLAFT/"));

                } catch (Exception ex) {
                    Console.WriteLine ("la excepcion 2 : " + ex);
                    ExceptionManager.resolve (ex);
                    return null;
                }
            }
            return bodyResponse;
        }
        public string EmailSender (string user, string manager, string rol, string email) {
            try {
                if (email != null) {
                    string bodyResponse = string.Empty;
                    string subject = string.Empty;
                    subject = "Llenado de Formulario - Plataforma LAFT";
                    var mm = new MailMessage ();
                    MailAddress from = new MailAddress ("operaciones_sctr@protectasecurity.pe", "PROTECTA SECURITY");
                    mm.From = from;
                    mm.To.Add (new MailAddress (email));
                    mm.Subject = subject;
                    bodyResponse = ComposeBodyReviewForm (user, manager, email, rol, bodyResponse);
                    mm.Body = bodyResponse;
                    mm.IsBodyHtml = true;
                    string fileName = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "logo.png");
                    AlternateView av = AlternateView.CreateAlternateViewFromString (bodyResponse, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);
                    LinkedResource lr = new LinkedResource (fileName, MediaTypeNames.Image.Jpeg);
                    mm.AlternateViews.Add (av);
                    lr.ContentId = "Logo";
                    av.LinkedResources.Add (lr);

                    try {

                        using (SmtpClient smtp = new SmtpClient ("smtp.gmail.com", 587)) {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential ("operaciones_sctr@protectasecurity.pe", "0perac10nesSCTR$$_");
                            smtp.EnableSsl = true;
                            smtp.Send (mm);
                        }

                    } catch (Exception ex) {
                        throw ex;
                    }
                }
                return email;
            } catch (Exception ex) {
                Console.WriteLine (ex.ToString ());
            }
            return null;
        }

        public string ComposeBodyReviewForm (string user, string manager, string email, string cargo, string bodyResponse) {
            if (user != null) {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReviewForm.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Manager]", string.Format ("<strong>{0}</strong>", manager))
                        .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Por favor ingrese a esta URL: http://190.216.170.173/ApplicationLAFT"));

                } catch (Exception ex) {
                    ExceptionManager.resolve (ex);
                    return null;
                }

            } else {
                try {
                    string path = string.Format ("{0}{1}", "C:\\TemplatesLAFT\\", "BodyReviewForm.html");
                    string readText = File.ReadAllText (path);
                    return readText

                        .Replace ("[Usuario]", string.Format ("<strong>{0}</strong>", user))
                        .Replace ("[Manager]", string.Format ("<strong>{0}</strong>", manager))
                        .Replace ("[Instruccion]", string.Format ("<strong>{0}</strong>", "Por favor. Le pedimos que pueda contactar a soporte."));
                } catch (Exception ex) {
                    ExceptionManager.resolve (ex);
                    return null;
                }
            }
            return bodyResponse;
        }
    }
}