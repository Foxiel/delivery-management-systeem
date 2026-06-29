using delivery_management_systeem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace delivery_management_systeem.Repositories
{
    public class BezorgingRepositorie : dbContext
    {
        public List<Bezorging> GetBezorgingInfoBestelling()
        {
            List<Bezorging> listBezorgingen = new();

            using var connection = GetConnection();

            string sql = @"
                SELECT
                    b.track_trace_code
                FROM Bezorging b";

            using var command = new SqlCommand(sql, (SqlConnection)connection);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listBezorgingen.Add(new Bezorging
                {
                    Code = reader["track_trace_code"].ToString()!
                });
            }

            return listBezorgingen;
        }
        public Bezorging RetourBezorging(string barcode)
        {
            Bezorging bezorging = new();

            using var connection = GetConnection();

            string sql = @"
        SELECT
            b.track_trace_code
        FROM Bezorging b
        WHERE b.track_trace_code = @barcode";

            using var command = new SqlCommand(sql, (SqlConnection)connection);
            command.Parameters.AddWithValue("@barcode", barcode);

            connection.Open();

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                bezorging.Code = reader["track_trace_code"].ToString()!;
            }

            return bezorging;
        }
    }
   
}