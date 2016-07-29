var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var courseSchema = require('../schemas/course.js');
var teeboxSchema = require('../schemas/teebox.js');
var holeSchema = require('../schemas/hole.js');

var Validator = function (course) {
    this.course = course;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(holeSchema);
        js.register(teeboxSchema);

        var isValid = js.validate(me.course, courseSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Course is not compatible to schema", message: "Course is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;