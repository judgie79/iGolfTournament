var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.scorecards);

var Validator = require('./scorecardValidator');
//var Updater = require('./scorecardUpdater');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, callback) {
    crudRepository.findById(id, callback);
}

module.exports.create = function (newscorecard, callback) {

    var val = new Validator(newscorecard);
    
    //var updater = new Updater();
    var db = mongoUtil.getDb();
    

    val.validateSchema().then(function(scorecard) {
        crudRepository.create(newscorecard, callback);
    }).catch(function(err) {
        callback(err, null);
    });
}

module.exports.update = function (id, updateScorecard, callback) {
    var val = new Validator(updateScorecard);
    
    //var updater = new Updater();
    var db = mongoUtil.getDb();

    val.validateSchema().then(function(scorecard) {
        crudRepository.update(id, updatePlayer, callback);
    }).catch(function(err) {
        callback(err, null);
    });
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}