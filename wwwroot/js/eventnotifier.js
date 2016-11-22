function EventNotifier() {
    this.listeners = []
}

EventNotifier.prototype.addEventListener = function(event, callback) {
    this.listeners[event] = this.listeners[event] || [];
    this.listeners[event].push(callback);
}

EventNotifier.prototype.trigger = function(event, arguments) {
    arguments = arguments || {};
    var currentEvent = this.listeners[event];
    if (currentEvent) {
        for (var i = 0; i < currentEvent.length; i++) {
            currentEvent[i](arguments);
        }
    }
}

EventNotifier.prototype.removeEventListener = function(event, callback) {
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