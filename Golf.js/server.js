//- include external modules -//
var config = require("./config.js");
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
app.use('/' + config.rest.apiPrefix + '/clubs', clubRouter);
app.use('/' + config.rest.apiPrefix + '/courses', courseRouter);
app.use('/' + config.rest.apiPrefix + '/players', playerRouter);
app.use('/' + config.rest.apiPrefix + '/tournaments', tournamentRouter);
app.use('/' + config.rest.apiPrefix + '/users', userRouter)

app.use('/schemas', express.static('schemas'));

mongoUtil.connectToServer(function (err) {
    // start the rest of your app here
    // Initialize the app.
    var server = app.listen(process.env.PORT || config.rest.port, function () {
        var port = server.address().port;
        console.log("App now running on port", port);



        //test
        var Validator = require('./db/tournamentValidator');
        var val = new Validator({
            "_id": "577e732ab77d5fe65d2b39f3",
            "title": "test",
            "date": "2002-10-02T15:00:00Z",
            "course": {
                "_id" : "577e732ab77d5fe65d2b39f3",
                "clubId" : "577e64a802447c7deb4f3d6d",
                "name" : "B & C",
                "teeboxes" : [ 
                    {
                        "color" : "#FFFF00",
                        "name" : "Herren Gelb",
                        "distance" : 5407.0,
                        "par" : 69.0,
                        "courseRating" : 68.9,
                        "slopeRating" : 125.0,
                        "holes" : [ 
                            {
                                "holeId" : "577e6aecb77d5fe65d2b39db",
                                "number" : 10.0,
                                "distance" : 141.0,
                                "par" : 3.0,
                                "hcp" : 17.0
                            }
                        ]
                    }
                ]
            },
            "club": {
                "_id" : "577e64a802447c7deb4f3d6d",
                "name" : "Golfgarten Deutsche Weinstraße",
                "address" : {
                    "street" : "",
                    "houseNo" : "",
                    "zip" : "",
                    "city" : "Dackenheim",
                    "country" : "DE"
                }
            }
        });
        val.validateSchema().then(function() {
            return val.tournamentNotStarted();
        }).then(function() {
            "test";
        }).catch(function (err) {
            "test";
        });
    });
});

