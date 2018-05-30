define([
    "dojo/topic",
    "dojo/_base/declare",
    "epi/dependency",
    "epi/shell/command/_Command",
    "epi/shell/widget/dialog/Alert",
    "epi/i18n!epi/nls/epinovaquickexport.widget"
], function (
    topic,
    declare,
    dependency,
    _Command,
    Alert,
    translator
) {
    return declare([_Command], {
        name: 'export',
        label: translator.export.button.label,
        tooltip: translator.export.button.tooltip,
        iconClass: 'epi-iconDownload',
        canExecute: true,

        _execute: function () {
            dojo.rawXhrPost({
                url: '/QuickExport/Export',
                handleAs: 'json',
                headers: { 'Content-Type': 'application/json' },
                timeout: 60000,
                postData: dojo.toJson({ 'id': this.model.contentLink }),
                load: function (data) {
                    if (!!data && !!data.success) {
                        window.location = '/QuickExport/Download?id=' + data.id;
                    } else {
                        new Alert({
                            heading: translator.errors.download,
                            description: translator.errors.suggest
                        }).show();
                    }
                },
                error: function (error) {
                    new Alert({
                        heading: translator.errors.download + ': ' + error,
                        description: translator.errors.suggest
                    }).show();
                }
            });
        }
    });
});