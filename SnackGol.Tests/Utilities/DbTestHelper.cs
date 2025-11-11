using System;
using LibraryConnection.Context;

namespace SnackGol.Tests.Utilities;

public static class DbTestHelper
{
    public static void UseFreshInMemoryDb()
    {
        System.Environment.SetEnvironmentVariable("USE_INMEMORY_DB", "1");
        using var db = new ApplicationDbContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}
