
var participantSchema = {
    "id": "participantSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Participant Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "teeBoxId": {
            "type": ["string", "object"]
        },
        "teeTime": {
            "type": "string",
            "format": "date-time"
        },
        "player": {
            "$ref" : "playerSchema"
        }
    },
    "required": ["teeBoxId", "player"]
}


module.exports = participantSchema;