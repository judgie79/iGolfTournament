
var ErrorHandler = function () {
    
}
// Generic error handler used by all endpoints.
ErrorHandler.prototype.handleError = function (res, reason, message, code, value) {
    console.log("ERROR: " + reason);
    
    var result = { 
            "error": reason,
            "message": message
        };

    if (value)
        result["value"] = value;

    res.status(code || 500).json(result);
}

module.exports = ErrorHandler;