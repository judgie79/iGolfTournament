var express = require("express");
var router = express.Router();

var userRepository = require('../db/userRepository.js');
var error = new require('./error')();

//PASSPORT
var passport = require('passport');
var LocalStrategy   = require('passport-local').Strategy;
var BasicStrategy   = require('passport-http').BasicStrategy;
  

var bcrypt = require('bcryptjs');

// Configure the local strategy for use by Passport.
//
// The local strategy require a `verify` function which receives the credentials
// (`username` and `password`) submitted by the user.  The function must verify
// that the password is correct and then invoke `cb` with a user object, which
// will be set at `req.user` in route handlers after authentication.
passport.use(new BasicStrategy(
  function(username, password, callback) {
    userRepository.findByUsername(username, true, function (err, user) {
        if (err) { return callback(err); }

        if (!user) { return callback(null, false); }

        //
        // Load password hash from DB
        bcrypt.compare(password, user.password, function(err, res) {
            // res === true

            if (res === true){
                return callback(null, user);
            } else {
                return callback(null, false);
            }
        });
    });
}));


// Configure Passport authenticated session persistence.
//
// In order to restore authentication state across HTTP requests, Passport needs
// to serialize users into and deserialize users out of the session.  The
// typical implementation of this is as simple as supplying the user ID when
// serializing, and querying the user record by ID from the database when
// deserializing.
passport.serializeUser(function(user, cb) {
   cb(null, user._id);
});

passport.deserializeUser(function(id, cb) {
    
     userRepository.findById(id, true, function(err, user) {
        if (err) { return cb(err); }
        cb(null, user);
    });
});

// router.post('/login', passport.authenticate('basic', { session: false }),
//   function(req, res) {
//     res.status(200).json(req.user);
//   });

// router.get('/logout',
//   function(req, res){
//     req.logout();
//     res.redirect('/');
//   });

//router.get("/", passport.authenticate('basic', { session: false }), function (req, res) {
router.get("/",  passport.authenticate('basic', { session: false }), function (req, res) {
    userRepository.findAll(function (err, users) {

        if (err) {
            error.handleError(res, err.message, "Failed to get users.");
        } else {
            
             var returnUsers = users.map(function(user) {

                 delete user.password;

                 return user;
             });
            
            res.status(200).json(returnUsers);
        }
    });
    
});

router.post("/",  passport.authenticate('basic', { session: false }), function (req, res) {

    var newUser = req.body;

    userRepository.create(newUser, function (err, user) {
        if (err) {
            error.handleError(res, err)
        } else {
            var userReturn = user.ops[0];
            
            delete userReturn.password;
            res.status(201).json(userReturn);
        }
    });
});

/*  "/:id"
 *    GET: find user by id
 *    PUT: update user by id
 *    DELETE: deletes user by id
 */

router.get("/:id",  passport.authenticate('basic', { session: false }), function (req, res) {

    userRepository.findById(req.params.id, false, function (err, user) {

        if (err) {
            error.handleError(res, err.message, "Failed to get user");
        } else {
            res.status(200).json(user);
        }
    });
});

router.put("/:id",  passport.authenticate('basic', { session: false }), function (req, res) {

    var updateUser = req.body;

    userRepository.update(req.params.id, updateUser, function (err, user) {
        if (err) {
            error.handleError(res, err.message, "Failed to update user");
        } else {
            res.status(204).json(user);
        }
    });
});

router.delete("/:id",  passport.authenticate('basic', { session: false }), function (req, res) {
    userRepository.delete(req.params.id, function (err, result) {
        if (err) {
            error.handleError(res, err.message, "Failed to delete user");
        } else {
            res.status(204).end();
        }
    });
});

module.exports = router;