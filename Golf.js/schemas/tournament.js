
var tournamentSchema = {
    "id": "tournamentSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Tournament Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
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
        }
    },
    "required": ["title"]
}


module.exports = tournamentSchema;