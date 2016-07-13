var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var PLAYERS_COLLECTION = "players";

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(PLAYERS_COLLECTION);

var playerSchema = require('../schemas/player.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newplayer, callback) {
    newplayer.hcp = Number(newplayer.hcp);


    var isValid = crudRepository.validate(newplayer, playerSchema);

    if (isValid.length == 0) {

        if (!newplayer.homeClub.name) {
            var db = mongoUtil.getDb();

            newplayer.homeClub._id;

            db.collection("clubs").findOne({ "_id": ObjectID(newplayer.homeClub._id) }, function (err, doc) {

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

module.exports.update = function (id, updatePlayer, callback) {
    var isValid = crudRepository.validate(updatePlayer, playerSchema);

    if (isValid.length == 0)
        crudRepository.update(id, updatePlayer, callback);
    else
        callback(isValid, null);
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}

module.exports.findMembersOfClub = function (clubId, callback) {
    var db = mongoUtil.getDb();
    db.collection(PLAYERS_COLLECTION).find({ "home.clubId": ObjectID(clubId) }).toArray(function (err, players) {

        callback(err, players);
    });
}