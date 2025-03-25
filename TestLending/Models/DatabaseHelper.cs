using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // Метод для выполнения SELECT-запросов, возвращает DataTable
    public DataTable ExecuteQuery(string query)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    // Универсальный метод для маппинга результата запроса в список объектов
    public List<T> ExecuteQuery<T>(string query, params object[] parameters) where T : new()
    {
        List<T> resultList = new List<T>();

        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                // Добавляем параметры
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@p{i}", parameters[i]);
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T obj = new T();
                        foreach (PropertyInfo prop in typeof(T).GetProperties())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                            {
                                prop.SetValue(obj, reader.GetValue(reader.GetOrdinal(prop.Name)));
                            }
                        }
                        resultList.Add(obj);
                    }
                }
            }
        }

        return resultList;
    }

    // Метод для выполнения команд INSERT, UPDATE, DELETE
    public int ExecuteNonQuery(string query)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
