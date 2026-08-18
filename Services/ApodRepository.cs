using NasaApodGallery.DTOs;
using NasaApodGallery.Models;
using Microsoft.Data.SqlClient;

namespace NasaApodGallery.Services
{
    public class ApodRepository : IApodRepository
    {
        private readonly string _connectionString;

        public ApodRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Apod>> GetAllAsync()
        {
            var list = new List<Apod>();

            string sql = @"
                SELECT Id, Date, Title, Explanation, Url, MediaType, ServiceVersion, SavedAt
                FROM Apod
                ORDER BY Date DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new Apod
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Title = reader.IsDBNull(2) ? "No Title" : reader.GetString(2),
                                Explanation = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Url = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                MediaType = reader.IsDBNull(5) ? null : reader.GetString(5),
                                ServiceVersion = reader.IsDBNull(6) ? null : reader.GetString(6),
                                SavedAt = reader.GetDateTime(7)
                            };

                            list.Add(item);
                        }
                        
                    }
                }
            }
            return list;
        }



        public async Task InsertIfNotExistsAsync(ApodDto dto)
        {
            string sql = @"
                IF NOT EXISTS (SELECT 1 FROM Apod WHERE Date = @Date)
                BEGIN
                    INSERT INTO Apod (Date, Title, Explanation, Url, MediaType, ServiceVersion, SavedAt)
                    VALUES (@Date, @Title, @Explanation, @Url, @MediaType, @ServiceVersion, GETUTCDATE())
                END";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Date", DateTime.Parse(dto.Date));
                    command.Parameters.AddWithValue("@Title", (object)dto.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Explanation", (object)dto.Explanation ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Url", (object)dto.Url ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MediaType", (object)dto.MediaType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ServiceVersion", (object)dto.ServiceVersion ?? DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
            }
        }
    }
    }
}