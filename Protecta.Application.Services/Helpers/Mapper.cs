using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Helpers
{
    public class MMapper
    {
        static public IEnumerable<PRO_RESOURCES> ConvertEntities(IEnumerable<PRO_RESOURCES> entities)
        {
            foreach (PRO_RESOURCES padre in entities)
            {
                padre.CHILDREN = new List<PRO_RESOURCES>();
                foreach (PRO_RESOURCES hijo in entities)
                {
                    if (hijo.NIDFATHER == padre.NIDRESOURCE)
                    {
                        ((List<PRO_RESOURCES>)(padre.CHILDREN)).Add(hijo);
                    }
                }

            }
            entities = entities.Where(x => x.NIDFATHER == 0);
            return entities.ToList(); ;
        }

        static public IEnumerable<PRO_RESOURCES> ConvertEntitiesFathers(
            IEnumerable<PRO_RESOURCES> sheets,
            IEnumerable<PRO_RESOURCES> allResourcesEntities
            )
        {
            IEnumerable<PRO_RESOURCES> bag = sheets.ToList();
            foreach (PRO_RESOURCES sheet in sheets)
            {
                SearchFather(sheet, allResourcesEntities, ref bag);
            }
            return bag;
        }

        private static void SearchFather(PRO_RESOURCES sheet, IEnumerable<PRO_RESOURCES> allResourcesEntities, ref IEnumerable<PRO_RESOURCES> bag)
        {
            PRO_RESOURCES father = null;
            if (sheet.NIDFATHER != 0)
            {
                father = allResourcesEntities.Where(x => x.NIDRESOURCE == sheet.NIDFATHER).First();
                bag = bag.Union(new List<PRO_RESOURCES> { father }, new ProductComparer());
                SearchFather(father, allResourcesEntities, ref bag);
            }
            return;
        }

        private class ProductComparer : IEqualityComparer<PRO_RESOURCES>
        {
            // Products are equal if their names and product numbers are equal.
            public bool Equals(PRO_RESOURCES x, PRO_RESOURCES y)
            {

                return x.NIDRESOURCE == y.NIDRESOURCE;
            }

            public int GetHashCode(PRO_RESOURCES x)
            {
                return (int)x.NIDRESOURCE;
            }

        }

        static public IEnumerable<PRO_RESOURCES> UnconvertEntitiesFathers(
            IEnumerable<PRO_RESOURCES> allResourcesEntities
            )
        {
            IEnumerable<PRO_RESOURCES> treeNodes = ConvertEntities(allResourcesEntities);
            IEnumerable<PRO_RESOURCES> bag = new List<PRO_RESOURCES>();
            return SearchSheet(treeNodes, ref bag);
        }

        private static IEnumerable<PRO_RESOURCES> SearchSheet(IEnumerable<PRO_RESOURCES> treeNodes, ref IEnumerable<PRO_RESOURCES> bag)
        {
            foreach (PRO_RESOURCES node in treeNodes)
            {
                if (node.CHILDREN.Count() > 0)
                {
                    SearchSheet(node.CHILDREN, ref bag);
                }
                else
                {
                    List<PRO_RESOURCES> tmpBag = bag.ToList();
                    tmpBag.Add(node);
                    bag = tmpBag;
                }
            }
            return bag;
        }
    }
}
