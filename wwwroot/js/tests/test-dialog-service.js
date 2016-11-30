function TestDialogService() {
    var self = this;

    self.confirm = function (message) {
        return true;
    };

    self.showMessage = function (message) {
    }

    self.prompt = function (message) {
        return message;
    }
}