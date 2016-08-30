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

        me.updateCourse(db, config.db.collections.courses, tournament, update)
            .then(function (updatedTournament) {
                return me.updateClub(db, config.db.collections.clubs, updatedTournament, update)
            })
            .then(function (updatedTournament) {
            
                if(tournament.type === "single") {
                    return me.updatePlayers(db, config.db.collections.players, updatedTournament, update);
                }
                else if(tournament.type === "team") {
                    return me.updateTeams(db, config.db.collections.players, updatedTournament, update);
                }
            
            })
            .then(function (updatedTournament) {
                return me.updateCourseHoles(db, config.db.collections.courseHoles, updatedTournament, update)
            })
            .then(function (updatedTournament) {
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

            resolve(tournament);
        });
    });
};

Updater.prototype.updateCourseHoles = function (db, collection, tournament, update) {

    return new Promise(function (resolve, reject) {
        var count = 0;
        for (var i = 0; i < tournament.course.teeboxes.length; i++) {
            var teebox = tournament.course.teeboxes[i];
            db.collection(collection).find({ "teeboxId": teebox._id.toHexString() }).toArray(function (err, courseHoles) {

                if (err) {
                    reject(err);
                }
                teebox.holes = {
                    front: courseHoles.filter(function (ch) {
                        return ch.frontOrBack.toLowerCase() === "front";
                    }),
                    back: courseHoles.filter(function (ch) {
                        return ch.frontOrBack.toLowerCase() === "back";
                    })
                };
                count++;

                if (count == tournament.course.teeboxes.length) {
                    resolve(tournament);
                }
            });
        }

        
    });
};

Updater.prototype.updateTeams = function (db, collection, tournament, update) {

    return new Promise(function (resolve, reject) {
        if (tournament.teams == null)
        {
             tournament.teams = [];
        }

        if (tournament.participants == null)
        {
             tournament.participants = [];
        }

        resolve(tournament);
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

                for (var i = 0; i < players.length; i++) {
                    var player = players[i];

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

            resolve(tournament);
        });
    });
};

module.exports = Updater;