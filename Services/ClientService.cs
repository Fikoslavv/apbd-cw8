using Microsoft.Data.SqlClient;

namespace apbd_cw8;

public class ClientService : ServiceBase<Client>
{
    public ClientService(string connectionString) : base(connectionString) { }

    public override IEnumerable<Client> GetData(IEnumerable<KeyValuePair<string, string[]>> fields)
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idclient, firstname, lastname, email, telephone, pesel from client" + this.GetSqlWhere(fields, out var cmdParamsFiller), connection))
        {
            cmdParamsFiller(command);
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

    public override int InsertData(Client client)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.InsertData(client, connection);
    }

    public override int InsertData(Client client, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("insert into client (firstname, lastname, email, telephone, pesel) values (@firstname, @lastname, @email, @telephone, @pesel); select scope_identity()", connection))
            {
                command.Parameters.AddWithValue("@firstname", client.FirstName);
                command.Parameters.AddWithValue("@lastname", client.LastName);
                command.Parameters.AddWithValue("@email", client.Email);
                command.Parameters.AddWithValue("@telephone", client.Telephone);
                command.Parameters.AddWithValue("@pesel", client.Pesel);

                client.IdClient = Convert.ToInt32(command.ExecuteScalar());
                return client.IdClient;
            }
        }
        catch { return -1; }
    }

    public override bool UpdateData(Client client)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.UpdateData(client, connection);
    }

    public override bool UpdateData(Client client, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("update client set firstname = @firstname, lastname = @lastname, email = @email, telephone = @telephone, pesel = @pesel where client.idclient = @idclient", connection))
            {
                command.Parameters.AddWithValue("@idclient", client.IdClient);
                command.Parameters.AddWithValue("@firstname", client.FirstName);
                command.Parameters.AddWithValue("@lastname", client.LastName);
                command.Parameters.AddWithValue("@email", client.Email);
                command.Parameters.AddWithValue("@telephone", client.Telephone);
                command.Parameters.AddWithValue("@pesel", client.Pesel);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }

    public override bool DeleteData(Client client)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.DeleteData(client, connection);
    }

    public override bool DeleteData(Client client, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("delete from client where client.idclient = @idclient", connection))
            {
                command.Parameters.AddWithValue("@idclient", client.IdClient);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }
}
