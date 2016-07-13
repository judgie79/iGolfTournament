var config = require("../config.js");

var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;

var mongoUtil = require('../db/mongoUtil');

var CrudRepository = require('./crudRepository.js');
var crudRepository = new CrudRepository(config.db.collections.users);
var bcrypt = require('bcryptjs');

//var userSchema = require('../schemas/user.js');

module.exports.findAll = function (callback) {
    crudRepository.findAll(callback);
}

module.exports.findById = function (id, includePW, callback) {
    crudRepository.findById(id, function(err, user) {
        
        if (err === null && user != null) {
            if (!includePW)
                delete user.password;
        }
        
        
        
        callback(err, user);
    });
}


module.exports.findByUsername = function (username, includePW, callback) {
    crudRepository.findOne({ "username": username }, function(err, user) {
        
        if (!includePW)
            delete user.password;
        
        callback(err, user);
    });
}

module.exports.findByEmail = function (email, callback) {
    crudRepository.findOne({ "email": email }, function(err, user) {
        
        delete user.password;
        
        callback(err, user);
    });
}

module.exports.create = function (newUser, callback) {
    
    //check if the username is unique
    module.exports.findByUsername(newUser.username, function(err, user) {
        if (err === null && user != null) {
            //username was found

            callback({message: "username was must be unique"}, null);

            return;
        }

        //check if the email is unique
        module.exports.findByEmail(newUser.email, function(err, user) {
            if (err === null && user != null) {
                //username was found

                callback({message: "email was must be unique"}, null);

                return;
            }


            bcrypt.genSalt(10, function(err, salt) {
                bcrypt.hash(newUser.password, salt, function(err, hash) {
                    // Store hash in your password DB.
                    newUser.password = hash;
                    delete newUser.passwordRepeat;
                    
                    crudRepository.create(newUser, callback);
                });
            });
        });
    });
}


module.exports.update = function (id, updateUser, callback) {
    
    bcrypt.genSalt(10, function(err, salt) {
        bcrypt.hash(updateUser.password, salt, function(err, hash) {
            // Store hash in your password DB.
            updateUser.password = hash;
            delete updateUser.passwordRepeat;
            
            crudRepository.update(id, updateUser, callback);
        });
    });

    
}

module.exports.delete = function (id, callback) {
    crudRepository.delete(id, callback);
}