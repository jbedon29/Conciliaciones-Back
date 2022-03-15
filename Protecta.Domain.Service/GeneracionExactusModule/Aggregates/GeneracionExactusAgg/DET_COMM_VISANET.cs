using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg
{
    public class DET_COMM_VISANET
    {
        public string SCERTYPE { get; set; }
        public int NBRANCH { get; set; }
        public int NPRODUCT { get; set; }
        public long NPOLICY { get; set; }
        public int NBRANCH_LED { get; set; }
        public int NCERTIF { get; set; }
        public long NRECEIPT { get; set; }
        public int NINSUR_AREA { get; set; }
        public string SBILLTYPE { get; set; }
        public int NBILLNUM { get; set; }
        public int NCURRENCY { get; set; }
        public long NPREMIUMN { get; set; }
        public int NINTERTYP { get; set; }
        public int NINTERMED { get; set; }
        public decimal NPERCENT { get; set; }
        public decimal NCOMMISSION { get; set; }
        public DateTime DEFFECDATE { get; set; }
        public int NBANK_CODE { get; set; } 
        public int NOPERACION_BANCO { get; set; }
        public int NTYPE { get; set; }
        public string STYP_COMM { get; set; }
        public long ID_UNICO { get; set; }
        public DateTime DCOMPDATE { get; set; }
        public int NUSERCODE { get; set; } 
        public string SKEY { get; set; }
    }
}
