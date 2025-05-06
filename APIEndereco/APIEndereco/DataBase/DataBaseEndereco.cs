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
                                        Id = Guid.Parse(reader["Id"].ToString()!),
                                        Cep = reader["Cep"].ToString(),
                                        Logradouro = reader["Logradouro"].ToString(),
                                        Complemento = reader["Complemento"].ToString(),
                                        Numero = reader["Numero"].ToString(),
                                        Bairro = reader["Bairro"].ToString(),
                                        Cidade = reader["Cidade"].ToString(),
                                        UF = reader["UF"].ToString()
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
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Endereco (Id, Cep, Logradouro, Complemento, Numero, Bairro, Cidade, UF) VALUES (@Id, @Cep, @Logradouro, @Complemento, @Numero, @Bairro, @Cidade, @UF)", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", new Guid());
                        cmd.Parameters.AddWithValue("@Cep", endereco.Cep);
                        cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                        cmd.Parameters.AddWithValue("@Complemento", endereco.Complemento);
                        cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                        cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                        cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                        cmd.Parameters.AddWithValue("@UF", endereco.UF);
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