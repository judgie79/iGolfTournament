
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var holeSchema = require('../schemas/hole.js');

var Validator = function (hole) {
    this.hole = hole;
};

Validator.prototype.validateSchema = function () {
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(holeSchema);

        var isValid = js.validate(me.hole, holeSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Hole is not compatible to schema", message: "Hole is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;