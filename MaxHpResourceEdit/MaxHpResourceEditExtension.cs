using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;

namespace MaxHpResourceEdit;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class MaxHpResourceEditExtension(
    ModHelper modHelper, 
    DatabaseServer databaseServer,
    ISptLogger<MaxHpResourceEditExtension> logger) : IOnLoad
{
    public Task OnLoad()
    {
        var tables = databaseServer.GetTables();
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var items = modHelper.GetJsonDataFromFile<ItemConfig[]>(pathToMod, "config.json");
        
        logger.LogWithColor("[MaxHpResourceEdit] The mod is loaded.", LogTextColor.Green);

        foreach (var item in items)
        {
            if (tables.Templates.Items[item.MongoId].Properties is null)
            {
                logger.Error($"[MaxHpResourceEdit] Item {item.MongoId} has no properties");
                continue;
            }
            
            tables.Templates.Items[item.MongoId].Properties!.MaxHpResource = item.Value;
        }
        
        return Task.CompletedTask;
    }
}