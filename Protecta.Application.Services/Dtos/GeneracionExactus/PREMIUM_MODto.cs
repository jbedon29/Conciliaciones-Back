using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.GeneracionExactus
{
    public class PREMIUM_MODto
    {
        public long NRECEIPT { get; set; }
        public int NPRODUCT { get; set; }
        public int NBRANCH { get; set; }
        public string SCERTYPE { get; set; }
        public int NDIGIT { get; set; }
        public int NPAYNUMBE { get; set; }
        public int NTRANSAC { get; set; }
        public long NAMOUNT { get; set; }
        public long NBALANCE { get; set; }
        public int NBORDEREAUX { get; set; }
        public string SCESSICOI { get; set; }
        public DateTime DCOMPDATE { get; set; }
        public int NCURRENCY { get; set; }
        public string SIND_REVER { get; set; }
        public int NINT_MORA { get; set; }
        public string SINTERMEI { get; set; }
        public string SPAY_FORM { get; set; }
        public DateTime DPOSTED { get; set; }
        public long NPREMIUM { get; set; }
        public DateTime DSTATDATE { get; set; }
        public string SSTATISI { get; set; }
        public int NUSERCODE { get; set; }
        public DateTime DLEDGERDAT { get; set; }
        public int NTYPE { get; set; }
        public int NEXCHANGE { get; set; }
        public string SINDASSOCPRO { get; set; }
        public int NID { get; set; }

        /*Agregado*/
        public int NBANK_CODE { get; set; }
        public long NCASH_MOV { get; set; }
        public int NBILLNUM { get; set; }
        public string SBILLTYPE { get; set; }

    }
}
