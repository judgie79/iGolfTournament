var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.clubs);

var Validator = require('./clubValidator');

module.exports.deleteParticipant = function (tournamentId, participantId, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        if (err) {
            callback(err, null);
        }

        var val = new Validator(doc);

        val.tournamentNotStarted().then(function () {
            db.collection(config.db.collections.tournaments).update({ "_id": new ObjectID(tournamentId) },
                { $pull: { "participants": { "_id": new ObjectID(participantId) } } }, false, callback);
        }).catch(function (err) {
            callback(err, updateTournament);
        });
    });
};

module.exports.updateParticipant = function (tournamentId, id, participant, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted().then(function () {
            delete participant._id;

            db.collection(config.db.collections.players).findOne(
                { "_id": new ObjectID(participant.player._id) },
                function (err, player) {
                    participant._id = new ObjectID(id);
                    participant.player = player;
                    participant.player._id = new ObjectID(participant.player._id);
                    participant.player.homeClub._id = new ObjectID(participant.player.homeClub._id);
                    participant.teeBoxId = new ObjectID(participant.teeBoxId);

                    db.collection(config.db.collections.tournaments).update(
                        {
                            _id: new ObjectID(tournamentId),
                            "participants._id": participant._id
                        },
                        {
                            $set: { "participants.$": participant }
                        },
                        function (err, part) {
                            db.collection(config.db.collections.tournaments).findOne(
                                { "_id": new ObjectID(tournamentId) },
                                function (err, tournament) {
                                    callback(err, tournament);
                                }
                            );
                        }
                    );
                });
        }).catch(function (err) {
            callback(err, null);
        });
    });
}

module.exports.registerParticipant = function (tournamentId, participant, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted().then(function () {
            db.collection(config.db.collections.tournaments).findOne(
                {
                    "_id": new ObjectID(tournamentId),
                    "participants.player._id": new ObjectID(participant.player._id),
                },
                function (err, tournament) {
                    if (tournament != null) {
                        callback({ message: "Player is already registered!" }, null);
                        return;
                    }

                    db.collection(config.db.collections.players).findOne(
                        { "_id": new ObjectID(participant.player._id) },
                        function (err, player) {
                            participant._id = new ObjectID();
                            participant.player = player;
                            participant.player._id = new ObjectID(participant.player._id);
                            participant.player.homeClub._id = new ObjectID(participant.player.homeClub._id);
                            participant.teeBoxId = new ObjectID(participant.teeBoxId);

                            db.collection(config.db.collections.tournaments).update(
                                {
                                    _id: new ObjectID(tournamentId)
                                },
                                {
                                    "$push": { "participants": participant }
                                },
                                function (err, part) {
                                    db.collection(config.db.collections.tournaments).findOne(
                                        { "_id": new ObjectID(tournamentId) },
                                        function (err, tournament) {
                                            callback(err, tournament);
                                        }
                                    );
                                }
                            );
                        });
                });
        }).catch(function (err) {
            callback(err, null);
        });
    });
}