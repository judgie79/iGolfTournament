var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courses);

var Validator = require('./courseValidator');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findCoursesOfClub = function (clubId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "clubId": new ObjectId(clubId) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newcourse, callback) {
    var val = new Validator(newcourse);

    var db = mongoUtil.getDb();
    val.validateSchema().then(function () {
        newcourse.clubId = new ObjectId(newcourse.clubId);

        if (newcourse.teeBoxes && newcourse.teeboxes.length > 0) {
            for (var i = 0; i < newcourse.teeboxes.length; i++) {
                var teeBox = newcourse.teeboxes[i];
                if (teeBox._id === "") {
                    teeBox._id = new ObjectId();
                } else {
                    teeBox._id = new ObjectId(hole._id);
                }

                if (teeBox.holes && teeBox.holes.front && teeBox.holes.front.length > 0) {
                    for (var h = 0; h < teeBox.holes.front.length; h++) {
                        var hole = teeBox.holes.front[h];

                        if (hole.holeId === "") {
                            hole.holeId = new ObjectId();
                        } else {
                            hole.holeId = new ObjectId(hole.holeId);
                        }
                    }
                }

                if (teeBox.holes && teeBox.holes.back && teeBox.holes.back.length > 0) {
                    for (var h = 0; h < teeBox.holes.back.length; h++) {
                        var hole = teeBox.holes.back[h];

                        if (hole.holeId === "") {
                            hole.holeId = new ObjectId();
                        } else {
                            hole.holeId = new ObjectId(hole.holeId);
                        }
                    }
                }
            }
        }

        crudRepository.create(newcourse, callback);

    }).catch(function (err) {
        callback(err, newcourse);
    });
}

module.exports.update = function (id, updateCourse, callback) {

    var val = new Validator(updateCourse);

    var db = mongoUtil.getDb();

    val.validateSchema().then(function () {

        db.collection(config.db.collections.courses).update(
            {
                "_id": new ObjectId(id),
            },
            {
                "$set": {
                    "name": updateCourse.name
                }
            },
            function (err, part) {
                db.collection(config.db.collections.courses).findOne(
                    { "_id": new ObjectId(id) },
                    function (err, course) {
                        callback(err, course);
                    }
                );
            }); 
    }).catch(function (err) {
        callback(err, updateCourse);
    });
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}