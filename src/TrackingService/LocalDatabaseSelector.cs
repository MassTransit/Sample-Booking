namespace TrackingService
{
    using System;
    using System.Data.SqlClient;


    public static class LocalDatabaseSelector
    {
        /// <summary>
        /// This is a list of the connection strings that we will attempt to find what LocalDb versions
        /// are on the local pc which we can run the unit tests against
        /// </summary>
        static readonly string[] _candidateConnectionStrings =
        {
            @"Data Source=(LocalDb)\MSSQLLocalDB;Integrated Security=True;Initial Catalog=Booking;", // the localdb installed with VS 2015
            @"Data Source=(LocalDb)\ProjectsV12;Integrated Security=True;Initial Catalog=Booking;", // the localdb with VS 2013
            @"Data Source=(LocalDb)\v11.0;Integrated Security=True;Initial Catalog=Booking;" // the older version of localdb
        };

        static readonly Lazy<string> _connectionString = new Lazy<string>(GetConnectionString);

        public static string ConnectionString => _connectionString.Value;

        /// <summary>
        /// Loops through the array of potential localdb connection strings to find one that we can use for the unit tests
        /// </summary>
        static string GetConnectionString()
        {
            foreach (var connectionString in _candidateConnectionStrings)
            {
                try
                {
                    using (new SqlConnection(connectionString))
                    {
                        return connectionString;
                    }
                }
                catch (Exception)
                {
                    // Did not find a connection, so try the next one
                }
            }

            throw new InvalidOperationException(
                "Couldn't connect to any of the LocalDB Databases. You might have a version installed that is not in the list. Please check the list and modify as necessary");
        }
    }
}