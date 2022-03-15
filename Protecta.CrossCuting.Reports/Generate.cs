using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Protecta.CrossCuting.Reports
{
    public class Generate
    {
        public string GeneratePDF(DataTable dt1, DataTable dt2)
        {
            string base64String = string.Empty;
            try
            {
                ReportDataSource dsOBJ = new ReportDataSource();
                dsOBJ.Name = "TblCuponPago";
                dsOBJ.Value = new DataTable();

                ReportDataSource dsOBJ2 = new ReportDataSource();
                dsOBJ.Name = "TblLugaresPago";
                dsOBJ.Value = dt2;

                IEnumerable<ReportDataSource> datasets = new List<ReportDataSource> { dsOBJ, dsOBJ2 };
                LocalReport localReport = new LocalReport();
                string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //localReport.ReportPath = startupPath + @"Reports\\ReportError.rdlc";
                localReport.ReportPath = @"C:\Report\CuponPago.rdlc";

                ReportParameter[] reportParameter = new ReportParameter[9];
                reportParameter[0] = new ReportParameter("RAMO", "sad");
                reportParameter[1] = new ReportParameter("POLIZA", "asd");
                reportParameter[2] = new ReportParameter("MONEDA","asd");
                reportParameter[3] = new ReportParameter("VIGENCIA_DESDE", "asd");
                reportParameter[4] = new ReportParameter("VIGENCIA_HASTA", "asd");
                reportParameter[5] = new ReportParameter("ASEGURADO","asd");
                reportParameter[6] = new ReportParameter("DOCUMENTO", "asd");
                reportParameter[7] = new ReportParameter("DIRECCION", "sd");
                reportParameter[8] = new ReportParameter("INTERMEDIARIO", "asd");
                reportParameter[9] = new ReportParameter("CUPON_CABECERA", "ad");

                localReport.SetParameters(reportParameter);
                localReport.Refresh();
                foreach (ReportDataSource datasource in datasets)
                {
                    localReport.DataSources.Add(datasource);
                }


                //Renderizado
                string deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat></DeviceInfo>";
                Warning[] warnings;
                string[] streams;
                string mimeType;
                byte[] renderedBytes;
                string encoding;
                string fileNameExtension;


                localReport.Refresh();
                renderedBytes = localReport.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                base64String = Convert.ToBase64String(renderedBytes, 0, renderedBytes.Length);
            }
            catch (Exception ex)
            {
                base64String = null;
            }
            return base64String;
        }
    }
}
