
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var scorecardSchema = require('../schemas/scorecard.js');

var Validator = function (scorecard) {
    this.scorecard = scorecard;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        //js.register(addressSchema);

        var isValid = js.validate(me.scorecard, scorecardSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Scorecard is not compatible to schema", message: "Scorecard is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;