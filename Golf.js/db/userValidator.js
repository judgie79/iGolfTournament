var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var userSchema = require('../schemas/user.js');
var addressSchema = require('../schemas/address.js');

var Validator = function (userRepository, user) {
    this.user = user;
    this.userRepository = userRepository;
};

Validator.prototype.validateNewUser = function () {
    var me = this;

    return new Promise(function (resolve, reject) {
        me.validateSchema.then(function () {
            return me.validateUsername();
        }).then(function() {
            return me.validateEmail();
        }).then(function() {
            return me.validatePassword();
        }).catch(function (err) {
            
        });
    });
}

Validator.prototype.validatePassword = function() {

    var me = this;
    return new Promise(function (resolve, reject) {
        if (me.user.password != me.user.passwordRepeat) {
            reject({ reason: "Passwords do not match", message: "Password and password repeat do not match." });
        } else {
            resolve();
        }
    });
}

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(addressSchema);
        var isValid = js.validate(me.user, userSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "User is not compatible to schema", message: "User is not compatible to schema", validationResult: isValid });
        }
    });
};

//check if the username is unique
Validator.prototype.validateUsername = function ()
 {
    var me = this;
    return new Promise(function (resolve, reject) {
        this.userRepository.findByUsername(me.user.username, false, function (err, user) {
            if (!err && user) {
                //username was found
                reject({
                    reason: "username must be unique",
                    message: "Please choose a different username"}, null);
            } else {
                resolve();
            }
        });
    });
 }   

//check if the email is unique    
Validator.prototype.validateEmail = function () {
        
    var me = this;
    return new Promise(function (resolve, reject) {
        this.userRepository.findByEmail(me.user.email, false, function (err, email) {
            if (!err && email) {
                //email was found
                reject({
                    reason: "email must be unique",
                    message: "This email address is already registered"}, null);
            } else {
                resolve();
            }
        });
    });
}

module.exports = Validator;
    