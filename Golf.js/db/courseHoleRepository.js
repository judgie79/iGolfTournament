var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courseHoles);

var Validator = require('./courseHoleValidator');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (courseId, teeboxId, id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (courseId, teeboxId, newhole, callback) {

    var val = new Validator(newhole);
    var db = mongoUtil.getDb();


    db.collection(config.db.collections.holes).findOne({
        "_id": new ObjectId(newhole._id)
    }, function (err, hole) {
        newhole.clubId = hole.clubId;
        newhole.name = hole.name;
        if (hole.courseImage) {
            newhole.courseImage = hole.courseImage;
        }
        newhole.frontOrBack = newhole.frontOrBack.toLowerCase();
        newhole.teeboxId = teeboxId;
        newhole.courseId = courseId;

        val.validateSchema().then(function () {
            crudRepository.create(newhole, callback);
        }).catch(function (err) {
            callback(err, null);
        });
    });


}

module.exports.update = function (courseId, teeboxId, id, updateHole, callback) {
    var val = new Validator(updateHole);
    var db = mongoUtil.getDb();

    db.collection(config.db.collections.holes).findOne({
        "_id": new ObjectId(updateHole._id)
    }, function (err, hole) {
        updateHole.clubId = hole.clubId;
        updateHole.name = hole.name;
        updateHole.courseImage = hole.courseImage;
        updateHole.frontOrBack = updateHole.frontOrBack.toLowerCase();
        val.validateSchema().then(function () {
            crudRepository.update(id, updateHole, callback);
        }).catch(function (err) {
            callback(err, null);
        });
    });
}

module.exports.delete = function (courseId, teeboxId, id, callback) {
    crudRepository.delete(id, callback);
}

module.exports.findHolesOfTeebox = function (teeboxId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courseHoles).find({ "teeboxId": teeboxId }).toArray(function (err, holes) {

        callback(err, holes);
    });
}