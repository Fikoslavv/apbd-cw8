using Microsoft.Data.SqlClient;

namespace apbd_cw8;

public class CountryTripService : ServiceBase<CountryTrip>
{
    public CountryTripService(string connectionString) : base(connectionString) { }

    public override IEnumerable<CountryTrip> GetData(IEnumerable<KeyValuePair<string, string[]>> fields)
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idcountry, idtrip from country_trip" + this.GetSqlWhere(fields, out var cmdParamsFiller), connection))
        {
            cmdParamsFiller(command);
            connection.Open();

            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    yield return new()
                    {
                        IdCountry = reader.GetInt32(reader.GetOrdinal("idcountry")),
                        IdTrip = reader.GetInt32(reader.GetOrdinal("idtrip")),
                    };
                }
        }
    }

    public override int InsertData(CountryTrip value) => throw new NotImplementedException();
    public override int InsertData(CountryTrip value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();

    public override bool UpdateData(CountryTrip value) => throw new NotImplementedException();
    public override bool UpdateData(CountryTrip value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();

    public override bool DeleteData(CountryTrip value) => throw new NotImplementedException();
    public override bool DeleteData(CountryTrip value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();
}
