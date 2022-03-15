using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos
{
    public class PRO_RESOURCESDto
    {
        public long NIDRESOURCE { get; set; }
        public long NIDFATHER { get; set; }
        public string SNAME { get; set; }
        public string SDESCRIPTION { get; set; }
        public string SHTML { get; set; }
        public string SSTATE { get; set; }
        public DateTime DREGISTER { get; set; }
        public long NUSERREGISTER { get; set; }
        public DateTime DUPDATE { get; set; }
        public long NUSERUPDATE { get; set; }
        public PRO_RESOURCESDto FATHER { get; set; }
        public IEnumerable<PRO_RESOURCESDto> CHILDREN { get; set; }
        public string STAG { get; set; }
        public decimal NVALIDATE { get; set; }
    }
}
