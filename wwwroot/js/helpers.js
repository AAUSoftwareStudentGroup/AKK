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

Handlebars.registerHelper("log", function(something) {
 console.log(something);
});

var templates = [];
function setUpContentUpdater(objs, callback) {
    asyncLoop({
    length : objs.length,
    functionToLoop : function(loop, i){
        var obj = objs.shift();
        $.get(obj.scriptSource, function(response) {
            templates[obj.elementId] = Handlebars.compile(response);
            obj.viewmodel.addEventListener(obj.event, function() {
                $("#" + obj.elementId).html(templates[obj.elementId](obj.viewmodel));
            });
            loop();
        });
    },
    callback : function(){
        callback();
    }
    });
}

//http://stackoverflow.com/a/7654602
var asyncLoop = function(o){
    var i=-1;

    var loop = function(){
        i++;
        if(i==o.length){o.callback(); return;}
        o.functionToLoop(loop, i);
    } 
    loop();//init
}