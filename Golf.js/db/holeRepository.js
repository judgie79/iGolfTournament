var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.holes);

var Validator = require('./holeValidator');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newhole, callback) {

    var val = new Validator(newhole);

    var updater = new Updater();
    var db = mongoUtil.getDb();


    val.validateSchema().then(function () {
        crudRepository.create(newhole, callback);
    }).catch(function (err) {
        callback(err, null);
    });
}

module.exports.update = function (id, updateHole, callback) {
    var val = new Validator(updateHole);

    var updater = new Updater();
    var db = mongoUtil.getDb();

    val.validateSchema().then(function () {
        crudRepository.update(id, updateHole, callback);
    }).catch(function (err) {
        callback(err, null);
    });
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}

module.exports.findHolesOfClub = function (clubId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.holes).find({ "clubId": new ObjectId(clubId) }).toArray(function (err, holes) {

        callback(err, holes);
    });
}