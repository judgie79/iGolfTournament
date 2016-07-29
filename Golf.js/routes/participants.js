var express = require("express");
var router = express.Router();

var participantRepository = require('../db/participantRepository.js');

var error = new require('./error')();

router.post("/:id/participants", function (req, res) {
    
    var participant = req.body;
    

    participantRepository.registerParticipant(req.params.id, participant, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(201).json(result);
        }
    });
});

router.put("/:tournamentId/participants/:id", function (req, res) {
    
    var participant = req.body;
    

    participantRepository.updateParticipant(req.params.tournamentId, req.params.id, participant, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(201).json(result);
        }
    });
});

router.delete("/:id/participants/:participantId", function (req, res) {
    
    participantRepository.deleteParticipant(req.params.id, req.params.participantId, function (err) {
        if (err) {
            error.handleError(res, err.message, "Failed to add participant");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;