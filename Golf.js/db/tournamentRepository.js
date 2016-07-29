var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.tournaments);

var Updater = require('./tournamentUpdater');
var Validator = require('./tournamentValidator');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
};

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, function (err, tournament) {
        var db = mongoUtil.getDb();

        var updater = new Updater();
        updater.updateTournament(tournament, false).then(function (updatedTournament) {
            callback(false, updatedTournament);
        }).catch(function (err) {
            callback(err, tournament);
        });
    });
};

module.exports.create = function (newtournament, callback) {
    var val = new Validator(newtournament);

    var db = mongoUtil.getDb();
    var updater = new Updater();
    
    updater.updateTournament(newtournament, false).then(function (updatedTournament) {
        val.validateSchema().then(function () {
            return val.tournamentNotStarted();
        }).then(function () {
            crudRepository.create(updatedTournament, callback);
        }).catch(function (err) {
            callback(err, newtournament);
        });
    }).catch(function (err) {
        callback(err, newtournament);
    });
};

module.exports.update = function (id, updateTournament, callback) {

    var val = new Validator(updateTournament);

    var db = mongoUtil.getDb();

    var updater = new Updater();
    updater.updateTournament(updateTournament, false).then(function (updatedTournament) {
        val.validateSchema().then(function () {
            return val.tournamentNotStarted();
        }).then(function () {
            crudRepository.update(id, updatedTournament, callback);
        }).catch(function (err) {
            callback(err, updateTournament);
        });
    }).catch(function (err) {
        callback(err, updateTournament);
    });


};

module.exports.start = function (id, tournament, callback) {
    var val = new Validator(tournament);

    val.validateSchema().then(function () {
        return val.tournamentNotStarted();
    }).then(function () {
        var updater = new Updater();
        updater.updateTournament(tournament, false).then(function (updatedTournament) {
            tournament.hasStarted = true;
            tournament.startDate = new Date();

            crudRepository.update(id, updatedTournament, callback);
        }).catch(function (err) {
            callback(err, tournament);
        });
    }).catch(function (err) {
        callback(err, tournament);
    });
};

module.exports.delete = function (id, callback) {


    crudRepository.findById(id, function (err, doc) {
        var val = new Validator(doc);

        val.tournamentNotStarted().then(function () {
            crudRepository.delete(id, callback);
        }).catch(function (err) {

        });
    });
};

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