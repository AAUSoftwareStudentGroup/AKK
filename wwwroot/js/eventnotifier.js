function EventNotifier() {
    this.listeners = []

    //The function which will add newly created events
    this.addEventListener = function(event, callback) {
        this.listeners[event] = this.listeners[event] || [];
        this.listeners[event].push(callback);
    }

    //When called, execute the event
    this.trigger = function(event, arguments) {
        arguments = arguments || {};
        var currentEvent = this.listeners[event];
        if (currentEvent) {
            for (var i = 0; i < currentEvent.length; i++) {
                currentEvent[i](arguments);
            }
        }
    }

    //Removes the event from the listen
    this.removeEventListener = function(event, callback) {
        if (!this.listeners[event]) return false;
        var eventListeners = this.listeners[event];
        for (var i = 0; i < eventListeners.length; i++) {
            if (eventListeners[i] == callback) {
                eventListeners.splice(i, 1);
                return true;
            }
        }
        return false;
    }
}