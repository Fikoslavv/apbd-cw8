using Microsoft.Data.SqlClient;

namespace apbd_cw8;

public class CountryService : ServiceBase<Country>
{
    public CountryService(string connectionString) : base(connectionString) { }

    public override IEnumerable<Country> GetData(IEnumerable<KeyValuePair<string, string[]>> fields)
    {
        using (var connection = new Microsoft.Data.SqlClient.SqlConnection(this.connectionString))
        using (var command = new Microsoft.Data.SqlClient.SqlCommand("select idcountry, name from country" + this.GetSqlWhere(fields, out var cmdParamsFiller), connection))
        {
            cmdParamsFiller(command);
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

    public override int InsertData(Country value) => throw new NotImplementedException();
    public override int InsertData(Country value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();

    public override bool UpdateData(Country value) => throw new NotImplementedException();
    public override bool UpdateData(Country value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();

    public override bool DeleteData(Country value) => throw new NotImplementedException();
    public override bool DeleteData(Country value, SqlConnection connection, SqlTransaction? transaction = null) => throw new NotImplementedException();
}
