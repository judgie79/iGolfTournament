//- include external modules -//
var config = require("./config.js");
var express = require("express");
var path = require("path");
var bodyParser = require("body-parser");

var mongoUtil = require('./db/mongoUtil');

//- include routing modules -//
var clubRouter = require('./routes/clubs');
var courseRouter = require('./routes/courses');
var teeboxRouter = require('./routes/teeboxes');
var playerRouter = require('./routes/players');
var userRouter = require('./routes/users');
var holeRouter = require('./routes/holes');
var tournamentRouter = require('./routes/tournaments');
var participantRouter = require('./routes/participants');
var teamRouter = require('./routes/teams');
var scorecardRouter = require('./routes/scorecards');


var passport = require('passport');


var app = express();
app.use(bodyParser.json());

// Initialize Passport and restore authentication state, if any, from the
// session.
app.use(passport.initialize());
app.use(passport.session()); // persistent login sessions

// API ROUTES BELOW
app.use('/' + config.rest.apiPrefix + '/clubs', clubRouter);
app.use('/' + config.rest.apiPrefix + '/clubs', holeRouter);
app.use('/' + config.rest.apiPrefix + '/courses', courseRouter);
app.use('/' + config.rest.apiPrefix + '/courses', teeboxRouter);
app.use('/' + config.rest.apiPrefix + '/players', playerRouter);
app.use('/' + config.rest.apiPrefix + '/tournaments', tournamentRouter);
app.use('/' + config.rest.apiPrefix + '/tournaments', participantRouter);
app.use('/' + config.rest.apiPrefix + '/tournaments', teamRouter);
app.use('/' + config.rest.apiPrefix + '/tournaments', scorecardRouter);
app.use('/' + config.rest.apiPrefix + '/users', userRouter);

app.use('/schemas', express.static('schemas'));

mongoUtil.connectToServer(function (err) {
    // start the rest of your app here
    // Initialize the app.
    var server = app.listen(process.env.PORT || config.rest.port, function () {
        var port = server.address().port;
        console.log("App now running on port", port);
    });
});

