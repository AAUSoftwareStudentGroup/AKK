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

Handlebars.registerHelper('if_eq_or', function (a, b, c, opts) {
    if (a == b || c) // Or === depending on your needs
        return opts.fn(this);
    else
        return opts.inverse(this);
});

Handlebars.registerHelper("log", function(something) {
 console.log(something);
});

Handlebars.registerHelper("formatdate", function(dateString) {
    var date = new Date(dateString);
    return date.getDay() + "/" + date.getMonth() + "/" + date.getFullYear();
});

Handlebars.registerHelper('breaklines', function(text) {
    text = Handlebars.Utils.escapeExpression(text);
    text = text.replace(/(\r\n|\n|\r)/gm, '<br>');
    return new Handlebars.SafeString(text);
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

function rotateImage(image, times, callback) {
    var ratio = image.width / image.height;
    var newImageWidth = image.width;
    var newImageHeight = image.height;

    if(times % 2 == 1)
    {
        ratio = image.height / image.width;
        newImageWidth = image.height;
        newImageHeight = image.width;
    }

    console.log(newImageHeight);
    console.log(newImageWidth);

    var canvas =  document.createElement('canvas');
    canvas.width = newImageWidth;
    canvas.height = newImageHeight;
    var context = canvas.getContext("2d");

    context.save();

    context.translate(canvas.width / 2, canvas.height / 2);

    context.rotate(times * 90 * (Math.PI * 2) / 360);

    context.drawImage(image, -(image.width/2), -(image.height/2), image.width, image.height);

    context.restore();

    var rotatedImageData = canvas.toDataURL();

    var rotatedImage = new Image();
    rotatedImage.src = rotatedImageData;
    rotatedImage.onload = function() {
        callback(rotatedImage);
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