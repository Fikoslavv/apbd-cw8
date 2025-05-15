using Microsoft.Data.SqlClient;

namespace apbd_cw8;

public class TripService : ServiceBase<Trip>
{
    public TripService(string connectionString) : base(connectionString) { }

    public override IEnumerable<Trip> GetData(IEnumerable<KeyValuePair<string, string[]>> fields)
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idtrip, name, description, datefrom, dateto, maxpeople from trip" + this.GetSqlWhere(fields, out var cmdParamsFiller), connection))
        {
            cmdParamsFiller(command);
            connection.Open();

            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    yield return new()
                    {
                        IdTrip = reader.GetInt32(reader.GetOrdinal("idtrip")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        DateFrom = reader.GetDateTime(reader.GetOrdinal("datefrom")),
                        DateTo = reader.GetDateTime(reader.GetOrdinal("dateto")),
                        MaxPeople = reader.GetInt32(reader.GetOrdinal("maxpeople")),
                    };
                }
        }
    }

    public override int InsertData(Trip trip)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.InsertData(trip, connection);
    }

    public override int InsertData(Trip trip, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("insert into trip (name, description, datefrom, dateto, maxpeople) values (@name, @description, @datefrom, @dateto, @maxpeople)", connection))
            {
                command.Parameters.AddWithValue("@name", trip.Name);
                command.Parameters.AddWithValue("@description", trip.Description);
                command.Parameters.AddWithValue("@dateto", trip.DateTo);
                command.Parameters.AddWithValue("@datefrom", trip.DateFrom);
                command.Parameters.AddWithValue("@maxpeople", trip.MaxPeople);

                trip.IdTrip = Convert.ToInt32(command.ExecuteScalar());
                return trip.IdTrip;
            }
        }
        catch { return -1; }
    }

    public override bool UpdateData(Trip trip)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.UpdateData(trip, connection);
    }

    public override bool UpdateData(Trip trip, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("update trip set name = @name, description = @description, datefrom = @datefrom, dateto = @dateto, maxpeople = @maxpeople where trip.idtrip = @idtrip", connection))
            {
                command.Parameters.AddWithValue("@idtrip", trip.IdTrip);
                command.Parameters.AddWithValue("@name", trip.Name);
                command.Parameters.AddWithValue("@description", trip.Description);
                command.Parameters.AddWithValue("@dateto", trip.DateTo);
                command.Parameters.AddWithValue("@datefrom", trip.DateFrom);
                command.Parameters.AddWithValue("@maxpeople", trip.MaxPeople);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }

    public override bool DeleteData(Trip trip)
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString);
        connection.Open();
        return this.DeleteData(trip, connection);
    }

    public override bool DeleteData(Trip trip, SqlConnection connection, SqlTransaction? transaction = null)
    {
        try
        {
            using (var command = new Microsoft.Data.SqlClient.SqlCommand("delete from trip where trip.idtrip = @idtrip", connection))
            {
                command.Parameters.AddWithValue("@idtrip", trip.IdTrip);

                return command.ExecuteNonQuery() > 0;
            }
        }
        catch { return false; }
    }
}
