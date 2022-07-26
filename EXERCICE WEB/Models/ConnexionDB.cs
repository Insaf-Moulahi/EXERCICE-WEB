using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;


    class ConnexionDB
    {
        public static SqlConnection MaConn;
    public static SqlConnection MaConnBP;
    public static SqlConnection MaConnPV;
    public SqlDataReader dr;
    public static SqlTransaction transaction;

    public static bool PingTest()
    {
        try
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send("192.168.1.44", timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
                return true;
            return false;
        }
        catch (Exception ex) { deconnect(ConnexionDB.MaConn); throw ex; }
    }
    public static DataAdapter GetResultSql(String sql)
    {
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd = MaConn.CreateCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;
            if (connect())
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                deconnect();
                return adapter;
            }
            deconnect();
            return null;
        }
        catch (Exception ex)
        {
            deconnect();
            throw ex;
        }
    }
    public static  bool TestStringConnection(string connectionString)
    {

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    public static bool connect(SqlConnection con)
    {
        try
        {
            con.Open();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool deconnect(SqlConnection con)
    {
        try
        {
            con.Close();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool ExecuteSqlWithTransaction2(ref string erreur, List<SqlCommand> listSql, List<SqlCommand> listSql2, string connect1, string connect2)
    {




        //config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        Configuration config = null;
        if (System.Web.HttpContext.Current != null)
        {
            config =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        }
        else
        {
            config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        ConnexionDB.MaConn = new SqlConnection(connect1);
        ConnexionDB.connect();
        SqlTransaction transaction1;


        transaction1 = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction1;



        ConnexionDB.MaConn = new SqlConnection(connect2);
        ConnexionDB.connect();
        SqlTransaction transaction2;


        transaction2 = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd2 = new SqlCommand();
        sqlCmd2 = MaConn.CreateCommand();
        sqlCmd2.Transaction = transaction2;


        try
        {
            foreach (SqlCommand sql in listSql)
            {

                sqlCmd.CommandText = sql.CommandText;
                sqlCmd.CommandType = CommandType.Text;
                foreach (SqlParameter p in sql.Parameters)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }
            
                sqlCmd.ExecuteNonQuery();
            }

            foreach (SqlCommand sql in listSql2)
            {

                sqlCmd2.CommandText = sql.CommandText;
                sqlCmd2.CommandType = CommandType.Text;
                foreach (SqlParameter p in sql.Parameters)
                {
                    sqlCmd2.Parameters.AddWithValue(p.ParameterName, p.Value);
                }
                sqlCmd2.ExecuteNonQuery();
            }


            transaction1.Commit();
            transaction2.Commit();
            transaction1.Dispose();
            transaction2.Dispose();
            ConnexionDB.deconnect();
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
            erreur = ex.Message;
            try
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction2.Rollback();
                transaction2.Dispose();
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }

            ConnexionDB.deconnect();
            return false;
        }


    }

    public static bool ExecuteSqlWithTransaction3(ref string erreur, List<SqlCommand> listSql, string connect1)
    {




        //config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        Configuration config = null;
        if (System.Web.HttpContext.Current != null)
        {
            config =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        }
        else
        {
            config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        ConnexionDB.MaConn = new SqlConnection(connect1);
        ConnexionDB.connect();
        SqlTransaction transaction1;


        transaction1 = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction1;



        try
        {
            foreach (SqlCommand sql in listSql)
            {

                sqlCmd.CommandText = sql.CommandText;
                sqlCmd.CommandType = CommandType.Text;
                foreach (SqlParameter p in sql.Parameters)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                sqlCmd.ExecuteNonQuery();
            }

           
            transaction1.Commit();
            transaction1.Dispose();
            ConnexionDB.deconnect();
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
            erreur = ex.Message;
            try
            {
                transaction.Rollback();
                transaction.Dispose();
               
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }

            ConnexionDB.deconnect();
            return false;
        }


    }

    public static bool connect()
    {
        try
        {
            MaConn.Open();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static bool ExecuteSql(String sql, SqlConnection con)
    {
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd = MaConn.CreateCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;
            if (connect(con))
            {
                SqlDataReader reader = sqlCmd.ExecuteReader();
                deconnect(con);
                return true;
            }
            deconnect(con);

            return false;
        }
        catch (Exception ex)
        {
            deconnect(ConnexionDB.MaConn);
            throw ex;
        }
    }
    public static bool ExecuteSqlWithTransaction(List<string> listSql, string Connect, ListBox ListLogTXT)
    {
        ConnexionDB.MaConn = new SqlConnection(Connect);
        ConnexionDB.connect();

        transaction = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }
            transaction.Commit();

            ConnexionDB.deconnect();
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
             ListLogTXT.Items.Add("Erreur Dintégration !!  "+ ex.Message ); 
                          
            try
            {
                transaction.Rollback();
                transaction.Dispose();
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }

            ConnexionDB.deconnect();
            return false;
        }


    }


    public static bool ExecuteSqlWithTransaction(List<string> listSql, SqlConnection con, ref string ERREUR)
    {
        ////ConnexionDB.MaConn = new SqlConnection(Connect);
        //ConnexionDB.connect();

        transaction = con.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = con.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }
            transaction.Commit();
            transaction.Dispose();
            return true;


        }
        catch (Exception ex)
        {

            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
            Console.WriteLine("  Message: {0}", ex.Message);
            ERREUR = ex.Message;
            try
            {
                transaction.Rollback();
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }
            return false;
        }
    }



    public static bool ExecuteSqlWithTransaction(List<string> listSql, string Connect)
    {
        ConnexionDB.MaConn = new SqlConnection(Connect);
        ConnexionDB.connect();

        transaction = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }
            transaction.Commit();

            ConnexionDB.deconnect();
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
             
            try
            {
                transaction.Rollback();
                transaction.Dispose();
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }

            ConnexionDB.deconnect();
            return false;
        }


    }
    //public static bool ExecuteSqlWithTransactionMultisBASE(ref string erreur, List<ClassHELP.BaseSQLs> Listesbase1)
    //{

    //    string cm = "";
    //    string base_encours = ""; 
    //    try
    //    {
         

    //        foreach (var bd in Listesbase1)
    //        {
               
    //            //XtraMessageBox.Show(base_encours);

    //            bd.Conn_base = new SqlConnection(bd.Connect);

             

    //            //XtraMessageBox.Show(base_encours);

    //            base_encours = bd.Connect;
           
    //            bd.Conn_base.Open();
        
    //            bd.trans_base = bd.Conn_base.BeginTransaction("SampleTransaction");

               
    //            foreach (var cmd in bd.cmds)
    //            {
                

    //                SqlCommand sqlCmd = new SqlCommand();
               

    //                sqlCmd = bd.Conn_base.CreateCommand();
              

    //                sqlCmd.Transaction = bd.trans_base;


    //                sqlCmd.CommandText = "SET LANGUAGE N'French'" + cmd.CommandText;
    //                cm = sqlCmd.CommandText;
               

    //                foreach (SqlParameter p in cmd.Parameters)
    //                {
    //                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
    //                }
         
                   
             

    //                sqlCmd.ExecuteNonQuery();
              

    //            }

    //        }

    //        foreach (var bd in Listesbase1)
    //        {


    //            bd.trans_base.Commit();
    //            bd.Conn_base.Close();

    //        }

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {

    //        erreur = ex.Message + "" + cm + "" + base_encours ;

    //        foreach (var bd in Listesbase1)
    //        {

    //            if (bd.trans_base != null)
    //            {
    //                bd.trans_base.Rollback();
    //            }
    //            bd.Conn_base.Close();

    //        }
    //        Console.WriteLine();

          
    //        return false;
    //    }

    //}
    public static bool ExecuteSqlWithTransaction(List<string> listSql, string Connect , ref SqlTransaction transaction, ListBox ListLogTXT)
    {

        ConnexionDB.MaConn = new SqlConnection(Connect);
        ConnexionDB.connect();

        transaction = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }
            
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
             ListLogTXT.Items.Add("Erreur Dintégration !!  " + ex.Message);

             

          
            return false;
        }

    }


    // dans 
    public static bool ExecuteSqlWithTransactionOuverte(List<string> listSql, string Connect, ref SqlTransaction transaction, ListBox ListLogTXT)
    {

        ConnexionDB.MaConn = new SqlConnection(Connect);
        ConnexionDB.connect();

        transaction = MaConn.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }

            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
             ListLogTXT.Items.Add("Erreur Dintégration !!  " + ex.Message);




            return false;
        }

    }
    
    public static bool deconnect()
    {
        try
        {
            MaConn.Close();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static bool ExecuteSqlWithTransaction(List<string> listSql, SqlConnection con)
    {
        //ConnexionDB.MaConn = new SqlConnection(Connect);
        //ConnexionDB.connect();

        transaction = con.BeginTransaction("SampleTransaction");
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = con.CreateCommand();
        sqlCmd.Transaction = transaction;
        try
        {
            foreach (var sql in listSql)
            {

                sqlCmd.CommandText = sql;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
            }
            transaction.Commit();
            return true;


        }
        catch (Exception ex)
        {

            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
            Console.WriteLine("  Message: {0}", ex.Message);

            try
            {
                transaction.Rollback();
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }
            return false;
        }
    }

    public static DataAdapter GetResultSql(String sql, SqlConnection con)
    {
        try
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd = con.CreateCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandTimeout = 1000;
            if (connect(con))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                deconnect(con);
                return adapter;
            }
            deconnect(con);
            return null;
        }
        catch (Exception ex)
        {
            deconnect(con);
            throw ex;
        }
    }

    public static SqlDataReader getDataReader(String SQL, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand(SQL, con);

        SqlDataReader dr;
        try
        {
            if (connect(con))
            {
                dr = cmd.ExecuteReader();
                cmd.Dispose();
                return dr;
            }
            return null;
        }
        catch (Exception ex)
        {
            deconnect(con);
            throw ex;
        }


    }

    public static DataAdapter procStockOrder(string sQuery, ArrayList paramIN, ArrayList paramOUT)
    {
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd = MaConn.CreateCommand();
        sqlCmd.CommandText = sQuery;
        sqlCmd.CommandType = CommandType.StoredProcedure;

        if (paramIN.Count > 0)
            foreach (string[] tableau in paramIN)
            {
                sqlCmd.Parameters.Add(tableau[0], tableau[1]).Direction = ParameterDirection.Input;
            }
        if (paramOUT.Count > 0)
            foreach (string[] tableau in paramOUT)
            {
                sqlCmd.Parameters.Add(tableau[0], SqlDbType.VarChar).Direction = ParameterDirection.Output;
            }
        try
        {

            if (connect(ConnexionDB.MaConn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                deconnect(ConnexionDB.MaConn);
                return adapter;
            }
            deconnect(ConnexionDB.MaConn);
            return null;
        }
        catch (Exception ex)
        {
            deconnect(ConnexionDB.MaConn);
            throw ex;
        }
    }
    private static void WriteToFile(string text)
    {
        //string path = ConfigurationManager.AppSettings["Path"];
        string path = @"C:\Users\Administrator\Desktop\Specific\icons\ServiceLog.txt";
        // string path = Application.StartupPath + "/logerr.txt";
        //StreamWriter file = new StreamWriter(path);
        //file.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
        //file.Close();
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            writer.Close();
        }
    }


    public static bool ExecuteSqlWithTransaction(ref string erreur, string Connect, List<SqlCommand> cmds)
    {
        ConnexionDB.MaConn = new SqlConnection(Connect);
        ConnexionDB.connect();
        string sql = "";
        transaction = MaConn.BeginTransaction("SampleTransaction");

        try
        {
            foreach (var cmd in cmds)
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd = MaConn.CreateCommand();
                sqlCmd.Transaction = transaction;
                sqlCmd.CommandText = cmd.CommandText;
                foreach (SqlParameter p in cmd.Parameters)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }
                sql = sqlCmd.CommandText;
                sqlCmd.ExecuteNonQuery();
            

            }
            transaction.Commit();

            ConnexionDB.deconnect();
            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine();
            WriteToFile("Erreur//"+DateTime.Now+" //" + sql.ToString() +"******"+ex.Message);
            try
            {
                transaction.Rollback();
                transaction.Dispose();

                erreur = ex.Message;
            }
            catch (Exception ex2)
            {
                // This catch block will handle any errors that may have occurred 
                // on the server that would cause the rollback to fail, such as 
                // a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                Console.WriteLine("  Message: {0}", ex2.Message);
            }

            ConnexionDB.deconnect();
            return false;
        }

    }
    //public static bool ExecuteSqlWithTransactionMultisBASE(ref string erreur, List<ClassCaisse.BaseSQLs> Listesbase1)
    //{

    //    string cm = "";
    //    try
    //    {

    //        foreach (var bd in Listesbase1)
    //        {

    //            bd.Conn_base = new SqlConnection(bd.Connect);
    //            bd.Conn_base.Open();
    //            bd.trans_base = bd.Conn_base.BeginTransaction("SampleTransaction");


    //            foreach (var cmd in bd.cmds)
    //            {
    //                SqlCommand sqlCmd = new SqlCommand();
    //                sqlCmd = bd.Conn_base.CreateCommand();
    //                sqlCmd.Transaction = bd.trans_base;
    //                sqlCmd.CommandText = cmd.CommandText;
    //                cm = sqlCmd.CommandText;
    //                foreach (SqlParameter p in cmd.Parameters)
    //                {
    //                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
    //                }
    //                sqlCmd.ExecuteNonQuery();

    //            }

    //        }

    //        foreach (var bd in Listesbase1)
    //        {


    //            bd.trans_base.Commit();
    //            bd.Conn_base.Close();

    //        }

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        erreur += ex.Message + "" + cm;
    //        foreach (var bd in Listesbase1)
    //        {


    //            bd.trans_base.Rollback();
    //            bd.Conn_base.Close();

    //        }
    //        Console.WriteLine();

       

    //        return false;
    //    }

    //}
    }

