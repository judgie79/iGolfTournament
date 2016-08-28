
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var holeSchema = require('../schemas/hole.js');
var courseHoleSchema = require('../schemas/courseHole.js');

var Validator = function (hole) {
    this.hole = hole;
};

Validator.prototype.validateSchema = function () {
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(holeSchema);
        js.register(courseHoleSchema);

        var isValid = js.validate(me.hole, courseHoleSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "CourseHole is not compatible to schema", message: "CourseHole is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;