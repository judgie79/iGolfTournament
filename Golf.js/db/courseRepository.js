var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;


var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.courses);

var courseSchema = require('../schemas/course.js');

module.exports.findAll = function (callBack) {
    crudRepository.findAll(callBack);
}

module.exports.findCoursesOfClub = function (courseId, callBack) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.courses).find({ "clubId": ObjectID(courseId) }).toArray(function (err, docs) {

        callBack(err, docs);
    });
}

module.exports.findById = function (id, callBack) {
    crudRepository.findById(id, callBack);
}

module.exports.create = function (newcourse, callBack) {
    var isValid = crudRepository.validate(newcourse, courseSchema);

    if (isValid.length == 0)
        crudRepository.create(newplayer, callback);
    else
        callback(isValid, null);
}

module.exports.update = function (id, updateCourse, callBack) {
    var isValid = crudRepository.validate(updateCourse, courseSchema);

    if (isValid.length == 0)
        crudRepository.update(id, updateCourse, callBack);
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}