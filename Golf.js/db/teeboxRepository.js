var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courses);

var Validator = require('./teeboxValidator');

module.exports.findAll = function (courseId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "_id": new ObjectID(courseId) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.findById = function (courseId, id, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "_id": new ObjectID(courseId), "teeboxes._id": new ObjectID(id) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.create = function (courseId, newTeebox, callback) {
    
    var val = new Validator(newTeebox);

    var db = mongoUtil.getDb();

    newTeebox._id = new ObjectID();

    var frontLength = 0;
    var frontPar = 0;

    if (newTeebox.holes && newTeebox.holes.front && newTeebox.holes.front.length > 0) {
        for (var h = 0; h < newTeebox.holes.front.length; h++) {
            var hole = newTeebox.holes.front[h];
            hole._id = new ObjectID();
            frontLength = frontLength + hole.distance;
            frontPar = frontPar + hole.par;
        }
    }

    var backLength = 0;
    var backPar = 0;

    if (newTeebox.holes && newTeebox.holes.back && newTeebox.holes.back.length > 0) {
        for (var h = 0; h < newTeebox.holes.back.length; h++) {
            var hole = newTeebox.holes.back[h];
            hole._id = new ObjectID();
            backLength = backLength + hole.distance;
            backPar = backPar + hole.par;
        }


    }

    newTeebox.distance = backLength + frontLength;
    newTeebox.par = backPar + frontPar;

    val.validateSchema().then(function() {
        db.collection(config.db.collections.courses).update(
        {
            "_id": new ObjectID(courseId)
        },
        {
            "$push": { "teeboxes": newTeebox }
        },
        function (err, part) {
            db.collection(config.db.collections.courses).findOne(
                { "_id": new ObjectID(courseId) },
                function (err, course) {
                    callback(err, course);
                }
            );
        });
    }).catch(function (err) {
        callback(err, newTeebox);
    });

    
}

module.exports.update = function (courseId, id, updateTeebox, callback) {
    var val = new Validator(updateTeebox);

    var db = mongoUtil.getDb();

    updateTeebox._id = new ObjectID(id);
    val.validateSchema().then(function() {
        db.collection(config.db.collections.courses).update(
        {
            "_id": new ObjectID(courseId),
            "teeboxes._id": updateTeebox._id,
        },
        {
            "$set": { "teeboxes.$": updateTeebox }
        },
        function (err, part) {
            db.collection(config.db.collections.tournaments).findOne(
                { "_id": new ObjectID(courseId) },
                function (err, course) {
                    callback(err, course);
                }
            );
        }); 
    }).catch(function (err) {
        callback(err, updateTeebox);
    });
}

module.exports.delete = function (id, callback) {
    updateTeebox._id = new ObjectID(id);
    db.collection(config.db.collections.courses).update(
        {
            "_id": new ObjectID(courseId),
            "teeboxes._id": updateTeebox._id,
        },
        {
            "$pull": { "teeboxes.$": updateTeebox }
        },
        function (err, part) {
            db.collection(config.db.collections.tournaments).findOne(
                { "_id": new ObjectID(courseId) },
                function (err, course) {
                    callback(err, course);
                }
            );
        }
    );
}