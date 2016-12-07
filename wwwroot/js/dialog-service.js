function DialogService() {
    var self = this;

    self.confirm = function(message) {
        return confirm(message);
    };


    self.prompt = function(message) {
        return prompt(message);
    }

    self.showMessage = function(message) {
        $("body").append(`<div class="message info" onclick="$(this).fadeOut(function() {$(this).remove()})"><span>${message}</span></div>`)
    }

    self.showError = function(message) {
        $("body").append(`<div class="message error" onclick="$(this).fadeOut(function() {$(this).remove()})"><span>${message}</span></div>`)
    }
}