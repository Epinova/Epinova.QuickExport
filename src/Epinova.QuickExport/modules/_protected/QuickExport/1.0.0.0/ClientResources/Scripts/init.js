define([
    "dojo",
    "epi/dependency",
    "epi-cms/plugin-area/navigation-tree",
    "epinova-quickexport/ExportCommand",
    "epinova-quickexport/ImportCommand",
    "epinova-quickexport/commandsProvider"
],
function (
    dojo,
    dependency,
    navigationTreePluginArea,
    ExportCommand,
    ImportCommand,
    CommandsProvider
) {
    return dojo.declare([], {
        timeout: 0,
        hasAccess: false,

        constructor: function (settings) {
            this.inherited(arguments);
            this.timeout = settings.timeout;
            this.hasAccess = settings.hasAccess;
        },

        initialize: function () {
            this.inherited(arguments);

            var commandregistry = dependency.resolve("epi.globalcommandregistry");

            var commandsProvider = new CommandsProvider({
                timeout: this.timeout,
                hasAccess: this.hasAccess
            });

            var exportCommand = new ExportCommand(this.timeout);
            exportCommand.set("isAvailable", this.hasAccess);

            var importCommand = new ImportCommand();
            importCommand.set("isAvailable", this.hasAccess);

            navigationTreePluginArea.add(exportCommand);
            navigationTreePluginArea.add(importCommand);
        }
    });
});