var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var CLUBS_COLLECTION = "Clubs";

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(CLUBS_COLLECTION);

var clubSchema = require('../schemas/club.js');

module.exports.findAll = function (callBack) {
    crudRepository.findAll(callBack);
}

module.exports.findById = function (id, callBack) {
    crudRepository.findById(id, callBack);
}

module.exports.create = function (newclub, callBack) {

    var isValid = crudRepository.validate(newclub, clubSchema);

    if (isValid.length == 0)
        crudRepository.create(newplayer, callback);
    else
        callback(isValid, null);
}

module.exports.update = function (id, updateClub, callBack) {
    var isValid = crudRepository.validate(updateClub, clubSchema);

    if (isValid.length == 0)
        crudRepository.update(newplayer, callback);
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}