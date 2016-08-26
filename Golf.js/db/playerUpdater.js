var Promise = require('promise');
var mongoUtil = require('../db/mongoUtil');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var config = require("../config.js");

var Updater = function () {

};


Updater.prototype.updateHomeClub = function (db, collection, player, update) {
    
    var me = this;
    return new Promise(function (resolve, reject) {
        var db = mongoUtil.getDb();

        db.collection(collection).findOne({ "_id": new ObjectID(player.homeClub._id) }, function (err, doc) {
            if (err) {
                reject(err);
            }

            delete player.homeClub._id;
            if (update)
                player.homeClub._id = new ObjectID(doc._id);
            else 
                player.homeClub._id = doc._id;
            
            player.homeClub.name = doc.name;
            delete player.homeClub.address

            resolve(player);
        });
    });
}

module.exports = Updater;