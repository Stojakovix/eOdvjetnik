using eOdvjetnik.Data;

namespace eOdvjetnik;

public partial class App : Application
	{
    static SchedulerDatabase database;
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkNrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQlljTX5ad0FjXH1XdnI=;Mgo+DSMBPh8sVXJ1S0d+X1RPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXpTckVqWHtedXdSQ2c=;ORg4AjUWIQA/Gnt2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdkFjUH5acHFWT2Ra;MTYxNDQ2N0AzMjMxMmUzMTJlMzMzNVRIVFF4ZHRKMkdLby91cUhqekN5MmUwdG1lNkwwdUVLekp2Rkx4OWpXa0U9;MTYxNDQ2OEAzMjMxMmUzMTJlMzMzNVd5cmpjVlEvWEZCckhncU1INXhERjh4K3BqekkrN2lqangreFpBRGs3S0U9;NRAiBiAaIQQuGjN/V0d+XU9Hc1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TckVnWXZfdHRQQmZVVw==;MTYxNDQ3MEAzMjMxMmUzMTJlMzMzNVU4ZDBBWDJRSVlUaFA5MmNtdkFBeERDczRDY0xZTVgxejhDU0hESVhuL289;MTYxNDQ3MUAzMjMxMmUzMTJlMzMzNUEwOFRZVDRoUzNoVXVNMS82V0t6bk1YamZzclR2dHJodnlKR0JQdkMvNk09;Mgo+DSMBMAY9C3t2VFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5XdkFjUH5acHFQQmdb;MTYxNDQ3M0AzMjMxMmUzMTJlMzMzNU1obVprOExnT3lHSnk2OFNQM0ZHOFA3ZXBLUnFqbjVzSHQ4cGtHUGlpL3c9;MTYxNDQ3NEAzMjMxMmUzMTJlMzMzNWhPVjVxd0lseTA5dzM1UERnVy9lVnh3MGVpdFBEbGJ6WFZLRjU4UWJreTg9;MTYxNDQ3NUAzMjMxMmUzMTJlMzMzNVU4ZDBBWDJRSVlUaFA5MmNtdkFBeERDczRDY0xZTVgxejhDU0hESVhuL289");
		

        InitializeComponent();

		MainPage = new AppShell();
	}

    public static SchedulerDatabase Database
    {
        get
        {
            if (database == null)
            {
                database = new SchedulerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SchedulerDatabase.db3"));
            }
            return database;
        }
    }
}
