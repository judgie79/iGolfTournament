﻿var express = require("express");
var router = express.Router();

var clubRepository = require('../db/clubRepository.js');
var courseRepository = require('../db/courseRepository.js');
var playerRepository = require('../db/playerRepository.js');
var reportRepository = require('../db/reportRepository.js');

var error = new require('./error')();

router.get("/report", function (req, res) {
    reportRepository.clubReport(function(err, reports) {
        if (err) {
            error.handleError(res, err.message, "Failed clubreport");
        } else {
            res.status(200).json(reports);
        }
    });
});

/*  "/"
 *    GET: finds all clubs
 */
router.get("/", function (req, res) {

    clubRepository.findAll(function (err, clubs) {

        if (err) {
            error.handleError(res, err.message, "Failed to get clubs.");
        } else {
            res.status(200).json(clubs);
        }
    });
    
});

/*  "/"
 *    POST: create new club
 */
router.post("/", function (req, res) {

    var newclub = req.body;

    clubRepository.create(newclub, function (err, club) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new club.");
        } else {
            res.status(201).json(club.ops[0]);
        }
    });
});

/*  "/:id"
 *    GET: find club by id
 *    PUT: update club by id
 *    DELETE: deletes club by id
 */

router.get("/:id", function (req, res) {

    clubRepository.findById(req.params.id, function (err, club) {

        if (err) {
            error.handleError(res, err.message, "Failed to get club");
        } else {
            res.status(200).json(club);
        }
    });
});

router.put("/:id", function (req, res) {

    var updateClub = req.body;

    clubRepository.update(req.params.id, updateClub, function (err, club) {
        if (err) {
            error.handleError(res, err.message, "Failed to update club");
        } else {
            res.status(204).json(club);
        }
    });
});

router.delete("/:id", function (req, res) {
    clubRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete club");
        } else {
            res.status(204).end();
        }
    });
});


router.get("/:id/courses", function (req, res) {

    courseRepository.findCoursesOfClub(req.params.id, function (err, courses) {

        if (err) {
            error.handleError(res, err.message, "Failed to get courses");
        } else {
            res.status(200).json(courses);
        }
    });
});

router.get("/:id/players", function (req, res) {

    playerRepository.findMembersOfClub(req.params.id, function (err, club) {

        if (err) {
            error.handleError(res, err.message, "Failed to get courses");
        } else {
            res.status(200).json(club);
        }
    });
});



module.exports = router;