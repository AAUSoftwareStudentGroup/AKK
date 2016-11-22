function EventNotifier() {
    this.listeners = []
}

EventNotifier.prototype.AddEventListener = function(event, callback) {
    this.listeners[event] = this.listeners[event] || [];
    this.listeners[event].push(callback);
}

EventNotifier.prototype.Trigger = function(event, arguments) {
    arguments = arguments || {};
    var currentEvent = this.listeners[event];
    if (currentEvent) {
        for (var i = 0; i < currentEvent.length; i++) {
            currentEvent[i](arguments);
        }
    }
}

EventNotifier.prototype.RemoveEventListener = function(event, callback) {
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