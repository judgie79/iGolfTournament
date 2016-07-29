
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var participantSchema = require('../schemas/participant.js');

var Validator = function (participant) {
    this.participant = participant;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        //js.register(addressSchema);

        var isValid = js.validate(me.participant, participantSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Participant is not compatible to schema", message: "Participant is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;