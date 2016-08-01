var express = require("express");
var router = express.Router();

var teamRepository = require('../db/teamRepository.js');

var error = new require('./error')();

router.post("/:tournamentId/teams", function (req, res) {
    
    var team = req.body;
    

    teamRepository.registerTeam(req.params.tournamentId, team, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add team");
        } else {
            res.status(201).json(result);
        }
    });
});

router.put("/:tournamentId/teams/:id", function (req, res) {
    
    var team = req.body;
    

    teamRepository.updateTeam(req.params.tournamentId, req.params.id, team, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add team");
        } else {
            res.status(201).json(result);
        }
    });
});

router.delete("/:tournamentId/teams/:teamId", function (req, res) {
    
    teamRepository.deleteTeam(req.params.tournamentId, req.params.teamId, function (err) {
        if (err) {
            error.handleError(res, err.message, "Failed to add team");
        } else {
            res.status(204).end();
        }
    });
});

//members
router.post("/:tournamentId/teams/:teamId/members", function (req, res) {
    
    var member = req.body;
    

    teamRepository.registerTeamMember(req.params.tournamentId, req.params.teamId, member, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to add team member");
        } else {
            res.status(201).json(result);
        }
    });
});

router.put("/:tournamentId/teams/:teamId/members/:id", function (req, res) {
    
    var member = req.body;
    

    teamRepository.updateTeamMember(req.params.tournamentId, req.params.teamId, req.params.id, member, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to update team member");
        } else {
            res.status(201).json(result);
        }
    });
});

router.delete("/:tournamentId/teams/:teamId/members/:id", function (req, res) {
    
    teamRepository.deleteTeamMember(req.params.tournamentId, req.params.teamId, req.params.id, function (err) {
        if (err) {
            error.handleError(res, err.message, "Failed to add team");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;