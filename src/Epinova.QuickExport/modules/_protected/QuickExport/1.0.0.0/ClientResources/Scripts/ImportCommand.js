define([
    "dojo/topic",
    "dojo/_base/declare",
    "epi/dependency",
    "epi/shell/command/_Command",
    "epi/shell/widget/dialog/Alert",
    "epi/i18n!epi/nls/epinovaquickexport.widget",
    "dijit/form/Form",
    "epi-cms/widget/viewmodel/MultipleFileUploadViewModel",
    "epi-cms/widget/MultipleFileUpload"
], function (
    topic,
    declare,
    dependency,
    _Command,
    Alert,
    translator,
    Form,
    MultipleFileUploadViewModel,
    MultipleFileUpload
) {
    return declare([_Command], {
        name: 'import',
        label: translator.import.button.label,
        tooltip: translator.import.button.tooltip,
        iconClass: 'epi-iconUpload',
        canExecute: true,

        _execute: function () {
            var target = this.model.contentLink;

            var uploader = new MultipleFileUpload({
                model: new MultipleFileUploadViewModel({}),
                createAsLocalAsset: false
            });

            uploader.uploaderInput.uploadUrl = '/QuickExport/Upload?id=' + target;

            var dialog = new Alert({
                title: translator.import.dialog.title,
                heading: translator.import.dialog.heading,
                description: translator.import.dialog.description,
                acknowledgeActionText: translator.import.dialog.close,
                closeIconVisible: true,
                content: uploader,
                onAction: function() {
                    topic.publish('/epi/cms/contentdata/childrenchanged', target);
                }
            });
            dialog.show();
        }
    });
});