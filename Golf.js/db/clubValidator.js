var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var clubSchema = require('../schemas/club.js');
var addressSchema = require('../schemas/address.js');

var Validator = function (club) {
    this.club = club;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(addressSchema);

        var isValid = js.validate(me.club, clubSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Club is not compatible to schema", message: "Club is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;