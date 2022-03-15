using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class PrintCupon
    {
       public int cuponera { get; set; }
       public int cuponInicial { get; set; }
       public int cuponFinal { get; set; }
       public int copias { get; set; }
       public Boolean flgImpCupon { get; set; }   
       public Boolean flgCronograma { get; set; }  
    }
}
