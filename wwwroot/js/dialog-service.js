function DialogService() {
    var self = this;

    self.confirm = function(message) {
        return confirm(message);
    };

    self.showMessage = function(message) {
        alert(message);
    }

    self.prompt = function(message) {
        return prompt(message);
    }
}