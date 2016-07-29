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

    val.validateSchema().then(function() {
        return updater.updateTournament(newtournament, true);
    }).then(function (updatedTournament) {
        crudRepository.create(updatedTournament, callback);
    }).catch(function (err) {
        callback(err, newtournament);
    });
};

module.exports.update = function (id, updateTournament, callback) {

    var val = new Validator(updateTournament);

    var db = mongoUtil.getDb();

    val.validateSchema().then(function() {
        return val.tournamentNotStarted();
    }).then(function() {
        var updater = new Updater();
        updater.updateTournament(tournament, false).then(function (updatedTournament) {
            crudRepository.update(updatedTournament, callback);
        }).catch(function (err) {
            callback(err, tournament);
        });
    }).catch(function(err) {
        callback(err, updateTournament);
    });
};

module.exports.start = function (tournament) {
    var val = new Validator(tournament);

    val.validateSchema().then(function() {
        return val.tournamentNotStarted();
    }).then(function() {
        var updater = new Updater();
        updater.updateTournament(tournament, false).then(function (updatedTournament) {
            tournament.hasStarted = true;
            tournament.startDate = new Date();

            crudRepository.update(updatedTournament, callback);
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