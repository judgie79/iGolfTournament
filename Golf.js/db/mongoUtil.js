﻿var config = require("../config.js");

var MongoClient = require('mongodb').MongoClient
    , assert = require('assert');

var _db;

module.exports = {

    connectToServer: function (callback) {

        // Connection URL 
        var url = config.db.connectionString + config.db.name;
        // Use connect method to connect to the Server 
        MongoClient.connect(url, function (err, db) {
            assert.equal(null, err);
            console.log("Connected correctly to server");

            _db = db;
            return callback(err);
        });
    },

    getDb: function () {
        return _db;
    }
};