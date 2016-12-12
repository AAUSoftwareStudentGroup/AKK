function TestDialogService() {
    var self = this;
    this.message = "";
    this.confirm = true;
    this.confirm = function (message) {
        return true;
    };

    this.showMessage = function (message) {
    }

    this.showError = function(message) {
    }

    this.showInfo = function (message) {
    }

    this.prompt = function (message) {
        return self.message;
    }
}