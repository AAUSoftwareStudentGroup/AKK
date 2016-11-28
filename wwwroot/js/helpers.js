Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1.g <= v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('if_eq', function (a, b, opts) {
    if (a == b) // Or === depending on your needs
        return opts.fn(this);
    else
        return opts.inverse(this);
});

var templates = [];
function setUpContentUpdater(obj) {
    $.get(obj.scriptSource, function(response) {
        templates[obj.elementId] = Handlebars.compile(response);
        obj.viewmodel.addEventListener(obj.event, function() {
            $("#" + obj.elementId).html(templates[obj.elementId](obj.viewmodel));
        });
        if (obj.callback) {
            obj.callback();
        }
    });
}