var config = require("../config.js");


var express = require('express');
var mongodb = require("mongodb");
var ObjectId = mongodb.ObjectId;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.tournaments);

var Validator = require('./tournamentValidator');

module.exports.deleteTeam = function (tournamentId, teamId, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        if (err) {
            callback(err, null);
        }

        var val = new Validator(doc);

        val.tournamentNotStarted().then(function () {
            db.collection(config.db.collections.tournaments).update({ "_id": new ObjectId(tournamentId) },
                { $pull: { "teams": { "_id": new ObjectId(teamId) } } }, false, callback);
        }).catch(function (err) {
            callback(err, updateTournament);
        });
    });
};

module.exports.updateTeam = function (tournamentId, id, team, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted().then(function () {
            delete team._id;

            team._id = new ObjectId(id);

            

            db.collection(config.db.collections.tournaments).update(
                {
                    _id: new ObjectId(tournamentId),
                    "teams._id": team._id
                },
                {
                    $set: { 
                        "teams.$.name": team.name,
                        "teams.$.teeTime": team.teeTime,
                        "teams.$.teeBoxId": team.teeBoxId
                     }
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
        }).catch(function (err) {
            callback(err, null);
        });
    });
}

module.exports.registerTeam = function (tournamentId, team, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted().then(function () {

            team._id = new ObjectId();
            team.teeTime = doc.date;
            team.members = team.members || [];
            db.collection(config.db.collections.tournaments).update(
                {
                    _id: new ObjectId(tournamentId)
                },
                {
                    "$push": { "teams": team }
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
}

//members

function getTeamHcp(members, excludeId, addValue) {

    var hcp = 0;

    if (members.length == 1 && !excludeId) {
        hcp = members[0].player.hcp;
    } else if (members.length > 1) {
        hcp = members.reduce(function (previousValue, currentValue) {

            if (excludeId) {
                if (currentValue._id === excludeId) {
                    return previousValue.player.hcp;
                }
            }

            if (isNaN(previousValue)) {
                return currentValue.player.hcp + previousValue.player.hcp;
            } else {
                return currentValue.player.hcp + previousValue;
            }
        });
    }


    if (addValue) {
        hcp = hcp + addValue;
    }

    if (isNaN(hcp))
        hcp = 0;

    if (hcp != 0) {

        var length = members.length;

        if (excludeId)
            length = length - 1;

        if (addValue)
            length = length + 1;

        hcp = hcp / length;
    }

    return hcp;
}

module.exports.deleteTeamMember = function (tournamentId, teamId, participantId, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        if (err) {
            callback(err, null);
        }

        var val = new Validator(doc);

        val.tournamentNotStarted().then(function () {

            var team = doc.teams.find(function (t) {
                return t._id == teamId;
            });

            var hcp = getTeamHcp(team.members, participantId);

            db.collection(config.db.collections.tournaments).update(

                {
                    "_id": new ObjectId(tournamentId),
                    "teams._id": new ObjectId(teamId)
                },
                {
                    "$pull": { "teams.$.members": { "_id": participantId } }
                },
                false, function (err, tourn) {
                    if (err) {
                        callback(err, null);
                    }

                    db.collection(config.db.collections.tournaments).update({
                        "_id": new ObjectId(tournamentId),
                        "teams._id": new ObjectId(teamId)
                    },
                        { "$set": { "teams.$.hcp": hcp } },
                        false, function (err, tourn) {
                            callback(err, null);
                        });
                });
        }).catch(function (err) {
            callback(err, updateTournament);
        });
    });
};

module.exports.updateTeamMember = function (tournamentId, id, team, callback) {
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
                            "teams._id": new ObjectId(teamId),
                            "teams.members._id": participant._id
                        },
                        {
                            $set: { "teams.$.members.$": participant }
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

module.exports.registerTeamMember = function (tournamentId, teamId, participant, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function (err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted().then(function () {


            db.collection(config.db.collections.tournaments).findOne(
                {
                    "_id": new ObjectId(tournamentId),
                    "teams._id": new ObjectId(teamId),
                    "teams.members._id": new ObjectId(participant._id),
                },
                function (err, tournament) {
                    if (tournament != null) {
                        callback({ message: "Participant is already a team member!" }, null);
                        return;
                    }

                    tournament = doc;

                    var team = tournament.teams.find(function (t) {
                        return t._id == teamId;
                    });

                    var fullPart = tournament.participants.find(function (part) {
                        return part._id.toHexString() === new ObjectId(participant._id).toHexString();
                    });

                    var hcp = getTeamHcp(team.members, false, fullPart.player.hcp);

                    fullPart.teeTime = team.teeTime;
                    //fullPart.player.hcp = hcp;
                    db.collection(config.db.collections.tournaments).update(
                        {
                            "_id": new ObjectId(tournamentId),
                            "teams._id": new ObjectId(teamId)
                        },
                        {
                            "$push": { "teams.$.members": fullPart }
                        },
                        function (err, part) {

                            db.collection(config.db.collections.tournaments).update(
                                {
                                    "_id": new ObjectId(tournamentId),
                                    "teams._id": new ObjectId(teamId)
                                },
                                { "$set": { "teams.$.hcp": hcp }},
                                false, function (err, tourn) {
                                    db.collection(config.db.collections.tournaments).findOne(
                                        { "_id": new ObjectId(tournamentId) },
                                        function (err, tournament) {
                                            callback(err, tournament);
                                        }
                                    );
                                });


                        }
                    );
                });
        }).catch(function (err) {
            callback(err, null);
        });
    });
}