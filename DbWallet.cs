using Realms;

namespace RealmBug20240322;

public partial class DbWallet : IRealmObject
{
    [PrimaryKey]
    public string WalletId { get; set; }
    
    public string Name { get; set; }
    
    public string? Alias { get; set; }
}