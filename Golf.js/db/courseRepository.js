﻿var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courses);

var courseSchema = require('../schemas/course.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findCoursesOfClub = function (clubId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "clubId": new ObjectID(clubId) }).toArray(function (err, docs) {

        callback(err, docs);
    });
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newcourse, callback) {
    var isValid = crudRepository.validate(newcourse, courseSchema);

    newcourse.clubId = new ObjectID(newcourse.clubId);

    if (newcourse.teeBoxes && newcourse.teeboxes.length > 0)
    {
        for (var i = 0; i < newcourse.teeboxes.length; i++)
        {
            var teeBox = newcourse.teeboxes[i];
            if (teeBox._id === "")
            {
                teeBox._id = new ObjectID();
            } else {
                teeBox._id = new ObjectId(hole._id);
            }
            
            if (teeBox.holes && teeBox.holes.length > 0)
            {
                    for (var h = 0; h < teeBox.holes.length; h++)
                    {
                        var hole = teeBox.holes[h];
                        
                        if (hole._id === "")
                        {
                            hole._id = new ObjectID();
                        } else {
                            hole._id = new ObjectId(hole._id);
                        }
                    }
            }
        }
    }

    if (isValid.length == 0)
        crudRepository.create(newcourse, callback);
    else
        callback(isValid, null);
}

module.exports.update = function (id, updateCourse, callback) {
    var isValid = crudRepository.validate(updateCourse, courseSchema);

    updateCourse.clubId = new ObjectID(updateCourse.clubId);
    for (var i = 0; i < updateCourse.teeboxes.length; i++)
    {
        var teeBox = updateCourse.teeboxes[i];

        if (teeBox._id == null || teeBox._id === "")
        {
            teeBox._id = new ObjectID();
        } else {
            teeBox._id = new ObjectID(teeBox._id);
        }
        
        if (teeBox.holes && teeBox.holes.length > 0)
        {
                for (var h = 0; h < teeBox.holes.length; h++)
                {
                    var hole = teeBox.holes[h];
                    
                    if (hole.holeId === "")
                    {
                        hole.holeId = new ObjectID();
                    } else {
                        hole.holeId = new ObjectID(hole.holeId);
                    }
                }
        }
    }

    if (isValid.length == 0)
        crudRepository.update(id, updateCourse, callback);
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}