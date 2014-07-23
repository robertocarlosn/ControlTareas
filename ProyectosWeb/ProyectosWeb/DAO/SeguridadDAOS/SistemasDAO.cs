﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ProyectosWeb.Models.Seguridad;
using ProyectosWeb.Models;

namespace ProyectosWeb.DAO.SeguridadDAOS
{
    public class SistemasDAO
    {
        private SqlConnection _conn;

        public SistemasDAO(SqlConnection conn)
        {
            _conn = conn;
        }

        public DbQueryResult UpdateSistemas(Sistema sis)
        {
            DbQueryResult resultado= new DbQueryResult();
            resultado.Success=false;
            _conn.Open();
            try{
            SqlCommand cmSql = _conn.CreateCommand();
                
                cmSql.CommandText = "update sistemas set clavesistemas=@parm1,nombre=@parm2,cliente=@parm3,descripcion=@parm4, "
                + " fechainicio=CONVERT(DATE,@parm5,20), fechafinestimada=CONVERT(DATE,@parm6,20),fechafinreal=nullif(CONVERT(DATE,@parm7,20),'1900/01/01'),tecnologias=@parm8 where  idsistemas=@parmId";

            cmSql.Parameters.Add("@parm7", SqlDbType.VarChar);
            cmSql.Parameters["@parm7"].Value = sis.fechaFinReal;
            cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm4", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm5", SqlDbType.VarChar);
            cmSql.Parameters.Add("@parm6", SqlDbType.VarChar);            
            cmSql.Parameters.Add("@parm8", SqlDbType.VarChar);

            cmSql.Parameters.Add("@parmId", SqlDbType.Int);
            cmSql.Parameters["@parm1"].Value = sis.clave;
            cmSql.Parameters["@parm2"].Value = sis.nombre;
            cmSql.Parameters["@parm3"].Value = sis.cliente;
            cmSql.Parameters["@parm4"].Value = sis.descripcion;
            cmSql.Parameters["@parm5"].Value = sis.fechaInicio;
            cmSql.Parameters["@parm6"].Value = sis.fechaFinEstimada;            
            cmSql.Parameters["@parm8"].Value = sis.tecnologias;

            cmSql.Parameters["@parmId"].Value = sis.idSistema;
            int exito = cmSql.ExecuteNonQuery();
            if(exito>0){
            resultado.Success=true;
            }
            }catch(Exception ex){
                resultado.ErrorMessage = ""+ex.Message;
            }
            _conn.Close();
            return resultado;
        }

        public List<Sistema> getSistemas()
        {
            _conn.Open();
            List<Sistema> listado = new List<Sistema>();
            SqlCommand cmSql = _conn.CreateCommand();

            cmSql.CommandText = "select * from sistemas p where p.Estado=0";
            
            SqlDataAdapter da = new SqlDataAdapter(cmSql);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                DataTable dtDatos = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int g1 = 0; g1 < ds.Tables[0].Rows.Count; g1++)
                    {
                        DataRow drDatos = dtDatos.Rows[g1];
                        Sistema p = new Sistema();
                        p.idSistema = int.Parse(drDatos["idsistemas"].ToString());
                        p.nombre = drDatos["nombre"].ToString();
                        listado.Add(p);
                    }
                }

            }
            _conn.Close();
            return listado;
        }
    }
}