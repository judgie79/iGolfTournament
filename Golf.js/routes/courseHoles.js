var express = require("express");
var router = express.Router();

var holeRepository = require('../db/courseHoleRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all holes
 *    POST: creates a new hole
 */
router.get("/:courseId/teeboxes/:teeboxId/holes", function (req, res) {

    holeRepository.findHolesOfTeebox(req.params.teeboxId, function (err, holes) {

        if (err) {
            error.handleError(res, err.message, "Failed to get holes.");
        } else {
            res.status(200).json(holes);
        }
    });

});

router.post("/:courseId/teeboxes/:teeboxId/holes", function (req, res) {

    var newhole = req.body;

    holeRepository.create(req.params.courseId, req.params.teeboxId, newhole, function (err, course) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new hole.");
        } else {
            res.status(201).json(course);
        }
    });
});

/*  "/:id"
 *    GET: find hole by id
 *    PUT: update hole by id
 *    DELETE: deletes hole by id
 */

router.get("/:courseId/teeboxes/:teeboxId/holes/:id", function (req, res) {

    holeRepository.findById(req.params.courseId, req.params.teeboxId, req.params.id, function (err, hole) {

        if (err) {
            error.handleError(res, err.message, "Failed to get hole");
        } else {
            res.status(200).json(hole);
        }
    });
});

router.put("/:courseId/teeboxes/:teeboxId/holes/:id", function (req, res) {

    var updateHole = req.body;

    holeRepository.update(req.params.courseId, req.params.teeboxId, req.params.id, updateHole, function (err, hole) {
        if (err) {
            error.handleError(res, err.message, "Failed to update hole");
        } else {
            res.status(204).json(hole);
        }
    });
});

router.delete("/:courseId/teeboxes/:teeboxId/holes/:id", function (req, res) {
    holeRepository.delete(req.params.courseId, req.params.teeboxId, req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete hole");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;