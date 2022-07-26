using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EXERCICEWEB.Models
{
    public class PersonneDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ExerciceconnectionString"].ToString());
        public int InsertPersonne(Personne pers)
        {
            pers.Age = DateTime.Now.Year -Convert.ToDateTime(pers.Date_Naissance).Year;

           SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Personne] ([Nom],[Prenom],[Date_De_Naissance],[Age]) VALUES(@Nom,@Prenom,@Date_De_Naissance,@Age)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Nom", pers.nom);
            cmd.Parameters.AddWithValue("@Prenom", pers.prénom);
            cmd.Parameters.AddWithValue("@Age", pers.Age);
            cmd.Parameters.AddWithValue("@Date_De_Naissance", pers.Date_Naissance);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdatePersonne(Personne pers)
        {
            pers.Age = DateTime.Now.Year - Convert.ToDateTime(pers.Date_Naissance).Year;
            SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Personne] SET Nom = @Nom,Prenom=@Prenom, Date_De_Naissance = @Date_De_Naissance,Age= @Age WHERE ID = @PersonneId", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PersonneId", pers.ID);
            cmd.Parameters.AddWithValue("@Nom", pers.nom);
            cmd.Parameters.AddWithValue("@Prenom", pers.prénom);
            cmd.Parameters.AddWithValue("@Age", pers.Age);
            cmd.Parameters.AddWithValue("@Date_De_Naissance", pers.Date_Naissance);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int DeletePersonne(int PersonneId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Personne WHERE ID = @PersonneId", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PersonneId", PersonneId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public List<Personne> GetListPersonne()
        {
            List<Personne> Personnes = new List<Personne>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personne ", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Personnes.Add(
                    new Personne
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        nom = Convert.ToString(dr["Nom"]),
                        prénom = Convert.ToString(dr["Prenom"]),
                        Date_Naissance = Convert.ToDateTime(dr["Date_De_Naissance"]), Age= Convert.ToInt32(dr["Age"])
                    });
            }
            return Personnes;
        }
        public List<Personne> GetListPersonne2(string search)
        {
            List<Personne> Personnes = new List<Personne>();
           
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personne where UPPER(Nom)  Like UPPER('" + search + "%') OR UPPER(Prenom)  Like UPPER('" + search + "%') ORDER BY Nom DESC", con);

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Personnes.Add(
                    new Personne
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        nom = Convert.ToString(dr["Nom"]),
                        prénom = Convert.ToString(dr["Prenom"]),
                        Date_Naissance = Convert.ToDateTime(dr["Date_De_Naissance"]),
                        Age = Convert.ToInt32(dr["Age"])
                    });
            }
            return Personnes;
        }
        public Personne GetPersonne(int ID)
        {
            Personne pers = new Personne();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personne whereID="+ID+"", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                pers.nom = Convert.ToString(dr["Nom"]);
                pers.prénom = Convert.ToString(dr["Prenom"]);
                pers.Date_Naissance = Convert.ToDateTime(dr["Date_De_Naissance"].ToString());
             }
            return pers;
        }
     

    }
}