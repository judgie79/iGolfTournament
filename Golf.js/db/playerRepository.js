var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.players);

var Validator = require('./playerValidator');
var Updater = require('./playerUpdater');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newplayer, callback) {

    var val = new Validator(newplayer);
    
    var updater = new Updater();
    var db = mongoUtil.getDb();
    

    val.validateSchema(function() {
        return validateHcp();
    }).then(function(player) {
        return updater.updateHomeClub(db, config.db.collections.clubs, newplayer, true);

    }).then(function(player) {
        crudRepository.create(newplayer, callback);
    }).catch(function(err) {
        callback(err, null);
    });
}

module.exports.update = function (id, updatePlayer, callback) {
    var val = new Validator(newplayer);
    
    var updater = new Updater();
    var db = mongoUtil.getDb();

    val.validateSchema(function() {
        return validateHcp();
    }).then(function(player) {
        return updater.updateHomeClub(db, config.db.collections.clubs, updatePlayer, true);

    }).then(function(player) {
        crudRepository.update(id, newplayer, callback);
    }).catch(function(err) {
        callback(err, null);
    });
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