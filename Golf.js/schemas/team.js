

var teamSchema = {
    "id": "teamSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Team Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
        },
        "name": {
            "type": "string"
        },
        "teeTime": {
            "type": "string",
            "format": "date-time"
        },
        "hcp": {
            "type": ["number", "integer"],
            "minimum": 0,
            "maximum": 54
        },
        "members": {
            "type": ["array", "null"],
            "items": {
                "$ref": "participantSchema"
            }
        } 
    },
    "required": ["name", "hcp"]
}


module.exports = playerSchema;