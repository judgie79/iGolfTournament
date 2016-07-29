var express = require("express");
var router = express.Router();

var teeboxRepository = require('../db/teeboxRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all teeboxs
 *    POST: creates a new teebox
 */
router.get("/:courseId/teeboxes", function (req, res) {

    teeboxRepository.findAll(req.params.courseId, function (err, teeboxes) {

        if (err) {
            error.handleError(res, err.message, "Failed to get teeboxes.");
        } else {
            res.status(200).json(teeboxes);
        }
    });
    
});

router.post("/:courseId/teeboxes", function (req, res) {

    var newteebox = req.body;

    teeboxRepository.create(req.params.courseId, newteebox, function (err, course) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new teebox.");
        } else {
            res.status(201).json(course);
        }
    });
});

/*  "/:id"
 *    GET: find teebox by id
 *    PUT: update teebox by id
 *    DELETE: deletes teebox by id
 */

router.get("/:courseId/teeboxes/:id", function (req, res) {

    teeboxRepository.findById(req.params.courseId, req.params.id, function (err, teebox) {

        if (err) {
            error.handleError(res, err.message, "Failed to get teebox");
        } else {
            res.status(200).json(teebox);
        }
    });
});

router.put("/:courseId/teeboxes/:id", function (req, res) {

    var updateTeebox = req.body;

    teeboxRepository.update(req.params.courseId, req.params.id, updateTeebox, function (err, teebox) {
        if (err) {
            error.handleError(res, err.message, "Failed to update teebox");
        } else {
            res.status(204).json(teebox);
        }
    });
});

router.delete("/:courseId/teeboxes/:id", function (req, res) {
    teeboxRepository.delete(req.params.courseId, req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete teebox");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;