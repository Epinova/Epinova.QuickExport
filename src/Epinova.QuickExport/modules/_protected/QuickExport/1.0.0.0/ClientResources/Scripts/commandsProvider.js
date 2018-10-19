define([
    "dojo",
    "dojo/_base/declare",
    "epi/shell/command/_CommandProviderMixin"
], function (
    dojo,
    declare,
    _CommandProviderMixin
) {
        return declare([_CommandProviderMixin], {

            constructor: function () {
                this.inherited(arguments);

                this._settings = {
                    timeout: -1,
                    hasAccess: true
                };
            }
        });
    });