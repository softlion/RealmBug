using Realms;

namespace RealmBug20240322;

public partial class App : Application
{
    public App()
    {
        try
        {
            _ = TriggerRealmBug();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        InitializeComponent();

        MainPage = new MainPage();
        UserAppTheme = PlatformAppTheme;
    }

    private async Task TriggerRealmBug()
    {
        using var db = Realm.GetInstance(new RealmConfiguration
        {
            SchemaVersion = 20,
        });

        var walletId = "someId" + new Random().Next();
        var wallet = new DbWallet { WalletId = walletId };
        await db.WriteAsync(() =>
        {
            try
            {
                db.Add(wallet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        
        wallet = db.Find<DbWallet>(walletId);
        await db.WriteAsync(() =>
        {
            wallet.Name = "Name";
            wallet.Alias = "Alias";
        });
        
        //Crash here
        var dbWallets = db.All<DbWallet>().ToList();

        var i = 0;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "RealmBug20240322";
        return window;
    }
}