var express = require("express");
var router = express.Router();

var scorecardRepository = require('../db/scorecardRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all scorecards
 *    POST: creates a new scorecard
 */
router.get("/:tournamentId/scorecards/", function (req, res) {

    scorecardRepository.findAll(function (err, scorecards) {

        if (err) {
            error.handleError(res, err.message, "Failed to get scorecards.");
        } else {
            res.status(200).json(scorecards);
        }
    });
    
});

router.post("/:tournamentId/scorecards/", function (req, res) {

    var newscorecard = req.body;
    
    scorecardRepository.create(newscorecard, function (err, scorecard) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new scorecard.");
        } else {
            res.status(201).json(scorecard.ops[0]);
        }
    });
});

/*  "/:id"
 *    GET: find scorecard by id
 *    PUT: update scorecard by id
 *    DELETE: deletes scorecard by id
 */

router.get("/:tournamentId/scorecards/:id", function (req, res) {

    scorecardRepository.findById(req.params.id, function (err, scorecard) {

        if (err) {
            error.handleError(res, err.message, "Failed to get scorecard");
        } else {
            res.status(200).json(scorecard);
        }
    });
});

router.put("/:tournamentId/scorecards/:id", function (req, res) {

    var updateScorecard = req.body;

    scorecardRepository.update(req.params.id, updateScorecard, function (err, scorecard) {
        if (err) {
            error.handleError(res, err.message, "Failed to update scorecard");
        } else {
            res.status(204).json(scorecard);
        }
    });
});

router.delete("/:tournamentId/scorecards/:id", function (req, res) {
    scorecardRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete scorecard");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;