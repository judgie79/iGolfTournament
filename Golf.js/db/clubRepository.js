var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.clubs);

var clubSchema = require('../schemas/club.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newclub, callback) {

    var isValid = crudRepository.validate(newclub, clubSchema);

    if (isValid.length == 0)
        crudRepository.create(newclub, callback);
    else
        callback(isValid, null);
}

module.exports.update = function (id, updateClub, callback) {
    var isValid = crudRepository.validate(updateClub, clubSchema);

    if (isValid.length == 0)
        crudRepository.update(id, updateClub, callback);
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).deleteMany({ "clubId": new ObjectID(id) }, function (err, doc) {

        if (err){
            callback(err, null);
        }
        crudRepository.delete(id, callback);
    });
    
    
}