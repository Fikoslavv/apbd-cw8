namespace apbd_cw8;

public class CountryTripService : ServiceBase<CountryTrip>
{
    public CountryTripService(string connectionString) : base(connectionString) { }

    public override IEnumerable<CountryTrip> GetData()
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idcountry, idtrip from country_trip", connection))
        {
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

    public override bool InsertData(CountryTrip value) => throw new NotImplementedException();

    public override bool UpdateData(CountryTrip value) => throw new NotImplementedException();

    public override bool DeleteData(CountryTrip value) => throw new NotImplementedException();
}
