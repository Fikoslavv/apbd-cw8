namespace apbd_cw8;

public class CountryService : ServiceBase<Country>
{
    public CountryService(string connectionString) : base(connectionString) { }

    public override IEnumerable<Country> GetData()
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idcountry, name from country", connection))
        {
            connection.Open();

            using (var reader = command.ExecuteReader())
            while (reader.Read())
            {
                yield return new()
                {
                    IdCountry = reader.GetInt32(reader.GetOrdinal("idcountry")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                };
            }
        }
    }

    public override bool InsertData(Country value) => throw new NotImplementedException();

    public override bool UpdateData(Country value) => throw new NotImplementedException();

    public override bool DeleteData(Country value) => throw new NotImplementedException();
}
