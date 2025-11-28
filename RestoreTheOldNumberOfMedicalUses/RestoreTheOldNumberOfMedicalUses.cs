using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;

namespace RestoreTheOldNumberOfMedicalUses;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class RestoreTheOldNumberOfMedicalUsesExtension(
    ModHelper modHelper, 
    DatabaseServer databaseServer,
    ISptLogger<RestoreTheOldNumberOfMedicalUsesExtension> logger) : IOnLoad
{
    public Task OnLoad()
    {
        var templates = databaseServer.GetTables().Templates;
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var items = modHelper.GetJsonDataFromFile<ItemConfig[]>(pathToMod, "config.json");
        
        logger.LogWithColor("[RestoreTheOldNumberOfMedicalUses] The mod is loaded.", LogTextColor.Green);

        foreach (var item in items)
        {
            if (templates.Items[item.MongoId].Properties is null)
            {
                logger.Error($"[RestoreTheOldNumberOfMedicalUses] Item {item.MongoId} has no properties");
                continue;
            }

            templates.Items[item.MongoId].Properties!.MaxHpResource = item.Value;
        }
        
        return Task.CompletedTask;
    }
}