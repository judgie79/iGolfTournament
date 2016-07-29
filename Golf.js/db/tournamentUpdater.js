var Promise = require('promise');
var mongoUtil = require('../db/mongoUtil');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var config = require("../config.js");

var Updater = function () {

};

Updater.prototype.updateTournament = function (tournament, update) {
    var me = this;

    return new Promise(function(resolve, reject) {
        var db = mongoUtil.getDb();

        me.updateCourse(db, config.db.collections.courses, tournament, update).then(function (updatedTournament) {
            return me.updateClub(db, config.db.collections.clubs, updatedTournament, update)
        }).then(function (updatedTournament) {
            return me.updatePlayers(db, config.db.collections.players, updatedTournament, update);
        }).then(function (updatedTournament) {
            resolve(updatedTournament);
        }).catch(function (err) {
            reject(err);
        });
    });

    
};

Updater.prototype.updateCourse = function (db, collection, tournament, update) {

    return new Promise(function (resolve, reject) {
        db.collection(collection).findOne({ _id: new ObjectID(tournament.course._id) }, function (err, course) {

            if (err) {
                reject(err);
            }

            tournament.course = course;
            if (update)
                tournament.course._id = new ObjectID(course._id);
            else if (tournament.course._id) {

            }
            resolve(tournament);
        });
    });
};

Updater.prototype.updatePlayers = function (db, collection, tournament, update) {

    return new Promise(function (resolve, reject) {
        if (tournament.participants == null || tournament.participants.length == 0)
        {
             tournament.participants = [];
             resolve(tournament);
        }


        var playIds = tournament.participants.map(function (participant) {
            return new ObjectID(participant.player._id);
        });

        var players = [];

        db.collection(collection).find({ "_id": { $in: playIds } }, function (err, docs) {
            players = docs;

            if (err) {
                resolve(tournament);
            }

            tournament.participants = tournament.participants.map(function (participant) {

                if (update)
                    participant._id = new ObjectID(participant._id);

                for (var i = 0; i < players.length; i++) {
                    var player = players[i];
                    if (update)
                        player._id = new ObjectID(player._id);

                    if (player._id === participant.player._id) {
                        participant.player = player;

                    }
                }

                return participant;
            });

            resolve(tournament);

        });
    });
};

Updater.prototype.updateClub = function (db, collection, tournament, update) {
    
    return new Promise(function (resolve, reject) {
        db.collection(collection).findOne({ _id: new ObjectID(tournament.club._id) }, function (err, club) {

            if (err) {
                reject(err);
            }

            tournament.club = club;


            if (update) {
                tournament.course.clubId = new ObjectID(club._id);
                tournament.club._id = new ObjectID(club._id);
            }

            resolve(tournament);
        });
    });
};

module.exports = Updater;