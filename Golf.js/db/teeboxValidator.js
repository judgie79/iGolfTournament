
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var holeSchema = require('../schemas/hole.js');
var courseHoleSchema = require('../schemas/courseHole.js');
var teeboxSchema = require('../schemas/teebox.js');

var Validator = function (teebox) {
    this.teebox = teebox;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(holeSchema);
        js.register(courseHoleSchema);

        var isValid = js.validate(me.teebox, teeboxSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Teebox is not compatible to schema", message: "Teebox is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;