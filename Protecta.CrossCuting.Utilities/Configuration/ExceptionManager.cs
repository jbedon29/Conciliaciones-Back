using System;
using System.Collections.Generic;
using System.Linq;


namespace Protecta.CrossCuting.Utilities.Configuration
{
    public class ExceptionManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExceptionManager));
        public static void resolve(Exception ex){
            log.Error("---------------------------------------------------------------------------");
            log.Error("Mensaje:: " + ex.Message);
            log.Error("Origen:: " + ex.Source);
            log.Error("Detalle::",ex);
        }

    }
}