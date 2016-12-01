function TestDialogService() {
    var self = this;
    this.message = "";
    this.confirm = true;
    self.confirm = function (message) {
        return true;
    };

    self.showMessage = function (message) {
    }

    self.prompt = function (message) {
        return this.message;
    }
}