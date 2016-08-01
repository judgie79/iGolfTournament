
var tournamentSchema = {
    "id": "tournamentSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Tournament Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "type": {
            "enum": [ "single", "team" ]
        },
        "scoreType": {
            "enum": [ "stableford", "strokeplay" ]
        },
        "title": {
            "type": "string"
        },
        "date": {
            "type": "string",
            "format": "date-time"
        },
        "course": {
            "$ref": "courseSchema"
        },
        "club": {
            "$ref": "clubSchema"
        },
        "participants": {
            "type": ["array", "null"],
            "items": {
                "$ref": "participantSchema"
            }
        },
        "teams": {
            "type": ["array", "null"],
            "items": {
                "$ref": "teamSchema"
            }
        },
    },
    "required": ["title"]
}


module.exports = tournamentSchema;