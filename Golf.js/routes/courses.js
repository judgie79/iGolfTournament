var express = require("express");
var router = express.Router();

var courseRepository = require('../db/courseRepository.js');

var error = new require('./error')();
/*  "/"
 *    GET: finds all courses
 *    POST: creates a new course
 */
router.get("/", function (req, res) {

    courseRepository.findAll(function (err, courses) {

        if (err) {
            error.handleError(res, err.message, "Failed to get courses.");
        } else {
            res.status(200).json(courses);
        }
    });
    
});

router.post("/", function (req, res) {

    var newcourse = req.body;

    courseRepository.create(newcourse, function (err, course) {
        if (err) {
            error.handleError(res, err.message, "Failed to create new course.");
        } else {
            res.status(201).json(course.ops[0]);
        }
    });
});

/*  "/:id"
 *    GET: find course by id
 *    PUT: update course by id
 *    DELETE: deletes course by id
 */

router.get("/:id", function (req, res) {

    courseRepository.findById(req.params.id, function (err, course) {

        if (err) {
            error.handleError(res, err.message, "Failed to get course");
        } else {
            res.status(200).json(course);
        }
    });
});

router.put("/:id", function (req, res) {

    var updateCourse = req.body;

    courseRepository.update(req.params.id, updateCourse, function (err, course) {
        if (err) {
            error.handleError(res, err.message, "Failed to update course");
        } else {
            res.status(204).json(course);
        }
    });
});

router.delete("/:id", function (req, res) {
    courseRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete course");
        } else {
            res.status(204).end();
        }
    });
});


module.exports = router;