using Microsoft.Data.SqlClient;

namespace APIEndereco.DataBase
{
    public class DatabaseEndereco
    {
        private readonly ILogger<DatabaseEndereco> _logger;
        public DatabaseEndereco(ILogger<DatabaseEndereco> logger)
        {
            _logger = logger;
        }
        public DatabaseEndereco()
        {
        }

        public string DatabaseConnection()
        {
            var json = File.ReadAllText("appsettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;
            string connectionString = jsonObj!["ConnectionStrings"]["DatabaseConnection"];

            return connectionString;
        }

        public List<Endereco> GetData()
        {
            List<Endereco> enderecos = new List<Endereco>();

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Endereco", con))
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var endereco = new Endereco
                                    {
                                        Id = (int)reader["Id"],
                                        Logradouro = reader["Logradouro"].ToString(),
                                        Bairro = reader["Bairro"].ToString(),
                                        Cidade = reader["Cidade"].ToString(),
                                        Estado = reader["Estado"].ToString()
                                    };
                                    enderecos.Add(endereco);
                                }
                            }
                            else
                            {
                                return new List<Endereco>();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados no banco de dados.");
            }
            return enderecos;
        }

        public void PostData(Endereco endereco)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Endereco (Logradouro, Bairro, Cidade, Estado) VALUES (@Logradouro, @Bairro, @Cidade, @Estado)", con))
                    {
                        cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                        cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                        cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                        cmd.Parameters.AddWithValue("@Estado", endereco.Estado);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir dados no banco de dados.");
            }

        }
    }
}