﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Seguridad;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAOS.Seguridad
{
   public class PantallaDAO
    {
        private SqlConnection _conn;
        private Consultas _consultas;
        public PantallaDAO(SqlConnection conn)
        {
           _conn=conn;
           _consultas = new Consultas(_conn);
        }
        public DbQueryResult  registrarPantalla(Pantalla pantalla)
        {
            DbQueryResult resultado = new DbQueryResult();          
            _conn.Open();
            try
            {                              
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                bool existe = _consultas.existeEnDB("select * from pantallas p where p.idasp='" + pantalla.idAsp + "'");

                 if (!existe)
                   {
                       cmSql.CommandText = " insert into pantallas(nombre,descripcion,idasp,idmodulo) values(@parm1,@parm2,@parm3,@parm4)";
                       cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                       cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                       cmSql.Parameters["@parm1"].Value = pantalla.nombre.Trim();
                       cmSql.Parameters["@parm2"].Value = pantalla.descripcion.Trim();
                       cmSql.Parameters["@parm3"].Value = pantalla.idAsp.Trim();
                       cmSql.Parameters["@parm4"].Value = pantalla.idModulo;
                       int exito = cmSql.ExecuteNonQuery();
                       if (exito > 0)
                       {
                           resultado.Success = true;
                       }
                   }
                   else
                   {
                       resultado.ErrorMessage = "existe";
                   }
                
            }
            catch (Exception ex) {
                resultado.ErrorMessage = ex.Message;
            }
            _conn.Close();                          
            return resultado;
        }
        public DbQueryResult UpdatePantalla(Pantalla pantalla)
        {
            DbQueryResult resultado = new DbQueryResult();
            _conn.Open();
            try
            {
                resultado.Success = false;
                SqlCommand cmSql = _conn.CreateCommand();

                
                cmSql.CommandText = " update pantallas set nombre=@parm1, descripcion=@parm2, idasp=@parm3 where idpantalla=@parm4";
                    cmSql.Parameters.Add("@parm1", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm2", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm3", SqlDbType.VarChar);
                    cmSql.Parameters.Add("@parm4", SqlDbType.Int);
                    cmSql.Parameters["@parm1"].Value = pantalla.nombre.Trim();
                    cmSql.Parameters["@parm2"].Value = pantalla.descripcion.Trim();
                    cmSql.Parameters["@parm3"].Value = pantalla.idAsp.Trim();
                    cmSql.Parameters["@parm4"].Value = pantalla.idPantalla;
                    int exito = cmSql.ExecuteNonQuery();
                    if (exito > 0)
                    {
                        resultado.Success = true;
                    }                
            }
            catch (Exception ex)
            {
                resultado.ErrorMessage = ex.Message;
            }
            _conn.Close();
            return resultado;
        }

        public Pantalla getPantalla(String nombre, String idAsp) {
            Pantalla p = new Pantalla();            
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                if (nombre!=null)
                {
                    cmSql.CommandText = "select * from pantallas m where m.nombre='" + nombre.Trim() + "'";
                }else
                if (idAsp != null && nombre == null)
                {
                    cmSql.CommandText = "select * from pantallas m where m.idasp='" + idAsp.Trim() + "'";
                }
                
                SqlDataAdapter da = new SqlDataAdapter(cmSql);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dtDatos = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            DataRow drDatos = dtDatos.Rows[0];
                           
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            p.nombre = drDatos["nombre"].ToString();
                            p.idAsp = drDatos["idasp"].ToString();                                                   
                    }
                }
            }catch(Exception e){
                
            }
            _conn.Close();
            return p;
        }

        public List<Pantalla> getPantallas()
        {
            List<Pantalla> listado = new List<Pantalla>();
            _conn.Open();
            try
            {
                SqlCommand cmSql = _conn.CreateCommand();
                cmSql.CommandText = "select * from pantallas p where p.Estado=0";
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
                            Pantalla p = new Pantalla();
                            p.idPantalla = int.Parse(drDatos["idpantalla"].ToString());
                            p.idModulo = int.Parse(drDatos["idmodulo"].ToString());
                            p.nombre = drDatos["nombre"].ToString();
                            p.idAsp = drDatos["idasp"].ToString();
                            listado.Add(p);
                        }
                    }
                }
            }
            catch { 
            
            }
            _conn.Close();
            return listado;
        }
    }
}
