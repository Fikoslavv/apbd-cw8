namespace apbd_cw8;

public class ClientService : ServiceBase<Client>
{
    public ClientService(string connectionString) : base(connectionString) { }

    public override IEnumerable<Client> GetData()
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idclient, firstname, lastname, email, telephone, pesel from client", connection))
        {
            connection.Open();

            using (var reader = command.ExecuteReader())
            while (reader.Read())
            {
                yield return new()
                {
                    IdClient = reader.GetInt32(reader.GetOrdinal("idclient")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Telephone = reader.GetString(reader.GetOrdinal("telephone")),
                    Pesel = reader.GetString(reader.GetOrdinal("pesel")),
                };
            }
        }
    }

    public override bool InsertData(Client client)
    {
        try
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("insert into client (firstname, lastname, email, telephone, pesel) values (@firstname, @lastname, @email, @telephone, @pesel)", connection))
            {
                command.Parameters.AddWithValue("@firstname", client.FirstName);
                command.Parameters.AddWithValue("@lastname", client.LastName);
                command.Parameters.AddWithValue("@email", client.Email);
                command.Parameters.AddWithValue("@telephone", client.Telephone);
                command.Parameters.AddWithValue("@pesel", client.Pesel);
    
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }

    public override bool UpdateData(Client client)
    {
        try
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("update client set firstname = @firstname, lastname = @lastname, email = @email, telephone = @telephone, pesel = @pesel where client.idclient = @idclient", connection))
            {
                command.Parameters.AddWithValue("@idclient", client.IdClient);
                command.Parameters.AddWithValue("@firstname", client.FirstName);
                command.Parameters.AddWithValue("@lastname", client.LastName);
                command.Parameters.AddWithValue("@email", client.Email);
                command.Parameters.AddWithValue("@telephone", client.Telephone);
                command.Parameters.AddWithValue("@pesel", client.Pesel);
    
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }

    public override bool DeleteData(Client client)
    {
        try
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("delete from client where client.idclient = @idclient", connection))
            {
                command.Parameters.AddWithValue("@idclient", client.IdClient);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }
}
