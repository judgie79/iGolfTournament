var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.players);

var playerSchema = require('../schemas/player.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newplayer, callback) {

    if (newplayer.membership) {
        module.exports.getHcp(newplayer, function(hcp) {
            if (hcp) {
               newplayer.hcp = hcp;
               newplayer.isOfficialHcp = true;
            } else {
                newplayer.isOfficialHcp = false;
                newplayer.hcp = Number(newplayer.hcp);
            }

        	createPlayer(newplayer, callback);
        });
    }
    else {
        newplayer.hcp = Number(newplayer.hcp);
        newplayer.isOfficialHcp = false;

        createPlayer(newplayer, callback);
    }
}

var createPlayer = function(newplayer, callback) {
    var isValid = crudRepository.validate(newplayer, playerSchema);

    if (isValid.length == 0) {

        if (!newplayer.homeClub.name) {
            var db = mongoUtil.getDb();

            newplayer.homeClub._id;

            db.collection(config.db.collections.clubs).findOne({ "_id": ObjectID(newplayer.homeClub._id) }, function (err, doc) {

                delete newplayer.homeClub._id;
                newplayer.homeClub._id = ObjectID(doc._id);
                newplayer.homeClub.name = doc.name;
                delete newplayer.homeClub.address

                crudRepository.create(newplayer, callback);
            });
        } else {
            crudRepository.create(newplayer, callback);
        }
    }
    else
        callback(isValid, null);
}

var setMembership = function (updatePlayer, callback) {
    if (updatePlayer.membership) {
           module.exports.getHcp(updatePlayer, function(hcp) {
                if (hcp) {
                updatePlayer.hcp = hcp;
                updatePlayer.isOfficialHcp = true;
                } else {
                    updatePlayer.isOfficialHcp = false;
                    updatePlayer.hcp = Number(updatePlayer.hcp);
                }
                callback(updatePlayer);
            });
        }
        else {
            updatePlayer.hcp = Number(updatePlayer.hcp);
            updatePlayer.isOfficialHcp = false;
            callback(updatePlayer);
        }
};

var setHomeClub = function (updatePlayer, callback) {
    if (!updatePlayer.homeClub.name) {
            var db = mongoUtil.getDb();

            updatePlayer.homeClub._id;

            db.collection(config.db.collections.clubs).findOne({ "_id": ObjectID(updatePlayer.homeClub._id) }, function (err, doc) {

                delete updatePlayer.homeClub._id;
                updatePlayer.homeClub._id = ObjectID(doc._id);
                updatePlayer.homeClub.name = doc.name;
                delete updatePlayer.homeClub.address

                callback(updatePlayer);
            });
        } else {
            callback(updatePlayer);
        }
};

module.exports.update = function (id, updatePlayer, callback) {
    var isValid = crudRepository.validate(updatePlayer, playerSchema);

    if (isValid.length == 0) {

        setMembership(updatePlayer, function(playerWithMembership) {
            setHomeClub(playerWithMembership, function(playerWithClub) {
                crudRepository.update(id, playerWithClub, callback);
            });
        });
        
    }
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}

module.exports.findMembersOfClub = function (clubId, callback) {
    var db = mongoUtil.getDb();
    db.collection(config.db.collections.players).find({ "home.clubId": ObjectID(clubId) }).toArray(function (err, players) {

        callback(err, players);
    });
}

module.exports.getHcp = function (player, callback) {

    if(player.membership.clubNr === "" || player.membership.nr === "" || player.membership.serviceNr === "")
    {
        callback(false);
    }
    
    var hcpServer = "http://www.golf.de/publish/turnierkalender/handicap-abfrage/abfrage";
    var request = require('request');

    // Set the headers
    var headers = {
        'User-Agent': 'Super Agent/0.0.1',
        'Content-Type': 'application/x-www-form-urlencoded'
    }

    // Configure the request
    var options = {
        url: hcpServer,
        method: 'GET',
        headers: headers,
        qs: { 'clubNr': player.membership.clubNr, 'ausweisNr': player.membership.nr, 'serviceNr': player.membership.serviceNr }
    }

    // Start the request
    request(options, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            // Print out the response body
            console.log(body)

            var cheerio = require('cheerio');
            var $ = cheerio.load(body);

            var hcp = Number($('.hcp .hcp_wrapper > .hcp_text_container').text().trim().replace(',', '.'));
            callback(hcp); 
            
        }
    });
}