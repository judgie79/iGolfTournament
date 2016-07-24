var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.tournaments);

//var tournamentSchema = require('../schemas/tournament.js');

var Validator = function(tournament) {
    this.tournament = tournament;
};

Validator.prototype.tournamentStarted = function(callback) {
    if(this.tournament.hasStarted != null && this.tournament.hasStarted ) {
        callback(false, true);
    }
    else {
        callback({ reason : "Tournament has already started", message : "Tournament has already started"}, false);
    }
};

Validator.prototype.tournamentNotStarted = function(callback) {
    if(this.tournament.hasStarted == null || !this.tournament.hasStarted) {
        callback(false, true);
    }
    else {
        callback({ reason : "Tournament has not started", message : "Tournament has not started"}, false);
    }
};

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
};

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, function(err, tournament) {
        var db = mongoUtil.getDb();

    var updater = new Updater();
    updater.updateCourse(db, config.db.collections.courses, tournament, function(errCourse, tournamentWithCourse) {
        
            if (errCourse) {
                callback(errCourse, tournamentWithCourse);
            }
            
            updater.updateClub(db, config.db.collections.clubs, tournamentWithCourse, function(errClub, tournamentWithClub) {
                if (errClub) {
                    callback(errClub, tournamentWithClub);
                }
                
                updater.updatePlayers(db, config.db.collections.players, tournamentWithClub, function(errPlayers, tournamentWithPlayers) {
                    if (errPlayers) {
                        callback(errPlayers, tournamentWithPlayers);
                    }

                        callback(errPlayers, tournamentWithPlayers);
                }, false);
            
            }, false);
        }, false);
    });
};

module.exports.create = function (newtournament, callback) {

    // var isValid = crudRepository.validate(newTournament, clubSchema);

    // if (isValid.length == 0)
    //     crudRepository.create(newTournament, callback);
    // else
    //     callback(isValid, null);

    var db = mongoUtil.getDb();

    var updater = new Updater();
    updater.updateCourse(db, config.db.collections.courses, newtournament, function(errCourse, tournamentWithCourse) {
        
        if (errCourse) {
            callback(errCourse, tournamentWithCourse);
        }
        
        updater.updateClub(db, config.db.collections.clubs, tournamentWithCourse, function(errClub, tournamentWithClub) {
            if (errClub) {
                callback(errClub, tournamentWithClub);
            }
            
            updater.updatePlayers(db, config.db.collections.players, tournamentWithClub, function(errPlayers, tournamentWithPlayers) {
                if (errPlayers) {
                    callback(errPlayers, tournamentWithPlayers);
                }

                    crudRepository.create(tournamentWithClub, callback);
            }, true);
           
        }, true);
    }, true);
};

var Updater = function() {

};

Updater.prototype.updateCourse = function(db, collection, tournament, callback, update) {
    
    db.collection(collection).findOne({ _id: new ObjectID(tournament.course._id) }, function (err, course) {

        if (err) {
            callback(err, tournament);
        }

        tournament.course = course;
        if(update)
            tournament.course._id = new ObjectID(course._id);

        callback(false, tournament);
    });
};

Updater.prototype.updatePlayers = function(db, collection, tournament, callback, update) {
    

    var playIds = tournament.participants.map(function(participant) {
            return new ObjectID(participant.player._id);
    });

    var players = [];
    
    db.collection(collection).find({ "_id" : { $in: playIds } }, function (err, docs) {
        players = docs;

        tournament.participants = tournament.participants.map(function (participant) {
            
            if(update)
                participant._id = new ObjectID(participant._id);

            for (var i = 0; i < players.length; i++) {
                var player = players[i];
                if(update)
                    player._id = new ObjectID(player._id);

                if (player._id === participant.player._id) {
                    participant.player = player;
                    
                }
            }
            
            return participant;
        });

        callback(err, tournament);
        
    });
};

Updater.prototype.updateClub = function(db, collection, tournament, callback, update) {
    db.collection(collection).findOne({ _id: new ObjectID(tournament.club._id) }, function (err, club) {

        if (err) {
            callback(err, tournament);
        }

        tournament.club = club;

        
        if(update) {
            tournament.course.clubId = new ObjectID(club._id);
            tournament.club._id = new ObjectID(club._id);
        }

        callback(false, tournament);
    });
};

module.exports.update = function (id, updateTournament, callback) {
    
    var val = new Validator(updateTournament);

    var db = mongoUtil.getDb();

    val.tournamentNotStarted(function(err, notStarted) {
        if(notStarted) {
            var updater = new Updater();
            updater.updateCourse(db, config.db.collections.courses, updateTournament, function(errCourse, tournamentWithCourse) {
                
                if (errCourse) {
                    callback(errCourse, tournamentWithCourse);
                }
                
                updater.updateClub(db, config.db.collections.clubs, tournamentWithCourse, function(errClub, tournamentWithClub) {
                    if (errClub) {
                        callback(errClub, tournamentWithClub);
                    }

                    updater.updatePlayers(db, config.db.collections.players, tournamentWithClub, function(errPlayers, tournamentWithPlayers) {
                        if (errPlayers) {
                            callback(errPlayers, tournamentWithPlayers);
                        }

                        crudRepository.update(id, tournamentWithPlayers, callback);
                    }, true);

                    
                }, true);
            }, true);
        }
        else {
            callback(err, updateTournament);
        }
    });
};

module.exports.start = function(tournament) {
    var val = new Validator(tournament);

    
    val.tournamentNotStarted(function(err, notStarted) {
        if(notStarted) {
            var updater = new Updater();
            updater.updateCourse(db, config.db.collections.courses, tournament, function(errCourse, tournamentWithCourse) {
                
                if (errCourse) {
                    callback(errCourse, tournamentWithCourse);
                }
                
                updater.updateClub(db, config.db.collections.clubs, tournamentWithCourse, function(errClub, tournamentWithClub) {
                    if (errClub) {
                        callback(errClub, tournamentWithClub);
                    }
                    
                    updater.updatePlayers(db, config.db.collections.players, tournamentWithClub, function(errPlayers, tournamentWithPlayers) {
                        if (errPlayers) {
                            callback(errPlayers, tournamentWithPlayers);
                        }
                        tournamentWithClub.hasStarted = true;
                        crudRepository.update(tournamentWithPlayers, callback);
                    }, true);
                }, true);
            }, true);
        }
        else {
            callback(err, tournament);
        }
    });
};

module.exports.delete = function (id, callback) {
    
    
    crudRepository.findById(id, function(err, doc){
        var val = new Validator(doc);

        val.tournamentNotStarted(function(err, notStarted) {
            if(notStarted) {
                crudRepository.delete(id, callback);
            }
            else {
                callback(err, doc);
            }
        });
    });
};

module.exports.deleteParticipant = function (tournamentId, participantId, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function(err, doc){
        var val = new Validator(doc);

        val.tournamentNotStarted(function(err, notStarted) {
            if(notStarted) {
               db.collection(config.db.collections.tournaments).update({ "_id": new ObjectID(tournamentId) },
                            { $pull : { "participants" : {"_id":new ObjectID(participantId) } } }, false, callback);
            }
            else {
                callback(err, updateTournament);
            }
        });
    });
};

module.exports.updateParticipant = function (tournamentId, id, participant, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function(err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted(function(err, notStarted) {
            if(notStarted) {
                delete participant._id;

                db.collection(config.db.collections.players).findOne(
                    {"_id": new ObjectID(participant.player._id)}, 
                    function(err, player) {
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
                                $set : { "participants.$" : participant }
                            },
                            function(err, part) {
                                db.collection(config.db.collections.tournaments).findOne(
                                    { "_id": new ObjectID(tournamentId) }, 
                                    function (err, tournament) {
                                        callback(err, tournament);
                                    }
                                );
                            }
                        );
                    });

                
            }
            else {
                callback(err, participant);
            }
        });
    });
}

module.exports.registerParticipant = function (tournamentId, participant, callback) {
    var db = mongoUtil.getDb();

    crudRepository.findById(tournamentId, function(err, doc) {
        var val = new Validator(doc);
        val.tournamentNotStarted(function(err, notStarted) {
            if(notStarted) {
                
                //db.getCollection('tournaments').find({"_id": ObjectId("5794e37f4b31cf3598e65805"), "participants.player._id" : ObjectId("577e74aeb77d5fe65d2b39f5")})
                db.collection(config.db.collections.tournaments).findOne(
                {
                    "_id": new ObjectID(tournamentId),
                    "participants.player._id": new ObjectID(participant.player._id), 
                },
                function(err, tournament) {
                    if (tournament != null) {
                        callback({message: "Player is already registered!"}, null);
                        return;
                    }
                    
                    db.collection(config.db.collections.players).findOne(
                    {"_id": new ObjectID(participant.player._id)}, 
                    function(err, player) {
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
                            function(err, part) {
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
            }
            else {
                callback(err, participant);
            }
        });
    });
}