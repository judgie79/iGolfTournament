//- include external modules -//
var express = require("express");
var path = require("path");
var bodyParser = require("body-parser");

var mongoUtil = require('./db/mongoUtil');

//- include routing modules -//
var clubRouter = require('./routes/clubs');
var courseRouter = require('./routes/courses');
var playerRouter = require('./routes/players');
var userRouter = require('./routes/users');
var tournamentRouter = require('./routes/tournaments');


var passport = require('passport');


var app = express();
app.use(bodyParser.json());

// Initialize Passport and restore authentication state, if any, from the
// session.
app.use(passport.initialize());
app.use(passport.session()); // persistent login sessions

// API ROUTES BELOW
app.use('/api/clubs', clubRouter);
app.use('/api/courses', courseRouter);
app.use('/api/players', playerRouter);
app.use('/api/tournaments', tournamentRouter);

app.use('/api/users', userRouter)

mongoUtil.connectToServer(function (err) {
    // start the rest of your app here
    // Initialize the app.
    var server = app.listen(process.env.PORT || 8080, function () {
        var port = server.address().port;
        console.log("App now running on port", port);        
    });
});

