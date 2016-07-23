var express = require('express');
var mongodb = require("mongodb");
var ObjectID = mongodb.ObjectID;
var mongoUtil = require('../db/mongoUtil');

var JaySchema = require('jayschema');
var js = new JaySchema();

var CrudRepository = function (collection) {
    this.collection = collection;
};

CrudRepository.prototype.findAll = function (callback) {
    var db = mongoUtil.getDb();
    db.collection(this.collection).find({}).toArray(function (err, docs) {

        callback(err, docs);
    });
};

CrudRepository.prototype.findById = function (id, callback) {
    var db = mongoUtil.getDb();
    db.collection(this.collection).findOne({ "_id": new ObjectID(id) }, function (err, doc) {

        callback(err, doc);
    });
}

CrudRepository.prototype.findOne = function (filter, callback) {
    var db = mongoUtil.getDb();
    db.collection(this.collection).findOne(filter, function (err, doc) {

        callback(err, doc);
    });
}

CrudRepository.prototype.create = function (newdoc, callback) {
    var db = mongoUtil.getDb();
    newdoc._id = new ObjectID();
    //newdoc.createDate = new Date();
    db.collection(this.collection).insertOne(newdoc, function (err, doc) {
        callback(err, doc);
    });
}

CrudRepository.prototype.update = function (id, updatedoc, callback) {
    var db = mongoUtil.getDb();

    delete updatedoc._id;

    db.collection(this.collection).updateOne({ _id:  new ObjectID(id)}, updatedoc, function (err, doc) {
        callback(err, doc);
    });
}

CrudRepository.prototype.delete = function (id, callback) {
    var db = mongoUtil.getDb();

    db.collection(this.collection).deleteOne({ "_id": new ObjectID(id) }, function (err, result) {
        callback(err, result);
    });
}

CrudRepository.prototype.validate = function (instance, schema) {

    return js.validate(instance, schema);
}

module.exports = CrudRepository;