var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courses);

var Validator = require('./teeboxValidator');

module.exports.findAll = function (courseId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "_id": new ObjectId(courseId) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.findById = function (courseId, id, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "_id": new ObjectId(courseId), "teeboxes._id": new ObjectId(id) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.create = function (courseId, newTeebox, callback) {
    
    var val = new Validator(newTeebox);

    var db = mongoUtil.getDb();

    newTeebox._id = new ObjectId();

    var parAndDistance = getParAndDistance(newTeebox);
    newTeebox.distance = parAndDistance.front.distance + parAndDistance.back.distance;
    newTeebox.par = parAndDistance.front.par + parAndDistance.back.par;

    val.validateSchema().then(function() {
        db.collection(config.db.collections.courses).update(
        {
            "_id": new ObjectId(courseId)
        },
        {
            "$push": { "teeboxes": newTeebox }
        },
        function (err, part) {
            db.collection(config.db.collections.courses).findOne(
                { "_id": new ObjectId(courseId) },
                function (err, course) {
                    callback(err, course);
                }
            );
        });
    }).catch(function (err) {
        callback(err, newTeebox);
    });

    
}

function getParAndDistance(teebox) {
    var frontLength = 0;
    var frontPar = 0;

    if (teebox.holes && teebox.holes.front && teebox.holes.front.length > 0) {
        for (var h = 0; h < teebox.holes.front.length; h++) {
            var hole = teebox.holes.front[h];
            frontLength = frontLength + hole.distance;
            frontPar = frontPar + hole.par;
        }
    }

    var backLength = 0;
    var backPar = 0;

    if (teebox.holes && teebox.holes.back && teebox.holes.back.length > 0) {
        for (var h = 0; h < teebox.holes.back.length; h++) {
            var hole = teebox.holes.back[h];
            backLength = backLength + hole.distance;
            backPar = backPar + hole.par;
        }
    }

    return {
        front: {
            distance: frontLength,
            par: frontPar
        },
        back: {
            distance: backLength,
            par: backPar
        }
    }
}

module.exports.update = function (courseId, id, updateTeebox, callback) {
    var val = new Validator(updateTeebox);

    var db = mongoUtil.getDb();

    updateTeebox._id = new ObjectId(id);

    

    val.validateSchema().then(function() {

        db.collection(config.db.collections.courses).findOne(
            { "_id": new ObjectId(courseId) },
            function (err, course) {
                callback(err, course);

                var teebox = course.teeboxes.find(function (t) {
                    return t._id == id;
                });

                updateTeebox.holes = teebox.holes;

                var parAndDistance = getParAndDistance(updateTeebox);
                updateTeebox.distance = parAndDistance.front.distance + parAndDistance.back.distance;
                updateTeebox.par = parAndDistance.front.par + parAndDistance.back.par;

                db.collection(config.db.collections.courses).update(
                    {
                        "_id": new ObjectId(courseId),
                        "teeboxes._id": updateTeebox._id,
                    },
                    {
                        "$set": { "teeboxes.$": updateTeebox }
                    },
                    function (err, part) {
                        db.collection(config.db.collections.tournaments).findOne(
                            { "_id": new ObjectId(courseId) },
                            function (err, course) {
                                callback(err, course);
                            }
                        );
                    });
            });
    }).catch(function (err) {
        callback(err, updateTeebox);
    });
}

module.exports.delete = function (id, callback) {
    updateTeebox._id = new ObjectId(id);
    db.collection(config.db.collections.courses).update(
        {
            "_id": new ObjectId(courseId),
            "teeboxes._id": updateTeebox._id,
        },
        {
            "$pull": { "teeboxes.$": updateTeebox }
        },
        function (err, part) {
            db.collection(config.db.collections.tournaments).findOne(
                { "_id": new ObjectId(courseId) },
                function (err, course) {
                    callback(err, course);
                }
            );
        }
    );
}