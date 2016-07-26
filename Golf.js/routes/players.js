var express = require("express");
var router = express.Router();

var playerRepository = require('../db/playerRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all players
 *    POST: creates a new player
 */
router.get("/", function (req, res) {

    playerRepository.findAll(function (err, players) {

        if (err) {
            error.handleError(res, err.message, "Failed to get players.");
        } else {
            res.status(200).json(players);
        }
    });
    
});

router.post("/", function (req, res) {

    var newplayer = req.body;
    

    if (!(req.body.firstname || req.body.lastname)) {
        error.handleError(res, "Invalid user input", "Must provide a first or last name.", 400);
    }

    playerRepository.create(newplayer, function (err, player) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new player.");
        } else {
            res.status(201).json(player.ops[0]);
        }
    });
});

/*  "/:id"
 *    GET: find player by id
 *    PUT: update player by id
 *    DELETE: deletes player by id
 */

router.get("/:id", function (req, res) {

    playerRepository.findById(req.params.id, function (err, player) {

        if (err) {
            error.handleError(res, err.message, "Failed to get player");
        } else {
            res.status(200).json(player);
        }
    });
});

router.put("/:id", function (req, res) {

    var updatePlayer = req.body;

    playerRepository.update(req.params.id, updatePlayer, function (err, player) {
        if (err) {
            error.handleError(res, err.message, "Failed to update player");
        } else {
            res.status(204).json(player);
        }
    });
});

router.delete("/:id", function (req, res) {
    playerRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete player");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;