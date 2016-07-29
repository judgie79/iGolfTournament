var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');



module.exports.clubReport = function (callback) {
    var reports = [];

    findClubsWithNoCourses().then(function(reportNoCourses) {
        
        [].push.apply(reports, reportNoCourses);
        
        return findClubsWithCoursesNoTeeboxes(reports);
    }).then(function(reportNoTeeboxes) {
        
        [].push.apply(reports, reportNoTeeboxes);

        callback(false, reports);
    }).catch(function(err) {
        callback(err, []);
    });
}


var Promise = require('promise');

function findClubsWithCoursesNoTeeboxes() {
    var me = this;
    return new Promise(function (resolve, reject) {

        var db = mongoUtil.getDb();

        db.collection(config.db.collections.courses).distinct('clubId', { "teeboxes": { "$lte": [] }},
            function (err, result) {
                db.collection(config.db.collections.clubs).find({ "_id": { "$in": result } }).toArray(function (err, clubs) {

                    if (err) {
                        reject(err);
                    }

                    var reports = (clubs.map(function (club) {
                        return {
                            reason: "Club with Course with no Teeboxes",
                            club: club
                        };
                    }));

                    resolve(reports);
                });
            });
    });
}

function findClubsWithNoCourses() {

    var me = this;
    return new Promise(function (resolve, reject) {

        var db = mongoUtil.getDb();

        db.collection(config.db.collections.courses).distinct('clubId',
            function (err, result) {
                db.collection(config.db.collections.clubs).find({ "_id": { "$nin": result } }).toArray(function (err, clubs) {

                    if (err) {
                        reject(err);
                    }

                    var reports = (clubs.map(function (club) {
                        return {
                            reason: "no Courses",
                            club: club
                        };
                    }));

                    resolve(reports);
                });
            });
    });
}