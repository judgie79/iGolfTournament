var express = require("express");
var router = express.Router();

var tournamentRepository = require('../db/tournamentRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all tournaments
 *    POST: creates a new tournament
 */
router.get("/", function (req, res) {

    tournamentRepository.findAll(function (err, tournaments) {

        if (err) {
            error.handleError(res, err.message, "Failed to get tournaments.");
        } else {
            res.status(200).json(tournaments);
        }
    });
    
});

router.post("/", function (req, res) {

    var newtournament = req.body;
    
    tournamentRepository.create(newtournament, function (err, tournament) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new tournament.");
        } else {
            res.status(201).json(tournament.ops[0]);
        }
    });
});

/*  "/:id"
 *    GET: find tournament by id
 *    PUT: update tournament by id
 *    DELETE: deletes tournament by id
 */

router.get("/:id", function (req, res) {

    tournamentRepository.findById(req.params.id, function (err, tournament) {

        if (err) {
            error.handleError(res, err.message, "Failed to get tournament");
        } else {
            res.status(200).json(tournament);
        }
    });
});

router.put("/:id", function (req, res) {

    var updateTournament = req.body;

    tournamentRepository.update(req.params.id, updateTournament, function (err, tournament) {
        if (err) {
            error.handleError(res, err.message, "Failed to update tournament");
        } else {
            res.status(204).json(tournament);
        }
    });
});

router.put("/:id/start", function (req, res) {

    var updateTournament = req.body;

    tournamentRepository.start(req.params.id, updateTournament, function (err, tournament) {
        if (err) {
            error.handleError(res, err.message, "Failed to update tournament");
        } else {
            res.status(204).json(tournament);
        }
    });
});

router.delete("/:id", function (req, res) {
    tournamentRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete tournament");
        } else {
            res.status(204).end();
        }
    });
});


router.post("/:id/participants", function (req, res) {
    
    var participant = req.body;
    

    tournamentRepository.registerParticipant(req.params.id, participant, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(201).json(result);
        }
    });
});

router.put("/:tournamentId/participants/:id", function (req, res) {
    
    var participant = req.body;
    

    tournamentRepository.updateParticipant(req.params.tournamentId, req.params.id, participant, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(201).json(result);
        }
    });
});

router.delete("/:id/participants/:participantId", function (req, res) {
    
    tournamentRepository.deleteParticipant(req.params.id, req.params.participantId, function (err) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;