using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace eOdvjetnik;

public static class Constants
{
    public const string DatabaseFilename = "DocsDatabase.db3";
    public static string dbLocation = "C:\\Users\\robi\\Source\\Repos\\robivin\\eOdvjetnik";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(dbLocation, DatabaseFilename);

}
   

