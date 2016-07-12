var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var TOURNAMENTS_COLLECTION = "tournaments";

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(TOURNAMENTS_COLLECTION);

//var tournamentSchema = require('../schemas/tournament.js');

module.exports.findAll = function (callBack) {
    crudRepository.findAll(callBack);
}

module.exports.findById = function (id, callBack) {
    crudRepository.findById(id, callBack);
}

module.exports.addPlayer = function (tournamentId, participant, callBack) {
    var db = mongoUtil.getDb();
    participant._id = new ObjectID(player._id);

    db.collection(TOURNAMENTS_COLLECTION).findOne({ _id: new ObjectID(tournamentId) }, function (err, doc) {

        doc.players = doc.players || [];
        doc.participants.push(participant);

        delete doc._id;

        db.collection(this.collection).updateOne({ _id: new ObjectID(tournamentId) }, updatedoc, function (err, doc) {
            callBack(err, doc);
        });
    });
}

module.exports.create = function (newTournament, callBack) {

    // var isValid = crudRepository.validate(newTournament, clubSchema);

    // if (isValid.length == 0)
    //     crudRepository.create(newTournament, callback);
    // else
    //     callback(isValid, null);

    var db = mongoUtil.getDb();

    db.collection("courses").findOne({ _id: new ObjectID(newTournament.course._id) }, function (err, course) {


        newTournament.course.name = course.name;
        newTournament.course.teeboxes = course.teeboxes;
        newTournament.course.teeboxes = course.teeboxes;

        db.collection("clubs").findOne({ _id: new ObjectID(newTournament.club._id) }, function (err, club) {

            newTournament.club.name = club.name
            newTournament.club.address = club.address;

            newTournament.course._id = new ObjectID(newTournament.course._id);
            newTournament.course.clubId = new ObjectID(newTournament.club._id);
            newTournament.club._id = new ObjectID(newTournament.club._id);

            crudRepository.create(newTournament, callBack);
        });
    });

    
}

module.exports.update = function (id, updateTournament, callBack) {
    // var isValid = crudRepository.validate(updateTournament, clubSchema);

    // if (isValid.length == 0)
    //     crudRepository.update(updateTournament, callback);
    // else
    //     callback(isValid, null);

    crudRepository.update(updateTournament, callback);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}