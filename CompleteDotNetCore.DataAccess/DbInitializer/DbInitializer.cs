using System;
namespace CompleteDotNetCore.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public DbInitializer()
        {
        }

        public void Initialize()
        {
            // Apply migrations, if not applied...

            // Create Roles, if not created...

            // If Roles not created, create Admin user as well.

            throw new NotImplementedException();
        }
    }
}

