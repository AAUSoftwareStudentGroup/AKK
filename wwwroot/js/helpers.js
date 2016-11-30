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

Handlebars.registerHelper('if_be', function (a, opts) {
    if (a) // Or === depending on your needs
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

function resizeImage(image, callback) {
    var maxWidth = 500;
    if (image.width < maxWidth) callback(image);
    var ratio = image.width / image.height;

    var canvas =  document.createElement('canvas');
    canvas.width = maxWidth;
    canvas.height = maxWidth / ratio;
    var context = canvas.getContext("2d");
    context.drawImage(image, 0, 0, canvas.width, canvas.height);
    var newImageData = canvas.toDataURL();

    var newImage = new Image();
    newImage.src = newImageData;
    newImage.onload = function() {
        callback(newImage);
    }
}

function readURL(input, callback) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    var i = document.createElement('img');
    reader.onload = function (e) {
        i.setAttribute("src", e.target.result);
        i.onload = function(e) {
            callback(i);
        }
    }
    reader.readAsDataURL(input.files[0]);
  }
}

function hslToRgb(h, s, l){
    var r, g, b;

    if(s == 0){
        r = g = b = l; // achromatic
    }else{
        var hue2rgb = function hue2rgb(p, q, t){
            if(t < 0) t += 1;
            if(t > 1) t -= 1;
            if(t < 1/6) return p + (q - p) * 6 * t;
            if(t < 1/2) return q;
            if(t < 2/3) return p + (q - p) * (2/3 - t) * 6;
            return p;
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;
        r = hue2rgb(p, q, h + 1/3);
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 1/3);
    }

    return {r: Math.round(r * 255), g: Math.round(g * 255), b: Math.round(b * 255)};
}