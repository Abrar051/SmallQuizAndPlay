(function ($) {
    $.session = {
        set: function (key, value, onceOnly) {
                try {
                    window.localStorage.setItem(key, value);
                } catch (e) {
                }
                return this;
        },
        get: function (key) {
            return window.localStorage.getItem(key);
        },
        remove: function (key) {
            try {
                window.localStorage.removeItem(key);
            } catch (e) { };
            return this;
        },
    }
})(jQuery);