using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.Infrastructure.Data.Extensions;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ProductoModule.Aggregates.ProductoAgg;

namespace Protecta.Infrastructure.Data.ProductoModule.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public ProductoRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Producto>> ListarProducto()
        {
            Producto productoEntity;
            List<Producto> productoList = new List<Producto>();

            List<OracleParameter> parameters = new List<OracleParameter>();  

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));            

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PRODUCTO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    productoEntity = new Producto();
                    productoEntity.IdProducto = dr["ID_PRODUCTO"].ToString();
                    productoEntity.DescProducto = dr["VC_DESCRIPCION"].ToString();                    
                    productoList.Add(productoEntity);
                }
            }

            return Task.FromResult<List<Producto>>(productoList);
        }

        public Task<List<Producto>> ListarProductoSCTR()
        {
            Producto productoEntity;
            List<Producto> productoList = new List<Producto>();

            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.REA_PRODUCT_GRUPORAMO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    productoEntity = new Producto();
                    productoEntity.IdProducto = dr["COD_PRODUCT"].ToString();
                    productoEntity.DescProducto = dr["DES_PRODUCT"].ToString();
                    productoList.Add(productoEntity);
                }
            }

            return Task.FromResult<List<Producto>>(productoList);
        }
    }
}
