function TestDialogService() {
    var self = this;
    this.message = "";
    self.confirm = function (message) {
        return true;
    };

    self.showMessage = function (message) {
    }

    self.prompt = function (message) {
        return this.message;
    }
}