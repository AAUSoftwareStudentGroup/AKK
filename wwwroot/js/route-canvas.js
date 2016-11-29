function RouteCanvas(canvas, image, viewModel, editable = false) {
    this.viewModel = viewModel;
    var self = this;
    this.canvas = canvas;
    this.image = image;

    this.isDrawing = false;
    this.currentCircle;

    this.viewModel.HoldPositions = this.viewModel.HoldPositions || [];

    this.viewModel.addEventListener("HoldsUpdated", function() {
        self.DrawCanvas();
    });

    this.resize = function(){
        var ratio = this.canvas.width / this.canvas.height;
        var currentWidth = $(this.canvas).width();
        var newHeight = currentWidth / ratio;
        this.canvas.width = currentWidth;
        this.canvas.height = newHeight;
        this.context = this.canvas.getContext("2d");
        this.context.strokeStyle = "#FF0000";
        this.context.lineWidth = 4;
    }
    this.resize();


    this.DrawCanvas();
    this.latestClick;

    if (editable) {        

        $(this.canvas).click(function(e) {
            var c = $(self.canvas);
            var position = c.offset();
            mmouseX = (e.pageX-position.left) /c.width();
            mmouseY = (e.pageY-position.top) / c.height();

            if (self.latestClick) {
                var dist = Math.sqrt(Math.pow(self.latestClick.x * c.width()  - mmouseX * c.width(),  2) + 
                                     Math.pow(self.latestClick.y * c.height() - mmouseY * c.height(), 2));
                var lastCircle = self.viewModel.HoldPositions.length - 1;
                if (dist < viewModel.HoldPositions[lastCircle].radius * c.width()) {
                    self.viewModel.HoldPositions[lastCircle].radius *= 1.2;
                    self.DrawCanvas();
                    return;
                }
            }
            self.viewModel.addHold({x: mmouseX, y: mmouseY, radius: 0.06});
            self.latestClick = {x: mmouseX, y: mmouseY};
        });
    }
}

RouteCanvas.prototype.DrawCanvas = function() {
    var c = $(this.canvas);
    this.context.clearRect(0,0, c.width(), c.height());
    this.context.drawImage(this.image, 0, 0, c.width(), c.height());

    for (var i = 0; i < this.viewModel.HoldPositions.length; i++) {
        var circle = this.viewModel.HoldPositions[i];
        this.DrawCircle(circle.x * c.width(), circle.y * c.height(), circle.radius * c.width())
    }
}

RouteCanvas.prototype.DrawCircle = function(x, y, r) {
    this.context.beginPath();
    this.context.arc(x, y, r, 0, Math.PI * 2);
    this.context.stroke();
} 


RouteCanvas.prototype.undo = function() {
    if (this.viewModel.HoldPositions.length){
        this.viewModel.HoldPositions.pop();
    }
    this.latestClick = null;
    this.DrawCanvas();
}