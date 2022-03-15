using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.Drawing;
using ExcelDataReader;
using System.Data;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Protecta.CrossCuting.Utilities.Files.IFiles;
using System.Linq;

namespace Protecta.CrossCuting.Utilities.ManageFiles
{
    public class ManageExcel : _ManageFile
    {
        private string GetExtension(string value)
        {
            string[] Array = value.Split('.');
            return Array[Array.Length - 1];
        }

        public async Task<DataSet> Process(Stream file, string NameFile)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                DataSet Data = null;
                reader = (GetExtension(NameFile) == "xls") ? ExcelReaderFactory.CreateBinaryReader(file) : ExcelReaderFactory.CreateOpenXmlReader(file);

                if (reader != null)
                {
                    Data = reader.AsDataSet();
                    reader.Close();
                }
                return await Task.FromResult<DataSet>(Data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}