define([
    "dojo",
    "epi/dependency",
    "epi-cms/plugin-area/navigation-tree",
    "epinova-quickexport/ExportCommand",
    "epinova-quickexport/ImportCommand"
],
function (
    dojo,
    dependency,
    navigationTreePluginArea,
    ExportCommand,
    ImportCommand
) {
    return dojo.declare([], {
        initialize: function () {
            navigationTreePluginArea.add(ExportCommand);
            navigationTreePluginArea.add(ImportCommand);
        }
    });
});