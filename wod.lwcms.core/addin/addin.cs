using System;
using System.Collections.Generic;
using System.Text;

namespace wod.lwcms.addin
{
    public interface IAddin
    {
        string name { get; }
        string description { get; }
        Dictionary<Type, Type> getServices();
        string initAlias();
        Dictionary<string, commands.command> initCommands(aliasResource resource);
        commands.command getBeforeCommand(string pageCommandId);
        commands.command getAfterCommand(string pageCommandId);

        void setSetting(List<models.addinSetting> setting);
    }
}
