using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.GeneracionExactus
{
    public class PV_PAIDWAYDto
    {
        public int NBRANCH
        {
            get;
            set;
        }

        public int NPRODUCT
        {
            get;
            set;
        }

        public long NPOLICY
        {
            get;
            set;
        }

        public long NCERTIF
        {
            get;
            set;
        }

        public int NIDPAIDTYPE
        {
            get;
            set;
        }

        public string SOPERATION_NUMBER
        {
            get;
            set;
        }

        public int NCURRENCY
        {
            get;
            set;
        }

        public string NAMOUNT
        {
            get;
            set;
        }
    }
}
