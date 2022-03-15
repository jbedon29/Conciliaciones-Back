
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.CrossCuting.Utilities.Files.IFiles
{
    public interface _ManageFile
    {
        Task<DataSet> Process( Stream file,string nameFile);
    }
}

