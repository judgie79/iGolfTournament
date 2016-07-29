var config = {
    db: {
        connectionString: "mongodb://localhost:27017/",
        name: "GolfTournament",
        collections: {
            clubs: "clubs",
            courses: "courses",
            players: "players",
            users: "users",
            tournaments: "tournaments",
            scorecards: "scorecards"
        }
    },
    rest: {
        port: 8080,
        apiPrefix: "api"
    }
};

module.exports = config;