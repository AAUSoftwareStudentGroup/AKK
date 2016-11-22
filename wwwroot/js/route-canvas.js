function RouteCanvas(canvas, image, viewModel, editable = false) {
    this.viewModel = viewModel;
    var self = this;
    this.canvas = canvas;
    this.image = image;
    this.context = this.canvas.getContext("2d");
    this.context.strokeStyle = "#FF0000";
    this.context.lineWidth = 4;

    this.isDrawing = false;
    this.currentCircle;

    this.viewModel.HoldPositions = this.viewModel.HoldPositions || [];

    this.viewModel.addEventListener("HoldsUpdated", this.DrawCanvas);

    this.DrawCanvas();
    if (editable) {
        this.canvas.addEventListener("touchstart", function (e) {
            mousePos = getTouchPos(canvas, e);
            var touch = e.touches[0];
            var mouseEvent = new MouseEvent("mousedown", {
                clientX: touch.clientX,
                clientY: touch.clientY
            });
            self.canvas.dispatchEvent(mouseEvent);
        }, false);
        
        self.canvas.addEventListener("touchend", function (e) {
            var mouseEvent = new MouseEvent("mouseup", {});
            self.canvas.dispatchEvent(mouseEvent);
        }, false);
        
        self.canvas.addEventListener("touchmove", function (e) {
            var touch = e.touches[0];
            var mouseEvent = new MouseEvent("mousemove", {
                clientX: touch.clientX,
                clientY: touch.clientY
            });
            self.canvas.dispatchEvent(mouseEvent);
        }, false);

        // Get the position of a touch relative to the canvas
        function getTouchPos(canvasDom, touchEvent) {
            var rect = canvasDom.getBoundingClientRect();
            return {
                x: touchEvent.touches[0].clientX - rect.left,
                y: touchEvent.touches[0].clientY - rect.top
            };
        }


        this.canvas.onmousedown = function(e) {
            var c = $(self.canvas);
            var position = c.offset();
            self.isDrawing = true;
            self.currentCircle = new Object();
            self.currentCircle.x = (e.pageX-position.left) / c.width();
            self.currentCircle.y = (e.pageY-position.top)  / c.height();
        };

        this.canvas.onmouseup = function(e) {
            self.isDrawing = false;
            self.viewModel.HoldPositions.push(self.currentCircle);
        };

        this.canvas.onmousemove = function(e) {
            var c = $(self.canvas);
            var position = c.offset();
            if (!self.isDrawing) return;
            
            self.DrawCanvas();

            mmouseX = (e.pageX-position.left) /c.width();
            mmouseY = (e.pageY-position.top) / c.height();

            var newRadius = Math.sqrt(Math.pow(self.currentCircle.x - mmouseX, 2)+Math.pow(self.currentCircle.y - mmouseY, 2))
            self.currentCircle.r = newRadius / 2;
            self.DrawCircle(self.currentCircle.x * c.width(), self.currentCircle.y * c.height(), self.currentCircle.r * c.width())
        };
    }
       

}

RouteCanvas.prototype.DrawCanvas = function() {
    var c = $(this.canvas);
    this.context.clearRect(0,0, c.width(), c.height());
    this.context.drawImage(this.image, 0, 0, c.width(), c.height());

    for (var i = 0; i < this.viewModel.HoldPositions.length; i++) {
        var circle = this.viewModel.HoldPositions[i];
        this.DrawCircle(circle.x * c.width(), circle.y * c.height(), circle.r * c.width())
    }
}

RouteCanvas.prototype.DrawCircle = function(x, y, r) {
    this.context.beginPath();
    this.context.arc(x, y, r, 0, Math.PI * 2);
    this.context.stroke();
} 


RouteCanvas.prototype.Undo = function() {
    if (this.viewModel.HoldPositions.length)
        this.HoldPositions.pop();
    this.DrawCanvas();
}