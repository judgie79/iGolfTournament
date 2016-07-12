var express = require("express");
var router = express.Router();

var clubRepository = require('../db/clubRepository.js');
var courseRepository = require('../db/courseRepository.js');
var playerRepository = require('../db/playerRepository.js');


/*  "/"
 *    GET: finds all clubs
 *    POST: creates a new club
 */
router.get("/", function (req, res) {

    clubRepository.findAll(function (err, clubs) {

        if (err) {
            handleError(res, err.message, "Failed to get clubs.");
        } else {
            res.status(200).json(clubs);
        }
    });
    
});

router.post("/", function (req, res) {

    var newclub = req.body;
    

    if (!req.body.name) {
        handleError(res, "Invalid user input", "Must provide a first or last name.", 400);
    }

    clubRepository.create(newclub, function (err, club) {
        if (err) {
            handleError(res, err.message, "Failed to create new club.");
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
            handleError(res, err.message, "Failed to get club");
        } else {
            res.status(200).json(club);
        }
    });
});

router.put("/:id", function (req, res) {

    var updateClub = req.body;

    clubRepository.update(req.params.id, updateClub, function (err, club) {
        if (err) {
            handleError(res, err.message, "Failed to update club");
        } else {
            res.status(204).json(club);
        }
    });
});

router.delete("/:id", function (req, res) {
    clubRepository.delete(req.params.id, function (err, result) {
        if (err) {
            handleError(res, err.message, "Failed to delete club");
        } else {
            res.status(204).end();
        }
    });
});


router.get("/:id/courses", function (req, res) {

    courseRepository.findCoursesOfClub(req.params.id, function (err, club) {

        if (err) {
            handleError(res, err.message, "Failed to get courses");
        } else {
            res.status(200).json(club);
        }
    });
});

router.get("/:id/players", function (req, res) {

    playerRepository.findMembersOfClub(req.params.id, function (err, club) {

        if (err) {
            handleError(res, err.message, "Failed to get courses");
        } else {
            res.status(200).json(club);
        }
    });
});



// Generic error handler used by all endpoints.
function handleError(res, reason, message, code) {
    console.log("ERROR: " + reason);
    res.status(code || 500).json({ "error": message });
}

module.exports = router;