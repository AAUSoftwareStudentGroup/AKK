function DialogService() {
    var self = this;

    self.confirm = function(message) {
        return confirm(message);
    };


    self.prompt = function(message) {
        return prompt(message);
    }

    self.showInfo = function(message) {
        $(".message").remove();
        $("body").append(`
            <div class="message info" onclick="$(this).fadeOut(function() {$(this).remove()})">
                <span>${message}</span>
                <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                    <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                </svg>
            </div>`);
    }

    self.showError = function(message) {
        $(".message").remove();
        $("body").append(`
            <div class="message error" onclick="$(this).fadeOut(function() {$(this).remove()})">
                <span>${message}</span>
                <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                    <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                </svg>
            </div>`);
    }
}