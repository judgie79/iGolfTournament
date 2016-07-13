var config = {
    db: {
        connectionString: "mongodb://localhost:27017/",
        name: "Golf",
        collections: {
            clubs: "clubs",
            courses: "courses",
            players: "players",
            users: "users",
            tournaments: "tournaments",
        }
    },
    rest: {
        port: 8080,
        apiPrefix: "api"
    }
};

module.exports = config;