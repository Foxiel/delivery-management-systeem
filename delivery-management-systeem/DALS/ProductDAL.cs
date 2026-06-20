using delivery_management_systeem.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace delivery_management_systeem.DALS
{
    public class ProductDAL : BaseDAL
    {
        public List<Product> GetProductInfoBestelling(int bestellingId)
        {
            List<Product> listProducten = new();

            using var connection = GetConnection();

            string sql = @"
        SELECT
            p.product_id,
            p.ean,
            p.naam,
            p.beschrijving,
            p.prijs,
            p.gewicht,
            p.garantie,
            br.aantal
        FROM Bestelregel br
        JOIN Product p
            ON br.product_id = p.product_id
        WHERE br.bestelling_id = @bestellingId";

            using var command = new SqlCommand(sql, (SqlConnection)connection);
            command.Parameters.AddWithValue("@bestellingId", bestellingId);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int aantal = Convert.ToInt32(reader["aantal"]);

                for (int i = 0; i < aantal; i++)
                {
                    listProducten.Add(new Product
                    {
                        EAN = reader["ean"].ToString()!,
                        Naam = reader["naam"].ToString()!,
                        Beschrijving = reader["beschrijving"].ToString()!,
                        Prijs = Convert.ToDecimal(reader["prijs"]),
                        Gewicht = Convert.ToDouble(reader["gewicht"]),
                        Garantie = reader["garantie"].ToString()!
                    });
                }
            }

            return listProducten;
        } 
        public Product productRetouren(string ean)
        {
            Product product = new();


            using var connection = GetConnection();

            string sql = @"
        SELECT
            p.product_id,
            p.ean,
            p.naam,
            p.beschrijving,
            p.prijs,
            p.gewicht,
            p.garantie
        FROM Product p
        WHERE p.ean = @ean";

            using var command = new SqlCommand(sql, (SqlConnection)connection);
            command.Parameters.AddWithValue("@ean", ean);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                product.EAN = reader["ean"].ToString()!;
                product.Naam = reader["naam"].ToString()!;
                product.Beschrijving = reader["beschrijving"].ToString()!;
                product.Prijs = Convert.ToDecimal(reader["prijs"]);
                product.Gewicht = Convert.ToDouble(reader["gewicht"]);
                product.Garantie = reader["garantie"].ToString()!;  
            }

            return product;
        }
    }
   
}
