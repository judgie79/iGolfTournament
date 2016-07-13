var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.tournaments);

//var tournamentSchema = require('../schemas/tournament.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newtournament, callback) {

    // var isValid = crudRepository.validate(newTournament, clubSchema);

    // if (isValid.length == 0)
    //     crudRepository.create(newTournament, callback);
    // else
    //     callback(isValid, null);

    var db = mongoUtil.getDb();

    db.collection(config.db.collections.courses).findOne({ _id: new ObjectID(newTournament.course._id) }, function (err, course) {


        newTournament.course.name = course.name;
        newTournament.course.teeboxes = course.teeboxes;

        db.collection(config.db.collections.clubs).findOne({ _id: new ObjectID(newTournament.club._id) }, function (err, club) {

            newTournament.club.name = club.name
            newTournament.club.address = club.address;

            newTournament.course._id = new ObjectID(newTournament.course._id);
            newTournament.course.clubId = new ObjectID(newTournament.club._id);
            newTournament.club._id = new ObjectID(newTournament.club._id);

            crudRepository.create(newTournament, callBack);
        });
    });
}

module.exports.update = function (id, updateTournament, callback) {
    crudRepository.update(id, updateTournament, callback);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}

module.exports.deleteParticipant = function (participantId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.tournaments).findOne({ "participants._id": new ObjectID(participantId) }, function (err, tournament) {

        if (tournament != null) {
            var foundIndex = -1;
            for(var i = tournament.participants - 1; i >= 0; i--) {
                var participant = tournament.participants[i];
                if (participant._id === participantId) {
                    foundIndex = i;
                }
            }

            if (foundIndex > -1) {
                delete tournament.participants[foundIndex];
            }
           

        } else {
            callback({message: "Tournament not found"}, null);
        }
    });
}


module.exports.registerParticipant = function (tournamentId, participant, callback) {
    var db = mongoUtil.getDb();

    db.collection(config.db.collections.tournaments).findOne({"player._id": new ObjectID(participant.player._id)}, function(err, tournament) {
        if (tournament != null) {
            callback({message: "Player is already registered!"}, null);
            return;
        }

        participant.player._id = new ObjectID(participant.player._id);
        participant.player.homeClub._id = new ObjectID(participant.player.homeClub._id);

        db.collection(config.db.collections.tournaments).findOne({ "_id": new ObjectID(tournamentId) }, function (err, tournament) {

            if (tournament != null) {

                participant._id = new ObjectID();
                tournament.participants.push(participant);
                
                delete tournament._id;

                db.collection(config.db.collections.tournaments).updateOne({ "_id": new ObjectID(tournamentId) }, tournament, function (err, doc) {
                    callback(err, tournament);
                });

            } else {
                callback({message: "Tournament not found"}, null);
            }
        });
    });
}