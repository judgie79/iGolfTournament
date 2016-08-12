var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectId;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.tournaments);

var Validator = require('./tournamentValidator');

module.exports.deleteParticipant = function (tournamentId, participantId, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        if (err) {
            callback(err, null);
        }

        var val = new Validator(doc);

        val.tournamentNotStarted().then(function () {
            db.collection(config.db.collections.tournaments).update({ "_id": new ObjectId(tournamentId) },
                { $pull: { "participants": { "_id": new ObjectId(participantId) } } }, false, callback);
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
                { "_id": new ObjectId(participant.player._id) },
                function (err, player) {
                    participant._id = new ObjectId(id);
                    participant.player = player;
                    participant.player._id = new ObjectId(participant.player._id);
                    participant.player.homeClub._id = new ObjectId(participant.player.homeClub._id);
                    participant.teeBoxId = new ObjectId(participant.teeBoxId);

                    db.collection(config.db.collections.tournaments).update(
                        {
                            _id: new ObjectId(tournamentId),
                            "participants._id": participant._id
                        },
                        {
                            $set: { "participants.$": participant }
                        },
                        function (err, part) {
                            db.collection(config.db.collections.tournaments).findOne(
                                { "_id": new ObjectId(tournamentId) },
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
                    "_id": new ObjectId(tournamentId),
                    "participants.player._id": new ObjectId(participant.player._id),
                },
                function (err, tournament) {
                    if (tournament != null) {
                        callback({ message: "Player is already registered!" }, null);
                        return;
                    }

                    db.collection(config.db.collections.players).findOne(
                        { "_id": new ObjectId(participant.player._id) },
                        function (err, player) {
                            participant._id = new ObjectId();
                            participant.player = player;
                            participant.player._id = new ObjectId(participant.player._id);
                            participant.player.homeClub._id = new ObjectId(participant.player.homeClub._id);
                            participant.teeBoxId = new ObjectId(participant.teeBoxId);

                            db.collection(config.db.collections.tournaments).update(
                                {
                                    _id: new ObjectId(tournamentId)
                                },
                                {
                                    "$push": { "participants": participant }
                                },
                                function (err, part) {
                                    db.collection(config.db.collections.tournaments).findOne(
                                        { "_id": new ObjectId(tournamentId) },
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