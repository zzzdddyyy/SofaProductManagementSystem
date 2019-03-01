using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Models;

namespace DAL
{
    public  class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string user;
        private string password;
        public DBConnect()
        {
            Initialize();
        }
        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "product";
            user = "root";
            password = "";
            string connectString;
            connectString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "USER=" + user + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectString);
        }
        //Opening connection database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to Server!Please contact Administrator.", "System Info", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username or password,please try again!", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        break;
                }
                return false;
            }
        }
        //Closing connection database
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //Insert Statement
        public void Insert(string query)
        {
            //opening database
            if(this.OpenConnection() == true)
            {
                try
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand myCmd = new MySqlCommand(query, connection);
                    //execute command
                    myCmd.ExecuteNonQuery();//不返回数据
                }
                catch (Exception ex)
                {

                    throw(ex);
                }
                finally
                {
                    //断开连接
                    this.CloseConnection();
                }
            }
        }
        //Update statement
        public void Update(string query)
        {
            //opening database
            if (this.OpenConnection() == true)
            {
                MySqlCommand myCmd = new MySqlCommand();
                //Execute command
                //Assidn a query
                myCmd.CommandText = query;
                //Assign a connection
                myCmd.Connection = connection;
                //Closing connection
                this.CloseConnection();
            }
        }
        //Delete statement
        public void Delete(string query)
        {
            //opening connection
            if (OpenConnection() == true)
            {
                try
                {
                    //create MySQL command
                    MySqlCommand myCmd = new MySqlCommand();
                    //Assign a query using CommandText
                    myCmd.CommandText = query;
                    //Assign a connection using Connection
                    myCmd.Connection = connection;
                    //ExeCute Query
                    myCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    throw(ex);
                }
                finally
                {
                    //closing connection
                    this.CloseConnection();
                }
                

            }
        }
        //Select statement
        public List<Sofa> Select(string query)
        {
            //Create a list to store the result
            List<Sofa> myList = new List<Sofa>();
            //opening connection
            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand myCmd = new MySqlCommand(query, connection);
                    MySqlDataReader myDR = myCmd.ExecuteReader();
                    while (myDR.Read())
                    {
                        myList.Add(new Sofa
                        {
                            Id = Convert.ToInt32(myDR["id"]),
                            Pcode = Convert.ToString(myDR["pcode"]),
                            Pname = Convert.ToString(myDR["pname"]),
                            Pstyle = Convert.ToString(myDR["pstyle"]),
                            Pprize = Convert.ToDouble(myDR["pprize"]),
                            Fillter = Convert.ToString(myDR["fillter"]),
                            Frame = Convert.ToString(myDR["frame"]),
                            Wrapper = Convert.ToString(myDR["wrapper"]),
                            Backrest = Convert.ToString(myDR["backrest"]),
                            Footrest = Convert.ToString(myDR["footrest"]),
                            Handrail = Convert.ToString(myDR["handrail"]),
                            Seatbox = Convert.ToString(myDR["seatbox"]),
                            Mid = Convert.ToString(myDR["mid"]),
                            Teatable = Convert.ToString(myDR["teatable"]),
                            Lay = Convert.ToString(myDR["lay"]),
                            Corner = Convert.ToString(myDR["corner"]),
                            Engi_draw = Convert.ToString(myDR["engi_draw"]),
                            Rendergraph = Convert.ToString(myDR["rendergraph"]),
                        });
                    }
                    //Close Data Reader
                    myDR.Close();
                    //return list to be displayed
                    return myList;
                }
                catch (Exception ex)
                {
                    throw(ex);
                }
                finally
                {
                    //Close Connnection
                    this.CloseConnection();
                }
            }
            else
            {
                return myList;
            }
        }
    }
}
