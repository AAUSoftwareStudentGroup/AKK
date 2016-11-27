function StorageService() {
    this.save = function(key, data) {
        localStorage.setItem(key, JSON.stringify(data));
    };
    this.load = function(key) {
        return JSON.parse(localStorage.getItem(key));
    };
}