var express = require("express");
var router = express.Router();

var holeRepository = require('../db/holeRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all holes
 *    POST: creates a new hole
 */
router.get("/:clubId/holes", function (req, res) {

    holeRepository.findHolesOfClub(req.params.clubId, function (err, holes) {

        if (err) {
            error.handleError(res, err.message, "Failed to get holes.");
        } else {
            res.status(200).json(holes);
        }
    });

});

router.post("/:clubId/holes", function (req, res) {

    var newhole = req.body;

    holeRepository.create(req.params.clubId, newhole, function (err, holeId) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new hole.");
        } else {
            res.status(201).json(holeId);
        }
    });
});

/*  "/:id"
 *    GET: find hole by id
 *    PUT: update hole by id
 *    DELETE: deletes hole by id
 */

router.get("/:clubId/holes/:id", function (req, res) {

    holeRepository.findById(req.params.clubId, req.params.id, function (err, hole) {

        if (err) {
            error.handleError(res, err.message, "Failed to get hole");
        } else {
            res.status(200).json(hole);
        }
    });
});

router.put("/:clubId/holes/:id", function (req, res) {

    var updateHole = req.body;

    holeRepository.update(req.params.clubId, req.params.id, updateHole, function (err, hole) {
        if (err) {
            error.handleError(res, err.message, "Failed to update hole");
        } else {
            res.status(204).json(hole);
        }
    });
});

router.delete("/:clubId/holes/:id", function (req, res) {
    holeRepository.delete(req.params.clubId, req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete hole");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;