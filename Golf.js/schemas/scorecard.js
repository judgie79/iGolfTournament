var scorecardSchema = {
    "id": "scorecardSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Scorecard Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
        },
        "participantId": {
            "type": ["string", "null"]
        },
        "playerId": {
            "type": ["string", "null"]
        },
        "tournamentId": {
            "type": ["string", "null"]
        },
        "teeBoxId": {
            "type": ["string", "null"]
        },
        "hcp" : {
            "type": ["number", "integer"]
        },
        "spvg" : {
            "type": ["number", "integer"]
        },
        "scores": {
            "type": ["array", "null"],
            "items" : {
                "type" : "object",
                "properties": {
                    "par" : {
                        "type": "integer",
                    },
                    "strokes" : {
                        "type": "integer",
                    },
                    "netto" : {
                        "type": "integer",
                    },
                    "brutto" : {
                        "type": "integer",
                    },
                }
            }
        }
    }
};

module.exports = scorecardSchema;