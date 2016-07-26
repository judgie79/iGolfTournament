var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.clubs);

var clubSchema = require('../schemas/club.js');
var Validator = require('./clubValidator');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newclub, callback) {

    var val = new Validator(newclub);

    var db = mongoUtil.getDb();
    val.validateSchema().then(function () {
        crudRepository.create(newclub, callback);
    }).catch(function (err) {
        callback(err, newclub);
    });
}

module.exports.update = function (id, updateClub, callback) {
   var val = new Validator(updateClub);

    var db = mongoUtil.getDb();
    val.validateSchema().then(function () {
        crudRepository.update(id, updateClub, callback);
    }).catch(function (err) {
        callback(err, updateClub);
    });
}

module.exports.delete = function (id, callback) {
    
    var db = mongoUtil.getDb();

    //delete all courses
    db.collection(config.db.collections.courses).deleteMany({ "clubId": new ObjectID(id) }, function (err, doc) {

        if (err){
            callback(err, null);
        }
        crudRepository.delete(id, callback);
    });
    
    
}