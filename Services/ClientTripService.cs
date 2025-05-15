using Microsoft.Data.SqlClient;

namespace apbd_cw8;

public class ClientTripService : ServiceBase<ClientTrip>
{
    public ClientTripService(string connectionString) : base(connectionString) { }

    public override IEnumerable<ClientTrip> GetData(IEnumerable<KeyValuePair<string, string[]>> fields)
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idclient, idtrip, registeredat, paymentdate from client_trip" + this.GetSqlWhere(fields, out var cmdParamsFiller), connection))
        {
            cmdParamsFiller(command);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                var paymentDateOridinal = reader.GetOrdinal("paymentdate");

                while (reader.Read())
                {
                    yield return new()
                    {
                        IdClient = reader.GetInt32(reader.GetOrdinal("idclient")),
                        IdTrip = reader.GetInt32(reader.GetOrdinal("idtrip")),
                        RegisteredAt = reader.GetInt32(reader.GetOrdinal("registeredat")),
                        PaymentDate = reader.IsDBNull(paymentDateOridinal) ? null : reader.GetInt32(paymentDateOridinal),
                    };
                }
            }
        }
    }

    public override int InsertData(ClientTrip data)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.InsertData(data, connection);
    }

    public override int InsertData(ClientTrip data, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("insert into client_trip (idclient, idtrip, registeredat, paymentdate) values (@idclient, @idtrip, @registeredat, @paymentdate)", connection))
            {
                command.Parameters.AddWithValue("@idclient", data.IdClient);
                command.Parameters.AddWithValue("@idtrip", data.IdTrip);
                command.Parameters.AddWithValue("@registeredat", data.RegisteredAt);
                command.Parameters.AddWithValue("@paymentdate", data.PaymentDate as object ?? DBNull.Value);

                return command.ExecuteNonQuery();
            }
        }
        catch { return -1; }
    }

    public override bool UpdateData(ClientTrip data)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.UpdateData(data, connection);
    }

    public override bool UpdateData(ClientTrip data, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("update client_trip set idclient = @idclient, idtrip = @idtrip, registeredat = @registeredat, paymentdate = @paymentdate where client_trip.idclient = @idclient and client_trip.idtrip = @idtrip", connection))
            {
                command.Parameters.AddWithValue("@idclient", data.IdClient);
                command.Parameters.AddWithValue("@idtrip", data.IdTrip);
                command.Parameters.AddWithValue("@registeredat", data.RegisteredAt);
                command.Parameters.AddWithValue("@paymentdate", data.PaymentDate as object ?? DBNull.Value);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }

    public override bool DeleteData(ClientTrip data)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.DeleteData(data, connection);
    }

    public override bool DeleteData(ClientTrip data, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("delete client_trip where client_trip.idclient = @idclient and client_trip.idtrip = @idtrip", connection))
            {
                command.Parameters.AddWithValue("@idclient", data.IdClient);
                command.Parameters.AddWithValue("@idtrip", data.IdTrip);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }
}
