﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetManagerV2.Models
{
    public class db
    {
        Send sender = new Send();
        private SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BudgetManagerEntities"].ConnectionString);

        public void OpenCon()
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CloseCon()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CreateTransaction(string data)
        {
            JObject json = JObject.Parse(data);

            SqlCommand command = new SqlCommand("INSERT INTO Transaction(Value, Text, Date, FK_Category) VALUES (@Value, @Text, @Date, @FK_Category)", connection);
            command.Parameters.Add(CreateParam("@Value", json["Value"], SqlDbType.Float));
            command.Parameters.Add(CreateParam("@Text", json["Text"], SqlDbType.NVarChar));
            command.Parameters.Add(CreateParam("@Date", json["Date"], SqlDbType.Date));
            command.Parameters.Add(CreateParam("@FK_Category", json["FK_Category"], SqlDbType.Int));

            try
            {
                OpenCon();
                command.ExecuteNonQuery();
                JObject confirmation = new JObject();
                confirmation.Add("Status", 200);
                confirmation.Add("Result", "Transaction was created");
                sender.Main(confirmation);
                CloseCon();
            }
            catch (Exception)
            {
                JObject confirmation = new JObject();
                confirmation.Add("Status", 400);
                confirmation.Add("Result", "Create failed");
                sender.Main(confirmation);
                connection.Dispose();
                throw;
            }

        }

        public void UpdateTransaction(string data)
        {
            JObject json = JObject.Parse(data);

            SqlCommand command = new SqlCommand("UPDATE Transaction SET Value = @Value, Text = @Text, Date = @Date, FK_Category = @FK_Category WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Value", json["Value"]);
            command.Parameters.AddWithValue("@Text", json["Text"]);
            command.Parameters.AddWithValue("@Date", json["Date"]);
            command.Parameters.AddWithValue("@FK_Category", json["FK_Category"]);
            command.Parameters.AddWithValue("@Id", json["Id"]);

            try
            {
                OpenCon();
                command.ExecuteNonQuery();
                JObject confirmation = new JObject();
                confirmation.Add("Status", 200);
                confirmation.Add("Result", "Transaction was updated");
                sender.Main(confirmation);
                CloseCon();
            }
            catch (Exception)
            {
                JObject confirmation = new JObject();
                confirmation.Add("Status", 400);
                confirmation.Add("Result", "Update failed");
                sender.Main(confirmation);
                connection.Dispose();
                throw;
            }
        }
        public void GetSingleTransactionById(string data)
        {
            JObject json = JObject.Parse(data);
            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Transaction WHERE Id = @id", connection);
            adapter.SelectCommand.Parameters.Add(CreateParam("@Id", json["Id"], SqlDbType.Int));

            try
            {
                JObject confirmation = new JObject();
                adapter.Fill(table);

                confirmation.Add("Status", 200);

                string JSONSTring = string.Empty;
                JSONSTring = JsonConvert.SerializeObject(table);

                confirmation.Add("Result", JSONSTring);
                sender.Main(confirmation);
                CloseCon();
            }
            catch (Exception)
            {
                JObject confirmation = new JObject();
                confirmation.Add("Status", 400);
                confirmation.Add("Result", "Get failed");
                sender.Main(confirmation);
                connection.Dispose();
                throw;
            }
        }
        public void GetAllTransactions(string data)
        {
            JObject json = JObject.Parse(data);

            SqlCommand command = new SqlCommand("SELECT * FROM Transaction", connection);
            command.Parameters.AddWithValue("@Id", json["Id"]);

            try
            {
                OpenCon();
                command.ExecuteNonQuery();
                JObject confirmation = new JObject();
                confirmation.Add("Status", 200);
                confirmation.Add("Result", "List of transaction was successfully returned");
                sender.Main(confirmation);
                CloseCon();
            }
            catch (Exception)
            {
                JObject confirmation = new JObject();
                confirmation.Add("Status", 400);
                confirmation.Add("Result", "Get failed");
                sender.Main(confirmation);
                connection.Dispose();
                throw;
            }
        }



        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type)
            {
                Value = value
            };
            return param;
        }
    }
}