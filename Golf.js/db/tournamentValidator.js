var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var tournamentSchema = require('../schemas/tournament.js');
var courseSchema = require('../schemas/course.js');
var clubSchema = require('../schemas/club.js');
var addressSchema = require('../schemas/address.js');
var holeSchema = require('../schemas/hole.js');

var Validator = function (tournament) {
    this.tournament = tournament;
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(addressSchema);
        js.register(courseSchema);
        js.register(clubSchema);
        js.register(holeSchema);
        

        var isValid = js.validate(me.tournament, tournamentSchema);

        if (isValid.length == 0) {
            resolve(me.tournament);
        } else {
            reject({ reason: "Tournament is not compatible to schema", message: "Tournament is not compatible to schema", validationResult: isValid });
        }
    });
};

Validator.prototype.tournamentStarted = function () {
    var me = this;
    return new Promise(function(resolve, reject) {
        if (me.tournament.hasStarted != null && me.tournament.hasStarted) {
            resolve(me.tournament);
        }
        else {
            reject({ reason: "Tournament has already started", message: "Tournament has already started" });
        }
    });
};

Validator.prototype.tournamentNotStarted = function () {
    var me = this;
    return new Promise(function(resolve, reject) {
        if (me.tournament.hasStarted == null || !me.tournament.hasStarted) {
                resolve(me.tournament);
            }
            else {
                reject({ reason: "Tournament has not started", message: "Tournament has not started" });
            }
    });
};

module.exports = Validator;